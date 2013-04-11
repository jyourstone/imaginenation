//Created by Shade 
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class PerformersHarpAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new PerformersHarpAddonDeed();
			}
		}

		[ Constructable ]
		public PerformersHarpAddon()
		{
			AddonComponent ac;
			ac = new AddonComponent( 1819 );
			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 3774 );
			AddComponent( ac, -1, 0, 7 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 3768 );
			ac.Hue = 2943;
			AddComponent( ac, -1, 0, 6 );
			ac = new AddonComponent( 1803 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 1819 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 1803 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 1811 );
			AddComponent( ac, 2, 2, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 0, 0, 3 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 1802 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 1802 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 2602 );
			ac.Hue = 2943;
			AddComponent( ac, 0, 1, 5 );
			ac = new AddonComponent( 3761 );
			ac.Hue = 2213;
            ac.Name = "Performer's Harp";
			AddComponent( ac, 0, 0, 11 );

		}

		public PerformersHarpAddon( Serial serial ) : base( serial )
		{
		}

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

            else
            {
                if (ac.ItemID == 3774)
                {
                    from.SendGump(new StageHarpGump());
                }
                else
                    return;
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class PerformersHarpAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new PerformersHarpAddon();
			}
		}

		[Constructable]
		public PerformersHarpAddonDeed()
		{
			Name = "Performer's Harp";
		}

		public PerformersHarpAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
namespace Server.Gumps
{
    public class StageHarpGump : Gump
    {
        public StageHarpGump()
            : base(0, 0)
        {

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(6, 15, 570, 140, 5054);
            AddAlphaRegion(16, 20, 550, 130);
            AddImageTiled(16, 20, 550, 20, 9354);
            AddLabel(19, 20, 200, "Harp Strings");
            AddLabel(55, 60, 0, @"C");
            AddLabel(55, 80, 0, @"C");
            AddLabel(55, 100, 0, @"C");
            AddLabel(95, 60, 0, @"C#");
            AddLabel(95, 80, 0, @"C#");
            AddLabel(145, 60, 0, @"D");
            AddLabel(145, 80, 0, @"D");
            AddLabel(185, 60, 0, @"D#");
            AddLabel(185, 80, 0, @"D#");
            AddLabel(235, 60, 0, @"E");
            AddLabel(235, 80, 0, @"E");
            AddLabel(275, 60, 0, @"F");
            AddLabel(275, 80, 0, @"F");
            AddLabel(315, 60, 0, @"F#");
            AddLabel(315, 80, 0, @"F#");
            AddLabel(365, 60, 0, @"G");
            AddLabel(365, 80, 0, @"G");
            AddLabel(405, 60, 0, @"G#");
            AddLabel(405, 80, 0, @"G#");
            AddLabel(455, 60, 0, @"A");
            AddLabel(455, 80, 0, @"A");
            AddLabel(495, 60, 0, @"A#");
            AddLabel(495, 80, 0, @"A#");
            AddLabel(545, 60, 0, @"B");
            AddLabel(545, 80, 0, @"B");
            AddButton(35, 62, 5601, 5605, 1, GumpButtonType.Reply, 0);
            AddButton(35, 82, 5601, 5605, 2, GumpButtonType.Reply, 0);
            AddButton(35, 102, 5601, 5605, 3, GumpButtonType.Reply, 0);
            AddButton(75, 62, 5601, 5605, 4, GumpButtonType.Reply, 0);
            AddButton(75, 82, 5601, 5605, 5, GumpButtonType.Reply, 0);
            AddButton(125, 62, 5601, 5605, 6, GumpButtonType.Reply, 0);
            AddButton(125, 82, 5601, 5605, 7, GumpButtonType.Reply, 0);
            AddButton(165, 62, 5601, 5605, 8, GumpButtonType.Reply, 0);
            AddButton(165, 82, 5601, 5605, 9, GumpButtonType.Reply, 0);
            AddButton(215, 62, 5601, 5605, 10, GumpButtonType.Reply, 0);
            AddButton(215, 82, 5601, 5605, 11, GumpButtonType.Reply, 0);
            AddButton(255, 62, 5601, 5605, 12, GumpButtonType.Reply, 0);
            AddButton(255, 82, 5601, 5605, 13, GumpButtonType.Reply, 0);
            AddButton(295, 62, 5601, 5605, 14, GumpButtonType.Reply, 0);
            AddButton(295, 82, 5601, 5605, 15, GumpButtonType.Reply, 0);
            AddButton(345, 62, 5601, 5605, 16, GumpButtonType.Reply, 0);
            AddButton(345, 82, 5601, 5605, 17, GumpButtonType.Reply, 0);
            AddButton(385, 62, 5601, 5605, 18, GumpButtonType.Reply, 0);
            AddButton(385, 82, 5601, 5605, 19, GumpButtonType.Reply, 0);
            AddButton(435, 62, 5601, 5605, 20, GumpButtonType.Reply, 0);
            AddButton(435, 82, 5601, 5605, 21, GumpButtonType.Reply, 0);
            AddButton(475, 62, 5601, 5605, 22, GumpButtonType.Reply, 0);
            AddButton(475, 82, 5601, 5605, 23, GumpButtonType.Reply, 0);
            AddButton(525, 62, 5601, 5605, 24, GumpButtonType.Reply, 0);
            AddButton(525, 82, 5601, 5605, 25, GumpButtonType.Reply, 0);
            AddButton(425, 120, 241, 242, 26, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            switch (info.ButtonID)
            {
                case 0: { m.SendMessage(60, "You stop playing."); break; }

                case 1: { m.PlaySound(1181); m.SendGump(new StageHarpGump()); break; } //do

                case 2: { m.PlaySound(1182); m.SendGump(new StageHarpGump()); break; } //do2

                case 3: { m.PlaySound(1183); m.SendGump(new StageHarpGump()); break; } //do3

                case 4: { m.PlaySound(1184); m.SendGump(new StageHarpGump()); break; } //do#

                case 5: { m.PlaySound(1185); m.SendGump(new StageHarpGump()); break; } //do#2

                case 6: { m.PlaySound(1186); m.SendGump(new StageHarpGump()); break; } //re

                case 7: { m.PlaySound(1187); m.SendGump(new StageHarpGump()); break; } //re2

                case 8: { m.PlaySound(1188); m.SendGump(new StageHarpGump()); break; } //re#

                case 9: { m.PlaySound(1189); m.SendGump(new StageHarpGump()); break; } //re#2

                case 10: { m.PlaySound(1190); m.SendGump(new StageHarpGump()); break; } //mi

                case 11: { m.PlaySound(1191); m.SendGump(new StageHarpGump()); break; } //mi2

                case 12: { m.PlaySound(1192); m.SendGump(new StageHarpGump()); break; } //fa

                case 13: { m.PlaySound(1193); m.SendGump(new StageHarpGump()); break; } //fa2

                case 14: { m.PlaySound(1194); m.SendGump(new StageHarpGump()); break; } //fa#

                case 15: { m.PlaySound(1195); m.SendGump(new StageHarpGump()); break; } //fa#2

                case 16: { m.PlaySound(1196); m.SendGump(new StageHarpGump()); break; } //so

                case 17: { m.PlaySound(1197); m.SendGump(new StageHarpGump()); break; } //so2

                case 18: { m.PlaySound(1198); m.SendGump(new StageHarpGump()); break; } //so#

                case 19: { m.PlaySound(1199); m.SendGump(new StageHarpGump()); break; } //so#2

                case 20: { m.PlaySound(1175); m.SendGump(new StageHarpGump()); break; } //la

                case 21: { m.PlaySound(1176); m.SendGump(new StageHarpGump()); break; } //la2

                case 22: { m.PlaySound(1177); m.SendGump(new StageHarpGump()); break; } //la#

                case 23: { m.PlaySound(1178); m.SendGump(new StageHarpGump()); break; } //la#2

                case 24: { m.PlaySound(1179); m.SendGump(new StageHarpGump()); break; } //ti

                case 25: { m.PlaySound(1180); m.SendGump(new StageHarpGump()); break; } //ti2

                case 26: { m.SendMessage(60, "You stop playing."); break; }

            }
        }
    }
}