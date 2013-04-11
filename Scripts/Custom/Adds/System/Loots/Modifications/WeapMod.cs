using Server.Items;
using Server.Scripts.Custom.Adds.System.Loots.Containers;

namespace Server.Scripts.Custom.Adds.System.Loots.Modifications
{
    public class WeapMod : BaseMod
    {
        private readonly BaseBonusContainer m_DamageContainer;
        private readonly BaseBonusContainer m_AccuracyContainer;
        private readonly BaseBonusContainer m_DurabilityContainer;

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="damageContainer">container with all possible types of damage related bonus and their chances.</param>
        public WeapMod(Damage damageContainer)
            : this(damageContainer, null, null)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="accuracyContainer">container with all possible types of accuracy related bonus and their chances.</param>
        public WeapMod(Accuracy accuracyContainer)
            : this(null, accuracyContainer, null)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="durabilityContainer">container with all possible types of durability related bonus and their chances.</param>
        public WeapMod(Durability durabilityContainer)
            : this(null, null, durabilityContainer)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="damageContainer">container with all possible types of damage related bonus and their chances.</param>
        /// <param name="accuracyContainer">container with all possible types of accuracy related bonus and their chances.</param>
        public WeapMod(Damage damageContainer, Accuracy accuracyContainer)
            : this(damageContainer, accuracyContainer, null)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="damageContainer">container with all possible types of damage related bonus and their chances.</param>
        /// <param name="durabilityContainer">container with all possible types of durability related bonus and their chances.</param>
        public WeapMod(Damage damageContainer, Durability durabilityContainer)
            : this(damageContainer, null, durabilityContainer)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="accuracyContainer">container with all possible types of accuracy related bonus and their chances.</param>
        /// <param name="durabilityContainer">container with all possible types of durability related bonus and their chances.</param>
        public WeapMod(Accuracy accuracyContainer, Durability durabilityContainer)
            : this(null, accuracyContainer, durabilityContainer)
        {
        }

        /// <summary>
        /// Class to hold the weapon bonus attributes and chances
        /// </summary>
        /// <param name="damageContainer">container with all possible types of damage related bonus and their chances.</param>
        /// <param name="accuracyContainer">container with all possible types of accuracy related bonus and their chances.</param>
        /// <param name="durabilityContainer">container with all possible types of durability related bonus and their chances.</param>
        public WeapMod(Damage damageContainer, Accuracy accuracyContainer, Durability durabilityContainer)
        {
            m_DamageContainer = damageContainer;
            m_AccuracyContainer = accuracyContainer;
            m_DurabilityContainer = durabilityContainer;
        }

        public void Mutate(BaseWeapon weapon)
        {
            if (m_DamageContainer != null)
            {
                if (m_DamageContainer.AttributeSpawnChance >= (Utility.RandomDouble() * 100))
                    weapon.DamageLevel = (WeaponDamageLevel)GetBonusLevel(m_DamageContainer.Attributes);
            }

            if (m_AccuracyContainer != null)
            {
                if (m_AccuracyContainer.AttributeSpawnChance >= (Utility.RandomDouble() * 100))
                    weapon.AccuracyLevel= (WeaponAccuracyLevel)GetBonusLevel(m_AccuracyContainer.Attributes);
            }

            if (m_DurabilityContainer != null)
            {
                if (m_DurabilityContainer.AttributeSpawnChance >= (Utility.RandomDouble() * 100))
                    weapon.DurabilityLevel = (WeaponDurabilityLevel)GetBonusLevel(m_DurabilityContainer.Attributes);
            }
        }
    }
}