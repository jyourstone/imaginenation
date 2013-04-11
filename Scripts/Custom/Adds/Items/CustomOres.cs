namespace Server.Items
{
	[TypeAlias( "Server.Custom.Items.AquaOre" )]
	public class AquaOre : BaseOre
	{
		[Constructable]
		public AquaOre() : this( 1 )
		{
		}

		[Constructable]
		public AquaOre( int amount ) : base( CraftResource.Aqua, amount )
		{
			Name = "Aqua Ore";
		}

		public AquaOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new AquaIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.RoseOre" )]
	public class RoseOre : BaseOre
	{
		[Constructable]
		public RoseOre() : this( 1 )
		{
		}

		[Constructable]
		public RoseOre( int amount ) : base( CraftResource.Rose, amount )
		{
			Name = "Rose Ore";
		}

		public RoseOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new RoseIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.OceanicOre" )]
	public class OceanicOre : BaseOre
	{
		[Constructable]
		public OceanicOre() : this( 1 )
		{
		}

		[Constructable]
		public OceanicOre( int amount ) : base( CraftResource.Oceanic, amount )
		{
			Name = "Oceanic Ore";
		}

		public OceanicOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new OceanicIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.OldCopperOre" )]
	public class OldCopperOre : BaseOre
	{
		[Constructable]
		public OldCopperOre() : this( 1 )
		{
		}

		[Constructable]
		public OldCopperOre( int amount ) : base( CraftResource.OldCopper, amount )
		{
			Name = "Old Copper Ore";
		}

		public OldCopperOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new OldCopperIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.MytherilOre" )]
	public class MytherilOre : BaseOre
	{
		[Constructable]
		public MytherilOre() : this( 1 )
		{
		}

		[Constructable]
		public MytherilOre( int amount ) : base( CraftResource.Mytheril, amount )
		{
			Name = "Mytheril Ore";
		}

		public MytherilOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new MytherilIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.BlackDiamondOre" )]
	public class BlackDiamondOre : BaseOre
	{
		[Constructable]
		public BlackDiamondOre() : this( 1 )
		{
		}

		[Constructable]
		public BlackDiamondOre( int amount ) : base( CraftResource.BlackDiamond, amount )
		{
			Name = "black diamond ore";
		}

		public BlackDiamondOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new BlackDiamondIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.AdamantiumOre" )]
	public class AdamantiumOre : BaseOre
	{
		[Constructable]
		public AdamantiumOre() : this( 1 )
		{
		}

		[Constructable]
		public AdamantiumOre( int amount ) : base( CraftResource.Adamantium, amount )
		{
			Name = "adamantium ore";
		}

		public AdamantiumOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new AdamantiumIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.SilverOre" )]
	public class SilverOre : BaseOre
	{
		[Constructable]
		public SilverOre() : this( 1 )
		{
		}

		[Constructable]
		public SilverOre( int amount ) : base( CraftResource.Silver, amount )
		{
			Name = "silver ore";
		}

		public SilverOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new SilverIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.IceOre" )]
	public class IceOre : BaseOre
	{
		[Constructable]
		public IceOre() : this( 1 )
		{
		}

		[Constructable]
		public IceOre( int amount ) : base( CraftResource.Ice, amount )
		{
			Name = "Ice Ore";
		}

		public IceOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new IceIngot();
		}
	}

    [TypeAlias("Server.Custom.Items.AmethystOre")]
    public class AmethystOre : BaseOre
    {
        [Constructable]
        public AmethystOre()
            : this(1)
        {
            Name = "Amethyst Ore";
        }

        [Constructable]
        public AmethystOre(int amount)
            : base(CraftResource.Amethyst, amount)
        {
        }

        public AmethystOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new AmethystIngot();
        }
    }

    [TypeAlias("Server.Custom.Items.HavocOre")]
    public class HavocOre : BaseOre
    {
        [Constructable]
        public HavocOre()
            : this(1)
        {
        }

        [Constructable]
        public HavocOre(int amount)
            : base(CraftResource.Havoc, amount)
        {
            Name = "Havoc Ore";
        }

        public HavocOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new HavocIngot();
        }
    }

	[TypeAlias( "Server.Custom.Items.BloodRockOre" )]
	public class BloodRockOre : BaseOre
	{
		[Constructable]
		public BloodRockOre() : this( 1 )
		{
		}

		[Constructable]
		public BloodRockOre( int amount ) : base( CraftResource.BloodRock, amount )
		{
			Name = "blood rock ore";
		}

		public BloodRockOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new BloodRockIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.OpiateOre" )]
	public class OpiateOre : BaseOre
	{
		[Constructable]
		public OpiateOre() : this( 1 )
		{
		}

		[Constructable]
		public OpiateOre( int amount ) : base( CraftResource.Opiate, amount )
		{
			Name = "opiate ore";
		}

		public OpiateOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new OpiateIngot();
		}
	}

    [TypeAlias("Server.Custom.Items.DwarvenOre")]
    public class DwarvenOre : BaseOre
    {
        [Constructable]
        public DwarvenOre()
            : this(1)
        {
        }

        [Constructable]
        public DwarvenOre(int amount)
            : base(CraftResource.Dwarven, amount)
        {
            Name = "Dwarven Ore";
        }

        public DwarvenOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new DwarvenIngot();
        }
    }

	[TypeAlias( "Server.Custom.Items.SandRockOre" )]
	public class SandRockOre : BaseOre
	{
		[Constructable]
		public SandRockOre() : this( 1 )
		{
			Name = "sand rock ore";
		}

		[Constructable]
		public SandRockOre( int amount ) : base( CraftResource.SandRock, amount )
		{
		}

		public SandRockOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new SandRockIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.BlackRockOre" )]
	public class BlackRockOre : BaseOre
	{
		[Constructable]
		public BlackRockOre() : this( 1 )
		{
		}

		[Constructable]
		public BlackRockOre( int amount ) : base( CraftResource.BlackRock, amount )
		{
			Name = "black rock ore";
		}

		public BlackRockOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new BlackRockIngot();
		}
	}

	[TypeAlias( "Server.Custom.Items.DaemonSteelOre" )]
	public class DaemonSteelOre : BaseOre
	{
		[Constructable]
		public DaemonSteelOre() : this( 1 )
		{
		}

		[Constructable]
		public DaemonSteelOre( int amount ) : base( CraftResource.DaemonSteel, amount )
		{
			Name = "Daemon Steel Ore";
		}

		public DaemonSteelOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new DaemonSteelIngot();
		}
	}
    [TypeAlias("Server.Custom.Items.FireOre")]
    public class FireOre : BaseOre
    {
        [Constructable]
        public FireOre()
            : this(1)
        {
        }

        [Constructable]
        public FireOre(int amount)
            : base(CraftResource.Fire, amount)
        {
            Name = "Fire Ore";
        }

        public FireOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new FireIngot();
        }
    }

    [TypeAlias("Server.Custom.Items.ReactiveOre")]
    public class ReactiveOre : BaseOre
    {
        [Constructable]
        public ReactiveOre()
            : this(1)
        {
        }

        [Constructable]
        public ReactiveOre(int amount)
            : base(CraftResource.Reactive, amount)
        {
            Name = "Reactive Ore";
        }

        public ReactiveOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new ReactiveIngot();
        }
    }

	[TypeAlias( "Server.Custom.Items.SapphireOre" )]
	public class SapphireOre : BaseOre
	{
		[Constructable]
		public SapphireOre() : this( 1 )
		{
		}

		[Constructable]
		public SapphireOre( int amount ) : base( CraftResource.Sapphire, amount )
		{
			Name = "Sapphire Ore";
		}

		public SapphireOre( Serial serial ) : base( serial )
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

		public override BaseIngot GetIngot()
		{
			return new SapphireIngot();
		}
	}
}