using Server.Items;
using Server.Scripts.Custom.Adds.System.Loots.Containers;

namespace Server.Scripts.Custom.Adds.System.Loots.Modifications
{
    public class ArMod : BaseMod
    {
        private readonly BaseBonusContainer m_ProtectionContainer;
        private readonly BaseBonusContainer m_DurabilityContainer;

        /// <summary>
        /// Class to hold the armor bonus attributes and chances
        /// </summary>
        /// <param name="protectionContainer">container with all possible types of armor protection related bonus and their chances.</param>
        public ArMod(Protection protectionContainer)
            : this(protectionContainer, null)
        {
        }

        /// <summary>
        /// Class to hold the armor bonus attributes and chances
        /// </summary>
        /// <param name="durabilityContainer">container with all possible types of armor durability related bonus and their chances.</param>
        public ArMod(Durability durabilityContainer)
            : this(null, durabilityContainer)
        {
        }

        /// <summary>
        /// Class to hold the armor bonus attributes and chances
        /// </summary>
        /// <param name="protectionContainer">container with all possible types of armor protection related bonus and their chances.</param>
        /// <param name="durabilityContainer">container with all possible types of armor durability related bonus and their chances.</param>
        public ArMod(Protection protectionContainer, Durability durabilityContainer)
        {
            m_ProtectionContainer = protectionContainer;
            m_DurabilityContainer = durabilityContainer;
        }

        public void Mutate(BaseArmor weapon)
        {
            if (m_ProtectionContainer != null)
            {
                if (m_ProtectionContainer.AttributeSpawnChance >= (Utility.RandomDouble() * 100))
                    weapon.ProtectionLevel = (ArmorProtectionLevel)GetBonusLevel(m_ProtectionContainer.Attributes);
            }

            if (m_DurabilityContainer != null)
            {
                if (m_DurabilityContainer.AttributeSpawnChance >= (Utility.RandomDouble() * 100))
                    weapon.Durability = (ArmorDurabilityLevel)GetBonusLevel(m_DurabilityContainer.Attributes);
            }
        }
    }
}