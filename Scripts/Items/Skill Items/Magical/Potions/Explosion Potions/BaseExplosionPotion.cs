using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseExplosionPotion : BasePotion
	{
		public abstract int MinDamage { get; }
		public abstract int MaxDamage { get; }

		public override bool RequireFreeHand{ get{ return false; } }

		private static bool LeveledExplosion = true; // Should explosion potions explode other nearby potions?
		private static bool InstantExplosion = false; // Should explosion potions explode on impact?
        private static bool RelativeLocation = true; // Is the explosion target location relative for mobiles?
        private const int ExplosionRange = 2; // How long is the blast radius?

		public BaseExplosionPotion( PotionEffect effect ) : base( 0xF0D, effect )
		{
		}

		public BaseExplosionPotion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

        public override bool OnDragLift(Mobile from)
        {
            if (EventItem)
            {
                if (ParentEntity == null)
                    return false;
            }

            return base.OnDragLift(from);
        }

	    public virtual object FindParent( Mobile from )
		{
			Mobile m = HeldBy;

			if ( m != null && m.Holding == this )
				return m;

			object obj = RootParent;

			if ( obj != null )
				return obj;

			if ( Map == Map.Internal )
				return from;

			return this;
		}

		private Timer m_Timer;

		private ArrayList m_Users;

		public override void Drink( Mobile from )
		{
            if (Core.AOS && (from.Paralyzed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)))
            {
                from.SendLocalizedMessage(1062725); // You can not use a purple potion while paralyzed.
                return;
            }

            if (from.BeginAction(typeof(BaseExplosionPotion)))
            {
                from.EndAction(typeof(BaseExplosionPotion)); //Timer should start when targeting

                ThrowTarget targ = from.Target as ThrowTarget;
                Stackable = false;
                // Scavenged explosion potions won't stack with those ones in backpack, and still will explode.

                if (targ != null && targ.Potion == this)
                    return;

                from.RevealingAction();

                if (m_Users == null)
                    m_Users = new ArrayList();

                if (!m_Users.Contains(from))
                    m_Users.Add(from);

                from.Target = new ThrowTarget(this);

                if (m_Timer == null)
                {
                    from.SendLocalizedMessage(500236); // You should throw it now!
                    //if (Core.ML)
                    //    m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.25), 5, new TimerStateCallback(Detonate_OnTick), new object[] {from, 3}); // 3.6 seconds explosion delay
                    //else
                    m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(0.75), TimeSpan.FromSeconds(1.0), 4, new TimerStateCallback(Detonate_OnTick), new object[] {from, 3}); // 2.6 seconds explosion delay
                }
            }
            else
                from.SendAsciiMessage("You can't use another explosion potion yet!");
		}

        protected bool CanUseExplosionPotion(Mobile from)
        {
            if (from.BeginAction(typeof(BaseExplosionPotion)))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerStateCallback(ReleaseLock), from);
                return true;
            }

            from.SendAsciiMessage("You can't use another explosion potion yet!");
            return false;
        }

        protected new static void ReleaseLock(object state)
        {
            ((Mobile)state).EndAction(typeof(BaseExplosionPotion));
        }

		private void Detonate_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			int timer = (int)states[1];

			object parent = FindParent( from );

			if ( timer == 0 )
			{
				Point3D loc;
				Map map;
			    Mobile m = from;

				if ( parent is Item )
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if ( parent is Mobile )
				{                   
                    if (m_Users != null && m_Users[m_Users.Count - 1] is Mobile)
                        m = (Mobile) m_Users[m_Users.Count - 1];
                    else
                        m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode( m, true, loc, map );
                m_Timer = null;
			}
			else
			{
				if ( parent is Item )
					((Item)parent).PublicOverheadMessage( MessageType.Regular, 906, true, timer.ToString() );
				else if ( parent is Mobile )
					((Mobile)parent).PublicOverheadMessage( MessageType.Regular, 906, true, timer.ToString() );

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted || m_Timer == null )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			IPoint3D p = (IPoint3D)states[1];
			Map map = (Map)states[2];

			Point3D loc = new Point3D( p );

			if ( InstantExplosion )
				Explode( from, true, loc, map );
			else
				MoveToWorld( loc, map );
		}

		private class ThrowTarget : Target
		{
			private readonly BaseExplosionPotion m_Potion;

			public BaseExplosionPotion Potion
			{
				get{ return m_Potion; }
			}

			public ThrowTarget( BaseExplosionPotion potion ) : base( 12, true, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Potion.Deleted || m_Potion.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

                if (m_Potion.CanUseExplosionPotion(from))
                {
                    SpellHelper.GetSurfaceTop(ref p);

                    from.RevealingAction();

                    IEntity to = new Entity(Serial.Zero, new Point3D(p), map);

                    if (p is Mobile)
                    {
                        if (!RelativeLocation) // explosion location = current mob location. 
                            p = ((Mobile) p).Location;
                        else
                            to = (Mobile) p;
                    }

                    Effects.SendMovingEffect(from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0);

                    if (m_Potion.Amount > 1)
                    {
                        Mobile.LiftItemDupe(m_Potion, 1).EventItem = m_Potion.EventItem;
                    }

                    m_Potion.Internalize();
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_Potion.Reposition_OnTick), new object[] {from, p, map});
                }
			}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			if ( Deleted )
				return;

            if (from != null)
            {
                CustomRegion cR = from.Region as CustomRegion;
                CustomRegion cR2 = Region.Find(loc, map) as CustomRegion;

                if ((cR != null && !cR.Controller.CanUsePotExplosion) || (cR2 != null && !cR2.Controller.CanUsePotExplosion))
                    return;
            }
            else
            {
                CustomRegion cR = Region.Find(loc, map) as CustomRegion;

                if ((cR != null && !cR.Controller.CanUsePotExplosion))
                    return;
            }

		    if (!EventItem || (EventItem && EventItemConsume))
                Consume();
            else
            {
                Mobile m;
                if (m_Users != null && m_Users[0] is Mobile)
                    m = (Mobile)m_Users[0];
                else
                    m = from;

                if (m != null && RootParentEntity != m)
                    m.AddToBackpack(this);

                m_Timer = null;
            }

		    for ( int i = 0; m_Users != null && i < m_Users.Count; ++i )
			{
				Mobile m = (Mobile)m_Users[i];
				ThrowTarget targ = m.Target as ThrowTarget;

				if ( targ != null && targ.Potion == this )
					Target.Cancel( m );
			}

			if ( map == null )
				return;

			Effects.PlaySound( loc, map, 0x207 );
			Effects.SendLocationEffect( loc, map, 0x36BD, 20 );

			int alchemyBonus = 0;

            if (direct && from != null)
			    alchemyBonus = (int)(from.Skills.Alchemy.Value / (Core.AOS ? 5 : 10));

			IPooledEnumerable eable = LeveledExplosion ? map.GetObjectsInRange( loc, ExplosionRange ) : map.GetMobilesInRange( loc, ExplosionRange );
			ArrayList toExplode = new ArrayList();

			int toDamage = 0;

			foreach ( object o in eable )
			{
				if ( o is Mobile )
				{
					toExplode.Add( o );
					++toDamage;
				}
				else if ( o is BaseExplosionPotion && o != this )
				{
					toExplode.Add( o );
				}
			}

			eable.Free();

			int min = Scale( from, MinDamage );
			int max = Scale( from, MaxDamage );
		    int count = 1;

			for ( int i = 0; i < toExplode.Count; ++i )
			{
				object o = toExplode[i];

				if ( o is Mobile )
				{
				    Mobile m = (Mobile)o;
                    GuardedRegion reg = (GuardedRegion)Region.Find(m.Location, m.Map).GetRegion(typeof(GuardedRegion));

                    //Taran: Don't hurt mobiles in guarded
                    if (reg == null || reg.Disabled)
                    {
                        if (from != null && from.CanBeHarmful(m, false))
                        {
                            from.DoHarmful(m);

                            int damage = Utility.RandomMinMax(min, max);

                            damage += alchemyBonus;

                            #region Taran - Damage based on AR
                            if (m is PlayerMobile)
                            {
                                int armorRating = (int) ((PlayerMobile) m).BaseArmorRatingSpells;

                                damage = (int) (damage*1.5) - armorRating;

                                if (damage > 49)
                                    damage = 49;

                                if (damage < 15)
                                    damage = 15;
                            }
                            #endregion

                            /*
				        if (!Core.AOS && damage > 40)
				            damage = 40;
				        else if (Core.AOS && toDamage > 2)
				            damage /= toDamage - 1;
                        */

                            AOS.Damage(m, from, damage, 0, 100, 0, 0, 0);
                        }
                    }
				}
				else if ( o is BaseExplosionPotion && direct )
				{
                    if (count > 4) //Only explode 5 pots at most including the used one
                        break;

				    BaseExplosionPotion pot = (BaseExplosionPotion)o;
				    Mobile m;

                    if (pot.m_Users != null && pot.m_Users[0] is Mobile)
                        m = (Mobile)pot.m_Users[0];
                    else
                        m = from;

					pot.Explode( m, false, pot.GetWorldLocation(), pot.Map );

                    count++;
				}
			}
		}
	}
}