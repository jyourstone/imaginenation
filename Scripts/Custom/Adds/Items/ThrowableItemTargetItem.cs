using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class ThrowableItemTargetItem : Item
    {
        private readonly DateTime m_Created;
        private readonly Timer m_Timer;

        public TimeSpan Duration { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        public ThrowableItemTargetItem(TimeSpan duration, int itemid)
        {
            Movable = false;

            m_Created = DateTime.Now;
            Duration = duration;
            ItemID = itemid;

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), new TimerCallback(OnTick));
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();
        }

        private void OnTick()
        {
            DateTime now = DateTime.Now;
            TimeSpan age = now - m_Created;

            if (age > Duration)
                Delete();

            else
            {
                List<Mobile> toDamage = new List<Mobile>();

                foreach (Mobile m in GetMobilesInRange(0))
                {
                    BaseCreature bc = m as BaseCreature;

                    if (m.Alive && !m.IsDeadBondedPet && (bc == null || bc.Controlled || bc.Summoned))
                    {
                        toDamage.Add(m);
                    }
                }

                for (int i = 0; i < toDamage.Count; i++)
                    Damage(toDamage[i]);

            }
        }

        public override bool OnMoveOver(Mobile m)
        {
            Damage(m);
            return true;
        }

        public void Damage(Mobile m)
        {
            AOS.Damage(m, null, Utility.RandomMinMax(MinDamage, MaxDamage), 0, 100, 0, 0, 0);
        }

        public ThrowableItemTargetItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            //Don't serialize these
        }

        public override void Deserialize(GenericReader reader)
        {
        }
    }
}
