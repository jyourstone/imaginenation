using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class MysteryStoneGump : Gump
	{
		public MysteryStoneGump() : base( 0, 0 )
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage( 1 );
			AddImage( 179, 107, 101 );
			AddImage( 265, 229, 23 );
			AddButton( 251, 254, 5, 248, 0, GumpButtonType.Page, 2 );
			AddImage( 265, 282, 24 );

			AddPage( 2 );
			AddImage( 179, 107, 101 );
			AddImage( 265, 229, 23 );
			AddImage( 253, 250, 1417 );
			AddButton( 280, 277, 11320, 11320, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			if( from == null )
				return;

			if( info.ButtonID == 1 )
			{
				Item[] GlassShards = from.Backpack.FindItemsByType( typeof( GlassShards ) );
				if( from.Backpack.ConsumeTotal( typeof( GlassShards ), 1 ) )
				{
					from.SendMessage( "You've been moved to a secret location!" );
					from.MoveToWorld( new Point3D( 5548, 1958, 0 ), Map.Felucca );
					Effects.PlaySound( from.Location, from.Map, 0x5C9 );
					from.FixedEffect( 0x376A, 10, 16 );
				}
				else
					from.SendMessage( "You don't have what is required to form this!" );
			}
		}
	}
}