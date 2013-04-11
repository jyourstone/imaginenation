using System;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a nightmare corpse" )]
	public class YoungNightmare : BaseMount
	{
		[Constructable]
		public YoungNightmare() : this( "Young Nightmare" )
		{
		}

        public override bool SubdueBeforeTame { get { return true; } } // Must be beaten into submission

		[Constructable]
		public YoungNightmare( string name ) : base( name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA8;

			SetStr( 296, 325 );
			SetDex( 96, 105 );
			SetInt( 86, 95 );

			SetHits( 218, 225 );

			SetDamage( 16, 26 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 40 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.EvalInt, 10.4, 30.0 );
			SetSkill( SkillName.Magery, 10.4, 30.0 );
			SetSkill( SkillName.MagicResist, 15.3, 30.0 );
            SetSkill(SkillName.Meditation, 5.3, 10.0);
			SetSkill( SkillName.Tactics, 9.6, 15.0 );
            SetSkill(SkillName.Anatomy, 5.3, 10.0);
			SetSkill( SkillName.Wrestling, 10.5, 20.5 );

			Fame = 9000;
            Karma = -20000;

			VirtualArmor = 20;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 100.0;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
				{
					BodyValue = 116;
					ItemID = 16039;
					break;
				}
				case 1:
				{
					BodyValue = 178;
					ItemID = 16041;
					break;
				}
				case 2:
				{
					BodyValue = 179;
					ItemID = 16055;
					break;
				}
			}

			PackItem( new SulfurousAsh( Utility.RandomMinMax( 3, 5 ) ) );
		}

        public override bool CanBeHarmful( Mobile target, bool message)
        {
            if (target is PlayerMobile)
            {
                return false;
            }

            return base.CanBeHarmful(target, message);
        }

	    public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Potions );
		}

		public override int GetAngerSound()
		{
			if ( !Controlled )
				return 0x16A;

			return base.GetAngerSound();
        }

        public override bool AlwaysAttackable { get { return true; } }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 10; } }
        public override HideType HideType { get { return HideType.Regular; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }

		public YoungNightmare( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( BaseSoundID == 0x16A )
				BaseSoundID = 0xA8;
		}
	}
}