using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wood elf corpse" )]
	public class WoodElfEliteWarrior : BaseCreature
	{
		[Constructable]
		public WoodElfEliteWarrior() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			Hue = 0x597;

			SetStr( 60, 80 );
			SetDex( 81, 95 );
			SetInt( 70, 100 );

			SetHits( 80, 100 );
			SetStam( 81, 95 );

			SetDamage( 10, 30 );

			SetSkill( SkillName.Swords, 60.0, 100.0 ); //we dont need to give him uberskillz
			SetSkill( SkillName.Tactics, 50.0, 100.0 );
			SetSkill( SkillName.MagicResist, 15.0, 38.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = 0;
			Karma = Utility.RandomMinMax( -2500, -5000 );

			VirtualArmor = 18;

			//AddItem(Server.Items.Hair.GetRandomHair(Female));
			AddItem( new Sandals( Utility.RandomNeutralHue() ) );
			AddItem( new VikingSword() );
			AddItem( new WoodenShield() );
			AddItem( new Goatee( Serial.NewItem ) );
			AddItem( new MetalKiteShield() );
			StuddedChest che = new StuddedChest();
			che.Hue = 0x045e;

			StuddedGloves glo = new StuddedGloves();
			glo.Hue = 0x045e;

			StuddedLegs leg = new StuddedLegs();
			leg.Hue = 0x0599;

			StuddedArms arm = new StuddedArms();
			arm.Hue = 0x0599;

			StuddedGorget gor = new StuddedGorget();
			gor.Hue = 0x045e;

			BodySash sas = new BodySash();
			sas.Hue = 0x0599;

			Cloak cape = new Cloak();
			cape.Hue = 0x045e;

			AddItem( cape );
			AddItem( gor );
			AddItem( sas );
			AddItem( arm );
			AddItem( glo );
			AddItem( leg );
			AddItem( che );
		}

		public WoodElfEliteWarrior( Serial serial ) : base( serial )
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