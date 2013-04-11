namespace Server.Items
{
	[TypeAlias( "Server.Custom.Items.AquaIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class AquaIngot : BaseIngot
	{
		[Constructable]
		public AquaIngot() : this( 1 )
		{
		}

		[Constructable]
		public AquaIngot( int amount ) : base( CraftResource.Aqua, amount )
		{
			Name = "aqua ingot";
		}

		public AquaIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.OldCopperIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class OldCopperIngot : BaseIngot
	{
		[Constructable]
		public OldCopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public OldCopperIngot( int amount ) : base( CraftResource.OldCopper, amount )
		{
			Name = "old copper ingot";
		}

		public OldCopperIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.RoseIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class RoseIngot : BaseIngot
	{
		[Constructable]
		public RoseIngot() : this( 1 )
		{
		}

		[Constructable]
		public RoseIngot( int amount ) : base( CraftResource.Rose, amount )
		{
			Name = "rose ingot";
		}

		public RoseIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.OceanicIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class OceanicIngot : BaseIngot
	{
		[Constructable]
		public OceanicIngot() : this( 1 )
		{
		}

		[Constructable]
		public OceanicIngot( int amount ) : base( CraftResource.Oceanic, amount )
		{
			Name = "oceanic ingot";
		}

		public OceanicIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.MytherilIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class MytherilIngot : BaseIngot
	{
		[Constructable]
		public MytherilIngot() : this( 1 )
		{
		}

		[Constructable]
		public MytherilIngot( int amount ) : base( CraftResource.Mytheril, amount )
		{
			Name = "mytheril ingot";
		}

		public MytherilIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.AdamantiumIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class AdamantiumIngot : BaseIngot
	{
		[Constructable]
		public AdamantiumIngot() : this( 1 )
		{
		}

		[Constructable]
		public AdamantiumIngot( int amount ) : base( CraftResource.Adamantium, amount )
		{
			Name = "adamantium ingot";
		}

		public AdamantiumIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.BlackDiamondIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class BlackDiamondIngot : BaseIngot
	{
		[Constructable]
		public BlackDiamondIngot() : this( 1 )
		{
		}

		[Constructable]
		public BlackDiamondIngot( int amount ) : base( CraftResource.BlackDiamond, amount )
		{
			Name = "black diamond ingot";
		}

		public BlackDiamondIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.SilverIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class SilverIngot : BaseIngot
	{
		[Constructable]
		public SilverIngot() : this( 1 )
		{
		}

		[Constructable]
		public SilverIngot( int amount ) : base( CraftResource.Silver, amount )
		{
			Name = "silver ingot";
		}

		public SilverIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.IceIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class IceIngot : BaseIngot
	{
		[Constructable]
		public IceIngot() : this( 1 )
		{
		}

		[Constructable]
		public IceIngot( int amount ) : base( CraftResource.Ice, amount )
		{
			Name = "ice ingot";
		}

		public IceIngot( Serial serial ) : base( serial )
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

    [TypeAlias("Server.Custom.Items.HavocIngot")]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
    public class HavocIngot : BaseIngot
    {
        [Constructable]
        public HavocIngot()
            : this(1)
        {
        }

        [Constructable]
        public HavocIngot(int amount)
            : base(CraftResource.Havoc, amount)
        {
            Name = "havoc ingot";
        }

        public HavocIngot(Serial serial)
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

	[TypeAlias( "Server.Custom.Items.BloodRockIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class BloodRockIngot : BaseIngot
	{
		[Constructable]
		public BloodRockIngot() : this( 1 )
		{
		}

		[Constructable]
		public BloodRockIngot( int amount ) : base( CraftResource.BloodRock, amount )
		{
			Name = "blood rock ingot";
		}

		public BloodRockIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.OpiateIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class OpiateIngot : BaseIngot
	{
		[Constructable]
		public OpiateIngot() : this( 1 )
		{
		}

		[Constructable]
		public OpiateIngot( int amount ) : base( CraftResource.Opiate, amount )
		{
			Name = "opiate ingot";
		}

		public OpiateIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.SandRockIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class SandRockIngot : BaseIngot
	{
		[Constructable]
		public SandRockIngot() : this( 1 )
		{
		}

		[Constructable]
		public SandRockIngot( int amount ) : base( CraftResource.SandRock, amount )
		{
			Name = "sand rock ingot";
		}

		public SandRockIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.BlackRockIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class BlackRockIngot : BaseIngot
	{
		[Constructable]
		public BlackRockIngot() : this( 1 )
		{
		}

		[Constructable]
		public BlackRockIngot( int amount ) : base( CraftResource.BlackRock, amount )
		{
			Name = "black rock ingot";
		}

		public BlackRockIngot( Serial serial ) : base( serial )
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

	[TypeAlias( "Server.Custom.Items.DaemonSteelIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class DaemonSteelIngot : BaseIngot
	{
		[Constructable]
		public DaemonSteelIngot() : this( 1 )
		{
		}

		[Constructable]
		public DaemonSteelIngot( int amount ) : base( CraftResource.DaemonSteel, amount )
		{
			Name = "daemon steel ingot";
		}

		public DaemonSteelIngot( Serial serial ) : base( serial )
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

    [TypeAlias("Server.Custom.Items.FireIngot")]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
    public class FireIngot : BaseIngot
    {
        [Constructable]
        public FireIngot()
            : this(1)
        {
        }

        [Constructable]
        public FireIngot(int amount)
            : base(CraftResource.Fire, amount)
        {
            Name = "fire ingot";
        }

        public FireIngot(Serial serial)
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

    [TypeAlias("Server.Custom.Items.ReactiveIngot")]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
    public class ReactiveIngot : BaseIngot
    {
        [Constructable]
        public ReactiveIngot()
            : this(1)
        {
        }

        [Constructable]
        public ReactiveIngot(int amount)
            : base(CraftResource.Reactive, amount)
        {
            Name = "reactive ingot";
        }

        public ReactiveIngot(Serial serial)
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

	[TypeAlias( "Server.Custom.Items.SapphireIngot" )]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
	public class SapphireIngot : BaseIngot
	{
		[Constructable]
		public SapphireIngot() : this( 1 )
		{
		}

		[Constructable]
		public SapphireIngot( int amount ) : base( CraftResource.Sapphire, amount )
		{
			Name = "sapphire ingot";
		}

		public SapphireIngot( Serial serial ) : base( serial )
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

    [TypeAlias("Server.Custom.Items.DwarvenIngot")]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
    public class DwarvenIngot : BaseIngot
    {
        [Constructable]
        public DwarvenIngot()
            : this(1)
        {
        }

        [Constructable]
        public DwarvenIngot(int amount)
            : base(CraftResource.Dwarven, amount)
        {
            Name = "dwarven ingot";
        }

        public DwarvenIngot(Serial serial)
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

    [TypeAlias("Server.Custom.Items.AmethystIngot")]
    [Flipable(7151, 7152, 7153, 7154, 7155, 7156)]
    public class AmethystIngot : BaseIngot
    {
        [Constructable]
        public AmethystIngot()
            : this(1)
        {
        }

        [Constructable]
        public AmethystIngot(int amount) : base(CraftResource.Amethyst, amount)
        {
            Name = "amethyst ingot";
        }

        public AmethystIngot(Serial serial)
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