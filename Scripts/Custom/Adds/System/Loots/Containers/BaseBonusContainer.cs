using System;

namespace Server.Scripts.Custom.Adds.System.Loots.Containers
{
    public abstract class BaseBonusContainer
    {
        public Attribute[] Attributes { get; private set; }
        public double AttributeSpawnChance { get; private set; }

        protected BaseBonusContainer(double attributeSpawnChance, params Attribute[] attributes)
        {
            AttributeSpawnChance = attributeSpawnChance;
            Array.Sort(attributes);

            double nextLevel = 0;
            for (int i = 0; i < attributes.Length;i++)
            {
                Attribute info = attributes[i];
                nextLevel += info.ChanceToGet;

                info.ChanceToGet = nextLevel;
            }

            Attributes = attributes;
        }
    }
}