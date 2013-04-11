using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    [Flipable(0x67A, 0x676)]
    public class CasinoDoor : Item
    {
        [Constructable]
        public CasinoDoor()
            : base(0x67A)
        {
            Movable = false;
            Hue = 1173;
            Name = "Casino door";
        }

        public CasinoDoor(Serial serial)
            : base(serial)
        {
        }


        public static bool IsInGuild(Mobile m)
        {
            return (m is PlayerMobile && ((PlayerMobile)m).NpcGuild == NpcGuild.ThievesGuild);
        }

        public override void OnDoubleClick(Mobile from)
        {
            Container pack = from.Backpack;
            if (from.InRange(this, 1))
            {
                if (pack != null && pack.ConsumeTotal(typeof(CasinoMembershipCard), 1))
                {
                    from.SendAsciiMessage("Welcome to the V.I.P area.");
                    from.PlaySound(510);
                    from.MoveToWorld(new Point3D(1340, 1717, 20), Map.Felucca);
                    from.Backpack.DropItem(new CasinoMembershipCard());
                }
                else
                {
                    from.SendAsciiMessage("You need to have a:");
                    from.SendAsciiMessage("Casino Membership Card to get pass.");
                }
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
        }

        public override void Serialize(GenericWriter writer)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
        }
    }
}