using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "Raging Bull" )]
    public class RagingBull : BaseCreature
    {
        [Constructable]
        public RagingBull() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "Raging Bull";
            Hue = 2964;
            Body = 232;
            BaseSoundID = 100;

            SetStr( 480 );
            SetDex( 400 );
            SetInt( 200 );
            SetHits( 2500 );
            SetDamage( 24, 36 );
            SetDamageType( ResistanceType.Physical, 65 );
            SetResistance( ResistanceType.Physical, 55 );
            SetResistance( ResistanceType.Cold, 35 );
            SetResistance( ResistanceType.Fire, 66 );
            SetResistance( ResistanceType.Energy, 70 );
            SetResistance( ResistanceType.Poison, 100 );
            Fame = 2000;
            VirtualArmor = 40;

            SetSkill(SkillName.Tactics, 300.0, 400.0);
            SetSkill(SkillName.MagicResist, 100.0, 150.0);
            SetSkill(SkillName.Parry, 200.0, 300.0);
            SetSkill(SkillName.Wrestling, 150.0, 200.0);
        }

        public override bool BardImmune{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
        public override bool CanRummageCorpses{ get{ return true; } }
        public override bool Unprovokable{ get{ return true; } }
        public override Poison HitPoison{ get{ return Poison.Lethal; } }
        public override bool AlwaysMurderer{ get{ return true; } }

        public override void GenerateLoot()
        {
            PackGold(1000);
            AddLoot(LootPack.SuperBoss);
        }

		public override WeaponAbility GetWeaponAbility()
		{
			switch ( Utility.Random(4 ) )
			{
				default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.ArmorIgnore;
                case 2: return WeaponAbility.Dismount;
                case 3: return WeaponAbility.ParalyzingBlow;
            }
        }

        public RagingBull( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}