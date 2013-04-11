using System;
using Server.Accounting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "Finlor's corpse" )]
	public class Finlor : Mobile
	{
        public override bool ClickTitle { get { return true; } }
                public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public Finlor()
		{
			Name = "Finlor";
                        Title = "the Sea Captain";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();
			Blessed = true;
			CantWalk = true;
			Direction = Direction.South;

			ThighBoots tb = new ThighBoots();
                        tb.Hue = 0;
                        AddItem( tb );

                        LongPants lp = new LongPants();
                        lp.Hue = 6;
                        AddItem( lp );

		        FancyShirt fs = new FancyShirt();
                        fs.Hue = 0;
                        AddItem( fs );

			TricorneHat th = new TricorneHat();
                        th.Hue = 52;
                        AddItem( th );

			BodySash bs = new BodySash();
			bs.Hue = 6;
			AddItem( bs );

			Cloak cl = new Cloak();
			cl.Hue = 38;
			AddItem( cl );

	                Scimitar sc = new Scimitar();
                        AddItem( sc );

			GoldBeadNecklace gn = new GoldBeadNecklace();
			AddItem( gn );

			GoldBracelet gb = new GoldBracelet();
			AddItem( gb );

			GoldEarrings ge = new GoldEarrings();
			AddItem( ge );

			GoldRing gr = new GoldRing();
			AddItem( gr );			
                        
                        AddItem( new PonyTail(1149));
                     
			AddItem( new Vandyke(1149));
			
		}

		public Finlor( Serial serial ) : base( serial )
		{
		}
        public override bool ShowContextMenu { get { return true; } }//
        public override void GetContextMenuEntries (Mobile from, System.Collections.Generic.List<ContextMenuEntry> list) 
	        { 
	                base.GetContextMenuEntries( from, list ); 
        	        list.Add( new FinlorEntry( from, this ) ); 
	        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class FinlorEntry : ContextMenuEntry
		{
			private readonly Mobile m_Mobile;
			private Mobile m_Giver;
			
			public FinlorEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
				

                          if( !( m_Mobile is PlayerMobile ) )
					return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				{
					if ( ! mobile.HasGump( typeof( FinlorGump ) ) )
					{
						mobile.SendGump( new FinlorGump( mobile ));
						mobile.AddToBackpack( new LetterToOrthal() );
						mobile.AddToBackpack( new SeaChest() );
					} 
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
         	        Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;
			Account acct=(Account)from.Account;
			bool MysticalSextantReceived = Convert.ToBoolean( acct.GetTag("MysticalSextantReceived") );

			if ( mobile != null)
			{
				if( dropped is CompletedSeafarerToolKit )
         			{
         				if(dropped.Amount!=1)
         				{
						PrivateOverheadMessage( MessageType.Regular, 1153, false, "Complete the Seafarer's Tool Kit!", mobile.NetState );
         					return false;
         				}
					if ( !MysticalSextantReceived ) //added account tag check
		                	{
						dropped.Delete(); 
						mobile.AddToBackpack( new MysticalSextant() );
		                mobile.AddToBackpack(new Gold (7500));
						mobile.SendGump( new FinlorFinishGump( mobile ));
                               	 	        acct.SetTag( "MysticalSextantReceived", "true" );				
         		      		}
					else //what to do if account has already been tagged
         				{
         					PrivateOverheadMessage( MessageType.Regular, 1153, false, "I see you have decided to complete another seafarer's tool kit.  Here is your reward.", mobile.NetState );
         					mobile.AddToBackpack( new MysticalSextant() );
                            mobile.AddToBackpack(new Gold(2500));
         					dropped.Delete();
         				}
         			}				
				if( dropped is MasterOfTheSeaChest )
				{
					dropped.Delete();
					mobile.AddToBackpack( new SeafarerToolKit() );
					mobile.AddToBackpack( new LetterToSnyden() );
					mobile.SendGump( new FinlorMidGump( mobile ));
					return true;
				} 
				else
         		{
					PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need for this item.", mobile.NetState );
     			}
			}
			return false;
		}
	}
}