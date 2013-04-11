using System.Collections.Generic;

namespace Server.Custom.SkillBoost
{
    public static class Values
    {
        private static Dictionary<int, double> m_SkillBoostValues = new Dictionary<int, double>();

        public static void Initialize()
        {
            DefaultValues();
        }

        public static Dictionary<int, double> SkillBoostValues
        {
            get { return m_SkillBoostValues; }
            set { m_SkillBoostValues = value; }
        }

        public static void DefaultValues()
        {
            m_SkillBoostValues.Clear();

            m_SkillBoostValues.Add(0, 1);
            m_SkillBoostValues.Add(1, 1);
            m_SkillBoostValues.Add(2, 1);
            m_SkillBoostValues.Add(3, 1);
            m_SkillBoostValues.Add(4, 1);
            m_SkillBoostValues.Add(5, 1);
            m_SkillBoostValues.Add(6, 1);
            m_SkillBoostValues.Add(7, 1);
            m_SkillBoostValues.Add(8, 1);
            m_SkillBoostValues.Add(9, 1);
            m_SkillBoostValues.Add(10, 1);
            m_SkillBoostValues.Add(11, 1);
            m_SkillBoostValues.Add(12, 1);
            m_SkillBoostValues.Add(13, 1);
            m_SkillBoostValues.Add(14, 1);
            m_SkillBoostValues.Add(15, 1);
            m_SkillBoostValues.Add(16, 1);
            m_SkillBoostValues.Add(17, 1);
            m_SkillBoostValues.Add(18, 1);
            m_SkillBoostValues.Add(19, 1);
            m_SkillBoostValues.Add(20, 1);
            m_SkillBoostValues.Add(21, 1);
            m_SkillBoostValues.Add(22, 1);
            m_SkillBoostValues.Add(23, 1);
            m_SkillBoostValues.Add(24, 1);
            m_SkillBoostValues.Add(25, 1);
            m_SkillBoostValues.Add(26, 1);
            m_SkillBoostValues.Add(27, 1);
            m_SkillBoostValues.Add(28, 1);
            m_SkillBoostValues.Add(29, 1);
            m_SkillBoostValues.Add(30, 1);
            m_SkillBoostValues.Add(31, 1);
            m_SkillBoostValues.Add(32, 1);
            m_SkillBoostValues.Add(33, 1);
            m_SkillBoostValues.Add(34, 1);
            m_SkillBoostValues.Add(35, 1);
            m_SkillBoostValues.Add(36, 1);
            m_SkillBoostValues.Add(37, 1);
            m_SkillBoostValues.Add(38, 1);
            m_SkillBoostValues.Add(39, 1);
            m_SkillBoostValues.Add(40, 1);
            m_SkillBoostValues.Add(41, 1);
            m_SkillBoostValues.Add(42, 1);
            m_SkillBoostValues.Add(43, 1);
            m_SkillBoostValues.Add(44, 1);
            m_SkillBoostValues.Add(45, 1);
            m_SkillBoostValues.Add(46, 1);
            m_SkillBoostValues.Add(47, 1);
            m_SkillBoostValues.Add(48, 1);
            m_SkillBoostValues.Add(49, 1);
            m_SkillBoostValues.Add(50, 1);
            m_SkillBoostValues.Add(51, 1);
        }

        public static void RecommendedValues()
        {
            m_SkillBoostValues.Clear();

            m_SkillBoostValues.Add(0, 1.12);
            m_SkillBoostValues.Add(1, 2);
            m_SkillBoostValues.Add(2, 2);
            m_SkillBoostValues.Add(3, 2);
            m_SkillBoostValues.Add(4, 2);
            m_SkillBoostValues.Add(5, 2);
            m_SkillBoostValues.Add(6, 1.34);
            m_SkillBoostValues.Add(7, 1.12);
            m_SkillBoostValues.Add(8, 1.12);
            m_SkillBoostValues.Add(9, 1.34);
            m_SkillBoostValues.Add(10, 2);
            m_SkillBoostValues.Add(11, 1.12);
            m_SkillBoostValues.Add(12, 1.34);
            m_SkillBoostValues.Add(13, 1.12);
            m_SkillBoostValues.Add(14, 2);
            m_SkillBoostValues.Add(15, 1.34);
            m_SkillBoostValues.Add(16, 2);
            m_SkillBoostValues.Add(17, 2);
            m_SkillBoostValues.Add(18, 1.34);
            m_SkillBoostValues.Add(19, 2);
            m_SkillBoostValues.Add(20, 1.34);
            m_SkillBoostValues.Add(21, 2);
            m_SkillBoostValues.Add(22, 1.34);
            m_SkillBoostValues.Add(23, 1.12);
            m_SkillBoostValues.Add(24, 1.34);
            m_SkillBoostValues.Add(25, 2);
            m_SkillBoostValues.Add(26, 2);
            m_SkillBoostValues.Add(27, 2);
            m_SkillBoostValues.Add(28, 1.34);
            m_SkillBoostValues.Add(29, 1.34);
            m_SkillBoostValues.Add(30, 1.34);
            m_SkillBoostValues.Add(31, 2);
            m_SkillBoostValues.Add(32, 2);
            m_SkillBoostValues.Add(33, 1.34);
            m_SkillBoostValues.Add(34, 1.12);
            m_SkillBoostValues.Add(35, 1.12);
            m_SkillBoostValues.Add(36, 2);
            m_SkillBoostValues.Add(37, 1.12);
            m_SkillBoostValues.Add(38, 2);
            m_SkillBoostValues.Add(39, 1.34);
            m_SkillBoostValues.Add(40, 2);
            m_SkillBoostValues.Add(41, 2);
            m_SkillBoostValues.Add(42, 2);
            m_SkillBoostValues.Add(43, 2);
            m_SkillBoostValues.Add(44, 1.12);
            m_SkillBoostValues.Add(45, 1.12);
            m_SkillBoostValues.Add(46, 2);
            m_SkillBoostValues.Add(47, 1.34);
            m_SkillBoostValues.Add(48, 1.34);
            m_SkillBoostValues.Add(49, 1);
            m_SkillBoostValues.Add(50, 1);
            m_SkillBoostValues.Add(51, 1);
        }

        public static double GetValue(int i)
        {
            double o;
            m_SkillBoostValues.TryGetValue(i, out o);

            return o;
        }
    }
}