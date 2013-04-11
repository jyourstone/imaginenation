using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Leather Armored Minotaur corpse" )]
	
	public class LeatherArmoredMinotaur : BaseCreature
	{
		[Constructable]
		public LeatherArmoredMinotaur() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Leather Armored Minotaur";
			Body = 281;
			BaseSoundID = 427;

			SetStr( 1267, 1545 );
			SetDex( 366, 475 );
			SetInt( 246, 270 );

			SetHits( 1976, 2352 );

			SetDamage( 50, 75 );

			SetDamageType( ResistanceType.Physical, 100 );			

			SetResistance( ResistanceType.Physical, 75, 85 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );

			SetSkill( SkillName.Macing, 145.1, 170.0 );
			SetSkill( SkillName.MagicResist, 145.1, 170.0 );
			SetSkill( SkillName.Tactics, 145.1, 170.0 );
			SetSkill( SkillName.Wrestling, 145.1, 170.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 75;

			PackItem( new Club() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public LeatherArmoredMinotaur( Serial serial ) : base( serial )
		{
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