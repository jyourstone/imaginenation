
namespace Server.Scripts.Custom.Adds.System.Loots.Modifications
{
    public abstract class BaseMod
    {
        protected static int GetBonusLevel(Attribute[] attributes)
        {
            double rnd = Utility.RandomDouble() * 100;

            double prevChance = 0;
            int attributeLevel = 1;
            for (int i = 0; i < attributes.Length; i++)
            {
                Attribute attribute = attributes[i];

                //If its the first time we find a attribute.ChanceToGet smaller than rnd or if the chances coming afterwards are the same
                if ((attribute.ChanceToGet <= rnd || prevChance >= rnd) && prevChance != attribute.ChanceToGet) 
                    continue;

                attributeLevel = attribute.Level;
                prevChance = attribute.ChanceToGet;
            }

            return attributeLevel;
        }
    }
}