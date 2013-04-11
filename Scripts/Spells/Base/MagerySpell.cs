using System;
using Server.Items;
using Server.Regions;

namespace Server.Spells
{
	public abstract class MagerySpell : Spell
	{
		public MagerySpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public abstract SpellCircle Circle { get; }

        public virtual int ManaCost { get { return ManaTable[(int)Circle]; } }

		public override bool ConsumeReagents()
		{
            if (Scroll != null || !Caster.Player)
                return true;

            if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                return true;

            if (Caster.Backpack == null)
                return false;

            if (Caster.Backpack.ConsumeTotal(Info.Reagents, Info.Amounts) == -1)
                return true;

			return false;
		}

		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

        public new virtual void GetCastSkills(out double min, out double max)
        {
            int circle = (int)Circle;

            if (Scroll != null)
                circle -= 2;

            double avg = ChanceLength * circle;

            //Is this good? If it is, maybe we should change it in skill check instead.
            //8th circle has about 10% chanse to fizzle on gm
            //7th will always succeed.
            min = avg - ChanceOffset * 1.25;
            max = avg + ChanceOffset / 5.6;
        }

        private static readonly int[] ManaTable = new[] { 4, 6, 9, 11, 14, 20, 40, 50 };

        public override int GetMana()
        {
            if (Scroll is BaseWand)
                return 0;

            if (Caster.AccessLevel >= AccessLevel.GameMaster)
                return 0;

            if (Scroll is SpellScroll)
            {
                SpellScroll scroll = Scroll as SpellScroll;

                if (scroll.ManaCost > 0)
                    return scroll.ManaCost;

                return (int)(ManaCost / 1.755);
            }

            return ManaCost;
        }

		public override double GetResistSkill( Mobile m )
		{
            //int maxSkill = (1 + (int)Circle) * 10;
            //maxSkill += (1 + ((int)Circle / 6)) * 25;

            //if ( m.Skills[SkillName.MagicResist].Value < maxSkill )
            //    m.CheckSkill( SkillName.MagicResist, 0.0, 120.0 );

            m.CheckSkill(SkillName.MagicResist, 0.0, 30.0); //All offensive spells have same resist change as Fire Field

			return m.Skills[SkillName.MagicResist].Value;
		}

        public override bool CheckFizzle()
        {
            if (Caster.AccessLevel >= AccessLevel.GameMaster)
                return true;

            if (Scroll != null && Scroll is BaseWand)
                return true;

            double minSkill, maxSkill;

            GetCastSkills(out minSkill, out maxSkill);

            return Caster.CheckSkill(CastSkill, minSkill, maxSkill);
        }

		public virtual bool CheckResisted( Mobile target )
        {
            double n = GetResistPercent(target);

            n /= 100.0;

            if (n <= 0.0)
                return false;

            if (n >= 1.0)
                return true;

            int maxSkill = (1 + (int)Circle) * 10;
            maxSkill += (1 + ((int)Circle / 6)) * 25;

            if (target.Skills[SkillName.MagicResist].Value < maxSkill)
                target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);

            return (n >= Utility.RandomDouble());
        }

		public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
            double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
            double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

            return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
        }

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}

        public override TimeSpan GetCastDelay()
        {
            if (Caster.AccessLevel >= AccessLevel.GameMaster)
                return TimeSpan.Zero;

            if (Scroll != null && Scroll is BaseWand)
                return TimeSpan.FromSeconds(1.5);

            CustomRegion cR = Caster.Region as CustomRegion;

            double delay;

            switch (((int)Circle + 1))
            {
                case 1:
                    {
                        delay = 0.75;
                        break;
                    }
                case 2:
                    {
                        if (cR != null && cR.Controller.FizzlePvP)
                            delay = 1.80;
                        else
                            delay = 1.00;
                        break;
                    }
                case 3:
                case 4:
                    {
                        if (cR != null && cR.Controller.FizzlePvP)
                            delay = 2.10;
                        else if (Scroll != null)
                            delay = 1.00;
                        else
                            delay = 2.10;

                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        if (cR != null && cR.Controller.FizzlePvP)
                            delay = 3.20;
                        else if (Scroll != null)
                            delay = 2.10;
                        else
                            delay = 3.20;

                        break;
                    }
                case 8:
                    {
                        if (cR != null && cR.Controller.FizzlePvP)
                            delay = 4.30;
                        else if (Scroll != null)
                            delay = 3.10;
                        else
                            delay = 4.30;

                        break;
                    }
                default:
                    {
                        delay = 1.00;
                        break;
                    }
            }

            return TimeSpan.FromSeconds(delay);
        }

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
	}
}