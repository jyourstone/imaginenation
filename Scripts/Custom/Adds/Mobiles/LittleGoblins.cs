using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a goblin scout corpse")]
    public class GoblinScout : BaseCreature
    {
        [Constructable]
        public GoblinScout()
            : base(AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Goblin Scout";
            Body = 296;
            BaseSoundID = 0x45A;

            SetStr(125, 140);
            SetDex(110, 135);
            SetInt(66, 90);

            SetHits(98, 122);
            SetMana(0);

            SetDamage(8, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 50.6, 75.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1500;

            VirtualArmor = 22;
           
        }

        public override int Meat
        {
            get { return 1; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool BleedImmune { get { return true; } }

        public GoblinScout(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [CorpseName("a goblin archer corpse")]
    public class GoblinArcher : BaseCreature
    {
        [Constructable]
        public GoblinArcher()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 5, 0.2, 0.4)
        {
            Name = "Goblin Archer";
            Body = 297;
            BaseSoundID = 0x45A;

            SetStr(125, 140);
            SetDex(110, 135);
            SetInt(66, 90);

            SetHits(98, 122);
            SetMana(0);

            SetDamage(8, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 50.6, 75.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1500;

            VirtualArmor = 22;

        }

        public override int Meat
        {
            get { return 1; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool BleedImmune { get { return true; } }

        public GoblinArcher(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [CorpseName("a goblin warrior corpse")]
    public class GoblinWarrior : BaseCreature
    {
        [Constructable]
        public GoblinWarrior()
            : base(AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Goblin Warrior";
            Body = 298;
            BaseSoundID = 0x45A;

            SetStr(165, 180);
            SetDex(140, 165);
            SetInt(66, 90);

            SetHits(158, 192);
            SetMana(0);

            SetDamage(12, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 50.6, 75.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1500;

            VirtualArmor = 22;

        }

        public override int Meat
        {
            get { return 2; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            PackGold(100, 150);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool BleedImmune { get { return true; } }

        public GoblinWarrior(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [CorpseName("an armored goblin corpse")]
    public class ArmoredGoblinArcher : BaseCreature
    {
        [Constructable]
        public ArmoredGoblinArcher()
            : base(AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Armored Goblin Archer";
            Body = 298;
            BaseSoundID = 0x45A;

            SetStr(160, 170);
            SetDex(160, 195);
            SetInt(66, 90);

            SetHits(158, 192);
            SetMana(0);

            SetDamage(12, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 50.6, 75.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1500;

            VirtualArmor = 22;

        }

        public override int Meat
        {
            get { return 2; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            PackGold(100, 150);
            Bow weapon = new Bow();
            weapon.DamageLevel = WeaponDamageLevel.Might;
            weapon.Hue = 2956;
            AddItem(weapon);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool BleedImmune { get { return true; } }

        public ArmoredGoblinArcher(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}