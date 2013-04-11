namespace Server.Scripts.Custom.Adds.System.Loots.Containers
{
    public class Durability : BaseBonusContainer
    {
        /// <summary>
        /// Class to represent the durability drop rate and the various levels and chances
        /// </summary>
        /// <param name="dropChance">Percentage for the item to get some durability bonus</param>
        /// <param name="attributeChance">Chances and levels for the various attributes</param>
        public Durability(double dropChance, params Attribute[] attributeChance)
            : base(dropChance, attributeChance)
        {
        }
    }
}