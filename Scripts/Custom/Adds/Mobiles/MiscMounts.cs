namespace Server.Mobiles
{
    [CorpseName("a giant toad corpse")]
    public class GiantToadMount : BaseMount
    {
        [Constructable]
        public GiantToadMount()
            : this("Ridable Giant Toad")
        {
        }

        [Constructable]
        public GiantToadMount(string name)
            : base(name, 0x50, 0x393F, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public GiantToadMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a gorilla corpse")]
    public class GorillaMount : BaseMount
    {
        [Constructable]
        public GorillaMount()
            : this("Ridable Gorilla")
        {
        }

        [Constructable]
        public GorillaMount(string name)
            : base(name, 0x1D, 0x3940, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public GorillaMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a Walrus corpse")]
    public class WalrusMount : BaseMount
    {
        [Constructable]
        public WalrusMount()
            : this("Ridable Walrus")
        {
        }

        [Constructable]
        public WalrusMount(string name)
            : base(name, 0xDD, 0x3941, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public WalrusMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a cow corpse")]
    public class CowMount : BaseMount
    {
        [Constructable]
        public CowMount()
            : this("Ridable Cow")
        {
        }

        [Constructable]
        public CowMount(string name)
            : base(name, 0xE7, 0x3942, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public CowMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a ant lion corpse")]
    public class AntLionMount : BaseMount
    {
        [Constructable]
        public AntLionMount()
            : this("Ridable Ant Lion")
        {
        }

        [Constructable]
        public AntLionMount(string name)
            : base(name, 0x313, 0x3943, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public AntLionMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a solen queen corpse")]
    public class RedSolenQueenMount : BaseMount
    {
        [Constructable]
        public RedSolenQueenMount()
            : this("Ridable Solen Queen")
        {
        }

        [Constructable]
        public RedSolenQueenMount(string name)
            : base(name, 0x30F, 0x3944, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public RedSolenQueenMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

namespace Server.Mobiles
{
    [CorpseName("a gazer corpse")]
    public class GazerMount : BaseMount
    {
        [Constructable]
        public GazerMount()
            : this("Ridable Gazer")
        {
        }

        [Constructable]
        public GazerMount(string name)
            : base(name, 0x16, 0x3945, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            //ControlSlots = 1;
            MinTameSkill = 90.6;
        }

        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public GazerMount(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}