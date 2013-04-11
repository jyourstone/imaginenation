using Server.Network;

namespace Server.Items
{
	public class MovableKarmaBong : RedRibbedFlask
	{
		[Constructable]
		public MovableKarmaBong()
		{
			Name = "Karma Device";
			Hue = 2549;
		}

		public MovableKarmaBong( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( GetWorldLocation(), 5 ) && from.InLOS( this ) )
			{
				from.PlaySound( 32 );
				from.SendMessage( "Your fame is: {0}", from.Fame );
				from.SendMessage( "Your karma is: {0}", from.Karma );
				from.SendMessage( "And your killcount is: {0}", from.Kills );
			}
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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
	}
}