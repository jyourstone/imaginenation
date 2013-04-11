using Server.Items;
namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class DestardGuardian : BaseCreature
	{
		[Constructable]
		public DestardGuardian () : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Destard Guardian";
			Body = 46;
			BaseSoundID = 362;
		    Hue = 2470;

			SetStr( 1096, 1185 );
			SetDex( 86, 175 );
			SetInt( 686, 775 );

			SetHits( 7950, 8100 );

			SetDamage( 50, 101 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 45 );

			SetResistance( ResistanceType.Physical, 85, 95 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 100.1, 150.0 );
			SetSkill( SkillName.Magery, 90.1, 150.0 );
			SetSkill( SkillName.Meditation, 102.5, 125.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 150.0 );
			SetSkill( SkillName.Wrestling, 97.6, 200.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.Gems, 6);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.HighScrolls, 3);
            AddLoot(LootPack.RandomWand, 1);
            PackGold(14000, 20000);
            AddItem(new RandomAccWeap(5));
        }

	    public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return false; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 19; } }
		public override int Scales{ get{ return 12; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Utility.RandomBool() ? Poison.Lesser : Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public DestardGuardian( Serial serial ) : base( serial )
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