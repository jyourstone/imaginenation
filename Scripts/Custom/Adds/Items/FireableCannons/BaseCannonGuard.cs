using System;
using Server.Items;

namespace Server.Mobiles
{
	public class BaseCannonGuard : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }
		private BaseCannon m_Cannon;
		private DateTime NextFire, NextThink;
		
		public BaseCannon Cannon{ get{ return m_Cannon;} set{ m_Cannon = value; } }
		
		public BaseCannonGuard() : base( AIType.AI_Archer, FightMode.Evil, 15, 1, 0.2, 0.4 )
		{
			Title = "the Cannoneer";
			this.CantWalk = true;
		}
				
		public override void OnCombatantChange()
		{
			if( Combatant != null )
			{
				FireCannon(Combatant);
			}
		}
		
		public override void OnDelete()
		{
			if( m_Cannon != null )
				m_Cannon.Delete();
			base.OnDelete();
		}
		
		public override void OnDeath(Container c)
		{
			m_Cannon.Delete();
			base.OnDeath(c);
		}
		
		public override void OnThink()
		{
			base.OnThink();
			if( NextThink > DateTime.Now )
				return;
			if( m_Cannon == null )
			{
				m_Cannon = new CannonNorth(this);
				Direction = Direction.North;
				Cannon.MoveToWorld( new Point3D(X,Y - 3,Z), Map);
			}
			
			if( m_Cannon.Deleted )
				Delete();
			
			if( Combatant == null )
				return;
			
			NextThink = DateTime.Now + TimeSpan.FromSeconds(2.5);
			FireCannon(Combatant);
		}
		
		public virtual void FireCannon(Mobile target)
		{
			if( NextFire > DateTime.Now )
				return;
			NextFire = DateTime.Now + TimeSpan.FromSeconds(10);
			m_Cannon.CCom.CheckFiringAngle(target.Location,this);
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}
		
		public BaseCannonGuard( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			writer.Write( m_Cannon );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			m_Cannon = (BaseCannon)reader.ReadItem();
		}
	}
}
