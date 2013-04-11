namespace Server.Scripts.Custom.Adds.Quests.AlchemistsJob
{
    public class ConjuringPowder : Item
    {
        [Constructable]
        public ConjuringPowder() : this(1)
        {
        }

        [Constructable]
        public ConjuringPowder(int amount) : base(0xF8F)
        {
            Name = "Conjuring powder";
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
            Hue = 0x8AB;
        }

        public ConjuringPowder(Serial serial) : base(serial)
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
    }
}