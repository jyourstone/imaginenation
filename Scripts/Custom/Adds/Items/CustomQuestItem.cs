namespace Server.Items
{
    public class CustomQuestItem : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public string HiddenMessage { get; set; }

        [Constructable]
        public CustomQuestItem() : base(3982)
        {
            Name = "Quest item";
        }

        public CustomQuestItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            
            writer.Write(0);

            writer.Write(HiddenMessage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); 

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        HiddenMessage = reader.ReadString();
                        break;
                    }
            }
        }
    }
}