using System;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Misc
{
	public class FishingPoleTicket : Item
	{
		private FishingPoleTicket fpt;

		[Constructable]
		public FishingPoleTicket() : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 1165;
			Name = "Fishing pole claim ticket";
		}

		public FishingPoleTicket( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			fpt = this;
            if (!IsChildOf(from.Backpack) && !IsChildOf(from.BankBox))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from.SendGump(new FishingRewardGump()))
		        Delete();
		}
	}

	public class FishingRewardGump : Gump
	{
        public enum Buttons
        {
            colorCodeEntry,
            okButton,
            nameEntry
        }

		private readonly Mobile m_From;
		private readonly FishingPoleTicket fprg;
        private int m_CurrentHue;
        private string m_Name;
        private int m_Itemid;

        public FishingRewardGump(Mobile from, int currentHue, string name, int itemid, FishingPoleTicket fpt)
            : base(100, 100) 
        {
            m_From = from;
            fprg = fpt;
            m_CurrentHue = currentHue;
            m_Name = name;
            m_Itemid = itemid;
        }

            public FishingRewardGump()
                : base(0, 0)
            {
                Closable = false;
                Disposable = true;
                Dragable = true;
                Resizable = false;
                AddPage(0);
                AddImage(4, 16, 7, 911);
                AddItem(212, 38, 3520, m_CurrentHue);
                AddItem(222, 55, 3542);
                AddItem(46, 135, 6039);
                AddItem(50, 154, 5364);
                AddTextEntry(168, 163, 105, 20, 0, (int)Buttons.colorCodeEntry, Convert.ToString(m_CurrentHue));//0, ((int)Buttons.colorCodeEntry).ToString(), Convert.ToInt32(Convert.ToString(m_CurrentHue)));
                AddTextEntry(168, 139, 105, 20, 0, (int)Buttons.nameEntry, m_Name );//0, ((int)Buttons.nameEntry).ToString(), Convert.ToInt32(m_Name));
                AddButton(214, 188, 247, 248, (int)Buttons.okButton, GumpButtonType.Reply, 0);//0, GumpButtonType.Reply, 0); 
                AddLabel(123, 139, 5, @"Name:");
                AddLabel(131, 163, 5, @"Hue:");
                AddLabel(28, 42, 4, @"Fishing Pole Claim Ticket");
                AddLabel(34, 68, 5, @"For successfully ridding the seas");
                AddLabel(34, 84, 5, @"of countless sea serpents, this ticket");
                AddLabel(34, 102, 5, @"entitles you to a custom-made pole.");
                AddItem(68, 157, 6046);
                AddItem(68, 121, 6047);
                AddItem(23, 157, 6049);
                AddItem(24, 121, 6051);
                AddItem(90, 143, 6053);
                AddItem(46, 116, 6054);
                AddItem(45, 176, 6055);
                AddItem(3, 143, 6056);
                AddItem(38, 120, 9634);
                AddItem(23, 138, 8443, 2210);

            }
        //------------------------------------------------------------------------------------
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			try
                {
                    var hue = info.GetTextEntry((int)Buttons.colorCodeEntry).Text;

                    if (hue.StartsWith("0x"))
                        m_CurrentHue = Convert.ToInt32(hue, 16);
                    else
                        m_CurrentHue = Convert.ToInt32(hue);

                    var allowed = false;

                    //All hues from 2-1197 are always allowed
                    if (m_CurrentHue >= 2 && m_CurrentHue <= 1197)
                        allowed = true;
                    //Else check to see if it's an allowed color
                    else
                    {
                        for (var i = 0; i < AllowedColors.m_AllowedColors.Length; ++i)
                        {
                            if (m_CurrentHue != AllowedColors.m_AllowedColors[i])
                                continue;

                            allowed = true;
                            break;
                        }
                    }

                    if (!allowed)
                    {
                        sender.Mobile.SendMessage("Invalid Hue Selected:  That is a restricted color");
                        sender.Mobile.SendGump(new FishingRewardGump()); //m_From, 2, m_Name, m_Itemid, fprg
                        return;
                    }
                }
                catch
                {
                    sender.Mobile.SendMessage("Invalid Hue Selected:  Enter as an integer or hex number (0x)");
                    sender.Mobile.SendGump(new FishingRewardGump()); //m_From, 2, m_Name, m_Itemid, fprg
                    return;
                }

                m_Name = info.GetTextEntry((int)Buttons.nameEntry).Text;
                switch (info.ButtonID)
                {
                    case (int)Buttons.okButton:
                        {
                            Process(sender.Mobile);
                            sender.Mobile.CloseGump(typeof(FishingRewardGump));
                            break;
                        }

                    default:
                        {
                            sender.Mobile.SendGump(new FishingRewardGump(m_From, m_CurrentHue, m_Name, m_Itemid, fprg));
                            break;
                        }
                }
            }
            private void Process(Mobile from)
            {
                Item rewardItem;
                {
                    rewardItem = new DeepSeaFishingPole();
                }
                rewardItem.Hue = m_CurrentHue;
                rewardItem.LootType = LootType.Blessed;

                if (m_Name != "")
                    rewardItem.Name = m_Name;

                from.AddToBackpack(rewardItem);
                //fprg.Delete();
            }
	}
}
