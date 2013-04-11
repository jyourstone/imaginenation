using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a water spirit corpse" )]
	public class WaterSpirit : BaseCreature
	{

		[Constructable]
		public WaterSpirit () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Water Spirit";
			Body = 16;
			BaseSoundID = 278;
            Hue = 2708;

			SetStr( 226, 255 );
			SetDex( 166, 185 );
			SetInt( 501, 525 );

			SetHits( 576, 593 );

			SetDamage( 9, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 10, 25 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.EvalInt, 60.1, 75.0 );
			SetSkill( SkillName.Magery, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 100.1, 115.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 50.1, 70.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
			ControlSlots = 3;
			CanSwim = true;

		}

		public override void GenerateLoot()
		{
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 2);
            if (Utility.RandomDouble() <= 0.5)
                PackItem(new MysticFishingNet());
		}

		public WaterSpirit( Serial serial ) : base( serial )
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