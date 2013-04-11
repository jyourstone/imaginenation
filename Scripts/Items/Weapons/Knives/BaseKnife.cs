using Server.Targets;

namespace Server.Items
{
	public abstract class BaseKnife : BaseMeleeWeapon
	{
      		public override int DefHitSound { get { return Utility.RandomList(0x23B, 0x23C); }}
		public override int DefMissSound {get { return Utility.RandomList(0x238, 0x239, 0x23A); }}

		public override SkillName DefSkill{ get{ return SkillName.Swords; } }
		public override WeaponType DefType{ get{ return WeaponType.Slashing; } }
		//public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

        	public override int GetSwingAnim(Mobile from)
	        {
        	    if (from.Mounted)
                	return 26;
	            else
        	        return Utility.RandomList(9, 10);
	        }

		public BaseKnife( int itemID ) : base( itemID )
		{
		}

		public BaseKnife( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
            if (!Sphere.CanUse(from, this))
                return;

            base.OnDoubleClick(from);

			from.SendLocalizedMessage( 1010018 ); // What do you want to use this item on?

			from.Target = new BladedItemTarget( this );
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );

			if ( !Core.AOS && Poison != null && PoisonCharges > 0 )
			{
				--PoisonCharges;

				if ( Utility.RandomDouble() >= 0.5 ) // 50% chance to poison
					defender.ApplyPoison( attacker, Poison );
			}
		}
	}
}