namespace Server.Mobiles
{
    [CorpseName("a grizzly bear corpse")]
    public class GrizzlyMount : BaseMount
    {
        [Constructable]
        public GrizzlyMount()
            : this("Ridable Grizzly Bear")
        {
        }

        [Constructable]
        public GrizzlyMount(string name)
            : base(name, 0xD4, 0x38FA, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(110, 120);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(110, 130);
            SetMana(0);

            SetDamage(1, 2);

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

        public override int Meat { get { return 4; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public GrizzlyMount(Serial serial)
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