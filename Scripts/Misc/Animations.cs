namespace Server.Misc
{
	public class Animations
	{
		public static void Initialize()
		{
			EventSink.AnimateRequest += EventSink_AnimateRequest;
		}

		private static void EventSink_AnimateRequest( AnimateRequestEventArgs e )
		{
			Mobile from = e.Mobile;

			int action;
            int delay = 0;

			switch ( e.Action )
			{
                case "bow":
                    {
                        if (!from.Mounted)
                            action = 32;
                        else
                        {
                            action = 28;
                            delay = 1;
                        }
                        break;
                    }
                case "salute":
                    {
                        if (!from.Mounted)
                            action = 33;
                        else
                        {
                            action = 28;
                            delay = 1;
                        }
                        break;
                    }
				default: return;
			}

            from.Animate(action, 5, 1, true, false, delay);
		}
	}
}