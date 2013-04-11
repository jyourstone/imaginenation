using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Scripts.Custom.Adds.Commands;

namespace Server.Misc
{
	public class Keywords
	{
		public static void Initialize()
		{
			// Register our speech handler
			EventSink.Speech += EventSink_Speech;
		}

		public static void EventSink_Speech( SpeechEventArgs args )
		{
			Mobile from = args.Mobile;
			int[] keywords = args.Keywords;

			for ( int i = 0; i < keywords.Length; ++i )
			{
				switch ( keywords[i] )
				{
					case 0x002A: // *i resign from my guild*
					{
						if ( from.Guild != null )
							((Guild)from.Guild).RemoveMember( from );

                        if (from is PlayerMobile && ((PlayerMobile)from).HasCustomRace)
                            ResetRace.DoResetRace(from);

						break;
					}
					case 0x0032: // *i must consider my sins*
					{
                        if (NotorietyHandlers.IsGuardCandidate(from))
                            from.SendAsciiMessage("Thou hast done great naughty! I wouldst not show my face in town if I were thee.");
                        else
                            from.SendAsciiMessage("Fear not, thou hast not commited a criminal act.");
                        
                        break;
					}
					case 0x0035: // i renounce my young player status*
					{
						if ( from is PlayerMobile && ((PlayerMobile)from).Young && !from.HasGump( typeof( RenounceYoungGump ) ) )
						{
							from.SendGump( new RenounceYoungGump() );
						}

						break;
					}
				}
			}
            if (args.Speech.ToLower().Contains("i have nothing to repent over"))
            {
                if (from is PlayerMobile && from.Kills > 4)
                {
                    PlayerMobile pm = ((PlayerMobile) from);
                    if (!pm.AlwaysMurderer)
                    {
                        pm.AlwaysMurderer = true;
                        from.SendAsciiMessage("You will no longer loose any killcounts");
                    }
                    else
                        from.SendAsciiMessage("Yes yes, I heard you the first time");
                }
                else
                    from.SendAsciiMessage("You need at least 5 killcounts to do this"); 
            }
            else if (args.Speech.ToLower().Contains("i wish to repent my sins"))
            {
                PlayerMobile pm = ((PlayerMobile)from);
                if (pm.AlwaysMurderer)
                {
                    pm.AlwaysMurderer = false;
                    from.SendAsciiMessage("You will now loose killcounts again");
                }
                else
                    from.SendAsciiMessage("Your killcounts already decays");
            }
		}
	}
}