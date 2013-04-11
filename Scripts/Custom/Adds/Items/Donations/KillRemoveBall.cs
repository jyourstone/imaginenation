using Server.Network;

namespace Server.Items
{
	public class KillRemoveBall : Item
	{
		[Constructable]
		public KillRemoveBall() : base(0x3198)
		{
			Name = "Kill remove ball";
			Hue = 2544;
		}

		public KillRemoveBall( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (IsChildOf(from.Backpack))
            {
                if (from.Kills != 0)
                {
                    from.BoltEffect(0);
                    --from.Kills;
                    from.SendAsciiMessage("A killcount has been removed!");
                    from.SendMessage("Your new killcount is: {0}", from.Kills);
                    Consume();

                }
                else
                    from.SendMessage("You are already at 0 kills!");
            }
            else
                from.SendLocalizedMessage(1060640); // "This item must be in your backpack to use it"
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