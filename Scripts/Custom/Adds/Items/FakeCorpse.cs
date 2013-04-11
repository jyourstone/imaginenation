using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Network;

namespace Server.Items
{
	public class FakeCorpse : Container, ICarvable
	{
        public override TimeSpan DecayTime { get { return TimeSpan.FromSeconds(3.0); } }

        public override bool IsDecoContainer
        {
            get { return false; }
        }

        private Timer m_DecayTimer;

        private static readonly TimeSpan m_DefaultDecayTime = TimeSpan.FromSeconds(3.0);

        public FakeCorpse(Mobile owner)
            : base(0x2006)
        {
            Stackable = true;
            Amount = owner.Body.BodyID; // protocol defines that for itemid 0x2006, amount=body
            Stackable = false;

            BeginDecay(m_DefaultDecayTime);
        }

        public static void CreateCorpses(Mobile from)
        {
            int maxCorpses = Utility.Random(1, 4);

            maxCorpses += Utility.Random(2);

            for (int i = 0; i < maxCorpses; i++)
            {
                Point3D loc = from.Location;
                loc.X += Utility.RandomMinMax(-1, 1);
                loc.Y += Utility.RandomMinMax(-1, 1);

                FakeCorpse fc = new FakeCorpse(from);
                fc.MoveToWorld(loc, Map.Felucca);
            }
        }

		public void BeginDecay( TimeSpan delay )
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTimer = new InternalTimer( this, delay );
			m_DecayTimer.Start();
		}

		public override void OnAfterDelete()
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTimer = null;
		}

		private class InternalTimer : Timer
		{
			private readonly FakeCorpse m_Corpse;

            public InternalTimer(FakeCorpse c, TimeSpan delay)
                : base(delay)
			{
				m_Corpse = c;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
                if (m_Corpse != null)
                {
                    m_Corpse.SendRemovePacket();
                    m_Corpse.Delete();
                }
			}
		}

		public static string GetCorpseName( Mobile m )
		{
            return string.Empty;
		}

		public override bool IsPublicContainer{ get{ return true; } }

        public FakeCorpse(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

		}

		public override void SendInfoTo( NetState state, bool sendOplPacket )
		{
			base.SendInfoTo( state, sendOplPacket );
		}

		public bool IsCriminalAction( Mobile from )
		{
            return true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{

			return true;
		}

		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			return CanLoot( from );
		}

		public override void OnItemUsed( Mobile from, Item item )
		{
			base.OnItemUsed( from, item );
		}

		public override void OnItemLifted( Mobile from, Item item )
		{
			base.OnItemLifted( from, item );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
		}

		//private Dictionary<Item, Point3D> m_RestoreTable;

		public bool GetRestoreInfo( Item item, ref Point3D loc )
		{
            return false;
		}

		public void SetRestoreInfo( Item item, Point3D loc )
		{

		}

		public void ClearRestoreInfo( Item item )
		{

		}

		public bool CanLoot( Mobile from )
		{
			return false;
		}

		public bool CheckLoot( Mobile from )
		{
			return false;
		}

		public virtual void Open( Mobile from, bool checkSelfLoot )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{

        }

		public override bool CheckContentDisplay( Mobile from )
		{
			return false;
		}

		public override bool DisplaysContent{ get{ return false; } }

		public override void AddNameProperty( ObjectPropertyList list )
		{

		}

		public override void OnAosSingleClick( Mobile from )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
           
        }

		public void Carve( Mobile from, Item item )
		{
            from.PublicOverheadMessage(MessageType.Regular, 33, true, "Auto cut detected - Killing.");
            from.Kill();
		}
	}
}