namespace Server.Regions
{
	public class HoldingCell : Region
	{
		public HoldingCell( int x, int y, Map map ) : base( "a Holding Cell", map, 100, new Rectangle2D( x - 1, y - 1, 4, 4 ) )
		{
			/*Priority = Region.HighestPriority;
			LoadFromXml = false;
			Coords = new ArrayList();
			Coords.Add( new Rectangle2D( x -1, y - 1, 4, 4 ) );

			GoLocation = new Point3D( x, y, map.GetAverageZ( x, y ) );
			Region.AddRegion(this);*/
		}

		public override bool AllowBeneficial( Mobile from, Mobile target )
		{
			from.SendMessage( "You may not do that in a holding cell." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool AllowHarmful( Mobile from, Mobile target )
		{
			from.SendMessage( "You may not do that in a holding cell." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = LightCycle.JailLevel;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			from.SendLocalizedMessage( 502629 ); // You cannot cast spells here.

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool OnSkillUse( Mobile from, int Skill )
		{
			from.SendMessage( "You may not use skills in a holding cell." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool OnCombatantChange( Mobile from, Mobile Old, Mobile New )
		{
			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override void OnEnter( Mobile m )
		{
			if( m.AccessLevel > AccessLevel.Player )
				m.SendMessage( "You have entered a holding cell." );
		}

		public override void OnExit( Mobile m )
		{
			if( m.AccessLevel > AccessLevel.Player )
				m.SendMessage( "You have left a holding cell." );
		}
	}
}