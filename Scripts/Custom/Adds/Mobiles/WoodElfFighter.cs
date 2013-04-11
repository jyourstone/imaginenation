using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wood elf corpse" )]
	public class WoodElfFighter : BaseCreature
	{
		[Constructable]
		public WoodElfFighter() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			Hue = 0x597;

			SetStr( 45, 65 ); //I've set him up with normal stats since we've defined his Hits and his weapons speed elsewhere.
			SetDex( 55, 70 );
			SetInt( 70, 100 );

			SetHits( 80, 100 ); // here are his hits according to the spherescript. random between 250 and 350
			SetStam( 81, 95 );

			SetDamage( 5, 10 ); //i set his damage to be low since he hits like 3 times a second. This will probably need to be tweaked.

			SetSkill( SkillName.Swords, 60.0, 80.0 ); //we dont need to give him uberskillz
			SetSkill( SkillName.Tactics, 50.0, 80.0 );
			SetSkill( SkillName.MagicResist, 15.0, 38.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = 0;
			Karma = Utility.RandomMinMax( -2500, -4000 );

			VirtualArmor = 6; //this might need to be tweaked

			//AddItem( Server.Items.Hair.GetRandomHair( Female ) );
			AddItem( new Sandals( Utility.RandomNeutralHue() ) );
			AddItem( new Longsword() );
			AddItem( new WoodenShield() );
			StuddedChest che = new StuddedChest();
			che.Hue = 0x0599;
			StuddedGloves glo = new StuddedGloves();
			glo.Hue = 0x0599;
			StuddedLegs leg = new StuddedLegs();
			leg.Hue = 0x0599;
			AddItem( glo );
			AddItem( leg );
			AddItem( che );
		}

		public WoodElfFighter( Serial serial ) : base( serial )
		{
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		//Do we want him immune to poison?

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
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
	}
}