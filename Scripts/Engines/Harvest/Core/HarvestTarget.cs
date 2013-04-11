using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Guilds;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Engines.Harvest
{
	public class HarvestTarget : Target
	{
		private readonly Item m_Tool;
		private readonly HarvestSystem m_System;

		public HarvestTarget( Item tool, HarvestSystem system ) : base( -1, true, TargetFlags.None )
		{
			m_Tool = tool;
			m_System = system;

			DisallowMultis = true;
		}

        //Taran: Check if region controller allows the skill
        public virtual bool CheckAllowed(Mobile from)
        {
            bool canUse = true;
            CustomRegion cR = from.Region as CustomRegion;

            if (cR == null)
                return true;

            if (m_System is Lumberjacking && cR.Controller.IsRestrictedSkill(44))
                canUse = false;

            if (m_System is Mining && cR.Controller.IsRestrictedSkill(45))
                canUse = false;

            if (m_System is Fishing && cR.Controller.IsRestrictedSkill(18))
                canUse = false;

            if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;

            if (!canUse)
                from.SendAsciiMessage("You cannot do this here");

            return canUse;
        }

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_System is Mining && targeted is StaticTarget )
			{
				int itemID = ((StaticTarget)targeted).ItemID;

				// grave
				if ( itemID == 0xED3 || itemID == 0xEDF || itemID == 0xEE0 || itemID == 0xEE1 || itemID == 0xEE2 || itemID == 0xEE8 )
				{
					PlayerMobile player = from as PlayerMobile;

					if ( player != null )
					{
						QuestSystem qs = player.Quest;

						if ( qs is WitchApprenticeQuest )
						{
							FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;

							if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.Bones )
							{
								player.SendLocalizedMessage( 1055037 ); // You finish your grim work, finding some of the specific bones listed in the Hag's recipe.
								obj.Complete();

								return;
							}
						}
					}
				}
			}

            if (m_System is Lumberjacking && targeted is IChopable)
                ((IChopable)targeted).OnChop(from);
            else if (m_System is Lumberjacking && targeted is ICarvable)
                ((ICarvable)targeted).Carve(from, m_Tool);
            else if (m_System is Lumberjacking && FurnitureAttribute.Check(targeted as Item))
                DestroyFurniture(from, (Item)targeted);
            else if (m_System is Mining && targeted is TreasureMap)
                ((TreasureMap)targeted).OnBeginDig(from);
            else if (m_System is Lumberjacking && targeted is GuildContainer) //Taran: Turn guildcontainer into deed
            {
                Guild guild = from.Guild as Guild;
                if (guild != null && !guild.Disbanded)
                {
                    GuildContainer gc = (GuildContainer)targeted;
                    Mobile leader = guild.Leader;

                    if (from.Guild.Id == gc.GuildID && from == leader)
                    {
                        Container cont = (Container)targeted;
                        Item[] found = cont.FindItemsByType(typeof(Item), true);

                        if (found.Length > 0)
                            from.SendAsciiMessage("The container must be empty before you can re-deed it");
                        else
                        {
                            cont.Delete();
                            from.AddToBackpack(new GuildContainerDeed());
                            from.SendAsciiMessage("You put the guild container deed in your backpack");
                        }
                    }
                    else
                        from.SendAsciiMessage("You must be the leader of the guild to do that");
                }
                else
                    from.SendAsciiMessage("You can't use an axe on that.");
            }

            #region Turn logs into boards
            else if (m_System is Lumberjacking && targeted is BaseLog) //Turn logs into boards
            {
                BaseLog log = (BaseLog)targeted;

                if (log.IsChildOf(from.Backpack))
                {
                    CraftResource cr = log.Resource;
                    int amount = log.Amount;

                    log.Delete();

                    switch (cr)
                    {
                        case CraftResource.RegularWood:
                            {
                                Board board = new Board(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.OakWood:
                            {
                                OakBoard board = new OakBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.AshWood:
                            {
                                AshBoard board = new AshBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.YewWood:
                            {
                                YewBoard board = new YewBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Heartwood:
                            {
                                HeartwoodBoard board = new HeartwoodBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Bloodwood:
                            {
                                BloodwoodBoard board = new BloodwoodBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Frostwood:
                            {
                                FrostwoodBoard board = new FrostwoodBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Mahoganywood:
                            {
                                MahoganyBoard board = new MahoganyBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Cedarwood:
                            {
                                CedarBoard board = new CedarBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Willowwood:
                            {
                                WillowBoard board = new WillowBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        case CraftResource.Mystwood:
                            {
                                MystWoodBoard board = new MystWoodBoard(amount);
                                from.AddToBackpack(board);
                                break;
                            }
                        default:
                            {
                                from.SendAsciiMessage("Error, no check for that log is made.");
                                break;
                            }
                    }
                    if (from.Mounted)
                        from.Animate(26, 5, 1, true, false, 0);
                    else
                        from.Animate(9, 5, 1, true, false, 0);
                    
                    from.PlaySound(573);
                    from.SendAsciiMessage("You chop the logs and turn them into boards");
                }
                else
                    from.SendAsciiMessage("That must be in your backpack before you can chop it");
            }
            #endregion
            else
            {
                if (!CheckAllowed(from))
                    return;
                m_System.StartHarvesting(from, m_Tool, targeted);
            }
		}

		private void DestroyFurniture( Mobile from, Item item )
		{
			if ( !from.InRange( item.GetWorldLocation(), 3 ) || !from.InLOS(item) )
			{
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}
		    if ( !item.IsChildOf( from.Backpack ) && !item.Movable )
		    {
		        from.SendLocalizedMessage( 500462 ); // You can't destroy that while it is here.
		        return;
		    }

		    from.SendLocalizedMessage( 500461 ); // You destroy the item.
			Effects.PlaySound( item.GetWorldLocation(), item.Map, 0x3B3 );

			if ( item is Container )
			{
				if ( item is TrapableContainer )
					(item as TrapableContainer).ExecuteTrap( from );

				((Container)item).Destroy();
			}
			else
			{
				item.Delete();
			}
		}
	}
}