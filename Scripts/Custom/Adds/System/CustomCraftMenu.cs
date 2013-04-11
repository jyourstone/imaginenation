using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Items;
using Server.Menus.ItemLists;
using Server.Network;
using Server.Regions;
using Solaris.CliLocHandler;

namespace Server.Custom.CraftMenu
{
    public class ResourceInfo
    {
        public int ItemID, Hue, ResourceIndex;

        public ResourceInfo(int itemID, int hue, int resourceIndex)
        {
            ItemID = itemID;
            Hue = hue;
            ResourceIndex = resourceIndex;
        }
    }

    public enum GumpLevel
    {
        SelectGroup,
        SelectResource,
        SelectItem
    }

    public class CustomCraftMenu : ItemListMenu
    {
        #region Statics

        //To find the resource you were using.
        private static Dictionary<CraftSubRes, ResourceInfo> resourceInfoList;
        //To find the group you were at.
        private static Dictionary<CraftItem, int> groupIndexList;
        //Craftsystems gumpOwner use
        public static CraftSystem[] CraftSystemList =
            new CraftSystem[]
                {
                    DefAlchemy.CraftSystem, DefBlacksmithy.CraftSystem, DefBowFletching.CraftSystem,
                    DefCarpentry.CraftSystem, DefCartography.CraftSystem, DefCooking.CraftSystem,
                    DefGlassblowing.CraftSystem, DefInscription.CraftSystem, DefMasonry.CraftSystem,
                    DefTailoring.CraftSystem, DefTinkering.CraftSystem
                };

        private static readonly int[] weaponIndexListBS = new int[] { 5, 6, 7, 8 };
        public static List<int> WeaponIndexListBS;

        #endregion

        #region Instace vars
        //Used gumpOwner acces all the memvers (so that we do not have gumpOwner copy them...)
        private readonly CustomCraftMenu m_CurrentCraftMenu;

        private readonly Dictionary<ItemListEntry, int> localGroupIndexList = new Dictionary<ItemListEntry,int>();
        private bool isFiltered = false;
        private CraftSystem m_CraftSystem;
        private GumpLevel m_GumpLevel;
        private Mobile m_To;
        private BaseTool m_Tool;
        private Type m_ResType;
        private int m_GroupIndex = -1, m_RepairIndex = -1, m_CustomResources = -1, m_SmeltIndex = -1, m_EnhanceIndex = -1, m_ResourceIndex=0;
        
        #endregion

        #region Get/Set

        public static Dictionary<CraftItem, int> GroupIndexList
        {
            get { return groupIndexList; }
            set { groupIndexList = value; }
        }

        public static Dictionary<CraftSubRes, ResourceInfo> ResourceInfoList
        {
            get { return resourceInfoList; }
            set { resourceInfoList = value; }
        }

        private Mobile GumpOwner
        {
            get { return m_CurrentCraftMenu.m_To; }
            set { m_CurrentCraftMenu.m_To = value; }
        }

        private CraftSystem CurrentCraftSystem
        {
            get { return m_CurrentCraftMenu.m_CraftSystem; }
            set { m_CurrentCraftMenu.m_CraftSystem = value; }
        }

        private GumpLevel SelectedGumpLevel
        {
            get { return m_CurrentCraftMenu.m_GumpLevel; }
            set { m_CurrentCraftMenu.m_GumpLevel = value; }
        }

        private BaseTool ToolUsed
        {
            get { return m_CurrentCraftMenu.m_Tool; }
            set { m_CurrentCraftMenu.m_Tool = value; }
        }

        private int SelectedGroupIndex
        {
            get { return m_CurrentCraftMenu.m_GroupIndex; }
            set { m_CurrentCraftMenu.m_GroupIndex = value; }
        }

        private int SelectedResourceIndex
        {
            get { return m_CurrentCraftMenu.m_ResourceIndex; }
            set { m_CurrentCraftMenu.m_ResourceIndex = value; }
        }

        private Type SelectedResType
        {
            get { return m_CurrentCraftMenu.m_ResType; }
            set { m_CurrentCraftMenu.m_ResType = value; }
        }

        #endregion

        public static void Initialize()
        {
            resourceInfoList = new Dictionary<CraftSubRes, ResourceInfo>();
            groupIndexList = new Dictionary<CraftItem, int>();
            WeaponIndexListBS = new List<int>();

            //Made so that it is easier gumpOwner check/customize
            for (int i = 0; i < weaponIndexListBS.Length; ++i)
                WeaponIndexListBS.Add(weaponIndexListBS[i]);

                //For each craftystem
                foreach (CraftSystem cs in CraftSystemList)
                {
                    Item resInstance;
                    CraftSubResCol res;
                    CraftSubRes subResource;

                    //Get all the group indexes
                    for (int i = 0; i < cs.CraftGroups.Count; i++)
                    {
                        CraftGroup craftGroup = cs.CraftGroups.GetAt(i);

                        for (int z = 0; z < craftGroup.CraftItems.Count; z++)
                        {
                            CraftItem toAdd = craftGroup.CraftItems.GetAt(z);
                            if (!groupIndexList.ContainsKey(toAdd))
                                groupIndexList.Add(toAdd, i);
                        }
                    }

                    if (cs.CraftSubRes.Init)
                    {
                        res = cs.CraftSubRes;
                        //For every resource
                        for (int i = 0; i < res.Count; ++i)
                        {
                            subResource = res.GetAt(i);
                            resInstance = (Item)Activator.CreateInstance(subResource.ItemType);

                            if (!resourceInfoList.ContainsKey(subResource))
                                resourceInfoList.Add(subResource, new ResourceInfo(resInstance.ItemID, resInstance.Hue, i));

                            resInstance.Delete();
                        }
                    }

                    //Using res 2
                    if (cs.CraftSubRes2.Init)
                    {
                        res = cs.CraftSubRes2;
                        //For every resource
                        for (int i = 0; i < res.Count; ++i)
                        {
                            subResource = res.GetAt(i);
                            resInstance = (Item)Activator.CreateInstance(subResource.ItemType);

                            if (!resourceInfoList.ContainsKey(subResource))
                                resourceInfoList.Add(subResource, new ResourceInfo(resInstance.ItemID, resInstance.Hue, i));

                            resInstance.Delete();
                        }
                    }
                }
        }

        public CustomCraftMenu(Mobile gumpOwner, CraftSystem craftSystem, BaseTool tool) : base(GetName(craftSystem), null)
        {
            m_CurrentCraftMenu = this;

            GumpOwner = gumpOwner;
            CurrentCraftSystem = craftSystem;
            SelectedGumpLevel = GumpLevel.SelectGroup;
            ToolUsed = tool;

            Entries = CreateGroupList();

            if (CurrentCraftSystem.CraftSubRes.Init && CurrentCraftSystem.CraftSubRes.Count >= 1)
                SelectedResType = CurrentCraftSystem.CraftSubRes.GetAt(0).ItemType;
        }

        public CustomCraftMenu(Mobile gumpOwner, CraftSystem craftSystem, BaseTool tool, int groupIndex, Type resType, int usedResourceIndex)
            : base(GetName(craftSystem), null)
        {
            m_CurrentCraftMenu = this;

            GumpOwner = gumpOwner;
            CurrentCraftSystem = craftSystem;
            ToolUsed = tool;
            SelectedGroupIndex = groupIndex;
            SelectedResType = resType;
            SelectedResourceIndex = usedResourceIndex;
            SelectedGumpLevel = GumpLevel.SelectItem;

            if (groupIndex == -1)
            {
                SelectedGumpLevel = GumpLevel.SelectGroup;
                Entries = CreateGroupList();
            }
            else
                Entries = CreateItemList();
        }

        public CustomCraftMenu(CustomCraftMenu ccm)  : base(GetName(ccm.CurrentCraftSystem), null)
        {
            m_CurrentCraftMenu = ccm;

            switch (SelectedGumpLevel)
            {
                case GumpLevel.SelectGroup:
                    Entries = CreateGroupList();
                    break;
                case GumpLevel.SelectResource:
                    Entries = CreateResList();
                    break;
                case GumpLevel.SelectItem:
                    Entries = CreateItemList();
                    break;
            }
        }

        public override void OnResponse(NetState state, int index)
        {
            switch (SelectedGumpLevel)
            {
                case GumpLevel.SelectGroup:
                    {
                        SpecialOptions();

                        //
                        if (isFiltered && localGroupIndexList.ContainsKey(Entries[index]))
                            index = localGroupIndexList[Entries[index]];

                        SelectedGroupIndex = index;
                        if (index == m_CustomResources)
                        {
                            SelectedGumpLevel = GumpLevel.SelectResource;
                            GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                        }
                        else if (index == m_RepairIndex)
                            Repair.Do(GumpOwner, CurrentCraftSystem, ToolUsed);
                        else if (index == m_SmeltIndex)
                            Resmelt.Do(GumpOwner, CurrentCraftSystem, ToolUsed);
                        else if (index == m_EnhanceIndex)
                            GumpOwner.SendAsciiMessage("Enhancing disabled.");
                        else
                        {
                            SelectedGumpLevel = GumpLevel.SelectItem;
                            GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                        }                     
                        break;
                    }
                case GumpLevel.SelectItem:
                    if (index == 0)//Go back
                    {
                        SelectedGumpLevel = GumpLevel.SelectGroup;
                        GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                    }
                    else
                        CreateItem(index-1);//Cause of the "back button"
                    break;
                case GumpLevel.SelectResource:
                    {
                        SelectedResourceIndex = (index);
                        SelectedResType = CurrentCraftSystem.CraftSubRes.GetAt(SelectedResourceIndex).ItemType;
                        SelectedGumpLevel = GumpLevel.SelectGroup;

                        GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                        break;
                    }
            }
        }

        public void SpecialOptions()
        {
            int totalOptions = Entries.Length - 1;
            int offset = 0;

            //Enhance index. 4th
            if (CurrentCraftSystem.CanEnhance)
            {
                m_EnhanceIndex = totalOptions - offset;
                offset++;
            }

            //Smelt index. 3rd
            if (CurrentCraftSystem.Resmelt)
            {
                m_SmeltIndex = totalOptions - offset;
                offset++;
            }

            //Repair Index. 2nd
            if (CurrentCraftSystem.Repair)
            {
                m_RepairIndex = totalOptions - offset;
                offset++;
            }

            //Resource index. 1st
            if (CurrentCraftSystem.CraftSubRes.Init && CurrentCraftSystem.CraftSubRes.Count >= 1)
                m_CustomResources = totalOptions - offset;
        }

        private static string GetName(CraftSystem craftSystem)
        {
            return string.Format("{0} menu ", craftSystem.MainSkill);
        }

        public void CreateItem(int index)
        {
            CraftGroup group = CurrentCraftSystem.CraftGroups.GetAt(SelectedGroupIndex);
            CraftItem item = group.CraftItems.GetAt(index);

            int num = CurrentCraftSystem.CanCraft(GumpOwner, ToolUsed, item.ItemType);

            if (num > 0)
            {
                SelectedGumpLevel = GumpLevel.SelectItem;
                GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
            }
            else
            {
                CustomRegion cR = GumpOwner.Region as CustomRegion;
                if (cR != null && GumpOwner.AccessLevel == AccessLevel.Player)
                {
                    if (ToolUsed.CraftSystem == DefAlchemy.CraftSystem && cR.Controller.IsRestrictedSkill(0))
                        return;
                    if ((ToolUsed.CraftSystem == DefBlacksmithy.CraftSystem) && cR.Controller.IsRestrictedSkill(7))
                        return;
                    if (ToolUsed.CraftSystem == DefBowFletching.CraftSystem && cR.Controller.IsRestrictedSkill(8))
                        return;
                    if (ToolUsed.CraftSystem == DefCarpentry.CraftSystem && cR.Controller.IsRestrictedSkill(11))
                        return;
                    if (ToolUsed.CraftSystem == DefCartography.CraftSystem && cR.Controller.IsRestrictedSkill(12))
                        return;
                    if (ToolUsed.CraftSystem == DefCooking.CraftSystem && cR.Controller.IsRestrictedSkill(13))
                        return;
                    if (ToolUsed.CraftSystem == DefTinkering.CraftSystem && cR.Controller.IsRestrictedSkill(37))
                        return;
                    if (ToolUsed.CraftSystem == DefTailoring.CraftSystem && cR.Controller.IsRestrictedSkill(34))
                        return;
                    if (ToolUsed.CraftSystem == DefInscription.CraftSystem && cR.Controller.IsRestrictedSkill(23))
                        return;
                }

                if (ToolUsed.CraftSystem == DefBlacksmithy.CraftSystem)
                {
                    if (GumpOwner.FindItemOnLayer(Layer.OneHanded) != ToolUsed)
                    {
                        SelectedGumpLevel = GumpLevel.SelectItem;
                        GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                        GumpOwner.SendAsciiMessage("You must have your smith hammer equipped to do this");
                    }
                    else
                        CurrentCraftSystem.CreateItem(GumpOwner, item.ItemType, SelectedResType, ToolUsed, item);

                }
                else
                {
                    if ( !ToolUsed.IsChildOf(GumpOwner.Backpack) )
                    {
                        SelectedGumpLevel = GumpLevel.SelectItem;
                        GumpOwner.SendMenu(new CustomCraftMenu(m_CurrentCraftMenu));
                        GumpOwner.SendAsciiMessage("You must have your tool in your backpack!");
                    }
                    else
                        CurrentCraftSystem.CreateItem(GumpOwner, item.ItemType, SelectedResType, ToolUsed, item);
                }
            }
        }

        public ItemListEntry[] CreateGroupList()
        {
            CraftGroupCol craftGroupCol = CurrentCraftSystem.CraftGroups;
            List<ItemListEntry> toReturn = new List<ItemListEntry>();

            int hue = 0;
            if (SelectedResourceIndex > 0)
                hue = ResourceInfoList[CurrentCraftSystem.CraftSubRes.GetAt(SelectedResourceIndex)].Hue;

            if (hue > 1)
                --hue; 

            for (int i = 0; i < craftGroupCol.Count; i++)
            {
                int itemID;
                CraftGroup craftGroup = craftGroupCol.GetAt(i);

                if (craftGroup.CraftItems.Count >= 1)
                    itemID = CraftItem.ItemIDOf(craftGroup.CraftItems.GetAt(0).ItemType);
                else
                    itemID = 5360; //Deed ID

                if (craftGroup.NameNumber > 0)
                {
                    ItemListEntry ile = new ItemListEntry(CliLoc.LocToString(craftGroup.NameNumber), itemID, hue);

                    if (!FilterIndex(i))
                        toReturn.Add(ile);

                    localGroupIndexList.Add(ile, i);
                }
                else
                {
                    ItemListEntry ile = new ItemListEntry(craftGroup.NameString, itemID, hue);

                    if (!FilterIndex(i))
                        toReturn.Add(ile);

                    localGroupIndexList.Add(ile, i);
                }
            }
            toReturn.AddRange(AddSpecialGroups());

            return toReturn.ToArray();
        }

        public bool FilterIndex(int index)
        {
            if (CurrentCraftSystem == DefBlacksmithy.CraftSystem)
            {
                //Remove items that are not supposed gumpOwner be colored.
                if (SelectedResType != null && SelectedResType != CurrentCraftSystem.CraftSubRes.GetAt(0).ItemType && WeaponIndexListBS.Contains(index))
                {
                    if (!isFiltered)
                        isFiltered = true;

                    return true;
                }
                return false;
            }
            return false;
        }

        public ItemListEntry[] AddSpecialGroups()
        {
            List<ItemListEntry> specialGroups = new List<ItemListEntry>();
            ItemListEntry entryToAdd;

            // If the system has more than one resource
            if (CurrentCraftSystem.CraftSubRes.Init && CurrentCraftSystem.CraftSubRes.Count >= 1)
            {
                if (CurrentCraftSystem != DefBlacksmithy.CraftSystem)
                {
                    ItemListEntry[] resources = CreateResList();

                    int i = 0;
                    if (resources.Length > 1)
                        i = 1;

                    entryToAdd = new ItemListEntry("other resources", resources[i].ItemID, resources[i].Hue);
                    specialGroups.Add(entryToAdd);
                }
                else
                {
                    entryToAdd = new ItemListEntry("Colored Armor", 5402);
                    specialGroups.Add(entryToAdd);
                }
            }

            //Repair
            if (CurrentCraftSystem.Repair)
            {
                int itemID = 5091;

                if (CurrentCraftSystem == DefTailoring.CraftSystem)
                    itemID = 5091;

                entryToAdd = new ItemListEntry("Repair", itemID);
                specialGroups.Add(entryToAdd);
                //localGroupIndexList.Add(entryToAdd, localGroupIndexList.Keys.Count);

            }

            //Smelt
            if (CurrentCraftSystem.Resmelt)
            {
                entryToAdd = new ItemListEntry("Re-smelt", 4027);
                specialGroups.Add(entryToAdd);
                //localGroupIndexList.Add(entryToAdd, localGroupIndexList.Keys.Count);
            }

            return specialGroups.ToArray();
        }

        public ItemListEntry[] CreateResList()
        {
            CraftSubResCol res = CurrentCraftSystem.CraftSubRes;
            List<ItemListEntry> toReturn = new List<ItemListEntry>();
            CraftSubRes subResource;

            if (CurrentCraftSystem.CraftSubRes.Init && CurrentCraftSystem.CraftSubRes.Count >= 1)
            {
                for (int i = 0; i < res.Count; ++i)
                {
                    subResource = res.GetAt(i);

                    int hue = ResourceInfoList[subResource].Hue;

                    if (hue > 1)
                        hue--;

                    if (subResource.NameNumber > 0)
                        toReturn.Add(new ItemListEntry(CliLoc.LocToString(subResource.NameNumber), ResourceInfoList[subResource].ItemID, hue));
                    else
                        toReturn.Add(new ItemListEntry(subResource.NameString, ResourceInfoList[subResource].ItemID, hue));
                }
            }

            if (CurrentCraftSystem.CraftSubRes2.Init)
            {
                res = CurrentCraftSystem.CraftSubRes2;
                for (int i = 0; i < res.Count; ++i)
                {
                    subResource = res.GetAt(i);

                    if (subResource.NameNumber > 0)
                        toReturn.Add(new ItemListEntry(CliLoc.LocToString(subResource.NameNumber), ResourceInfoList[subResource].ItemID, ResourceInfoList[subResource].Hue));
                    else
                        toReturn.Add(new ItemListEntry(subResource.NameString, ResourceInfoList[subResource].ItemID, ResourceInfoList[subResource].Hue));
                }
            }

            return toReturn.ToArray();
        }


        public ItemListEntry[] CreateItemList()
        {
            CraftGroupCol craftGroupCol = CurrentCraftSystem.CraftGroups;
            CraftGroup craftGroup = craftGroupCol.GetAt(SelectedGroupIndex);
            CraftItemCol craftItemCol = craftGroup.CraftItems;
            ItemListEntry[] toReturn = new ItemListEntry[craftItemCol.Count+1];
            toReturn[0] = new ItemListEntry("Previous menu", 4766);//Previous page

            int hue = 0;
            if (CurrentCraftSystem.CraftSubRes.Init && CurrentCraftSystem.CraftSubRes.Count >= 1 && SelectedResType != CurrentCraftSystem.CraftSubRes.GetAt(0).ItemType)
                hue = ResourceInfoList[CurrentCraftSystem.CraftSubRes.GetAt(SelectedResourceIndex)].Hue;

            if (hue > 1)
                --hue;

            string resourceList = string.Empty;
            for (int i = 0; i < craftItemCol.Count; ++i)
            {
                CraftItem craftItem = craftItemCol.GetAt(i);

                //Get the item id and resources required for the items
                int itemID;
                if (craftItemCol.Count >= 1)
                {
                    itemID = CraftItem.ItemIDOf(craftItemCol.GetAt(i).ItemType);

                    int amount = CurrentCraftSystem.CraftGroups.GetAt(SelectedGroupIndex).CraftItems.GetAt(i).Resources.GetAt(0).Amount;

                    if (SelectedResType == null)
                    {
                        if (!string.IsNullOrEmpty(CurrentCraftSystem.CraftGroups.GetAt(SelectedGroupIndex).CraftItems.GetAt(i).Resources.GetAt(0).NameString))
                            resourceList = string.Format(" [{0} {1}]", resourceList, amount);
                        else if (CurrentCraftSystem.CraftGroups.GetAt(SelectedGroupIndex).CraftItems.GetAt(i).Resources.GetAt(0).NameNumber > 0)
                            resourceList = string.Format(" [{0} {1}]", amount, CliLoc.LocToString(CurrentCraftSystem.CraftGroups.GetAt(SelectedGroupIndex).CraftItems.GetAt(i).Resources.GetAt(0).NameNumber));
                    }
                    else
                        resourceList = string.Format(" [{0} {1}]", amount, CraftResources.GetName(CraftResources.GetFromType(SelectedResType)));

                }
                else
                    itemID = 5360; //Deed ID


                //Becomes 0 if we use a string instead
                if (craftItem.NameNumber > 0)
                    toReturn[i + 1] = new ItemListEntry(CliLoc.LocToString(craftItem.NameNumber) + resourceList, itemID, hue);
                else
                    toReturn[i + 1] = new ItemListEntry(craftItem.NameString + resourceList, itemID, hue);
            }

            return toReturn;
        }

        public static CraftSubRes GetSubRes(CraftSystem cs, Type rt, CraftItem ci)
        {
            CraftSubResCol resCol;

            if (ci != null)
                resCol = (ci.UseSubRes2 ? cs.CraftSubRes2 : cs.CraftSubRes);
            else if (cs.CraftSubRes.Init)
                resCol = cs.CraftSubRes;
            else if (cs.CraftSubRes2.Init)
                resCol = cs.CraftSubRes2;
            else
                resCol = null;

            if (resCol != null)
                return resCol.SearchFor(rt);
            return null;
        }
    }
}
