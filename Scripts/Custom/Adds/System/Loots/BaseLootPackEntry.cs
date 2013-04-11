namespace Server.Scripts.Custom.Adds.System.Loots
{
    public abstract class BaseLootPackEntry
    {
        protected double m_Chance;
        protected bool m_AtSpawnTime;
        protected LootPackItem[] m_Items;
        protected LootPackDice m_Quantity;

        public double Chance
        {
            get { return m_Chance; }
            set { m_Chance = value; }
        }

        public LootPackItem[] Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public LootPackDice Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; }
        }

        public abstract Item Construct(Mobile from, int luckChance, bool spawning);

        public class LootPackDice
        {
            private int m_Count, m_Sides, m_Bonus;

            public int Count
            {
                get { return m_Count; }
                set { m_Count = value; }
            }

            public int Sides
            {
                get { return m_Sides; }
                set { m_Sides = value; }
            }

            public int Bonus
            {
                get { return m_Bonus; }
                set { m_Bonus = value; }
            }

            public int Roll()
            {
                int v = m_Bonus;

                for (int i = 0; i < m_Count; ++i)
                    v += Utility.Random(1, m_Sides);

                return v;
            }

            public LootPackDice(string str)
            {
                int start = 0;
                int index = str.IndexOf('d', start);

                if (index < start)
                    return;

                m_Count = Utility.ToInt32(str.Substring(start, index - start));

                bool negative;

                start = index + 1;
                index = str.IndexOf('+', start);

                if (negative = (index < start))
                    index = str.IndexOf('-', start);

                if (index < start)
                    index = str.Length;

                m_Sides = Utility.ToInt32(str.Substring(start, index - start));

                if (index == str.Length)
                    return;

                start = index + 1;
                index = str.Length;

                m_Bonus = Utility.ToInt32(str.Substring(start, index - start));

                if (negative)
                    m_Bonus *= -1;
            }

            public LootPackDice(int count, int sides, int bonus)
            {
                m_Count = count;
                m_Sides = sides;
                m_Bonus = bonus;
            }
        }

    }
}
