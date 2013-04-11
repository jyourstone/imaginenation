using System;

using Server;
using Server.Commands.Generic;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Khazman.Commands
{
	public class ToStaticCommand : BaseCommand
	{
		public ToStaticCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.Single | CommandSupport.Area | CommandSupport.Multi;
			Commands = new string[]{ "ToStatic" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "ToStatic";
			Description = "Converts the targeted item(s) into the static equivalent(s).";
		}

		public static void Initialize()
		{
			TargetCommands.Register( new ToStaticCommand() );
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
						AddonComponent component = (AddonComponent)((addon.Components)[i]);
						Static newStatic = new Static( component.ItemID );
						
						newStatic.Hue = component.Hue;
						newStatic.MoveToWorld( new Point3D( component.Location ), component.Map );
					}
				}
				
				addon.Delete();
				
				AddResponse( "The add-on has been converted to static objects." );
			}
			else if( o is Item && !(o is Static) )
			{
				Item i = (Item)o;
				Static newItem = new Static( i.ItemID );

				newItem.Hue = i.Hue;
				newItem.Layer = i.Layer;
				newItem.Light = i.Light;
				newItem.MoveToWorld( new Point3D( i.Location ), i.Map );
				
				if( i.Parent == args.Mobile )
					newItem.Bounce( args.Mobile );

				if( i is Container )
					((Container)i).Destroy();
				else
					i.Delete();
				
				AddResponse( "The item has been converted to a static." );
			}
			else
			{
				LogFailure( "This command only works with non-static items or add-ons." );
			}
		}
	}
}
