using Server.Ethics;
using Server.Factions;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an algazar corpse" )]
	public class Algazar : BaseCreature
	{
		[Constructable]
		public Algazar() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Algazar";
			Body = 9;
			BaseSoundID = 357;
			Hue = 0x45;
			SetStr( 700 );
			SetDex( 100 );
			SetInt( 250 );

			SetHits( 300, 400 );
			SetStam( 100 );
			SetMana( 200 );

			SetDamage( 35 );

			SetSkill( SkillName.Parry, 75.0 );
			SetSkill( SkillName.Magery, 75.0 );
			SetSkill( SkillName.MagicResist, 75.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = Utility.RandomMinMax( 3, 18 );
		}

		public Algazar( Serial serial ) : base( serial )
		{
		}

		public override double DispelDifficulty
		{
			get { return 125.0; }
		}

		public override double DispelFocus
		{
			get { return 45.0; }
		}

		//public override Faction FactionAllegiance { get { return Shadowlords.Instance; } }

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Regular; }
		}

		public override int TreasureMapLevel
		{
			get { return 4; }
		}

		public override int Meat
		{
			get { return 9; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 2 );
            if (Utility.RandomDouble() <= 0.15)
            {
                BaseArmor armor = Loot.RandomArmorOrShield();
                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(2, 3);
                AddItem(armor);
            }
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

	[CorpseName( "an electric algazar corpse" )]
	public class ElectricAlgazar : BaseCreature
	{
		[Constructable]
		public ElectricAlgazar() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Electric Algazar";
			Body = 9;
            BaseSoundID = 357;
			Hue = 0x796;

			SetStr( 700 );
			SetDex( 100 );
			SetInt( 250 );

			SetHits( 700 );
			SetMana( 200, 300 );
			SetStam( 100, 300 );
			SetDamage( 38 );

			SetSkill( SkillName.Parry, 75.0 );
			SetSkill( SkillName.Magery, 80.0 );
			SetSkill( SkillName.MagicResist, 80.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = Utility.RandomMinMax( 20, 31 );
		}

		public ElectricAlgazar( Serial serial ) : base( serial )
		{
		}

		public override double DispelDifficulty
		{
			get { return 125.0; }
		}

		public override double DispelFocus
		{
			get { return 45.0; }
		}

		//public override Faction FactionAllegiance { get { return Shadowlords.Instance; } }

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Regular; }
		}

		public override int TreasureMapLevel
		{
			get { return 4; }
		}

		public override int Meat
		{
			get { return 9; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.HighScrolls, 1 );
            AddLoot(LootPack.MedScrolls, 1);
            PackGold(1200, 1800);
            if (Utility.RandomDouble() <= 0.3)
                AddLoot(LootPack.RandomWand, 1);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(4));
            if (Utility.RandomDouble() <= 0.3)
            {
                BaseArmor armor = Loot.RandomArmorOrShield();
                armor.ProtectionLevel = (ArmorProtectionLevel)4;
                AddItem(armor);
            }

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