using System;
using Server.Items;
using Server.Mobiles;
using Solaris.CliLocHandler;
using Xanthos.ShrinkSystem;

namespace Server.Items
{
    public class PVPPackage : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [Constructable]
        public PVPPackage() : base(0x1ECD)
        {
            Hue = 2881;
            Name = "PvP package crystal";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.BankBox))
            {
                if (from.AccessLevel == AccessLevel.Player && Owner != from)
                {
                    from.SendAsciiMessage("This is not your item, only the owner can claim this.");
                    return;
                }

                Backpack bp = new Backpack { Hue = 1170, Name = "Gift bag"};

                //PvP
                Bag pvpBag = new Bag {Hue = 1, Name = "Gift bag"};
                pvpBag.DropItem(new FlamestrikeScroll {Amount = 200, Name = "Flame Strike (gift)"});
                pvpBag.DropItem(new ManaPotion { Amount = 500, Name = "Mana Potion (gift)" });
                pvpBag.DropItem(new GreaterHealPotion { Amount = 500, Name = "greater heal potion (gift)" });
                pvpBag.DropItem(new KillRemoveBall {Name = "Kill remove ball (gift)" });
                pvpBag.DropItem(new KillRemoveBall { Name = "Kill remove ball (gift)" });
                pvpBag.DropItem(new KillRemoveBall { Name = "Kill remove ball (gift)" });
                pvpBag.DropItem(new KillRemoveBall { Name = "Kill remove ball (gift)" });
                pvpBag.DropItem(new KillRemoveBall { Name = "Kill remove ball (gift)" });

                //Armor
                Bag armorBag = new Bag { Hue = 1218, Name = "Gift bag" };
                armorBag.DropItem(new HeaterShield { Resource = CraftResource.BloodRock, Quality  = ArmorQuality.Exceptional, Name = "heater shield (gift)"});
                armorBag.DropItem(new PlateChest { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail chest (gift)" });
                armorBag.DropItem(new PlateLegs { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail legs (gift)" });
                armorBag.DropItem(new PlateArms { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail arms (gift)" });
                armorBag.DropItem(new PlateGloves { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gloves (gift)" });
                armorBag.DropItem(new PlateGorget { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gorget (gift)" });
                armorBag.DropItem(new PlateHelm { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "plate helm (gift)" });

                Bag armorBag2 = new Bag { Hue = 1218, Name = "Gift bag" };
                armorBag2.DropItem(new HeaterShield { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "heater shield (gift)" });
                armorBag2.DropItem(new PlateChest { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail chest (gift)" });
                armorBag2.DropItem(new PlateLegs { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail legs (gift)" });
                armorBag2.DropItem(new PlateArms { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail arms (gift)" });
                armorBag2.DropItem(new PlateGloves { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gloves (gift)" });
                armorBag2.DropItem(new PlateGorget { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gorget (gift)" });
                armorBag2.DropItem(new PlateHelm { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "plate helm (gift)" });

                Bag armorBag3 = new Bag { Hue = 1218, Name = "Gift bag" };
                armorBag3.DropItem(new HeaterShield { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "heater shield (gift)" });
                armorBag3.DropItem(new PlateChest { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail chest (gift)" });
                armorBag3.DropItem(new PlateLegs { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail legs (gift)" });
                armorBag3.DropItem(new PlateArms { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail arms (gift)" });
                armorBag3.DropItem(new PlateGloves { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gloves (gift)" });
                armorBag3.DropItem(new PlateGorget { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gorget (gift)" });
                armorBag3.DropItem(new PlateHelm { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "plate helm (gift)" });

                //Add to backpack
                bp.DropItem(new Robe{Hue = 1, Name = "robe (gift)"});
                bp.DropItem(new BankCheck(80000) {Name = "(gift)" });
                bp.DropItem(new ExceptionalWeaponCrystal());
                bp.DropItem(new ExceptionalWeaponCrystal());
                bp.DropItem(pvpBag);
                bp.DropItem(armorBag);
                bp.DropItem(armorBag2);
                bp.DropItem(armorBag3);
                
                //Add to bank
                from.BankBox.DropItem(bp);

                from.SendAsciiMessage("Your items have been placed in a backpack in your bank box");

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your bank box for you to use it.");
            }
        }

        public PVPPackage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            writer.Write(Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    Owner = reader.ReadMobile();
                    break;
                case 0:
                    break;
            }
        }
    }

    public class PVMPackage : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [Constructable]
        public PVMPackage()
            : base(0x1ECD)
        {
            Hue = 2997;
            Name = "PvM package crystal";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.BankBox))
            {
                if (from.AccessLevel == AccessLevel.Player && Owner != from)
                {
                    from.SendAsciiMessage("This is not your item, only the owner can claim this.");
                    return;
                }

                Backpack bp = new Backpack { Hue = 1170, Name = "Gift bag" };

                Bag pvmBag = new Bag {Hue = 1, Name = "Gift bag"};
                pvmBag.DropItem(new BladeSpiritsScroll{Amount = 200, Name = "Blade spirit (gift)"});
                pvmBag.DropItem(new EnergyVortexScroll{Amount = 100, Name = "Energy vortex (gift)"});
                pvmBag.DropItem(new ManaPotion{Amount = 100, Name = "Mana Potion (gift)"});
                pvmBag.DropItem(new Arrow{Amount = 10000, Name = "arrow (gift)"});
                pvmBag.DropItem(new Bloodmoss{Amount = 400, Name = "Blood Mooss (gift)"});
                pvmBag.DropItem(new MandrakeRoot { Amount = 400, Name = "Mandrake Root (gift)" });
                pvmBag.DropItem(new Ginseng{Amount = 400, Name = "Ginseng (gift)"});
                pvmBag.DropItem(new BlackPearl { Amount = 400, Name = "Black Pearl (gift)" });
                pvmBag.DropItem(new SpidersSilk { Amount = 400, Name = "Spiders' Silk (gift)" });
                pvmBag.DropItem(new SulfurousAsh { Amount = 400, Name = "Sulfurous Ash (gift)" });
                pvmBag.DropItem(new Nightshade { Amount = 400, Name = "Nightshade (gift)" });
                pvmBag.DropItem(new Garlic { Amount = 400, Name = "Garlic (gift)" });
                
                //Armor
                Bag armorBag = new Bag { Hue = 1218 };
                armorBag.DropItem(new HeaterShield { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "heater shield (gift)" });
                armorBag.DropItem(new PlateChest { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail chest (gift)" });
                armorBag.DropItem(new PlateLegs { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail legs (gift)" });
                armorBag.DropItem(new PlateArms { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail arms (gift)" });
                armorBag.DropItem(new PlateGloves { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gloves (gift)" });
                armorBag.DropItem(new PlateGorget { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "platemail gorget (gift)" });
                armorBag.DropItem(new PlateHelm { Resource = CraftResource.BloodRock, Quality = ArmorQuality.Exceptional, Name = "plate helm (gift)" });

                //Mount
                Horse horse = new Horse
                                  {
                                      ItemID = 16034,
                                      BodyValue = 120,
                                      Name = "Mustang",
                                      ControlMaster = from,
                                      Controlled = true,
                                      MinTameSkill = 100.0,
                                      Hue = Utility.Random(4, 900)
                                  };

                //Add to backpack
                bp.DropItem(new ShrinkItem(horse));
                bp.DropItem(new Robe { Hue = 1963, Name = "robe (gift)"});
                bp.DropItem(new BankCheck(125000) {Name = "(gift)"});
                bp.DropItem(pvmBag);
                bp.DropItem(armorBag);
                bp.DropItem(new ExceptionalWeaponCrystal());

                //Add to bank
                from.BankBox.DropItem(bp);

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your bank box for you to use it.");
            }
        }

        public PVMPackage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            writer.Write(Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    Owner = reader.ReadMobile();
                    break;
                case 0:
                    break;
            }
        }
    }

    public class ExceptionalWeaponCrystal : Item
    {
        [Constructable]
        public ExceptionalWeaponCrystal() : base(0x1ECD)
        {
            Weight = 1.0;
            Name = "Exceptional weapon crystal (gift)";
            Hue = 2812;
        }

        public ExceptionalWeaponCrystal(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                BaseWeapon w = BaseWeapon.CreateRandomWeapon();

                while (w is BaseRanged)
                {
                    w.Delete();
                    w = BaseWeapon.CreateRandomWeapon();
                }

                w.Identified = true;

                w.DamageLevel = (WeaponDamageLevel)Utility.Random(6);

                w.Name = CliLoc.LocToString(w.LabelNumber) + " (gift)";

                int roll = Utility.Random(99);
                if (roll < 51)
                    w.AccuracyLevel = WeaponAccuracyLevel.Accurate;
                else if (roll < 77)
                    w.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                else if (roll < 90)
                    w.AccuracyLevel = WeaponAccuracyLevel.Eminently;
                else if (roll < 97)
                    w.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
                else if (roll < 100)
                    w.AccuracyLevel = WeaponAccuracyLevel.Supremely;

                from.AddToBackpack(w);
                Delete();
            }
            else
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
        }
    }
}