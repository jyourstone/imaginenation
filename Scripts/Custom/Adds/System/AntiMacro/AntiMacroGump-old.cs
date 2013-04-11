using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.AntiMacro
{
    public class OldAntiMacroGump : Gump
    {
        private readonly PlayerMobile m_GumpOwner;
        private MacroTimer m_MacroTimer;
        private readonly int m_CorrectChoice = 0;

        private static readonly int[] m_TextColorList = new int[] { 10, 20, 60, 70, 100, 1952, 1452 };
        private static readonly int[] m_BackgroundList = new int[] { 5058, 5059, 5060, 5061, 5221, 5222, 5223 };

        private static int m_GumpLenght = 300;
        private static int m_ButtonHeightOffset = 40;
        private static int m_CharHeightOffset = 4;
        private static int m_CharWidthOffset = 2;

        private static string m_GumpName = "Anti Macro Gump";

        private static string m_CorrectButtonString = "RIGHT BUTTON";
        private static string m_WrongButtonString = "WRONG BUTTON";

        public OldAntiMacroGump(PlayerMobile gumpOwner) : base(Utility.Random(5,300), Utility.Random(30,60))
        {
            m_GumpOwner = gumpOwner;
            //PlayerMobile target = obj as PlayerMobile; 

            if (m_GumpOwner.AntiMacroGump)
                return;
            //Randomise the amount of options this gump will have.
            int optionCount = Utility.Random(3, 5);

            //Sets the "I'm not autoharvesting" button.
            m_CorrectChoice = Utility.Random(0, optionCount);

            //Start the macro check timer. If the user doesn't respond within 120 seconds, he will be penalized.
            m_MacroTimer = new MacroTimer(gumpOwner, TimeSpan.FromSeconds(120));
            m_MacroTimer.Start();

            #region CreateGump
            Closable = false;
            Disposable = true;
            Dragable = false;
            Resizable = false;

            //Generate a random background for the gump
            AddImageTiled(0, 0, m_GumpLenght, (int)((optionCount + 1.5) * m_ButtonHeightOffset), GetRandomBackground());

            //Display the gump name
            AddLabel(100, 0, 33, m_GumpName);
            #endregion

            for (int i = 0; i <= optionCount; i++)
            {
                //Initialize the starting locations for buttons and text labels
                int xStart = Utility.RandomMinMax(20, 60);
                int yStart = (m_ButtonHeightOffset / 2) + (i * m_ButtonHeightOffset);

                //Add a button that will be located next to the string, no matter if it's fake or not.
                AddButton(xStart, yStart, 2151, 2154, i, GumpButtonType.Reply, 0);

                //The starting x location to be used to place the text
                int xLabelStart = xStart + Utility.RandomMinMax(35, 60);

                //Will display the button on somewhat random locations.
                if (i == m_CorrectChoice)      //This will display the right button text
                    for (int j = 0; j < m_CorrectButtonString.Length; j++)
                        AddLabel(xLabelStart + (j * 11) + Utility.RandomMinMax(-m_CharWidthOffset, m_CharWidthOffset), (yStart + 5) + Utility.RandomMinMax(-m_CharHeightOffset, m_CharHeightOffset), GetRandomTextColor(), m_CorrectButtonString[j].ToString());
                else                           //This will display the wrong button text
                    for (int j = 0; j < m_WrongButtonString.Length; j++)
                        AddLabel(xLabelStart + (j * 11) + Utility.RandomMinMax(-m_CharWidthOffset, m_CharWidthOffset), (yStart + 5) + Utility.RandomMinMax(-m_CharHeightOffset, m_CharHeightOffset), GetRandomTextColor(), m_WrongButtonString[j].ToString());
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID != m_CorrectChoice)
            {
                if (m_GumpOwner.AntiMacroTimeCheck < (DateTime.Now - TimeSpan.FromHours(24)))
                {
                    m_GumpOwner.AntiMacroFailed = 1;
                    m_GumpOwner.AntiMacroTimeCheck = DateTime.Now;
                }
                else
                    m_GumpOwner.AntiMacroFailed++;

                m_GumpOwner.Kill();

                if (m_GumpOwner.AntiMacroFailed >= 3)
                {
                    m_GumpOwner.SendAsciiMessage("You failed the gump too many times and have been moved to a secure location.");
                    m_GumpOwner.Location = new Point3D(2567, 448, 0);
                    m_GumpOwner.Map = Map.Felucca;
                    m_GumpOwner.AntiMacroFailed = 0;
                }
            }
            else
                m_GumpOwner.AntiMacroFailed = 0;

            //m_GumpOwner.Frozen = false;
            m_GumpOwner.CloseGump(typeof(OldAntiMacroGump));
            m_GumpOwner.AntiMacroGump = false;

            if (m_MacroTimer != null)
            {
                m_MacroTimer.Stop();
                m_MacroTimer = null;
            }
        }

        private void PlaceGumpName()
        {
            //Calculate the amount of free space that the gump name wont take.
            int xLoc = (m_GumpLenght - (m_GumpName.Length * 9));

            //Divide that by two, so that we get the same amount of ree space on both sides of the gump name.
            xLoc /= 2;

            AddLabel(xLoc, 0, 33, m_GumpName);
        }

        private int GetRandomTextColor()
        {
            return m_TextColorList[Utility.Random(m_TextColorList.Length)];
        }

        private int GetRandomBackground()
        {
            return m_BackgroundList[Utility.Random(m_TextColorList.Length)];
        }

        public class MacroTimer : Timer
        {
            private readonly PlayerMobile m_From;

            public MacroTimer(PlayerMobile from, TimeSpan duration)
                : base(duration)
            {
                m_From = from;
                //m_From.Frozen = true; //removed to prevent people crashing while frozen
                m_From.AntiMacroGump = true;
                //m_From.SendAsciiMessage(33, string.Format("You now have {0} seconds to respond befor you are killed.", duration.TotalSeconds));
            }

            protected override void OnTick()
            {
                if (m_From.AntiMacroTimeCheck < (DateTime.Now - TimeSpan.FromHours(24)))
                {
                    m_From.AntiMacroFailed = 1;
                    m_From.AntiMacroTimeCheck = DateTime.Now;
                }
                else
                    m_From.AntiMacroFailed++;

                m_From.Kill();
                //m_From.Frozen = false;
                m_From.CloseGump(typeof(OldAntiMacroGump));
                m_From.AntiMacroGump = false;

                if (m_From.AntiMacroFailed >= 3)
                {
                    m_From.SendAsciiMessage("You failed the gump too many times and have been moved to a secure location.");
                    m_From.Location = new Point3D(2567, 448, 0);
                    m_From.Map = Map.Felucca;
                    m_From.AntiMacroFailed = 0;
                }
            }
        }
    }
}