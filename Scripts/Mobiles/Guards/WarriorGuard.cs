using System;
using Server.Items;
using Server.Regions;

namespace Server.Mobiles
{
	public class WarriorGuard : BaseGuard
	{
		private Timer m_AttackTimer, m_IdleTimer;
        private readonly BaseArmor[] m_GuardArmor = new BaseArmor[] { new PlateGorget(), new PlateArms(), new PlateGloves(), new PlateLegs() };
		private Mobile m_Focus;

        [Constructable]
        public WarriorGuard(bool disappears) : this(null)
        {
            Disappears = disappears;
        }

		[Constructable]
		public WarriorGuard() : this( null )
		{
		}

        public WarriorGuard(Mobile target) : base(target)
        {
            InitStats(1000, 1000, 1000);

            SpeechHue = 0;

            Hue = Utility.RandomSkinHue();

            #region Armor
            for (int i = 0; i < m_GuardArmor.Length; ++i)
            {
                m_GuardArmor[i].Resource = GuardTheme;
                AddItem(m_GuardArmor[i]);
                m_GuardArmor[i].Movable = false;
            }
            #endregion

            #region Cloth
            BaseClothing bC = null;

            switch (Utility.Random(3))
            {
                case 0:
                    bC = new Doublet();
                    break;
                case 1:
                    bC = new Tunic();
                    break;
                case 2:
                    bC = new BodySash();
                    bC.Layer = Layer.Earrings;
                    break;
            }

            if (bC != null)
            {
                bC.Resource = GuardTheme;
                AddItem(bC);
                bC.Movable = false;
            }
            #endregion

            #region Male/Female
            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                if (string.IsNullOrEmpty(Name))
                {
                    Name = NameList.RandomName("female");
                    Title = "the guard";
                }

                FemalePlateChest ar = new FemalePlateChest();
                ar.Resource = GuardTheme;
                AddItem(ar);
                ar.Movable = false;
            }
            else
            {
                Body = 0x190;
                if (string.IsNullOrEmpty(Name))
                {
                    Name = NameList.RandomName("male");
                    Title = "the guard";
                }

                PlateChest ar = new PlateChest();
                ar.Resource = GuardTheme;
                AddItem(ar);
                ar.Movable = false;
            }
            #endregion

            Utility.AssignRandomHair(this);

            if (Utility.RandomBool())
                Utility.AssignRandomFacialHair(this, HairHue);

            #region Weapon
            BaseWeapon weapon;

            switch(Utility.Random(2))
            {
                case 1: weapon = new Halberd(); break;
                case 2:
                    weapon = new Longsword();
                    BaseShield shield = new OrderShield();
                    shield.Resource = GuardTheme;
                    AddItem(shield);
                    shield.Movable = false;
                    break;
                default:
                    weapon = new Halberd(); break;
            }

            weapon.Crafter = this;
            weapon.Resource = GuardTheme;
            weapon.Quality = WeaponQuality.Exceptional;
            weapon.Speed = 300;
            weapon.MinDamage = 10000;
            weapon.MaxDamage = 10000;
            AddItem(weapon);
            weapon.Movable = false;
            #endregion

            Container pack = new Backpack();
            pack.Movable = false;
            pack.DropItem(new Gold(10, 25));
            AddItem(pack);

            Skills[SkillName.Anatomy].Base = 120.0;
            Skills[SkillName.Tactics].Base = 125.0;
            Skills[SkillName.Swords].Base = 100.0;
            Skills[SkillName.MagicResist].Base = 120.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

            //if (Utility.RandomDouble() <= 0.1)
            //    new Horse().Rider = this;

            NextCombatTime = DateTime.Now + TimeSpan.FromSeconds(1.10);
            Focus = target;
        }

	    public WarriorGuard( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Focus != null && m_Focus.Alive )
				new AvengeTimer( m_Focus ).Start(); // If a guard dies, three more guards will spawn

			return base.OnBeforeDeath();
		}

        [CommandProperty( AccessLevel.GameMaster )]
		public override Mobile Focus
		{

			get
			{
				return m_Focus;
			}
			set
			{
				if ( Deleted )
					return;

				Mobile oldFocus = m_Focus;

				if ( oldFocus != value )
				{
					m_Focus = value;

                    OnFocusChanged(oldFocus, m_Focus);

					if ( value != null )
						AggressiveAction( value );

					Combatant = value;

                    if (oldFocus != null && !oldFocus.Alive)
                    {
                  
                        Say("Thou hast suffered thy punishment, scoundrel.");

                  
                    }


					//if ( value != null )
					//	Say( 500131 ); // Thou wilt regret thine actions, swine!

					if ( m_AttackTimer != null )
					{
						m_AttackTimer.Stop();
						m_AttackTimer = null;
					}

					if ( m_IdleTimer != null )
					{
						m_IdleTimer.Stop();
						m_IdleTimer = null;
					}

					if ( m_Focus != null )
					{
						m_AttackTimer = new AttackTimer( this );
						m_AttackTimer.Start();
						((AttackTimer)m_AttackTimer).DoOnTick();
					}
                    else if (Disappears)
					{
						m_IdleTimer = new IdleTimer( this );
						m_IdleTimer.Start();
					}
				}
                else if (m_Focus == null && m_IdleTimer == null && Disappears)
				{
					m_IdleTimer = new IdleTimer( this );
					m_IdleTimer.Start();
				}
			}
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_Focus );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Focus = reader.ReadMobile();

                    if (m_Focus != null)
                    {
                        m_AttackTimer = new AttackTimer(this);
                        m_AttackTimer.Start();
                    }
                    else if (Disappears)
                    {
                        m_IdleTimer = new IdleTimer(this);
                        m_IdleTimer.Start();
                    }

				    break;
				}
			}
		}

		public override void OnAfterDelete()
		{
			if ( m_AttackTimer != null )
			{
				m_AttackTimer.Stop();
				m_AttackTimer = null;
			}

			if ( m_IdleTimer != null )
			{
				m_IdleTimer.Stop();
				m_IdleTimer = null;
			}

			base.OnAfterDelete();
		}

		private class AvengeTimer : Timer
		{
			private readonly Mobile m_Focus;

			public AvengeTimer( Mobile focus ) : base( TimeSpan.FromSeconds( 1.00 ), TimeSpan.FromSeconds( 1.00 ), 3 )
			{
				m_Focus = focus;
			}

			protected override void OnTick()
			{
				Spawn( m_Focus, m_Focus, 1, true );
			}
		}

		private class AttackTimer : Timer
		{
			private readonly WarriorGuard m_Owner;

			public AttackTimer( WarriorGuard owner ) : base( TimeSpan.FromSeconds( 1.00 ), TimeSpan.FromSeconds( 1.00 ) )
			{
				m_Owner = owner;
			}

			public void DoOnTick()
			{
				OnTick();
			}

            protected override void OnTick()
            {
                if (m_Owner.Deleted)
                {
                    Stop();
                    return;
                }

                m_Owner.Criminal = false;
                m_Owner.Kills = 0;
                m_Owner.Stam = m_Owner.StamMax;

                Mobile target = m_Owner.Focus;

                if ((target != null && (target.Deleted || !target.Alive || !m_Owner.CanBeHarmful(target) || target.Map == Map.Internal)))
                {
                    foreach (Mobile m in m_Owner.GetMobilesInRange(15))
                    {
                        if (Misc.NotorietyHandlers.IsGuardCandidate(m))
                        {
                            Region region = (Region.Find(m.Location, m.Map));
                            GuardedRegion guardedRegion = (GuardedRegion)region.GetRegion(typeof(GuardedRegion));

                            //Player is in guarded
                            if (guardedRegion != null && !guardedRegion.Disabled)
                            {
                                target = m;
                                m_Owner.Focus = m;
                                break;
                            }
                        }
                    }
                }

                if (target != null && (target.Deleted || !target.Alive || !m_Owner.CanBeHarmful(target) || target.Map == Map.Internal))
                {
                    m_Owner.Focus = null;
                    Stop();
                    return;
                }
                else if (m_Owner.Weapon is Fists)
                {
                    m_Owner.Kill();
                    Stop();
                    return;
                }

                if (target != null && m_Owner.Combatant != target)
                    m_Owner.Combatant = target;

                if (target == null)
                {
                    m_Owner.Focus = null;
                    Stop();
                    return;
                }

                if (target.Hidden)
                {
                    target.RevealingAction();
                    target.SendLocalizedMessage(500814); // You have been revealed!
                }

                Region from = (Region.Find(target.Location, target.Map));
                GuardedRegion reg = (GuardedRegion) from.GetRegion(typeof (GuardedRegion));

                //Player is in unguarded
                if (reg == null || reg.Disabled)
                {
                    //Player is still within range, follow on foot
                    if (m_Owner.InRange(target, 30))
                        m_Owner.Move(m_Owner.GetDirectionTo(target));

                    //Player has gone away, remove guard
                    else
                    {
                        m_Owner.Focus = null;
                        Stop();
                        return;
                    }
                }
                //Player is in guarded, teleport
                else if (!m_Owner.InRange(target, 1))
                    TeleportTo(target);
            }

		    private void TeleportTo( Mobile target )
			{
		        Spells.SpellHelper.Turn(m_Owner, target);
				Point3D from = m_Owner.Location;
				Point3D to = target.Location;

		        m_Owner.Location = to;

				Effects.SendLocationParticles( EffectItem.Create( from, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				Effects.SendLocationParticles( EffectItem.Create(   to, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

				m_Owner.PlaySound( 0x1FE );
			}
		}

		private class IdleTimer : Timer
		{
			private readonly WarriorGuard m_Owner;
			private int m_Stage;

			public IdleTimer( WarriorGuard owner ) : base( TimeSpan.FromSeconds( 1.00 ), TimeSpan.FromSeconds( 1.00 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted || !m_Owner.Disappears)
				{
					Stop();
					return;
				}

				if ( (m_Stage++ % 4) == 0 || !m_Owner.Move( m_Owner.Direction ) )
					m_Owner.Direction = (Direction)Utility.Random( 8 );

                if (m_Stage > 2)
                {
                    Effects.SendLocationParticles( EffectItem.Create(m_Owner.Location, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    m_Owner.PlaySound(0x1FE);

                    m_Owner.Delete();
                }
			}
		}
	}
}