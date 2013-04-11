using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a radioactive corpse" )]
	public class RadioactiveSlime : BaseCreature
	{
		[Constructable]
		public RadioactiveSlime() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Radioactive Slime";
			Body = 51;
            Hue = 1957;
			BaseSoundID = 456;

			SetStr( 400, 420 );
			SetDex( 140, 160 );
			SetInt( 100, 110 );

			SetHits( 415, 450 );

			SetDamage( 10, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.Poisoning, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 95.1, 100.0 );
			SetSkill( SkillName.Tactics, 109.3, 114.0 );
			SetSkill( SkillName.Wrestling, 109.3, 114.0 );

			Fame = 4000;
			Karma = -5000;

			VirtualArmor = 30;

            PackItem( new RadioactiveAcid( 1,2 ) );


		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public override bool BardImmune { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies | FoodType.GrainsAndHay | FoodType.Eggs; } }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(4))
            {
                default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.DoubleStrike;
                case 2: return WeaponAbility.ParalyzingBlow;
                case 3: return WeaponAbility.InfectiousStrike;
            }
        }

		public RadioactiveSlime( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
