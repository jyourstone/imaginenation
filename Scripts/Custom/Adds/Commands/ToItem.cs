using Server;
using Server.Commands.Generic;
using Server.Items;

namespace Khazman.Commands
{
	public class ToItemCommand : BaseCommand
	{
		public ToItemCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.Single | CommandSupport.Area | CommandSupport.Multi;
			Commands = new[]{ "ToItem" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "ToItem";
			Description = "Converts the targeted static(s) into the item equivalent(s).";
		}

		public static void Initialize()
		{
			TargetCommands.Register( new ToItemCommand() );
		}
		
		public override void Execute( Server.Commands.CommandEventArgs args, object o )
		{
			if( o is AddonComponent )
			{
				BaseAddon addon = ((AddonComponent)o).Addon;
				
				if( addon.Components.Count > 0 )
				{
					for( int i = 0; i < addon.Components.Count; i++ )
					{
						AddonComponent component = (addon.Components)[i];
                        Item newItem = new Item( component.ItemID) {Hue = component.Hue, Name = component.Name};

					    newItem.MoveToWorld( new Point3D( component.Location ), component.Map );
					}
				}
				
				addon.Delete();
				
				AddResponse( "The add-on has been converted to an item." );
			}
			else if( o is Static )
			{
				Static s = (Static)o;
				Item newItem = new Item( s.ItemID ) {Hue = s.Hue, Layer = s.Layer, Light = s.Light, Name = s.Name};

			    newItem.MoveToWorld( new Point3D( s.Location ), s.Map );
				
				if( s.Parent == args.Mobile )
					newItem.Bounce( args.Mobile );

				s.Delete();
				
				AddResponse( "The static has been converted to an item." );
			}
			else
			{
				LogFailure( "This command only works with static items or add-ons." );
			}
		}
	}
}
