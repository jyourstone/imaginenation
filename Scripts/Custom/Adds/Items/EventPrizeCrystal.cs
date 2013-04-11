using System;
using Carding.Mobiles;
using Xanthos.ShrinkSystem;
using Server;
using Server.Scripts.Custom.Adds.System.Loots;

namespace Server.Items
{
    class EventPrizeCrystal : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public double GoldChance
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double NickelChance
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GoldAmount
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NickelAmount
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double VanqWeaponChance
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double InvulArmorChance
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double BlessedItemChance
        {
            get;
            set;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MustangChance
        {
            get;
            set;
        }

        [Constructable]
        public EventPrizeCrystal() : base(0x1ECD)
        {
            Weight = 1.0;
            Name = "Event Prize Crystal";
            Hue = 2570;
            GoldChance = 70;
            GoldAmount = 7000;
            NickelChance = 6;
            NickelAmount = 5;
            VanqWeaponChance = 8;
            InvulArmorChance = 7;
            BlessedItemChance = 5;
            MustangChance = 4;

            initPrizes();
        }

        public EventPrizeCrystal(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(this.GoldAmount);
            writer.Write(this.GoldChance);
            writer.Write(this.NickelAmount);
            writer.Write(this.NickelChance);
            writer.Write(this.InvulArmorChance);
            writer.Write(this.VanqWeaponChance);
            writer.Write(this.BlessedItemChance);
            writer.Write(this.MustangChance);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        GoldAmount = reader.ReadInt();
                        GoldChance = reader.ReadDouble();
                        NickelAmount = reader.ReadInt();
                        NickelChance = reader.ReadDouble();
                        InvulArmorChance = reader.ReadDouble();
                        VanqWeaponChance = reader.ReadDouble();
                        BlessedItemChance = reader.ReadDouble();
                        MustangChance = reader.ReadDouble();
                        break;
                    }
            }

            initPrizes();
        }

        public class Prize
        {
            public Prize(double chance, Type type) : this(chance, type, 1) { }

            public Prize(double chance, Type type, int amount)
            {
                Chance = (int)(chance * 100);
                Type = type;
                Amount = amount;
            }

            public int Chance { get; set; }
            Type Type { get; set; }
            int Amount { get; set; }


            internal Item Construct()
            {
                try
                {
                    Item item;

                    if (Type == typeof(Gold))
                        item = new Gold(Amount);
                    else if (Type == typeof(ImagineNickel))
                        item = new ImagineNickel(Amount);
                    else if (Type == typeof(BaseWeapon))
                    {
                        BaseWeapon w = Loot.RandomWeapon();
                        w.DamageLevel = WeaponDamageLevel.Vanq;
                        int accuracyRoll = Utility.Random(99);
                        if (accuracyRoll < 31) // 30% to get Accurate
                            w.AccuracyLevel = WeaponAccuracyLevel.Accurate;
                        else if (accuracyRoll < 56) // 25% to get Surpassingly
                            w.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                        else if (accuracyRoll < 76) // 20% to get Eminently
                            w.AccuracyLevel = WeaponAccuracyLevel.Eminently;
                        else if (accuracyRoll < 91) // 15% to get Exceedingly
                            w.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
                        else if (accuracyRoll < 100) // 10% to get Supremely
                            w.AccuracyLevel = WeaponAccuracyLevel.Supremely;
                        item = w;
                    }
                    else if (Type == typeof(BaseArmor))
                    {
                        BaseArmor armor = Loot.RandomArmorOrShield();
                        armor.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
                        armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
                        item = armor;
                    }
                    else if (Type == typeof(BaseClothing))
                    {
                        item = Loot.RandomClothing();
                        item.LootType = LootType.Blessed;
                        item.Hue = Utility.RandomList(Sphere.RareHues);
                    }
                    else if (Type == typeof(BaseJewel))
                    {
                        item = Loot.RandomJewelry();
                        item.LootType = LootType.Blessed;
                        item.Hue = Utility.RandomList(Sphere.RareHues);
                    }
                    else if (Type == typeof(Mustang))
                    {
                        Mustang m = new Mustang();
                        MustangCollection.Randomize().ApplyTo(m);
                        item = new ShrinkItem(m);
                    }
                    else
                        item = Activator.CreateInstance(Type) as Item;
                    return item;
                }
                catch
                {
                }
                return null;
            }
        }

        private Prize[] Prizes;

        private void initPrizes()
        {
            Prizes = new[] {
                new Prize(GoldChance, typeof(Gold), GoldAmount),
                new Prize(NickelChance, typeof(ImagineNickel), NickelAmount),
                new Prize(VanqWeaponChance, typeof(BaseWeapon)),
                new Prize(InvulArmorChance, typeof(BaseArmor)),
                new Prize(BlessedItemChance, typeof(BaseClothing)),
                new Prize(BlessedItemChance, typeof(BaseJewel)),
                new Prize(MustangChance, typeof(Mustang))};
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                for (int i = 0; i < Prizes.Length; ++i)
                {
                    Prize entry = Prizes[i];

                    bool shouldAdd = (entry.Chance > Utility.Random(10000));
                    if (!shouldAdd)
                        continue;

                    Item item = entry.Construct();
                    if (item != null)
                    {
                        from.AddToBackpack(item);
                        from.SendMessage("You have been awarded a prize: " + Sphere.ComputeName(item));
                        Delete();
                    }
                    return;
                }
            }
            else
            {
                from.SendLocalizedMessage(1080058);
            }
        }
    }
}
