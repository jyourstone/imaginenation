using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public interface DeepSeaFishingPoleItem
	{
		int Charges{ get; set; }
		//int Recharges{ get; set; }
		int MaxCharges{ get; }
		//int MaxRecharges{ get; }
		string DeepSeaFishingPoleItemName{ get; }
	}

	public class FishingBait : Item
	{
		[Constructable]
		public FishingBait() : this( 1 )
		{
		}

		[Constructable]
		public FishingBait( int amount ) : base( 0x9D2 )
		{
		    Name = "Fish bait";
			Stackable = true;
			Weight = 0.1;
		    Hue = 456;
			Amount = amount;
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
            {
                from.SendAsciiMessage("That is not accessible.");
            }
            else if (from.InRange(GetWorldLocation(), 2))
            {
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }	

		private class InternalTarget : Target
		{
			private readonly FishingBait m_Bait;

			public InternalTarget( FishingBait bait ) : base( -1, false, TargetFlags.None )
			{
				m_Bait = bait;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bait.Deleted )
					return;

				if ( !from.InRange( m_Bait.GetWorldLocation(), 2 ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				}
				else if ( targeted is DeepSeaFishingPoleItem )
				{
					DeepSeaFishingPoleItem chargeItem = (DeepSeaFishingPoleItem)targeted;

					if ( chargeItem.Charges >= chargeItem.MaxCharges )
					{
                        from.SendAsciiMessage("The pole cannot hold any more bait");
					}
					/*else if ( chargeItem.Recharges >= chargeItem.MaxRecharges )
					{
                        from.SendAsciiMessage("The pole has worn out and can no longer hold bait");
					}*/
					else
					{
						if ( chargeItem.Charges + m_Bait.Amount > chargeItem.MaxCharges )
						{
							int delta = chargeItem.MaxCharges - chargeItem.Charges;

							m_Bait.Amount -= delta;
							chargeItem.Charges = chargeItem.MaxCharges;
							//chargeItem.Recharges += delta;
						}
						else
						{
							chargeItem.Charges += m_Bait.Amount;
							//chargeItem.Recharges += m_Bait.Amount;
							m_Bait.Delete();
						}

						if ( chargeItem is Item )
						{
                            from.SendAsciiMessage("You attach the bait to the pole");
						}
					}
				}
				else
				{
                    from.SendAsciiMessage("The bait cannot be attached to this item");
				}
			}
		}

		public FishingBait( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}