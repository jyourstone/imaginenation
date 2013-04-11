using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a daemon knight corpse" )]
	public class DaemonKnight : BaseCreature
	{
		[Constructable]
		public DaemonKnight() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "Daemon" );
			Title = "the Daemon Knight";

			Body = 0x0009;
			BaseSoundID = 357;
			Hue = 0x0493;
			SetStr( 475, 505 );
			SetDex( 76, 95 );
			SetInt( 300, 325 );

			SetHits( 475, 505 );
			SetStam( 76, 95 );
			SetMana( 300, 325 );

			SetDamage( 33, 37 );

			SetSkill( SkillName.Swords, 65.0, 75.0 );
			SetSkill( SkillName.Parry, 65.0, 75.0 );
			SetSkill( SkillName.Magery, 70.0, 85.0 );
			SetSkill( SkillName.MagicResist, 70.0, 80.0 );
			SetSkill( SkillName.Tactics, 70.0, 80.0 );
			SetSkill( SkillName.Wrestling, 60.0, 80.0 );

			Fame = Utility.RandomMinMax( 4000, 8000 );
			Karma = Utility.RandomMinMax( -5000, -6000 );

			VirtualArmor = Utility.RandomMinMax( 3, 20 );

			PackItem( new Longsword() );
		}

		public DaemonKnight( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return false; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Deadly; }
		}

		public override int TreasureMapLevel
		{
			get { return 5; }
		}

		public override int Meat
		{
			get { return 2; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich);
			AddLoot( LootPack.MedScrolls, 2 );
            if (Utility.RandomDouble() <= 0.1)
                AddLoot(LootPack.RandomWand, 1);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(3));
            PackGold(400, 500);
		}

        public override void OnCarve(Mobile from, Corpse corpse, Item item)
        {
            corpse.DropItem(new DaemonHeart());
            base.OnCarve(from, corpse, item);

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