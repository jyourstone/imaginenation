using Server.Network;

namespace Server.Items
{
	[Flipable]
	public class ResRobe : BaseOuterTorso, IScissorable
	{
		[Constructable]
		public ResRobe() : base( 0x1F03 )
		{
			Name = "Resurrection robe";
			Weight = 3.0;
			MaxHitPoints = 1;
			HitPoints = 1;
		}

		public ResRobe( Serial serial ) : base( serial )
		{
		}

		#region IScissorable Members
		bool IScissorable.Scissor( Mobile from, Scissors scissors )
		{
			if( !Deleted && ( IsChildOf( from ) || IsChildOf( from.BankBox ) || (from.InRange( this, 3 ) && from.InLOS(this) ) ) )
			{
				from.SendAsciiMessage( "You put the bandages in your pack." );
				from.AddToBackpack( new Bandage( 3 ) );
				Delete();
				return true;
			}
			else
			{
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return false;
			}
		}
		#endregion

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