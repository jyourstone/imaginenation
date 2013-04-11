using System.Collections.Generic;
namespace Server.Mobiles
{
	[CorpseName( "a smoldering corpse" )]
	public class SpiritMinion : BaseCreature
	{
        private static readonly string[] m_Names = new string[]
			{
				"Fire Sprite",
				"Fire Fly"
			};
		public override bool InitialInnocent{ get{ return true; } }
        
		[Constructable]
		public SpiritMinion() : base( AIType.AI_SphereMage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
            Name = m_Names[Utility.Random(m_Names.Length)];
			Body = 128;
			BaseSoundID = 0x467;
		    Hue = 2519;

			SetStr( 90, 110 );
			SetDex( 301, 400 );
			SetInt( 201, 250 );

			SetHits( 75, 100 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.1, 70.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 80.1, 90.5 );

			Fame = 2000;

			VirtualArmor = 60;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

        public override HideType HideType { get { return HideType.Regular; } }

		public override int Hides{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public SpiritMinion( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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