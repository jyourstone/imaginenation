using System;
using Server.Items;

namespace Server.Spells.First
{
    public class NightSightSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1E3; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Night Sight", "In Lor",
				263,
				9031,
				Reagent.SulfurousAsh,
				Reagent.SpidersSilk
			);

		public NightSightSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnCast()
        {
            if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else if (SphereSpellTarget is Mobile && Caster.InLOS(SphereSpellTarget) && CheckSequence())
            {
                Mobile targ = (Mobile)SphereSpellTarget;

                new LightCycle.NightSightTimer(targ).Start();
                int level = (int)Math.Abs(LightCycle.DungeonLevel * (Caster.Skills[SkillName.Magery].Base / 100));

                if (level > 25 || level < 0)
                    level = 25;

                targ.LightLevel = level;

                targ.FixedParticles(0x376A, 10, 15, 5007, EffectLayer.Waist);
                targ.PlaySound(Sound);
            }

            FinishSequence();
        }
	}
}
