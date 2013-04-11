using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a glowing goblin corpse" )]
	public class GoblinMage : BaseCreature
	{
		[Constructable]
		public GoblinMage() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Goblin Mage";
			Body = 140;
			BaseSoundID = 0x45A;
			Hue = Utility.RandomMinMax( 1410, 1445 );
			SetStr( 110, 145 );
			SetDex( 85, 109 );
			SetInt( 156, 180 );

			SetHits( 50, 70 );

			SetDamage( 4, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 60.1, 72.5 );
			SetSkill( SkillName.Magery, 60.1, 72.5 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.Wrestling, 40.1, 50.0 );

			Fame = 3000;

			VirtualArmor = 27;

			PackReg( 3 );
			PackReg( 3 );

			if( 0.05 > Utility.RandomDouble() )
				PackItem( new OrcishKinMask() );
		}

		public GoblinMage( Serial serial ) : base( serial )
		{
		}

		public override InhumanSpeech SpeechType
		{
			get { return InhumanSpeech.Orc; }
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override int TreasureMapLevel
		{
			get { return 1; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.SavagesAndOrcs; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.LowScrolls );
            PackGold(100);
		}

		public override bool IsEnemy( Mobile m )
		{
			if( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
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

	[CorpseName( "a goblin corpse" )]
	public class GoblinLord : BaseCreature
	{
		[Constructable]
		public GoblinLord() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Goblin Lord";
			Body = 138;
			BaseSoundID = 0x45A;
			Hue = Utility.RandomMinMax( 1410, 1445 );
			SetStr( 142, 210 );
			SetDex( 86, 110 );
			SetInt( 76, 80 );

			SetHits( 90, 118 );

			SetDamage( 4, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 70.1, 85.0 );
			SetSkill( SkillName.Swords, 60.1, 85.0 );
			SetSkill( SkillName.Tactics, 75.1, 90.0 );
			SetSkill( SkillName.Wrestling, 60.1, 85.0 );

			Fame = 2500;
			Karma = -2500;

			switch( Utility.Random( 5 ) )
			{
				case 0:
					PackItem( new Lockpick() );
					break;
				case 1:
					PackItem( new MortarPestle() );
					break;
				case 2:
					PackItem( new Bottle() );
					break;
				case 3:
					PackItem( new RawRibs() );
					break;
				case 4:
					PackItem( new Shovel() );
					break;
			}

			PackItem( new RingmailChest() );

			if( 0.3 > Utility.RandomDouble() )
				PackItem( Loot.RandomPossibleReagent() );

			if( 0.2 > Utility.RandomDouble() )
				PackItem( new BolaBall() );
		}

		public GoblinLord( Serial serial ) : base( serial )
		{
		}

		public override InhumanSpeech SpeechType
		{
			get { return InhumanSpeech.Orc; }
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override int TreasureMapLevel
		{
			get { return 1; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.SavagesAndOrcs; }
		}

		public override void GenerateLoot()
		{
			//AddLoot( LootPack.Meager );
			AddLoot( LootPack.Average );
            PackGold(150);
			// TODO: evil orc helm
		}

		public override bool IsEnemy( Mobile m )
		{
			if( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
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

	[CorpseName( "a goblin corpse" )]
	public class GoblinCaptain : BaseCreature
	{
		[Constructable]
		public GoblinCaptain() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "orc" );
			Body = 7;
			BaseSoundID = 0x45A;
			Hue = Utility.RandomMinMax( 1410, 1445 );
			SetStr( 106, 140 );
			SetDex( 96, 130 );
			SetInt( 81, 105 );

			SetHits( 62, 82 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.MagicResist, 70.1, 85.0 );
			SetSkill( SkillName.Swords, 70.1, 95.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 28;

			// TODO: Skull?
			switch( Utility.Random( 7 ) )
			{
				case 0:
					PackItem( new Arrow() );
					break;
				case 1:
					PackItem( new Lockpick() );
					break;
				case 2:
					PackItem( new Shaft() );
					break;
				case 3:
					PackItem( new Ribs() );
					break;
				case 4:
					PackItem( new Bandage() );
					break;
				case 5:
					PackItem( new BeverageBottle( BeverageType.Wine ) );
					break;
				case 6:
					PackItem( new Jug( BeverageType.Cider ) );
					break;
			}

			if( Core.AOS )
				PackItem( Loot.RandomNecromancyReagent() );
		}

		public GoblinCaptain( Serial serial ) : base( serial )
		{
		}

		public override InhumanSpeech SpeechType
		{
			get { return InhumanSpeech.Orc; }
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.SavagesAndOrcs; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
            PackGold(150);
		}

		public override bool IsEnemy( Mobile m )
		{
			if( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
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

	[CorpseName( "a goblin corpse" )]
	public class Goblin : BaseCreature
	{
		[Constructable]
		public Goblin() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "orc" );
			Body = 17;
			BaseSoundID = 0x45A;
			Hue = Utility.RandomMinMax( 1410, 1445 );
			SetStr( 91, 115 );
			SetDex( 76, 100 );
			SetInt( 31, 55 );

			SetHits( 53, 68 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 50.1, 75.0 );
			SetSkill( SkillName.Tactics, 55.1, 80.0 );
			SetSkill( SkillName.Wrestling, 50.1, 70.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 20;

			switch( Utility.Random( 20 ) )
			{
				case 0:
					PackItem( new Scimitar() );
					break;
				case 1:
					PackItem( new Katana() );
					break;
				case 2:
					PackItem( new WarMace() );
					break;
				case 3:
					PackItem( new WarHammer() );
					break;
				case 4:
					PackItem( new Kryss() );
					break;
				case 5:
					PackItem( new Pitchfork() );
					break;
			}

			PackItem( new ThighBoots() );

			switch( Utility.Random( 3 ) )
			{
				case 0:
					PackItem( new Ribs() );
					break;
				case 1:
					PackItem( new Shaft() );
					break;
				case 2:
					PackItem( new Candle() );
					break;
			}

			if( 0.2 > Utility.RandomDouble() )
				PackItem( new BolaBall() );
		}

		public Goblin( Serial serial ) : base( serial )
		{
		}

		public override InhumanSpeech SpeechType
		{
			get { return InhumanSpeech.Orc; }
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override int TreasureMapLevel
		{
			get { return 1; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.SavagesAndOrcs; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool IsEnemy( Mobile m )
		{
			if( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
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