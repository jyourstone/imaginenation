using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class Stealing
	{
		public static void Initialize()
		{
			SkillInfo.Table[33].Callback = OnUse;
		}

		public static readonly bool ClassicMode = false;
		public static readonly bool SuspendOnMurder = false;

		public static bool IsInGuild( Mobile m )
		{
			return ( m is PlayerMobile && ((PlayerMobile)m).NpcGuild == NpcGuild.ThievesGuild );
		}

		public static bool IsInnocentTo( Mobile from, Mobile to )
		{
			return ( Notoriety.Compute( from, to ) == Notoriety.Innocent );
		}

        private static bool TryStealItem(Item toSteal, object root, Mobile m_Thief)
        {
            PlayerMobile pm = null;

            if (root is PlayerMobile)
                pm = (PlayerMobile)root;

            if (m_Thief.AccessLevel == AccessLevel.Counselor)
                m_Thief.SendAsciiMessage("Naughty counselor!");
            else if (m_Thief.AccessLevel > AccessLevel.Counselor)
                m_Thief.SendAsciiMessage("You don't need to steal anything, just take what you want!");
            else if (m_Thief is PlayerMobile && ((PlayerMobile)m_Thief).Young)
                m_Thief.SendAsciiMessage("You cannot steal anything as a young player.");
            else if (!IsEmptyHanded(m_Thief))
                m_Thief.SendLocalizedMessage(1005584); // Both hands must be free to steal.
            else if (root is BaseVendor && ((BaseVendor)root).IsInvulnerable)
                m_Thief.SendLocalizedMessage(1005598); // You can't steal from shopkeepers.
            else if (root is PlayerVendor)
                m_Thief.SendLocalizedMessage(502709); // You can't steal from vendors.
            else if (!m_Thief.CanSee(toSteal))
                m_Thief.SendLocalizedMessage(500237); // Target can not be seen.
            else if (m_Thief.Backpack == null || !m_Thief.Backpack.CheckHold(m_Thief, toSteal, false, true))
                m_Thief.SendLocalizedMessage(1048147); // Your backpack can't hold anything else.
            //ARTEGORDONMOD
            // allow stealing of STEALABLE items on the ground or in containers
            else if ( (toSteal.Parent == null || !toSteal.Movable) && !ItemFlags.GetStealable(toSteal) )
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!
            else if (toSteal.LootType == LootType.Newbied || toSteal.CheckBlessed(root))
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!
            else if (toSteal is Container)
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!
            else if (!m_Thief.InRange(toSteal.GetWorldLocation(), 1))
                m_Thief.SendLocalizedMessage(502703); // You must be standing next to an item to steal it.
            else if (toSteal.Parent is Mobile)
                m_Thief.SendLocalizedMessage(1005585); // You cannot steal items which are equiped.
            else if (root == m_Thief)
                m_Thief.SendLocalizedMessage(502704); // You catch yourself red-handed.
            else if (root is Mobile && ((Mobile)root).AccessLevel > AccessLevel.Player)
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!
            else if (root is Mobile && !m_Thief.CanBeHarmful((Mobile)root))
                m_Thief.SendAsciiMessage("Something prevents you to steal from that mobile");
            /*else if (root is Corpse)
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!*/
            else if (pm != null && !pm.Snoopers.Contains(m_Thief))
                m_Thief.SendAsciiMessage("You waited too long between snooping and stealing the item");
            else if (m_Thief.SolidHueOverride == 2535)
                m_Thief.SendAsciiMessage("You cannot steal while using the pitsrune");
            else if (toSteal.UnStealable)
                m_Thief.SendLocalizedMessage(502710); // You can't steal that!
            else
            {
                double w = toSteal.Weight + toSteal.TotalWeight;

                if (w > 15)
                {
                    m_Thief.SendMessage("That is too heavy to steal.");
                }
                else
                    return true;
            }

            return false;
        }

		private class StealingTarget : Target, IAction
		{
			private readonly Mobile m_Thief;

			public StealingTarget( Mobile thief ) : base ( 1, false, TargetFlags.None )
			{
				m_Thief = thief;

				AllowNonlocal = true;
			}

		    private void BeginStealItem( Item toSteal, object root )
			{
	            bool releaseLock = true;

                if (TryStealItem(toSteal, root, m_Thief))
                {
                    new StealTimer(m_Thief, toSteal).Start();
                    releaseLock = false;
                }

		        if (releaseLock && m_Thief is PlayerMobile)
                    ((PlayerMobile)m_Thief).EndPlayerAction();
			}

			protected override void OnTarget( Mobile from, object target )
			{
                object root;

				from.RevealingAction();

				if ( target is Item )
				{
					root = ((Item)target).RootParent;
                    BeginStealItem((Item)target, root);
				} 
				else if ( target is Mobile )
				{
					Container pack = ((Mobile)target).Backpack;

                    if (pack != null && pack.Items.Count > 0)
                    {
                        int randomIndex = Utility.Random(pack.Items.Count);

                        root = target;
                        BeginStealItem(pack.Items[randomIndex], root);
                    }
                    else
                    {
                        m_Thief.SendAsciiMessage("That mobile has nothing to steal");
                        if (m_Thief is PlayerMobile)
                            ((PlayerMobile) m_Thief).EndPlayerAction();
                    }
				} 
				else 
				{
					m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
                    if (m_Thief is PlayerMobile)
                        ((PlayerMobile)m_Thief).EndPlayerAction();
				}
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

		public static bool IsEmptyHanded( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.OneHanded ) != null )
				return false;

			if ( from.FindItemOnLayer( Layer.TwoHanded ) != null )
				return false;

			return true;
		}

		public static TimeSpan OnUse( Mobile m )
		{          
            if (m.BeginAction(typeof(IAction)))
            {
                if (!IsEmptyHanded(m))
                {
                    m.SendLocalizedMessage(1005584); // Both hands must be free to steal.
                    if (m is PlayerMobile)
                        ((PlayerMobile)m).EndPlayerAction();
                }
                else
                {
                    m.Target = new StealingTarget(m);
                    m.RevealingAction();

                    m.SendLocalizedMessage(502698); // Which item do you want to steal?
                }
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
            
		}

        private class StealTimer : Timer, IAction
        {
            private readonly Mobile m_Thief;
            private readonly Item toSteal;

            public StealTimer(Mobile thief, Item steal)
                : base(TimeSpan.FromSeconds(4.0))
            {
                m_Thief = thief;
                toSteal = steal;

                if (m_Thief is PlayerMobile)
                    ((PlayerMobile)m_Thief).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                Item stolen = null;
                object root = toSteal.RootParent;

                if (!TryStealItem(toSteal, root, m_Thief))
                {
                    if (m_Thief is PlayerMobile)
                        ((PlayerMobile) m_Thief).EndPlayerAction();
                    
                    return;
                }

                if (toSteal.Stackable && toSteal.Amount > 1)
                {
                    int maxAmount;

                    if (toSteal.Weight == 0)
                        maxAmount = (int)((m_Thief.Skills[SkillName.Stealing].Value / 10.0) / 0.1);
                    else
                        maxAmount = (int)((m_Thief.Skills[SkillName.Stealing].Value / 10.0) / toSteal.Weight);
                    
                    if (maxAmount < 1)
                        maxAmount = 1;
                    else if (maxAmount > toSteal.Amount)
                        maxAmount = toSteal.Amount;

                    int amount = Utility.RandomMinMax(1, maxAmount);

                    int pileWeight = (int)Math.Ceiling(toSteal.Weight * amount);
                    pileWeight *= 10;

                    if (pileWeight == 0)
                        pileWeight = 1;

                    double minSkill = -(m_Thief.Skills[SkillName.Stealing].Value / 2) + 50;
                    double maxSkill = 100 + (pileWeight * 3);

                    if (amount >= toSteal.Amount)
                    {
                        if (m_Thief.CheckTargetSkill(SkillName.Stealing, toSteal, minSkill, maxSkill))
                            stolen = toSteal;
                    }
                    else
                    {
                        if (m_Thief.CheckTargetSkill(SkillName.Stealing, toSteal, minSkill, maxSkill))
                        {
                            stolen = Mobile.LiftItemDupe(toSteal, toSteal.Amount - amount);

                            if (stolen == null)
                                stolen = toSteal;
                        }
                    }
                }
                else
                {
                    double w = toSteal.Weight + toSteal.TotalWeight;

                    int iw = (int)Math.Ceiling(w);
                    iw *= 10;

                    double minSkill = -(m_Thief.Skills[SkillName.Stealing].Value/2) + 50;
                    double maxSkill = 100 + (iw*3);

                    if (m_Thief.CheckTargetSkill(SkillName.Stealing, toSteal, minSkill, maxSkill))
                        stolen = toSteal;
                }

                bool caught;
                if (root is Mobile)
                {
                    Mobile mobRoot = (Mobile) root;
                    caught = ((m_Thief.Skills[SkillName.Stealing].Value + (m_Thief.Dex - mobRoot.Dex) / 3.0) < Utility.Random(150));
                }
                else
                    caught = ((m_Thief.Skills[SkillName.Stealing].Value + (m_Thief.Dex - 100) / 3.0) < Utility.Random(150));

                if (stolen != null)
                {
                    // ARTEGORDONMOD
                    // set the taken flag to trigger release from any controlling spawner
                    ItemFlags.SetTaken(stolen, true);
                    // clear the stealable flag so that the item can only be stolen once if it is later locked down.
                    ItemFlags.SetStealable(stolen, false);
                    // release it if it was locked down
                    stolen.Movable = true;

                    m_Thief.AddToBackpack(stolen);
                    StolenItem.Add(stolen, m_Thief, root as Mobile);

                    m_Thief.SendLocalizedMessage(502724); // You succesfully steal the item.
                }
                else
                {
                    m_Thief.SendLocalizedMessage(502723); // You fail to steal the item.
                }

                if (caught)
                {
                    if (root == null)
                    {
                        m_Thief.CriminalAction(true);
                    }
                    else if (root is Corpse && ((Corpse)root).IsCriminalAction(m_Thief))
                    {
                        m_Thief.CriminalAction(true);
                    }
                    else if (root is Mobile)
                    {
                        Mobile mobRoot = (Mobile)root;

                        if (!IsInGuild(mobRoot) && IsInnocentTo(m_Thief, mobRoot))
                            m_Thief.CriminalAction(true);

                        string message = String.Format("You notice {0} trying to steal from {1}.", m_Thief.Name, mobRoot.Name);

                        foreach (NetState ns in m_Thief.GetClientsInRange(8))
                        {
                            if (ns.Mobile == mobRoot)
                                ns.Mobile.SendAsciiMessage("You notice {0} trying to steal from you.", m_Thief.Name);

                            else if (ns.Mobile != m_Thief)
                                ns.Mobile.SendMessage(message);
                        }
                    }
                }
                /*
                else if (root is Corpse && ((Corpse)root).IsCriminalAction(m_Thief))
                {
                    m_Thief.CriminalAction(true);
                }*/

                if (root is Mobile && ((Mobile)root).Player && m_Thief is PlayerMobile && IsInnocentTo(m_Thief, (Mobile)root) && !IsInGuild((Mobile)root))
                {
                    PlayerMobile pm = (PlayerMobile)m_Thief;

                    pm.PermaFlags.Add((Mobile)root);
                    pm.Delta(MobileDelta.Noto);
                }

                if (m_Thief is PlayerMobile)
                    ((PlayerMobile)m_Thief).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(502723); // You fail to steal the item.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}


    public class StolenItem
	{
		public static readonly TimeSpan StealTime = TimeSpan.FromMinutes( 1.0 );

		private readonly Item m_Stolen;
		private readonly Mobile m_Thief;
		private readonly Mobile m_Victim;
		private DateTime m_Expires;

		public Item Stolen{ get{ return m_Stolen; } }
		public Mobile Thief{ get{ return m_Thief; } }
		public Mobile Victim{ get{ return m_Victim; } }
		public DateTime Expires{ get{ return m_Expires; } }

		public bool IsExpired{ get{ return ( DateTime.Now >= m_Expires ); } }

		public StolenItem( Item stolen, Mobile thief, Mobile victim )
		{
			m_Stolen = stolen;
			m_Thief = thief;
			m_Victim = victim;

			m_Expires = DateTime.Now + StealTime;
		}

		private static readonly Queue m_Queue = new Queue();

		public static void Add( Item item, Mobile thief, Mobile victim )
		{
			Clean();

			m_Queue.Enqueue( new StolenItem( item, thief, victim ) );
		}

		public static bool IsStolen( Item item )
		{
			Mobile victim = null;

			return IsStolen( item, ref victim );
		}

		public static bool IsStolen( Item item, ref Mobile victim )
		{
			Clean();

			foreach ( StolenItem si in m_Queue )
			{
				if ( si.m_Stolen == item && !si.IsExpired )
				{
					victim = si.m_Victim;
					return true;
				}
			}

			return false;
		}

		public static void ReturnOnDeath( Mobile killed, Container corpse )
		{
			Clean();

			foreach ( StolenItem si in m_Queue )
			{
				if ( si.m_Stolen.RootParent == corpse && si.m_Victim != null && !si.IsExpired )
				{
					if ( si.m_Victim.AddToBackpack( si.m_Stolen ) )
						si.m_Victim.SendLocalizedMessage( 1010464 ); // the item that was stolen is returned to you.
					else
						si.m_Victim.SendLocalizedMessage( 1010463 ); // the item that was stolen from you falls to the ground.

					si.m_Expires = DateTime.Now; // such a hack
				}
			}
		}

		public static void Clean()
		{
			while ( m_Queue.Count > 0 )
			{
				StolenItem si = (StolenItem) m_Queue.Peek();

				if ( si.IsExpired )
					m_Queue.Dequeue();
				else
					break;
			}
		}
	}
}