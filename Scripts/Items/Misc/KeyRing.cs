using System.Collections.Generic;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class KeyRing : Item
	{
		public static readonly int MaxKeys = 20;

		private List<Key> m_Keys;

		public List<Key> Keys { get { return m_Keys; } }

		[Constructable]
		public KeyRing() : base( 0x1011 )
		{
			Weight = 1.0; // They seem to have no weight on OSI ?!
            Name = "key ring";
            LootType = LootType.Newbied;
			m_Keys = new List<Key>();
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
				return false;
			}

			Key key = dropped as Key;

			if ( key == null || key.KeyValue == 0 )
			{
				from.SendLocalizedMessage( 501689 ); // Only non-blank keys can be put on a keyring.
				return false;
			}
			else if ( Keys.Count >= MaxKeys )
			{
				from.SendLocalizedMessage( 1008138 ); // This keyring is full.
				return false;
			}
			else
			{
				Add( key, from );
				from.SendLocalizedMessage( 501691 ); // You put the key on the keyring.
				return true;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
				return;
			}

            if (m_Keys.Count > 0)
            {
                from.SendAsciiMessage("What do you wish to use your keys on?");
                from.Target = new InternalTarget(this);
            }
            else
                from.SendAsciiMessage("You do not have any keys on your keyring");
		}

        private class InternalTarget : Target
        {
            private readonly KeyRing m_KeyRing;

            public InternalTarget(KeyRing keyRing)
                : base(-1, false, TargetFlags.None)
            {
                CheckLOS = false;
                m_KeyRing = keyRing;
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                OnTarget(from, targeted);
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_KeyRing.Deleted || !m_KeyRing.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.
                    return;
                }
                if (m_KeyRing == targeted)
                {
                    m_KeyRing.Open(from);
                    from.SendLocalizedMessage(501685); // You open the keyring.
                }
                else if (!from.InRange(targeted, 3) || !from.InLOS(targeted))
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                else if (targeted is Item)
                {
                    Item target = (Item)targeted;
                    Region itemRegion = Region.Find(target.Location, target.Map);
                    string message = "That does not have a lock.";

                    foreach (Key key in m_KeyRing.Keys)
                    {
                        if (target is ILockable)
                        {
                            if (key.UseOn(from, (ILockable)target))
                            {
                                message = null;
                                break;
                            }

                            message = "The key does not fit that lock.";
                            continue;
                        }
                        if (itemRegion is Regions.HouseRegion)
                        {
                            Multis.BaseHouse house = ((Regions.HouseRegion)itemRegion).House;

                            if (house == null || !from.Alive || house.Deleted)
                                continue;
                            if (target.RootParent != null)
                            {
                                message = "You can not lock that down";
                                continue;
                            }
                            if (house.HouseKeyVal != key.KeyValue)
                            {
                                message = "You must use the house key to lock down or unlock items.";
                                continue;
                            }
                            if (!target.Movable && !house.IsLockedDown(target))
                            {
                                message = "You can't unlock that!";
                                continue;
                            }
                            if (key.OnHouseItemTarget(from, target, ((Regions.HouseRegion)itemRegion).House))
                            {
                                message = null;
                                break;
                            }
                        }
                    }

                    if (message != null)
                        from.SendAsciiMessage(message);
                }
                else
                    from.SendAsciiMessage("You can't use a key on that!");
            }
        }

		public override void OnDelete()
		{
			base.OnDelete();

			foreach ( Key key in m_Keys )
			{
				key.Delete();
			}

			m_Keys.Clear();
		}

		public void Add( Key key, Mobile m )
		{
			key.Internalize();
			m_Keys.Add( key );

            m.PlaySound(740);

			UpdateItemID();
		}

		public void Open( Mobile from )
		{
			Container cont = Parent as Container;

			if ( cont == null )
				return;

			for ( int i = m_Keys.Count - 1; i >= 0; i-- )
			{
				Key key = m_Keys[i];

				if ( !key.Deleted && !cont.TryDropItem( from, key, true ) )
					break;

				m_Keys.RemoveAt( i );
			}

			UpdateItemID();
		}

		public void RemoveKeys( uint keyValue )
		{
			for ( int i = m_Keys.Count - 1; i >= 0; i-- )
			{
				Key key = m_Keys[i];

				if ( key.KeyValue == keyValue )
				{
					key.Delete();
					m_Keys.RemoveAt( i );
				}
			}

			UpdateItemID();
		}

		public bool ContainsKey( uint keyValue )
		{
			foreach ( Key key in m_Keys )
			{
				if ( key.KeyValue == keyValue )
					return true;
			}

			return false;
		}

		private void UpdateItemID()
		{
			if ( Keys.Count < 1 )
				ItemID = 0x1011;
			else if ( Keys.Count < 3 )
				ItemID = 0x1769;
			else if ( Keys.Count < 5 )
				ItemID = 0x176A;
			else
				ItemID = 0x176B;
		}

		public KeyRing( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.WriteItemList( m_Keys );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Keys = reader.ReadStrongItemList<Key>();
		}

        public override void OnSingleClick(Mobile from)
        {
            NetState state = from.NetState;
            if (m_Keys.Count == 0)
                base.OnSingleClick(from);
            else if (m_Keys.Count == 1)
                state.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0x3b2, 3, "", string.Format("Key ring ({0} key)", m_Keys.Count)));
            else if (m_Keys.Count > 1)
                state.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0x3b2, 3, "", string.Format("Key ring ({0} keys)", m_Keys.Count)));
        }
	}
}