using System;
using Server.Engines.Harvest;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    public class DeepSeaFishingPole : Item, DeepSeaFishingPoleItem
    {
        public SkillMod m_SkillMod;
        public int m_Charges;
        public int m_MaxCharges;
        public virtual HarvestSystem HarvestSystem { get { return Fishing.System; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set
            {
                if (value > MaxCharges)
                    m_Charges = MaxCharges;
                else if (value < 0)
                    m_Charges = 0;
                else
                    m_Charges = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxCharges { get { return 100; } }

        public string DeepSeaFishingPoleItemName { get { return "Deep sea fishing pole"; } }

        [Constructable]
        public DeepSeaFishingPole()
            : base(0x0DC0)
        {
            Name = "Deep sea fishing pole";
            Charges = 10;
            Hue = 361;
            LootType = LootType.Blessed;
            Layer = Layer.OneHanded;
            Weight = 4.0;
        }

        public override void OnSingleClick(Mobile from)
        {
            if (Charges != 0)
            {
                LabelTo(from, "{0} with {1} piece{2} of bait", Name, Charges, m_Charges >= 2 ? "s" : "");
            }
            if (Charges == 0)
            {
                LabelTo(from, "{0}", Name);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            Fishing.System.BeginHarvesting(from, this);

            if (Charges >= 1 && m_SkillMod == null && from.FindItemOnLayer(Layer.OneHanded) is DeepSeaFishingPole)
            {
                m_SkillMod = new DefaultSkillMod(SkillName.Fishing, true, 1);
                from.AddSkillMod(m_SkillMod);
            }

            if (!Sphere.EquipOnDouble(from, this))
                return;


            base.OnDoubleClick(from);
        }

        public override void OnItemUsed(Mobile from, Item item)
        {
            if (Charges < 1)
            {
                if (m_SkillMod != null)
                {
                    m_SkillMod.Remove();
                    m_SkillMod = null;
                }
                from.SendAsciiMessage("The fish have chewed off all your bait!");
            }


            base.OnItemUsed(from, item);
        }

        public override bool OnEquip(Mobile m)
        {
            base.OnEquip(m);
            if (Charges >= 1 && m_SkillMod == null)
            {
                m_SkillMod = new DefaultSkillMod(SkillName.Fishing, true, 1);
                m.AddSkillMod(m_SkillMod);
            }
            return true;
        }

        public override void OnRemoved(object parent)
        {
            Mobile pl = null;
            if (parent is Mobile)
                pl = (Mobile)parent;
            if (m_SkillMod != null && pl != null)
            {
                if (pl.FindItemOnLayer(Layer.OneHanded) != this)
                {
                    m_SkillMod.Remove();
                    m_SkillMod = null;
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            BaseHarvestTool.AddContextMenuEntries(from, this, list, Fishing.System);
        }

        public DeepSeaFishingPole(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Charges = reader.ReadInt();
        }
    }
}