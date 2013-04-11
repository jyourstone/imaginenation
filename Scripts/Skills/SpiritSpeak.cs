using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.SkillHandlers
{
	class SpiritSpeak
	{
		public static void Initialize()
		{
			SkillInfo.Table[32].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                new SpiritSpeakStartTimer(m).Start();
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
		}

        private class SpiritSpeakStartTimer : Timer, IAction
        {
            private readonly Mobile m_Owner;

            public SpiritSpeakStartTimer(Mobile m)
                : base(TimeSpan.FromSeconds(2.5))
            {
                m_Owner = m;

                if (m_Owner is PlayerMobile)
                    ((PlayerMobile)m_Owner).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_Owner.CheckSkill(SkillName.SpiritSpeak, 0, 100))
                {
                    m_Owner.CanHearGhosts = true;

                    double duration = m_Owner.Skills[SkillName.SpiritSpeak].Base / 50;
                    duration *= 90;

                    if (duration < 15)
                        duration = 15;

                    new SpiritSpeakEndTimer(m_Owner, duration).Start();

                    m_Owner.PlaySound(0x24A);
                    m_Owner.SendLocalizedMessage(502444);//You contact the netherworld.     
                }
                else
                {
                    m_Owner.SendLocalizedMessage(502443);//You fail to contact the netherworld.
                    m_Owner.CanHearGhosts = false;
                }

                if (m_Owner is PlayerMobile)
                    ((PlayerMobile)m_Owner).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You fail to contact the netherworld");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }

		private class SpiritSpeakEndTimer : Timer
		{
			private readonly Mobile m_Owner;
            private static readonly Dictionary<Mobile, SpiritSpeakEndTimer> m_EndTimerCollection = new Dictionary<Mobile, SpiritSpeakEndTimer>();

            public SpiritSpeakEndTimer(Mobile m, double duration) : base(TimeSpan.FromSeconds(duration))
			{
				m_Owner = m;
				Priority = TimerPriority.FiveSeconds;

                //Check if the mobile alreayd has a end timer bound to it.
                if (m_EndTimerCollection.ContainsKey(m_Owner))
                {
                    if (m_EndTimerCollection[m_Owner] != null)
                        m_EndTimerCollection[m_Owner].Stop();

                    m_EndTimerCollection[m_Owner] = this;
                }
                else
                    m_EndTimerCollection.Add(m_Owner, this);
			}

			protected override void OnTick()
			{
                //Remove the mobile from the dictionary when the timer ends
                if (m_EndTimerCollection.ContainsKey(m_Owner))
                    m_EndTimerCollection.Remove(m_Owner);

				m_Owner.CanHearGhosts = false;
				m_Owner.SendLocalizedMessage( 502445 );//You feel your contact with the neitherworld fading.
			}
		}

		private class SpiritSpeakSpell : Spell
		{
            private static readonly SpellInfo m_Info = new SpellInfo("Spirit Speak", "", 269);

			public override bool BlockedByHorrificBeast{ get{ return false; } }

			public SpiritSpeakSpell( Mobile caster ) : base( caster, null, m_Info )
			{
			}

			public override bool ClearHandsOnCast{ get{ return false; } }

            public override double CastDelayFastScalar { get { return 0; } }
            public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.0); } }

			public override int GetMana()
			{
				return 0;
			}

			public override void OnCasterHurt()
			{
				if ( IsCasting )
					Disturb( DisturbType.Hurt, false, true );
			}

			public override bool ConsumeReagents()
			{
				return true;
			}

			public override bool CheckFizzle()
			{
				return true;
			}

			public override bool CheckNextSpellTime{ get{ return false; } }

			public override void OnDisturb( DisturbType type, bool message )
			{
				Caster.NextSkillTime = DateTime.Now;

				base.OnDisturb( type, message );
			}

			public override bool CheckDisturb( DisturbType type, bool checkFirst, bool resistable )
			{
				if ( type == DisturbType.EquipRequest || type == DisturbType.UseRequest )
					return false;

				return true;
			}

			public override void SayMantra()
			{
				// Anh Mi Sah Ko
				Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, 1062074, "", false );
				Caster.PlaySound( 0x24A );
			}

			public override void OnCast()
			{
				Corpse toChannel = null;

				foreach ( Item item in Caster.GetItemsInRange( 3 ) )
				{
                    if (item is Corpse && !((Corpse)item).Channeled)
                    {
						toChannel = (Corpse)item;
						break;
					}
				}

				int max, min, mana, number;

				if ( toChannel != null )
				{
					min = 1 + (int)(Caster.Skills[SkillName.SpiritSpeak].Value * 0.25);
					max = min + 4;
					mana = 0;
					number = 1061287; // You channel energy from a nearby corpse to heal your wounds.
				}
				else
				{
					min = 1 + (int)(Caster.Skills[SkillName.SpiritSpeak].Value * 0.25);
					max = min + 4;
					mana = 10;
					number = 1061286; // You channel your own spiritual energy to heal your wounds.
				}

				if ( Caster.Mana < mana )
				{
					Caster.SendLocalizedMessage( 1061285 ); // You lack the mana required to use this skill.
				}
				else
				{
					Caster.CheckSkill( SkillName.SpiritSpeak, 0.0, 120.0 );

					if ( Utility.RandomDouble() > (Caster.Skills[SkillName.SpiritSpeak].Value / 100.0) )
					{
						Caster.SendLocalizedMessage( 502443 ); // You fail your attempt at contacting the netherworld.
					}
					else
					{
						if ( toChannel != null )
						{
							toChannel.Channeled = true;
							toChannel.Hue = 0x835;
						}

						Caster.Mana -= mana;
						Caster.SendLocalizedMessage( number );

						if ( min > max )
							min = max;

						Caster.Hits += Utility.RandomMinMax( min, max );

						Caster.FixedParticles( 0x375A, 1, 15, 9501, 2100, 4, EffectLayer.Waist );
					}
				}

				FinishSequence();
			}
		}
	}
}