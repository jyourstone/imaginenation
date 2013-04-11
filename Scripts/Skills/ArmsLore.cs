using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class ArmsLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.ArmsLore].Callback = OnUse;
		}

		public static TimeSpan OnUse(Mobile m)
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.SendAsciiMessage("What would you like to evaluate?");
                m.Target = new InternalTarget();
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target, IAction
		{
			public InternalTarget() : base( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaseLock = true;

                if (targeted is BaseWeapon)
                {
                    releaseLock = false;
                    new InternalTimer(from, (BaseWeapon)targeted).Start();
                }
                else if (targeted is BaseArmor)
                {
                    releaseLock = false;
                    new InternalTimer(from, (BaseArmor)targeted).Start();
                }
                    /*
                else if (targeted is SwampDragon && ((SwampDragon)targeted).HasBarding)
                {
                    SwampDragon pet = (SwampDragon)targeted;

                    if (from.CheckTargetSkill(SkillName.ArmsLore, targeted, 0, 100))
                    {
                        int perc = (4 * pet.BardingHP) / pet.BardingMaxHP;

                        if (perc < 0)
                            perc = 0;
                        else if (perc > 4)
                            perc = 4;

                        pet.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1053021 - perc, from.NetState);
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                     */
                else
                {
                    from.SendLocalizedMessage(500352); // This is neither weapon nor armor.
                }

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
                if (m_Targeted is BaseWeapon)
                {
                    if (m_From.CheckTargetSkill(SkillName.ArmsLore, m_Targeted, 0, 100))
                    {
                        BaseWeapon weap = (BaseWeapon)m_Targeted;
                        /*
                        string itemCondition;

                        if (weap.HitPoints > weap.MaxHitPoints)
                            itemCondition = "absolutely flawless";
                        else if (weap.HitPoints == weap.MaxHitPoints)
                            itemCondition = "in full repair";
                        else if (weap.HitPoints > weap.MaxHitPoints / 2)
                            itemCondition = "a bit worn";
                        else if (weap.HitPoints > weap.MaxHitPoints / 3)
                            itemCondition = "well worn";
                        else if (weap.HitPoints > 3)
                            itemCondition = "badly damaged";
                        else
                            itemCondition = "about to fall apart";
                         
                        m_From.SendAsciiMessage(string.Format("Attack [{0}]. This item is {1}. It is repairable. {2}", GetScaledValue(m_From, weap.MaxDamage), itemCondition,weap.PoisonCharges > 0 ? "It appears to be poisoned" : ""));
                        */
                        m_From.SendAsciiMessage("Attack: " + weap.MaxDamage);
                        m_From.SendAsciiMessage("Speed: " + weap.Speed);
                        if (weap.DurabilityLevel != WeaponDurabilityLevel.Indestructible)
                            m_From.SendAsciiMessage("Durability: {0} of {1}", weap.HitPoints, weap.MaxHitPoints);
                        else
                            m_From.SendAsciiMessage("Durability: Indestructible");
                        m_From.SendAsciiMessage("It is repairable");
                        if (weap.PoisonCharges > 0)
                            m_From.SendAsciiMessage("It appears to be poisoned");

                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else if (m_Targeted is BaseArmor)
                {
                    if (m_From.CheckTargetSkill(SkillName.ArmsLore, m_Targeted, 0, 100))
                    {
                        BaseArmor arm = (BaseArmor)m_Targeted;
                        /*
                        string itemCondition;

                        if (arm.HitPoints > arm.MaxHitPoints)
                            itemCondition = "absolutely flawless";
                        else if (arm.HitPoints == arm.MaxHitPoints)
                            itemCondition = "in full repair";
                        else if (arm.HitPoints > arm.MaxHitPoints / 2)
                            itemCondition = "a bit worn";
                        else if (arm.HitPoints > arm.MaxHitPoints / 3)
                            itemCondition = "well worn";
                        else if (arm.HitPoints > 3)
                            itemCondition = "badly damaged";
                        else
                            itemCondition = "about to fall apart";

                        m_From.SendAsciiMessage(string.Format("Defense [{0}]. This item is {1}. It is repairable.", GetScaledValue(m_From, (int)arm.ArmorRating), itemCondition));
                        */

                        m_From.SendAsciiMessage("Defense: " + arm.ArmorRating);
                        if (arm.Durability != ArmorDurabilityLevel.Indestructible)
                            m_From.SendAsciiMessage("Durability: {0} of {1}", arm.HitPoints, arm.MaxHitPoints);
                        else
                            m_From.SendAsciiMessage("Durability: Indestructible");
                        m_From.SendAsciiMessage("It is repairable");
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500353); // You are not certain...
                    }
                }

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }
            /*
            public int GetScaledValue(Mobile m, int value)
            {
                return value;
            }
            */
            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You are uncertain about this item");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}