using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sapphire dragon corpse" )]
	public class SapphireDragon : BaseCreature
	{
		[Constructable]
		public SapphireDragon() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Sapphire Dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;
			Hue = 0x07ab;
			SetStr( 1000, 1100 );
			SetDex( 96, 105 );
			SetInt( 436, 575 );

			SetHits( 678, 795 );

			SetDamage( 45, 60 );

			SetDamageType( ResistanceType.Physical, 100 );
            AddItem(new DragonsBlood(Utility.RandomMinMax(18, 25)));

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 95 );
			SetSkill( SkillName.AnimalTaming, 99.0 );
			SetSkill( SkillName.Parry, 75.0, 95.0 );

            Fame = 20000;
            Karma = -20000;

			VirtualArmor = 60;

			Tamable = false;
			ControlSlots = 3;
			MinTameSkill = 93.9;
		}

		public SapphireDragon( Serial serial ) : base( serial )
		{
		}

		public override bool HasBreath
		{
			get { return true; }
		}

		public override bool AutoDispel
		{
			get { return false; }
		}

		public override int TreasureMapLevel
		{
			get { return 4; }
		}

		public override int Meat
		{
			get { return 99; }
		}

		public override int Hides
		{
			get { return 20; }
		}

        public override HideType HideType
        {
            get
            {
                double roll = Utility.RandomDouble();

                if (roll <= 0.05)
                    return HideType.Barbed;
                if (roll <= 0.2)
                    return HideType.Horned;
                if (roll <= 0.5)
                    return HideType.Spined;

                return HideType.Regular;
            }
        }

		public override int Scales
		{
			get { return 0; }
		}

		public override ScaleType ScaleType
		{
			get { return ( Body == 12 ? ScaleType.Yellow : ScaleType.Red ); }
		}

		public override FoodType FavoriteFood
		{
			get { return FoodType.Meat; }
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 8);
            AddLoot(LootPack.HighScrolls, 2);
            PackGold(1500);
            if (Utility.RandomDouble() <= 0.4)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(3, 5)));
            if (Utility.RandomDouble() <= 0.10)
            {
                BaseArmor armor = Loot.RandomArmorOrShield();
                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(3, 5);
                AddItem(armor);
            }
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