using Server.Engines.Craft;

namespace Server.Items
{
	public abstract class BaseReagent : Item
	{
        private const string notEnoughRegs = "You do not have enough reagents to make that potion.";
        private const string notSkilledEnough = "You do not have the required skills to create that potion.";

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

        public virtual int PotionGroupIndex { get { return -1; } }
        public virtual int PotionIndex { get { return -1; } }

		public BaseReagent( int itemID ) : this( itemID, 1 )
		{
		}

		public BaseReagent( int itemID, int amount ) : base( itemID )
		{
			Stackable = true;
			Amount = amount;
		}

		public BaseReagent( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (EventItem)
                return;

            if (PotionGroupIndex == -1)
            {
                from.SendAsciiMessage("You don't know how to use that.");
                return;
            }

            if (DefAlchemy.CraftSystem.CraftGroups.Count - 1 < PotionGroupIndex)
                return;

            if (from.Backpack.FindItemByType(typeof(Bottle)) == null)
            {
                from.SendAsciiMessage("You don't have any bottles for this potion.");
                return;
            }

            BaseTool toolToUse = null;
            foreach (BaseTool t in from.Backpack.FindItemsByType(typeof(BaseTool)))
            {
                if (t.CraftSystem == DefAlchemy.CraftSystem)
                {
                    toolToUse = t;
                    break;
                }
            }

            if (toolToUse == null)
            {
                from.SendAsciiMessage("You don't have a mortar and pestle.");
                return;
            }

            CraftItem cI = null;
            if (PotionIndex == -1)
            {
                CraftItemCol craftItemCol = DefAlchemy.CraftSystem.CraftGroups.GetAt(PotionGroupIndex).CraftItems;
                for (int i = craftItemCol.Count - 1; i >= 0; --i)
                {
                    CraftItem localCI = craftItemCol.GetAt(i);
                    if (localCI.Resources.GetAt(0).Amount <= Amount && localCI.Skills.GetAt(0).MinSkill <= from.Skills.Alchemy.Base)
                    {
                        cI = localCI;
                        break;
                    }

                    if (i == 0 && cI == null)
                    {
                        if (localCI.Skills.GetAt(0).MinSkill > from.Skills.Alchemy.Base)
                            from.SendAsciiMessage(notSkilledEnough);
                        else if (localCI.Resources.GetAt(0).Amount > Amount)
                            from.SendAsciiMessage(notEnoughRegs);
                    }
                }
            }
            else
            {
                if (DefAlchemy.CraftSystem.CraftGroups.Count - 1 >= PotionIndex &&
                    DefAlchemy.CraftSystem.CraftGroups.GetAt(PotionGroupIndex).CraftItems.Count - 1 >= PotionIndex)
                {
                    cI = DefAlchemy.CraftSystem.CraftGroups.GetAt(PotionGroupIndex).CraftItems.GetAt(PotionIndex);

                    if (cI.Skills.GetAt(0).MinSkill > from.Skills.Alchemy.Base)
                    {
                        from.SendAsciiMessage(notSkilledEnough);
                        cI = null;
                    }
                    else if (cI.Resources.GetAt(0).Amount > Amount)
                    {
                        from.SendAsciiMessage(notEnoughRegs);
                        cI = null;
                    }
                }
            }

            if (cI != null && from.Combatant == null)
            {
                //if (from is PlayerMobile)
                //    ((PlayerMobile)from).AbortCurrentPlayerAction();
                DefAlchemy.CraftSystem.CreateItem(from, cI.ItemType, cI.Resources.GetAt(0).ItemType, toolToUse, cI, false);
            }
            else if (from.Combatant != null)
                from.SendAsciiMessage("Can't do that while in combat.");

            base.OnDoubleClick(from);
        }

        public override void Consume()
        {
            if (EventItem)
            {
                Mobile owner = null;

                if (RootParent is Mobile)
                    owner = (Mobile)RootParent;
                else if (Parent is Mobile)
                    owner = (Mobile)Parent;
                else
                    return;

                if (!owner.IsInEvent && owner.AccessLevel == AccessLevel.Player)
                {
                    owner.SendAsciiMessage("Event item deleted.");
                    Delete();
                }
            }
            else
                base.Consume();
        }

        public override void Consume(int amount)
        {
            if (EventItem)
            {
                Mobile owner = null;

                if (RootParent is Mobile)
                    owner = (Mobile)RootParent;
                else if (Parent is Mobile)
                    owner = (Mobile)Parent;
                else
                    return;
                
                if ( !owner.IsInEvent && owner.AccessLevel == AccessLevel.Player)
                {
                    owner.SendAsciiMessage("Event item deleted.");
                    Delete();
                }
            }
            else
                base.Consume(amount);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}