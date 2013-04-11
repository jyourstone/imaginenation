using System;
using Server.Guilds;
using Server.Items;
//bounty system here
using Server.BountySystem;
//end bounty system

namespace Server.Mobiles
{
	public class OrderGuard : BaseShieldGuard
	{
		public override int Keyword{ get{ return 0x21; } } // *order shield*
		public override BaseShield Shield{ get{ return new OrderShield(); } }
		public override int SignupNumber{ get{ return 1007141; } } // Sign up with a guild of order if thou art interested.
		public override GuildType Type{ get{ return GuildType.Order; } }

		public override bool BardImmune{ get{ return true; } } 

		[Constructable]
		public OrderGuard()
		{
		}

		public OrderGuard( Serial serial ) : base( serial )
		{
		}

        //bounty system here
        public override bool OnDragDrop(Mobile from, Item item)
        {
            try
            {
                bool IsHandled = false;

                // Check if the item being dropped is a head
                if (item is Head)
                {
                    Head head = (Head)item;

                    this.Say(500670); // Ah, a head! Let me check to see if there is a bounty on this.

                    BountyBoardEntry entry;
                    bool canClaim = false;
                    Mobile murderer = (Mobile)head.Owner;

                    // Check if the head is a valid head
                    if (murderer != null && head.Killer != null)
                    {
                        // check if there is a bounty and if the murderer can claim it
                        if (BountyBoard.hasBounty(from, murderer, out entry, out canClaim))
                        {
                            // check if the claimer killed the murderer
                            if (head.Killer != from)
                            {
                                Say(500543); // I had heard this scum was taken care of...but not by you
                            }
                            else
                            {
                                //check age of head
                                if (head.CreationTime < (entry.ExpireTime - BountyBoardEntry.DefaultDecayRate))
                                {
                                    Say("Their is a bounty on this murderer but this head is from long ago!");
                                }
                                else
                                {
                                    // Check that the player does not have negative karma or is a criminal
                                    if (from.Karma >= 0 && from.Kills < 5 && !from.Criminal)
                                    {
                                        if (canClaim)
                                        {
                                            if (entry.Expired)
                                            {
                                                Say("The bounty on this murderer has expired.");
                                                BountyBoard.RemoveEntry(entry, true);
                                            }
                                            else
                                            {
                                                IsHandled = true;
                                                Say(1042855, String.Format("{0}\t{1}\t", murderer.Name, entry.Price.ToString())); // The bounty on ~1_PLAYER_NAME~ was ~2_AMOUNT~ gold, and has been credited to your account.
                                                from.BankBox.DropItem(new Gold(entry.Price));

                                                // Give the karma they lost back + an amount equal to the bounty
                                                from.Karma += entry.Price;
                                                BountyBoard.RemoveEntry(entry, false);
                                                head.Delete();
                                            }
                                        }
                                        else
                                        {
                                            Say("The bounty owner did not approve a reward to you!");
                                        }
                                    }
                                    else
                                    {
                                        Say(500542); // We only accept bounty hunting from honorable folk! Away with thee!
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.Say(1042854, murderer.Name); // There was no bounty on ~1_PLAYER_NAME~.						
                        }
                    }
                    else
                    {
                        Say(500660); // If this were the head of a murderer, I would check for a bounty.
                    }
                }
                else
                {
                    return base.OnDragDrop(from, item);
                }

                return IsHandled;
            }
            catch
            {
                return false;
            }
        }
        //end bounty system

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