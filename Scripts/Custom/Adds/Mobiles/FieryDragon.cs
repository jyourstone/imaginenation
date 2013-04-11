using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fiery dragon corpse")]
    public class FieryDragon : BaseCreature
    {
        [Constructable]
        public FieryDragon()
            : base(AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Fiery Dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = 1158;
            SetStr(1000, 1100);
            SetDex(86, 105);
            SetInt(436, 475);

            SetHits(538, 575);

            SetDamage(45, 60);

            SetDamageType(ResistanceType.Physical, 100);
            AddItem(new DragonsBlood(Utility.RandomMinMax(4, 11)));

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 95);
            SetSkill(SkillName.AnimalTaming, 99.0);
            SetSkill(SkillName.Parry, 55.0, 95.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 60;

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 93.9;
        }

        public FieryDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool HasBreath
        {
            get { return true; }
        }

        public override bool AutoDispel
        {
            get { return false; }
        }

        public override int TreasureMapLevel
        {
            get { return 4; }
        }

        public override int Meat
        {
            get { return 99; }
        }

        public override int Hides
        {
            get { return 20; }
        }

        public override HideType HideType
        {
            get
            {
                double roll = Utility.RandomDouble();

                if (roll <= 0.05)
                    return HideType.Barbed;
                if (roll <= 0.2)
                    return HideType.Horned;
                if (roll <= 0.5)
                    return HideType.Spined;

                return HideType.Regular;
            }
        }

        public override int Scales
        {
            get { return 11; }
        }

        public override ScaleType ScaleType
        {
            get { return (ScaleType.Red); }
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 8);
            if (Utility.RandomDouble() <= 0.1)
                AddLoot(LootPack.RandomWand, 1);
            PackGold(1000);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(2, 3)));
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