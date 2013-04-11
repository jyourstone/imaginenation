using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
	public class Anatomy
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Anatomy].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.Target = new InternalTarget();
                m.SendLocalizedMessage(500321); // Whom shall I examine?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
		}

		private class InternalTarget : Target, IAction
		{
			public InternalTarget() :  base ( 8, false, TargetFlags.None )
			{

			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaseLock = true;

                SpellHelper.Turn(from, targeted);

                if (targeted is PlayerMobile)
                {
                    releaseLock = false;
                    new TextTimer(from, targeted).Start();
                }

                else if (targeted is BaseCreature)
                {
                    BaseCreature m = (BaseCreature) targeted;
                    if (m.BodyValue == 0x190 || m.BodyValue == 0x191)
                    {
                        releaseLock = false;

                        if (from.Skills[SkillName.Anatomy].Base < 100.0)
                            new TextTimer(from, targeted).Start();
                        else
                            new GumpTimer(from, targeted).Start();
                    }
                    else
                        from.SendAsciiMessage("You can only use anatomy on humanoids");
                }

                else
                    from.SendAsciiMessage("You can only use anatomy on humanoids");

                if (releaseLock && from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
            }

            #region TargetFailed

            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnNonlocalTarget(from, targeted);
            }

            protected override void OnTargetInSecureTrade(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetInSecureTrade(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetCancel(from, cancelType);
            }

            #endregion

            #region IAction Members

            public void AbortAction(Mobile from)
            {
            }

            #endregion
        }

        private class TextTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly object m_Targeted;

            public TextTimer(Mobile from, object targeted)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                Mobile targ = (Mobile)m_Targeted;

                if (m_From.CheckTargetSkill(SkillName.Anatomy, targ, 0, 100))
                {
                    string strMsg, dexMsg;
                    int str = targ.Str;
                    int dex = targ.Dex;

                    #region Set msg
                    if (str < 11)
                        strMsg = "rather feeble";
                    else if (str < 21 && dex > 10)
                        strMsg = "somewhat weak";
                    else if (str < 31 && dex > 20)
                        strMsg = "to be of normal strength";
                    else if (str < 41 && dex > 30)
                        strMsg = "somewhat strong";
                    else if (str < 51 && dex > 40)
                        strMsg = "extremely strong";
                    else if (str < 61 && dex > 50)
                        strMsg = "extraordinarily strong";
                    else if (str < 71 && dex > 60)
                        strMsg = "extraordinarily agile";
                    else if (str < 81 && dex > 70)
                        strMsg = "as strong as an ox";
                    else if (str < 91 && dex > 80)
                        strMsg = "like one of the strongest people you have ever seen";
                    else
                        strMsg = "superhumanly strong";

                    if (dex < 11)
                        dexMsg = "very clumsy";
                    else if (dex < 21 && dex > 10)
                        dexMsg = "somewhat uncoordinated";
                    else if (dex < 31 && dex > 20)
                        dexMsg = "moderately dexterous";
                    else if (dex < 41 && dex > 30)
                        dexMsg = "somewhat agile";
                    else if (dex < 51 && dex > 40)
                        dexMsg = "very agile";
                    else if (dex < 61 && dex > 50)
                        dexMsg = "extremely agile";
                    else if (dex < 71 && dex > 60)
                        dexMsg = "extraordinarily agile";
                    else if (dex < 81 && dex > 70)
                        dexMsg = "like they move like quicksilver";
                    else if (dex < 91 && dex > 80)
                        dexMsg = "like one of the fastest people you have ever seen";
                    else
                        dexMsg = "superhumanly agile";

                    #endregion

                    targ.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} looks {1} and {2}.", targ.Name, strMsg, dexMsg), m_From.NetState);
                }
                else
                    targ.PrivateOverheadMessage(MessageType.Regular, 906, true, CliLoc.LocToString(1042666), m_From.NetState); // You can not quite get a sense of their physical characteristics.

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You can't think of anything about this creature");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }

        private class GumpTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly object m_Targeted;

            public GumpTimer(Mobile from, object targeted)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                BaseCreature targ = (BaseCreature)m_Targeted;

                if (m_From.CheckTargetSkill(SkillName.Anatomy, targ, 0, 100))
                {
                    m_From.CloseGump(typeof(AnatomyGump));
                    m_From.SendGump(new AnatomyGump(targ));
                }
                else
                    targ.PrivateOverheadMessage(MessageType.Regular, 906, true, CliLoc.LocToString(1042666), m_From.NetState); // You can not quite get a sense of their physical characteristics.

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You can't think of anything about this creature");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}

    public class AnatomyGump : Gump
    {
        private static string FormatSkill(Mobile c, SkillName name)
        {
            Skill skill = c.Skills[name];

            if (skill.Base < 10.0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", skill.Base);
        }

        private static string FormatAttributes(int cur, int max)
        {
            if (max == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}/{1}</div>", cur, max);
        }

        private static string FormatStat(int val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}</div>", val);
        }

        private static string FormatDouble(double val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", val);
        }

        private static string FormatElement(int val)
        {
            if (val <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}%</div>", val);
        }

        private const int LabelColor = 0x24E5;

        public AnatomyGump(BaseCreature c)
            : base(250, 50)
        {
            AddPage(0);

            AddImage(100, 100, 2080);
            AddImage(118, 137, 2081);
            AddImage(118, 207, 2081);
            AddImage(118, 277, 2081);
            AddImage(118, 347, 2083);

            AddHtml(147, 108, 210, 18, String.Format("<center><i>{0}</i></center>", c.Name), false, false);

            AddButton(240, 77, 2093, 2093, 2, GumpButtonType.Reply, 0);

            AddImage(140, 138, 2091);
            AddImage(140, 335, 2091);

            int pages = (Core.AOS ? 5 : 3);
            int page = 0;


            #region Attributes
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1049593, 200, false, false); // Attributes

            AddHtmlLocalized(153, 168, 160, 18, 1049578, LabelColor, false, false); // Hits
            AddHtml(280, 168, 75, 18, FormatAttributes(c.Hits, c.HitsMax), false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1049579, LabelColor, false, false); // Stamina
            AddHtml(280, 186, 75, 18, FormatAttributes(c.Stam, c.StamMax), false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1049580, LabelColor, false, false); // Mana
            AddHtml(280, 204, 75, 18, FormatAttributes(c.Mana, c.ManaMax), false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1028335, LabelColor, false, false); // Strength
            AddHtml(320, 222, 35, 18, FormatStat(c.Str), false, false);

            AddHtmlLocalized(153, 240, 160, 18, 3000113, LabelColor, false, false); // Dexterity
            AddHtml(320, 240, 35, 18, FormatStat(c.Dex), false, false);

            AddHtmlLocalized(153, 258, 160, 18, 3000112, LabelColor, false, false); // Intelligence
            AddHtml(320, 258, 35, 18, FormatStat(c.Int), false, false);

            AddImage(128, 278, 2086);
            AddHtmlLocalized(147, 276, 160, 18, 3001016, 200, false, false); // Miscellaneous

            AddHtmlLocalized(153, 294, 160, 18, 1049581, LabelColor, false, false); // Armor Rating
            AddHtml(320, 294, 35, 18, FormatStat(c.VirtualArmor), false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, pages);
            #endregion

            #region Resistances
            if (Core.AOS)
            {
                AddPage(++page);

                AddImage(128, 152, 2086);
                AddHtmlLocalized(147, 150, 160, 18, 1061645, 200, false, false); // Resistances

                AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
                AddHtml(320, 168, 35, 18, FormatElement(c.PhysicalResistance), false, false);

                AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
                AddHtml(320, 186, 35, 18, FormatElement(c.FireResistance), false, false);

                AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
                AddHtml(320, 204, 35, 18, FormatElement(c.ColdResistance), false, false);

                AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
                AddHtml(320, 222, 35, 18, FormatElement(c.PoisonResistance), false, false);

                AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
                AddHtml(320, 240, 35, 18, FormatElement(c.EnergyResistance), false, false);

                AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
                AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            }
            #endregion

            #region Damage
            if (Core.AOS)
            {
                AddPage(++page);

                AddImage(128, 152, 2086);
                AddHtmlLocalized(147, 150, 160, 18, 1017319, 200, false, false); // Damage

                AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
                AddHtml(320, 168, 35, 18, FormatElement(c.PhysicalDamage), false, false);

                AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
                AddHtml(320, 186, 35, 18, FormatElement(c.FireDamage), false, false);

                AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
                AddHtml(320, 204, 35, 18, FormatElement(c.ColdDamage), false, false);

                AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
                AddHtml(320, 222, 35, 18, FormatElement(c.PoisonDamage), false, false);

                AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
                AddHtml(320, 240, 35, 18, FormatElement(c.EnergyDamage), false, false);

                AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
                AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            }
            #endregion

            #region Skills
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 3001030, 200, false, false); // Combat Ratings

            AddHtmlLocalized(153, 168, 160, 18, 1044103, LabelColor, false, false); // Wrestling
            AddHtml(320, 168, 35, 18, FormatSkill(c, SkillName.Wrestling), false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1044087, LabelColor, false, false); // Tactics
            AddHtml(320, 186, 35, 18, FormatSkill(c, SkillName.Tactics), false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1044086, LabelColor, false, false); // Magic Resistance
            AddHtml(320, 204, 35, 18, FormatSkill(c, SkillName.MagicResist), false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1044061, LabelColor, false, false); // Anatomy
            AddHtml(320, 222, 35, 18, FormatSkill(c, SkillName.Anatomy), false, false);

            AddHtmlLocalized(153, 240, 160, 18, 1044090, LabelColor, false, false); // Poisoning
            AddHtml(320, 240, 35, 18, FormatSkill(c, SkillName.Poisoning), false, false);

            AddImage(128, 260, 2086);
            AddHtmlLocalized(147, 258, 160, 18, 3001032, 200, false, false); // Lore & Knowledge

            AddHtmlLocalized(153, 276, 160, 18, 1044085, LabelColor, false, false); // Magery
            AddHtml(320, 276, 35, 18, FormatSkill(c, SkillName.Magery), false, false);

            AddHtmlLocalized(153, 294, 160, 18, 1044076, LabelColor, false, false); // Evaluating Intelligence
            AddHtml(320, 294, 35, 18, FormatSkill(c, SkillName.EvalInt), false, false);

            AddHtmlLocalized(153, 312, 160, 18, 1044106, LabelColor, false, false); // Meditation
            AddHtml(320, 312, 35, 18, FormatSkill(c, SkillName.Meditation), false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion

            #region Misc
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1049563, 200, false, false); // Preferred Foods

            int foodPref = 3000340;

            if ((c.FavoriteFood & FoodType.FruitsAndVegies) != 0)
                foodPref = 1049565; // Fruits and Vegetables
            else if ((c.FavoriteFood & FoodType.GrainsAndHay) != 0)
                foodPref = 1049566; // Grains and Hay
            else if ((c.FavoriteFood & FoodType.Fish) != 0)
                foodPref = 1049568; // Fish
            else if ((c.FavoriteFood & FoodType.Meat) != 0)
                foodPref = 1049564; // Meat

            AddHtmlLocalized(153, 168, 160, 18, foodPref, LabelColor, false, false);

            /*
            AddImage(128, 188, 2086);
            AddHtmlLocalized(147, 186, 160, 18, 1049569, 200, false, false); // Pack Instincts

            int packInstinct = 3000340;

            if ((c.PackInstinct & PackInstinct.Canine) != 0)
                packInstinct = 1049570; // Canine
            else if ((c.PackInstinct & PackInstinct.Ostard) != 0)
                packInstinct = 1049571; // Ostard
            else if ((c.PackInstinct & PackInstinct.Feline) != 0)
                packInstinct = 1049572; // Feline
            else if ((c.PackInstinct & PackInstinct.Arachnid) != 0)
                packInstinct = 1049573; // Arachnid
            else if ((c.PackInstinct & PackInstinct.Daemon) != 0)
                packInstinct = 1049574; // Daemon
            else if ((c.PackInstinct & PackInstinct.Bear) != 0)
                packInstinct = 1049575; // Bear
            else if ((c.PackInstinct & PackInstinct.Equine) != 0)
                packInstinct = 1049576; // Equine
            else if ((c.PackInstinct & PackInstinct.Bull) != 0)
                packInstinct = 1049577; // Bull

            AddHtmlLocalized(153, 204, 160, 18, packInstinct, LabelColor, false, false);

            if (!Core.AOS)
            {
                AddImage(128, 224, 2086);
                AddHtmlLocalized(147, 222, 160, 18, 1049594, 200, false, false); // Loyalty Rating

                AddHtmlLocalized(153, 240, 160, 18, (!c.Controlled || c.Loyalty == 0) ? 1061643 : 1049595 + (c.Loyalty / 10), LabelColor, false, false);
            }

            AddImage(128, 260, 2086);
            AddHtml(147, 258, 160, 18, "<basefont color=#003142>Tamers</basefont>", false, false);
            AddHtml(153, 276, 160, 18, string.Format("<basefont color=#4a3929>{0}</basefont>", c.Owners.Count), false, false);
            */
            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            
            #endregion
        }
    }
}