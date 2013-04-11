namespace Server.Items
{
    public class JeweledShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 25; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 105; } }
        public override int InitMaxHits { get { return 130; } }

        public override int AosStrReq { get { return 35; } }

        [Constructable]
        public JeweledShield()
            : base(0x3BB4)
        {
            Weight = 10.0;
            Name = "Jeweled Shield";
            BaseArmorRating = 18;
            Layer = Layer.TwoHanded;
        }

        public JeweledShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);//version
        }
    }

    public class KnightsShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 25; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 105; } }
        public override int InitMaxHits { get { return 130; } }

        public override int AosStrReq { get { return 35; } }

        [Constructable]
        public KnightsShield()
            : base(0x3BB5)
        {
            Weight = 10.0;
            Name = "Knight's Shield";
            BaseArmorRating = 16;
            Layer = Layer.TwoHanded;
        }

        public KnightsShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);//version
        }
    }

    public class BronzedShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 25; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 105; } }
        public override int InitMaxHits { get { return 130; } }

        public override int AosStrReq { get { return 35; } }

        [Constructable]
        public BronzedShield()
            : base(0x3BB6)
        {
            Weight = 10.0;
            Name = "Bronzed Shield";
            BaseArmorRating = 16;
            Layer = Layer.TwoHanded;
        }

        public BronzedShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);//version
        }
    }

    public class LeafShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 25; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 105; } }
        public override int InitMaxHits { get { return 130; } }

        public override int AosStrReq { get { return 35; } }

        [Constructable]
        public LeafShield()
            : base(0x3BB7)
        {
            Weight = 10.0;
            Name = "Leaf Shield";
            BaseArmorRating = 28; //doesnt get bonus for resource so set a flat amount
            Layer = Layer.TwoHanded;
        }

        public LeafShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);//version
        }
    }

    public class CalvaryShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 25; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 105; } }
        public override int InitMaxHits { get { return 130; } }

        public override int AosStrReq { get { return 35; } }

        [Constructable]
        public CalvaryShield()
            : base(0x3BB8)
        {
            Weight = 10.0;
            Name = "Calvary Shield";
            BaseArmorRating = 16;
            Layer = Layer.TwoHanded;
        }

        public CalvaryShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);//version
        }
    }
}