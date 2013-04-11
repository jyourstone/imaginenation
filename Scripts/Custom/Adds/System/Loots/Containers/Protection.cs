namespace Server.Scripts.Custom.Adds.System.Loots.Containers
{
    public class Protection : BaseBonusContainer
    {
        public Protection(double dropChance, params Attribute[] attributeChance)
            : base(dropChance, attributeChance)
        {
        }
    }
}