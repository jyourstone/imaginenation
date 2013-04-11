using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class GrimReaper : BaseCreature
	{
		[Constructable]
		public GrimReaper() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Grim Reaper";
			Body = 0x03ca;
			BaseSoundID = 0x482;

			SetStr( 70, 100 );
			SetDex( 50, 70 );
			SetInt( 40, 70 );

			SetHits( 40, 60 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.Parry, 60.0, 85.0 );
			SetSkill( SkillName.MagicResist, 60.0, 85.0 );
			SetSkill( SkillName.Tactics, 60.0, 85.0 );
			SetSkill( SkillName.Wrestling, 60.0, 85.0 );

			Fame = Utility.RandomMinMax( 1000, 2000 );
			Karma = Utility.RandomMinMax( -5000, -3000 );

			VirtualArmor = 11;

			//PackGold( 20, 90 );
		}

		public GrimReaper( Serial serial ) : base( serial )
		{
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
			AddLoot( LootPack.Poor );
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