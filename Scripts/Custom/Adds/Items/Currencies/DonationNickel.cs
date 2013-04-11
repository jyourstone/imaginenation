namespace Server.Items
{
    public class DonationNickel : Item
    {

        [Constructable]
        public DonationNickel()
            : this(1)
        {
        }

        [Constructable]
        public DonationNickel(int amountFrom, int amountTo)
            : this(Utility.RandomMinMax(amountFrom, amountTo))
        {
        }

        [Constructable]
        public DonationNickel(int amount)
            : base(0xEF0)
        {
            Stackable = true;
            Amount = amount;
            Hue = 2643;
            Name = "Donation Nickel";
            LootType = LootType.Blessed;
            DonationItem = true;
            Weight = 0;
        }

        public DonationNickel(Serial serial)
            : base(serial)
        {
        }

        public override int GetDropSound()
        {
            if (Amount <= 1)
                return 0x2E4;
            if (Amount <= 5)
                return 0x2E5;
            return 0x2E6;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}