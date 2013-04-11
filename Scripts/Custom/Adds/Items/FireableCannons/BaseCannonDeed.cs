/*
 * Created by SharpDevelop.
 * User: UOT
 * Date: 2/10/2005
 * Time: 11:49 AM
 * Version: 1.0.0
 */
using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0x14F0, 0x14EF )]
	public abstract class BaseCannonDeed : Item
	{
		public abstract int Hits{ get; set; }
		public abstract int HitsMax{ get; set; }
		
		public abstract BaseCannon FireCannon{ get; }
		
		public BaseCannonDeed() : base( 0x14F0 )
		{
			Weight = 0.1;
		}
		
		public BaseCannonDeed( Serial serial ) : base( serial )
		{
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060658, "#{0}\t{1}", "1049578", Hits ); // ~1_val~: ~2_val~
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			writer.Write( (int) Hits );
			writer.Write( (int) HitsMax );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			Hits = reader.ReadInt();
			HitsMax = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
				from.Target = new InternalTarget( this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}
		
		private class InternalTarget : Target
		{
			private BaseCannonDeed m_Deed;
			
			public InternalTarget( BaseCannonDeed deed ) : base( -1, true, TargetFlags.None )
			{
				m_Deed = deed;
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D p = targeted as IPoint3D;
				Map map = from.Map;
				
				if ( p == null || map == null || m_Deed.Deleted )
					return;
				if ( targeted is Container )
				{
					from.SendLocalizedMessage( 501978 ); // The weight is too great to combine in a container.
					return;
				}
				if ( targeted is Item )
				{
					if( ((Item)targeted).Parent != null )
					{
						from.SendLocalizedMessage( 501978 ); // The weight is too great to combine in a container.
						return;
					}
				}
				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					BaseCannon cannon = m_Deed.FireCannon;
					cannon.Owner = from;
					cannon.CCom.HitsMax = m_Deed.HitsMax;
					cannon.CCom.Hits = m_Deed.Hits;
					m_Deed.Delete();
					cannon.MoveToWorld(new Point3D(p),map);
				}
				else
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
			}
		}
	}
}
