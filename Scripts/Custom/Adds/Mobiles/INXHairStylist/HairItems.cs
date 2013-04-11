namespace Server.INXHairStylist
{
    #region Hair styles

    public class Mohawk : Item
    {
        [Constructable]
        public Mohawk() : base(0x2044)
        {
        }

        public Mohawk(Serial serial) : base(serial)
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

    public class PageboyHair : Item
    {
        [Constructable]
        public PageboyHair(): base(0x2045)
        {
        }

        public PageboyHair(Serial serial) : base(serial)
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

    public class BunsHair : Item
    {
        [Constructable]
        public BunsHair() : base(0x2046)
        {
        }

        public BunsHair(Serial serial) : base(serial)
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

    public class LongHair : Item
    {
        [Constructable]
        public LongHair() : base(0x203C)
        {
        }

        public LongHair(Serial serial) : base(serial)
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

    public class ShortHair : Item
    {
        [Constructable]
        public ShortHair() : base(0x203B)
        {
        }

        public ShortHair(Serial serial) : base(serial)
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

    public class PonyTail : Item
    {
        [Constructable]
        public PonyTail() : base(0x203D)
        {
        }

        public PonyTail(Serial serial) : base(serial)
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

    public class Afro : Item
    {
        [Constructable]
        public Afro()  : base(0x2047)
        {
        }

        public Afro(Serial serial) : base(serial)
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

    public class ReceedingHair : Item
    {
        [Constructable]
        public ReceedingHair() : base(0x2048)
        {
        }

        public ReceedingHair(Serial serial) : base(serial)
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

    public class TwoPigTails : Item
    {
        [Constructable]
        public TwoPigTails() : base(0x2049)
        {
        }

        public TwoPigTails(Serial serial) : base(serial)
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

    public class KrisnaHair : Item
    {
        [Constructable]
        public KrisnaHair() : base(0x204A)
        {
        }

        public KrisnaHair(Serial serial) : base(serial)
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

    #region Elven Hair
    public class MidLongHair : Item
    {
        [Constructable]
        public MidLongHair() : base(0x2FBF)
        {
        }

        public MidLongHair(Serial serial) : base(serial)
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

    public class LongFeatherHair : Item
    {
        [Constructable]
        public LongFeatherHair() : base(0x2FC0)
        {
        }

        public LongFeatherHair(Serial serial) : base(serial)
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

    public class ShortElfHair : Item
    {
        [Constructable]
        public ShortElfHair() : base(0x2FC1)
        {
        }

        public ShortElfHair(Serial serial)  : base(serial)
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

    public class LongElfHair : Item
    {
        [Constructable]
        public LongElfHair() : base(0x2FCD)
        {
        }

        public LongElfHair(Serial serial) : base(serial)
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

    public class FlowerHair : Item
    {
        [Constructable]
        public FlowerHair() : base(0x2FCC)
        {
        }

        public FlowerHair(Serial serial) : base(serial)
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

    public class LongBigKnobHair : Item
    {
        [Constructable]
        public LongBigKnobHair() : base(0x2FCE)
        {
        }

        public LongBigKnobHair(Serial serial)  : base(serial)
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

    public class Mullet : Item
    {
        [Constructable]
        public Mullet(): base(0x2FC2)
        {
        }

        public Mullet(Serial serial) : base(serial)
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

    public class LongBigBraidHair : Item
    {
        [Constructable]
        public LongBigBraidHair() : base(0x2FCF)
        {
        }

        public LongBigBraidHair(Serial serial) : base(serial)
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

    public class LongBigBunHair : Item
    {
        [Constructable]
        public LongBigBunHair() : base(0x2FD0)
        {
        }

        public LongBigBunHair(Serial serial) : base(serial)
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

    public class SpikedHair : Item
    {
        [Constructable]
        public SpikedHair() : base(0x2FD1)
        {
        }

        public SpikedHair(Serial serial) : base(serial)
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
    public class LongElfTwoHair : Item
    {
        [Constructable]
        public LongElfTwoHair() : base(0x2FD2)
        {
        }

        public LongElfTwoHair(Serial serial)  : base(serial)
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

    #endregion

    #region Beard styles

    public class LongBeard : Item
	{
        [Constructable]
		public LongBeard()
			: base( 0x203E )
		{
		}

		public LongBeard( Serial serial ) : base( serial )
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

	public class ShortBeard : Item
	{
        [Constructable]
		public ShortBeard()
			: base( 0x203f )
		{
		}

		public ShortBeard( Serial serial ) : base( serial )
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

	public class Goatee : Item
	{
        [Constructable]
		public Goatee()
			: base( 0x2040 )
		{
		}

		public Goatee( Serial serial ) : base( serial )
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

	public class Mustache : Item
	{
        [Constructable]
		public Mustache()
			: base( 0x2041 )
		{
		}

		public Mustache( Serial serial ) : base( serial )
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

	public class MediumShortBeard : Item
	{
        [Constructable]
		public MediumShortBeard()
			: base( 0x204B )
		{
		}

		public MediumShortBeard( Serial serial ) : base( serial )
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

	public class MediumLongBeard : Item
	{
        [Constructable]
		public MediumLongBeard()
			: base( 0x204C )
		{
		}

		public MediumLongBeard( Serial serial ) : base( serial )
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

	public class Vandyke : Item
	{
        [Constructable]
		public Vandyke()
			: base( 0x204D )
		{
		}

		public Vandyke( Serial serial ) : base( serial )
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
    #endregion
}