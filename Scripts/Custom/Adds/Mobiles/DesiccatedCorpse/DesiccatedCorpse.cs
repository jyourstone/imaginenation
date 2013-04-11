using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a desiccated corpse" )]
	public class DesiccatedCorpse : BaseCreature
	{
		[Constructable]
		public DesiccatedCorpse() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Desiccated Corpse";
			Body = 3;
			BaseSoundID = 471;
			Hue = 1423;
			
			SetStr( 56, 91 );
			SetDex( 32, 55 );
			SetInt( 25, 45 );

			SetHits( 42, 60 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 25 );
			SetResistance( ResistanceType.Cold, 30, 45 );
			SetResistance( ResistanceType.Poison, 80, 100 );

			SetSkill( SkillName.MagicResist, 20.1, 45.0 );
			SetSkill( SkillName.Tactics, 38.1, 52.0 );
			SetSkill( SkillName.Wrestling, 42.1, 54.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 24;
			
			switch ( Utility.Random( 10 ))
			{
				case 0: PackItem( new LeftArm() ); break;
				case 1: PackItem( new RightArm() ); break;
				case 2: PackItem( new Torso() ); break;
				case 3: PackItem( new Bone() ); break;
				case 4: PackItem( new RibCage() ); break;
				case 5: PackItem( new RibCage() ); break;
				case 6: PackItem( new BonePile() ); break;
				case 7: PackItem( new BonePile() ); break;
				case 8: PackItem( new BonePile() ); break;
				case 9: PackItem( new BonePile() ); break;
			}
		}

	     public override bool OnBeforeDeath()
           {
    	           ( new Rot() ).MoveToWorld( this.Location, this.Map );
     	           this.PlaySound( 191 );
     	           return base.OnBeforeDeath();
           }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public DesiccatedCorpse( Serial serial ) : base( serial )
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
