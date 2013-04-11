using Server.Items;

namespace Server.Mobiles
{
	public class BrigandArcher : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public BrigandArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = 0;
			Title = "the brigand archer";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

			SetStr( 150, 200 );
			SetDex( 175, 225 );
			SetInt( 75, 100 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.MagicResist, 65.0, 87.5 );
			SetSkill( SkillName.Archery, 85.0, 97.5 );
			SetSkill( SkillName.Tactics, 75.0, 97.5 );
			SetSkill( SkillName.Wrestling, 55.0, 77.5 );

			Fame = 1500;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());

			switch ( Utility.Random( 2 ))
			{
				case 0: AddItem( new Crossbow() ); break;
				case 1: AddItem( new HeavyCrossbow() ); break;
			}

            PackItem(new Bolt(Utility.RandomMinMax(50, 80)));

			Utility.AssignRandomHair( this );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public BrigandArcher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}