namespace Server.Items
{
    [Flipable(0x390E, 0x390D, 0x390C)]
    public class GiftBoxes : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x102; } }
        [Constructable]
        public GiftBoxes()
            : base(0x390E)
        {
            Weight = 1.0;
            Name = "Gift box";
        }

        public GiftBoxes(Serial serial)
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

    [Flipable(0x3BBB, 0x3BBC)]
    public class FullDresser : Item
    {
        [Constructable]
        public FullDresser()
            : base(0x3BBB)
        {
            Weight = 25.0;
            Name = "Full dresser";
        }

        public FullDresser(Serial serial)
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

    [Flipable(0x3BB9, 0x3BBA)]
    public class FullArmoire : Item
    {
        [Constructable]
        public FullArmoire()
            : base(0x3BB9)
        {
            Weight = 25.0;
            Name = "Full armoire";
        }

        public FullArmoire(Serial serial)
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

    [Flipable(0x3BC5, 0x3BC6)]
    public class OakNightStand : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x51; } }
        [Constructable]
        public OakNightStand()
            : base(0x3BC5)
        {
            Weight = 5.0;
            Name = "Oak night stand";
        }

        public OakNightStand(Serial serial)
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

    [Flipable(0x3BD0, 0x3BCF)]
    public class WineRack : Item
    {
        [Constructable]
        public WineRack()
            : base(0x3BD0)
        {
            Weight = 25.0;
            Name = "Wine rack";
        }

        public WineRack(Serial serial)
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

    [Flipable(0x3BC9, 0x3BCA)]
    public class FullShelves : Item
    {
        [Constructable]
        public FullShelves()
            : base(0x3BC9)
        {
            Weight = 25.0;
            Name = "Full shelf";
        }

        public FullShelves(Serial serial)
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

    [Flipable(0x3BCC, 0x3BCB)]
    public class HalberdRack : Item
    {
        [Constructable]
        public HalberdRack()
            : base(0x3BCC)
        {
            Weight = 25.0;
            Name = "Weapon rack";
        }

        public HalberdRack(Serial serial)
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

    [Flipable(0x3BCD, 0x3BCD)]
    public class ShieldRack : Item
    {
        [Constructable]
        public ShieldRack()
            : base(0x3BCD)
        {
            Weight = 25.0;
            Name = "Shield rack";
        }

        public ShieldRack(Serial serial)
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

    [Flipable(0x3BD1, 0x3BD2)]
    public class CookingTools : Item
    {
        [Constructable]
        public CookingTools()
            : base(0x3BD2)
        {
            Weight = 2.0;
            Name = "Cooking utensils";
        }

        public CookingTools(Serial serial)
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

    [Flipable(0x3BC7, 0x3BC8)]
    public class SmallBookShelf : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x4D; } }
        [Constructable]
        public SmallBookShelf()
            : base(0x3BC7)
        {
            Weight = 12.0;
            Name = "Small bookshelf";
        }

        public SmallBookShelf(Serial serial)
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

    #region paintings
    [Flipable(0x3BF5, 0x3BF6)]
    public class Painting11 : Item
    {
        [Constructable]
        public Painting11()
            : base(0x3BF5)
        {
            Weight = 3.0;
            Name = "Painting";
        }

        public Painting11(Serial serial)
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

    [Flipable(0x3BE5, 0x3BE6)]
    public class Painting12 : Item
    {
        [Constructable]
        public Painting12()
            : base(0x3BE5)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting12(Serial serial)
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

    public class Painting13 : Item
    {
        [Constructable]
        public Painting13()
            : base(0x3BE4)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting13(Serial serial)
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

    public class Painting14 : Item
    {
        [Constructable]
        public Painting14()
            : base(0x3BE9)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting14(Serial serial)
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

    public class Painting15 : Item
    {
        [Constructable]
        public Painting15()
            : base(0x3BF4)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting15(Serial serial)
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

    public class Painting16 : Item
    {
        [Constructable]
        public Painting16()
            : base(0x3BF1)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting16(Serial serial)
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

    public class Painting17 : Item
    {
        [Constructable]
        public Painting17()
            : base(0x3BF3)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting17(Serial serial)
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

    public class Painting18 : Item
    {
        [Constructable]
        public Painting18()
            : base(0x3BF0)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting18(Serial serial)
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

    public class Painting19 : Item
    {
        [Constructable]
        public Painting19()
            : base(0x3BF2)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting19(Serial serial)
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

    public class Painting20 : Item
    {
        [Constructable]
        public Painting20()
            : base(0x3BEF)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting20(Serial serial)
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

    public class Painting21 : Item
    {
        [Constructable]
        public Painting21()
            : base(0x3BE7)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting21(Serial serial)
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

    public class Painting22 : Item
    {
        [Constructable]
        public Painting22()
            : base(0x3BEC)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting22(Serial serial)
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

    public class Painting23 : Item
    {
        [Constructable]
        public Painting23()
            : base(0x3BEE)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting23(Serial serial)
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

    public class Painting24 : Item
    {
        [Constructable]
        public Painting24()
            : base(0x3BE8)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting24(Serial serial)
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

    public class Painting25 : Item
    {
        [Constructable]
        public Painting25()
            : base(0x3BEC)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting25(Serial serial)
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

    public class Painting26 : Item
    {
        [Constructable]
        public Painting26()
            : base(0x3BEA)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting26(Serial serial)
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

    public class Painting27 : Item
    {
        [Constructable]
        public Painting27()
            : base(0x3BED)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting27(Serial serial)
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

    public class Painting28 : Item
    {
        [Constructable]
        public Painting28()
            : base(0x3BEB)
        {
            Weight = 2.0;
            Name = "Painting";
        }

        public Painting28(Serial serial)
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
    #endregion
}