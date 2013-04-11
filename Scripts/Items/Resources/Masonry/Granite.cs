namespace Server.Items
{
	public abstract class BaseGranite : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}
		
        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 1:
				case 0:
				{
					m_Resource = (CraftResource)reader.ReadInt();
					break;
				}
			}

            if (version < 1)
                Stackable = Core.ML;
		}

		public override double DefaultWeight
		{
            get { return Core.ML ? 1.0 : 5.0; } // Pub 57
        }

        public BaseGranite( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseGranite( CraftResource resource, int amount ) : base( 0x1779 )
		{
            Stackable = true;
            Amount = amount;
			Hue = CraftResources.GetHue( resource );
            Stackable = Core.ML;

			m_Resource = resource;
		}

		public BaseGranite( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, string.Format("{0} {1} granite{2}", Amount > 1 ? Amount.ToString() : "", CraftResources.GetName(m_Resource).ToLower(), Amount > 1 ? "s" : ""));
        }

		public override int LabelNumber{ get{ return 1044607; } } // high quality granite

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}
	}

	public class Granite : BaseGranite
	{
        [Constructable]
		public Granite() : this( 1 )
		{
		}
		[Constructable]
		public Granite( int amount) : base( CraftResource.Iron, amount )
		{
		    Stackable = true;
		    Name = "iron granite";
		}

		public Granite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "iron granite";
		}
	}

	public class DullCopperGranite : BaseGranite
	{
        [Constructable]
		public DullCopperGranite() : this( 1 )
		{
		}

		[Constructable]
		public DullCopperGranite(int amount) : base( CraftResource.DullCopper, amount )
		{
		    Stackable = true;
		    Name = "dull copper granite";
		}

		public DullCopperGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "dull copper granite";
		}
	}

	public class ShadowIronGranite : BaseGranite
	{
        [Constructable]
		public ShadowIronGranite() : this( 1 )
		{
		}

		[Constructable]
		public ShadowIronGranite(int amount) : base( CraftResource.ShadowIron, amount )
		{
		    Stackable = true;
		    Name = "shadow iron granite";
		}

		public ShadowIronGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "shadow iron granite";
		}
	}

	public class CopperGranite : BaseGranite
	{
        [Constructable]
		public CopperGranite() : this( 1 )
		{
		}

		[Constructable]
		public CopperGranite(int amount) : base( CraftResource.Copper, amount )
		{
		    Stackable = true;
		    Name = "copper granite";
		}

		public CopperGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "copper granite";
		}
	}

	public class BronzeGranite : BaseGranite
	{
        [Constructable]
		public BronzeGranite() : this( 1 )
		{
		}

		[Constructable]
		public BronzeGranite(int amount) : base( CraftResource.Bronze, amount )
		{
		    Stackable = true;
		    Name = "bronze granite";
		}

		public BronzeGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "bronze granite";
		}
	}

	public class GoldGranite : BaseGranite
	{
        [Constructable]
		public GoldGranite() : this( 1 )
		{
		}

		[Constructable]
		public GoldGranite(int amount) : base( CraftResource.Gold, amount )
		{
		    Stackable = true;
		    Name = "gold granite";
		}

		public GoldGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "gold granite";
		}
	}

	public class AgapiteGranite : BaseGranite
	{
        [Constructable]
		public AgapiteGranite() : this( 1 )
		{
		}

		[Constructable]
		public AgapiteGranite(int amount) : base( CraftResource.Agapite, amount )
		{
		    Stackable = true;
		    Name = "agapite granite";
		}

		public AgapiteGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "agapite granite";
		}
	}

	public class VeriteGranite : BaseGranite
	{
        [Constructable]
		public VeriteGranite() : this( 1 )
		{
		}

		[Constructable]
		public VeriteGranite(int amount) : base( CraftResource.Verite, amount )
		{
            Stackable = true;
		    Name = "verite granite";
		}

		public VeriteGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "verite granite";
		}
	}
    
    public class OldCopperGranite : BaseGranite
    {
        [Constructable]
		public OldCopperGranite() : this( 1 )
		{
		}

        [Constructable]
        public OldCopperGranite(int amount) : base(CraftResource.OldCopper, amount)
        {
            Stackable = true;
            Name = "old copper granite";
        }

        public OldCopperGranite(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "old copper granite";
        }
    }

    public class SilverGranite : BaseGranite
    {
        [Constructable]
		public SilverGranite() : this( 1 )
		{
		}

        [Constructable]
        public SilverGranite(int amount)
            : base(CraftResource.Silver, amount)
        {
            Stackable = true;
            Name = "silver granite";
        }

        public SilverGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "silver granite";
        }
    }

    public class RoseGranite : BaseGranite
    {
        [Constructable]
		public RoseGranite() : this( 1 )
		{
		}

        [Constructable]
        public RoseGranite(int amount)
            : base(CraftResource.Rose, amount)
        {
            Stackable = true;
            Name = "rose granite";
        }

        public RoseGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "rose granite";
        }
    }

	public class ValoriteGranite : BaseGranite
	{
        [Constructable]
		public ValoriteGranite() : this( 1 )
		{
		}

		[Constructable]
		public ValoriteGranite(int amount) : base( CraftResource.Valorite, amount )
		{
            Stackable = true;
		    Name = "valorite granite";
		}

		public ValoriteGranite( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
                Name = "valorite granite";
		}
	}

    public class BloodRockGranite : BaseGranite
    {
        [Constructable]
        public BloodRockGranite()
            : this(1)
        {
        }

        [Constructable]
        public BloodRockGranite(int amount)
            : base(CraftResource.BloodRock, amount)
        {
            Stackable = true;
            Name = "blood rock granite";
        }

        public BloodRockGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "blood rock granite";
        }
    }

    public class AquaGranite : BaseGranite
    {
        [Constructable]
        public AquaGranite()
            : this(1)
        {
        }

        [Constructable]
        public AquaGranite(int amount)
            : base(CraftResource.Aqua, amount)
        {
            Stackable = true;
            Name = "aqua granite";
        }

        public AquaGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "aqua granite";
        }
    }

    public class MytherilGranite : BaseGranite
    {
        [Constructable]
        public MytherilGranite()
            : this(1)
        {
        }

        [Constructable]
        public MytherilGranite(int amount)
            : base(CraftResource.Mytheril, amount)
        {
            Stackable = true;
            Name = "mytheril granite";
        }

        public MytherilGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "mytheril granite";
        }
    }

    public class AmethystGranite : BaseGranite
    {
        [Constructable]
        public AmethystGranite()
            : this(1)
        {
        }

        [Constructable]
        public AmethystGranite(int amount)
            : base(CraftResource.Amethyst, amount)
        {
            Stackable = true;
            Name = "amethyst granite";
        }

        public AmethystGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "amethyst granite";
        }
    }

    public class DwarvenGranite : BaseGranite
    {
        [Constructable]
        public DwarvenGranite()
            : this(1)
        {
        }

        [Constructable]
        public DwarvenGranite(int amount)
            : base(CraftResource.Dwarven, amount)
        {
            Stackable = true;
            Name = "dwarven granite";
        }

        public DwarvenGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "dwarven granite";
        }
    }

    public class IceGranite : BaseGranite
    {
        [Constructable]
        public IceGranite()
            : this(1)
        {
        }

        [Constructable]
        public IceGranite(int amount)
            : base(CraftResource.Ice, amount)
        {
            Stackable = true;
            Name = "ice granite";
        }

        public IceGranite(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Name = "ice granite";
        }
    }
}