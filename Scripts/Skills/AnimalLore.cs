using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class AnimalLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.AnimalLore].Callback = OnUse;
		}

		public static TimeSpan OnUse(Mobile m)
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.Target = new InternalTarget();
                m.SendLocalizedMessage(500328); // What animal should I look at?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

		    return TimeSpan.Zero;
		}

		private class InternalTarget : Target, IAction
		{
		    public InternalTarget() : base( 8, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaseLock = true;

				if ( !from.Alive )
				{
					from.SendLocalizedMessage( 500331 ); // The spirits of the dead are not the province of animal lore.
				}
				else if ( targeted is BaseCreature )
				{
                    BaseCreature c = (BaseCreature)targeted;

					if ( !c.IsDeadPet )
					{
                        if (c.Body.IsAnimal || c.Body.IsMonster || c.Body.IsSea)
                        {
                            releaseLock = false;
                            new InternalTimer(from, c).Start();
                        }
                        else
                            from.SendLocalizedMessage(500329); // That's not an animal!
					}
					else
						from.SendLocalizedMessage( 500331 ); // The spirits of the dead are not the province of animal lore.
				}
                else if (targeted is PlayerMobile)
                {
                    PlayerMobile p = (PlayerMobile)targeted;

                    releaseLock = false;
                    new InternalTimer(from, p).Start();
                }
                else
                    from.SendAsciiMessage("You can't target that!");
			
               SpellHelper.Turn(from, targeted);

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

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly Mobile m_Targeted;

            public InternalTimer(Mobile from, Mobile targeted)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.CheckTargetSkill(SkillName.AnimalLore, m_Targeted, 0.0, 100.0))
                {
                    if (m_Targeted is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)m_Targeted;
                        string gender = pm.Body.IsFemale ? "female" : "man";

                        pm.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} is a {1}, {2} is {3} own master.", pm.Name, gender, pm.Body.IsFemale ? "she" : "he", pm.Body.IsFemale ? "her" : "his"), m_From.NetState);
                        pm.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} looks {1}.", pm.Body.IsFemale ? "She" : "He", GetHunger(pm)), m_From.NetState);
                    }

                    else if (m_Targeted is BaseCreature)
                    {
                        BaseCreature c = (BaseCreature)m_Targeted;

                        if ((!c.Controlled || !c.Tamable) && m_From.Skills[SkillName.AnimalLore].Base < 90.0)
                            m_From.SendLocalizedMessage(1049674); // At your skill level, you can only lore tamed creatures.

                        else if (!c.Tamable && m_From.Skills[SkillName.AnimalLore].Base < 100.0)
                            m_From.SendLocalizedMessage(1049675); // At your skill level, you can only lore tamed or tameable creatures.

                        else
                        {
                            m_From.CloseGump(typeof(AnimalLoreGump));
                            m_From.SendGump(new AnimalLoreGump(c));
                        }
                    }
                    else
                        m_From.SendAsciiMessage("This is neither a player or a basecreature"); //Should never come to this
                }
                else
                    m_Targeted.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("You can't think of anything you know offhand."), m_From.NetState);

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
                
                //Sphere-style
                /*
                                if (m_From.CheckTargetSkill(SkillName.AnimalLore, m_Targeted, 0.0, 100.0))
                                {
                                    if (m_Targeted is BaseCreature)
                                    {
                                        BaseCreature c = (BaseCreature)m_Targeted;

                                        if (!c.Controlled)
                                        {
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It is it's own master."), m_From.NetState);
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It looks {0}.", GetHunger(c)), m_From.NetState);
                                        }
                                        else if (c.Controlled && c.ControlMaster == m_From)
                                        {
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It is loyal to you."), m_From.NetState);
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It looks {0}.", GetLoyality(c)), m_From.NetState);
                                        }
                                        else if (c.Controlled && c.ControlMaster != null && c.ControlMaster != m_From)
                                        {
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It is loyal to {0}", c.ControlMaster.Name), m_From.NetState);
                                            c.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("It looks {0}.", GetLoyality(c)), m_From.NetState);
                                        }
                                    }
                                    else if (m_Targeted is PlayerMobile)
                                    {
                                        PlayerMobile pm = (PlayerMobile)m_Targeted;
                                        string gender = pm.Body.IsFemale ? "female" : "man";

                                        pm.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} is a {1}, {2} is {3} own master.", pm.Name, gender, pm.Body.IsFemale ? "she" : "he", pm.Body.IsFemale ? "her" : "his"), m_From.NetState);
                                        pm.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} looks {1}.", pm.Body.IsFemale ? "she" : "he", GetHunger(pm)), m_From.NetState);
                                    }
                                }
                                else
                                    m_Targeted.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("You can't think of anything you know offhand."), m_From.NetState);

                                if (m_From is PlayerMobile)
                                    ((PlayerMobile)m_From).EndPlayerAction();
                            }

                            private string GetLoyality(BaseCreature from)
                            {
                                int l = (int)(from.Loyalty / 2);

                                if (l >= 1 && l <= 6)
                                    return "confused";
                                else if (l >= 7 && l <= 13)
                                    return "very unhappy";
                                else if (l >= 14 && l <= 20)
                                    return "unhappy";
                                else if (l >= 21 && l <= 27)
                                    return "fairly content";
                                else if (l >= 28 && l <= 35)
                                    return "content";
                                else if (l >= 36 && l <= 41)
                                    return "happy";
                                else if (l >= 42 && l <= 49)
                                    return "very happy";
                                else
                                    return "extremely happy";
                            }
                                */
            }

            private static string GetHunger(Mobile from)
            {
                int h = from.Hunger;

                if (h == 0 || h == 1)
                    return "starving";
                if (h == 2 || h == 3)
                    return "very hungry";
                if (h == 4 || h == 5)
                    return "hungry";
                if (h == 6 || h == 7)
                    return "fairly content";
                if (h == 8 || h == 9)
                    return "content";
                if (h == 10 || h == 11)
                    return "fed";
                if (h == 12 || h == 13)
                    return "well fed";
                return "stuffed";
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You can't think of anything off hand");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}

	public class AnimalLoreGump : Gump
	{
		private static string FormatSkill( Mobile c, SkillName name )
		{
			Skill skill = c.Skills[name];

			if ( skill.Base < 10.0 )
				return "<div align=right>---</div>";

			return String.Format( "<div align=right>{0:F1}</div>", skill.Value );
		}

		private static string FormatAttributes( int cur, int max )
		{
			if ( max == 0 )
				return "<div align=right>---</div>";

			return String.Format( "<div align=right>{0}/{1}</div>", cur, max );
		}

		private static string FormatStat( int val )
		{
			if ( val == 0 )
				return "<div align=right>---</div>";

			return String.Format( "<div align=right>{0}</div>", val );
		}

		private static string FormatDouble( double val )
		{
			if ( val == 0 )
				return "<div align=right>---</div>";

			return String.Format( "<div align=right>{0:F1}</div>", val );
		}

		private static string FormatElement( int val )
		{
			if ( val <= 0 )
				return "<div align=right>---</div>";

			return String.Format( "<div align=right>{0}%</div>", val );
		}

        #region Mondain's Legacy
        private static string FormatDamage(int min, int max)
        {
            if (min <= 0 || max <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}-{1}</div>", min, max);
        }
        #endregion

		private const int LabelColor = 0x24E5;

		public AnimalLoreGump( BaseCreature c ) : base( 250, 50 )
		{
			AddPage( 0 );

			AddImage( 100, 100, 2080 );
			AddImage( 118, 137, 2081 );
			AddImage( 118, 207, 2081 );
			AddImage( 118, 277, 2081 );
			AddImage( 118, 347, 2083 );

			AddHtml( 147, 108, 210, 18, String.Format( "<center><i>{0}</i></center>", c.Name ), false, false );

			AddButton( 240, 77, 2093, 2093, 2, GumpButtonType.Reply, 0 );

			AddImage( 140, 138, 2091 );
			AddImage( 140, 335, 2091 );

			int pages = ( Core.AOS ? 5 : 3 );
			int page = 0;


			#region Attributes
			AddPage( ++page );

			AddImage( 128, 152, 2086 );
			AddHtmlLocalized( 147, 150, 160, 18, 1049593, 200, false, false ); // Attributes

			AddHtmlLocalized( 153, 168, 160, 18, 1049578, LabelColor, false, false ); // Hits
			AddHtml( 280, 168, 75, 18, FormatAttributes( c.Hits, c.HitsMax ), false, false );

			AddHtmlLocalized( 153, 186, 160, 18, 1049579, LabelColor, false, false ); // Stamina
			AddHtml( 280, 186, 75, 18, FormatAttributes( c.Stam, c.StamMax ), false, false );

			AddHtmlLocalized( 153, 204, 160, 18, 1049580, LabelColor, false, false ); // Mana
			AddHtml( 280, 204, 75, 18, FormatAttributes( c.Mana, c.ManaMax ), false, false );

			AddHtmlLocalized( 153, 222, 160, 18, 1028335, LabelColor, false, false ); // Strength
			AddHtml( 320, 222, 35, 18, FormatStat( c.Str ), false, false );

			AddHtmlLocalized( 153, 240, 160, 18, 3000113, LabelColor, false, false ); // Dexterity
			AddHtml( 320, 240, 35, 18, FormatStat( c.Dex ), false, false );

			AddHtmlLocalized( 153, 258, 160, 18, 3000112, LabelColor, false, false ); // Intelligence
			AddHtml( 320, 258, 35, 18, FormatStat( c.Int ), false, false );

			if ( Core.AOS )
			{
				int y = 276;

			if ( Core.SE )
			{
					double bd = BaseInstrument.GetBaseDifficulty( c );
					if ( c.Uncalmable )
						bd = 0;

					AddHtmlLocalized( 153, 276, 160, 18, 1070793, LabelColor, false, false ); // Barding Difficulty
					AddHtml( 320, y, 35, 18, FormatDouble( bd ), false, false );

					y += 18;
			}

				AddImage( 128, y + 2, 2086 );
				AddHtmlLocalized( 147, y, 160, 18, 1049594, 200, false, false ); // Loyalty Rating
				y += 18;

				AddHtmlLocalized( 153, y, 160, 18, (!c.Controlled || c.Loyalty == 0) ? 1061643 : 1049595 + (c.Loyalty / 10), LabelColor, false, false );
			}
			else
			{
				AddImage( 128, 278, 2086 );
				AddHtmlLocalized( 147, 276, 160, 18, 3001016, 200, false, false ); // Miscellaneous

				AddHtmlLocalized( 153, 294, 160, 18, 1049581, LabelColor, false, false ); // Armor Rating
				AddHtml( 320, 294, 35, 18, FormatStat( c.VirtualArmor ), false, false );

                AddHtml(153, 312, 160, 18, string.Format("<basefont color=#4a3929>{0}</basefont>", "Karma"), false, false); //Karma
                AddHtml(320, 312, 35, 18, FormatStat(c.Karma), false, false);
			}

			AddButton( 340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1 );
			AddButton( 317, 358, 5603, 5607, 0, GumpButtonType.Page, pages );
			#endregion

			#region Resistances
			if ( Core.AOS )
			{
				AddPage( ++page );

				AddImage( 128, 152, 2086 );
				AddHtmlLocalized( 147, 150, 160, 18, 1061645, 200, false, false ); // Resistances

				AddHtmlLocalized( 153, 168, 160, 18, 1061646, LabelColor, false, false ); // Physical
				AddHtml( 320, 168, 35, 18, FormatElement( c.PhysicalResistance ), false, false );

				AddHtmlLocalized( 153, 186, 160, 18, 1061647, LabelColor, false, false ); // Fire
				AddHtml( 320, 186, 35, 18, FormatElement( c.FireResistance ), false, false );

				AddHtmlLocalized( 153, 204, 160, 18, 1061648, LabelColor, false, false ); // Cold
				AddHtml( 320, 204, 35, 18, FormatElement( c.ColdResistance ), false, false );

				AddHtmlLocalized( 153, 222, 160, 18, 1061649, LabelColor, false, false ); // Poison
				AddHtml( 320, 222, 35, 18, FormatElement( c.PoisonResistance ), false, false );

				AddHtmlLocalized( 153, 240, 160, 18, 1061650, LabelColor, false, false ); // Energy
				AddHtml( 320, 240, 35, 18, FormatElement( c.EnergyResistance ), false, false );

                #region Mondain's Legacy
                if (Core.ML)
                {
                    AddHtmlLocalized(153, 258, 160, 18, 1076750, LabelColor, false, false); // Base Damage
                    AddHtml(300, 258, 55, 18, FormatDamage(c.DamageMin, c.DamageMax), false, false);
                }
                #endregion

				AddButton( 340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1 );
				AddButton( 317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1 );
			}
			#endregion

			#region Damage
			if ( Core.AOS )
			{
				AddPage( ++page );

				AddImage( 128, 152, 2086 );
				AddHtmlLocalized( 147, 150, 160, 18, 1017319, 200, false, false ); // Damage

				AddHtmlLocalized( 153, 168, 160, 18, 1061646, LabelColor, false, false ); // Physical
				AddHtml( 320, 168, 35, 18, FormatElement( c.PhysicalDamage ), false, false );

				AddHtmlLocalized( 153, 186, 160, 18, 1061647, LabelColor, false, false ); // Fire
				AddHtml( 320, 186, 35, 18, FormatElement( c.FireDamage ), false, false );

				AddHtmlLocalized( 153, 204, 160, 18, 1061648, LabelColor, false, false ); // Cold
				AddHtml( 320, 204, 35, 18, FormatElement( c.ColdDamage ), false, false );

				AddHtmlLocalized( 153, 222, 160, 18, 1061649, LabelColor, false, false ); // Poison
				AddHtml( 320, 222, 35, 18, FormatElement( c.PoisonDamage ), false, false );

				AddHtmlLocalized( 153, 240, 160, 18, 1061650, LabelColor, false, false ); // Energy
				AddHtml( 320, 240, 35, 18, FormatElement( c.EnergyDamage ), false, false );

				AddButton( 340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1 );
				AddButton( 317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1 );
			}
			#endregion

			#region Skills
			AddPage( ++page );

			AddImage( 128, 152, 2086 );
			AddHtmlLocalized( 147, 150, 160, 18, 3001030, 200, false, false ); // Combat Ratings

			AddHtmlLocalized( 153, 168, 160, 18, 1044103, LabelColor, false, false ); // Wrestling
			AddHtml( 320, 168, 35, 18, FormatSkill( c, SkillName.Wrestling ), false, false );

			AddHtmlLocalized( 153, 186, 160, 18, 1044087, LabelColor, false, false ); // Tactics
			AddHtml( 320, 186, 35, 18, FormatSkill( c, SkillName.Tactics ), false, false );

			AddHtmlLocalized( 153, 204, 160, 18, 1044086, LabelColor, false, false ); // Magic Resistance
			AddHtml( 320, 204, 35, 18, FormatSkill( c, SkillName.MagicResist ), false, false );

			AddHtmlLocalized( 153, 222, 160, 18, 1044061, LabelColor, false, false ); // Anatomy
			AddHtml( 320, 222, 35, 18, FormatSkill( c, SkillName.Anatomy ), false, false );

            #region Mondain's Legacy
            if (c is CuSidhe)
            {
                AddHtmlLocalized(153, 240, 160, 18, 1044077, LabelColor, false, false); // Healing
                AddHtml(320, 240, 35, 18, FormatSkill(c, SkillName.Healing), false, false);
            }
            else
            {
                AddHtmlLocalized(153, 240, 160, 18, 1044090, LabelColor, false, false); // Poisoning
                AddHtml(320, 240, 35, 18, FormatSkill(c, SkillName.Poisoning), false, false);
            }
            #endregion

			AddImage( 128, 260, 2086 );
			AddHtmlLocalized( 147, 258, 160, 18, 3001032, 200, false, false ); // Lore & Knowledge

			AddHtmlLocalized( 153, 276, 160, 18, 1044085, LabelColor, false, false ); // Magery
			AddHtml( 320, 276, 35, 18, FormatSkill( c, SkillName.Magery ), false, false );

			AddHtmlLocalized( 153, 294, 160, 18, 1044076, LabelColor, false, false ); // Evaluating Intelligence
			AddHtml( 320, 294, 35, 18,FormatSkill( c, SkillName.EvalInt ), false, false );

			AddHtmlLocalized( 153, 312, 160, 18, 1044106, LabelColor, false, false ); // Meditation
			AddHtml( 320, 312, 35, 18, FormatSkill( c, SkillName.Meditation ), false, false );

			AddButton( 340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1 );
			AddButton( 317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1 );
			#endregion

			#region Misc
			AddPage( ++page );

			AddImage( 128, 152, 2086 );
			AddHtmlLocalized( 147, 150, 160, 18, 1049563, 200, false, false ); // Preferred Foods

			int foodPref = 3000340;

			if ( (c.FavoriteFood & FoodType.FruitsAndVegies) != 0 )
				foodPref = 1049565; // Fruits and Vegetables
			else if ( (c.FavoriteFood & FoodType.GrainsAndHay) != 0 )
				foodPref = 1049566; // Grains and Hay
			else if ( (c.FavoriteFood & FoodType.Fish) != 0 )
				foodPref = 1049568; // Fish
			else if ( (c.FavoriteFood & FoodType.Meat) != 0 )
				foodPref = 1049564; // Meat
            else if ((c.FavoriteFood & FoodType.Eggs) != 0)
                foodPref = 1044477; // Eggs

			AddHtmlLocalized( 153, 168, 160, 18, foodPref, LabelColor, false, false );

			AddImage( 128, 188, 2086 );
			AddHtmlLocalized( 147, 186, 160, 18, 1049569, 200, false, false ); // Pack Instincts

			int packInstinct = 3000340;

			if ( (c.PackInstinct & PackInstinct.Canine) != 0 )
				packInstinct = 1049570; // Canine
			else if ( (c.PackInstinct & PackInstinct.Ostard) != 0 )
				packInstinct = 1049571; // Ostard
			else if ( (c.PackInstinct & PackInstinct.Feline) != 0 )
				packInstinct = 1049572; // Feline
			else if ( (c.PackInstinct & PackInstinct.Arachnid) != 0 )
				packInstinct = 1049573; // Arachnid
			else if ( (c.PackInstinct & PackInstinct.Daemon) != 0 )
				packInstinct = 1049574; // Daemon
			else if ( (c.PackInstinct & PackInstinct.Bear) != 0 )
				packInstinct = 1049575; // Bear
			else if ( (c.PackInstinct & PackInstinct.Equine) != 0 )
				packInstinct = 1049576; // Equine
			else if ( (c.PackInstinct & PackInstinct.Bull) != 0 )
				packInstinct = 1049577; // Bull

			AddHtmlLocalized( 153, 204, 160, 18, packInstinct, LabelColor, false, false );

            AddImage(128, 224, 2086);
            AddHtml(147, 222, 160, 18, "<basefont color=#003142>Loyal To</basefont>", false, false);
            AddHtml(153, 240, 160, 18, string.Format("<basefont color=#4a3929>{0}</basefont>", (!c.Controlled || c.ControlMaster == null) ? "Nobody" : c.ControlMaster.Name), false, false);

			if ( !Core.AOS )
			{
				AddImage( 128, 260, 2086 );
				AddHtmlLocalized( 147, 258, 160, 18, 1049594, 200, false, false ); // Loyalty Rating

				AddHtmlLocalized( 153, 276, 160, 18, (!c.Controlled || c.Loyalty == 0) ? 1061643 : 1049595 + (c.Loyalty / 10), LabelColor, false, false );
			}

            AddImage(128, 296, 2086);
            AddHtml(147, 296, 160, 18, "<basefont color=#003142>Tamers</basefont>", false, false);
            AddHtml(153, 312, 160, 18, string.Format("<basefont color=#4a3929>{0}</basefont>", c.Owners.Count), false, false);

			AddButton( 340, 358, 5601, 5605, 0, GumpButtonType.Page, 1 );
			AddButton( 317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1 );
			#endregion
		}
	}
}