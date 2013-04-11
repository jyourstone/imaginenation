using Server.Items;
using Server.Scripts.Custom.Adds.System.Loots;
using Server.Scripts.Custom.Adds.System.Loots.Containers;
using Server.Scripts.Custom.Adds.System.Loots.Modifications;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragon : BaseCreature
	{
		[Constructable]
		public SkeletalDragon () : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Skeletal Dragon";
			Body = 104;
			BaseSoundID = 0x488;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 558, 599 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.EvalInt, 80.1, 100.0 );
			SetSkill( SkillName.Magery, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;

            PackItem(Engines.Plants.Seed.RandomPeculiarSeed(4));
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		    AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 5 );
            PackGold(3000);
            AddItem(new Bone(Utility.RandomMinMax(15, 25)));
		}
        public static readonly LootPack MagicWeaponAndArmorDrop = new LootPack(new[]
                                                                    {
                                                                       new NewLootPackEntry(WeaponAndArmor, 80, new WeapMod(
                                                                       new Damage(100.0, new Attribute(4, 50), new Attribute(3, 50)),
                                                                       new Accuracy(50.0, new Attribute(4, 33), new Attribute(3, 33), new Attribute(2, 33)),
                                                                       new Durability(20.0, new Attribute(3, 33), new Attribute(4,33), new Attribute(5,33))),
                                                                       new ArMod( new Protection(90.0, new Attribute(3)),
                                                                       new Durability(100, new Attribute(3, 50), new Attribute(4, 50))))
                                                                    } );

	    private static readonly LootPackItem[] WeaponAndArmor = null;
	    public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 100; } }
		public override int BreathEffectHue{ get{ return 0x480; } }
		public override double BonusPetDamageScalar{ get{ return (Core.SE)? 3.0 : 1.0; } }
		// TODO: Undead summoning?

		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BleedImmune{ get{ return true; } }

		public SkeletalDragon( Serial serial ) : base( serial )
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