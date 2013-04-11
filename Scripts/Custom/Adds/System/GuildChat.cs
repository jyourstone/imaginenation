/*using System;
using Server.Guilds;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{ 
   public class G 
      { 
      private static int usercount; 
      
      public static void Initialize() 
      { 
         CommandSystem.Register( "G", AccessLevel.Player, G_OnCommand ); 
		 CommandSystem.Register( "ListenToGuild", AccessLevel.GameMaster, ListenToGuild_OnCommand );
      } 
	  
	  public static void ListenToGuild_OnCommand( CommandEventArgs e )
	  {
		 e.Mobile.BeginTarget( -1, false, TargetFlags.None, ListenToGuild_OnTarget );
		 e.Mobile.SendMessage( "Target a guilded player." );
	  }
	  
	  public static void ListenToGuild_OnTarget( Mobile from, object obj )
	  {
		 if ( obj is Mobile )
		 {
			var g = ((Mobile)obj).Guild as Guild;


			 if ( g == null )
			 {
				from.SendMessage( "They are not in a guild." );
			 }
			 else if ( g.Listeners.Contains( from ) )
			 {
				g.RemoveListener( from );
				from.SendMessage( "You are no longer listening to " + g.Name);
			 }
			 else
			 {
				g.AddListener( from );
				
				from.SendMessage( "You are now listening to " + g.Name );
			 }
		 }
	 }

      [Usage( "G [<text>|list]" )] 
      [Description( "Broadcasts a message to all online members of your guild." )] 
      private static void G_OnCommand( CommandEventArgs e ) 
      { 
         switch( e.ArgString.ToLower() ) 
         { 
            case "list": 
               List ( e.Mobile ); 
               break; 
            default: 
               Msg ( e ); 
               break; 
         } 
      } 

      private static void List( Mobile g ) 
      { 
         usercount = 0; 
         var GuildC = g.Guild as Guild; 
         if ( GuildC == null ) 
         { 
            g.SendMessage( "You are not in a guild!" ); 
         } 
         else 
         { 
            foreach ( var state in NetState.Instances ) 
            { 
               var m = state.Mobile; 
               if ( m != null && GuildC.IsMember( m ) ) 
               { 
                  usercount++; 
               } 
            } 
            if (usercount == 1) 
            { 
               g.SendMessage( "There is 1 member of your guild online." ); 
            } 
            else 
            { 
               g.SendMessage( "There are {0} members of your guild online.", usercount ); 
            } 
            g.SendMessage ("Online list:" ); 
            foreach ( var state in NetState.Instances ) 
            { 
               var m = state.Mobile; 
               if ( m != null && GuildC.IsMember( m ) ) 
               { 
                  var region = m.Region.ToString(); 
                  if (region == "Region") 
                  { 
                     region = "Britannia"; 
                  }
                  g.SendMessage( "{0} ({1})", m.Name, region ); 
               } 
            }
         }
      }

      private static void Msg( CommandEventArgs e  ) 
      { 
         var from = e.Mobile;

         if (from.Squelched)
         {
             from.SendAsciiMessage("You can not say anything, you have been squelched.");
             return;
         }

         var GuildC = from.Guild as Guild; 

         if ( GuildC == null ) 
         { 
            from.SendMessage( "You are not in a guild!" ); 
         } 
         else
         {
             string guild = from.Guild.Abbreviation == "none" ? from.Guild.Name : from.Guild.Abbreviation;
             EventSink.InvokeGuildChat(new GuildChatEventArgs(from, GuildC, e.ArgString));
             foreach (var state in NetState.Instances)
             {
                 var m = state.Mobile;
                 if (m != null && GuildC.IsMember(m))
                 {
                     m.SendMessage(from.SpeechHue, String.Format("Guild [{0}]: {1}", from.Name, e.ArgString));
                 }
             }

             for (var i = 0; i < GuildC.Listeners.Count; ++i)
             {
                 var mob = (Mobile) GuildC.Listeners[i];

                 if (mob.Guild != GuildC)
                     ((Mobile) GuildC.Listeners[i]).SendMessage("{0}[{1}]: {2}", guild, from.Name, e.ArgString);
             }

            //Staff can see guildchat
            Packet p = null;
            foreach (NetState ns in from.GetClientsInRange(14))
            {
                var mob = ns.Mobile;

                if (mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel )
                {
                    if (p == null)
                        p = Packet.Acquire(new UnicodeMessage(from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, String.Format("[GuildChat]: {0}", e.ArgString)));

                    ns.Send(p);
                }
            }

            Packet.Release(p);
         }
      }
   } 
}*/