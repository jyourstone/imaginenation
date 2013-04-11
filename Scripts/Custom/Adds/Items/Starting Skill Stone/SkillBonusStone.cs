using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    [Flipable(0xEDC, 0xEDB)]
    public class SkillBonusStone : Item
    {
        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile))
                return;
            PlayerMobile pl = (PlayerMobile)from;
            if (!from.InRange(GetWorldLocation(), 3) || !from.InLOS(this))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }
            if (pl.HasStartingSkillBoost)
            {
                pl.SendAsciiMessage("Can only be used once per character!");
                return;
            }
            List<int> myskills = new List<int>();
            SkillBonusGump.EnsureClosed(from);
            if (from.Skills.Total == 0)
                from.SendGump(new SkillBonusStoneInfoGump());
            else
                from.SendGump(new SkillBonusGump(from, myskills));
        }

        [Constructable]
        public SkillBonusStone() : base(0xEDC)
        {
            Name = "Skill Stone";
            Movable = false;
        }

        public SkillBonusStone(Serial serial)
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

            switch (version)
            {
                case 0:
                    {
                        break;
                    }
            }
        }
    }
}