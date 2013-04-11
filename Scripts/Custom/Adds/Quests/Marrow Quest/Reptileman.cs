using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a reptileman corpse" )]
	public class Reptileman : BaseCreature
	{
		[Constructable]
		public Reptileman() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name =  "Reptileman";
			Body = Utility.RandomList( 35, 36 );
			BaseSoundID = 417;
            Hue = 2943;

			SetStr( 196, 220 );
			SetDex( 186, 205 );
			SetInt( 436, 460 );

			SetHits( 358, 372 );

			SetDamage( 6, 13 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.MagicResist, 95.0, 100.0 );
			SetSkill( SkillName.Tactics, 105.1, 110.0 );
			SetSkill( SkillName.Wrestling, 150.1, 170.0 );
            SetSkill( SkillName.Magery, 120.0, 130.0 );

			Fame = 4500;
            Karma = -5000;

			VirtualArmor = 40;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
            PackGold(100, 110);
            AddItem(new ReptileBone());
		}

        public override bool BardImmune { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool Unprovokable { get { return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Regular; } }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(3))
            {
                default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.Dismount;
                case 2: return WeaponAbility.ParalyzingBlow;
            }
        }

		public Reptileman( Serial serial ) : base( serial )
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