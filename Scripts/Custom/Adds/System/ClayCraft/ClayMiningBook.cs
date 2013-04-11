/* Created by Hammerhand*/

using Server.Mobiles;

namespace Server.Items
{
	public class ClayMiningBook : Item
	{
		[Constructable]
		public ClayMiningBook() : base( 0xFF4 )
		{
            Name = "Mining Clay";
			Weight = 1.0;
            Hue = 1115;
		}

        public ClayMiningBook(Serial serial)
            : base(serial)
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
			PlayerMobile pm = from as PlayerMobile;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( pm == null || from.Skills[SkillName.Mining].Base < 100.0 )
			{
				pm.SendMessage( "Only a Grandmaster Miner can learn from this book." );
			}
			else if ( pm.ClayMining )
			{
				pm.SendMessage( "You have already learned this information." );
			}
			else
			{
				pm.ClayMining = true;
                pm.SendMessage("You have learned how to mine chunks of Clay. Target swamp areas when mining to look for Clay.");
				Delete();
			}
		}
	}
}