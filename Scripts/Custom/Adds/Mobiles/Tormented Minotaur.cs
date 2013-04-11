using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a tormented minotaur corpse" )]
	
	public class TormentedMinotaur : BaseCreature
	{
		[Constructable]
		public TormentedMinotaur() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Tormented Minotaur";
			Body = 262;
			BaseSoundID = 427;

			SetStr( 767, 945 );
			SetDex( 166, 175 );
			SetInt( 146, 170 );

			SetHits( 676, 752 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Physical, 100 );			

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 120.1, 140.0 );
			SetSkill( SkillName.Wrestling, 120.1, 140.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			PackItem( new Club() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public TormentedMinotaur( Serial serial ) : base( serial )
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