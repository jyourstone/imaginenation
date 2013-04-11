using System;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class ScoreKeeper : IComparable
    {
        private Mobile m_Player;
        private int m_Kills;
        private int m_Deaths;

        public Mobile Player { get { return m_Player; } }
        public int Kills { get { return m_Kills; } set { m_Kills = value; } }
        public int Deaths { get { return m_Deaths; } set { m_Deaths = value; } }

        public ScoreKeeper( Mobile m )
        {
            m_Player = m;
            m_Deaths = 0;
            m_Kills = 0;
        }

        public ScoreKeeper()
        {

        }

        public void Serialize( GenericWriter writer )
        {
            writer.Write( 0 );

            writer.Write( m_Player );
            writer.Write( m_Kills );
            writer.Write( m_Deaths );
        }

        public void Deserialize( GenericReader reader )
        {
            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        m_Player = reader.ReadMobile();
                        m_Kills = reader.ReadInt();
                        m_Deaths = reader.ReadInt();
                        break;
                    }
            }
        }

        public int CompareTo(object obj)
        {
            ScoreKeeper scoreKeeper = (ScoreKeeper)obj;

            if (scoreKeeper.Kills == Kills)
                return Deaths - scoreKeeper.Deaths;

            return scoreKeeper.Kills - Kills;
        }
    }
}
