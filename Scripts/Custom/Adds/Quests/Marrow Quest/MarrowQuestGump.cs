using System; using Server; using Server.Commands;using Server.Gumps; using Server.Network;using Server.Items;using Server.Mobiles;namespace Server.Gumps
{ public class MarrowQuestGump : Gump { 
public static void Initialize() { 
CommandSystem.Register( "MarrowQuestGump", AccessLevel.GameMaster, new CommandEventHandler( MarrowQuestGump_OnCommand ) ); 
}
private static void MarrowQuestGump_OnCommand( CommandEventArgs e ) 
{
e.Mobile.SendGump( new MarrowQuestGump( e.Mobile ) ); } 
public MarrowQuestGump( Mobile owner ) : base( 50,50 ) 
{
//----------------------------------------------------------------------------------------------------
AddPage( 0 );AddImageTiled(  54, 33, 369, 400, 2624 );AddAlphaRegion( 54, 33, 369, 400 );AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
AddImage( 97, 49, 9005 );AddImageTiled( 58, 39, 29, 390, 10460 );AddImageTiled( 412, 37, 31, 389, 10460 );
AddLabel( 140, 60, 0x34, "Marrow's Task" );
//----------------------/----------------------------------------------/
AddHtml( 107, 140, 300, 230, " < BODY > " + 
"<BASEFONT COLOR=YELLOW>Hello there, might you be interested in<BR>" +
"<BASEFONT COLOR=YELLOW>helping me with a little task?<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>I am researching a new type of potion<BR>" +
"<BASEFONT COLOR=YELLOW>and I need some reptile bones to finish<BR>" +
"<BASEFONT COLOR=YELLOW>my tests.  As you can see though, I am <BR>" +
"<BASEFONT COLOR=YELLOW>no fighter and cannot get them myself.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>Although my research has made my coin <BR>" +
"<BASEFONT COLOR=YELLOW>purse rather light these days I will be<BR>" +
"<BASEFONT COLOR=YELLOW>sure to reward you with something.  I<BR>" +
"<BASEFONT COLOR=YELLOW>just need 20 bones, that should be <BR>" +
"<BASEFONT COLOR=YELLOW>plenty.  You should be able to find<BR>" +
"<BASEFONT COLOR=YELLOW>some right outside of town.  Thanks!<BR>" +
"</BODY>", false, true);
//----------------------/----------------------------------------------/
AddImage( 430, 9, 10441);AddImageTiled( 40, 38, 17, 391, 9263 );AddImage( 6, 25, 10421 );AddImage( 34, 12, 10420 );AddImageTiled( 94, 25, 342, 15, 10304 );AddImageTiled( 40, 427, 415, 16, 10304 );AddImage( -10, 314, 10402 );AddImage( 56, 150, 10411 );AddImage( 155, 120, 2103 );AddImage( 136, 84, 96 );AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); }
//----------------------/----------------------------------------------/
public override void OnResponse( NetState state, RelayInfo info ){ Mobile from = state.Mobile; switch ( info.ButtonID ) { case 0:{ break; }}}}}
