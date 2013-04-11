//revised
using Server.Items;
namespace Server.Mobiles
{
	[CorpseName( "a slimy corpse" )]
	public class PoisonousSlime : BaseCreature
	{
		[Constructable]
		public PoisonousSlime () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Poisonous Slime";
			Body = 51;
			BaseSoundID = 0;
            Hue = 557;

			SetStr( 100 );
			SetDex( 65 );
			SetInt( 10 );

			SetHits( 182 );

			SetDamage( 6, 18 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 100 );

			SetResistance( ResistanceType.Physical, 17, 20 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 10, 23 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 85.0 );

			VirtualArmor = 40;
		}

        public override void GenerateLoot()
        {
            PackGold(1000, 1500);
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, 6);
            if (Utility.RandomDouble() <= 0.40)
                PackItem(Engines.Plants.Seed.RandomBonsaiSeed());
            if (Utility.RandomDouble() <= 0.12)
                AddItem(new RandomAccWeap(Utility.RandomList(3)));
        }

        public override bool AutoDispel{ get{ return true; } }
        public override bool Unprovokable{ get{ return true; } }
        public override Poison PoisonImmune{ get{ return Poison.Regular; } }
        public override Poison HitPoison{ get{ return Poison.Lesser; } }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (!attacker.Poisoned)
            {
                int poison = Utility.RandomMinMax(0, 1);
                base.OnGotMeleeAttack(attacker);
                attacker.ApplyPoison(attacker, Poison.GetPoison(poison));
                attacker.SendMessage("You got poisoned when striking in the poisonous slimy body");
            }
        }
        

		public PoisonousSlime( Serial serial ) : base( serial )
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