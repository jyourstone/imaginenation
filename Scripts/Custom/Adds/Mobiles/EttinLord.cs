using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "an ettin lord corpse" )]
	public class EttinLord : BaseCreature
	{
		[Constructable]
		public EttinLord() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Ettin Lord";
			Body = 18;
			BaseSoundID = 367;

			SetStr( 453, 590 );
			SetDex( 75, 100 );
			SetInt( 40, 65 );

			SetHits( 250, 300 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.MagicResist, 75.1, 100.0 );
			SetSkill( SkillName.Tactics, 70.1, 95.0 );
			SetSkill( SkillName.Wrestling, 70.1, 85.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;
		}

		public EttinLord( Serial serial ) : base( serial )
		{
		}

		public override Faction FactionAllegiance
		{
			get { return Minax.Instance; }
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override int TreasureMapLevel
		{
			get { return 4; }
		}

		public override int Meat
		{
			get { return 6; }
		}

		public override void GenerateLoot()
		{
			//AddLoot(LootPack.HighItems_Always, 1);
			AddLoot( LootPack.Rich );
            PackGold(100, 150);
			AddLoot( LootPack.Potions );
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