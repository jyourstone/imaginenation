using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    class StartingTeleporter : Teleporter
    {
        public override bool OnMoveOver(Mobile m)
        {
            if (!(m is PlayerMobile))
                return false;
            PlayerMobile pl = (PlayerMobile)m;

            if (Active)
            {
                if (pl.HasStartingSkillBoost)
                {
                    StartTeleport(m);
                    return false;
                }
                else
                    pl.SendGump(new StartingConfirmationGump(m, this));
            }
            return true;
        }

        [Constructable]
		public StartingTeleporter()
		{
		}

        public StartingTeleporter(Serial serial)
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

namespace Server.Gumps
{
    public class StartingConfirmationGump : Gump
    {
        readonly Teleporter tele;
        public StartingConfirmationGump(Mobile m, Teleporter t) : base(100,100)
        {
            m.CloseAllGumps();
            tele = t;
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(0, 0, 387, 103, 9200);
			AddHtml( 8, 8, 371, 62, @"You have not chosen your starting skills using the skill stone. Are you sure you want to enter the world without doing so?", true, false);
            AddButton(208, 72, 247, 248, (int)Buttons.Button2, GumpButtonType.Reply, 0);
            AddButton(120, 72, 242, 241, (int)Buttons.Button1, GumpButtonType.Reply, 0);
		}
		
		public enum Buttons
		{
			Button1,
			Button2,
		}

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (tele == null || tele.Deleted)
                return;
            if (info.ButtonID == 1)
                tele.StartTeleport(sender.Mobile);
        }
    }
}
