namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class DragonMount : BaseMount
    {
        [Constructable]
        public DragonMount()
            : this("Ridable Dragon")
        {
        }

        [Constructable]
        public DragonMount(string name)
            : base(name, 0x22, 0x390F, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //BaseSoundID = 0xAD;

            SetStr(210, 300);
            SetDex(75, 120);
            SetInt(20, 40);

            SetHits(200, 270);
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
        
        public DragonMount(Serial serial)
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