using System;
using Server.Mobiles;

namespace Server.Engines.Harvest
{
    public class HarvestTimer : Timer, IAction
    {
        private readonly Mobile m_From;
        private readonly Item m_Tool;
        private readonly HarvestSystem m_System;
        private readonly HarvestDefinition m_Definition;
        private readonly object m_ToHarvest;
        private readonly object m_Locked;
        private int m_Index;
        private readonly int m_Count;

        public HarvestTimer(Mobile from, Item tool, HarvestSystem system, HarvestDefinition def, object toHarvest, object locked)
            : base(TimeSpan.Zero, def.EffectDelay)
        {
            m_From = from;
            m_Tool = tool;
            m_System = system;
            m_Definition = def;
            m_ToHarvest = toHarvest;
            m_Locked = locked;
            //m_Count = Utility.RandomList( def.EffectCounts );
            m_Count = 1 + Utility.Random(1, 5);

            //Update the action
            if (from is PlayerMobile)
                ((PlayerMobile)from).ResetPlayerAction(this);
        }

        protected override void OnTick()
        {
            if (!m_System.OnHarvesting(m_From, m_Tool, m_Definition, m_ToHarvest, m_Locked, ++m_Index == m_Count))
            {
                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();

                Stop();
            }
        }

        #region IAction Members

        public void AbortAction(Mobile from)
        {
            m_Definition.SendMessageTo(from, m_Definition.FailMessage);

            if (from is PlayerMobile)
                ((PlayerMobile)from).EndPlayerAction();

            Stop();
        }

        #endregion
    }
}