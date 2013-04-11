using System;
using System.Collections;
using Server.Factions;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
    public class AnimalTaming
    {
        private static readonly Hashtable m_BeingTamed = new Hashtable();

        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.AnimalTaming].Callback = OnUse;
        }

        private static bool m_DisableMessage;

        public static bool DisableMessage
        {
            get { return m_DisableMessage; }
            set { m_DisableMessage = value; }
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.Target = new InternalTarget();
                m.SendLocalizedMessage(502789); // Tame which animal?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

        public static bool CheckMastery(Mobile tamer, BaseCreature creature)
        {
            BaseCreature familiar = (BaseCreature)SummonFamiliarSpell.Table[tamer];

            if (familiar != null && !familiar.Deleted && familiar is DarkWolfFamiliar)
            {
                if (creature is DireWolf || creature is GreyWolf || creature is TimberWolf || creature is WhiteWolf || creature is BakeKitsune)
                    return true;
            }

            return false;
        }

        public static bool MustBeSubdued(BaseCreature bc)
        {
            if (bc.Owners.Count > 0) { return false; } //Checks to see if the animal has been tamed before
            return bc.SubdueBeforeTame && (bc.Hits > (bc.HitsMax / 10));
        }

        public static void ScaleStats(BaseCreature bc, double scalar)
        {
            if (bc.RawStr > 0)
                bc.RawStr = (int)Math.Max(1, bc.RawStr * scalar);

            if (bc.RawDex > 0)
                bc.RawDex = (int)Math.Max(1, bc.RawDex * scalar);

            if (bc.RawInt > 0)
                bc.RawInt = (int)Math.Max(1, bc.RawInt * scalar);

            if (bc.HitsMaxSeed > 0)
            {
                bc.HitsMaxSeed = (int)Math.Max(1, bc.HitsMaxSeed * scalar);
                bc.Hits = bc.Hits;
            }

            if (bc.StamMaxSeed > 0)
            {
                bc.StamMaxSeed = (int)Math.Max(1, bc.StamMaxSeed * scalar);
                bc.Stam = bc.Stam;
            }
        }

        public static void ScaleSkills(BaseCreature bc, double scalar)
        {
            ScaleSkills(bc, scalar, scalar);
        }

        public static void ScaleSkills(BaseCreature bc, double scalar, double capScalar)
        {
            int totalCapIncrease = Core.SE ? 2000 : 0; // 2000 points for GM Anatomy and Meditation

            for (int i = 0; i < bc.Skills.Length; ++i)
            {
                bc.Skills[i].Base *= scalar;
                
				if ( bc.Skills[i].Base > bc.Skills[i].Cap )
				{
					if( Core.SE )
						totalCapIncrease += ( bc.Skills[i].BaseFixedPoint - bc.Skills[i].CapFixedPoint );

                    bc.Skills[i].Cap = bc.Skills[i].Base;
                }
			}
			
			bc.SkillsCap += totalCapIncrease;	//increase will be 0 if not .SE per above
        }

        private class InternalTarget : Target, IAction
        {
            private bool m_SetSkillTime = true;

            public InternalTarget()
                : base(6, false, TargetFlags.None)
            {
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_SetSkillTime)
                    from.NextSkillTime = DateTime.Now;
            }

            public virtual void ResetPacify(object obj)
            {
                if (obj is BaseCreature)
                {
                    ((BaseCreature)obj).BardPacified = true;
                }
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                bool releaseLock = true;

                if (targeted is Dragon || targeted is Drake || targeted is AncientWyrm || targeted is SwampDragon || targeted is Ridgeback || targeted is WhiteWyrm || targeted is Wyvern || targeted is Daemon)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();

                    from.SendAsciiMessage("You can't tame that!");
                    return;
                }

                if (targeted is GoodMustang)
                {
                    if (from.Karma < 7499 )
                    {
                        if (from is PlayerMobile)
                            ((PlayerMobile)from).EndPlayerAction();

                        from.SendAsciiMessage("You are not righteous enough to tame this animal");
                        return;
                    }
                }

                if (targeted is EvilMustang)
                {
                    if (from.Karma > -7499 )
                    {
                        if (from is PlayerMobile)
                            ((PlayerMobile)from).EndPlayerAction();

                        from.SendAsciiMessage("You are not wicked enough to tame this animal");
                        return;
                    }
                }

                if (targeted is Mobile)
                {
                    if (targeted is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)targeted;
                        bool alreadyOwned = creature.Owners.Contains(from);

                        if (!creature.Tamable)
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(1049655)); // That creature cannot be tamed.
                        }
                        else if (creature.Controlled)
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(502804)); // That animal looks tame already.
                        }
                        else if (from.Female && !creature.AllowFemaleTamer)
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(1049653)); // That creature can only be tamed by males.
                        }
                        else if (!from.Female && !creature.AllowMaleTamer)
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(1049652)); // That creature can only be tamed by females.
                        }
                        else if (creature.Owners.Count >= BaseCreature.MaxOwners && !creature.Owners.Contains(from))
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(1005615)); // This animal has had too many owners and is too upset for you to tame.
                        }
                        else if (MustBeSubdued(creature))
                        {
                            from.SendAsciiMessage(CliLoc.LocToString(1054025)); // You must subdue this creature before you can tame it!
                        }
                        else if (alreadyOwned)
                        {
                            from.SendAsciiMessage("{0} remembers you and accepts you once more as its master.", creature.Name);
                            creature.SetControlMaster(from);
                        }
                        else if (CheckMastery(from, creature) || from.Skills[SkillName.AnimalTaming].Value >= creature.MinTameSkill)
                        {
                            FactionWarHorse warHorse = creature as FactionWarHorse;

                            if (warHorse != null)
                            {
                                Faction faction = Faction.Find(from);

                                if (faction == null || faction != warHorse.Faction)
                                {
                                    if (from is PlayerMobile)
                                        ((PlayerMobile)from).EndPlayerAction();

                                    from.SendAsciiMessage(CliLoc.LocToString(1042590)); // You cannot tame this creature.
                                    return;
                                }
                            }

                            if (creature.CanAngerOnTame && 0.95 >= Utility.RandomDouble())
                            {
                                from.SendAsciiMessage(CliLoc.LocToString(502805)); // You seem to anger the beast!
                                creature.PlaySound(creature.GetAngerSound());
                                from.Direction = creature.GetDirectionTo(from);

                                if (creature.BardPacified && Utility.RandomDouble() > .24)
                                {
                                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback(ResetPacify), creature);
                                }
                                else
                                {
                                    creature.BardEndTime = DateTime.Now;
                                }

                                creature.BardPacified = false;

                                creature.Move(creature.Direction);

                                if (from is PlayerMobile && !((PlayerMobile)from).HonorActive)
                                    creature.Combatant = from;
                            }
                            else
                            {
                                releaseLock = false;
                                m_BeingTamed[targeted] = from;
                                from.SendAsciiMessage("You start to tame the creature.");
                                new InternalTimer(from, creature, Utility.Random(4, 4)).Start();

                                m_SetSkillTime = false;
                            }
                        }
                        else if (CheckMastery(from, creature) || from.Skills[SkillName.AnimalTaming].Value <= creature.MinTameSkill)
                        {
                            FactionWarHorse warHorse = creature as FactionWarHorse;

                            if (warHorse != null)
                            {
                                Faction faction = Faction.Find(from);

                                if (faction == null || faction != warHorse.Faction)
                                {
                                    if (from is PlayerMobile)
                                        ((PlayerMobile)from).EndPlayerAction();

                                    from.SendAsciiMessage(CliLoc.LocToString(1042590)); // You cannot tame this creature.
                                    return;
                                }
                            }

                            if (creature.CanAngerOnTame && 0.95 >= Utility.RandomDouble())
                            {
                                from.SendAsciiMessage(CliLoc.LocToString(502805)); // You seem to anger the beast!
                                creature.PlaySound(creature.GetAngerSound());
                                from.Direction = creature.GetDirectionTo(from);
                                creature.Combatant = from;
                            }
                            else
                            {
                                releaseLock = false;
                                m_BeingTamed[targeted] = from;
                                from.SendAsciiMessage("You start to tame the creature.");
                                new InternalTimer(from, creature, Utility.Random(4, 4)).Start();

                                m_SetSkillTime = false;
                            }
                        }
                        else
                        {
                            creature.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 502806, from.NetState); // You have no chance of taming this creature.
                        }
                    }
                    else
                    {
                        ((Mobile)targeted).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 502469, from.NetState); // That being cannot be tamed.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502801); // You can't tame that!
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

            private class InternalTimer : Timer, IAction
            {
                private readonly Mobile m_Tamer;
                private readonly BaseCreature m_Creature;
                private readonly int m_MaxCount;
                private int m_Count;
                private bool m_Paralyzed;
                private readonly DateTime m_StartTime;

                public InternalTimer(Mobile tamer, BaseCreature creature, int count)
                    : base(TimeSpan.FromSeconds(3.0), TimeSpan.FromSeconds(3.0), count)
                {
                    m_Tamer = tamer;
                    m_Creature = creature;
                    m_MaxCount = count;
                    m_Paralyzed = creature.Paralyzed;
                    m_StartTime = DateTime.Now;
                    Priority = TimerPriority.TwoFiftyMS;

                    if (tamer is PlayerMobile)
                        ((PlayerMobile)tamer).ResetPlayerAction(this);

                    m_Tamer.RevealingAction();
                    string msg = "";

                    switch (Utility.Random(3))
                    {
                        case 0:
                            if (!string.IsNullOrEmpty(m_Creature.Name))
                                msg = string.Format("I always wanted {0} like you.", m_Creature.Name);
                            else
                                msg = CliLoc.LocToString(502790);
                            break;

                        case 1:
                            if (!string.IsNullOrEmpty(m_Creature.Name))
                                msg = string.Format("Good {0}.", m_Creature.Name);
                            else
                                msg = CliLoc.LocToString(1005608);
                            break;

                        case 2:
                            if (!string.IsNullOrEmpty(m_Creature.Name))
                                msg = string.Format("Here {0}.", m_Creature.Name);
                            else
                                msg = CliLoc.LocToString(1010593);
                            break;

                        case 3:
                            if (!string.IsNullOrEmpty(m_Creature.Name))
                                msg = string.Format("I won't hurt you");
                            else
                                msg = CliLoc.LocToString(1010593);
                            break;
                    }

                    SpellHelper.Turn(m_Tamer, m_Creature);
                    m_Tamer.PublicOverheadMessage(MessageType.Regular, 906, true, msg);

                }

                protected override void OnTick()
                {
                    m_Count++;

                    DamageEntry de = m_Creature.FindMostRecentDamageEntry(false);
                    bool alreadyOwned = (m_Creature.LastOwner == m_Tamer);

                    if (!m_Tamer.InRange(m_Creature, 6))
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(502795)); // You are too far away to continue taming.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (!m_Tamer.CheckAlive())
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(502796)); // You are dead, and cannot continue taming.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (!m_Tamer.CanSee(m_Creature) || !m_Tamer.InLOS(m_Creature))// || !CanPath())
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(1049654)); // You do not have a clear path to the animal you are taming, and must cease your attempt.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (!m_Creature.Tamable)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(1049655)); // That creature cannot be tamed.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (m_Creature.Controlled)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(502804)); // That animal looks tame already.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (m_Creature.Owners.Count >= BaseCreature.MaxOwners && !m_Creature.Owners.Contains(m_Tamer))
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(1005615)); // This animal has had too many owners and is too upset for you to tame.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (MustBeSubdued(m_Creature))
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(1054025)); // You must subdue this creature before you can tame it!
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (de != null && de.LastDamage > m_StartTime)
                    {
                        m_BeingTamed.Remove(m_Creature);
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendAsciiMessage(CliLoc.LocToString(502794)); // The animal is too angry to continue taming.
                        Stop();
                        if (m_Tamer is PlayerMobile)
                            ((PlayerMobile)m_Tamer).EndPlayerAction();
                    }
                    else if (m_Count < m_MaxCount)
                    {
                        m_Tamer.RevealingAction();
                        string msg = "";

                        switch (Utility.Random(4))
                        {
                            case 0:
                                if (!string.IsNullOrEmpty(m_Creature.Name))
                                    msg = string.Format("I always wanted a {0} like you.", m_Creature.Name);
                                else
                                    msg = CliLoc.LocToString(502790);
                                break;

                            case 1:
                                if (!string.IsNullOrEmpty(m_Creature.Name))
                                    msg = string.Format("Good {0}.", m_Creature.Name);
                                else
                                    msg = CliLoc.LocToString(1005608);
                                break;

                            case 2:
                                if (!string.IsNullOrEmpty(m_Creature.Name))
                                    msg = string.Format("Here {0}.", m_Creature.Name);
                                else
                                    msg = CliLoc.LocToString(1010593);
                                break;

                            case 3:
                                if (!string.IsNullOrEmpty(m_Creature.Name))
                                    msg = string.Format("I won't hurt you");
                                else
                                    msg = CliLoc.LocToString(1010593);
                                break;
                        }

                        SpellHelper.Turn(m_Tamer, m_Creature);

                        m_Tamer.PublicOverheadMessage(MessageType.Regular, 906, true, msg);

                        if (!alreadyOwned) // Passively check animal lore for gain
                            m_Tamer.CheckTargetSkill(SkillName.AnimalLore, m_Creature, 0.0, 120.0);

                        if (m_Creature.Paralyzed)
                            m_Paralyzed = true;
                    }
                    else
                    {
                        m_Tamer.RevealingAction();
                        m_Tamer.NextSkillTime = DateTime.Now;
                        m_BeingTamed.Remove(m_Creature);

                        if (m_Creature.Paralyzed)
                            m_Paralyzed = true;

                        if (!alreadyOwned) // Passively check animal lore for gain
                            m_Tamer.CheckTargetSkill(SkillName.AnimalLore, m_Creature, 0.0, 120.0);

                        double minSkill = m_Creature.MinTameSkill; //+ (m_Creature.Owners.Count * 6.0); Taran - Same chance to tame no matter how many owners the mount has had

                        if (minSkill > -29.9 && CheckMastery(m_Tamer, m_Creature))
                            minSkill = -29.9; // 50% at 0.0?

                        double tamerSkill = m_Tamer.Skills[SkillName.AnimalTaming].Value;

                        if (minSkill > tamerSkill) //Taran - Adding chance to tame and gain even if you have lower Animal Taming than MinTameSkill
                        {
                            if (minSkill - tamerSkill <= 5) //1% chance to tame when less than 5% difference in taming and mintameskill
                                minSkill = tamerSkill + 19.3;

                            else if (minSkill - tamerSkill <= 10) //0.5% chance to tame when less than 10% difference in taming and mintameskill
                                minSkill = tamerSkill + 19.65;

                            else if (minSkill - tamerSkill <= 20)  //0.25% chance to tame when less than 20% difference in taming and mintameskill
                                minSkill = tamerSkill + 19.825;

                            else if (minSkill - tamerSkill > 20)  //0.1% chance to tame when more than 20% difference in taming and mintameskill
                                minSkill = tamerSkill + 19.93;
                        }
                        //Taran - Changed the chance to tame below
                        if (alreadyOwned || m_Tamer.CheckTargetSkill(SkillName.AnimalTaming, m_Creature, minSkill - 20.0, minSkill + 50.0))
                        {
                            if (m_Creature.Owners.Count == 0) // First tame
                            {
                                if (m_Paralyzed)
                                    ScaleSkills(m_Creature, 0.86); // 86% of original skills if they were paralyzed during the taming
                                else
                                    ScaleSkills(m_Creature, 0.90); // 90% of original skills

                                if (m_Creature.StatLossAfterTame)
                                    ScaleStats(m_Creature, 0.50);
                            }

                            if (alreadyOwned)
                            {
                                //m_Tamer.SendLocalizedMessage( 502797 ); // That wasn't even challenging.
                                m_Tamer.SendAsciiMessage("{0} remembers you and accepts you once more as its master.", m_Creature.Name);

                                if (m_Tamer is PlayerMobile)
                                    ((PlayerMobile)m_Tamer).EndPlayerAction();
                            }
                            else
                            {
                                if (m_Creature.Controlled && m_Creature.ControlMaster != null)
                                    m_Tamer.SendAsciiMessage("Someone else tamed that creature before you.");
                                else
                                {
                                    m_Tamer.SendAsciiMessage(CliLoc.LocToString(502799));
                                    //m_Tamer.SendAsciiMessage("The {0} accepts you as its master.", m_Creature);
                                    m_Creature.Owners.Add(m_Tamer);
                                }
                            }

                            if (!m_Creature.Controlled && m_Creature.ControlMaster == null)
                            {
                                //m_Tamer.SendAsciiMessage("The {0} accepts you as its master.", m_Creature.Name);
                                m_Creature.SetControlMaster(m_Tamer);
                            }
                            else
                                m_Tamer.SendAsciiMessage("Someone else tamed that creature before you.");


                            m_Creature.IsBonded = false;

                            if (m_Tamer is PlayerMobile)
                                ((PlayerMobile)m_Tamer).EndPlayerAction();
                        }
                        else
                        {
                            m_Tamer.SendAsciiMessage(CliLoc.LocToString(502798)); // You fail to tame the creature.

                            if (m_Tamer is PlayerMobile)
                                ((PlayerMobile)m_Tamer).EndPlayerAction();
                        }
                    }
                }
                /*
                private bool CanPath()
                {
                    IPoint3D p = m_Tamer as IPoint3D;

                    if (p == null)
                        return false;

                    if (m_Creature.InRange(new Point3D(p), 1))
                        return true;

                    MovementPath path = new MovementPath(m_Creature, new Point3D(p));

                    return path.Success;
                }
                */

                #region IAction Members

                public void AbortAction(Mobile from)
                {
                    from.SendAsciiMessage("You fail to tame the creature");

                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();

                    Stop();
                }

                #endregion
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
            }

            #endregion
        }
    }
}