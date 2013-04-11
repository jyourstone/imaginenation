using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
	public class Kindling : Item
	{
		[Constructable]
		public Kindling() : this( 1 )
		{
		}

		[Constructable]
		public Kindling( int amount ) : base( 0xDE1 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public Kindling( Serial serial ) : base( serial )
		{
		}

		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from.BeginAction(typeof(IAction)))
            {
                bool releaseLock = true;

                if (from.InRange(GetWorldLocation(), 2) && from.InLOS(this))
                {
			        if ( !VerifyMove( from ) )
				        return;

			        Point3D fireLocation = GetFireLocation( from );

			        if ( fireLocation == Point3D.Zero )
			        {
				        from.SendLocalizedMessage( 501695 ); // There is not a spot nearby to place your campfire.
                        return;
			        }

                    else
                    {
                        new InternalTimer(from).Start();
                        releaseLock = false;

                        if (!from.CheckSkill(SkillName.Camping, 0.0, 100.0))
                        {
                            from.SendLocalizedMessage(501696); // You fail to ignite the campfire.
                        }
                        else
                        {
                            Consume();

                            if (!Deleted && Parent == null)
                                from.PlaceInBackpack(this);

                            new Campfire().MoveToWorld(fireLocation, from.Map);
                        }
                    }
                }
                else
				    from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.

                if (releaseLock && from is PlayerMobile)
                {
                    ((PlayerMobile)from).EndPlayerAction();
                }
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");
		}

		private Point3D GetFireLocation( Mobile from )
		{
			if ( from.Region.IsPartOf( typeof( DungeonRegion ) ) )
				return Point3D.Zero;

			if ( Parent == null )
				return Location;

			ArrayList list = new ArrayList( 4 );

			AddOffsetLocation( from,  0, -1, list );
			AddOffsetLocation( from, -1,  0, list );
			AddOffsetLocation( from,  0,  1, list );
			AddOffsetLocation( from,  1,  0, list );

			if ( list.Count == 0 )
				return Point3D.Zero;

			int idx = Utility.Random( list.Count );
			return (Point3D) list[idx];
		}

		private void AddOffsetLocation( Mobile from, int offsetX, int offsetY, ArrayList list )
		{
			Map map = from.Map;

			int x = from.X + offsetX;
			int y = from.Y + offsetY;

			Point3D loc = new Point3D( x, y, from.Z );

			if ( map.CanFit( loc, 1 ) && from.InLOS( loc ) )
			{
				list.Add( loc );
			}
			else
			{
				loc = new Point3D( x, y, map.GetAverageZ( x, y ) );

				if ( map.CanFit( loc, 1 ) && from.InLOS( loc ) )
					list.Add( loc );
			}
		}

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;

            public InternalTimer(Mobile from)
                : base(TimeSpan.FromSeconds(1.0))
            {
               m_From = from;

               if (from is PlayerMobile)
                   ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(501696); // You fail to ignite the campfire.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}