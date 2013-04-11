using Server.Network;
using Xanthos.Interfaces;
using Xanthos.ShrinkSystem;

namespace Server.Items
{
	public class ShrinkPotion : BasePotion, IShrinkTool
	{
		private int shrinkCharges = 1;

		public ShrinkPotion( Serial serial ) : base( serial )
		{
		}

		[Constructable]
		public ShrinkPotion() : base( 0xF0B, PotionEffect.Shrink )
		{
			Hue = 902;
			Name = "Shrink potion";
		}

		#region IShrinkTool Members
		public int ShrinkCharges
		{
			get { return shrinkCharges; }
			set { shrinkCharges = value; }
		}
		#endregion

		public override void Drink( Mobile from )
		{
			if( from.InRange( GetWorldLocation(), 1) && from.InLOS( this ) )
				from.Target = new ShrinkTarget( from, this, false );
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