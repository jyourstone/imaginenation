using System;

namespace Server.Scripts.Custom.Adds.System.Loots
{
    public class Attribute : IComparable
    {
        public int Level;
        public double ChanceToGet;

        /// <summary>
        /// Creates a generic attribute property with a certain level and a 100% chance to drop
        /// </summary>
        /// <param name="levelValue">Level of the property, in range 1-5</param>
        public Attribute(int levelValue)
            : this(levelValue, 100)
        {
        }

        /// <summary>
        /// Creates a generic attribute property with a certain level and a certain drop chance
        /// </summary>
        /// <param name="levelValue">Level of the property, in range 1-5</param>
        /// <param name="chanceToGet">the percentage chance to get this bonus attribute</param>
        public Attribute(int levelValue, double chanceToGet)
        {
            Level = levelValue;
            ChanceToGet = chanceToGet;
        }

        public int CompareTo(object obj)
        {
            if (obj is Attribute)
            {
                Attribute other = (Attribute)obj;
                return ChanceToGet.CompareTo(other.ChanceToGet);
            }

            throw new ArgumentException("Object is not a Bonus");
        }
    }
}
