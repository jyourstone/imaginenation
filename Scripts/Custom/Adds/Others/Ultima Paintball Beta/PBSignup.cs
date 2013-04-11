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
using Server;
using System.IO;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Text;
using Server.Mobiles;
using System.Collections;
using Server.Targeting;
using Server.Commands;
using System.Reflection;
using Server.Prompts;
using Server.Games.PaintBall;

namespace Server.Games.Paintball
{
    public class PBSignup : Gump
    {
        Mobile mob;
        public PBGameItem m_PBGI;
		public static string m_message;

		public static void GetFile()
		{
			string filePath = Path.Combine( Core.BaseDirectory, "Data/pbrules.txt" );

			if ( !File.Exists( filePath ) )
				return;

			try
			{
				Load( filePath );
			}
			catch ( Exception e )
			{
				Console.WriteLine( "Warning: Exception caught loading name lists:" );
				Console.WriteLine( e );
			}
			//mob.SendGump( new Motd() );
		}
		public static void Load( string filePath)
		{
			m_message = String.Empty;
			StreamReader SR;
			string S;
			SR=File.OpenText(filePath);
			S=SR.ReadLine();
			while(S!=null)
			{
			m_message += S;
			S=SR.ReadLine();
			}
			SR.Close();
		}

        public PBSignup(Mobile from, PBGameItem pbgi ) : base( 0, 0 )
        {
        	mob = from;
        	GetFile();
        	m_PBGI = pbgi;
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			

			AddPage(0);
			AddBackground(159, 98, 430, 430, 9350);
			AddImage(165, 101, 5504, 1265);
			AddImage(517, 101, 5504, 1265);
			AddLabel(313, 108, 47, @"Ultima Paintball");
			AddLabel(346, 133, 47, @"Signup");
			AddHtml( 180, 214, 393, 171, m_message, (bool)false, (bool)true);
			AddButton(290, 499, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			AddButton(393, 499, 243, 241, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			AddHtml( 180, 397, 392, 49, @"I, " + from.Name + ", Have read the rules and wish to compete in this tournament", (bool)false, (bool)false);
			

            
        }

        		public enum Buttons
		{
			Button1,
			Button2,
		}


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case (int)Buttons.Button1:
				{
            			m_PBGI.AddPlayer(from);
					break;
				}
				case (int)Buttons.Button2:
				{

					break;
				}

            }
        }
    }
}