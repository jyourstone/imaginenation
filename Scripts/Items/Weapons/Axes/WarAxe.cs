using Server.Engines.Harvest;

namespace Server.Items
{
	[Flipable( 0x13B0, 0x13AF )]
	public class WarAxe: BaseAxe
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override int AosSpeed{ get{ return 33; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 13; } }
		public override int OldMaxDamage{ get{ return 26; } }
		public override int OldSpeed{ get{ return 350; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		public override SkillName DefSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefType{ get{ return WeaponType.Bashing; } }
		//public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }

      		public override int DefHitSound { get { return 0x232; }}
		public override int DefMissSound {get { return Utility.RandomList(0x238, 0x239, 0x23A); }}

        	public override int GetSwingAnim(Mobile from)
	        {
        	    if (from.Mounted)
                	return 26;
	            else
        	        return Utility.RandomList(12, 13, 14);
	        }

		public override HarvestSystem HarvestSystem{ get{ return null; } }

		[Constructable]
		public WarAxe() : base( 0x13B0 )
		{
			Weight = 8.0;
			//Name = "war axe";
			//Layer = Layer.TwoHanded;
		}

		public WarAxe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
            {
                MinDamage = 13;
                MaxDamage = 26;
            }
		}
	}
}