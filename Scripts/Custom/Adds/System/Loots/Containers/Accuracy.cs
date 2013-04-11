namespace Server.Scripts.Custom.Adds.System.Loots.Containers
{
    public class Accuracy : BaseBonusContainer
    {
        /// <summary>
        /// Class to represent the accuracy drop rate and the various levels and chances
        /// </summary>
        /// <param name="dropChance">Percentage for the weapon to get some accurate bonus</param>
        /// <param name="attributeChance">Chances and levels for the various attributes</param>
        public Accuracy(double dropChance, params Attribute[] attributeChance)
            : base(dropChance, attributeChance)
        {
        }
    }
}