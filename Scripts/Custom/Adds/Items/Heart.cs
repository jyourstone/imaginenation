using Server.Network;

namespace Server.Items
{
	public class Heart : Item
	{
	    [Constructable]
		public Heart() : base( 0x1CED )
		{
			Weight = 1.0;
		}

		public Heart( Serial serial ) : base( serial )
		{
		}

	    [CommandProperty(AccessLevel.GameMaster)]
	    public Mobile Owner { get; set; }

	    public override void OnDoubleClick( Mobile from )
		{
			// Fill the Mobile with FillFactor
			if( Food.FillHunger( from, 4 ) )
			{
				// Play a random "eat" sound
				from.PlaySound( Utility.Random( 0x3A, 3 ) );

				if( from.Body.IsHuman && !from.Mounted )
					from.Animate( 34, 5, 1, true, false, 0 );

				if( Owner != null )
					from.PublicOverheadMessage( MessageType.Emote, 0x22, true, string.Format( "*You see {0} eat some {1}*", from.Name, Name ) );

				Consume();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 2 ); // version

			writer.Write( Name );
			writer.Write( Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			Name = reader.ReadString();
			Owner = reader.ReadMobile();
		}
	}
}