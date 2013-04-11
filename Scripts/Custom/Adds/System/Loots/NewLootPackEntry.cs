using Server.Items;
using Server.Scripts.Custom.Adds.System.Loots.Modifications;

namespace Server.Scripts.Custom.Adds.System.Loots
{
    public class NewLootPackEntry : BaseLootPackEntry
    {
        private readonly WeapMod m_WeaponModifiers;
        private readonly ArMod m_ArmorModifiers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dropChance"></param>
        /// <param name="weaponModifiers"></param>
        public NewLootPackEntry(LootPackItem[] items, double dropChance, WeapMod weaponModifiers) 
            : this(items, dropChance, weaponModifiers, null)
        {
        }

        public NewLootPackEntry(LootPackItem[] items, double dropChance, ArMod armorModifiers)
            : this(items, dropChance, null, armorModifiers)
        {
        }

        public NewLootPackEntry(LootPackItem[] items, double dropChance, WeapMod weaponModifiers, ArMod armorModifiers)
        {
            m_Items = items;
            m_Chance = dropChance * 100;

            m_WeaponModifiers = weaponModifiers;
            m_ArmorModifiers = armorModifiers;
        }

        public override Item Construct(Mobile from, int luckChance, bool spawning)
        {
            if (m_AtSpawnTime != spawning)
                return null;

            int totalChance = 0;
            for (int i = 0; i < m_Items.Length; ++i)
                totalChance += m_Items[i].Chance;

            int rnd = Utility.Random(totalChance);
            for (int i = 0; i < m_Items.Length; ++i)
            {
                LootPackItem item = m_Items[i];

                if (rnd < item.Chance)
                    return Mutate(from, luckChance, item.Construct(false, false));

                rnd -= item.Chance;
            }

            return null;
        }

        public Item Mutate(Mobile from, int luckChance, Item item)
        {
            if (item == null)
                return null;

            if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;

                //Taran: Below we remove the items we don't want mobs to drop
                while (weapon is BaseKnife || weapon is Pickaxe || weapon is Hatchet || weapon is BaseStaff)
                {
                    weapon.Delete();
                    weapon = BaseWeapon.CreateRandomWeapon();
                }

                if (m_WeaponModifiers != null)
                    m_WeaponModifiers.Mutate(weapon);

                //if (5 > Utility.Random(100))
                //   weapon.Slayer = SlayerName.Silver;

                //if (from != null && weapon.AccuracyLevel == 0 && weapon.DamageLevel == 0 && weapon.DurabilityLevel == 0 && weapon.Slayer == SlayerName.None && 5 > Utility.Random(100))
                //    weapon.Slayer = SlayerGroup.GetLootSlayerType(from.GetType());

                weapon.Identified = true;
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;

                if (m_ArmorModifiers != null)
                    m_ArmorModifiers.Mutate(armor);

                armor.Identified = true;
            }
            else if (item is BaseInstrument)
            {
                SlayerName slayer = SlayerGroup.GetLootSlayerType(from.GetType());

                if (slayer == SlayerName.None)
                {
                    item.Delete();
                    return null;
                }

                BaseInstrument instr = (BaseInstrument)item;

                instr.Quality = InstrumentQuality.Regular;
                instr.Slayer = slayer;
            }

            return item;
        }
    }
}