using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Second;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Targeting;

namespace Server.Spells
{
    public abstract class Spell : ISpell
    {
        private readonly Mobile m_Caster;
        private readonly Item m_Scroll;
        private readonly SpellInfo m_Info;
        private SpellState m_State;
        private DateTime m_StartCastTime;

        //Maka
        private object m_SphereSpellTarget;
        private readonly PlayerMobile m_PlayerCaster;

        public SpellState State { get { return m_State; } set { m_State = value; } }
        public Mobile Caster { get { return m_Caster; } }
        public SpellInfo Info { get { return m_Info; } }
        public string Name { get { return m_Info.Name; } }
        public string Mantra { get { return m_Info.Mantra; } }
        public Type[] Reagents { get { return m_Info.Reagents; } }
        public Item Scroll { get { return m_Scroll; } }
        public DateTime StartCastTime { get { return m_StartCastTime; } }

        private static readonly TimeSpan NextSpellDelay = TimeSpan.FromSeconds(0.75);
        private static TimeSpan AnimateDelay = TimeSpan.FromSeconds(1.5);

        public virtual SkillName CastSkill { get { return SkillName.Magery; } }
        public virtual SkillName DamageSkill { get { return SkillName.EvalInt; } }

        public virtual bool CheckLOS { get { return true; } } // Jonny

        public virtual bool RevealOnCast { get { return true; } }
        public virtual bool ClearHandsOnCast { get { return true; } }
        public virtual bool ShowHandMovement { get { return true; } }

        public virtual bool DelayedDamage { get { return false; } }

        public virtual bool DelayedDamageStacking { get { return true; } }
        //In reality, it's ANY delayed Damage spell Post-AoS that can't stack, but, only 
        //Expo & Magic Arrow have enough delay and a short enough cast time to bring up 
        //the possibility of stacking 'em.  Note that a MA & an Explosion will stack, but
        //of course, two MA's won't.

        //Maka
        public virtual bool SpellDisabled { get { return false; } }
        public virtual bool CanTargetGround { get { return false; } }
        public virtual bool SpellFizzlesOnHurt { get { return false; } }
        public virtual bool HasNoTarget { get { return false; } }
        public object SphereSpellTarget { get { return m_SphereSpellTarget; } }
        public PlayerMobile PlayerCaster { get { return m_PlayerCaster; } }

        public virtual int Sound { get; set; }

        //private static Dictionary<Type, Dictionary<Mobile, Timer>> m_ContextTable = new Dictionary<Type, Dictionary<Mobile, Timer>>();
        private static readonly Dictionary<Type, DelayedDamageContextWrapper> m_ContextTable = new Dictionary<Type, DelayedDamageContextWrapper>();

        private class DelayedDamageContextWrapper
        {
            private readonly Dictionary<Mobile, Timer> m_Contexts = new Dictionary<Mobile, Timer>();

            public void Add(Mobile m, Timer t)
            {
                Timer oldTimer;
                if (m_Contexts.TryGetValue(m, out oldTimer))
                {
                    oldTimer.Stop();
                    m_Contexts.Remove(m);
                }

                m_Contexts.Add(m, t);
            }

            public void Remove(Mobile m)
            {
                m_Contexts.Remove(m);
            }
        }

        public void StartDelayedDamageContext(Mobile m, Timer t)
        {
            if (DelayedDamageStacking)
                return; //Sanity

            DelayedDamageContextWrapper contexts;

            if (!m_ContextTable.TryGetValue(GetType(), out contexts))
            {
                contexts = new DelayedDamageContextWrapper();
                m_ContextTable.Add(GetType(), contexts);
            }

            contexts.Add(m, t);
        }

        public void RemoveDelayedDamageContext(Mobile m)
        {
            DelayedDamageContextWrapper contexts;

            if (!m_ContextTable.TryGetValue(GetType(), out contexts))
                return;

            contexts.Remove(m);
        }

        public void HarmfulSpell(Mobile m)
        {
            if (m is BaseCreature)
                ((BaseCreature)m).OnHarmfulSpell(m_Caster);
        }

        public Spell(Mobile caster, Item scroll, SpellInfo info)
        {
            m_Caster = caster;
            m_Scroll = scroll;
            m_Info = info;

            //Assign this here so that we wont have to cast it every time
            if (m_Caster.Player && m_Caster is PlayerMobile)
                m_PlayerCaster = (PlayerMobile)m_Caster;
        }

        public virtual int GetNewAosDamage(int bonus, int dice, int sides, Mobile singleTarget)
        {
            if (singleTarget != null)
                return GetNewAosDamage(bonus, dice, sides, (Caster.Player && singleTarget.Player),
                                       GetDamageScalar(singleTarget));
            return GetNewAosDamage(bonus, dice, sides, false);
        }

        public virtual int GetNewAosDamage(int bonus, int dice, int sides, bool playerVsPlayer)
        {
            return GetNewAosDamage(bonus, dice, sides, playerVsPlayer, 1.0);
        }

        public virtual int GetNewAosDamage(int bonus, int dice, int sides, bool playerVsPlayer, double scalar)
        {
            var damage = Utility.Dice(dice, sides, bonus) * 100;
            var damageBonus = 0;

            var inscribeSkill = GetInscribeFixed(m_Caster);
            var inscribeBonus = (inscribeSkill + (1000 * (inscribeSkill / 1000))) / 200;
            damageBonus += inscribeBonus;

            var intBonus = Caster.Int / 10;
            damageBonus += intBonus;

            var sdiBonus = AosAttributes.GetValue(m_Caster, AosAttribute.SpellDamage);
            // PvP spell damage increase cap of 15% from an item’s magic property
            if (playerVsPlayer && sdiBonus > 15)
                sdiBonus = 15;
            damageBonus += sdiBonus;

            var context = TransformationSpellHelper.GetContext(Caster);

            if (context != null && context.Spell is ReaperFormSpell)
                damageBonus += ((ReaperFormSpell)context.Spell).SpellDamageBonus;

            damage = AOS.Scale(damage, 100 + damageBonus);

            var evalSkill = GetDamageFixed(m_Caster);
            var evalScale = 30 + ((9 * evalSkill) / 100);

            damage = AOS.Scale(damage, evalScale);

            damage = AOS.Scale(damage, (int)(scalar * 100));

            return damage / 100;
        }

        public virtual int GetSphereDamage(Mobile caster, Mobile target, int baseDamage)
        {
            var evalIntBonus = caster.Skills[DamageSkill].Value / 100;
            var resist = target.Skills[SkillName.MagicResist].Value;

            if (evalIntBonus < 0.5)
                evalIntBonus = 0.5;

            resist /= 10;

            var damage = baseDamage - resist;
            damage *= evalIntBonus;

            return (int)damage;
        }

        public virtual bool IsCasting { get { return m_State == SpellState.Casting; } }

        public virtual void OnCasterHurt()
        {
            //Confirm: Monsters and pets cannot be disturbed.
            if (!(Caster is PlayerMobile))
                return;

            //Maka
            if (IsCasting)
            {
                if (SpellFizzlesOnHurt)
                    DoFizzle();
                else if (Caster.Region is CustomRegion && ((CustomRegion)Caster.Region).Controller.FizzlePvP)
                    DoFizzle();
            }
        }

        public virtual void OnCasterKilled()
        {
            Disturb(DisturbType.Kill);
        }

        public virtual void OnConnectionChanged()
        {
            FinishSequence();
        }

        public virtual bool OnCasterMoving(Direction d)
        {
            return true;
        }

        public virtual bool OnCasterEquiping(Item item)
        {
            return true;
        }

        public virtual bool OnCasterUsingObject(object o)
        {
            return true;
        }

        public virtual bool OnCastInTown(Region r)
        {
            if (r is GuardedRegion)
                return m_Info.AllowTown;
            return true;
        }

        public virtual bool ConsumeReagents()
        {
            if (m_PlayerCaster == null || m_Scroll != null)
                return true;

            if (AosAttributes.GetValue(m_Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                return true;

            var pack = m_Caster.Backpack;

            if (pack == null)
                return false;

            if (pack.ConsumeTotal(m_Info.Reagents, m_Info.Amounts) == -1)
                return true;

            return false;
        }

        public virtual double GetInscribeSkill(Mobile m)
        {
            // There is no chance to gain
            // m.CheckSkill( SkillName.Inscribe, 0.0, 120.0 );

            return m.Skills[SkillName.Inscribe].Value;
        }

        public virtual int GetInscribeFixed(Mobile m)
        {
            // There is no chance to gain
            // m.CheckSkill( SkillName.Inscribe, 0.0, 120.0 );

            return m.Skills[SkillName.Inscribe].Fixed;
        }

        public virtual int GetDamageFixed(Mobile m)
        {
            //m.CheckSkill(DamageSkill, 0.0, 120.0);

            return m.Skills[DamageSkill].Fixed;
        }

        public virtual double GetDamageSkill(Mobile m)
        {
            //m.CheckSkill(DamageSkill, 0.0, 120.0);

            return m.Skills[DamageSkill].Value;
        }

        /*
		public virtual int GetResistFixed( Mobile m )
		{
			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if ( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, 120.0 );

			return m.Skills[SkillName.MagicResist].Fixed;
		}
         * */

        public virtual double GetResistSkill(Mobile m)
        {
            return m.Skills[SkillName.MagicResist].Value;
        }

        public virtual double GetDamageScalar(Mobile target)
        {
            var scalar = 1.0;

            if (!Core.AOS)	//EvalInt stuff for AoS is handled elsewhere
            {
                var casterEI = m_Caster.Skills[DamageSkill].Value;
                var targetRS = target.Skills[SkillName.MagicResist].Value;

                /*
                if( Core.AOS )
                    targetRS = 0;
                */

               // m_Caster.CheckSkill(DamageSkill, 0.0, 120.0);

                if (casterEI > targetRS)
                    scalar = (1.0 + ((casterEI - targetRS) / 500.0));
                else
                    scalar = (1.0 + ((casterEI - targetRS) / 200.0));

                // magery damage bonus, -25% at 0 skill, +0% at 100 skill, +5% at 120 skill
                scalar += (m_Caster.Skills[CastSkill].Value - 100.0) / 400.0;

                if (!target.Player && !target.Body.IsHuman /*&& !Core.AOS*/ )
                    scalar *= 2.0; // Double magery damage to monsters/animals if not AOS
            }

            if (target is BaseCreature)
                ((BaseCreature)target).AlterDamageScalarFrom(m_Caster, ref scalar);

            if (m_Caster is BaseCreature)
                ((BaseCreature)m_Caster).AlterDamageScalarTo(target, ref scalar);

            if (Core.SE)
                scalar *= GetSlayerDamageScalar(target);

            target.Region.SpellDamageScalar(m_Caster, target, ref scalar);

            return scalar;
        }

        public virtual double GetSlayerDamageScalar(Mobile defender)
        {
            var atkBook = Spellbook.FindEquippedSpellbook(m_Caster);

            var scalar = 1.0;
            if (atkBook != null)
            {
                var atkSlayer = SlayerGroup.GetEntryByName(atkBook.Slayer);
                var atkSlayer2 = SlayerGroup.GetEntryByName(atkBook.Slayer2);

                if (atkSlayer != null && atkSlayer.Slays(defender) || atkSlayer2 != null && atkSlayer2.Slays(defender))
                {
                    defender.FixedEffect(0x37B9, 10, 5);	//TODO: Confirm this displays on OSIs
                    scalar = 2.0;
                }


                var context = TransformationSpellHelper.GetContext(defender);

                if ((atkBook.Slayer == SlayerName.Silver || atkBook.Slayer2 == SlayerName.Silver) && context != null && context.Type != typeof(HorrificBeastSpell))
                    scalar += .25; // Every necromancer transformation other than horrific beast take an additional 25% damage

                if (scalar != 1.0)
                    return scalar;
            }

            ISlayer defISlayer = Spellbook.FindEquippedSpellbook(defender);

            if (defISlayer == null)
                defISlayer = defender.Weapon as ISlayer;

            if (defISlayer != null)
            {
                var defSlayer = SlayerGroup.GetEntryByName(defISlayer.Slayer);
                var defSlayer2 = SlayerGroup.GetEntryByName(defISlayer.Slayer2);

                if (defSlayer != null && defSlayer.Group.OppositionSuperSlays(m_Caster) || defSlayer2 != null && defSlayer2.Group.OppositionSuperSlays(m_Caster))
                    scalar = 2.0;
            }

            return scalar;
        }

        public virtual void DoFizzle()
        {
            if (m_PlayerCaster != null) //Only consume mana on fizzle for players
            {
                CustomRegion cR = m_PlayerCaster.Region as CustomRegion;

                if (cR == null || !cR.Controller.FizzlePvP)
                {
                    int mana = ScaleMana(GetMana());
                    m_Caster.Mana -= (int) Math.Round((double) mana/2, 0);
                }
            }

            m_Caster.SendAsciiMessage("The spell fizzles.");

            m_Caster.FixedEffect(0x3735, 6, 30);
            m_Caster.PlaySound(0x5C);

            if (m_CastTimer != null)
                m_CastTimer.Stop();

            if (m_AnimTimer != null)
                m_AnimTimer.Stop();

            m_State = SpellState.None;
            m_Caster.Spell = null;
        }

        private CastTimer m_CastTimer;
        private AnimTimer m_AnimTimer;

        public void Disturb(DisturbType type)
        {
            DoFizzle();
        }

        public virtual bool CheckDisturb(DisturbType type, bool firstCircle, bool resistable)
        {
            if (resistable && m_Scroll is BaseWand)
                return false;

            return true;
        }

        public void Disturb(DisturbType type, bool firstCircle, bool resistable)
        {
            if (!CheckDisturb(type, firstCircle, resistable))
                return;

            if (m_State == SpellState.Casting)
            {
                if (!firstCircle && !Core.AOS && this is MagerySpell && ((MagerySpell)this).Circle == SpellCircle.First)
                    return;

                m_State = SpellState.None;
                m_Caster.Spell = null;

                OnDisturb(type, true);

                if (m_CastTimer != null)
                    m_CastTimer.Stop();

                if (m_AnimTimer != null)
                    m_AnimTimer.Stop();

                if (Core.AOS && m_Caster.Player && type == DisturbType.Hurt)
                    DoHurtFizzle();

                m_Caster.NextSpellTime = DateTime.Now;// +GetDisturbRecovery();
            }
            else if (m_State == SpellState.Sequencing)
            {
                if (!firstCircle && !Core.AOS && this is MagerySpell && ((MagerySpell)this).Circle == SpellCircle.First)
                    return;

                m_State = SpellState.None;
                m_Caster.Spell = null;

                OnDisturb(type, false);

                Target.Cancel(m_Caster);

                if (Core.AOS && m_Caster.Player && type == DisturbType.Hurt)
                    DoHurtFizzle();
            }
        }

        public virtual void DoHurtFizzle()
        {
            m_Caster.FixedEffect(0x3735, 6, 30);
            m_Caster.PlaySound(0x5C);
        }

        public virtual void OnDisturb(DisturbType type, bool message)
        {
            //if ( message )
            //    m_Caster.SendLocalizedMessage( 500641 ); // Your concentration is disturbed, thus ruining thy spell.
            m_Caster.FixedEffect(0x3735, 6, 30);
            m_Caster.PlaySound(0x5C);
        }

        public virtual bool CheckCast()
        {
            return true;
        }

        public virtual void SayMantra()
        {
            if (m_Scroll is BaseWand)
                return;

            if  (!(m_Caster is PlayerMobile) && (m_Caster.Body != 0x190 && m_Caster.Body != 0x191)) //Taran - Only human mobs have powerwords
                return;

            //Maka
            if (!string.IsNullOrEmpty(m_Info.Mantra))
            {
                if (m_PlayerCaster != null && m_PlayerCaster.HiddenWithSpell)
                {
                    if (Caster.Map != null)
                    {
                        var eable = Caster.GetClientsInRange(12);

                        foreach (NetState state in eable)
                            if (state != null && state.Mobile.InLOS(Caster))
                            {
                                if (state.Mobile == Caster)
                                    Caster.PrivateOverheadMessage(MessageType.Spell, 906, true, Info.Mantra, Caster.NetState);
                                else if (state.Mobile.AccessLevel > AccessLevel.Player && state.Mobile.AccessLevel >= Caster.AccessLevel)
                                    Caster.PrivateOverheadMessage(MessageType.Spell, 906, true, Info.Mantra, state);
                                else
                                    state.Mobile.SendAsciiMessage(Info.Mantra);
                            }

                        eable.Free();
                    }
                }
                else
                    m_Caster.PublicOverheadMessage(MessageType.Spell, 906, true, m_Info.Mantra, true);
            }
        }

        public virtual bool BlockedByHorrificBeast { get { return true; } }
        public virtual bool BlockedByAnimalForm { get { return true; } }
        public virtual bool BlocksMovement { get { return true; } }

        public virtual bool CheckNextSpellTime { get { return !(m_Scroll is BaseWand); } }

        //Maka
        public bool IsCastPossible()
        {
            //Nasir - GM's can cast anything
            if (m_Caster.AccessLevel >= AccessLevel.GameMaster)
                return true;

            if (m_PlayerCaster != null && m_PlayerCaster.Stoned)
                return false;

            if (SpellDisabled)
            {
                m_Caster.SendAsciiMessage("This spell has been disabled.");
                return false;
            }

            if (!m_Caster.CheckAlive())
            {
                m_Caster.SendAsciiMessage("This is beyond your ability");
                return false;
            }
            if (m_Scroll is BaseWand)
            {
                return true;
            }
            if (m_Scroll == null && !HasReagents())
            {
                DisplayMissingReagents();
                return false;
            }
            if (m_Caster.Mana < ScaleMana(GetMana()))
            {
                m_Caster.SendAsciiMessage("You lack sufficient mana for this spell.");
                return false;
            }
            return true;
        }

        //Maka
        public bool HasReagents()
        {
            if (m_Caster.Backpack != null && 
                m_Info.Reagents.Length <= m_Caster.Backpack.FindItemsByType(m_Info.Reagents, true).Length)
            {
                return true;
            }
            
            return false;
        }

        //Maka
        public void DisplayMissingReagents()
        {
            var reagents = string.Empty;
            var missingReagent = new List<string>();

            for (var i = 0; i < m_Info.Reagents.Length; i++)
            {
                var toFind = m_Info.Reagents[i];

                if (m_Caster.Backpack.FindItemByType(toFind) == null)
                {
                    if (SpellInfo.ReagentShortStringList.ContainsKey(toFind))
                        missingReagent.Add(SpellInfo.ReagentShortStringList[toFind]);
                    else
                        missingReagent.Add("Error");
                }
            }

            for (var i = 0; i < missingReagent.Count; i++)
            {
                if (i == 0)
                    reagents = string.Format("({0}", missingReagent[i]);//first

                if (i == missingReagent.Count - 1)
                {
                    if (i != 0)
                        reagents = string.Format("{0}, {1})", reagents, missingReagent[i]);//last if more than one
                    else
                        reagents = string.Format("{0})", reagents);//only one
                }
                    
                else
                {
                    if (i != 0)
                        reagents = string.Format("{0}, {1}", reagents, missingReagent[i]);//anything else if not first
                }
                    
            }

            Caster.SendAsciiMessage("You lack reagents for this spell.");
            Caster.SendAsciiMessage(reagents);
        }

        //Maka
        public bool RequestTargetBeforCasting()
        {
            if (IsCastPossible())
            {
                if (m_Caster.Paralyzed && !m_Caster.HasFreeHand())
                {
                    return false;
                }

                if (m_Caster.Target != null)
                {
                    m_Caster.SendAsciiMessage("Targeting cancelled.");
                    Caster.Target.Cancel(Caster, TargetCancelType.Canceled);
                }

                if (HasNoTarget) // If the spell doesn't require a target, just call the callback
                    SphereCastCallback(m_Caster, m_Caster);
                else
                    m_Caster.BeginTarget(15, CanTargetGround, TargetFlags.None, SphereCastCallback).CheckLOS = CheckLOS;
                
                return true;
            }

            return false;
        }

        public bool ValidTarget(Mobile from, object target)
        {
            var canCast = true;
            //Nested the "if"s like this so that we do not have to make a second object check
            if (target is RecallRune)
            {
                if (!(this is RecallSpell || this is MarkSpell || this is Seventh.GateTravelSpell))
                    canCast = false;
            }
            else if (target is Runebook)
            {
                if (!(this is RecallSpell || this is Seventh.GateTravelSpell))
                    canCast = false;
            }
            else if (target is Key)
            {
                if (!(this is RecallSpell))
                    canCast = false;
            }
            else if (this is RecallSpell || this is MarkSpell || this is Seventh.GateTravelSpell)
                canCast = false;
            else if (target is PlayerMobile && ((PlayerMobile)target).AccessLevel > Caster.AccessLevel)
            {
                canCast = false;
            }

            if (!canCast)
            {
                Caster.SendAsciiMessage("You can't target that!");
                return false;
            }

            #region Check if targets are inside or outside house and block cast if necessary
            /*Taran: Why was this added? I remember adding it but not sure why...
            if (target is Mobile && !(target is PlayerMobile))
            {
                var m_Target = (Mobile)target;

                var house = BaseHouse.FindHouseAt(m_Caster);
                var house2 = BaseHouse.FindHouseAt(m_Target);

                if (house != null && house != house2)
                {
                    m_Caster.SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }
            
            else if (target is LandTarget)
            {
                var m_Target = (LandTarget)target;

                var house = BaseHouse.FindHouseAt(m_Caster);
                var house2 = BaseHouse.FindHouseAt(m_Target.Location, m_Caster.Map, m_Target.Location.Z);

                if (house != house2)
                {
                    m_Caster.SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }

            else if (target is StaticTarget)
            {
                var m_Target = (StaticTarget)target;

                var house = BaseHouse.FindHouseAt(m_Caster);
                var house2 = BaseHouse.FindHouseAt(m_Target.Location, m_Caster.Map, m_Target.Location.Z);

                if (house != house2)
                {
                    m_Caster.SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }

            else if (target is Item)
            {
                var m_Target = (Item)target;

                var house = BaseHouse.FindHouseAt(m_Caster);
                var house2 = BaseHouse.FindHouseAt(m_Target);

                if (house != house2)
                {
                    m_Caster.SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }
            */
            #endregion

            return true;
        }

        //Maka
        public void SphereCastCallback(Mobile caster, object target)
        {
            //Fixes some bugs with misstargeting.
            if (!ValidTarget(caster, target))
            {
                if (m_Caster.Spell != null)
                    ((Spell)m_Caster.Spell).DoFizzle();

                return;
            }

            if (caster != target)
                SpellHelper.Turn(Caster, target);

            if (m_Caster.Spell != null && m_Caster.Spell.IsCasting)
                ((Spell)m_Caster.Spell).DoFizzle();

            SphereCast(caster, target);
        }

        public bool SphereCast(Mobile from, object obj)
        {
            m_StartCastTime = DateTime.Now;

            if (m_PlayerCaster.Paralyzed && !m_PlayerCaster.HasFreeHand())
            {
                m_Caster.SendAsciiMessage(33, "You must have your hands free to cast while paralyzed!");
                return false;
            }

            if (IsCastPossible())
            {
                m_SphereSpellTarget = obj;

                if (m_Caster.CheckSpellCast(this) && CheckCast() && m_Caster.Region.OnBeginSpellCast(m_Caster, this))
                {
                    if (m_Caster is PlayerMobile && ClearHandsOnCast) //Mobiles don't need to disarm
                        m_Caster.ClearHands();

                    m_State = SpellState.Casting;

                    m_Caster.Spell = this;

                    m_PlayerCaster.WeaponTimerCheck();
                    m_PlayerCaster.BandageCheck();

                    m_PlayerCaster.AbortCurrentPlayerAction();

                    //Maka
                    if (RevealOnCast && !m_PlayerCaster.HiddenWithSpell)
                        m_Caster.RevealingAction();

                    SayMantra();

                    var castDelay = GetCastDelay();

                    var count = (int)Math.Ceiling(castDelay.TotalSeconds / AnimateDelay.TotalSeconds);

                    if (count != 0)
                    {
                        m_AnimTimer = new AnimTimer(this, 1);
                        m_AnimTimer.Start();
                    }

                    if (m_Info.LeftHandEffect > 0)
                        Caster.FixedParticles(0, 10, 5, m_Info.LeftHandEffect, EffectLayer.LeftHand);

                    if (m_Info.RightHandEffect > 0)
                        Caster.FixedParticles(0, 10, 5, m_Info.RightHandEffect, EffectLayer.RightHand);

                    if (Core.ML)
                        WeaponAbility.ClearCurrentAbility(m_Caster);

                    m_CastTimer = new CastTimer(this, castDelay);
                    m_CastTimer.Start();

                    OnBeginCast();
                    return true;
                }
                return false;
            }
            return false;
        }

        public virtual bool Cast()
        {
            if (m_PlayerCaster != null) //Player cast
                return RequestTargetBeforCasting();
            return DirectCast(); //Mobile cast
        }

        public bool DirectCast() //Mobile cast
        {
            m_StartCastTime = DateTime.Now;

            if (Core.AOS && m_Caster.Spell is Spell && ((Spell)m_Caster.Spell).State == SpellState.Sequencing)
                ((Spell)m_Caster.Spell).Disturb(DisturbType.NewCast);

            if (!m_Caster.CheckAlive())
            {
                return false;
            }
            if (m_Caster.Spell != null && m_Caster.Spell.IsCasting)
            {
                m_Caster.SendLocalizedMessage(502642); // You are already casting a spell.
            }
            else if (BlockedByHorrificBeast && TransformationSpellHelper.UnderTransformation(m_Caster, typeof(HorrificBeastSpell)) || (BlockedByAnimalForm && AnimalForm.UnderTransformation(m_Caster)))
            {
                m_Caster.SendLocalizedMessage(1061091); // You cannot cast that spell in this form.
            }
            else if (m_Caster.Frozen)
            {
                m_Caster.SendLocalizedMessage(502643); // You can not cast a spell while frozen.
            }
            else if (CheckNextSpellTime && DateTime.Now < m_Caster.NextSpellTime)
            {
                m_Caster.SendLocalizedMessage(502644); // You have not yet recovered from casting a spell.
            }
            else if (m_Caster is PlayerMobile && ((PlayerMobile)m_Caster).PeacedUntil > DateTime.Now)
            {
                m_Caster.SendLocalizedMessage(1072060); // You cannot cast a spell while calmed.
            }
            //Taran: Tacky fix for checking range and LoS for NPC's, I don't know where the NPC spell targeting method is so I'm using Combatant instead.
            else if (m_Caster is BaseCreature && m_Caster.Combatant != null && (!m_Caster.InLOS(m_Caster.Combatant) || !m_Caster.InRange(m_Caster.Combatant, 12) || !m_Caster.CanSee(m_Caster.Combatant)))
            {
                m_Caster.SendAsciiMessage("You cannot see your target");
            }
            else if (m_Caster.Mana >= ScaleMana(GetMana()))
            {
                if (m_Caster.Spell == null && m_Caster.CheckSpellCast(this) && CheckCast() && m_Caster.Region.OnBeginSpellCast(m_Caster, this))
                {
                    m_State = SpellState.Casting;
                    m_Caster.Spell = this;

                    if (RevealOnCast)
                    {
                        if (m_Caster is PlayerMobile)
                        {
                            if (!((PlayerMobile)m_Caster).HiddenWithSpell)
                                m_Caster.RevealingAction();
                        }
                        else
                            m_Caster.RevealingAction();
                    }

                    SayMantra();

                    var castDelay = GetCastDelay();

                    if (ShowHandMovement && m_Caster.Body.IsHuman)
                    {
                        var count = (int)Math.Ceiling(castDelay.TotalSeconds / AnimateDelay.TotalSeconds);

                        if (count != 0)
                        {
                            m_AnimTimer = new AnimTimer(this, 1);
                            m_AnimTimer.Start();
                        }

                        if (m_Info.LeftHandEffect > 0)
                            Caster.FixedParticles(0, 10, 5, m_Info.LeftHandEffect, EffectLayer.LeftHand);

                        if (m_Info.RightHandEffect > 0)
                            Caster.FixedParticles(0, 10, 5, m_Info.RightHandEffect, EffectLayer.RightHand);
                    }

                    if (m_Caster is PlayerMobile && ClearHandsOnCast) //Mobiles don't need to disarm
                        m_Caster.ClearHands();

                    m_CastTimer = new CastTimer(this, castDelay);
                    m_CastTimer.Start();

                    OnBeginCast();

                    return true;
                }
                return false;
            }
            else
            {
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502625); // Insufficient mana
            }

            return false;
        }

        public abstract void OnCast();

        public virtual void OnPlayerCast()
        {
            OnCast();
        }

        public virtual void OnBeginCast()
        {
        }

        public virtual void GetCastSkills(out double min, out double max)
        {
            min = max = 0;	//Intended but not required for overriding.
        }

        //private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

        public virtual bool CheckFizzle()
        {
            if (m_Scroll is BaseWand)
                return true;

            double minSkill, maxSkill;

            GetCastSkills(out minSkill, out maxSkill);

            if (DamageSkill != CastSkill)
                Caster.CheckSkill(DamageSkill, 0.0, Caster.Skills[DamageSkill].Cap);

            return Caster.CheckSkill(CastSkill, minSkill, maxSkill);
        }

        public abstract int GetMana();

        public virtual int ScaleMana(int mana)
        {
            var scalar = 1.0;

            if (!MindRotSpell.GetMindRotScalar(Caster, ref scalar))
                scalar = 1.0;

            // Lower Mana Cost = 40%
            var lmc = AosAttributes.GetValue(m_Caster, AosAttribute.LowerManaCost);
            if (lmc > 40)
                lmc = 40;

            scalar -= (double)lmc / 100;

            return (int)(mana * scalar);
        }

        public virtual TimeSpan GetDisturbRecovery()
        {
            if (Core.AOS)
                return TimeSpan.Zero;

            var delay = 1.0 - Math.Sqrt((DateTime.Now - m_StartCastTime).TotalSeconds / GetCastDelay().TotalSeconds);

            if (delay < 0.2)
                delay = 0.2;

            return TimeSpan.FromSeconds(delay);
        }

        public virtual int CastRecoveryBase { get { return 6; } }
        //public virtual int CastRecoveryCircleScalar{ get{ return 0; } }
        public virtual int CastRecoveryFastScalar { get { return 1; } }
        public virtual int CastRecoveryPerSecond { get { return 4; } }
        public virtual int CastRecoveryMinimum { get { return 0; } }

        public virtual TimeSpan GetCastRecovery()
        {
            if (!Core.AOS)
                return NextSpellDelay;

            var fcr = AosAttributes.GetValue(m_Caster, AosAttribute.CastRecovery);

            fcr -= ThunderstormSpell.GetCastRecoveryMalus(m_Caster);

            var fcrDelay = -(CastRecoveryFastScalar * fcr);

            var delay = CastRecoveryBase + fcrDelay;

            if (delay < CastRecoveryMinimum)
                delay = CastRecoveryMinimum;

            return TimeSpan.FromSeconds((double)delay / CastRecoveryPerSecond);
        }

        public abstract TimeSpan CastDelayBase { get; }
        public virtual double CastDelayFastScalar { get { return 1; } }
        public virtual double CastDelaySecondsPerTick { get { return 0.25; } }
        public virtual TimeSpan CastDelayMinimum { get { return TimeSpan.FromSeconds(0.25); } }

        /*
		public virtual int CastDelayBase{ get{ return 3; } }
		public virtual int CastDelayCircleScalar{ get{ return 1; } }
		public virtual int CastDelayFastScalar{ get{ return 1; } }
		public virtual int CastDelayPerSecond{ get{ return 4; } }
		public virtual int CastDelayMinimum{ get{ return 1; } }
         * */

        public virtual TimeSpan GetCastDelay()
        {
            if (m_Caster.AccessLevel >= AccessLevel.GameMaster)
                return TimeSpan.Zero;

            if (m_Scroll is BaseWand)
                return TimeSpan.FromSeconds(1.5);

            // Faster casting cap of 2 (if not using the protection spell) 
            // Faster casting cap of 0 (if using the protection spell) 
            // Paladin spells are subject to a faster casting cap of 4 
            // Paladins with magery of 70.0 or above are subject to a faster casting cap of 2 
            int fcMax = 4;

            if (CastSkill == SkillName.Magery || CastSkill == SkillName.Necromancy || (CastSkill == SkillName.Chivalry && m_Caster.Skills[SkillName.Magery].Value >= 70.0))
                fcMax = 2;

            var fc = AosAttributes.GetValue(m_Caster, AosAttribute.CastSpeed);

            if (fc > fcMax)
                fc = fcMax;

            if (ProtectionSpell.Registry.ContainsKey(m_Caster))
                fc -= 2;

            if (EssenceOfWindSpell.IsDebuffed(m_Caster))
                fc -= EssenceOfWindSpell.GetFCMalus(m_Caster);

            var baseDelay = CastDelayBase;

            var fcDelay = TimeSpan.FromSeconds(-(CastDelayFastScalar * fc * CastDelaySecondsPerTick));

            //int delay = CastDelayBase + circleDelay + fcDelay;
            var delay = baseDelay + fcDelay;

            if (delay < CastDelayMinimum)
                delay = CastDelayMinimum;

            //return TimeSpan.FromSeconds( (double)delay / CastDelayPerSecond );
            return delay;
        }

        public virtual void FinishSequence()
        {
            m_State = SpellState.None;

            if (Caster != null)
            {
                if (Caster is PlayerMobile)
                {
                    if (!((PlayerMobile) Caster).HiddenWithSpell) //Taran: Don't reveal when hidden with spell or pot
                        Caster.RevealingAction();
                }
                else
                    Caster.RevealingAction();
            }

            if (m_Caster.Spell == this)
                m_Caster.Spell = null;
        }

        public virtual int ComputeKarmaAward()
        {
            return 0;
        }

        public virtual bool CheckSequence()
        {
            CustomRegion cR;
            IPoint3D p = m_SphereSpellTarget as IPoint3D;
            SpellHelper.GetSurfaceTop(ref p);

            //Iza - gms can cast anything
            if (m_Caster.AccessLevel >= AccessLevel.GameMaster)
                return true;
            //Iza - end

            int mana = ScaleMana(GetMana());

            if (m_Caster.Deleted || !m_Caster.Alive || m_Caster.Spell != this || m_State != SpellState.Sequencing)
            {
                DoFizzle();
            }

            else if (m_SphereSpellTarget != null && m_SphereSpellTarget != m_Caster && (!m_Caster.CanSee(m_SphereSpellTarget) || !m_Caster.InLOS(m_SphereSpellTarget) || !m_Caster.InRange(m_SphereSpellTarget, 15)))
            {
                m_Caster.SendAsciiMessage("Target cannot be seen.");
                DoFizzle();
            }

            else if (m_Scroll != null && !(m_Scroll is Runebook) && (m_Scroll.Amount <= 0 || m_Scroll.Deleted || m_Scroll.RootParent != m_Caster || (m_Scroll is BaseWand && (((BaseWand)m_Scroll).Charges <= 0 || m_Scroll.Parent != m_Caster))))
            {
                DoFizzle();
            }

            else if (!ConsumeReagents())
            {
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502630); // More reagents are needed for this spell.
            }

            else if (m_Caster.Mana < mana)
            {
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502625); // Insufficient mana for this spell.
            }

            else if (m_Caster is PlayerMobile && ((PlayerMobile)m_Caster).PeacedUntil > DateTime.Now)
            {
                m_Caster.SendLocalizedMessage(1072060); // You cannot cast a spell while calmed.
                DoFizzle();
            }

            else if ((cR = Caster.Region as CustomRegion) != null && cR.Controller.IsRestrictedSpell(this))
            {
                m_Caster.SendAsciiMessage("You cannot cast that spell here!");
            }

            else if (p != null && (cR = Region.Find(new Point3D(p), Caster.Map ) as CustomRegion) != null && cR.Controller.IsRestrictedSpell(this))
            {
                m_Caster.SendAsciiMessage("You cannot cast that spell there!");
            }

            else if (Caster.SolidHueOverride == 2535)
            {
                Caster.SendAsciiMessage("You cannot cast spells while using a pitsrune");
            }

            else if (CheckFizzle())
            {
                m_Caster.Mana -= mana;

                if (m_Scroll != null)
                {
                    if (m_Scroll is SpellScroll)
                        m_Scroll.Consume();
                    else if (m_Scroll is BaseWand) // could most likley remove this, but there is a chanse that its not a base wand.
                    {
                        ((BaseWand)m_Scroll).ConsumeCharge(m_Caster);

                        var m = m_Scroll.Movable;

                        m_Scroll.Movable = false;

                        m_Scroll.Movable = m;
                    }
                }

                if (m_Caster is PlayerMobile && ClearHandsOnCast) //Mobiles don't need to disarm
                    m_Caster.ClearHands();

                var karma = ComputeKarmaAward();

                if (karma != 0)
                    Titles.AwardKarma(Caster, karma, true);

                if (TransformationSpellHelper.UnderTransformation(m_Caster, typeof(VampiricEmbraceSpell)))
                {
                    var garlic = false;

                    for (var i = 0; !garlic && i < m_Info.Reagents.Length; ++i)
                        garlic = (m_Info.Reagents[i] == Reagent.Garlic);

                    if (garlic)
                    {
                        m_Caster.SendLocalizedMessage(1061651); // The garlic burns you!
                        AOS.Damage(m_Caster, Utility.RandomMinMax(17, 23), 100, 0, 0, 0, 0);
                    }
                }

                //Passivly gain eval on finishing the spell.
                m_Caster.CheckSkill(DamageSkill, 0.0, 120.0);

                return true;
            }
            else
                DoFizzle();

            return false;
        }

        public bool CheckBSequence(Mobile target)
        {
            return CheckBSequence(target, false);
        }

        public bool CheckBSequence(Mobile target, bool allowDead)
        {
            if (!target.Alive && !allowDead)
            {
                m_Caster.SendLocalizedMessage(501857); // This spell won't work on that!
                return false;
            }
            if (Caster.CanBeBeneficial(target, true, allowDead) && CheckSequence())
            {
                if ( target.Player)
                    Caster.DoBeneficial(target);

                return true;
            }
            return false;
        }

        public bool CheckHSequence(Mobile target)
        {
            if (!target.Alive)
            {
                m_Caster.SendLocalizedMessage(501857); // This spell won't work on that!
                return false;
            }
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                Caster.DoHarmful(target);
                if (target is PlayerMobile && (target.Region is CustomRegion && ((CustomRegion)target.Region).Controller.FizzlePvP))
                    ((PlayerMobile)target).BandageCheck();
                return true;
            }
            return false;
        }

        private class AnimTimer : Timer
        {
            private readonly Spell m_Spell;

            public AnimTimer(Spell spell, int count)
                : base(TimeSpan.Zero, AnimateDelay, count)
            {
                m_Spell = spell;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Spell.State != SpellState.Casting || m_Spell.m_Caster.Spell != m_Spell)
                {
                    Stop();
                    return;
                }

                if (m_Spell.Caster.Body.IsAnimal || m_Spell.Caster.Body.IsMonster) //Taran: Anim for polymorph
                    m_Spell.Caster.Animate(11, 7, 1, true, false, 2);
                //Maka
                else if (!m_Spell.Caster.Mounted && m_Spell.Caster.Body.IsHuman && m_Spell.m_Info.Action >= 0)
                    m_Spell.Caster.Animate(m_Spell.m_Info.Action, 7, 1, true, false, 2);
                else if (m_Spell.Caster.Mounted && m_Spell.Caster.Body.IsHuman && m_Spell.m_Info.Action >= 0)
                    m_Spell.Caster.Animate(m_Spell.m_Info.Action == 263 ? 27 : 26, 5, 1, true, false, 2);

                if (!Running)
                    m_Spell.m_AnimTimer = null;
            }
        }

        private class CastTimer : Timer
        {
            private readonly Spell m_Spell;

            public CastTimer(Spell spell, TimeSpan castDelay) : base(castDelay)
            {
                m_Spell = spell;
                Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                Mobile caster = m_Spell.m_Caster;

                //Taran: Tacky fix for checking range and LoS for NPC's, I don't know where the NPC spell targeting method is so I'm using Combatant instead.
                if (caster is BaseCreature && caster.Combatant != null && (!caster.InLOS(caster.Combatant) || !caster.InRange(caster.Combatant, 12) || !caster.CanSee(caster.Combatant)))
                    m_Spell.DoFizzle();

                else if (m_Spell.m_State == SpellState.Casting)
                {
                    caster.OnSpellCast(m_Spell);
                    caster.Region.OnSpellCast(caster, m_Spell);

                    m_Spell.m_State = SpellState.Sequencing;

                    if (m_Spell.PlayerCaster != null)
                        m_Spell.OnPlayerCast();
                    else
                    {
                        //var originalTarget = m_Spell.m_Caster.Target;
                        caster.NextSpellTime = DateTime.Now + m_Spell.GetCastRecovery();
                        m_Spell.OnCast();
                    }
                }

                m_Spell.m_CastTimer = null;
            }
        }
    }
}