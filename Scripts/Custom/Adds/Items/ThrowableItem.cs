using System;
using System.Collections;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public class ThrowableItem : Item
    {
        private static Point3D target;

        private int m_DetonationDelay;
        private int m_UseDelay;
        private int m_Range = 12;
        private int m_MinDamage;
        private int m_MaxDamage;
        private int m_AnimationId = 3699;
        private int m_AnimationHue = 900;
        private bool m_DeleteOnUse = true;
        private bool m_RequireFreeHands = true;
        private bool m_DetonateNearby;
        private bool m_CheckGuarded = true;
        private int m_BlastRadius;
        private int m_DetonationEffect;
        private int m_DetonationEffectHue;
        private int m_DetonationEffectDuration = 20;
        private int m_DetonationSound;
        private int m_TargetItemID;
        private int m_TargetItemHue;
        private TimeSpan m_TargetItemDuration;
        private int m_TargetItemMinDamage;
        private int m_TargetItemMaxDamage;
        private string m_TargetItemName;
        private bool m_MoveOnUse = true;
        private bool m_CheckLoS = true;

        #region Getters & Setters

        [CommandProperty(AccessLevel.GameMaster)]
        public int DetonationDelay
        {
            get { return m_DetonationDelay; }
            set { m_DetonationDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UseDelay
        {
            get { return m_UseDelay; }
            set { m_UseDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range
        {
            get { return m_Range; }
            set { m_Range = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage
        {
            get { return m_MinDamage; }
            set
            {
                m_MinDamage = value;

                if (value > m_MaxDamage)
                    m_MaxDamage = value;
            }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage
        {
            get { return m_MaxDamage; }
            set
            {
                m_MaxDamage = value;

                if (value < m_MinDamage)
                    m_MinDamage = value;
            }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimationID
        {
            get { return m_AnimationId; }
            set { m_AnimationId = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimationHue
        {
            get { return m_AnimationHue; }
            set { m_AnimationHue = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeleteOnUse
        {
            get { return m_DeleteOnUse; }
            set { m_DeleteOnUse = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool RequireFreeHands
        {
            get { return m_RequireFreeHands; }
            set { m_RequireFreeHands = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DetonateNearby
        {
            get { return m_DetonateNearby; }
            set { m_DetonateNearby = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CheckGuarded
        {
            get { return m_CheckGuarded; }
            set { m_CheckGuarded = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int BlastRadius
        {
            get { return m_BlastRadius; }
            set { m_BlastRadius = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DetonationEffect
        {
            get { return m_DetonationEffect; }
            set { m_DetonationEffect = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DetonationEffectHue
        {
            get { return m_DetonationEffectHue; }
            set { m_DetonationEffectHue = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DetonationEffectDuration
        {
            get { return m_DetonationEffectDuration; }
            set { m_DetonationEffectDuration = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DetonationSound
        {
            get { return m_DetonationSound; }
            set { m_DetonationSound = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetItemID
        {
            get { return m_TargetItemID; }
            set { m_TargetItemID = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetItemHue
        {
            get { return m_TargetItemHue; }
            set { m_TargetItemHue = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan TargetItemDuration
        {
            get { return m_TargetItemDuration; }
            set { m_TargetItemDuration = value; }
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetItemMinDamage
        {
            get { return m_TargetItemMinDamage; }
            set
            {
                m_TargetItemMinDamage = value;

                if (value > m_TargetItemMaxDamage)
                    m_TargetItemMaxDamage = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetItemMaxDamage
        {
            get { return m_TargetItemMaxDamage; }
            set
            {
                m_TargetItemMaxDamage = value;

                if (value < m_TargetItemMinDamage)
                    m_TargetItemMinDamage = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string TargetItemName
        {
            get { return m_TargetItemName; }
            set { m_TargetItemName = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MoveOnUse
        {
            get { return m_MoveOnUse; }
            set { m_MoveOnUse = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CheckLoS
        {
            get { return m_CheckLoS; }
            set { m_CheckLoS = value; }
        }

        #endregion

        [Constructable]
        public ThrowableItem() : base(3699)
		{
			Weight = 1.0;
			Hue = 900;
			Name = "Throwable Item";
            Stackable = false;
		}

        public ThrowableItem(Serial serial) : base(serial)
        {
        }

        #region Serialize & Deserialize

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            //version 0
            writer.Write(m_DetonationDelay);
            writer.Write(m_UseDelay);
            writer.Write(m_Range);
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_AnimationId);
            writer.Write(m_AnimationHue);
            writer.Write(m_DeleteOnUse);
            writer.Write(m_RequireFreeHands);
            writer.Write(m_DetonateNearby);
            writer.Write(m_CheckGuarded);
            writer.Write(m_BlastRadius);
            writer.Write(m_DetonationEffect);
            writer.Write(m_DetonationEffectHue);
            writer.Write(m_DetonationEffectDuration);
            writer.Write(m_DetonationSound);
            writer.Write(m_TargetItemID);
            writer.Write(m_TargetItemHue);
            writer.Write(m_TargetItemDuration);
            writer.Write(m_TargetItemMinDamage);
            writer.Write(m_TargetItemMaxDamage);
            writer.Write(m_TargetItemName);
            writer.Write(m_MoveOnUse);
            writer.Write(m_CheckLoS);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_DetonationDelay = reader.ReadInt();
                        m_UseDelay = reader.ReadInt();
                        m_Range = reader.ReadInt();
                        m_MinDamage = reader.ReadInt();
                        m_MaxDamage = reader.ReadInt();
                        m_AnimationId = reader.ReadInt();
                        m_AnimationHue = reader.ReadInt();
                        m_DeleteOnUse = reader.ReadBool();
                        m_RequireFreeHands = reader.ReadBool();
                        m_DetonateNearby = reader.ReadBool();
                        m_CheckGuarded = reader.ReadBool();
                        m_BlastRadius = reader.ReadInt();
                        m_DetonationEffect = reader.ReadInt();
                        m_DetonationEffectHue = reader.ReadInt();
                        m_DetonationEffectDuration = reader.ReadInt();
                        m_DetonationSound = reader.ReadInt();
                        m_TargetItemID = reader.ReadInt();
                        m_TargetItemHue = reader.ReadInt();
                        m_TargetItemDuration = reader.ReadTimeSpan();
                        m_TargetItemMinDamage = reader.ReadInt();
                        m_TargetItemMaxDamage = reader.ReadInt();
                        m_TargetItemName = reader.ReadString();
                        m_MoveOnUse = reader.ReadBool();
                        m_CheckLoS = reader.ReadBool();
                        break;
                    }
            }
        }

        #endregion

        public override bool OnDragLift(Mobile from)
        {
            if (EventItem)
            {
                if (ParentEntity == null)
                    return false;
            }

            return base.OnDragLift(from);
        }

        public object FindParent(Mobile from)
        {
            Mobile m = HeldBy;

            if (m != null && m.Holding == this)
                return m;

            object obj = RootParent;

            if (obj != null)
                return obj;

            if (Map == Map.Internal)
                return from;

            return this;
        }

        private Timer m_Timer;

        private ArrayList m_Users;

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation() , 2) || !from.InLOS(this))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            if (from.Paralyzed || from.Frozen)
            {
                from.SendAsciiMessage("You cannot use this while frozen");
                return;
            }

            if (!from.CanBeginAction(typeof(ThrowableItem)))
            {
                from.SendAsciiMessage("You must wait before throwing again");
                return;
            }

            ThrowTarget targ = from.Target as ThrowTarget;

            if (targ != null && targ.ThrowableItem == this)
                return;

            from.RevealingAction();

            from.BeginAction(typeof(ThrowableItem));
            Timer.DelayCall(TimeSpan.FromSeconds(m_UseDelay), new TimerStateCallback(EndThrowableDelay), from);

            if (m_Users == null)
                m_Users = new ArrayList();

            if (!m_Users.Contains(from))
                m_Users.Add(from);

            from.Target = new ThrowTarget(this);

            if (m_Timer == null && m_DetonationDelay > 0)
            {
                from.SendLocalizedMessage(500236); // You should throw it now!
                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(0.75), TimeSpan.FromSeconds(1.0), m_DetonationDelay + 1, new TimerStateCallback(Detonate_OnTick), new object[] { from, m_DetonationDelay });
            }
        }

        private void Detonate_OnTick(object state)
        {
            if (Deleted)
                return;

            object[] states = (object[])state;
            Mobile from = (Mobile)states[0];
            int timer = (int)states[1];

            object parent = FindParent(from);

            if (timer == 0)
            {
                Point3D loc;
                Map map;
                Mobile m = from;

                if (parent is Item)
                {
                    Item item = (Item)parent;

                    loc = item.GetWorldLocation();
                    map = item.Map;
                }
                else if (parent is Mobile)
                {
                    if (m_Users != null && m_Users[m_Users.Count - 1] is Mobile)
                        m = (Mobile)m_Users[m_Users.Count - 1];
                    else
                        m = (Mobile)parent;

                    loc = m.Location;
                    map = m.Map;
                }
                else
                {
                    return;
                }

                Explode(m, loc, map, true);
                m_Timer = null;
            }
            else
            {
                if (parent is Item)
                    ((Item)parent).PublicOverheadMessage(MessageType.Regular, 906, true, timer.ToString());
                else if (parent is Mobile)
                    ((Mobile)parent).PublicOverheadMessage(MessageType.Regular, 906, true, timer.ToString());

                states[1] = timer - 1;
            }
        }

        private void Reposition_OnTick(object state)
        {
            if (Deleted || m_Timer == null)
                return;

            object[] states = (object[])state;
            IPoint3D p = (IPoint3D)states[1];
            Map map = (Map)states[2];

            Point3D loc = new Point3D(p);

            if (m_MoveOnUse)
                MoveToWorld(loc, map);
        }

        private class ThrowTarget : Target
        {
            private readonly ThrowableItem m_ThrowableItem;

            public ThrowableItem ThrowableItem
            {
                get { return m_ThrowableItem; }
            }

            public ThrowTarget(ThrowableItem throwableItem) : base(throwableItem.m_Range, true, TargetFlags.None)
            {
                m_ThrowableItem = throwableItem;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_ThrowableItem.Deleted || m_ThrowableItem.Map == Map.Internal)
                    return;

                IPoint3D p = targeted as IPoint3D;

                if (p == null)
                    return;

                Map map = from.Map;

                if (map == null)
                    return;

                SpellHelper.GetSurfaceTop(ref p);

                from.RevealingAction();

                IEntity to;

                if (p is Mobile)
                    to = (Mobile)p;
                else
                    to = new Entity(Serial.Zero, new Point3D(p), map);

                target = to.Location;

                Effects.SendMovingEffect(from, to, m_ThrowableItem.m_AnimationId, 7, 0, false, false, m_ThrowableItem.m_AnimationHue, 0);

                if (m_ThrowableItem.Amount > 1)
                    Mobile.LiftItemDupe(m_ThrowableItem, 1).EventItem = m_ThrowableItem.EventItem;

                if (m_ThrowableItem.MoveOnUse)
                    m_ThrowableItem.Internalize();
                
                if (m_ThrowableItem.m_DetonationDelay > 0)
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_ThrowableItem.Reposition_OnTick), new object[] { from, p, map });
                else
                {
                    if (m_ThrowableItem.MoveOnUse)
                        m_ThrowableItem.MoveToWorld(to.Location, to.Map);

                    m_ThrowableItem.Explode(from, to.Location, to.Map, true);
                }
            }

            #region TargetFailed

            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_CheckLoS)
                {
                    if (m_ThrowableItem.m_DetonationDelay <= 0)
                        EndThrowableDelay(from);

                    base.OnCantSeeTarget(from, targeted);
                }
                else
                {
                    if (m_ThrowableItem.Deleted || m_ThrowableItem.Map == Map.Internal)
                        return;

                    IPoint3D p = targeted as IPoint3D;

                    if (p == null)
                        return;

                    Map map = from.Map;

                    if (map == null)
                        return;

                    SpellHelper.GetSurfaceTop(ref p);

                    from.RevealingAction();

                    IEntity to;

                    if (p is Mobile)
                        to = (Mobile)p;
                    else
                        to = new Entity(Serial.Zero, new Point3D(p), map);

                    target = to.Location;

                    Effects.SendMovingEffect(from, to, m_ThrowableItem.m_AnimationId, 7, 0, false, false, m_ThrowableItem.m_AnimationHue, 0);

                    if (m_ThrowableItem.Amount > 1)
                        Mobile.LiftItemDupe(m_ThrowableItem, 1).EventItem = m_ThrowableItem.EventItem;

                    if (m_ThrowableItem.MoveOnUse)
                        m_ThrowableItem.Internalize();

                    if (m_ThrowableItem.m_DetonationDelay > 0)
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_ThrowableItem.Reposition_OnTick), new object[] { from, p, map });
                    else
                    {
                        if (m_ThrowableItem.MoveOnUse)
                            m_ThrowableItem.MoveToWorld(to.Location, to.Map);

                        m_ThrowableItem.Explode(from, to.Location, to.Map, true);
                    }
                }
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnNonlocalTarget(from, targeted);
            }

            protected override void OnTargetInSecureTrade(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetInSecureTrade(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_CheckLoS)
                {
                    if (m_ThrowableItem.m_DetonationDelay <= 0)
                        EndThrowableDelay(from);

                    base.OnTargetOutOfLOS(from, targeted);
                }
                else
                {
                    if (m_ThrowableItem.Deleted || m_ThrowableItem.Map == Map.Internal)
                        return;

                    IPoint3D p = targeted as IPoint3D;

                    if (p == null)
                        return;

                    Map map = from.Map;

                    if (map == null)
                        return;

                    SpellHelper.GetSurfaceTop(ref p);

                    from.RevealingAction();

                    IEntity to;

                    if (p is Mobile)
                        to = (Mobile)p;
                    else
                        to = new Entity(Serial.Zero, new Point3D(p), map);

                    target = to.Location;

                    Effects.SendMovingEffect(from, to, m_ThrowableItem.m_AnimationId, 7, 0, false, false, m_ThrowableItem.m_AnimationHue, 0);

                    if (m_ThrowableItem.Amount > 1)
                        Mobile.LiftItemDupe(m_ThrowableItem, 1).EventItem = m_ThrowableItem.EventItem;

                    if (m_ThrowableItem.MoveOnUse)
                        m_ThrowableItem.Internalize();

                    if (m_ThrowableItem.m_DetonationDelay > 0)
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_ThrowableItem.Reposition_OnTick), new object[] { from, p, map });
                    else
                    {
                        if (m_ThrowableItem.MoveOnUse)
                            m_ThrowableItem.MoveToWorld(to.Location, to.Map);

                        m_ThrowableItem.Explode(from, to.Location, to.Map, true);
                    }
                }
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (m_ThrowableItem.m_DetonationDelay <= 0)
                    EndThrowableDelay(from);

                base.OnTargetCancel(from, cancelType);
            }

            #endregion
        }

        public void Explode(Mobile from, Point3D loc, Map map, bool detonatenearby)
        {
            if (Deleted)
                return;

            if (!m_MoveOnUse)
                loc = target;

            if ((m_DeleteOnUse && !EventItem) || (EventItem && EventItemConsume))
                Consume();
            else
            {
                if (m_DeleteOnUse)
                {
                    Mobile m;
                    if (m_Users != null && m_Users[0] is Mobile)
                        m = (Mobile) m_Users[0];
                    else
                        m = from;

                    if (RootParentEntity != m)
                        m.AddToBackpack(this);
                }

                m_Timer = null;
            }

            for (int i = 0; m_Users != null && i < m_Users.Count; ++i)
            {
                Mobile m = (Mobile)m_Users[i];
                ThrowTarget targ = m.Target as ThrowTarget;

                if (targ != null && targ.ThrowableItem == this)
                    Target.Cancel(m);
            }

            if (map == null)
                return;

            if (m_DetonationSound > 0)
                Effects.PlaySound(loc, map, m_DetonationSound);

            if (m_DetonationEffect > 0 && m_DetonationEffectDuration > 0)
            {
                if (m_BlastRadius > 0)
                {
                    ArrayList effects = new ArrayList();

                    int startX = loc.X - m_BlastRadius;
                    int startY = loc.Y - m_BlastRadius;
                    int endX = loc.X + m_BlastRadius + 1;
                    int endY = loc.Y + m_BlastRadius + 1;

                    Point2D start = new Point2D(startX, startY);
                    Point2D end = new Point2D(endX, endY);

                    Rectangle2D rect = new Rectangle2D(start, end);

                    Point2D point;

                    for (int x = rect.Start.X; x < rect.End.X; ++x)
                        for (int y = rect.Start.Y; y < rect.End.Y; ++y)
                        {
                            point = new Point2D(x, y);
                            if (!effects.Contains(point))
                                effects.Add(point);
                        }

                    if (effects.Count < 500)
                    {
                        Point3D location;

                        foreach (Point2D p in effects)
                        {
                            location = new Point3D(p.X, p.Y, loc.Z + 2);
                            Effects.SendLocationEffect(location, map, m_DetonationEffect, m_DetonationEffectDuration, m_DetonationEffectHue - 1, 0);
                        }
                    }
                    else
                    {
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Too big blast radius to create effects");
                        return;
                    }

                    effects.Clear();
                }
                else
                {
                    Effects.SendLocationEffect(loc, map, m_DetonationEffect, m_DetonationEffectDuration, m_DetonationEffectHue - 1, 0);
                }
            }

            if (m_TargetItemID > 0)
            {
                if (m_BlastRadius > 0)
                {
                    Point2D point;
                    ArrayList effects = new ArrayList();

                    int startX = loc.X - m_BlastRadius;
                    int startY = loc.Y - m_BlastRadius;
                    int endX = loc.X + m_BlastRadius + 1;
                    int endY = loc.Y + m_BlastRadius + 1;

                    Point2D start = new Point2D(startX, startY);
                    Point2D end = new Point2D(endX, endY);

                    Rectangle2D rect = new Rectangle2D(start, end);

                    for (int x = rect.Start.X; x < rect.End.X; ++x)
                        for (int y = rect.Start.Y; y < rect.End.Y; ++y)
                        {
                            point = new Point2D(x, y);
                            if (!effects.Contains(point))
                                effects.Add(point);
                        }

                    if (effects.Count < 500)
                    {
                        foreach (Point2D p in effects)
                        {
                            new ThrowableItemTargetItem(m_TargetItemDuration, m_TargetItemID)
                                {
                                    Name = m_TargetItemName,
                                    Hue = m_TargetItemHue,
                                    MinDamage = m_TargetItemMinDamage,
                                    MaxDamage = m_TargetItemMaxDamage,
                                    Location = new Point3D(p.X, p.Y, loc.Z + 2),
                                    Map = Map
                                };
                        }
                        if (m_MoveOnUse)
                            Location = new Point3D(X, Y, loc.Z + 2);
                    }
                    else
                    {
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Too big blast radius to create target items");
                        return;
                    }

                    effects.Clear();
                }
                else
                {
                    new ThrowableItemTargetItem(m_TargetItemDuration, m_TargetItemID)
                    {
                        Name = m_TargetItemName,
                        Hue = m_TargetItemHue,
                        MinDamage = m_TargetItemMinDamage,
                        MaxDamage = m_TargetItemMaxDamage,
                        Location = loc,
                        Map = Map
                    };
                }

                
            }

            IPooledEnumerable eable = m_DetonateNearby ? map.GetObjectsInRange(loc, BlastRadius) : map.GetMobilesInRange(loc, BlastRadius);
            ArrayList toExplode = new ArrayList();

            foreach (object o in eable)
            {
                if (o is Mobile)
                {
                    toExplode.Add(o);
                }
                else if (o is ThrowableItem && o != this)
                {
                    toExplode.Add(o);
                }
            }

            eable.Free();

            int min = m_MinDamage;
            int max = m_MaxDamage;

            for (int i = 0; i < toExplode.Count; ++i)
            {
                object o = toExplode[i];

                if (o is Mobile)
                {
                    Mobile m = (Mobile)o;

                    if (m_MaxDamage > 0)
                    {
                        GuardedRegion reg = (GuardedRegion)Region.Find(m.Location, m.Map).GetRegion(typeof(GuardedRegion));

                        if (m_CheckGuarded && reg != null && !reg.Disabled)
                        {
                            if (from == null || (SpellHelper.ValidIndirectTarget(from, m) && from.CanBeHarmful(m, false)))
                            {
                                if (from != null)
                                    from.DoHarmful(m);

                                int damage = Utility.RandomMinMax(min, max);
                                AOS.Damage(m, from, damage, 0, 100, 0, 0, 0);
                            }
                        }

                        else
                        {
                            if (from == null || from.CanBeHarmful(m, false))
                            {
                                if (from != null)
                                    from.DoHarmful(m);

                                int damage = Utility.RandomMinMax(min, max);
                                AOS.Damage(m, from, damage, 0, 100, 0, 0, 0);
                            }
                        }
                    }
                }

                else if (o is ThrowableItem && detonatenearby)
                {
                    ThrowableItem throwable = (ThrowableItem)o;
                    Mobile m;

                    if (throwable.m_Users != null && throwable.m_Users[0] is Mobile)
                        m = (Mobile)throwable.m_Users[0];
                    else
                        m = from;

                    target = throwable.GetWorldLocation();
                    throwable.Explode(m, throwable.GetWorldLocation(), throwable.Map, false);
                }
            }
        }

        private static void EndThrowableDelay(object state)
        {
            ((Mobile)state).EndAction(typeof(ThrowableItem));
        }
    }
}