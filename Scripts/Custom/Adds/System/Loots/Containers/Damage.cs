namespace Server.Scripts.Custom.Adds.System.Loots.Containers
{
    public class Damage : BaseBonusContainer
    {
        /// <summary>
        /// Class to represent the magic bonus drop rate and the various levels and chances
        /// </summary>
        /// <param name="dropChance">Percentage for the weapon to get some damage bonus</param>
        /// <param name="attributeChance">Chances and levels for the various attributes</param>
        public Damage(double dropChance, params Attribute[] attributeChance)
            : base(dropChance, attributeChance)
        {
        }
    }
}