//Rev 187

using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Solaris.CliLocHandler;

namespace Server.Engines.Craft
{
	public enum ConsumeType
	{
		All,
		Half,
		None
	}

	public interface ICraftable
	{
		int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue);
	}

	public class CraftItem : IAction
	{
		private static readonly Dictionary<Type, int> _itemIds = new Dictionary<Type, int>();

		private static readonly Type[] m_ColoredItemTable = new[]
		                                                        {
		                                                            /*typeof(BaseWeapon),*/ typeof(BaseArmor), typeof(BaseClothing), typeof(BaseJewel), typeof(DragonBardingDeed)
		                                                        };

		private static readonly Type[] m_ColoredResourceTable = new[]
		                                                            {
		                                                                typeof(BaseIngot), typeof(BaseOre), typeof(BaseLeather), typeof(BaseHides), typeof(UncutCloth), 
                                                                        typeof(Cloth) /*typeof(BaseGranite), typeof(BaseScales),*/, typeof(BaseLog), typeof(BaseBoard)
		                                                            };

		private static readonly int[] m_HeatSources = new[]{ 0x461, 0x48E, // Sandstone oven/fireplace
			0x92B, 0x96C, // Stone oven/fireplace
			0xDE3, 0xDE9, // Campfire
			0xFAC, 0xFAC, // Firepit
			0x184A, 0x184C, // Heating stand (left)
			0x184E, 0x1850, // Heating stand (right)
			0x398C, 0x399F, // Fire field
			0x2DDB, 0x2DDC, //Elven stove
            0x19AA, 0x19BB,	// Veteran Reward Brazier
            0x197A, 0x19A9, // Large Forge 
			0x0FB1, 0x0FB1, // Small Forge
			0x2DD8, 0x2DD8 // Elven Forge
		};

        private static readonly Type[] m_MarkableTable = new[] { typeof(BaseArmor), typeof(BaseWeapon), typeof(BaseClothing), typeof(BaseInstrument), typeof(DragonBardingDeed), typeof(BaseTool), typeof(BaseHarvestTool), typeof(FukiyaDarts), typeof(Shuriken), typeof(Spellbook), typeof(Runebook), typeof(BaseQuiver) };

		private static readonly int[] m_Mills = new[] { 0x1920, 0x1921, 0x1922, 0x1923, 0x1924, 0x1295, 0x1926, 0x1928, 0x192C, 0x192D, 0x192E, 0x129F, 0x1930, 0x1931, 0x1932, 0x1934 };

		private static readonly int[] m_Ovens = new[]{ 0x461, 0x46F, // Sandstone oven
			0x92B, 0x93F, // Stone oven
			0x2DDB, 0x2DDC //Elven stove
		};

	    private static readonly Type[][] m_TypesTable = new[]
	                                                        {
	                                                            new[] {typeof (Log), typeof (Board)},
	                                                            new[] {typeof (HeartwoodLog), typeof (HeartwoodBoard)},
	                                                            new[] {typeof (BloodwoodLog), typeof (BloodwoodBoard)},
	                                                            new[] {typeof (FrostwoodLog), typeof (FrostwoodBoard)},
	                                                            new[] {typeof (OakLog), typeof (OakBoard)},
	                                                            new[] {typeof (AshLog), typeof (AshBoard)},
	                                                            new[] {typeof (YewLog), typeof (YewBoard)},
	                                                            new[] {typeof (MahoganyLog), typeof (MahoganyBoard)},
	                                                            new[] {typeof (CedarLog), typeof (CedarBoard)},
	                                                            new[] {typeof (WillowLog), typeof (WillowBoard)},
	                                                            new[] {typeof (MystWoodLog), typeof (MystWoodBoard)},
	                                                            new[] {typeof (Leather), typeof (Hides)},
	                                                            new[] {typeof (SpinedLeather), typeof (SpinedHides)},
	                                                            new[] {typeof (HornedLeather), typeof (HornedHides)},
	                                                            new[] {typeof (BarbedLeather), typeof (BarbedHides)},
	                                                            new[] {typeof (BlankMap), typeof (BlankScroll)},
	                                                            new[] {typeof (Cloth), typeof (UncutCloth)},
	                                                            new[] {typeof (CheeseWheel), typeof (CheeseWedge)},
	                                                            new[] {typeof (Pumpkin), typeof (SmallPumpkin)},
	                                                            new[] {typeof (WoodenBowlOfPeas), typeof (PewterBowlOfPeas)}
	                                                        };

		private readonly CraftResCol m_arCraftRes;
		private readonly CraftSkillCol m_arCraftSkill;
		private bool m_ForceNonExceptional;
		private readonly int m_GroupNameNumber;
		private readonly string m_GroupNameString;

	    private readonly int m_NameNumber;
		private readonly string m_NameString;

		private bool m_NeedHeat;
		private bool m_NeedMill;
		private bool m_NeedOven;
		private Recipe m_Recipe;
	    private int m_ResAmount;
		private int m_ResHue;
	    private CraftSystem m_System;
		private readonly Type m_Type;

	    private bool m_UseSubRes2;

		public CraftItem(Type type, TextDefinition groupName, TextDefinition name)
		{
			m_arCraftRes = new CraftResCol();
			m_arCraftSkill = new CraftSkillCol();

			m_Type = type;

			m_GroupNameString = groupName;
			m_NameString = name;

			m_GroupNameNumber = groupName;
			m_NameNumber = name;
		}

		public bool ForceNonExceptional
		{
			get { return m_ForceNonExceptional; }
			set { m_ForceNonExceptional = value; }
		}

	    public Expansion RequiredExpansion { get; set; }

	    public Recipe Recipe
		{
			get { return m_Recipe; }
		}

	    public int Mana { get; set; }

	    public int Hits { get; set; }

	    public int Stam { get; set; }

	    public bool UseSubRes2
		{
			get { return m_UseSubRes2; }
			set { m_UseSubRes2 = value; }
		}

	    public bool UseAllRes { get; set; }

	    public bool NeedHeat
		{
			get { return m_NeedHeat; }
			set { m_NeedHeat = value; }
		}

		public bool NeedOven
		{
			get { return m_NeedOven; }
			set { m_NeedOven = value; }
		}

		public bool NeedMill
		{
			get { return m_NeedMill; }
			set { m_NeedMill = value; }
		}

		public Type ItemType
		{
			get { return m_Type; }
		}

		public string GroupNameString
		{
			get { return m_GroupNameString; }
		}

		public int GroupNameNumber
		{
			get { return m_GroupNameNumber; }
		}

		public string NameString
		{
			get { return m_NameString; }
		}

		public int NameNumber
		{
			get { return m_NameNumber; }
		}

        public CraftResCol Resources
        {
			get { return m_arCraftRes; }
		}

		public CraftSkillCol Skills
		{
			get { return m_arCraftSkill; }
		}

		public object GetCS()
		{
			return m_System;
		}

		public void AddRecipe(int id, CraftSystem system)
		{
			if (m_Recipe != null)
			{
				Console.WriteLine(@"Warning: Attempted add of recipe #{0} to the crafting of {1} in CraftSystem {2}.", id, m_Type.Name, system);
				return;
			}

			m_Recipe = new Recipe(id, system, this);
		}

		public static int ItemIDOf(Type type)
		{
			int itemId;

			if (!_itemIds.TryGetValue(type, out itemId))
			{
				if (type == typeof(FactionExplosionTrap))
					itemId = 14034;
				else if (type == typeof(FactionGasTrap))
					itemId = 4523;
				else if (type == typeof(FactionSawTrap))
					itemId = 4359;
				else if (type == typeof(FactionSpikeTrap))
					itemId = 4517;

				if (itemId == 0)
				{
					object[] attrs = type.GetCustomAttributes(typeof(CraftItemIDAttribute), false);

					if (attrs.Length > 0)
					{
						CraftItemIDAttribute craftItemID = (CraftItemIDAttribute)attrs[0];
						itemId = craftItemID.ItemID;
					}
				}

				if (itemId == 0)
				{
					Item item = null;

					try
					{
						item = Activator.CreateInstance(type) as Item;
					}
					catch
					{
					}

					if (item != null)
					{
						itemId = item.ItemID;
						item.Delete();
					}
				}

				_itemIds[type] = itemId;
			}

			return itemId;
		}

		public void AddRes(Type type, TextDefinition name, int amount)
		{
			AddRes(type, name, amount, "");
		}

		public void AddRes(Type type, TextDefinition name, int amount, TextDefinition message)
		{
			CraftRes craftRes = new CraftRes(type, name, amount, message);
			m_arCraftRes.Add(craftRes);
		}

		public void AddSkill(SkillName skillToMake, double minSkill, double maxSkill)
		{
			CraftSkill craftSkill = new CraftSkill(skillToMake, minSkill, maxSkill);
			m_arCraftSkill.Add(craftSkill);
		}

		public bool ConsumeAttributes(Mobile from, ref object message, bool consume)
		{
		    if (Hits > 0 && from.Hits < Hits)
			{
				message = "You lack the required hit points to make that.";
				return false;
			}
		    bool consumHits = consume;

		    if (Mana > 0 && from.Mana < Mana)
			{
				message = "You lack the required mana to make that.";
				return false;
			}
		    bool consumMana = consume;

		    if (Stam > 0 && from.Stam < Stam)
			{
				message = "You lack the required stamina to make that.";
				return false;
			}
		    bool consumStam = consume;

		    if (consumMana)
				from.Mana -= Mana;

			if (consumHits)
				from.Hits -= Hits;

			if (consumStam)
				from.Stam -= Stam;

			return true;
		}

		#region Tables
		#endregion

		public bool IsMarkable(Type type)
		{
			if (m_ForceNonExceptional) //Don't even display the stuff for marking if it can't ever be exceptional.
				return false;

			for (int i = 0; i < m_MarkableTable.Length; ++i)
				if (type == m_MarkableTable[i] || type.IsSubclassOf(m_MarkableTable[i]))
					return true;

			return false;
		}

		public bool RetainsColorFrom(CraftSystem system, Type type)
		{
            if (DefTailoring.IsNonColorable(m_Type))
            {
                return false;
            }

			if (system.RetainsColorFrom(this, type))
				return true;

		    bool inItemTable = false, inResourceTable = false;

			for (int i = 0; !inItemTable && i < m_ColoredItemTable.Length; ++i)
				inItemTable = (m_Type == m_ColoredItemTable[i] || m_Type.IsSubclassOf(m_ColoredItemTable[i]));

			for (int i = 0; inItemTable && !inResourceTable && i < m_ColoredResourceTable.Length; ++i)
				inResourceTable = (type == m_ColoredResourceTable[i] || type.IsSubclassOf(m_ColoredResourceTable[i]));

			return (inItemTable && inResourceTable);
		}

		public bool Find(Mobile from, int[] itemIDs)
		{
			Map map = from.Map;

			if (map == null)
				return false;

			IPooledEnumerable eable = map.GetItemsInRange(from.Location, 2);

			foreach (Item item in eable)
				if ((item.Z + 16) > from.Z && (from.Z + 16) > item.Z && Find(item.ItemID, itemIDs))
				{
					eable.Free();
					return true;
				}

			eable.Free();

			for (int x = -2; x <= 2; ++x)
				for (int y = -2; y <= 2; ++y)
				{
					int vx = from.X + x;
					int vy = from.Y + y;

                    StaticTile[] tiles = map.Tiles.GetStaticTiles(vx, vy, true);

					for (int i = 0; i < tiles.Length; ++i)
					{
						int z = tiles[i].Z;
                        int id = tiles[i].ID;

						if ((z + 16) > from.Z && (from.Z + 16) > z && Find(id, itemIDs))
							return true;
					}
				}

			return false;
		}

		public bool Find(int itemID, int[] itemIDs)
		{
			bool contains = false;

			for (int i = 0; !contains && i < itemIDs.Length; i += 2)
				contains = (itemID >= itemIDs[i] && itemID <= itemIDs[i + 1]);

			return contains;
		}

		public bool IsQuantityType(Type[][] types)
		{
			for (int i = 0; i < types.Length; ++i)
			{
				Type[] check = types[i];

				for (int j = 0; j < check.Length; ++j)
					if (typeof(IHasQuantity).IsAssignableFrom(check[j]))
						return true;
			}

			return false;
		}

		public int ConsumeQuantity(Container cont, Type[][] types, int[] amounts)
		{
			if (types.Length != amounts.Length)
				throw new ArgumentException();

			Item[][] items = new Item[types.Length][];
			int[] totals = new int[types.Length];

			for (int i = 0; i < types.Length; ++i)
			{
				items[i] = cont.FindItemsByType(types[i], true);

				for (int j = 0; j < items[i].Length; ++j)
				{
					IHasQuantity hq = items[i][j] as IHasQuantity;

					if (hq == null)
						totals[i] += items[i][j].Amount;
					else
					{
						if (hq is BaseBeverage && ((BaseBeverage)hq).Content != BeverageType.Water)
							continue;

						totals[i] += hq.Quantity;
					}
				}

				if (totals[i] < amounts[i])
					return i;
			}

			for (int i = 0; i < types.Length; ++i)
			{
				int need = amounts[i];

				for (int j = 0; j < items[i].Length; ++j)
				{
					Item item = items[i][j];
					IHasQuantity hq = item as IHasQuantity;

					if (hq == null)
					{
						int theirAmount = item.Amount;

						if (theirAmount < need)
						{
							item.Delete();
							need -= theirAmount;
						}
						else
						{
							item.Consume(need);
							break;
						}
					}
					else
					{
						if (hq is BaseBeverage && ((BaseBeverage)hq).Content != BeverageType.Water)
							continue;

						int theirAmount = hq.Quantity;

						if (theirAmount < need)
						{
							hq.Quantity -= theirAmount;
							need -= theirAmount;
						}
						else
						{
							hq.Quantity -= need;
							break;
						}
					}
				}
			}

			return -1;
		}

		public int GetQuantity(Container cont, Type[] types)
		{
			Item[] items = cont.FindItemsByType(types, true);

			int amount = 0;

			for (int i = 0; i < items.Length; ++i)
			{
				IHasQuantity hq = items[i] as IHasQuantity;

				if (hq == null)
					amount += items[i].Amount;
				else
				{
					if (hq is BaseBeverage && ((BaseBeverage)hq).Content != BeverageType.Water)
						continue;

					amount += hq.Quantity;
				}
			}

			return amount;
		}

		public bool ConsumeRes(Mobile from, Type typeRes, CraftSystem craftSystem, ref int resHue, ref int maxAmount, ConsumeType consumeType, ref object message)
		{
			return ConsumeRes(from, typeRes, craftSystem, ref resHue, ref maxAmount, consumeType, ref message, false);
		}

		public bool ConsumeRes(Mobile from, Type typeRes, CraftSystem craftSystem, ref int resHue, ref int maxAmount, ConsumeType consumeType, ref object message, bool isFailure)
		{
			Container ourPack = from.Backpack;

			if (ourPack == null)
				return false;

			if (m_NeedHeat && !Find(from, m_HeatSources))
			{
				message = 1044487; // You must be near a fire source to cook.
				return false;
			}

			if (m_NeedOven && !Find(from, m_Ovens))
			{
				message = 1044493; // You must be near an oven to bake that.
				return false;
			}

			if (m_NeedMill && !Find(from, m_Mills))
			{
				message = 1044491; // You must be near a flour mill to do that.
				return false;
			}

			Type[][] types = new Type[m_arCraftRes.Count][];
			int[] amounts = new int[m_arCraftRes.Count];

			maxAmount = int.MaxValue;

			CraftSubResCol resCol = (m_UseSubRes2 ? craftSystem.CraftSubRes2 : craftSystem.CraftSubRes);

		    CraftRes res;
		    for (int i = 0; i < types.Length; ++i)
			{
				CraftRes craftRes = m_arCraftRes.GetAt(i);
				Type baseType = craftRes.ItemType;

				// Resource Mutation
				if ((baseType == resCol.ResType) && (typeRes != null))
				{
					baseType = typeRes;

					CraftSubRes subResource = resCol.SearchFor(baseType);

					if (subResource != null && from.Skills[craftSystem.MainSkill].Base < subResource.RequiredSkill)
					{
						message = subResource.Message;
						return false;
					}
				}
				// ******************

				for (int j = 0; types[i] == null && j < m_TypesTable.Length; ++j)
					if (m_TypesTable[j][0] == baseType)
						types[i] = m_TypesTable[j];

				if (types[i] == null)
					types[i] = new[] { baseType };

				amounts[i] = craftRes.Amount;

				// For stackable items that can ben crafted more than one at a time
				if (UseAllRes)
				{
					int tempAmount = ourPack.GetAmount(types[i]);
					tempAmount /= amounts[i];
					if (tempAmount < maxAmount)
					{
						maxAmount = tempAmount;

						if (maxAmount == 0)
						{
							res = m_arCraftRes.GetAt(i);

							if (res.MessageNumber > 0)
								message = res.MessageNumber;
							else if (!String.IsNullOrEmpty(res.MessageString))
								message = res.MessageString;
							else
								message = 502925; // You don't have the resources required to make that item.

							return false;
						}
					}
				}
				// ****************************

				if (isFailure && !craftSystem.ConsumeOnFailure(from, types[i][0], this))
					amounts[i] = 0;
			}

			// We adjust the amount of each resource to consume the max posible
			if (UseAllRes)
				for (int i = 0; i < amounts.Length; ++i)
					amounts[i] *= maxAmount;
			else
				maxAmount = -1;

			Item consumeExtra = null;

			if (m_NameNumber == 1041267)
			{
				// Runebooks are a special case, they need a blank recall rune

				List<RecallRune> runes = ourPack.FindItemsByType<RecallRune>();

				for (int i = 0; i < runes.Count; ++i)
				{
					RecallRune rune = runes[i];

					if (rune != null && !rune.Marked)
					{
						consumeExtra = rune;
						break;
					}
				}

				if (consumeExtra == null)
				{
					message = 1044253; // You don't have the components needed to make that.
					return false;
				}
			}

			int index;

			// Consume ALL
			switch (consumeType)
			{
                case ConsumeType.All: // Consume ALL
			        m_ResHue = 0;
			        m_ResAmount = 0;
			        m_System = craftSystem;
			        index = IsQuantityType(types) ? ConsumeQuantity(ourPack, types, amounts) : ourPack.ConsumeTotalGrouped(types, amounts, true, OnResourceConsumed, CheckHueGrouping);
			        resHue = m_ResHue;
			        break;
                case ConsumeType.Half: // Consume Half ( for use all resource craft type )
			        for (int i = 0; i < amounts.Length; i++)
			        {
			            amounts[i] /= 2;

			            if (amounts[i] < 1)
			                amounts[i] = 1;
			        }
			        m_ResHue = 0;
			        m_ResAmount = 0;
			        m_System = craftSystem;
			        index = IsQuantityType(types) ? ConsumeQuantity(ourPack, types, amounts) : ourPack.ConsumeTotalGrouped(types, amounts, true, OnResourceConsumed, CheckHueGrouping);
			        resHue = m_ResHue;
			        break;
                default: // ConstumeType.None ( it's basicaly used to know if the crafter has enough resource before starting the process )
			        index = -1;
			        if (IsQuantityType(types))
			        {
			            for (int i = 0; i < types.Length; i++)
			                if (GetQuantity(ourPack, types[i]) < amounts[i])
			                {
			                    index = i;
			                    break;
			                }
			        }
			        else
			            for (int i = 0; i < types.Length; i++)
			                if (ourPack.GetBestGroupAmount(types[i], true, CheckHueGrouping) < amounts[i])
			                {
			                    index = i;
			                    break;
			                }
			        break;
			}

			if (index == -1)
			{
				if (consumeType != ConsumeType.None)
					if (consumeExtra != null)
						consumeExtra.Delete();

				return true;
			}
		    res = m_arCraftRes.GetAt(index);

		    if (res.MessageNumber > 0)
		        message = res.MessageNumber;
		    else if (!string.IsNullOrEmpty(res.MessageString))
		        message = res.MessageString;
		    else
		        message = 502925; // You don't have the resources required to make that item.

		    return false;
		}

		private void OnResourceConsumed(Item item, int amount)
		{
			if (!RetainsColorFrom(m_System, item.GetType()))
				return;

			if (amount >= m_ResAmount)
			{
				m_ResHue = item.Hue;
				m_ResAmount = amount;
			}
		}

		private static int CheckHueGrouping(Item a, Item b)
		{
			return b.Hue.CompareTo(a.Hue);
		}

		public double GetExceptionalChance(CraftSystem system, double chance, Mobile from)
		{
			if (m_ForceNonExceptional)
				return 0.0;

            double bonus = 0.0;

            if (from.Talisman is BaseTalisman)
            {
                BaseTalisman talisman = (BaseTalisman)from.Talisman;

                if (talisman.Skill == system.MainSkill)
                {
                    chance -= talisman.SuccessBonus / 100.0;
                    bonus = talisman.ExceptionalBonus / 100.0;
                }
            }

			switch (system.ECA)
			{
				//default:
                case CraftECA.CustomChance: chance = chance * 0.85 - 0.1; break;
				case CraftECA.ChanceMinusSixty: chance -= 0.6; break;
                case CraftECA.FiftyPercentChanceMinusTenPercent: chance = chance * 0.5 - 0.1; break;
				case CraftECA.ChanceMinusSixtyToFourtyFive:
					{
						double offset = 0.60 - ((from.Skills[system.MainSkill].Value - 95.0) * 0.03);

						if (offset < 0.45)
							offset = 0.45;
						else if (offset > 0.60)
							offset = 0.60;

						chance -= offset;
					    break;
					}
			}

            if (chance > 0)
                return chance + bonus;

            return chance;
		}

		public bool CheckSkills(Mobile from, Type typeRes, CraftSystem craftSystem, ref int quality, ref bool allRequiredSkills)
		{
			return CheckSkills(from, typeRes, craftSystem, ref quality, ref allRequiredSkills, true);
		}

		public bool CheckSkills(Mobile from, Type typeRes, CraftSystem craftSystem, ref int quality, ref bool allRequiredSkills, bool gainSkills)
		{
			double chance = GetSuccessChance(from, typeRes, craftSystem, gainSkills, ref allRequiredSkills);

			if (GetExceptionalChance(craftSystem, chance, from) > Utility.RandomDouble())
				quality = 2;

			return (chance > Utility.RandomDouble());
		}

		public double GetSuccessChance(Mobile from, Type typeRes, CraftSystem craftSystem, bool gainSkills, ref bool allRequiredSkills)
		{
			double minMainSkill = 0.0;
			double maxMainSkill = 0.0;
			double valMainSkill = 0.0;

			allRequiredSkills = true;

			for (int i = 0; i < m_arCraftSkill.Count; i++)
			{
				CraftSkill craftSkill = m_arCraftSkill.GetAt(i);

				double minSkill = craftSkill.MinSkill;
				double maxSkill = craftSkill.MaxSkill;
				double valSkill = from.Skills[craftSkill.SkillToMake].Value;

				if (valSkill < minSkill)
					allRequiredSkills = false;

				if (craftSkill.SkillToMake == craftSystem.MainSkill)
				{
					minMainSkill = minSkill;
					maxMainSkill = maxSkill;
					valMainSkill = valSkill;
				}

				if (gainSkills) // This is a passive check. Success chance is entirely dependant on the main skill
					from.CheckSkill(craftSkill.SkillToMake, minSkill, maxSkill);
			}

			double chance;

			if (allRequiredSkills)
				chance = craftSystem.GetChanceAtMin(this) + ((valMainSkill - minMainSkill) / (maxMainSkill - minMainSkill) * (1.0 - craftSystem.GetChanceAtMin(this)));
			else
				chance = 0.0;

            if (allRequiredSkills && from.Talisman is BaseTalisman)
            {
                BaseTalisman talisman = (BaseTalisman)from.Talisman;

                if (talisman.Skill == craftSystem.MainSkill)
                    chance += talisman.SuccessBonus / 100.0;
            }

			if (allRequiredSkills && valMainSkill == maxMainSkill)
				chance = 1.0;

			return chance;
		}

		public void Craft(Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool)
		{
			Craft(from, craftSystem, typeRes, tool, true);
		}

		public void Craft(Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool showGumps)
		{
			if (from is PlayerMobile && ((PlayerMobile)from).BeginPlayerAction(this, true))
			{
                if (RequiredExpansion == Expansion.None || (from.NetState != null && from.NetState.SupportsExpansion(RequiredExpansion)))
                {
                    bool allRequiredSkills = true;
                    double chance = GetSuccessChance(from, typeRes, craftSystem, false, ref allRequiredSkills);

                    if (allRequiredSkills && chance >= 0.0)
                    {
                        if (Recipe == null || ((PlayerMobile)from).HasRecipe(Recipe))
                        {
                            int badCraft = craftSystem.CanCraft(from, tool, m_Type);

                            if (badCraft <= 0)
                            {
                                int resHue = 0;
                                int maxAmount = 0;
                                object message = null;

                                if (ConsumeRes(from, typeRes, craftSystem, ref resHue, ref maxAmount, ConsumeType.None, ref message))
                                {
                                    message = null;

                                    if (ConsumeAttributes(from, ref message, false))
                                    {
                                        CraftContext context = craftSystem.GetContext(from);

                                        if (context != null)
                                            context.OnMade(this);

                                        int iMin = craftSystem.MinCraftEffect;
                                        int iMax = (craftSystem.MaxCraftEffect - iMin) + 1;
                                        int iRandom = Utility.Random(iMax);
                                        iRandom += iMin + 1;

                                        if (craftSystem == DefAlchemy.CraftSystem)
                                            iRandom = DefAlchemy.GetDelay(this);

                                        new InternalTimer(from, craftSystem, this, typeRes, tool, iRandom, false).Start();
                                    }
                                    else
                                    {
                                        ((PlayerMobile)from).EndPlayerAction();
                                        from.SendGump(new CraftGump(from, craftSystem, tool, message));
                                    }
                                }
                                else
                                {
                                    ((PlayerMobile)from).EndPlayerAction();
                                    from.SendGump(new CraftGump(from, craftSystem, tool, message));
                                }
                            }
                            else
                            {
                                ((PlayerMobile)from).EndPlayerAction();
                                from.SendGump(new CraftGump(from, craftSystem, tool, badCraft));
                            }
                        }
                        else
                        {
                            ((PlayerMobile)from).EndPlayerAction();
                            from.SendGump(new CraftGump(from, craftSystem, tool, 1072847)); // You must learn that recipe from a scroll.
                        }
                    }
                    else
                    {
                        ((PlayerMobile)from).EndPlayerAction();
                        from.SendGump(new CraftGump(from, craftSystem, tool, 1044153)); // You don't have the required skills to attempt this item.
                    }
                }
                else
                {
                    ((PlayerMobile)from).EndPlayerAction();
                    from.SendGump(new CraftGump(from, craftSystem, tool, RequiredExpansionMessage(RequiredExpansion))); //The {0} expansion is required to attempt this item.
                }
			}
			else
				from.SendLocalizedMessage(500119); // You must wait to perform another action
		}

		private static object RequiredExpansionMessage(Expansion expansion) //Eventually convert to TextDefinition, but that requires that we convert all the gumps to ues it too.  Not that it wouldn't be a bad idea.
		{
			switch (expansion)
			{
				case Expansion.SE:
					return 1063307; // The "Samurai Empire" expansion is required to attempt this item.
				case Expansion.ML:
					return 1072650; // The "Mondain's Legacy" expansion is required to attempt this item.
				default:
					return String.Format("The \"{0}\" expansion is required to attempt this item.", ExpansionInfo.GetInfo(expansion).Name);
			}
		}

		public void CompleteCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CustomCraft customCraft)
		{
			CompleteCraft(quality, makersMark, from, craftSystem, typeRes, tool, customCraft, true);
		}

		public void CompleteCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CustomCraft customCraft, bool showGumps)
		{
			int badCraft = craftSystem.CanCraft(from, tool, m_Type);

            if (badCraft > 0)
            {
                if (tool != null && !tool.Deleted && tool.UsesRemaining > 0)
                    from.SendGump(new CraftGump(from, craftSystem, tool, badCraft));
                else
                    from.SendLocalizedMessage(badCraft);

                return;
            }

		    if (tool != null && !tool.Deleted && tool.UsesRemaining > 0)
		    {
		        if (tool.CraftSystem == DefBlacksmithy.CraftSystem)
		        {
		            if (from.FindItemOnLayer(Layer.OneHanded) != tool)
		            {
		                from.SendGump(new CraftGump(from, craftSystem, tool, string.Format("You must have your smith hammer equipped!")));
		                return;
		            }
		        }
		        else
		        {
		            if (!tool.IsChildOf(from.Backpack))
		            {
		                from.SendGump(new CraftGump(from, craftSystem, tool, string.Format("You must have your tool in your backpack!")));
		                return;
		            }
		        }
		    }
		    else
		    {
		        from.SendLocalizedMessage(badCraft);
		        return;
		    }


		    object checkMessage = null;
			int checkResHue = 0, checkMaxAmount = 0;
			
			// Not enough resource to craft it
			if ( !ConsumeRes( from, typeRes, craftSystem, ref checkResHue, ref checkMaxAmount, ConsumeType.None, ref checkMessage ) )
			{
                if (!tool.Deleted && tool.UsesRemaining > 0)
                    from.SendGump(new CraftGump(from, craftSystem, tool, checkMessage));
                else if (checkMessage is int && (int)checkMessage > 0)
                    from.SendLocalizedMessage((int)checkMessage);
                else if (checkMessage is string)
                    from.SendMessage((string)checkMessage);

				return;
			}
			if (!ConsumeAttributes(from, ref checkMessage, false))
			{
				if (!tool.Deleted && tool.UsesRemaining > 0)
                    from.SendGump(new CraftGump(from, craftSystem, tool, checkMessage));
				else if (checkMessage is int && (int)checkMessage > 0)
					from.SendLocalizedMessage((int)checkMessage);
				else if (checkMessage is string)
					from.SendMessage((string)checkMessage);

				return;
			}

			bool toolBroken = false;

			int ignored = 1;
			int endquality = 1;

			bool allRequiredSkills = true;

			if (CheckSkills(from, typeRes, craftSystem, ref ignored, ref allRequiredSkills))
			{
				// Resource
				int resHue = 0;
				int maxAmount = 0;

				object message = null;

				// Not enough resource to craft it
				if (!ConsumeRes(from, typeRes, craftSystem, ref resHue, ref maxAmount, ConsumeType.All, ref message))
				{
					if (!tool.Deleted && tool.UsesRemaining > 0)
                        from.SendGump(new CraftGump(from, craftSystem, tool, message));
					else if (message is int && (int)message > 0)
						from.SendLocalizedMessage((int)message);
					else if (message is string)
						from.SendMessage((string)message);

					return;
				}
			    if (!ConsumeAttributes(from, ref message, true))
			    {
			        if (!tool.Deleted && tool.UsesRemaining > 0)
			            from.SendGump(new CraftGump(from, craftSystem, tool, message));
			        else if (message is int && (int)message > 0)
			            from.SendLocalizedMessage((int)message);
			        else if (message is string)
			            from.SendMessage((string)message);

			        return;
			    }

			    if (tool is WeaversSpool)
                    tool.UsesRemaining--;

				if (craftSystem is DefBlacksmithy)
				{
					AncientSmithyHammer hammer = from.FindItemOnLayer(Layer.OneHanded) as AncientSmithyHammer;
					if (hammer != null && hammer != tool)
					{
						hammer.UsesRemaining--;
						if (hammer.UsesRemaining < 1)
							hammer.Delete();
					}
				}

                if (tool.UsesRemaining < 1)
                    toolBroken = true;

                if (toolBroken)
                    tool.Delete();

				int num = 0;

				Item item;
				if (customCraft != null)
					item = customCraft.CompleteCraft(out num);
				else if (typeof(MapItem).IsAssignableFrom(ItemType) && from.Map != Map.Trammel && from.Map != Map.Felucca)
				{
					item = new IndecipherableMap();
					from.SendLocalizedMessage(1070800); // The map you create becomes mysteriously indecipherable.
				}
				else
					item = Activator.CreateInstance(ItemType) as Item;

				if (item != null && !item.Deleted)
				{
                    if (craftSystem is DefCarpentry)
                    {
                        if (RetainsColorFrom(m_System, item.GetType()))
                        {
                            if (typeRes == typeof (OakLog))
                                item.Name = string.Format("Oak {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof (AshLog))
                                item.Name = string.Format("Ash {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof (YewLog))
                                item.Name = string.Format("Yew {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof (HeartwoodLog))
                                item.Name = string.Format("Heartwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof (BloodwoodLog))
                                item.Name = string.Format("Bloodwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof (FrostwoodLog))
                                item.Name = string.Format("Frostwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof(MahoganyLog))
                                item.Name = string.Format("Mahoganywood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof(CedarLog))
                                item.Name = string.Format("Cedarwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof(WillowLog))
                                item.Name = string.Format("Willowwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                            else if (typeRes == typeof(MystWoodLog))
                                item.Name = string.Format("Mystwood {0}", !string.IsNullOrEmpty(item.Name) ? item.Name.ToLower() : CliLoc.LocToString(item.LabelNumber).ToLower());
                        }
                    }

					if (item is ICraftable)
					{
                        if (item.Hue == 0)
                            item.Hue = resHue;

						endquality = ((ICraftable)item).OnCraft(quality, makersMark, from, craftSystem, typeRes, tool, this, resHue);
					}
					else if (item.Hue == 0)
					{
						item.Hue = resHue;

					    //Flag all high quality non ICraftable with a makers mark in the string
						if (craftSystem.MarkOption && makersMark && quality == 2 && from.Skills[craftSystem.MainSkill].Base >= 100.0)
						{
							endquality = quality;

						    item.Name = string.Format("{0} crafted by {1}", !string.IsNullOrEmpty(item.Name) ? item.Name : CliLoc.LocToString(item.LabelNumber), from.Name);
						}
					}

					if (maxAmount > 0)
						if (!item.Stackable && item is IUsesRemaining)
							((IUsesRemaining)item).UsesRemaining *= maxAmount;
						else
							item.Amount = maxAmount;

                    #region Add to pack or ground if overweight
                    //Taran: Check to see if player is overweight. If they are and the item drops to the
                    //ground then a check is made to see if it can be stacked. If it can't and  more than 
                    //20 items of the same type exist in the same tile then the last item gets removed. This 
                    //check is made so thousands of items can't exist in 1 tile and crash people in the same area.
                    if (from.AddToBackpack(item))
                    {
                        if (item is SpellScroll)
                            from.SendAsciiMessage("You put the {0} scroll in your pack.", item.Name ?? CliLoc.LocToString(item.LabelNumber));
                        else
                            from.SendAsciiMessage("You put the {0} in your pack.", item.Name ?? CliLoc.LocToString(item.LabelNumber));
                    }
                    else if (!item.Deleted)
                    {
                        IPooledEnumerable eable = from.Map.GetItemsInRange(from.Location, 0);
                        int amount = 0;
                        Item toRemove = null;

                        foreach (Item i in eable)
                        {
                            if (i != item && i.ItemID == item.ItemID)
                            {
                                if (i.StackWith(from, item, false))
                                {
                                    toRemove = item;
                                    break;
                                }
                                
                                amount++;
                            }
                        }

                        from.SendAsciiMessage("You are overweight and put the {0} on the ground.", item.Name ?? CliLoc.LocToString(item.LabelNumber));

                        if (toRemove != null)
                            toRemove.Delete();

                        else if (amount >= 5 && amount < 20)
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} identical items on the ground detected, no more than 20 is allowed!", amount));

                        else if (amount >= 20)
                        {
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, "Too many identical items on the ground, removing!");
                            item.Delete();
                        }

                        eable.Free();
                    }
                    #endregion

                    if (from.AccessLevel > AccessLevel.Player)
						CommandLogging.WriteLine(from, "Crafting {0} with craft system {1}", CommandLogging.Format(item), craftSystem.GetType().Name);

				}

				if (num == 0)
					num = craftSystem.PlayEndingEffect(from, false, true, toolBroken, endquality, makersMark, this);

				bool queryFactionImbue = false;
				int availableSilver = 0;
				FactionItemDefinition def = null;
				Faction faction = null;

				if (item is IFactionItem)
				{
					def = FactionItemDefinition.Identify(item);

					if (def != null)
					{
						faction = Faction.Find(from);

						if (faction != null)
						{
							Town town = Town.FromRegion(from.Region);

							if (town != null && town.Owner == faction)
							{
								Container pack = from.Backpack;

								if (pack != null)
								{
									availableSilver = pack.GetAmount(typeof(Silver));

									if (availableSilver >= def.SilverCost)
										queryFactionImbue = Faction.IsNearType(from, def.VendorType, 12);
								}
							}
						}
					}
				}

				// TODO: Scroll imbuing

				if (queryFactionImbue)
					from.SendGump(new FactionImbueGump(quality, item, from, craftSystem, tool, num, availableSilver, faction, def));
				else if (!tool.Deleted && tool.UsesRemaining > 0)
					from.SendGump(new CraftGump(from, craftSystem, tool, num));
				else if (num > 0)
					from.SendLocalizedMessage(num);
			}
			else if (!allRequiredSkills)
				if (!tool.Deleted && tool.UsesRemaining > 0)
					from.SendGump(new CraftGump(from, craftSystem, tool, 1044153));
				else
					from.SendLocalizedMessage(1044153); // You don't have the required skills to attempt this item.
			else
			{
				ConsumeType consumeType = (UseAllRes ? ConsumeType.Half : ConsumeType.All);
				int resHue = 0;
				int maxAmount = 0;

				object message = null;

				// Not enough resource to craft it
				if (!ConsumeRes(from, typeRes, craftSystem, ref resHue, ref maxAmount, consumeType, ref message, true))
				{
					if (!tool.Deleted && tool.UsesRemaining > 0)
						from.SendGump(new CraftGump(from, craftSystem, tool, message));
					else if (message is int && (int)message > 0)
						from.SendLocalizedMessage((int)message);
					else if (message is string)
						from.SendMessage((string)message);

					return;
				}

				//tool.UsesRemaining--;

                if (tool is WeaversSpool)
                    tool.UsesRemaining--;

				if (tool.UsesRemaining < 1)
					toolBroken = true;

				if (toolBroken)
					tool.Delete();

				// SkillCheck failed.
				int num = craftSystem.PlayEndingEffect(from, true, true, toolBroken, endquality, false, this);

				if (!tool.Deleted && tool.UsesRemaining > 0)
                    from.SendGump(new CraftGump(from, craftSystem, tool, num));
				else if (num > 0)
					from.SendLocalizedMessage(num);
			}
		}

		#region Nested type: InternalTimer
		private class InternalTimer : Timer, IAction
		{
			private readonly CraftItem m_CraftItem;
			private readonly CraftSystem m_CraftSystem;
			private readonly Mobile m_From;
			private int m_iCount;
			private readonly int m_iCountMax;
			private readonly bool m_ShowGumps = true;
			private readonly BaseTool m_Tool;
			private readonly Type m_TypeRes;

			public InternalTimer(Mobile from, CraftSystem craftSystem, CraftItem craftItem, Type typeRes, BaseTool tool, int iCountMax, bool showGumps)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(craftSystem.Delay), iCountMax)
			{
				m_From = from;
				m_CraftItem = craftItem;
				m_iCount = 0;
				m_iCountMax = iCountMax;
				m_CraftSystem = craftSystem;
				m_TypeRes = typeRes;
				m_Tool = tool;
				m_ShowGumps = showGumps;

				if (from is PlayerMobile)
					((PlayerMobile)from).ResetPlayerAction(this);
			}

			protected override void OnTick()
			{
				m_iCount++;

				m_From.DisruptiveAction();

				if (m_iCount < m_iCountMax)
					if (m_CraftSystem == DefAlchemy.CraftSystem)
					{
						if (m_iCount % 2 != 0)
							((DefAlchemy)m_CraftSystem).PlayAlchEffect(m_From, m_CraftItem.Resources.GetAt(0).ItemType, m_iCount);
					}
					else
						m_CraftSystem.PlayCraftEffect(m_From);
				else
				{
					if (m_From is PlayerMobile)
						((PlayerMobile)m_From).EndPlayerAction();

					int badCraft = m_CraftSystem.CanCraft(m_From, m_Tool, m_CraftItem.m_Type);

					if (badCraft > 0)
					{
						if (m_Tool != null && !m_Tool.Deleted && m_Tool.UsesRemaining > 0)
						{
		                    m_From.SendGump(new CraftGump(m_From, m_CraftSystem, m_Tool, badCraft));

							if (m_CraftSystem == DefAlchemy.CraftSystem)
							{
								//m_From.PublicOverheadMessage(MessageType.Emote, 0x22, true, string.Format("*You see {0} toss the failed mixture*", m_From.Name));
								//For you
								m_From.LocalOverheadMessage(MessageType.Emote, 0x22, true, "*You toss the failed mixture*");

								//For everyone else
								m_From.NonlocalOverheadMessage(MessageType.Emote, 0x22, true,
															 string.Format("*You see {0} toss the failed mixture*", m_From.Name));
							}
							else //Display message
								m_From.SendAsciiMessage(CliLoc.LocToString(badCraft));
						}
						else
							m_From.SendLocalizedMessage(badCraft);

						return;
					}

					int quality = 1;
					bool allRequiredSkills = true;

					m_CraftItem.CheckSkills(m_From, m_TypeRes, m_CraftSystem, ref quality, ref allRequiredSkills, false);

					CraftContext context = m_CraftSystem.GetContext(m_From);

					if (context == null)
						return;

					if (typeof(CustomCraft).IsAssignableFrom(m_CraftItem.ItemType))
					{
						CustomCraft cc = null;

						try
						{
							cc = Activator.CreateInstance(m_CraftItem.ItemType, new object[] { m_From, m_CraftItem, m_CraftSystem, m_TypeRes, m_Tool, quality }) as CustomCraft;
						}
						catch
						{
						}

						if (cc != null)
							cc.EndCraftAction();

						return;
					}

					bool makersMark = false;

					if (quality == 2 && m_From.Skills[m_CraftSystem.MainSkill].Base >= 100.0)
						makersMark = m_CraftItem.IsMarkable(m_CraftItem.ItemType);

					if (makersMark && context.MarkOption == CraftMarkOption.PromptForMark)
						m_From.SendGump(new QueryMakersMarkGump(quality, m_From, m_CraftItem, m_CraftSystem, m_TypeRes, m_Tool));
					else
					{
						if (context.MarkOption == CraftMarkOption.DoNotMark)
							makersMark = false;

						m_CraftItem.CompleteCraft(quality, makersMark, m_From, m_CraftSystem, m_TypeRes, m_Tool, null, m_ShowGumps);
					}
				}
			}

			#region IAction Members

			public void AbortAction(Mobile from)
			{
				if (m_CraftSystem == DefAlchemy.CraftSystem)
					from.PublicOverheadMessage(MessageType.Emote, 0x22, true, string.Format("*You see {0} toss the failed mixture*", m_From.Name));
				else //Display message
					from.SendAsciiMessage("You failed to craft that item.");

				if (from is PlayerMobile)
					((PlayerMobile)from).EndPlayerAction();

				Stop();
			}

			#endregion
		}

		#endregion

		#region IAction Members

		public void AbortAction(Mobile from)
		{
		}

		#endregion
	}
}