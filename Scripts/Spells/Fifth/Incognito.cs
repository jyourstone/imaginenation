using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Seventh;

namespace Server.Spells.Fifth
{
    public class IncognitoSpell : MagerySpell
    {
        private static readonly Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();
        public override int Sound { get { return 0x3BD; } }

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Incognito", "Kal In Ex",
				206,
				9002,
				Reagent.Bloodmoss,
				Reagent.Garlic,
				Reagent.Nightshade
			);

		public IncognitoSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
            /*
			if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042402 ); // You cannot use incognito while wearing body paint
				return false;
			}
            */
			return true;
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is Mobile)
                Target((Mobile)SphereSpellTarget);
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else
                DoFizzle();
        }

        public void Target(Mobile toHide)
        {
            if (CheckSequence())
            {
                //Only humans can be man and woman?
                //if (!toHide.Body.IsHuman)
                //    return;

                StopTimer(toHide);
                DisguiseTimers.StopTimer(toHide);

                //toHide.BodyMod = Utility.RandomList(400, 401);
                //toHide.HueMod = Utility.RandomSkinHue();
                toHide.NameMod = GetNameMod(toHide.BodyValue);// toHide.Body.IsFemale ? "Woman" : "Man";/*NameList.RandomName("male");*/

                PlayerMobile pm = toHide as PlayerMobile;

                if (pm != null && pm.HairItemID != 5147 && pm.HairItemID != 7947) //Don't change hair on daemons and goblins
                {
                    pm.SetHairMods(Utility.RandomList(m_HairIDs), pm.Body.IsFemale ? 0 : Utility.RandomList(m_BeardIDs));

                    Item hair = pm.FindItemOnLayer(Layer.Hair);

                    if (hair != null)
                        hair.Hue = Utility.RandomHairHue();

                    hair = pm.FindItemOnLayer(Layer.FacialHair);

                    if (hair != null)
                        hair.Hue = Utility.RandomHairHue();
                }

                //BaseArmor.ValidateMobile(toHide);

                Timer t = new InternalTimer(toHide);
                t.Start();

                m_Timers.Add(toHide, t);

                Caster.PlaySound(Sound);
                toHide.PlaySound(Sound);
            }

            FinishSequence();
        }

		public override void OnCast()
		{
            /*
			if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042402 ); // You cannot use incognito while wearing body paint
			}*/
            if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendLocalizedMessage(1061631); // You can't do that while disguised.
            }/*
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ))// || Caster.IsBodyMod )
			{
				DoFizzle();
			}*/
			else if ( CheckSequence() )
			{
				if ( Caster.BeginAction( typeof( IncognitoSpell ) ) )
				{
                    DisguiseTimers.StopTimer(Caster);

					//Caster.BodyMod = Utility.RandomList( 400, 401 );
					//Caster.HueMod = Utility.RandomSkinHue();

				    Caster.NameMod = GetNameMod(Caster.BodyValue); // Caster.Body.IsFemale ? "Woman"/*NameList.RandomName( "female" )*/ : "Man";/*NameList.RandomName("male");*/

					//Caster.FixedParticles( 0x373A, 10, 15, 5036, EffectLayer.Head );
					Caster.PlaySound( 0x3BD );

					//BaseArmor.ValidateMobile( Caster );

					StopTimer( Caster );

					Timer t = new InternalTimer( Caster );

                    m_Timers.Add(Caster, t);

					t.Start();
				}
				else
				{
					Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				}
			}

			FinishSequence();
		}

        public static string GetNameMod(int body)
        {
            string name;

            switch (body)
            {
                case 400:
                    name = "Man";
                    break;
                case 401:
                    name = "Woman";
                    break;
                case 0xD0:
                    name = "Chicken";
                    break;
                case 0xD9:
                    name = NameList.RandomName("dog");
                    break;
                case 0xC9:
                    name = NameList.RandomName("cat");
                    break;
                case 0xE1:
                    name = "Wolf";
                    break;
                case 0xD6:
                    name = "Panther";
                    break;
                case 0x1D:
                    name = "Gorilla";
                    break;
                case 0xD3:
                    name = "Black Bear";
                    break;
                case 0xD4:
                    name = "Grizzly Bear";
                    break;
                case 0xD5:
                    name = "Polar Bear";
                    break;
                case 0xCC:
                    name = NameList.RandomName("horse");
                    break;
                case 0x33:
                    name = "Slime";
                    break;
                case 0x11:
                    name = NameList.RandomName("orc");
                    break;
                case 0x24:
                    name = NameList.RandomName("lizardman");
                    break;
                case 0x04:
                    name = "Gargoyle";
                    break;
                case 0x01:
                    name = "Ogre";
                    break;
                case 0x36:
                    name = "Troll";
                    break;
                case 0x02:
                    name = "Ettin";
                    break;
                case 0x15:
                    name = "Giant Serpent";
                    break;
                case 0x09:
                    name = NameList.RandomName("daemon");
                    break;
                case 0x3B:
                case 0xC:
                    name = "Dragon";
                    break;
                default:
                    name = null;
                    break;
            }

            return name;
        }

		public static bool StopTimer( Mobile m )
		{
            if (!m_Timers.ContainsKey(m))
                return false;

            //Stop the ticking timer
            m_Timers[m].Stop();

            RemoveIncognitoEffect(m);
    
            //Remove it from our dictionary
            m_Timers.Remove(m);

			return true;
		}

        private static void RemoveIncognitoEffect(Mobile m)
        {
            if (m is PlayerMobile)
                ((PlayerMobile)m).SetHairMods(-1, -1);

            //m.BodyMod = 0;
            //m.HueMod = -1;
            m.NameMod = null;

            //BaseArmor.ValidateMobile(m);
        }

		private static readonly int[] m_HairIDs = new int[]
			{
				0x2044, 0x2045, 0x2046,
				0x203C, 0x203B, 0x203D,
				0x2047, 0x2048, 0x2049,
				0x204A, 0x0000
			};

		private static readonly int[] m_BeardIDs = new int[]
			{
				0x203E, 0x203F, 0x2040,
				0x2041, 0x204B, 0x204C,
				0x204D, 0x0000
			};

		private class InternalTimer : Timer
		{
			private readonly Mobile m_Owner;

			public InternalTimer( Mobile owner ) : base( TimeSpan.Zero )
			{
				m_Owner = owner;

				int val = ((6 * owner.Skills.Magery.Fixed) / 50) + 1;

				if ( val > 144 )
					val = 144;

				Delay = TimeSpan.FromSeconds( val );
				Priority = TimerPriority.OneSecond;
			}

            protected override void OnTick()
            {
                //Restore our mobile to its previous state
                RemoveIncognitoEffect(m_Owner);

                //Remove the mobile and the timer from the dictionary
                m_Timers.Remove(m_Owner);
            }
		}
	}
}
