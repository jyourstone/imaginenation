using System;
namespace Server.Items
{
    public enum AcceptedTypes
    {
        Armors = 0,
        Weapons,
        Cloths,
        Others
    }

    public class EventItemContainer : Backpack
	{
        private AcceptedTypes m_AcceptedType;

        public override int DefaultMaxWeight
        {
            get { return 0; }
        }

        public override int DefaultMaxItems
        {
            get { return 0; }
        }

        public AcceptedTypes AcceptsType
        {
            get { return m_AcceptedType; }
        }

        public EventItemContainer(AcceptedTypes acceptedType)
        {
            m_AcceptedType = acceptedType;
        }

        public EventItemContainer(Serial serial)
            : base(serial)
        {
        }

		public override void DropItem( Item dropped )
		{
            if (AcceptsItem(dropped))
                base.DropItem(dropped);
            else
                dropped.Delete();
		}

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            return base.TryDropItem(from, dropped, sendFullMessage);
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (AcceptsItem(item))
                return base.OnDragDropInto(from, item, p);
            else
                from.SendAsciiMessage(string.Format("This container only accepts item of type {0}.", m_AcceptedType));

            return false;
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (AcceptsItem(item))
                return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
            else
                m.SendAsciiMessage(string.Format("This container only accepts item of type {0}", m_AcceptedType));

            return false;
        }

        public bool AcceptsItem(Item i)
        {
            //Nested ifs like this so that we cant put the specific types in the "other bag"
            if (i is BaseArmor)
            {
                if (m_AcceptedType == AcceptedTypes.Armors)
                    return true;
            }
            else if (i is BaseWeapon)
            {
                if (m_AcceptedType == AcceptedTypes.Weapons)
                    return true;
            }
            else if (i is BaseClothing)
            {
                if (m_AcceptedType == AcceptedTypes.Cloths)
                    return true;
            }
            else if (m_AcceptedType == AcceptedTypes.Others)
                return true;

            return false;
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); //version

            writer.Write((int)m_AcceptedType);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            m_AcceptedType = (AcceptedTypes)reader.ReadInt();
		}

        public override void DisplayTo(Mobile to)
        {
            Point3D loc = to.Location;
            loc.Z -= 50;
            MoveToWorld(loc, to.Map);
            
            base.DisplayTo(to);
        }
	}
}