using System;
using System.Collections.Generic;
using Server.Custom.Polymorph;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Fifth;

namespace Server.Spells.Seventh
{
    public class PolymorphSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 527; } }
        public override bool HasNoTarget { get { return true; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Polymorph", "Vas Ylem Rel",
				263,
				9002,
				Reagent.Bloodmoss,
				Reagent.SpidersSilk,
				Reagent.MandrakeRoot
			);

		private readonly int m_NewBody;
        private readonly PolymorphEntry m_PolymorphEntry;

		public PolymorphSpell( Mobile caster, Item scroll, int body ) : base( caster, scroll, m_Info )
		{
			m_NewBody = body;
		}

		public PolymorphSpell( Mobile caster, Item scroll ) : this(caster,scroll,0)
		{
		}

        public PolymorphSpell(Mobile caster, Item scroll, PolymorphEntry polymorphEntry)  : base(caster, scroll, m_Info)
        {
            m_PolymorphEntry = polymorphEntry;
            m_NewBody = polymorphEntry.BodyID;
        }

		public override bool CheckCast()
		{
			if ( TransformationSpellHelper.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061633 ); // You cannot polymorph while in that form.
				return false;
			}
            if (DisguiseTimers.IsDisguised(Caster))
            {
				Caster.SendLocalizedMessage( 502167 ); // You cannot polymorph while disguised.
				return false;
			}
			if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042512 ); // You cannot polymorph while wearing body paint
				return false;
			}/*
            if ( Caster.NameMod != null || Caster.HueMod != -1 )
            {
                Caster.SendAsciiMessage("You cannot polymorph while incognito");
                return false;
            }*/
			if ( m_NewBody == 0 )
			{
                Caster.SendGump(new PolymorphGump(Caster, Scroll));
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
            if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( TransformationSpellHelper.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061633 ); // You cannot polymorph while in that form.
			}
            else if (DisguiseTimers.IsDisguised(Caster))
            {
				Caster.SendLocalizedMessage( 502167 ); // You cannot polymorph while disguised.
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042512 ); // You cannot polymorph while wearing body paint
			}/*
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
				DoFizzle();
			}*/
			else if ( CheckSequence() )
			{
                StopTimer(Caster); //Reset polymorph spell

				if ( Caster.BeginAction( typeof( PolymorphSpell ) ) )
				{
                    if (m_PolymorphEntry.BodyID != 0)
                    {
                        if (!((Body)m_PolymorphEntry.BodyID).IsHuman)
                        {
                            IMount mt = Caster.Mount;

                            if (mt != null)
                                mt.Rider = null;
                        }

                        if (m_PolymorphEntry.BodyID == 0x3B) //Dragon, two different body IDs
                            Caster.BodyMod = Utility.RandomList(0x3B, 0xC);
                        else
                            Caster.BodyMod = m_PolymorphEntry.BodyID;

                        Caster.PlaySound(Sound);

                        StopTimer(Caster);

                        Timer t = new InternalTimer(Caster, m_PolymorphEntry);

                        m_Timers.Add(Caster, t);

                        t.Start();

                        BaseArmor.ValidateMobile(Caster);
                        BaseWeapon.ValidateMobile(Caster);

                        if (Caster.NameMod != null) //Caster has incognito, need to update name
                            Caster.NameMod = IncognitoSpell.GetNameMod(Caster.BodyValue);
                    }
				}
				else
				{
					Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				}
			}

			FinishSequence();
		}

        private static readonly Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

		public static bool StopTimer( Mobile m )
		{
            if (!m_Timers.ContainsKey(m))
                return false;

            RemoveMorphEffect(m);

            //Stop the timer
            m_Timers[m].Stop();

            //Remove it from the dictionary
			m_Timers.Remove( m );

			return true;
		}

        private static void RemoveMorphEffect(Mobile m)
        {
            m.BodyMod = 0;

            string modName = m.Serial + "Polymorph";
            m.RemoveStatMod(modName + "Str");
            m.RemoveStatMod(modName + "Dex");
            m.RemoveStatMod(modName + "Int");

            m.VirtualArmorMod = 0;
            
            if (m is PlayerMobile)
            {
                PlayerMobile pm = ((PlayerMobile) m);
                pm.MinDamage = 0;
                pm.MaxDamage = 0;
                pm.SwingSpeed = 0;
            }

            m.EndAction(typeof(PolymorphSpell));

            BaseArmor.ValidateMobile(m);

            if (m.NameMod != null) //Player has incognito, need to update name
                m.NameMod = IncognitoSpell.GetNameMod(m.BodyValue);
        }

		private class InternalTimer : Timer
		{
			private readonly Mobile m_Owner;
            private readonly PolymorphEntry m_PolymorphEntry;

            public InternalTimer(Mobile owner, PolymorphEntry polymorphEntry)
                : base(TimeSpan.Zero)
            {
                m_Owner = owner;
                m_PolymorphEntry = polymorphEntry;

                double val = (owner.Skills[SkillName.Magery].Value) * 0.15;

                if (val > 120)
                    val = 120;

                Delay = TimeSpan.FromMinutes(val);
                Priority = TimerPriority.OneSecond;

                string modName = m_Owner.Serial + "Polymorph";

                PlayerMobile pm = ((PlayerMobile) m_Owner);

                foreach (Custom.Polymorph.StatMod sm in PolymorphEntry.EntryInfo[m_PolymorphEntry.ArtID].StatMods)
                {
                    switch (sm.Type)
                    {
                        case StatModType.Strength:
                            m_Owner.AddStatMod(new StatMod(StatType.Str, modName + "Str", sm.Value, TimeSpan.Zero));
                            break;
                        case StatModType.Dexterity:
                            m_Owner.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", sm.Value, TimeSpan.Zero));
                            break;
                        case StatModType.Intelligence:
                            m_Owner.AddStatMod(new StatMod(StatType.Int, modName + "Int", sm.Value, TimeSpan.Zero));
                            break;
                        case StatModType.Armor:
                            m_Owner.VirtualArmorMod = sm.Value;
                            break;
                        case StatModType.MinDamage:
                            if (pm != null)
                                pm.MinDamage = sm.Value;
                            break;
                        case StatModType.MaxDamage:
                            if (pm != null)
                                pm.MaxDamage = sm.Value;
                            break;
                        case StatModType.SwingSpeed:
                            if (pm != null)
                                pm.SwingSpeed = sm.Value;
                            break;
                        default:
                            break;
                    }
                }
            }

		    protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;

                    string modName = m_Owner.Serial + "Polymorph";
                    m_Owner.RemoveStatMod(modName + "Str");
                    m_Owner.RemoveStatMod(modName + "Dex");
                    m_Owner.RemoveStatMod(modName + "Int");

				    m_Owner.VirtualArmorMod = 0;

                    if (m_Owner is PlayerMobile)
                    {
                        PlayerMobile pm = ((PlayerMobile)m_Owner);
                        pm.MinDamage = 0;
                        pm.MaxDamage = 0;
                        pm.SwingSpeed = 0;
                    }

					m_Owner.EndAction( typeof( PolymorphSpell ) );
                    StopTimer(m_Owner);

					BaseArmor.ValidateMobile( m_Owner );

                    if (m_Owner.NameMod != null) //Player has incognito, need to update name
                        m_Owner.NameMod = IncognitoSpell.GetNameMod(m_Owner.BodyValue);
				}
			}
		}
	}
}
