using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class SkeletalArcher : BaseCreature
	{
		[Constructable]
		public SkeletalArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Skeletal Archer";
			Body = Utility.RandomList( 50, 56 );
			BaseSoundID = 0x48D;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 16, 40 );

			SetHits( 34, 48 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Archery, 45.1, 55.0 );

			AddItem( new Arrow( 50 ) );
			AddItem( new Bow() );

			Fame = 450;

			VirtualArmor = 16;
		}

		public SkeletalArcher( Serial serial ) : base( serial )
		{
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Lesser; }
		}

		public override void GenerateLoot()
		{
            PackGold(100, 200);
			AddLoot( LootPack.Poor );
            AddItem(new Bone(Utility.RandomMinMax(1, 3)));
            PackGold(250);
            if (Utility.RandomDouble() <= 0.15)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(1, 2)));
			switch( Utility.Random( 5 ) )
			{
				case 0:
					PackItem( new BoneArms() );
					break;
				case 1:
					PackItem( new BoneChest() );
					break;
				case 2:
					PackItem( new BoneGloves() );
					break;
				case 3:
					PackItem( new BoneLegs() );
					break;
				case 4:
					PackItem( new BoneHelm() );
					break;
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