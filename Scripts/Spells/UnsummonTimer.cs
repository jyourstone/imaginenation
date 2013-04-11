using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	class UnsummonTimer : Timer
	{
		private readonly BaseCreature m_Creature;
		private Mobile m_Caster;

		public UnsummonTimer( Mobile caster, BaseCreature creature, TimeSpan delay ) : base( delay )
		{
			m_Caster = caster;
			m_Creature = creature;
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
            if (!m_Creature.Deleted)
            {
                //Iza - Unsummon Effect
                m_Creature.PlaySound(510);
                Effects.SendLocationParticles(EffectItem.Create(m_Creature.Location, m_Creature.Map, EffectItem.DefaultDuration), 0x3728, 20, 20, 2023);
                m_Creature.Delete();
            }
		}
	}
}