using System;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Xanthos.ShrinkSystem;

namespace Server.Regions
{
    public class CustomRegion : GuardedRegion
    {               
        private readonly RegionControl m_Controller;
        private static readonly Hashtable m_NextDisplayTime = new Hashtable();

        public RegionControl Controller
        {
            get { return m_Controller; }
        }

        public CustomRegion(RegionControl control): base(control.RegionName, control.Map, control.RegionPriority, control.RegionArea)
        {
            Disabled = !control.IsGuarded;
            Music = control.Music;
            m_Controller = control;
            EventSink.Disconnected += OnPlayerDisconnected;
            EventSink.Login += OnPlayerLogin;
        }

        private Timer m_Timer;

        public override void OnDeath( Mobile m )
        {
            //bool toreturn = true;

            if ( m != null && !m.Deleted)
            {

                if (m is PlayerMobile && m_Controller.NoPlayerItemDrop)
                {
                    if (m.Female)
                    {
                        m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        m.Body = 403;
                        m.Hidden = true;
                    }
                    else
                    {
                        m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        m.Body = 402;
                        m.Hidden = true;
                    }
                    m.Hidden = false;
                    //toreturn = false;
                }
                else if ( !(m is PlayerMobile) && m_Controller.NoNPCItemDrop)
                {
                    if (m.Female)
                    {
                        m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        m.Body = 403;
                        m.Hidden = true;
                    }
                    else
                    {
                        m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        m.Body = 402;
                        m.Hidden = true;
                    }
                    m.Hidden = false;
                    //toreturn = false;
                }
                //else
                    //toreturn = true;

                // Start a 1 second timer
                // The Timer will check if they need moving, corpse deleting etc.
                m_Timer = new MovePlayerTimer(m, m_Controller);
                m_Timer.Start();

                base.OnDeath(m);
            }

            return;
        }

        private class MovePlayerTimer : Timer
        {
            private readonly Mobile m;
            private readonly RegionControl m_Controller;

            public MovePlayerTimer(Mobile m_Mobile, RegionControl controller)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.FiftyMS;
                m = m_Mobile;
                m_Controller = controller;
            }

            protected override void OnTick()
            {
                if (m == null)
                {
                    Stop();
                    return;
                }

                // Emptys the corpse and places items on ground
                if ( m is PlayerMobile )
                {
                    if (m_Controller.EmptyPlayerCorpse)
                    {
                        if (m.Corpse != null)
                        {
                            ArrayList corpseitems = new ArrayList(m.Corpse.Items);

                            foreach (Item item in corpseitems)
                            {
                                if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack) && (item.Layer != Layer.Hair) && (item.Layer != Layer.FacialHair) && (item.Layer != Layer.Mount))
                                {
                                    if ((item.LootType != LootType.Blessed))
                                    {
                                        item.MoveToWorld(m.Corpse.Location, m.Corpse.Map);
                                    }
                                }
                            }
                        }
                    }
                }
                else if ( m_Controller.EmptyNPCCorpse )
                {
                    if (m.Corpse != null)
                    {
                        ArrayList corpseitems = new ArrayList(m.Corpse.Items);

                        foreach (Item item in corpseitems)
                        {
                            if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack) && (item.Layer != Layer.Hair) && (item.Layer != Layer.FacialHair) && (item.Layer != Layer.Mount))
                            {
                                if ((item.LootType != LootType.Blessed))
                                {
                                    item.MoveToWorld(m.Corpse.Location, m.Corpse.Map);
                                }
                            }
                        }
                    }
                }

                Mobile newnpc = null;   

                // Resurrects Players
                if (m is PlayerMobile)
                {
                    if (m_Controller.ResPlayerOnDeath && !m.Alive)
                    {
                        if (m.Corpse != null)
                            m.MoveToWorld(m.Corpse.GetWorldLocation(), m.Corpse.Map);
                        m.Resurrect();
                        m.SendMessage("You have been Resurrected");
                    }
                }
                else if (m_Controller.ResNPCOnDeath && !m.Alive)
                {
                    if (m.Corpse != null)
                    {
                        Type type = m.GetType();
                        newnpc = Activator.CreateInstance(type) as Mobile;
                        if (newnpc != null)
                        {
                            newnpc.Location = m.Corpse.Location;
                            newnpc.Map = m.Corpse.Map;
                        }
                    }
                }

                // Deletes the corpse 
                if ( m is PlayerMobile )
                {
                    if (m_Controller.DeletePlayerCorpse)
                    {
                        if (m.Corpse != null)
                        {
                            m.Corpse.Delete();
                        }
                    }
                }
                else if ( m_Controller.DeleteNPCCorpse )
                {
                    if (m.Corpse != null)
                    {
                        m.Corpse.Delete();
                    }
                }           

                // Move Mobiles
                if ( m is PlayerMobile )
                {
                    if (m_Controller.MovePlayerOnDeath)
                    {
                        {
                            m.Map = m_Controller.MovePlayerToMap;
                            m.Location = m_Controller.MovePlayerToLoc;
                        }
                    }
                }
                else if ( m_Controller.MoveNPCOnDeath )
                {
                    if (newnpc != null)
                    {
                        newnpc.Map = m_Controller.MoveNPCToMap;
                        newnpc.Location = m_Controller.MoveNPCToLoc;
                    }
                }
                 
                Stop();

            }
        }

        public override bool IsDisabled()
        {
            if (!m_Controller.IsGuarded != Disabled)
                m_Controller.IsGuarded = !Disabled;

			return Disabled;
        }

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if ((!m_Controller.AllowBenefitPlayer && target.Player) || (!m_Controller.AllowBenefitNPC && target is BaseCreature))
            {
                from.SendMessage("You cannot perform benificial acts on your target.");
                return false;
            }

            return base.AllowBeneficial(from, target);
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            string message = string.Empty;

            if (!m_Controller.AllowPvP && from.Player && target.Player)
                message = "You cannot attack other players here";
            else if (!m_Controller.AllowHarmPlayer && target.Player)
                message = "You cannot attack here";
            else if (!m_Controller.AllowHarmNPC && target is BaseCreature)
                message = "You cannot attack NPC's here";

            if (!string.IsNullOrEmpty(message))
            {
                from.SendMessage(message);
                return false;
            }

            return base.AllowHarmful(from, target);
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return m_Controller.AllowHousing;
        }

        public override bool AllowSpawn()
        {
            return m_Controller.AllowSpawn;
        }

        public override bool CanUseStuckMenu(Mobile m)
        {
            if (!m_Controller.CanUseStuckMenu)
                m.SendMessage("You cannot use the Stuck menu here.");
            return m_Controller.CanUseStuckMenu;
        }

        public override bool OnDamage(Mobile m, ref int Damage)
        {
            if (!m_Controller.CanBeDamaged)
            {
                m.SendMessage("You cannot be damaged here.");
            }

            return m_Controller.CanBeDamaged;
        }

        public override bool AllowTrade
        {
            get
            {
                return m_Controller.AllowTrade;
            }
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                bool restricted = m_Controller.IsRestrictedSpell(s);
                if (restricted)
                {
                    from.SendMessage("You cannot cast that spell here.");
                    return false;
                }

                //if ( s is EtherealSpell && !CanMountEthereal ) Grr, EthereealSpell is private :<
                if (!m_Controller.CanMountEthereal && ((Spell)s).Info.Name == "Ethereal Mount") //Hafta check with a name compare of the string to see if ethy
                {
                    from.SendMessage("You cannot mount your ethereal here.");
                    return false;
                }
            }

            //Console.WriteLine( m_Controller.GetRegistryNumber( s ) );

            //return base.OnBeginSpellCast( from, s );
            return true;	//Let users customize spells, not rely on weather it's guarded or not.
        }

        public override bool OnDecay(Item item)
        {
            return m_Controller.ItemDecay;
        }

        public override bool OnHeal(Mobile m, ref int Heal)
        {
            if (!m_Controller.CanHeal)
            {
                m.SendMessage("You cannot be healed here.");
            }

            return m_Controller.CanHeal;
        }

        public override bool OnSkillUse(Mobile m, int skill)
        {
            bool restricted = m_Controller.IsRestrictedSkill(skill);
            if (restricted && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("You cannot use that skill here.");
                return false;
            }

            return base.OnSkillUse(m, skill);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);

            if (!(m is PlayerMobile))
                return;

            if (!m_Controller.AllowHarmPlayer || !m_Controller.CanBeDamaged)
                m.SendMessage("You lose your feeling of safety.");

            if (m_Controller.ShowExitMessage && !string.IsNullOrEmpty(Name))
                m.SendMessage("You have left {0}", Name);
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (!(m is PlayerMobile))
                return;

            PlayerMobile from = (PlayerMobile) m;

            if (!m_Controller.AllowHarmPlayer || !m_Controller.CanBeDamaged)
                from.SendMessage("You have a feeling of complete safety.");
            
            if (m_Controller.ShowEnterMessage && !string.IsNullOrEmpty(Name))
                //if (!m_Controller.BroadcastArriveMsg || (m_Controller.BroadcastArriveMsg && !from.ShowArriveMsg))
                    from.SendMessage("You have entered {0}", Name);

			if ( ((PlayerMobile)m).Young && m_Controller.IgnoreYoungProtection)
				m.SendGump( new YoungDungeonWarning() );
        }

        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (!m_Controller.CanEnter && !Contains(oldLocation))
            {
                m.SendMessage("You cannot enter this area.");
                return false;
            }

            return true;
        }

        public override TimeSpan GetLogoutDelay(Mobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return m_Controller.PlayerLogoutDelay;

            return base.GetLogoutDelay(m);
        }

        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (!m_Controller.CanUnshrink && o is ShrinkItem)
            {
                m.SendMessage("You cannot unshrink pets here.");
                return false;
            }
            if (!m_Controller.CanUsePotHeal && o is BaseHealPotion)
            {
                m.SendMessage("You cannot drink heal potions here.");
                return false;
            }
            if (!m_Controller.CanUsePotMana && o is BaseManaPotion)
            {
                m.SendMessage("You cannot drink mana potions here.");
                return false;
            }
            if (!m_Controller.CanUsePotStam && o is BaseRefreshPotion)
            {
                m.SendMessage("You cannot drink refresh potions here.");
                return false;
            }
            if (!m_Controller.CanUsePotShrink && o is ShrinkPotion)
            {
                m.SendMessage("You cannot use shrink potions here.");
                return false;
            }
            if (!m_Controller.CanUsePotExplosion && o is BaseExplosionPotion)
            {
                m.SendMessage("You cannot use explosion potions here");
                return false;
            }
            if (!m_Controller.CanUsePotOthers && o is BasePotion)
            {
                m.SendMessage("You cannot drink that potion here.");
                return false;
            }

            if (o is Corpse)
            {
                Corpse c = (Corpse)o;

                bool canLoot;

                if (c.Owner == m)
                    canLoot = m_Controller.CanLootOwnCorpse;
                else if (c.Owner is PlayerMobile)
                    canLoot = m_Controller.CanLootPlayerCorpse;
                else
                    canLoot = m_Controller.CanLootNPCCorpse;

                if (!canLoot)
                    m.SendMessage("You cannot loot that corpse here.");

                if (m.AccessLevel >= AccessLevel.GameMaster && !canLoot)
                {
                    m.SendMessage("This is unlootable but you are able to open that with your Godly powers.");
                    return true;
                }

                return canLoot;
            }

            #region Check for restricted skilluse through basetools
            //Taran: There must be a better way to check restricted skills when using crafting tools
            if (m.AccessLevel >= AccessLevel.GameMaster)
                return true;

            bool canUse = true;

            if (o is BaseTool)
            {
                BaseTool tool = (BaseTool)o;

                if (tool.CraftSystem == DefAlchemy.CraftSystem && m_Controller.IsRestrictedSkill(0))
                    canUse = false;

                else if ((tool.CraftSystem == DefBlacksmithy.CraftSystem) && m_Controller.IsRestrictedSkill(7))
                    canUse = false;

                else if (tool.CraftSystem == DefBowFletching.CraftSystem && m_Controller.IsRestrictedSkill(8))
                    canUse = false;

                else if (tool.CraftSystem == DefCarpentry.CraftSystem && m_Controller.IsRestrictedSkill(11))
                    canUse = false;

                else if (tool.CraftSystem == DefCartography.CraftSystem && m_Controller.IsRestrictedSkill(12))
                    canUse = false;

                else if (tool.CraftSystem == DefCooking.CraftSystem && m_Controller.IsRestrictedSkill(13))
                    canUse = false;

                else if (tool.CraftSystem == DefTinkering.CraftSystem && m_Controller.IsRestrictedSkill(37))
                    canUse = false;

                else if (tool.CraftSystem == DefTailoring.CraftSystem && m_Controller.IsRestrictedSkill(34))
                    canUse = false;

                else if (tool.CraftSystem == DefInscription.CraftSystem && m_Controller.IsRestrictedSkill(23))
                    canUse = false;
            }

            else if (o is BaseIngot && m_Controller.IsRestrictedSkill(7))
                canUse = false;

            else if (o is BaseReagent && m_Controller.IsRestrictedSkill(0))
                canUse = false;

            else if (o is Lockpick && m_Controller.IsRestrictedSkill(24))
                canUse = false;

            else if (o is Kindling && m_Controller.IsRestrictedSkill(10))
                canUse = false;

            else if (o is BaseInstrument && m_Controller.IsRestrictedSkill(29))
                canUse = false;

            if (!canUse)
                m.SendMessage("You cannot use this here.");

            return canUse;
            #endregion
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            if (m_Controller.LightLevel >= 0)
                global = m_Controller.LightLevel;
            else
                base.AlterLightLevel(m, ref global, ref personal);
        }

        private void OnPlayerDisconnected(DisconnectedEventArgs e)
        {
            Mobile from = e.Mobile;

            if (m_Controller.LogoutMove && from.Region == this)
                new LogoutMove(from, m_Controller, m_Controller.PlayerLogoutDelay).Start();
        }

        private static void OnPlayerLogin(LoginEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            if (pm != null)
            {
                if (pm.CurrentLogoutMoveTimer != null)
                {
                    pm.CurrentLogoutMoveTimer.Stop();
                    pm.CurrentLogoutMoveTimer = null;
                }
            }
        }

        private class LogoutMove : Timer
        {
            private readonly Mobile m;
            private readonly RegionControl m_Controller;

            public LogoutMove(Mobile m_Mobile, RegionControl controller, TimeSpan delay) : base(delay)
            {
                Priority = TimerPriority.OneSecond;
                m = m_Mobile;
                m_Controller = controller;
                ((PlayerMobile)m).CurrentLogoutMoveTimer = this;
            }

            protected override void OnTick()
            {
                if (m != null && m_Controller != null && m_Controller.LogoutMap != null)
                {
                    m.Location = m_Controller.LogoutLoc;
                    m.LogoutLocation = m_Controller.LogoutLoc;
                    m.Map = m_Controller.LogoutMap;
                    m.LogoutMap = m_Controller.LogoutMap;

                    Stop();
                    ((PlayerMobile)m).CurrentLogoutMoveTimer = null;
                }
            }
        }

        /*public override bool CheckAccessibility(Item item, Mobile from)
        {

            if (item is BasePotion && !m_Controller.CanUsePotions)
            {
                from.SendMessage("You cannot drink potions here.");
                return false;
            }

            if (item is Corpse)
            {
                Corpse c = item as Corpse;

                bool canLoot;

                if (c.Owner == from)
                    canLoot = m_Controller.CanLootOwnCorpse;
                else if (c.Owner is PlayerMobile)
                    canLoot = m_Controller.CanLootPlayerCorpse;
                else
                    canLoot = m_Controller.CanLootNPCCorpse;

                if (!canLoot)
                    from.SendMessage("You cannot loot that corpse here.");

                if (from.AccessLevel >= AccessLevel.GameMaster && !canLoot)
                {
                    from.SendMessage("This is unlootable but you are able to open that with your Godly powers.");
                    return true;
                }

                return canLoot;
            }

            return base.CheckAccessibility(item, from);
        }*/

    }
}
