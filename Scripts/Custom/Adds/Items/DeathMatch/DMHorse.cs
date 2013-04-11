using System.Collections.Generic;
using Server.Mobiles;
using Server.Multis;

namespace Server.Scripts.Custom.Adds.Items.DeathMatch
{
    public class DMHorse : Horse, IMount
    {
        private static readonly Dictionary<Mobile, DMHorse> m_Horses = new Dictionary<Mobile, DMHorse>();

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        #region Statics

        public static void TryMountHorse(Mobile owner)
        {
            DMHorse horse;

            if (m_Horses.TryGetValue(owner, out horse))
                horse.Rider = owner;
        }

        public static void TryRemoveHorse(Mobile owner)
        {
            DMHorse horse;

            if (m_Horses.TryGetValue(owner, out horse))
            {
                m_Horses.Remove(owner);
                horse.Delete();
            }
        }

        public static void TryGiveHorse(Mobile owner)
        {
            DMHorse horse;

            if (m_Horses.TryGetValue(owner, out horse))
                horse.Rider = owner;
            else
            {
                horse = new DMHorse(owner);
                m_Horses.Add(owner, horse);
            }
        }

        #endregion

        protected DMHorse() : this( null)
        {
        }

        protected DMHorse(Mobile owner)
        {
            if (owner == null)
                return;

            Blessed = true;
            Owners.Add(owner);
            Owner = owner;
            Name = string.Format("{0}'s Horse", owner.Name);
            Rider = Owner;
        }

        public DMHorse(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDelete()
        {
            if (m_Horses.ContainsValue(this))
            {
                Mobile owner = null;
                foreach (Mobile key in m_Horses.Keys)
                {
                    if (m_Horses[key] != this) 
                        continue;

                    owner = key;
                    break;
                }

                if (owner != null)
                    m_Horses.Remove(owner);
            }

            base.OnDelete();
        }

        public override void OnDeath(Server.Items.Container c)
        {
            if (m_Horses.ContainsValue(this))
            {
                Mobile owner = null;
                foreach (Mobile key in m_Horses.Keys)
                {
                    if (m_Horses[key] != this)
                        continue;

                    owner = key;
                    break;
                }

                if (owner != null)
                    m_Horses.Remove(owner);
            }

            base.OnDeath(c);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsDeadPet)
                return;

            if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(1061628); // You can't do that while polymorphed.
                return;
            }

            if (!CheckMountAllowed(from, true))
                return;


            if (from.Female ? !AllowFemaleRider : !AllowMaleRider)
            {
                OnDisallowedRider(from);
                return;
            }

            if (!DesignContext.Check(from))
                return;

            if (from.HasTrade)
            {
                from.SendLocalizedMessage(1042317, "", 0x41); // You may not ride at this time
                return;
            }

            if (from.InRange(this, 5) && from.InLOS(this))
            {
                bool canAccess = (from.AccessLevel >= AccessLevel.GameMaster)
                    || (Controlled && ControlMaster == from)
                    || (Summoned && SummonMaster == from);

                if (canAccess)
                {
                    if (from.Mounted)
                        return;

                    Effects.PlaySound(this, Map, 168);
                    Rider = from;
                }
                else
                    from.SendAsciiMessage("You dont own that mount.");
            }
            else
                from.SendLocalizedMessage(500206); // That is too far away to ride.
        }

        #region IMount Members

        Mobile IMount.Rider
        {
            get
            {
                return Rider;
            }
            set
            {
                if (value == null)
                    return;

                Rider = value;
            }
        }

        #endregion
    }
}