using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class ItemIdentification
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.ItemID].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile from )
		{
            if (from.BeginAction(typeof(IAction)))
            {
                from.SendLocalizedMessage(500343); // What do you wish to appraise and identify?
                from.Target = new InternalTarget();
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");

			return TimeSpan.Zero;
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target, IAction
		{
			public InternalTarget() :  base ( 8, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object o )
			{
                bool releaseLock = true;

                if (o is Item)
                {
                    releaseLock = false;
                    new InternalTimer(from, o as Item).Start();
                }
                else if (o is Mobile)
                    from.SendAsciiMessage("Thats not an item!");
                else
                    from.SendLocalizedMessage(500353); // You are not certain...

                //allows the identify skill to reveal attachments
                Server.Engines.XmlSpawner2.XmlAttach.RevealAttachments(from, o);

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
            private readonly Item m_Targeted;

            public InternalTimer(Mobile from, Item targeted)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_Targeted.Deleted)
                    return;

                if (m_From.CheckTargetSkill(SkillName.ItemID, m_Targeted, 0, 100))
                {
                    if (m_Targeted is BaseWeapon)
                    {
                        if (((BaseWeapon)m_Targeted).Identified)
                            m_From.SendAsciiMessage("That is already identified.");
                        else
                            ((BaseWeapon)m_Targeted).Identified = true;
                    }
                    else if (m_Targeted is BaseArmor)
                    {
                        if (((BaseArmor)m_Targeted).Identified)
                            m_From.SendAsciiMessage("That is already identified.");
                        else
                            ((BaseArmor)m_Targeted).Identified = true;
                    }
                    else if (m_Targeted is CustomQuestItem)
                    {
                        CustomQuestItem cqi = m_Targeted as CustomQuestItem;

                        if (!string.IsNullOrEmpty(cqi.HiddenMessage))
                            cqi.LabelTo(m_From, cqi.HiddenMessage);
                    }
                    else
                        m_From.SendAsciiMessage("You already know what kind of item that is.");

                    if (m_Targeted.LootType == LootType.Blessed || m_Targeted.LootType == LootType.Newbied)
                        m_From.SendAsciiMessage("It has a slight magical aura.");
                }
                else
                    m_From.SendLocalizedMessage(500353); // You are not certain...

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(500353); // You are not certain...

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}