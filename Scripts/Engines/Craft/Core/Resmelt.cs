using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.Engines.Craft
{
    public enum SmeltResult
    {
        Success,
        Invalid,
        NoSkill
    }

	public class Resmelt
	{
	    public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			int num = craftSystem.CanCraft( from, tool, null );

            if (num > 0 && num != 1044267)
                from.SendGump(new CraftGump(from, craftSystem, tool, num));
			else
			{
				from.Target = new InternalTarget( craftSystem, tool );
				from.SendLocalizedMessage( 1044273 ); // Target an item to recycle.
			}
		}

		private class InternalTarget : Target
		{
			private readonly CraftSystem m_CraftSystem;
			private readonly BaseTool m_Tool;

			public InternalTarget( CraftSystem craftSystem, BaseTool tool ) :  base ( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

            private SmeltResult Resmelt(Mobile from, Item item, CraftResource resource)
            {
				try
				{
					if ( CraftResources.GetType( resource ) != CraftResourceType.Metal )
                        return SmeltResult.Invalid;

					CraftResourceInfo info = CraftResources.GetInfo( resource );

					if ( info == null || info.ResourceTypes.Length == 0 )
                        return SmeltResult.Invalid;

					CraftItem craftItem = m_CraftSystem.CraftItems.SearchFor( item.GetType() );

					if ( craftItem == null || craftItem.Resources.Count == 0 )
                        return SmeltResult.Invalid;

					CraftRes craftResource = craftItem.Resources.GetAt( 0 );

					if ( craftResource.Amount < 2 )
                        return SmeltResult.Invalid; // Not enough metal to resmelt

                    double difficulty = 0.0;

                    switch (resource)
                    {
                        case CraftResource.DullCopper: difficulty = 65.0; break;
                        case CraftResource.ShadowIron: difficulty = 70.0; break;
                        case CraftResource.Copper: difficulty = 75.0; break;
                        case CraftResource.Bronze: difficulty = 80.0; break;
                        case CraftResource.Gold: difficulty = 85.0; break;
                        case CraftResource.Agapite: difficulty = 90.0; break;
                        case CraftResource.Verite: difficulty = 95.0; break;
                        case CraftResource.Valorite: difficulty = 99.0; break;
                    }

                    if (difficulty > from.Skills[SkillName.Mining].Value)
                        return SmeltResult.NoSkill;

					Type resourceType = info.ResourceTypes[0];
					Item ingot = (Item)Activator.CreateInstance( resourceType );

					if ( item is DragonBardingDeed || (item is BaseArmor && ((BaseArmor)item).PlayerConstructed) || (item is BaseWeapon && ((BaseWeapon)item).PlayerConstructed) || (item is BaseClothing && ((BaseClothing)item).PlayerConstructed) )
						ingot.Amount = craftResource.Amount / 2;
					else
						ingot.Amount = 1;

                    if (item.Amount > 1)
                        item.Amount--;
                    else
                        item.Delete();

                    #region Add to pack or ground if overweight
                    //Taran: Check to see if player is overweight. If they are and the item drops to the
                    //ground then a check is made to see if it can be stacked. If it can't and  more than 
                    //20 items of the same type exist in the same tile then the last item gets removed. This 
                    //check is made so thousands of items can't exist in 1 tile and crash people in the same area.
                    if (from.AddToBackpack(ingot))
                        from.SendAsciiMessage("You put the {0} in your pack.", ingot.Name ?? CliLoc.LocToString(ingot.LabelNumber));
                    else if (!ingot.Deleted)
                    {
                        IPooledEnumerable eable = from.Map.GetItemsInRange(from.Location, 0);
                        int amount = 0;
                        Item toRemove = null;

                        foreach (Item i in eable)
                        {
                            if (i != ingot && i.ItemID == ingot.ItemID)
                            {
                                if (i.StackWith(from, ingot, false))
                                {
                                    toRemove = ingot;
                                    break;
                                }
                                
                                amount++;
                            }
                        }

                        from.SendAsciiMessage("You are overweight and put the {0} on the ground.", ingot.Name ?? CliLoc.LocToString(ingot.LabelNumber));

                        if (toRemove != null)
                            toRemove.Delete();

                        else if (amount >= 5 && amount < 20)
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} identical items on the ground detected, no more than 20 is allowed!", amount));

                        else if (amount >= 20)
                        {
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, "Too many identical items on the ground, removing!");
                            ingot.Delete();
                        }

                        eable.Free();
                    }
                    #endregion

                    from.PlaySound( 0x2A );
					from.PlaySound( 0x240 );
                    return SmeltResult.Success;
                }
				catch
				{
				}

                return SmeltResult.Invalid;
            }

			protected override void OnTarget( Mobile from, object targeted )
			{
				int num = m_CraftSystem.CanCraft( from, m_Tool, null );

                if (num > 0)
                {
                    if (num == 1044267)
                    {
                        bool anvil, forge;

                        DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge);

                        if (!anvil)
                            num = 1044266; // You must be near an anvil
                        else if (!forge)
                            num = 1044265; // You must be near a forge.
                    }
                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, num));
                }
                else
                {
                    SmeltResult result = SmeltResult.Invalid;
                    bool isStoreBought = false;
                    int message;

                    if (targeted is BaseArmor)
                    {
                        result = Resmelt(from, (BaseArmor)targeted, ((BaseArmor)targeted).Resource);
                        isStoreBought = !((BaseArmor)targeted).PlayerConstructed;
                    }
                    else if (targeted is BaseWeapon)
                    {
                        result = Resmelt(from, (BaseWeapon)targeted, ((BaseWeapon)targeted).Resource);
                        isStoreBought = !((BaseWeapon)targeted).PlayerConstructed;
                    }
                    else if (targeted is DragonBardingDeed)
                    {
                        result = Resmelt(from, (DragonBardingDeed)targeted, ((DragonBardingDeed)targeted).Resource);
                        isStoreBought = false;
                    }

                    switch (result)
                    {
                        default:
                        case SmeltResult.Invalid: message = 1044272; break; // You can't melt that down into ingots.
                        case SmeltResult.NoSkill: message = 1044269; break; // You have no idea how to work this metal.
                        case SmeltResult.Success: message = isStoreBought ? 500418 : 1044270; break; // You melt the item down into ingots.
                    }

                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, message));
                }
			}
		}
	}
}