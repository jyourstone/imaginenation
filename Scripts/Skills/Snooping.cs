using System;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.SkillHandlers
{
	public class Snooping
	{
		public static void Configure()
		{
			Container.SnoopHandler = Container_Snoop;
		}

		public static bool CheckSnoopAllowed( Mobile from, Mobile to )
		{
			Map map = from.Map;

			if ( to.Player )
				return from.CanBeHarmful( to, false, true ); // normal restrictions

			if ( map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0 )
				return true; // felucca you can snoop anybody

			GuardedRegion reg = (GuardedRegion) to.Region.GetRegion( typeof( GuardedRegion ) );

			if ( reg == null || reg.IsDisabled() )
				return true; // not in town? we can snoop any npc

			BaseCreature cret = to as BaseCreature;

			if ( to.Body.IsHuman && (cret == null || (!cret.AlwaysAttackable && !cret.AlwaysMurderer)) )
				return false; // in town we cannot snoop blue human npcs

			return true;
		}

		public static void Container_Snoop( Container cont, Mobile from )
		{

            if (from.BeginAction(typeof(IAction)))
            {
                bool releaseLock = true;
                bool canSnoop = true;

                if (from.AccessLevel > AccessLevel.Player || from.InRange(cont.GetWorldLocation(), 1))
                {
                    Mobile root = cont.RootParent as Mobile;

                    if (root != null && !root.Alive)
                        canSnoop = false;
                    else if (root != null && root.AccessLevel > AccessLevel.Player && from.AccessLevel == AccessLevel.Player)
                    {
                        from.SendLocalizedMessage(500209); // You can not peek into the container.
                        canSnoop = false;
                    }
                    else if (root != null && from.AccessLevel == AccessLevel.Player && !CheckSnoopAllowed(from, root))
                    {
                        from.SendLocalizedMessage(1001018); // You cannot perform negative acts on your target.
                        canSnoop = false;
                    }

                    if (canSnoop)
                    {
                        if (from.AccessLevel > AccessLevel.Player)
                            cont.DisplayTo(from);
                        else
                        {
                            new InternalTimer(from, cont).Start();
                            releaseLock = false;
                        }
                    }
                }
                else
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

                if (releaseLock && from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");
		}

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly Container m_Container;
            private readonly Mobile m_Root;

            public InternalTimer(Mobile from, Container container)
                : base(TimeSpan.FromSeconds(2.5))
            {
                m_From = from;
                m_Container = container;
                m_Root = m_Container.RootParent as Mobile;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.InRange(m_Container.GetWorldLocation(), 1))
                {
                    if (m_Root == null)
                        return;

                    if (!m_Root.Alive)
                        return;

                    if (!CheckSnoopAllowed(m_From, m_Root))
                    {
                        m_From.SendLocalizedMessage(1001018); // You cannot perform negative acts on your target.
                        return;
                    }

                    if (m_From.CheckTargetSkill(SkillName.Snooping, m_Container, 0.0, 100.0 - (m_From.Dex - m_Root.Dex - 5)))
                    {
                        if (m_Container is TrapableContainer && ((TrapableContainer)m_Container).ExecuteTrap(m_From))
                            return;

                        m_Container.DisplayTo(m_From);

                        if (m_Root is PlayerMobile)
                        {
                            PlayerMobile pm = (PlayerMobile)m_Root;
                            pm.Snoopers.Add(m_From);
                            new SnoopTimer(pm, m_From).Start();
                        }
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500210); // You failed to peek into the container.

                        SendSnoopMessage();

                        if (m_From.Karma > -2000)
                            Titles.AwardKarma(m_From, -4, true);

                        if (m_From.Skills[SkillName.Hiding].Value / 2 < Utility.Random(100))
                            m_From.RevealingAction();
                    }
                }
                else
                    m_From.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            private void SendSnoopMessage()
            {
                Map map = m_From.Map;

                if (map != null)
                {
                    string message = String.Format("You notice {0} attempting to peek into {1}'s container.", m_From.Name, m_Root.Name);

                    IPooledEnumerable eable = map.GetClientsInRange(m_From.Location, 8);

                    foreach (NetState ns in eable)
                    {
                        if (ns == m_Root.NetState)
                            m_From.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format(("You notice {0} attempting to peek into your container"), m_From.Name), ns.Mobile.NetState);
                        else if (ns != m_From.NetState)
                            m_From.PrivateOverheadMessage(MessageType.Regular, 906, true, message, ns.Mobile.NetState);
                    }

                    eable.Free();
                }
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(500210); // You failed to peek into the container.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }

        private class SnoopTimer : Timer
        {
            private readonly PlayerMobile m_Victim;
            private readonly Mobile m_Snooper;

            public SnoopTimer(PlayerMobile victim, Mobile snooper)
                : base(TimeSpan.FromSeconds(55))
            {
                m_Victim = victim;
                m_Snooper = snooper;
            }

            protected override void OnTick()
            {
                m_Victim.Snoopers.Remove(m_Snooper);
            }
        }

    }
}