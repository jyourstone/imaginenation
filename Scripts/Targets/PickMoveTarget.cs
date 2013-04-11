using Server.Commands.Generic;
using Server.Targeting;

namespace Server.Targets
{
	public class PickMoveTarget : Target
	{
		public PickMoveTarget() : base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( !BaseCommand.IsAccessible( from, o ) )
			{
				from.SendMessage( "That is not accessible." );
				return;
			}

            if (o is Item)
            {
                from.SendAsciiMessage("Where would you like to move the item?");
                from.Target = new MoveTarget(o);
            }
            else if (o is Mobile)
            {
                Mobile m = (Mobile)o;

                if (string.IsNullOrEmpty(m.Name))
                    from.SendAsciiMessage(string.Format("Where would you like to move {0}?", m.Name));
                else
                    from.SendAsciiMessage(string.Format("Where would you like to move {0}?", m));

                from.Target = new MoveTarget(o);
            }
            else
                from.SendAsciiMessage("You cannot move that.");
		}
	}
}