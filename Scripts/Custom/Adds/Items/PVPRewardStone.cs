using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Xanthos.ShrinkSystem;

namespace Server.Items
{
	public class PVPRewardStone : Item
	{
		[Constructable]
		public PVPRewardStone() : base( 0xED4 )
		{
			Name = "PVP Reward Stone";
			Movable = false;
			Hue = 1944;
		}

		public PVPRewardStone( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( this, 4 ) && from.InLOS(this) )
			{
				from.SendGump( new pvprewardgump() );
				from.PlaySound(47);
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


	public class pvprewardgump : Gump
	{
			#region Buttons enum
			public enum Buttons
		{
			clothdeed = 1,
			scrolltransformer = 2,
			karmadevice = 3,
			killremove = 4,
			littlegp  = 5,
			mediumgp = 6,
			weapondeed = 7,
			muchgp = 8,
			raredyetub = 9,
			layersash = 10,
			warmask = 11,
			warmount = 12,
            randomlantern = 13,
            lanterndeed = 14,
            randomjewelry = 15,
            jewelrydeed = 16,
		}
		#endregion
	
		private Mobile m_From;
		
		public pvprewardgump() : base( 0, 0 )
			
			
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
            AddImage(77, 515, 2607);//bottom
		    AddImage(503, 79, 2605); //right
            AddImage(503, 515, 2608);//bottom right corner
            AddImage(33, 515, 2606);//bottom left corner
			AddImage(33, 79, 2603); //left
		    AddImage(77, 36, 2601); //top
			AddImage(33, 35, 2600); //top left corner
			AddImage(504, 35, 2602); //top right corner
			AddImage(77, 80, 2604); //background
		    AddImage(77, 201, 2604);//background extension   
            AddImage(503, 200, 2605); //right extension
            AddImage(33, 200, 2603); //left extension
			AddImage(215, 285, 1418); //castle pic
			AddLabel(214, 57, 2960, @"PVP REWARDS STONE");
			AddButton(79, 121, 2117, 2118, (int)Buttons.clothdeed, GumpButtonType.Reply, 0);
			AddLabel(99, 119, 1174, @"Renamed/newbied/recolored Cloth Deed");
			AddButton(79, 149, 2117, 2118, (int)Buttons.scrolltransformer, GumpButtonType.Reply, 0);
			//AddLabel(99, 146, 1174, @"Scroll Transformer");
			//AddButton(79, 176, 2117, 2118, (int)Buttons.karmadevice, GumpButtonType.Reply, 0);
			AddLabel(99, 173, 1174, @"Karma Device");
			AddButton(79, 203, 2117, 2118, (int)Buttons.killremove, GumpButtonType.Reply, 0);
			AddLabel(99, 201, 1174, @"One Kill Remove Ball");
			AddButton(79, 229, 2117, 2118, (int)Buttons.littlegp, GumpButtonType.Reply, 0);
			AddLabel(99, 226, 1174, @"8000 GP");
			AddButton(79, 254, 2117, 2118, (int)Buttons.mediumgp, GumpButtonType.Reply, 0);
			AddLabel(99, 251, 1174, @"17000 GP");
			AddButton(79, 96, 2117, 2118, (int)Buttons.weapondeed, GumpButtonType.Reply, 0);
			AddLabel(99, 93, 1174, @"Renamed/recolored  Weapon Deed");
			AddButton(79, 280, 2117, 2118, (int)Buttons.muchgp, GumpButtonType.Reply, 0);
			AddLabel(99, 277, 1174, @"80000 GP");
			AddLabel(310, 93, 54, @"[30 Tourney tickets]");
			AddLabel(355, 118, 54, @"[5 Tourney tickets]");
			AddLabel(229, 146, 54, @"[11 Team tickets]");
			AddLabel(191, 173, 54, @"[11 Team tickets]");
			AddLabel(232, 201, 54, @"[5 Team tickets]");
			AddLabel(165, 226, 54, @"[1 Team ticket]");
			AddLabel(169, 251, 54, @"[2 Team tickets]");
			AddLabel(175, 277, 54, @"[1 Tourney ticket]");
			AddButton(79, 305, 2117, 2118, (int)Buttons.raredyetub, GumpButtonType.Reply, 0);
			AddLabel(99, 302, 1174, @"Random Rare Dyetub");
			AddLabel(235, 302, 54, @"[3 Tourney tickets]");
			AddButton(79, 329, 2117, 2118, (int)Buttons.layersash, GumpButtonType.Reply, 0);
			AddLabel(98, 326, 1174, @"Newbied Layered Sash");
			AddLabel(236, 326, 54, @"[3 Tourney tickets]");
			AddButton(79, 352, 2117, 2118, (int)Buttons.warmask, GumpButtonType.Reply, 0);
			AddLabel(99, 348, 1174, @"War Mask");
			AddLabel(169, 348, 54, @"[1 Team ticket]");
			AddButton(79, 375, 2117, 2118, (int)Buttons.warmount, GumpButtonType.Reply, 0);
			AddLabel(99, 371, 1174, @"War Bear Mount");
			AddLabel(207, 371, 54, @"[4 Tourney tickets]");
		    AddButton(79, 398, 2117, 2118, (int) Buttons.randomlantern, GumpButtonType.Reply, 0);
            AddLabel(99, 396, 1174, @"Random Colored Lantern");
            AddLabel(250, 396, 54, @"[5 Team tickets]");
            AddButton(79, 420, 2117, 2118, (int)Buttons.lanterndeed, GumpButtonType.Reply, 0);
            AddLabel(99, 418, 1174, @"Renamed/newbied/recolored Lantern");
            AddLabel(314, 418, 54, @"[4 Tourney tickets]");
            AddButton(79, 442, 2117, 2118, (int)Buttons.randomjewelry, GumpButtonType.Reply, 0);
            AddLabel(99, 440, 1174, @"Random Colored Jewelry");
            AddLabel(250, 440, 54, @"[3 Team tickets]");

            AddButton(79, 464, 2117, 2118, (int)Buttons.jewelrydeed, GumpButtonType.Reply, 0);
            AddLabel(99, 460, 1174, @"Renamed/newbied/recolored Jewelry");
            AddLabel(314, 460, 54, @"[30 Team tickets]");

		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{	
			m_From = sender.Mobile;
			
			Container pack = m_From.Backpack;
			Container bank = m_From.BankBox;
			
			string nomoney = "You do not have sufficient team or tourney tickets to buy this";

			switch( info.ButtonID )
			{
				case 0:
				{
					m_From.PlaySound( 46 );
					break;
				}	
				
				case 1:
				{
					if( (  pack != null && pack.ConsumeTotal( typeof( TourTicket ), 5 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 5 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new ClothDeed() );
						m_From.SendAsciiMessage("You bought a R/R Cloth Deed, page a staff member to help you convert it!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 2:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 11 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 11 ) ) )
					{
						m_From.PlaySound( 247 );
						//m_From.Backpack.DropItem( new ScrollTransformer() );
						m_From.SendAsciiMessage("You bought a Scroll Transformer!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 3:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 11 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 11 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new MovableKarmaBong() );
						m_From.SendAsciiMessage("You bought a Karma Bong!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}	
					
				case 4:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 5 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 5 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new KillRemoveBall() );
						m_From.SendAsciiMessage("You bought a Kill Removal ball, double click on the ball to remove 1 kill!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}	
				
				case 5:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 1 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 1 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new Gold(8000) );
						
						m_From.SendAsciiMessage("You place the gold in your backpack!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 6:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 2 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 2 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new Gold(17000) );
						
						m_From.SendAsciiMessage("You place the gold in your backpack!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 7:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TourTicket ), 30 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 30 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new WeaponDeed() );
						m_From.SendAsciiMessage("You bought a  R/R Weapon Deed, page a staff member to help you convert it!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
			
				case 8:
				{
					if ( ( pack != null && pack.ConsumeTotal( typeof( TourTicket ), 1 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 1 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new Gold(60000) );
						m_From.Backpack.DropItem( new Gold(20000) );
						
						m_From.SendAsciiMessage("You place the gold in your backpack!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
			
				case 9:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TourTicket ), 3 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 3 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new RareDyeTub() );
						
						m_From.SendAsciiMessage("You bought a Rare Dye Tub!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
			
				case 10:
				{
					if( (  pack != null && pack.ConsumeTotal( typeof( TourTicket ), 3 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 3 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new LayeredBodySash() );
						
						m_From.SendAsciiMessage("You bought a Layered Body Sash!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 11:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 1 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 3 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new WarMask() );
						
						m_From.SendAsciiMessage("You bought a War Mask!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}
				
				case 12:
				{
					if( ( pack != null && pack.ConsumeTotal( typeof( TourTicket ), 4 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TourTicket ), 4 ) ) )
					{
						m_From.PlaySound( 247 );
						m_From.Backpack.DropItem( new ShrinkItem(new WarBearMount()));
						
						m_From.SendAsciiMessage("You bought a War Bear!");
					}
					else
						m_From.SendAsciiMessage( nomoney );
						m_From.PlaySound( 46 );
					break;
				}

                case 13:
			    {
			            if( ( pack != null && pack.ConsumeTotal( typeof( TeamTourTicket ), 5 ) ) || ( bank != null && bank.ConsumeTotal( typeof( TeamTourTicket ), 5 ) ) )
			            {
                            m_From.PlaySound(247);
                            m_From.Backpack.DropItem(new RandomLanternDeed());

                            m_From.SendAsciiMessage("You bought a Random Lantern Deed!");
			            }
                        else
                            m_From.SendAsciiMessage(nomoney);
                        m_From.PlaySound(46);
                        break;
			    }

                case 14:
                {
                    if ((pack != null && pack.ConsumeTotal(typeof(TourTicket), 4)) || (bank != null && bank.ConsumeTotal(typeof(TourTicket), 4)))
                    {
                        m_From.PlaySound(247);
                        m_From.Backpack.DropItem(new LanternDeed());

                        m_From.SendAsciiMessage("You bought a R/R Lantern Deed! Page staff to help you convert it.");
                    }
                    else
                        m_From.SendAsciiMessage(nomoney);
                    m_From.PlaySound(46);
                    break;
                }

                case 15:
                {
                    if ((pack != null && pack.ConsumeTotal(typeof(TeamTourTicket), 3)) || (bank != null && bank.ConsumeTotal(typeof(TeamTourTicket), 3)))
                    {
                        m_From.PlaySound(247);
                        m_From.Backpack.DropItem(new RandomJewelryDeed());

                        m_From.SendAsciiMessage("You bought a Random Jewelry Deed!");
                    }
                    else
                        m_From.SendAsciiMessage(nomoney);
                    m_From.PlaySound(46);
                    break;
                }

                case 16:
                {
                    if ((pack != null && pack.ConsumeTotal(typeof(TeamTourTicket), 30)) || (bank != null && bank.ConsumeTotal(typeof(TeamTourTicket), 30)))
                    {
                        m_From.PlaySound(247);
                        m_From.Backpack.DropItem(new JewelryDeed());

                        m_From.SendAsciiMessage("You bought a R/R/N Jewelry Deed! Page staff to help you convert it.");
                    }
                    else
                        m_From.SendAsciiMessage(nomoney);
                    m_From.PlaySound(46);
                    break;
                }
			}
		}
	}
}