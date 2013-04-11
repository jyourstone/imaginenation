using System;
using Server.IN.Logging;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class RewardGump : Gump
	{
		public enum Buttons
		{
			colorCodeEntry,
			upButton,
			downButton,
			applyButton,
			okButton,
			nameEntry
		}

		private readonly SkillInfo m_Skill;
		private int m_CurrentHue;
		private string m_Name;
	    private int m_Itemid;

        public RewardGump(SkillInfo skill, int currentHue, string name) : this(skill, currentHue, name, -1)
        {
        }

	    public RewardGump( SkillInfo skill, int currentHue, string name, int itemid ) : base( 100, 100 )
		{
			m_Skill = skill;
			m_CurrentHue = currentHue;
			m_Name = name;
	        m_Itemid = itemid;


			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
            AddPage(0);
            AddImageTiled(126, 125, 200, 227, 3504);
            AddImageTiled(318, 125, 24, 227, 3505);
            AddImageTiled(105, 125, 23, 227, 3503);
            AddImageTiled(126, 102, 195, 24, 3501);
            AddImageTiled(119, 342, 212, 26, 3507);
            AddImage(105, 102, 3500);
            AddImage(318, 102, 3502);
            AddImage(105, 342, 3506);
            AddImage(318, 342, 3508);
            AddImageTiled(151, 297, 146, 25, 3507);
            AddImage(296, 276, 3502);
            AddImage(296, 297, 3508);
            AddButton(126, 205, 5600, 5604, (int)Buttons.upButton, GumpButtonType.Reply, 0);
            AddButton(126, 225, 5602, 5606, (int)Buttons.downButton, GumpButtonType.Reply, 0);
            AddImageTiled(140, 276, 159, 14, 3501);
            AddImage(126, 276, 3500);
            AddImage(126, 297, 3506);
            AddImageTiled(151, 290, 148, 8, 3504);
            AddButton(161, 333, 2124, 2123, (int)Buttons.applyButton, GumpButtonType.Reply, 0);
            AddButton(237, 333, 2130, 2129, (int)Buttons.okButton, GumpButtonType.Reply, 0);
            AddLabel(150, 121, 347, GetSkillTitle());
            AddLabel(128, 160, 347, @"Congrats! Enter a colour code:");
            AddLabel(147, 263, 347, @"Enter a name in the box:");
            AddImage(311, 113, 2530);
            AddImage(121, 325, 2530);
            AddImage(145, 200, 3500);
            AddImageTiled(170, 220, 55, 26, 3507);
            AddImageTiled(170, 200, 55, 26, 3501);
            AddImage(145, 220, 3506);
            AddImage(218, 220, 3508);
            AddImage(218, 200, 3502);
            AddItem( 268, 211, GetItemId(), m_CurrentHue );
            AddTextEntry( 154, 291, 136, 19, 0, (int)Buttons.nameEntry, m_Name );
            AddTextEntry( 172, 214, 40, 19, 0, (int)Buttons.colorCodeEntry, Convert.ToString( m_CurrentHue ) );
		}

		private int GetItemId()
		{
            if (m_Itemid > 0)
                return m_Itemid;

		    switch (m_Skill.SkillID)
		    {
		        case 0: // alchemy
		            m_Itemid = 0xE9B;
		            break;
		        case 7: // Blacksmithy
		            m_Itemid = 0x13E3;
		            break;
		        case 8: // Bowcraft/fletching
		            m_Itemid = 0x13B1;
		            break;
		        case 23: // Inscription
		            m_Itemid = 0xFC0;
		            break;
		        case 25: // Magery
		            m_Itemid = 0xE3B;
		            break;
		        case 34: // Tailoring
		            m_Itemid = 0xFAB;
		            break;
		        case 35: // Animal Taming
		            m_Itemid = 0xE82;
		            break;
                case 44: // Lumberjack
		            m_Itemid = 0xF44;
		            break;
		        case 45: // Mining
		            m_Itemid = 0xE86;
		            break;
                //Hiding, stealth, snooping, stealing
                case 21:
                case 28:
                case 33:
                case 47:
		            m_Itemid = 5397;
		            break;
		    }

		    return m_Itemid;
		}

		private string GetSkillTitle()
		{
			string Title = null;

			switch( m_Skill.SkillID )
			{
				case 0: // alchemy
					Title = "GM Alchemy Reward";
					break;
				case 7: // Blacksmithy
					Title = "GM Blacksmith Reward";
					break;
				case 8: // Bowcraft/fletching
					Title = "GM Bowcraft Reward";
					break;
				case 11: // Carpentry
					Title = "GM Carpentry Reward";
					break;
				case 23: // Inscription
					Title = "GM Inscription Reward";
					break;
				case 25: // Magery
					Title = "GM Magery Reward";
					break;
                case 29: // Musicianship
			        Title = "GM Bard Reward";
			        break;
				case 34: // Tailoring
					Title = "GM Tailoring Reward";
					break;
				case 35: // Animal Taming
					Title = "GM Animal Taming Reward";
					break;
                case 44: // Lumberjack
			        Title = "GM Lumberjacking Reward";
			        break;
                case 45: // Mining
                    Title = "GM Mining Reward";
                    break;
                //"Thieving" skills
                case 21:
                case 28:
                case 33:
                case 47:
			        Title = "GM Thieving reward";
			        break;
			}

			return Title;
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			try
			{
				var hue = info.GetTextEntry( (int)Buttons.colorCodeEntry ).Text;

				if( hue.StartsWith( "0x" ) )
					m_CurrentHue = Convert.ToInt32( hue, 16 );
				else
					m_CurrentHue = Convert.ToInt32( hue );

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
                    sender.Mobile.SendGump(new RewardGump(m_Skill, 2, m_Name, m_Itemid));
                    return;
                }
			}
			catch
			{
				sender.Mobile.SendMessage( "Invalid Hue Selected:  Enter as an integer or hex number (0x)" );
                sender.Mobile.SendGump(new RewardGump(m_Skill, 2, m_Name, m_Itemid));
				return;
			}

			m_Name = info.GetTextEntry( (int)Buttons.nameEntry ).Text;

			switch( info.ButtonID )
			{
				case (int)Buttons.upButton:
				{
                    sender.Mobile.SendGump(new RewardGump(m_Skill, ++m_CurrentHue, m_Name, m_Itemid));
					break;
				}
				case (int)Buttons.downButton:
				{
                    sender.Mobile.SendGump(new RewardGump(m_Skill, --m_CurrentHue, m_Name, m_Itemid));
					break;
				}
				case (int)Buttons.applyButton:
				{
                    sender.Mobile.SendGump(new RewardGump(m_Skill, m_CurrentHue, m_Name, m_Itemid));
					break;
				}
				case (int)Buttons.okButton:
				{
					Process( sender.Mobile );
					sender.Mobile.CloseGump( typeof( RewardGump ) );
					break;
				}
				default:
				{
                    sender.Mobile.SendGump(new RewardGump(m_Skill, m_CurrentHue, m_Name, m_Itemid));
					break;
				}
			}
		}

		private void Process( Mobile from )
		{
			Item rewardItem;

            switch( m_Skill.SkillID )
			{
				case 0: // alchemy
					rewardItem = new MortarPestle();
					break;
				case 7: // Blacksmithy
					rewardItem = new GMSmithHammer();
					break;
				case 8: // Bowcraft/fletching
					rewardItem = new Bow();
					break;
				case 11: // Carpentry
                    switch (m_Itemid)
                    {
                        case 4138:
                            rewardItem = new Hammer();
                            break;
                        case 4148:
                            rewardItem = new Saw();
                            break;
                        case 4146:
                            rewardItem = new SmoothingPlane();
                            break;
                        case 4325:
                            rewardItem = new Froe();
                            break;
                        case 4326:
                            rewardItem = new Inshave();
                            break;
                        case 4324:
                            rewardItem = new DrawKnife();
                            break;
                        default:
                            return;
                    }
			        break;
				case 23: // Inscription
					rewardItem = new ScribesPen();
					break;
				case 25: // Magery
					rewardItem = new Spellbook();
					( rewardItem as Spellbook ).Content = ulong.MaxValue;
                    (rewardItem as Spellbook).LootType = LootType.Blessed;
					break;
				case 34: // Tailoring
					rewardItem = new RewardDyeTub();
					break;
				case 35: // Animal Taming
					rewardItem = new ShepherdsCrook();
					break;
                case 44: //Lumberjacking
			        rewardItem = new Hatchet();
			        break;
                case 45: // Mining
                    rewardItem = new Pickaxe();
                    break;
                //Bardic skills
                case 9:
                case 15:
                case 22:
                case 29:
                    switch (m_Itemid)
                    {
                        case 3740:
                            rewardItem = new Drums();
                            break;
                        case 3761:
                            rewardItem = new Harp();
                            break;
                        case 3762:
                            rewardItem = new LapHarp();
                            break;
                        case 3763:
                            rewardItem = new Lute();
                            break;
                        case 3741:
                            rewardItem = new Tambourine();
                            break;
                        case 3742:
                            rewardItem = new TambourineTassel();
                            break;
                        default:
                            return;
                    }
			        break;
                //"Thieving" skills
                case 21:
                case 28:
                case 33:
                case 47:
			        rewardItem = new Cloak();
			        break;
				default:
					return;
			}

			rewardItem.Hue = m_CurrentHue;
			rewardItem.LootType = LootType.Blessed;

			if( m_Name != "" )
				rewardItem.Name = m_Name;

			if( rewardItem is RewardDyeTub )
			{
				var tub = (RewardDyeTub)rewardItem;
				tub.DyedHue = rewardItem.Hue;
                tub.Redyable = false;
                tub.LootType = LootType.Blessed;
                tub.Owner = from;

			}
			else if( rewardItem is GMSmithHammer )
			{
				var hammer = (GMSmithHammer)rewardItem;
				hammer.Owner = from;
			}

			from.AddToBackpack( rewardItem );

			// Log
			RewardLogging.WriteLine( from, m_Skill, m_Name, m_CurrentHue );
		}
	}
}