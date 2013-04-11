/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/


using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Games.PaintBall
{
	public class PBGrenade : Item
	{
		

		

		private static bool LeveledExplosion = true; // Should explosion potions explode other nearby potions?
		private static bool InstantExplosion = false; // Should explosion potions explode on impact?
		private const int   ExplosionRange   = 2;     // How long is the blast radius?
		
		public PBGameItem m_PBGI;

			
		public PBGrenade(int hue, PBGameItem pbgi ) : base( 0x0E2A  )
		{
			Hue = hue;
			Name = "Paintball Grenade";
			m_PBGI = pbgi;
			//3839
		}

		public PBGrenade( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public virtual object FindParent( Mobile from )
		{
			Mobile m = this.HeldBy;

			if ( m != null && m.Holding == this )
				return m;

			object obj = this.RootParent;

			if ( obj != null )
				return obj;

			if ( Map == Map.Internal )
				return from;

			return this;
		}

		private Timer m_Timer;

		private ArrayList m_Users;

		public override void OnDoubleClick( Mobile from )
		{
		
			ThrowTarget targ = from.Target as ThrowTarget;

			if ( m_Users == null )
				m_Users = new ArrayList();

			if ( !m_Users.Contains( from ) )
				m_Users.Add( from );

			from.Target = new ThrowTarget( this );

			if ( m_Timer == null )
			{
				from.SendLocalizedMessage( 500236 ); // You should throw it now!
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), TimeSpan.FromSeconds( 1.0 ), 4, new TimerStateCallback( Detonate_OnTick ), new object[]{ from, 3 } );
			}
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

				if ( parent is Item )
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if ( parent is Mobile )
				{
					Mobile m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode( from, true, loc, map );
			}
			else
			{
				if ( parent is Item )
					((Item)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );
				else if ( parent is Mobile )
					((Mobile)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted )
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
			private PBGrenade m_Nade;

			public PBGrenade Nade
			{
				get{ return m_Nade; }
			}

			public ThrowTarget( PBGrenade nade ) : base( 12, true, TargetFlags.None )
			{
				m_Nade = nade;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Nade.Deleted || m_Nade.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

				SpellHelper.GetSurfaceTop( ref p );

				from.RevealingAction();

				IEntity to;

				if ( p is Mobile )
					to = (Mobile)p;
				else
					to = new Entity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Nade.ItemID & 0x3FFF, 7, 0, false, false, m_Nade.Hue, 0 );

				if( m_Nade.Amount > 1 )
				{
					Mobile.LiftItemDupe( m_Nade, 1 );
				}

				m_Nade.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Nade.Reposition_OnTick ), new object[]{ from, p, map } );
			}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			PBGrenade nade = this;
			if ( Deleted )
				return;

			Consume();

			for ( int i = 0; m_Users != null && i < m_Users.Count; ++i )
			{
				Mobile m = (Mobile)m_Users[i];
				ThrowTarget targ = m.Target as ThrowTarget;

				if ( targ != null && targ.Nade == this )
					Target.Cancel( m );
			}

			if ( map == null )
				return;

			Effects.PlaySound( loc, map, 0x207 );
			Effects.SendLocationEffect( loc, map, 0x36BD, 35, this.Hue, 0 );

			int alchemyBonus = 0;

			if ( direct )
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
				else if ( o is PBGrenade && o != this )
				{
					toExplode.Add( o );
				}
			}

			eable.Free();

		

			for ( int i = 0; i < toExplode.Count; ++i )
			{
				object o = toExplode[i];

				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					if ( from == null || (SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false )) )
					{
						if ( from != null )
							//from.DoHarmful( m );
							
							if ( m_PBGI.Players.Contains(m) || m_PBGI.NpcPlayers.Contains(m) )
							{
							DoDamage( nade, from, m );
							}
							else
							{
							from.SendMessage( "You cannot attack someone that is not playing!" );
				            m.SendMessage( "You are not playing Paintball, please leave the area." );
							}

					

						/*

						if ( !Core.AOS && damage > 40 )
							damage = 40;
						else if ( Core.AOS && toDamage > 2 )
							damage /= toDamage - 1;

						AOS.Damage( m, from, damage, 0, 100, 0, 0, 0 ); */
					}
				}
				else if ( o is PBGrenade )
				{
					PBGrenade pot = (PBGrenade)o;

					pot.Explode( from, false, pot.GetWorldLocation(), pot.Map );
				}
				else
				{
					DoDamage( nade, from );
				}
			}
		}
		public void DoDamage(PBGrenade nade, Mobile attacker, Mobile defender)
		{
			
			
			if( defender.FindItemOnLayer( Layer.Cloak ) == null )
			{
				if (m_PBGI != null )
				{
				m_PBGI.KillPlayer( defender );
				defender.SendMessage( "You are not on a team!  Removing you from the game." );
				}
				defender.SendMessage( "Error: PBGI returns null. You are not in the game, Please Rejoin or contact a GM." );
				
			}
			else
			{
			PBArmor armor;
			//armor = defender.NeckArmor as PBArmor;

			int defenderTeam = defender.FindItemOnLayer( Layer.Cloak ).Hue;
			int attackerTeam = attacker.FindItemOnLayer( Layer.Cloak ).Hue;
			
			if ( attackerTeam == defenderTeam )
				{
				attacker.SendMessage( "You are sprayed with paint from you`re own grenade." );
			    }
			else
				{
			
for ( int i = 0; i < m_PBGI.NadeDamage ; ++i )
		{
			if( defender.NeckArmor != null && defender.NeckArmor.Hue == defenderTeam )
			{
				armor = defender.NeckArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else if( defender.HandArmor != null && defender.HandArmor.Hue == defenderTeam )
			{
				armor = defender.HandArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else if( defender.ArmsArmor != null && defender.ArmsArmor.Hue == defenderTeam )
			{
				armor = defender.ArmsArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else if( defender.HeadArmor != null && defender.HeadArmor.Hue == defenderTeam )
			{
				armor = defender.HeadArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else if( defender.LegsArmor != null && defender.LegsArmor.Hue == defenderTeam )
			{
				armor = defender.LegsArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else if( defender.ChestArmor != null && defender.ChestArmor.Hue == defenderTeam )
			{
				armor = defender.ChestArmor as PBArmor;
				armor.WasHit( this, 0 );
			}
			else
			{
				attacker.SendMessage( "You have just removed that player from the game!" );
				m_PBGI.KillPlayer( defender );
			
			}


			//armor.WasHit( this, 0 );
		
			//armor.NadeHit( nade, 0 );
			}
		}
	}
}
		public void DoDamage(PBGrenade nade, Mobile defender)
		{
			PBArmor armor;
			armor = defender.NeckArmor as PBArmor;
			
			if( defender.FindItemOnLayer( Layer.Cloak ) == null )
			{
				if (m_PBGI != null )
				{
				m_PBGI.KillPlayer( defender );
				defender.SendMessage( "You are not on a team!  Removing you from the game." );
				}
				defender.SendMessage( "Error: PBGI returns null. You are not in the game, Please Rejoin or contact a GM." );
				
			}

			int defenderTeam = defender.FindItemOnLayer( Layer.Cloak ).Hue;
			
for ( int i = 0; i < m_PBGI.NadeDamage ; ++i )
			{
			if( defender.NeckArmor != null && defender.NeckArmor.Hue == defenderTeam )
				armor = defender.NeckArmor as PBArmor;
			else if( defender.HandArmor != null && defender.HandArmor.Hue == defenderTeam )
				armor = defender.HandArmor as PBArmor;
			else if( defender.ArmsArmor != null && defender.ArmsArmor.Hue == defenderTeam )
				armor = defender.ArmsArmor as PBArmor;
			else if( defender.HeadArmor != null && defender.HeadArmor.Hue == defenderTeam )
				armor = defender.HeadArmor as PBArmor;
			else if( defender.LegsArmor != null && defender.LegsArmor.Hue == defenderTeam )
				armor = defender.LegsArmor as PBArmor;
			else if( defender.ChestArmor != null && defender.ChestArmor.Hue == defenderTeam )
				armor = defender.ChestArmor as PBArmor;
			else
			{
				
				m_PBGI.KillPlayer( defender );
			
			}


			armor.WasHit( this, 0 );
		
			//armor.NadeHit( nade, 0 );
}
		}
	}
}
