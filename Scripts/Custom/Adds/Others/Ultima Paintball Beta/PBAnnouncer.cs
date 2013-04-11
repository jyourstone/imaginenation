/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/



using System;
using System.Text;
using System.Collections.Generic;
using Server;
using Server.Prompts;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Misc;
using Server.Commands;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Games.PaintBall
{
	public class PBAnnouncer : Mobile
	{
		
		public PBGameItem m_PBGI;
		[Constructable]
		public PBAnnouncer( PBGameItem pbgi )
		{
			
            m_PBGI = pbgi;
			
			InitStats( 100, 100, 25 );

			Title = "the painball game announcer";
			Hue = Utility.RandomSkinHue();

				NameHue = 0x35;
				
				this.Blessed = true;

			if ( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );
			}
			

			AddItem( new HoodedShroudOfShadows( 1 ) );
			if ( pbgi != null )
			{
				pbgi.Announcers.Add(this);
			}
		}

		public override void OnAfterDelete()
		{
		if ( m_PBGI != null )
		{
			if( m_PBGI.Announcers.Contains( this) )
			{	
			m_PBGI.Announcers.Remove( this );
			}
		}
			else
			{
				
			}
		}
		public override void OnDoubleClick( Mobile from )
		{
			if( m_PBGI == null )
			{
				this.Delete();
			    from.SendMessage( "This is not connected to a paintball system." );
			}
		}
		public PBAnnouncer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Core.AOS && NameHue == 0x35 )
				NameHue = -1;
		}
	}
  }


