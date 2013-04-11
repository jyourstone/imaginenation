using System;
using Server;
using System.Collections.Generic;
using Server.Commands; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{ 
   public class LudwigGump : Gump 
   { 
      public static void Initialize() 
      { 
         CommandSystem.Register( "LudwigGump", AccessLevel.GameMaster, new CommandEventHandler( LudwigGump_OnCommand ) ); 
      } 

      public static void LudwigGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new LudwigGump( e.Mobile ) ); 
      } 

      public LudwigGump( Mobile owner ) : base( 50,50 ) 
      { 
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Ludwig's Missing Masterpieces" );
			

			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=WHITE>*Ludwig waves for your attention*<BR>Please traveler, could you assist me?<BR>" +
"<BASEFONT COLOR=WHITE>I am a simple musician, hired by Lord British himself and I am to perform for him tomorrow evening.<BR>" +
"<BASEFONT COLOR=WHITE>However, while setting out for a peaceful place to finish my greatest work I was ambushed by mischievous imps!<BR>" +
"<BASEFONT COLOR=WHITE>They ran off with all my best works and if I do not have them by the time I am to perform I will surely be jailed!<BR> "+
"<BASEFONT COLOR=WHITE>Please find those evil, violet colored imps and make them pay!  I'm sure they are still nearby, hiding in the woods.<BR>" +
"<BASEFONT COLOR=WHITE>If you can find 8 pieces of my sheet music I shall reward you with one of my greatest possessions." +
 "</BODY>", false, true);
			

			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
               //Cancel 
               //from.SendMessage( "Thank you!" );
               break; 
            } 

         }
      }
   }
}
