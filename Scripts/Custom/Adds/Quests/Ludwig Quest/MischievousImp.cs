using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an imp corpse" )]
	public class MischievousImp : BaseCreature
	{
		[Constructable]
		public MischievousImp() : base( AIType.AI_SphereMage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
            Name = "Mischievous Imp";
			Body = 74; 
			Hue = 2911; 
			BaseSoundID = 422; 

			SetStr( 422, 511 );
			SetDex( 155, 188 );
			SetInt( 600, 610 );

			SetHits( 345, 379 );
			SetMana( 500 );

			SetDamage( 10, 12 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );

            SetSkill(SkillName.EvalInt, 99.1, 109.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 150.5, 180.0);
            SetSkill(SkillName.Tactics, 90.1, 99.0);
            SetSkill(SkillName.Wrestling, 90.1, 99.0);

			Fame = 5300;
			Karma = -5300;

			VirtualArmor = 55;		
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            if (Utility.RandomDouble() <= 0.40)
                PackItem(new SheetMusic() );
        }

		public override Poison PoisonImmune { get { return Poison.Lesser; } }

        public MischievousImp(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
