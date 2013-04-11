using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Jailing;
using Server.Mobiles;

namespace Server.Targeting
{
	public class WarnTarget : Target
	{
		private readonly bool m_warn = false;

		public WarnTarget( bool warn ) : base( -1, false, TargetFlags.None )
		{
			m_warn = warn;
		}

		public WarnTarget() : base( -1, false, TargetFlags.None )
		{
			m_warn = true;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( from is PlayerMobile && targeted is PlayerMobile )
			{
				Mobile m = (Mobile)targeted;
                if (from.AccessLevel < m.AccessLevel)
                {
                    from.SendMessage("{0} has a higher access level than you and you can not do that.", m.Name);
                    CommandLogging.WriteLine(from, from.Name + " tried to " + "warn" + " " + m.Name);
                    m.SendMessage(from.Name + " tried to " + "warn" + " you");
                }
				else if( m_warn )
					from.SendGump( new JailWarnGump( from, m, "", 0, null ) );
				else
					from.SendGump( new JailReviewGump( from, m, 0, null ) );
			}
		}
	}

	public class MacroTarget : Target
	{
		public MacroTarget() : base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( from is PlayerMobile && targeted is PlayerMobile )
			{
				Mobile m = (Mobile)targeted;
				JailSystem.macroTest( from, m );
			}
		}
	}

	public class CageTarget : Target
	{
		public CageTarget() : base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( from is PlayerMobile && targeted is PlayerMobile )
				if( from is PlayerMobile && targeted is PlayerMobile )
				{
					Mobile m = (Mobile)targeted;
					new JailCage( m );
					Point3D newcell = m.Location;
					m.Location = new Point3D( 0, 0, 0 );
					m.Location = newcell;
				}
		}
	}

	public class JailTarget : Target
	{
		private readonly bool m_releasing = false;

		public JailTarget( bool releasing ) : base( -1, false, TargetFlags.None )
		{
			m_releasing = releasing;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( from is PlayerMobile && targeted is PlayerMobile )
			{
				string temp = "jail";
				Mobile m = (Mobile)targeted;
				if( from.AccessLevel < m.AccessLevel )
				{
					from.SendMessage( "{0} has a higher access level than you and you can not do that.", m.Name );
					if( m_releasing )
						temp = "release";
					CommandLogging.WriteLine( from, from.Name + " tried to " + temp + " " + m.Name );
					m.SendMessage( from.Name + " tried to " + temp + " you" );
				}
				else
					//jailor has a higher (or equal) access level than the target				
					if( m_releasing )
					{
						JailSystem js = JailSystem.fromAccount( (Account)m.Account );
						if( js == null )
						{
							from.SendMessage( m.Name + " no jail object" );
							return;
						}
						js.forceRelease( from );
						m.SendLocalizedMessage( 501659 );
					}
					else
						//temp="jailed";
						JailSystem.newJailingFromGMandPlayer( from, m );
			}
			else
				from.SendLocalizedMessage( 503312 );
		}
	}
}