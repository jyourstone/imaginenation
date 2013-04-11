using System;
using System.Collections;
using System.Reflection;
using Server.Commands;
using Server.Commands.Generic;
using Server.Network;
using Server.Prompts;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class SetListGump : Gump
    {
        //private readonly PropertyInfo m_Property;
        private readonly Mobile m_Mobile;
        private readonly ArrayList m_ObjectList;
        private readonly Stack m_Stack;
        //private readonly Type m_Type;
        private int m_Page;
        private readonly ArrayList m_List;
        private object m_Object;

        public static readonly bool OldStyle = PropsConfig.OldStyle;

        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;

        public static readonly int TextHue = PropsConfig.TextHue;
        public static readonly int TextOffsetX = PropsConfig.TextOffsetX;

        public static readonly int OffsetGumpID = PropsConfig.OffsetGumpID;
        public static readonly int HeaderGumpID = PropsConfig.HeaderGumpID;
        public static readonly int EntryGumpID = PropsConfig.EntryGumpID;
        public static readonly int BackGumpID = PropsConfig.BackGumpID;
        public static readonly int SetGumpID = PropsConfig.SetGumpID;

        public static readonly int SetWidth = PropsConfig.SetWidth;
        public static readonly int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
        public static readonly int SetButtonID1 = PropsConfig.SetButtonID1;
        public static readonly int SetButtonID2 = PropsConfig.SetButtonID2;

        public static readonly int PrevWidth = PropsConfig.PrevWidth;
        public static readonly int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
        public static readonly int PrevButtonID1 = PropsConfig.PrevButtonID1;
        public static readonly int PrevButtonID2 = PropsConfig.PrevButtonID2;

        public static readonly int NextWidth = PropsConfig.NextWidth;
        public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
        public static readonly int NextButtonID1 = PropsConfig.NextButtonID1;
        public static readonly int NextButtonID2 = PropsConfig.NextButtonID2;

        public static readonly int OffsetSize = PropsConfig.OffsetSize;

        public static readonly int EntryHeight = PropsConfig.EntryHeight;
        public static readonly int BorderSize = PropsConfig.BorderSize;

        private static readonly bool PrevLabel = OldStyle;
        private static readonly bool NextLabel = OldStyle;

        private static readonly int PrevLabelOffsetX = PrevWidth + 1;
        private static readonly int PrevLabelOffsetY = 0;

        private static readonly int NextLabelOffsetX = -29;
        private static readonly int NextLabelOffsetY = 0;

        private static readonly int NameWidth = 107;
        private static readonly int ValueWidth = 128;

        private static readonly int EntryCount = 15;

        private static readonly int TypeWidth = NameWidth + OffsetSize + ValueWidth;

        private static readonly int TotalWidth = OffsetSize + NameWidth + OffsetSize + ValueWidth + OffsetSize + SetWidth + OffsetSize;
        private static readonly int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

        private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
        private static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;

        public SetListGump(Mobile mobile, ArrayList list, object obj, Stack stack, int page, ArrayList lst)
            : base(GumpOffsetX, GumpOffsetY)
        {
            m_Mobile = mobile;
            m_List = new ArrayList(list);

            m_Object = obj;
            m_Stack = stack;
            m_Page = page;
            m_ObjectList = lst;

            Initialize(0);
        }

		public SetListGump( Mobile mobile, Stack stack, ArrayList list, int page ) : base( GumpOffsetX, GumpOffsetY )
        {
			m_Mobile = mobile;
			m_List = list;
			m_Stack = stack;

			Initialize( page );
		}

        public void Initialize(int page)
        {
            m_Page = page;

            int count = m_List.Count - (page * EntryCount);

            if (count < 0)
                count = 0;
            else if (count > EntryCount)
                count = EntryCount;

            int lastIndex = (page * EntryCount) + count - 1;

            if (lastIndex >= 0 && lastIndex < m_List.Count && m_List[lastIndex] == null)
                --count;

            int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

            AddPage(0);

            AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
            AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID);

            int x = BorderSize + OffsetSize;
            int y = BorderSize + OffsetSize;

            int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) - (OldStyle ? SetWidth + OffsetSize : 0);

            if (OldStyle)
                AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
            else
                AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

            if (page > 0)
            {
                AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

                if (PrevLabel)
                    AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
            }

            x += PrevWidth + OffsetSize;

            if (!OldStyle)
                AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, HeaderGumpID);

            x += emptyWidth + OffsetSize;

            if (!OldStyle)
                AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

            if ((page + 1) * EntryCount < m_List.Count)
            {
                AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

                if (NextLabel)
                    AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
            }

            for (int i = 0, index = page * EntryCount; i < count && index < m_List.Count; ++i, ++index)
            {
                x = BorderSize + OffsetSize;
                y += EntryHeight + OffsetSize;

                object o = m_List[index];

                if (o == null)
                {
                    AddImageTiled(x - OffsetSize, y, TotalWidth, EntryHeight, BackGumpID + 4);
                }
                else
                {
                    AddImageTiled(x, y, TypeWidth, EntryHeight, EntryGumpID);
                    AddLabelCropped(x + TextOffsetX, y, TypeWidth - TextOffsetX, EntryHeight, TextHue, o.ToString());
                    x += TypeWidth + OffsetSize;

                    if (SetGumpID != 0)
                        AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

                    AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 3, GumpButtonType.Reply, 0);
                }
            }
        }

        private class InternalPrompt : Prompt
        {
            private readonly PropertyInfo m_Property;
            private readonly Mobile m_Mobile;
            private readonly object m_Object;
            private readonly Stack m_Stack;
            private readonly Type m_Type;
            private readonly int m_Page;
            private readonly ArrayList m_List;

            public InternalPrompt(PropertyInfo prop, Mobile mobile, object o, Stack stack, Type type, int page, ArrayList list)
            {
                m_Property = prop;
                m_Mobile = mobile;
                m_Object = o;
                m_Stack = stack;
                m_Type = type;
                m_Page = page;
                m_List = list;
            }

            public override void OnCancel(Mobile from)
            {
                m_Mobile.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
            }

            public override void OnResponse(Mobile from, string text)
            {
                object toSet;
                bool shouldSet;

                try
                {
                    int serial = Utility.ToInt32(text);

                    toSet = World.FindEntity(serial);

                    if (toSet == null)
                    {
                        shouldSet = false;
                        m_Mobile.SendMessage("No object with that serial was found.");
                    }
                    else if (!m_Type.IsAssignableFrom(toSet.GetType()))
                    {
                        toSet = null;
                        shouldSet = false;
                        m_Mobile.SendMessage("The object with that serial could not be assigned to a property of type : {0}", m_Type.Name);
                    }
                    else
                    {
                        shouldSet = true;
                    }
                }
                catch
                {
                    toSet = null;
                    shouldSet = false;
                    m_Mobile.SendMessage("Bad format");
                }

                if (shouldSet)
                {
                    try
                    {
                        CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, toSet == null ? "(null)" : toSet.ToString());
                        m_Property.SetValue(m_Object, toSet, null);
                        PropertiesGump.OnValueChanged(m_Object, m_Property, m_Stack);
                    }
                    catch
                    {
                        m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                m_Mobile.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
            }
        }



        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (!BaseCommand.IsAccessible(from, m_List))
            {
                from.SendMessage("You may no longer access their properties.");
                return;
            }

            switch (info.ButtonID)
            {
                case 1: // Previous
                    {
                        if (m_Page > 0)
                            from.SendGump(new SetListGump(from, m_Stack, m_List, m_Page - 1));

                        break;
                    }
                case 2: // Next
                    {
                        if ((m_Page + 1) * EntryCount < m_List.Count)
                            from.SendGump(new SetListGump(from, m_Stack, m_List, m_Page + 1));

                        break;
                    }
                default:
                    {
                        int index = (m_Page * EntryCount) + (info.ButtonID - 3);

                        if (index >= 0 && index < m_List.Count)
                        {
                            from.SendGump(new PropertiesGump(from, m_List[index], m_Stack, null));
                            PropertyInfo prop = m_List[index] as PropertyInfo;
                        }
                        break;
                    }
            }
        }
    }
}