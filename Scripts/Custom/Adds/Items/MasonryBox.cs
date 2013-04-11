// Original Ingot Box Author Unknown
// Scripted by Karmageddon
using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;
using Server.Engines.Craft;

namespace Server.Items
{	
	public class MasonryBox : Item 
	{
		private int m_Granite;
		private int m_OldCopper;
		private int m_ShadowIron;
		private int m_Silver;
        private int m_Verite;
		private int m_Rose;
		private int m_Gold;
        private int m_Ice;
        private int m_Amethyst;
		private int m_Valorite;
        private int m_BloodRock;
        private int m_Aqua;
        private int m_Mytheril;
	    private int m_Dwarven;
        private int m_WithdrawIncrement;		
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Dwarven { get { return m_Dwarven; } set { m_Dwarven = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Mytheril { get { return m_Mytheril; } set { m_Mytheril = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Aqua { get { return m_Aqua; } set { m_Aqua = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BloodRock { get { return m_BloodRock; } set { m_BloodRock = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Amethyst { get { return m_Amethyst; } set { m_Amethyst = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Ice { get { return m_Ice; } set { m_Ice = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Granite{ get{ return m_Granite; } set{ m_Granite = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int OldCopper{ get{ return m_OldCopper; } set{ m_OldCopper = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ShadowIron{ get{ return m_ShadowIron; } set{ m_ShadowIron = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Silver{ get{ return m_Silver; } set{ m_Silver = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Rose{ get{ return m_Rose; } set{ m_Rose = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Gold{ get{ return m_Gold; } set{ m_Gold = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Verite{ get{ return m_Verite; } set{ m_Verite = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Valorite{ get{ return m_Valorite; } set{ m_Valorite = value; InvalidateProperties(); } }
		
		[Constructable]
		public MasonryBox() : base( 0xE80 )
		{
			Movable = true;
			Weight = 10.0;
			Hue = 991;
			Name = "Masonry Box";
			WithdrawIncrement = 10;
		}
		
		[Constructable]
		public MasonryBox( int withdrawincrement ) : base( 0xE80 )
		{
			Movable = true;
			Weight = 10.0;
			Hue = 991;
			Name = "Masonry Box";
			WithdrawIncrement = withdrawincrement;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 3 ) || !from.InLOS(this) )
			    from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			else if ( from is PlayerMobile )
			    from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
		}

		public void BeginCombine( Mobile from )
		{
			from.Target = new MasonryBoxTarget( this );
		}

		public void EndCombine( Mobile from, object o )
		{
			if ( o is Item && ((Item)o).IsChildOf( from.Backpack ) )
			{
				if (!( o is BaseGranite || o is BaseTool ))
				{
					from.SendMessage( "That is not an item you can put in here." );
				}
				if ( o is Granite )
				{

					if ( Granite >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Granite += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				if ( o is OldCopperGranite )
				{

					if ( OldCopper >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						OldCopper += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				if ( o is ShadowIronGranite )
				{

					if ( ShadowIron >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						ShadowIron += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				if (o is SilverGranite )
				{

					if ( Silver >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Silver += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				if (o is RoseGranite )
				{

					if ( Rose >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Rose += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				
				if (o is GoldGranite )
				{

					if ( Gold >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Gold += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				
				if (o is VeriteGranite )
				{

					if ( Verite >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Verite += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				if (o is ValoriteGranite )
				{

					if ( Valorite >= 100 )
					from.SendMessage( "That Granite type is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Valorite += curItem.Amount;
						curItem.Delete();
						from.SendGump( new MasonryBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
                if (o is IceGranite)
                {

                    if (Ice >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        Ice += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                if (o is AmethystGranite)
                {

                    if (Amethyst >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        Amethyst += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                if (o is BloodRockGranite)
                {

                    if (BloodRock >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        BloodRock += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                if (o is AquaGranite)
                {

                    if (Aqua >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        Aqua += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                if (o is DwarvenGranite)
                {

                    if (Dwarven >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        Dwarven += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                if (o is MytherilGranite)
                {

                    if (Mytheril >= 100)
                        from.SendMessage("That Granite type is too full to add more.");
                    else
                    {
                        Item curItem = o as Item;
                        Mytheril += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new MasonryBoxGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }	
				
			}
			else
			{
				from.SendLocalizedMessage( 1045158 ); // You must have the item in your backpack to target it.
			}
		}

		public MasonryBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) m_Granite);
			writer.Write( (int) m_OldCopper);
			writer.Write( (int) m_ShadowIron);
			writer.Write( (int) m_Silver);
			writer.Write( (int) m_Rose);			
			writer.Write( (int) m_Gold);		
			writer.Write( (int) m_Verite);
			writer.Write( (int) m_Valorite);
			writer.Write( (int) m_WithdrawIncrement);
            writer.Write((int)m_Ice);
            writer.Write((int)m_Mytheril);
            writer.Write((int)m_Dwarven);
            writer.Write((int)m_Amethyst);
            writer.Write((int)m_Aqua);
            writer.Write((int)m_BloodRock);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			m_Granite = reader.ReadInt();
			m_OldCopper = reader.ReadInt();
			m_ShadowIron = reader.ReadInt();
			m_Silver = reader.ReadInt();
			m_Rose = reader.ReadInt();			
			m_Gold = reader.ReadInt();
            m_Verite = reader.ReadInt();
			m_Valorite = reader.ReadInt();
            m_Ice = reader.ReadInt();
            m_Mytheril = reader.ReadInt();
            m_Dwarven = reader.ReadInt();
            m_Amethyst = reader.ReadInt();
            m_Aqua = reader.ReadInt();
            m_BloodRock = reader.ReadInt();
			m_WithdrawIncrement = reader.ReadInt();
		}
	}
}


namespace Server.Items
{
	public class MasonryBoxGump : Gump
	{
		private PlayerMobile m_From;
		private MasonryBox m_Box;

		public MasonryBoxGump( PlayerMobile from, MasonryBox box ) : base( 25, 25 )
		{
			m_From = from;
			m_Box = box;

			m_From.CloseGump( typeof( MasonryBoxGump ) );

			AddPage( 0 );

			AddBackground( 12, 19, 486, 290, 9250);
			AddLabel( 200, 30, 65, @"Masonry Box");
			
			AddLabel( 60, 50, 65, @"Add Item");
			AddButton( 25, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

			AddLabel( 60, 75, 65, @"Close");
			AddButton( 25, 75, 4005, 4007, 0, GumpButtonType.Reply, 0);

            AddLabel(60, 115, 1172, @"Granite");
			AddLabel( 150, 115, 1172, box.Granite.ToString() );
			AddButton( 25, 115, 4005, 4007, 3, GumpButtonType.Reply, 0);
			
			AddLabel( 60, 135, 67, @"Old Copper");
			AddLabel( 150, 135, 67, box.OldCopper.ToString() );
			AddButton( 25, 135, 4005, 4007, 4, GumpButtonType.Reply, 0);
			
			AddLabel( 60, 155, 1904, @"Shadow Iron");
			AddLabel( 150, 155, 1904, box.ShadowIron.ToString() );
			AddButton( 25, 155, 4005, 4007, 5, GumpButtonType.Reply, 0);
			
			AddLabel( 60, 175, 1172, @"Silver");
			AddLabel( 150, 175, 1172, box.Silver.ToString() );
			AddButton( 25, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);
			
			AddLabel( 60, 195, 2948, @"Rose");
			AddLabel(  150, 195, 2948, box.Rose.ToString() );
			AddButton(  25, 195, 4005, 4007, 7, GumpButtonType.Reply, 0 );
			
			AddLabel( 60, 215, 1001, @"Golden");
			AddLabel(  150, 215, 1001, box.Gold.ToString() );
			AddButton(  25, 215, 4005, 4007, 8, GumpButtonType.Reply, 0 );
									
			AddLabel( 60, 235, 2207, @"Verite");
			AddLabel( 150, 235, 2207, box.Verite.ToString() );
			AddButton( 25, 235, 4005, 4007, 9, GumpButtonType.Reply, 0 );			
			
			AddLabel( 60, 255, 1301, @"Valorite");
			AddLabel( 150, 255, 1301, box.Valorite.ToString() );
			AddButton( 25, 255, 4005, 4007, 10, GumpButtonType.Reply, 0);
            //
            AddLabel(210, 115, 2513, @"Ice");
            AddLabel(300, 115, 2513, box.Ice.ToString());
            AddButton(175, 115, 4005, 4007, 11, GumpButtonType.Reply, 0);

            AddLabel(210, 135, 1325, @"Mytheril");
            AddLabel(300, 135, 1325, box.Mytheril.ToString());
            AddButton(175, 135, 4005, 4007, 12, GumpButtonType.Reply, 0);

            AddLabel(210, 155, 1947, @"Dwarven");
            AddLabel(300, 155, 1947, box.Dwarven.ToString());
            AddButton(175, 155, 4005, 4007, 13, GumpButtonType.Reply, 0);

            AddLabel(210, 175, 1940, @"Aqua");
            AddLabel(300, 175, 1940, box.Aqua.ToString());
            AddButton(175, 175, 4005, 4007, 14, GumpButtonType.Reply, 0);

            AddLabel(210, 195, 1175, @"BloodRock");
            AddLabel(300, 195, 1175, box.BloodRock.ToString());
            AddButton(175, 195, 4005, 4007, 15, GumpButtonType.Reply, 0);

            AddLabel(210, 215, 1229, @"Amethyst");
            AddLabel(300, 215, 1229, box.Amethyst.ToString());
            AddButton(175, 215, 4005, 4007, 16, GumpButtonType.Reply, 0);
									
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Box.Deleted )
			return;

            if (!m_From.InRange(m_Box.GetWorldLocation(), 3) || !m_From.InLOS(m_Box))
                return;
			
			if ( info.ButtonID == 1)
			{
				m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				m_Box.BeginCombine( m_From );
			}
			
			if ( info.ButtonID == 3 )
			{				
				if (m_Box.Granite > 0)
                		{
                    			m_From.AddToBackpack(new Granite());  					//Sends all stored Granites of whichever type to players backpack
                    			m_Box.Granite = m_Box.Granite - 1;						     						//Sets the count in the key back to 0
                    			m_From.SendGump(new MasonryBoxGump(m_From, m_Box));					//Resets the gump with the new info
                		}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
			
			if ( info.ButtonID == 4 )
			{
				if ( m_Box.OldCopper > 0 )
				{
					m_From.AddToBackpack( new OldCopperGranite() );
					m_Box.OldCopper = m_Box.OldCopper - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
			if ( info.ButtonID == 5 )
			{
				if ( m_Box.ShadowIron > 0 )
				{
					m_From.AddToBackpack( new ShadowIronGranite() );
					m_Box.ShadowIron = m_Box.ShadowIron - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
			if ( info.ButtonID == 6 )
			{
				if ( m_Box.Silver > 0 )
				{
					m_From.AddToBackpack( new SilverGranite() );
					m_Box.Silver = m_Box.Silver - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
			if ( info.ButtonID == 7 )
			{
				if ( m_Box.Rose > 0 )
				{
					m_From.AddToBackpack( new RoseGranite() );
					m_Box.Rose = m_Box.Rose - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}			
			if ( info.ButtonID == 8 )
			{
				if ( m_Box.Gold > 0 )
				{
					m_From.AddToBackpack( new GoldGranite() );
					m_Box.Gold = m_Box.Gold - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
					
			if ( info.ButtonID == 9 )
			{
				if ( m_Box.Verite > 0 )
				{
					m_From.AddToBackpack( new VeriteGranite() );
					m_Box.Verite = m_Box.Verite - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
			if ( info.ButtonID == 10 )
			{
				if ( m_Box.Valorite > 0 )
				{
					m_From.AddToBackpack( new ValoriteGranite() );
					m_Box.Valorite = m_Box.Valorite - 1;
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
				}
				else
				{
					m_From.SendMessage( "You do not have any of that Granite!" );
					m_From.SendGump( new MasonryBoxGump( m_From, m_Box ) );
					m_Box.BeginCombine( m_From );
				}
			}
            if (info.ButtonID == 11)
            {
                if (m_Box.Ice > 0)
                {
                    m_From.AddToBackpack(new IceGranite());
                    m_Box.Ice = m_Box.Ice - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
            if (info.ButtonID == 12)
            {
                if (m_Box.Mytheril > 0)
                {
                    m_From.AddToBackpack(new MytherilGranite());
                    m_Box.Mytheril = m_Box.Mytheril - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
            if (info.ButtonID == 13)
            {
                if (m_Box.Dwarven > 0)
                {
                    m_From.AddToBackpack(new DwarvenGranite());
                    m_Box.Dwarven = m_Box.Dwarven - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
            if (info.ButtonID == 14)
            {
                if (m_Box.Aqua > 0)
                {
                    m_From.AddToBackpack(new AquaGranite());
                    m_Box.Aqua = m_Box.Aqua - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
            if (info.ButtonID == 15)
            {
                if (m_Box.BloodRock > 0)
                {
                    m_From.AddToBackpack(new BloodRockGranite());
                    m_Box.BloodRock = m_Box.BloodRock - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
            if (info.ButtonID == 16)
            {
                if (m_Box.Amethyst > 0)
                {
                    m_From.AddToBackpack(new AmethystGranite());
                    m_Box.Amethyst = m_Box.Amethyst - 1;
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new MasonryBoxGump(m_From, m_Box));
                    m_Box.BeginCombine(m_From);
                }
            }
		}
	}

}

namespace Server.Items
{
	public class MasonryBoxTarget : Target
	{
		private MasonryBox m_Box;

		public MasonryBoxTarget( MasonryBox box ) : base( 18, false, TargetFlags.None )
		{
			m_Box = box;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_Box.Deleted )
			return;

			m_Box.EndCombine( from, targeted );
		}
	}
}
