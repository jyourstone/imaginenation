namespace Server.Items
{
    [Flipable(0x2BD9, 0x2BDA)]
	public class GreenStocking : Item
	{
		[Constructable]
		public GreenStocking() : base( 0x2BD9 )
		{
			Name = "Green stocking";
			Weight = 1;
		    Movable = true;
		}

		public GreenStocking( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
    [Flipable(0x2BDB, 0x2BDC)]
    public class RedStocking : Item
    {
        [Constructable]
        public RedStocking()
            : base(0x2BDB)
        {
            Name = "Red stocking";
            Weight = 1;
            Movable = true;
        }

        public RedStocking(Serial serial)
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
    [Flipable(0x2BDD, 0x2BDE)]
    public class RedCandyCane : Item
    {
        [Constructable]
        public RedCandyCane()
            : base(0x2BDD)
        {
            Name = "Red candy cane";
            Weight = 1;
            Movable = true;
        }

        public RedCandyCane(Serial serial)
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
    [Flipable(0x2BDF, 0x2BE0)]
    public class GreenCandyCane : Item
    {
        [Constructable]
        public GreenCandyCane()
            : base(0x2BDF)
        {
            Name = "Green candy cane";
            Weight = 1;
            Movable = true;
        }

        public GreenCandyCane(Serial serial)
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
    [Flipable(0x2BE1, 0x2BE2)]
    public class GingerBreadMan : Item
    {
        [Constructable]
        public GingerBreadMan()
            : base(0x2BE1)
        {
            Name = "Gingerbread man";
            Weight = 1;
            Movable = true;
        }

        public GingerBreadMan(Serial serial)
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
    public class FestiveShrub : Item
    {
        [Constructable]
        public FestiveShrub()
            : base(0x2378)
        {
            Name = "Festive shrub";
            Weight = 5;
            Movable = true;
        }

        public FestiveShrub(Serial serial)
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
    [Flipable(0x2374, 0x2375)]
    public class DecorativeMistletoe : Item
    {
        [Constructable]
        public DecorativeMistletoe()
            : base(0x2374)
        {
            Name = "Decorative mistletoe";
            Weight = 1;
            Movable = true;
        }

        public DecorativeMistletoe(Serial serial)
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
    [Flipable(0x236E, 0x2371)]
    public class HolidayCandle : Item
    {
        [Constructable]
        public HolidayCandle()
            : base(0x236E)
        {
            Name = "Holiday candle";
            Weight = 1;
            Movable = true;
        }

        public HolidayCandle(Serial serial)
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
    public class SnowCoveredTree : Item
    {
        [Constructable]
        public SnowCoveredTree()
            : base(0x2377)
        {
            Name = "Snowy tree";
            Weight = 5;
            Movable = true;
        }

        public SnowCoveredTree(Serial serial)
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
    public class LumpOfCoal : Item
    {
        [Constructable]
        public LumpOfCoal()
            : base(0xE73)
        {
            Name = "Lump of coal";
            Weight = 1;
            Movable = true;
            Stackable = false;
            Hue = 1105;
        }

        public LumpOfCoal(Serial serial)
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
    public class Snowball : Item
    {
        [Constructable]
        public Snowball()
            : base(0xE73)
        {
            Name = "Snowball";
            Weight = 1;
            Movable = true;
            Stackable = false;
            Hue = 1157;
        }

        public Snowball(Serial serial)
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
    public class SnowGlobe : Item
    {
        [Constructable]
        public SnowGlobe()
            : base(0xE2D)
        {
            Name = "Snow globe";
            Weight = 1;
            Movable = true;
            Stackable = false;
            Hue = 1154;
        }

        public SnowGlobe(Serial serial)
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
    [Flipable(0x232C, 0x232D)]
    public class HolidayWreath : Item
    {
        [Constructable]
        public HolidayWreath()
            : base(0x232C)
        {
            Name = "Holiday wreath";
            Weight = 1;
            Movable = true;
        }

        public HolidayWreath(Serial serial)
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
    public class HolidayCactus : Item
    {
        [Constructable]
        public HolidayCactus()
            : base(0x2376)
        {
            Name = "Holiday cactus";
            Weight = 5;
            Movable = true;
        }

        public HolidayCactus(Serial serial)
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
    public class MiniXmasTree : Item
    {
        [Constructable]
        public MiniXmasTree()
            : base(0x3F16)
        {
            Name = "Mini holiday tree";
            Weight = 2;
            Movable = true;
        }

        public MiniXmasTree(Serial serial)
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
    public class RandomReindeer : Item
    {
        [Constructable]
        public RandomReindeer()
            : base(0x3A55)
        {
            //Name = "Reindeer";
            Weight = 2;
            Movable = true;
            switch (Utility.Random(8))
            {
                case 0:
                    Name = "Dancer";
                    break;
                case 1:
                    Name = "Prancer";
                    break;
                case 2:
                    Name = "Vixen";
                    break;
                case 3:
                    Name = "Cupid";
                    break;
                case 4:
                    Name = "Comet";
                    break;
                case 5:
                    Name = "Blitzen";
                    break;
                case 6:
                    Name = "Donner";
                    break;
                case 7:
                    Name = "Dasher";
                    break;
            }
        }

        public RandomReindeer(Serial serial)
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