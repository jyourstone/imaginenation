using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class KarmaResetBall : Item
    {
        [Constructable]
        public KarmaResetBall() : base(6251)
        {
            Name = "Karma Reset ball";
            Hue = 2567;
        }

        public KarmaResetBall(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (IsChildOf(m.Backpack))
            {
                if (m.Karma != 0)
                {
                    m.BoltEffect(0);
                    m.Karma = 0;
                    m.SendAsciiMessage("Your karma has been reset!");
                    m.SendMessage("Your new karma is: {0}", m.Karma);
                    Consume();

                }
                else
                    m.SendMessage("You already have 0 karma!");
            }
            else
                m.SendLocalizedMessage(1060640); // "This item must be in your backpack to use it"
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