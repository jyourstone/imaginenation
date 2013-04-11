using System;
using Server.Custom.Polymorph;
using Server.Mobiles;

namespace Server.Spells.Fifth
{
    public class SummonCreatureSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 0x215; } }

		private readonly bool m_HasNoTarget;
        public override bool CanTargetGround { get { return true; } }
        public override bool HasNoTarget { get { return m_HasNoTarget; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
                "Summon Creature", "Kal Xen",
                263,
                9040,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk
            );

        private readonly SummonCreatureEntry m_SummonCreatureEntry;

        public SummonCreatureSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
			m_HasNoTarget = true;
        }

        public SummonCreatureSpell(Mobile caster, Item scroll, SummonCreatureEntry summonCreatureEntry) : base(caster, scroll, m_Info)
        {
            m_SummonCreatureEntry = summonCreatureEntry;
            Caster.SendAsciiMessage("Where do you wish to summon the creature?");
			m_HasNoTarget = false;
        }

        public override void OnPlayerCast()
        {
            Target((IPoint3D)SphereSpellTarget);
        }

        public override void OnCast()
        {
            Target(Caster.Location);
        }

        public override bool CheckCast()
        {
            if (m_SummonCreatureEntry == null)
            {
                Caster.SendGump(new SummonCreatureGump(Caster, Scroll));
                return false;
            }

            return base.CheckCast();
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p) || !Caster.InLOS(p))
            {
                Caster.SendAsciiMessage("Target is not in line of sight.");
                DoFizzle();
            }
            else if (CheckSequence())
            {
                BaseCreature creature = null;

                if (m_SummonCreatureEntry != null)
                    creature = (BaseCreature)Activator.CreateInstance(m_SummonCreatureEntry.Creature);

                if (creature != null)
                    Summon(creature, Caster, Sound, TimeSpan.FromSeconds(60 + (Caster.Skills.Magery.Base * 2)), new Point3D(p));
            }

            FinishSequence();
        }

        public void Summon(BaseCreature creature, Mobile caster, int sound, TimeSpan duration, Point3D spawnAt)
        {
            Map map = caster.Map;

            if (map == null)
                return;

            if (map.CanFit(spawnAt.X, spawnAt.Y, spawnAt.Z, 1, true, false, true))
            {
                BaseCreature.Summon(creature, caster, spawnAt, sound, duration);
                return;
            }

            creature.Delete();
            caster.SendLocalizedMessage(501942); // That location is blocked.
        }
    }
}