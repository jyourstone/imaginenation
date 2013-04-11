using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an stone golem corpse" )]
	public class StoneGolem : BaseCreature
	{
		[Constructable]
		public StoneGolem() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Stone Golem";
			Body = 14;
			BaseSoundID = 268;
			Hue = 0x0492;

			SetStr( 550, 750 );
			SetDex( 165, 185 );
			SetInt( 170, 195 );

			SetDamage( 20, 40 );

			SetSkill( SkillName.MagicResist, 85.0, 90.0 );
			SetSkill( SkillName.Tactics, 85.0, 100.0 );
			SetSkill( SkillName.Wrestling, 175.0, 200.0 );
			SetSkill( SkillName.Parry, 60.0, 90.0 );

			Fame = Utility.RandomMinMax( 1000, 6000 );

			VirtualArmor = 50;

			//PackGold(500, 700);
		}

		public StoneGolem( Serial serial ) : base( serial )
		{
		}

		public override double DispelDifficulty
		{
			get { return 117.5; }
		}

		public override double DispelFocus
		{
			get { return 45.0; }
		}

		public override int TreasureMapLevel
		{
			get { return 1; }
		}

		public override void GenerateLoot()
		{
			PackItem( new IronOre( 3 ) ); // TODO: Five small iron ore
			PackItem( new MandrakeRoot( 8 ) );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems );
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