using System;
using System.Collections.Generic;
using Server.Gumps;

namespace Server.Custom.PvpToolkit.DMatch.Items
{
    public class DMScoreGump : Gump
    {
        public Mobile m_From;
        public List<ScoreKeeper> m_List;

        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;
        private const int RedHue = 0x20;
        private const int YellowHue = 49;

        public DMScoreGump(Mobile from, List<ScoreKeeper> list) : base( 50, 40 )
        {
            from.CloseGump( typeof( DMScoreGump ) );

            m_List = list;
            m_From = from;

            m_List.Sort();

            AddPage( 0 );
            AddBackground( 0, 0, 420, 470, 5054 );
            AddBlackAlpha( 10, 10, 400, 450 );

            AddLabel( 160, 15, RedHue, "Deathmatch Top 15" );
            AddLabel( 20, 40, LabelHue, "Players" );
            AddLabel( 130, 40, LabelHue, "Points");
            AddLabel( 240, 40, LabelHue, "Kills" );
            AddLabel( 350, 40, LabelHue, "Deaths" );

            int count = 0;
            int kills = 0;
            int deaths = 0;
            double points = 0.0;

            m_List.Sort();
            for (int i = 0; i < m_List.Count && i < 15; ++i)
            {
                ScoreKeeper scoreKeeper = m_List[i];

                string name = string.Empty;

                if (scoreKeeper.Player != null && !string.IsNullOrEmpty(scoreKeeper.Player.Name) &&
                    scoreKeeper.Player.Name.Trim().Length <= 15)
                    name = scoreKeeper.Player.Name;

                string score = scoreKeeper.Points.ToString();
                string wins = scoreKeeper.Kills.ToString();
                string loses = scoreKeeper.Deaths.ToString();

                if (scoreKeeper.Player != m_From)
                {
                    AddLabel(20, 70 + ((count%20)*20), GreenHue, name);
                    AddLabel(140, 70 + ((count % 20) * 20), GreenHue, score);
                    AddLabel(248, 70 + ((count % 20) * 20), GreenHue, wins);
                    AddLabel(360, 70 + ((count%20)*20), GreenHue, loses);
                }
                else
                {
                    kills = scoreKeeper.Kills;
                    deaths = scoreKeeper.Deaths;
                    points = scoreKeeper.Points;
                    AddLabel(20, 70 + ((count % 20) * 20), YellowHue, name);
                    AddLabel(140, 70 + ((count % 20) * 20), YellowHue, score);
                    AddLabel(248, 70 + ((count % 20) * 20), GreenHue, wins);
                    AddLabel(360, 70 + ((count % 20) * 20), YellowHue, loses);
                }

                count++;
            }
            AddLabel(20, 90 + ((count % 20) * 20), YellowHue, "Your points: " + points);
            AddLabel(20, 110 + ((count % 20) * 20), YellowHue, "Your kills: " + kills);
            AddLabel(20, 130 + ((count % 20) * 20), YellowHue, "Your deaths: " + deaths);
        }

        public string Color( string text, int color )
        {
            return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
        }

        public void AddBlackAlpha( int x, int y, int width, int height )
        {
            AddImageTiled( x, y, width, height, 2624 );
            AddAlphaRegion( x, y, width, height );
        }
    }
}

