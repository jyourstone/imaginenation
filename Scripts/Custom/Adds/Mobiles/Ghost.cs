using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class Ghost : BaseCreature
	{
		[Constructable]
		public Ghost() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Ghost";
			Body = 0x03ca;
		    Hue = -1;
			BaseSoundID = 0x482;

			SetStr( 100, 125 );
			SetDex( 90, 120 );
			SetInt( 60, 90 );

			SetHits( 100, 125 );

			SetDamage( 8, 16 );

			SetSkill( SkillName.Parry, 60.0, 85.0 );
			SetSkill( SkillName.MagicResist, 60.0, 85.0 );
			SetSkill( SkillName.Tactics, 60.0, 85.0 );
			SetSkill( SkillName.Wrestling, 60.0, 85.0 );

			Fame = Utility.RandomMinMax( 1500, 3000 );

			VirtualArmor = 21;
		}

		public Ghost( Serial serial ) : base( serial )
		{
		}

        public override void DisplayPaperdollTo(Mobile to)
        {
            return;
        }

		public override Poison PoisonImmune
		{
			get { return Poison.Lethal; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			PackGold( 20, 90 );
			PackItem( Loot.RandomWeapon() );
			PackItem( new Bone() );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}