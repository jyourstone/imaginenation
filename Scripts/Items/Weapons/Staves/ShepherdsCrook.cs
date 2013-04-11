using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.CannedEvil;

namespace Server.Items
{
    [Flipable(0xE81, 0xE82)]
    public class ShepherdsCrook : BaseStaff
    {
        //public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
        //public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

        public override int AosStrengthReq { get { return 20; } }
        public override int AosMinDamage { get { return 13; } }
        public override int AosMaxDamage { get { return 15; } }
        public override int AosSpeed { get { return 40; } }

        public override int OldStrengthReq { get { return 10; } }
        public override int OldMinDamage { get { return 3; } }
        public override int OldMaxDamage { get { return 12; } }
        public override int OldSpeed { get { return 450; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 50; } }

        [Constructable]
        public ShepherdsCrook()
            : base(0xE81)
        {
            Weight = 4.0;
            Layer = Layer.TwoHanded;
            //Name = "shepherds crook";
        }

        public ShepherdsCrook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Weight == 2.0)
                Weight = 4.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.BeginAction(typeof(IAction)))
            {
                from.SendLocalizedMessage(502464); // Target the animal you wish to herd.
                from.Target = new HerdingTarget(from);

                base.OnDoubleClick(from);
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");
        }

        private class HerdingTarget : Target
        {
            public HerdingTarget(Mobile from)
                : base(10, false, TargetFlags.None)
            {
                if (from is PlayerMobile)
                {
                    ((PlayerMobile)from).EndPlayerAction();
                }
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                bool releaseLock = true;
                if (targ is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)targ;
                    if (IsHerdable(bc))
                    {
                        from.SendLocalizedMessage(502475); // Click where you wish the animal to go.
                        from.Target = new InternalTarget(bc);
                    }
                    else
                    {
                        from.SendLocalizedMessage(502468); // That is not a herdable animal.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502472); // You don't seem to be able to persuade that to move.
                }
                
                if (releaseLock && from is PlayerMobile)
                {
                    ((PlayerMobile)from).EndPlayerAction();
                }
            }

            private static readonly Type[] m_ChampTamables = new Type[]
			{
				typeof( StrongMongbat ), typeof( Imp ), typeof( Scorpion ), typeof( GiantSpider ),
				typeof( Snake ), typeof( LavaLizard ), typeof( Drake ), typeof( Dragon ),
				typeof( Kirin ), typeof( Unicorn ), typeof( GiantRat ), typeof( Slime ),
				typeof( DireWolf ), typeof( HellHound ), typeof( DeathwatchBeetle ), 
				typeof( LesserHiryu ), typeof( Hiryu )
			};

            private static bool IsHerdable(BaseCreature bc)
            {
                if (bc.IsParagon)
                    return false;

                if (bc.Tamable)
                    return true;

                Map map = bc.Map;

                ChampionSpawnRegion region = Region.Find(bc.Home, map) as ChampionSpawnRegion;

                if (region != null)
                {
                    ChampionSpawn spawn = region.ChampionSpawn;

                    if (spawn != null && spawn.IsChampionSpawn(bc))
                    {
                        Type t = bc.GetType();

                        foreach (Type type in m_ChampTamables)
                            if (type == t)
                                return true;
                    }
                }

                return false;
            }

            private class InternalTarget : Target
            {
                bool releaseLock = true;
                private readonly BaseCreature m_Creature;
                public InternalTarget(BaseCreature c)
                    : base(10, true, TargetFlags.None)
                {
                    m_Creature = c;
                }

                protected override void OnTarget(Mobile from, object targ)
                {
                    if (targ is IPoint2D)
                    {
                        new InternalTimer(from, m_Creature, targ).Start();
                        releaseLock = false;
                    }
                    
                    if (releaseLock && from is PlayerMobile)
                    {
                        ((PlayerMobile)from).EndPlayerAction();
                    }
                }
            }
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly BaseCreature m_Creature;
            private readonly object m_Object;

            public InternalTimer(Mobile from, BaseCreature basecreature, object targ)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Creature = basecreature;
                m_Object = targ;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);

                if (from.Mounted)
                    from.Animate(29, 5, 1, true, false, 0);

                if (!from.Mounted)
                    from.Animate(12, 5, 1, true, false, 0);
            }

            protected override void OnTick()
            {
                if (m_From.CheckTargetSkill(SkillName.Herding, m_Creature, 0, 100))
                {
                    m_Creature.StopFlee();//So the creature will follow where you target even if it have follow on someone.
                    m_Creature.ControlOrder = OrderType.Follow; //So the creature will follow where you target even if it's tamed and have a control order
                    if (m_Creature.ControlMaster != null)
                        m_Creature.CurrentSpeed *= 5.0; //Tacky fix so they don't walk too fast when they are tamed

                    IPoint2D p = (IPoint2D)m_Object;

                    if (m_Object != m_From)
                        p = new Point2D(p.X, p.Y);

                    m_Creature.TargetLocation = p;
                    m_Creature.PrivateOverheadMessage(MessageType.Regular, 906, true, string.Format("The animal goes where it is instructed"), m_From.NetState); // The animal walks where it was instructed to.
                }
                else
                {
                    m_From.SendLocalizedMessage(502472); // You don't seem to be able to persuade that to move.
                }

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(502472); // You don't seem to be able to persuade that to move.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }

    }
}