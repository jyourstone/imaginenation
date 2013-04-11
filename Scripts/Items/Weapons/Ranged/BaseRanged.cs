using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseRanged : BaseMeleeWeapon
	{
		public abstract int EffectID{ get; }
		public abstract Type AmmoType{ get; }
		public abstract Item Ammo{ get; }

      		public override int DefHitSound { get { return 0x234; }}
		public override int DefMissSound {get { return Utility.RandomList(0x238, 0x239, 0x23A); }}

		public override SkillName DefSkill{ get{ return SkillName.Archery; } }
		public override WeaponType DefType{ get{ return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		public override SkillName AccuracySkill{ get{ return SkillName.Tactics; } }

        private Timer m_RecoveryTimer; // so we don't start too many timers
        private bool m_Balanced;
        private int m_Velocity;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Balanced
        {
            get { return m_Balanced; }
            set { m_Balanced = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; InvalidateProperties(); }
        }

		public BaseRanged( int itemID ) : base( itemID )
		{
		}

		public BaseRanged( Serial serial ) : base( serial )
		{
		}

        public override TimeSpan OnSwing(Mobile attacker, Mobile defender)
        {
            WeaponAbility a = WeaponAbility.GetCurrentAbility(attacker);

            // If you move the swing delay will be reset
            if (DateTime.Now > (attacker.LastMoveTime + GetDelay(attacker)))
            {
                bool canSwing = true;
                Spells.SpellHelper.Turn(attacker, defender);

                if (canSwing && attacker.HarmfulCheck(defender))
                {
                    attacker.DisruptiveAction();
                    attacker.Send(new Swing(0, attacker, defender));

                    if (OnFired(attacker, defender))
                    {
                        if (CheckHit(attacker, defender, attacker as PlayerMobile))
                        {
                            if (attacker is PlayerMobile)
                            {
                                PlayerMobile pm = (PlayerMobile) attacker;

                                if (pm.LastSwingActionResult == PlayerMobile.SwingAction.Hit)
                                    pm.SwingCount++;
                                else
                                {
                                    pm.LastSwingActionResult = PlayerMobile.SwingAction.Hit;
                                    pm.SwingCount = 2;
                                }
                            }
                            OnHit(attacker, defender);
                        }
                        else
                        {
                            if (attacker is PlayerMobile)
                            {
                                PlayerMobile pm = (PlayerMobile) attacker;
                                if (pm.LastSwingActionResult == PlayerMobile.SwingAction.Miss)
                                    pm.SwingCount++;
                                else
                                {
                                    pm.LastSwingActionResult = PlayerMobile.SwingAction.Miss;
                                    pm.SwingCount = 2;
                                }
                            }
                            OnMiss(attacker, defender);
                        }
                    }
                    else
                        attacker.SendAsciiMessage("You are out of ammo");
                }

                attacker.RevealingAction();

                return GetDelay(attacker);
            }
            else
            {
                attacker.RevealingAction();

                return TimeSpan.FromSeconds(0.25);
            }
        }

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
            if (!EventItem)
			    if ( attacker.Player && !defender.Player && (defender.Body.IsAnimal || defender.Body.IsMonster) && 0.4 >= Utility.RandomDouble() )
				    defender.AddToBackpack( Ammo );

            if (Core.ML && m_Velocity > 0)
            {
                int bonus = (int)attacker.GetDistanceToSqrt(defender);

                if (bonus > 0 && m_Velocity > Utility.Random(100))
                {
                    AOS.Damage(defender, attacker, bonus * 3, 100, 0, 0, 0, 0);

                    if (attacker.Player)
                        attacker.SendLocalizedMessage(1072794); // Your arrow hits its mark with velocity!

                    if (defender.Player)
                        defender.SendLocalizedMessage(1072795); // You have been hit by an arrow with velocity!
                }
            }

			base.OnHit( attacker, defender, damageBonus );
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
            if (!EventItem)
            {
                if (attacker.Player && 0.4 >= Utility.RandomDouble())
                {
                    if (Core.SE)
                    {
                        PlayerMobile p = attacker as PlayerMobile;

                        if (p != null)
                        {
                            Type ammo = AmmoType;

                            if (p.RecoverableAmmo.ContainsKey(ammo))
                                p.RecoverableAmmo[ammo]++;
                            else
                                p.RecoverableAmmo.Add(ammo, 1);

                            if (!p.Warmode)
                            {
                                if (m_RecoveryTimer == null)
                                    m_RecoveryTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(p.RecoverAmmo));

                                if (!m_RecoveryTimer.Running)
                                    m_RecoveryTimer.Start();
                            }
                        }
                    }
                    else
                    {
                        Point3D loc = new Point3D(defender.X + Utility.RandomMinMax(-1, 1), defender.Y + Utility.RandomMinMax(-1, 1), defender.Z);
                        IPooledEnumerable eable = defender.Map.GetItemsInRange(loc, 0);
                        int ammoCount = 0;

                        foreach (Item i in eable)
                        {
                            if ((i is Arrow && Ammo is Arrow) || (i is Bolt && Ammo is Bolt))
                            {
                                i.Amount++;
                                ammoCount++;
                                break;
                            }
                        }

                        if (ammoCount < 1)
                            Ammo.MoveToWorld(loc, defender.Map);

                        eable.Free();
                    }
                }
            }
		    base.OnMiss( attacker, defender );
		}

		public virtual bool OnFired( Mobile attacker, Mobile defender )
		{
            BaseQuiver quiver = attacker.FindItemOnLayer(Layer.Cloak) as BaseQuiver;
			Container pack = attacker.Backpack;

            if (!EventItem || (EventItem && EventItemConsume))
            {
                if (attacker.Player)
                {
                    if (quiver == null || quiver.LowerAmmoCost == 0 || quiver.LowerAmmoCost > Utility.Random(100))
                    {
                        if (quiver != null && quiver.ConsumeTotal(AmmoType, 1))
                            quiver.InvalidateWeight();
                        else if (pack == null || !pack.ConsumeTotal(AmmoType, 1))
                            return false;
                    }
                }
            }

		    if (attacker.Mounted)
            {
                if (DefAnimation == WeaponAnimation.ShootBow)
                    attacker.Animate(27, 5, 1, true, false, 0);
                else if (DefAnimation == WeaponAnimation.ShootXBow)
                    attacker.Animate(28, 5, 1, true, false, 0);
            }
            else
            {
                if (DefAnimation == WeaponAnimation.ShootBow)
                    attacker.Animate(18, 5, 1, true, false, 0);
                else if (DefAnimation == WeaponAnimation.ShootXBow)
                    attacker.Animate(19, 5, 1, true, false, 0);
            }

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 3 ); // version

            writer.Write((bool)m_Balanced);
            writer.Write((int)m_Velocity);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 3:
                    {
                        m_Balanced = reader.ReadBool();
                        m_Velocity = reader.ReadInt();

                        goto case 2;
                    }
				case 2:
				case 1:
				{
					break;
				}
				case 0:
				{
					/*m_EffectID =*/ reader.ReadInt();
					break;
				}
			}

			if ( version < 2 )
			{
				WeaponAttributes.MageWeapon = 0;
				WeaponAttributes.UseBestSkill = 0;
			}
		}

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
        }
	}
}