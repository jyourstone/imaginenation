/***************************************************************************
 *                             INXHairStylist.cs
 *                            -------------------
 *   last edited          : August 24, 2007
 *   web site             : www.in-x.org
 *   author               : Makaveli
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   Created by the "Imagine Nation: Xtreme" dev team for "IN:X" and the RunUo
 *   community. If you miss the old school Sphere 0.51 gameplay, and want to
 *   try it on the best and most stable emulator, visit us at www.in-x.org.
 *      
 *   Imagine Nation: Xtreme
 *   A full sphere 0.51 replica.
 * 
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Commands;
using Server.Gumps;
using Server.HuePickers;
using Server.Network;
using Server.Targeting;
using PC = Server.Gumps.PropsConfig;

namespace Server.INXHairStylist
{
    public class ConfigSelectionGump : Gump
    {
        public enum GumpChoice
        {
            ViewPaperDoll = 1,
            ConfigSettings,
        }

        private readonly Mobile m_From;
        private readonly INXHairStylist m_HairStylist ;

		public ConfigSelectionGump(Mobile from, INXHairStylist hairStylist) : base( 180, 180 )
		{
            m_From = from;
            m_HairStylist  = hairStylist;

			AddPage(0);

			AddImage(105, 131, 3507);
			AddImage(105, 105, 3501);

            AddLabel(135, 121, 0, "Paperdoll");
            AddButton(115, 126, 2362, 2360, (int)GumpChoice.ViewPaperDoll, GumpButtonType.Reply, 0); // button for the paperdoll
			
            AddLabel(270, 121, 0, "Config");
            AddButton(324, 126, 2362, 2360, (int)GumpChoice.ConfigSettings, GumpButtonType.Reply, 0); // button for the config

			AddImage(80, 105, 3500);
			AddImage(346, 105, 3502);
			AddImage(79, 131, 3506);
			AddImage(346, 131, 3508);
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)GumpChoice.ViewPaperDoll)
                m_HairStylist .DisplayPaperdollTo(m_From);
            else if (info.ButtonID == (int)GumpChoice.ConfigSettings)
                m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist ));
        }
	}

    public class HairStylistConfigGump : Gump
    {
        public enum GumpChoice
        {
            SelectPage = 100,
            EditSettings = 300
        }

        private readonly INXHairStylist m_HairStylist ;
        private readonly Mobile m_From;
        private readonly PropertyInfo[] m_PropertyList;
        private static int m_OptionsPerPage = 4;
        private readonly int m_Page = 0;

        private static Type typeofMobile = typeof(Mobile);
        private static Type typeofItem = typeof(Item);
        private static Type typeofType = typeof(Type);
        private static readonly Type typeofPoint3D = typeof(Point3D);
        private static Type typeofPoint2D = typeof(Point2D);
        private static readonly Type typeofTimeSpan = typeof(TimeSpan);
        private static readonly Type typeofCustomEnum = typeof(CustomEnumAttribute);
        private static readonly Type typeofEnum = typeof(Enum);
        private static readonly Type typeofBool = typeof(Boolean);
        private static readonly Type typeofString = typeof(String);
        private static Type typeofPoison = typeof(Poison);
        private static Type typeofMap = typeof(Map);
        private static Type typeofSkills = typeof(Skills);
        private static Type typeofPropertyObject = typeof(PropertyObjectAttribute);
        private static Type typeofNoSort = typeof(NoSortAttribute);

        private static readonly Type[] typeofNumeric = new Type[]
			{
				typeof( Byte ),
				typeof( Int16 ),
				typeof( Int32 ),
				typeof( Int64 ),
				typeof( SByte ),
				typeof( UInt16 ),
				typeof( UInt32 ),
				typeof( UInt64 )
			};

        private static readonly Type[] typeofReal = new Type[]
			{
				typeof( Single ),
				typeof( Double )
			};

        public HairStylistConfigGump(Mobile from, INXHairStylist hairStylist)
            : base(85, 40)
        {
            m_From = from;
            m_HairStylist  = hairStylist;
            m_PropertyList = GetPropertyInfoList();

            InitializePage(0);
        }

        public HairStylistConfigGump(Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page)
            : base(85, 40)
        {
            m_From = from;
            m_HairStylist  = hairStylist;
            m_PropertyList = propertyList;
            m_Page = page;

            InitializePage(m_Page);
        }

        private void InitializePage(int pageNumber)
        {
            //The background
            AddBackground(60, 95, 515, 330, 5054);

            //Adds see through
            AddAlphaRegion(60, 95, 515, 330);

            int startIndex = (pageNumber * m_OptionsPerPage) > m_PropertyList.Length ? m_PropertyList.Length : (pageNumber * m_OptionsPerPage);
            int endIndex = (startIndex + m_OptionsPerPage) > m_PropertyList.Length ? m_PropertyList.Length : (startIndex + m_OptionsPerPage);

            for (int i = startIndex; i < endIndex; ++i)
            {
                PropertyInfo pI = m_PropertyList[i];
                DescriptionAttribute dA = GetDescriptionAttribute(pI);
                CommandPropertyAttribute cpa = GetCPA(pI);

                //Field name
                AddLabelCropped(437, 120 + (((endIndex - 1) - i) * 70), 128, 20, 0x384, pI.Name);

                //Prop value
                AddImageTiled(435, 140 + (((endIndex - 1) - i) * 70), 110, 20, 0x0BBC);
                AddLabelCropped(437, 140 + (((endIndex - 1) - i) * 70), 108, 20, 0, PropertiesGump.ValueToString(m_HairStylist ,pI));

                //Edit button
                if (pI.CanWrite && cpa != null && m_From.AccessLevel >= cpa.WriteLevel)
                    AddButton(550, 142 + (((endIndex - 1) - i) * 70), 0x15E1, 0x15E5, (int)GumpChoice.EditSettings + i, GumpButtonType.Reply, 0);

                //Variabel description
                AddHtml(70, 102 + (((endIndex - 1) - i) * 70), 360, 60, dA.Description, true, false);
                //Delimiter image
                AddImageTiled(70, 165 + (((endIndex - 1) - i) * 70), 497, 2, 96);
            }

            //Pagination
            for (int i = 0; i <= (m_PropertyList.Length / m_OptionsPerPage); i++)
            {
                //Highlight current page
                if (i == pageNumber)
                    AddImageTiled(67 + (i * 25), 387, 25, 25, 0xBBC);

                AddButton(70 + (i * 25), 390, 0x8B1 + i, 0x8B1 + i, (int)GumpChoice.SelectPage + i, GumpButtonType.Reply, 0);
            }           
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID >= (int)GumpChoice.SelectPage && info.ButtonID <= (int)GumpChoice.SelectPage + (m_PropertyList.Length / m_OptionsPerPage))
                from.SendGump(new HairStylistConfigGump(m_From,m_HairStylist , m_PropertyList, info.ButtonID - (int)GumpChoice.SelectPage));
            else if (info.ButtonID >= (int)GumpChoice.EditSettings && info.ButtonID <= (int)GumpChoice.EditSettings + m_PropertyList.Length)
            {
                int index = info.ButtonID - (int)GumpChoice.EditSettings;

                PropertyInfo prop = m_PropertyList[index];

                if (prop == null)
                    return;

                CommandPropertyAttribute cpa = GetCPA(prop);
                if (!prop.CanWrite || ( cpa != null && from.AccessLevel < cpa.WriteLevel))
                    return;

                Type type = prop.PropertyType;

                if (IsType(type, typeofPoint3D))
                    from.SendGump(new HairStylistSetPoint3DGump(prop, from, m_HairStylist , m_PropertyList, m_Page));
                
                else if (IsType(type, typeofTimeSpan))
                    from.SendGump(new HairStylistSetTimeSpanGump(prop, from, m_HairStylist , m_PropertyList,m_Page));
                else if (IsCustomEnum(type))
                    from.SendGump(new HairStylistSetCustomEnumGump(prop, from, m_HairStylist , m_PropertyList, GetCustomEnumNames(type), m_Page));
                else if (IsType(type, typeofEnum))
                    from.SendGump(new HairStylistSetListOptionGump(prop, from, m_HairStylist , m_PropertyList, Enum.GetNames(type), GetObjects(Enum.GetValues(type)), m_Page));
                else if (IsType(type, typeofBool))
                    from.SendGump(new HairStylistSetListOptionGump(prop, from, m_HairStylist , m_PropertyList, PropertiesGump.m_BoolNames, PropertiesGump.m_BoolValues, m_Page));
                else if (IsType(type, typeofString) || IsType(type, typeofReal) || IsType(type, typeofNumeric))
                    from.SendGump(new HairStylistSetGump(prop, from, m_HairStylist , m_PropertyList, m_Page));
            }
        }

        public PropertyInfo[] GetPropertyInfoList()
        {
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();

            PropertyInfo[] props = m_HairStylist .GetType().GetProperties();
            foreach (PropertyInfo pi in props)
                if (pi.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0)
                    propertyInfoList.Add(pi);

            return propertyInfoList.ToArray();
        }

        public DescriptionAttribute GetDescriptionAttribute(PropertyInfo pI)
        {
            if (pI.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0)
                return (DescriptionAttribute)pI.GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
            else
                return new DescriptionAttribute("Description missing.");
        }

        private static bool IsType(Type type, Type check)
        {
            return type == check || type.IsSubclassOf(check);
        }

        private static bool IsType(Type type, Type[] check)
        {
            for (int i = 0; i < check.Length; ++i)
                if (IsType(type, check[i]))
                    return true;

            return false;
        }

        private static string[] GetCustomEnumNames(Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeofCustomEnum, false);

            if (attrs.Length == 0)
                return new string[0];

            CustomEnumAttribute ce = attrs[0] as CustomEnumAttribute;

            if (ce == null)
                return new string[0];

            return ce.Names;
        }

        private static object[] GetObjects(Array a)
        {
            object[] list = new object[a.Length];

            for (int i = 0; i < list.Length; ++i)
                list[i] = a.GetValue(i);

            return list;
        }

        private static bool IsCustomEnum(Type type)
        {
            return type.IsDefined(typeofCustomEnum, false);
        }

        private static CommandPropertyAttribute GetCPA(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(typeof(CommandPropertyAttribute), false);

            if (attrs.Length > 0)
                return attrs[0] as CommandPropertyAttribute;
            else
                return null;
        }

        private class HairStylistSetPoint3DGump : Gump
        {
            private readonly PropertyInfo m_Property;
            private readonly Mobile m_From;
            private readonly INXHairStylist m_HairStylist ;
            private readonly int m_Page;
            private readonly PropertyInfo[] m_PropertyList;

            private static readonly int CoordWidth = 70;
            private static readonly int EntryWidth = CoordWidth + PropsConfig.OffsetSize + CoordWidth + PropsConfig.OffsetSize + CoordWidth;

            private static readonly int TotalWidth = PropsConfig.OffsetSize + EntryWidth + PropsConfig.OffsetSize + PropsConfig.SetWidth + PropsConfig.OffsetSize;
            private static readonly int TotalHeight = PropsConfig.OffsetSize + (4 * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));

            private static readonly int BackWidth = PropsConfig.BorderSize + TotalWidth + PropsConfig.BorderSize;
            private static readonly int BackHeight = PropsConfig.BorderSize + TotalHeight + PropsConfig.BorderSize;

            public HairStylistSetPoint3DGump(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page)
                : base(PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
            {
                m_Property = prop;
                m_From = from;
                m_HairStylist  = hairStylist;
                m_Page = page;
                m_PropertyList = propertyList;

                Point3D p = (Point3D)prop.GetValue(m_HairStylist , null);

                AddPage(0);

                AddBackground(0, 0, BackWidth, BackHeight, PropsConfig.BackGumpID);
                AddImageTiled(PropsConfig.BorderSize, PropsConfig.BorderSize, TotalWidth - (PropsConfig.OldStyle ? PropsConfig.SetWidth + PropsConfig.OffsetSize : 0), TotalHeight, PropsConfig.OffsetGumpID);

                int x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                int y = PropsConfig.BorderSize + PropsConfig.OffsetSize;

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, prop.Name);
                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Use your location");
                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);
                AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 1, GumpButtonType.Reply, 0);

                x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Target a location");
                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);
                AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 2, GumpButtonType.Reply, 0);

                x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                AddImageTiled(x, y, CoordWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, CoordWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "X:");
                AddTextEntry(x + 16, y, CoordWidth - 16, PropsConfig.EntryHeight, PropsConfig.TextHue, 0, p.X.ToString());
                x += CoordWidth + PropsConfig.OffsetSize;

                AddImageTiled(x, y, CoordWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, CoordWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Y:");
                AddTextEntry(x + 16, y, CoordWidth - 16, PropsConfig.EntryHeight, PropsConfig.TextHue, 1, p.Y.ToString());
                x += CoordWidth + PropsConfig.OffsetSize;

                AddImageTiled(x, y, CoordWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, CoordWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Z:");
                AddTextEntry(x + 16, y, CoordWidth - 16, PropsConfig.EntryHeight, PropsConfig.TextHue, 2, p.Z.ToString());
                x += CoordWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);
                AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 3, GumpButtonType.Reply, 0);
            }

            private class InternalTarget : Target
            {
                private readonly PropertyInfo m_Property;
                private readonly Mobile m_From;
                private readonly INXHairStylist m_HairStylist ;
                private readonly int m_Page;
                private readonly PropertyInfo[] m_PropertyList;

                public InternalTarget(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page)
                    : base(-1, true, TargetFlags.None)
                {
                    m_Property = prop;
                    m_From = from;
                    m_HairStylist  = hairStylist;
                    m_PropertyList = propertyList;
                    m_Page = page;
                }

                protected override void OnTarget(Mobile from, object targeted)
                {
                    IPoint3D p = targeted as IPoint3D;

                    if (p != null)
                    {
                        try
                        {
                            CommandLogging.LogChangeProperty(m_From, m_HairStylist , string.Format("INX Hair Stylist: {0}",m_Property.Name), new Point3D(p).ToString());
                            m_Property.SetValue(m_HairStylist , new Point3D(p), null);
                        }
                        catch
                        {
                            m_From.SendMessage("An exception was caught. The property may not have changed.");
                        }
                    }
                }

                protected override void OnTargetFinish(Mobile from)
                {
                    m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Point3D toSet;
                bool shouldSet, shouldSend;

                switch (info.ButtonID)
                {
                    case 1: // Current location
                        {
                            toSet = m_From.Location;
                            shouldSet = true;
                            shouldSend = true;

                            break;
                        }
                    case 2: // Pick location
                        {
                            m_From.Target = new InternalTarget(m_Property, m_From, m_HairStylist , m_PropertyList, m_Page);

                            toSet = Point3D.Zero;
                            shouldSet = false;
                            shouldSend = false;

                            break;
                        }
                    case 3: // Use values
                        {
                            TextRelay x = info.GetTextEntry(0);
                            TextRelay y = info.GetTextEntry(1);
                            TextRelay z = info.GetTextEntry(2);

                            toSet = new Point3D(x == null ? 0 : Utility.ToInt32(x.Text), y == null ? 0 : Utility.ToInt32(y.Text), z == null ? 0 : Utility.ToInt32(z.Text));
                            shouldSet = true;
                            shouldSend = true;

                            break;
                        }
                    default:
                        {
                            toSet = Point3D.Zero;
                            shouldSet = false;
                            shouldSend = true;

                            break;
                        }
                }

                if (shouldSet)
                {
                    try
                    {
                        CommandLogging.LogChangeProperty(m_From, m_HairStylist , string.Format("INX Hair Stylist: {0}", m_Property.Name), toSet.ToString());
                        m_Property.SetValue(m_HairStylist , toSet, null);
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                if (shouldSend)
                    m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
            }
        }

        private class HairStylistSetTimeSpanGump : Gump
        {
            private readonly PropertyInfo m_Property;
            private readonly Mobile m_From;
            private readonly INXHairStylist m_HairStylist ;
            
            private readonly int m_Page;
            private readonly PropertyInfo[] m_PropertyList;

            private static readonly int EntryWidth = 212;

            private static readonly int TotalWidth = PropsConfig.OffsetSize + EntryWidth + PropsConfig.OffsetSize + PropsConfig.SetWidth + PropsConfig.OffsetSize;
            private static readonly int TotalHeight = PropsConfig.OffsetSize + (7 * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));

            private static readonly int BackWidth = PropsConfig.BorderSize + TotalWidth + PropsConfig.BorderSize;
            private static readonly int BackHeight = PropsConfig.BorderSize + TotalHeight + PropsConfig.BorderSize;

            public HairStylistSetTimeSpanGump(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page) : base(PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
            {
                m_Property = prop;
                m_From = from;
                m_HairStylist  = hairStylist;
                m_Page = page;
                m_PropertyList = propertyList;

                TimeSpan ts = (TimeSpan)prop.GetValue(hairStylist, null);

                AddPage(0);

                AddBackground(0, 0, BackWidth, BackHeight, PropsConfig.BackGumpID);
                AddImageTiled(PropsConfig.BorderSize, PropsConfig.BorderSize, TotalWidth - (PropsConfig.OldStyle ? PropsConfig.SetWidth + PropsConfig.OffsetSize : 0), TotalHeight, PropsConfig.OffsetGumpID);

                AddRect(0, prop.Name, 0, -1);
                AddRect(1, ts.ToString(), 0, -1);
                AddRect(2, "Zero", 1, -1);
                AddRect(3, "From H:M:S", 2, -1);
                AddRect(4, "H:", 3, 0);
                AddRect(5, "M:", 4, 1);
                AddRect(6, "S:", 5, 2);
            }

            private void AddRect(int index, string str, int button, int text)
            {
                int x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                int y = PropsConfig.BorderSize + PropsConfig.OffsetSize + (index * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, str);

                if (text != -1)
                    AddTextEntry(x + 16 + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX - 16, PropsConfig.EntryHeight, PropsConfig.TextHue, text, "");

                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                if (button != 0)
                    AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, button, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                TimeSpan toSet;
                bool shouldSet, shouldSend;

                TextRelay h = info.GetTextEntry(0);
                TextRelay m = info.GetTextEntry(1);
                TextRelay s = info.GetTextEntry(2);

                switch (info.ButtonID)
                {
                    case 1: // Zero
                        {
                            toSet = TimeSpan.Zero;
                            shouldSet = true;
                            shouldSend = true;

                            break;
                        }
                    case 2: // From H:M:S
                        {
                            bool successfulParse = false;
                            if (h != null && m != null && s != null)
                            {
                                successfulParse = TimeSpan.TryParse(h.Text + ":" + m.Text + ":" + s.Text, out toSet);
                            }
                            else
                            {
                                toSet = TimeSpan.Zero;
                            }


                            shouldSet = shouldSend = successfulParse;

                            break;
                        }
                    case 3: // From H
                        {
                            if (h != null)
                            {
                                try
                                {
                                    toSet = TimeSpan.FromHours(Utility.ToDouble(h.Text));
                                    shouldSet = true;
                                    shouldSend = true;

                                    break;
                                }
                                catch
                                {
                                }
                            }

                            toSet = TimeSpan.Zero;
                            shouldSet = false;
                            shouldSend = false;

                            break;
                        }
                    case 4: // From M
                        {
                            if (m != null)
                            {
                                try
                                {
                                    toSet = TimeSpan.FromMinutes(Utility.ToDouble(m.Text));
                                    shouldSet = true;
                                    shouldSend = true;

                                    break;
                                }
                                catch
                                {
                                }
                            }

                            toSet = TimeSpan.Zero;
                            shouldSet = false;
                            shouldSend = false;

                            break;
                        }
                    case 5: // From S
                        {
                            if (s != null)
                            {
                                try
                                {
                                    toSet = TimeSpan.FromSeconds(Utility.ToDouble(s.Text));
                                    shouldSet = true;
                                    shouldSend = true;

                                    break;
                                }
                                catch
                                {
                                }
                            }

                            toSet = TimeSpan.Zero;
                            shouldSet = false;
                            shouldSend = false;

                            break;
                        }
                    default:
                        {
                            toSet = TimeSpan.Zero;
                            shouldSet = false;
                            shouldSend = true;

                            break;
                        }
                }

                if (shouldSet)
                {
                    try
                    {
                        CommandLogging.LogChangeProperty(m_From, m_HairStylist , string.Format("INX Hair Stylist: {0}", m_Property.Name), toSet.ToString());
                        m_Property.SetValue(m_HairStylist , toSet, null);                       
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                if (shouldSend)
                    m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
            }
        }

        public class HairStylistSetCustomEnumGump : SetListOptionGump
        {
            private new readonly PropertyInfo m_Property;
            private readonly Mobile m_From;
            private readonly INXHairStylist m_HairStylist ;
            private new readonly int m_Page;
            private readonly PropertyInfo[] m_PropertyList;
            private readonly string[] m_Names;

            public HairStylistSetCustomEnumGump(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, string[] names, int page)
                : base(prop, from, hairStylist, null, page, new ArrayList(propertyList), names, null)
            {
                m_Property = prop;
                m_From = from;
                m_HairStylist  = hairStylist;
                m_PropertyList = propertyList;
                m_Names = names;
                m_Page = page;
            }

            public override void OnResponse(NetState sender, RelayInfo relayInfo)
            {
                int index = relayInfo.ButtonID - 1;

                if (index >= 0 && index < m_Names.Length)
                {
                    try
                    {
                        MethodInfo info = m_Property.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });

                        string result = "";

                        if (info != null)
                            result = Properties.SetDirect(m_From, m_HairStylist , m_HairStylist , m_Property, m_Property.Name, info.Invoke(null, new object[] { m_Names[index] }), true);
                        else if (m_Property.PropertyType == typeof(Enum) || m_Property.PropertyType.IsSubclassOf(typeof(Enum)))
                            result = Properties.SetDirect(m_From, m_HairStylist , m_HairStylist , m_Property, m_Property.Name, Enum.Parse(m_Property.PropertyType, m_Names[index], false), true);

                        m_From.SendMessage(result);                            
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
            }
        }

        public class HairStylistSetListOptionGump : Gump
        {
            protected PropertyInfo m_Property;
            protected Mobile m_From;
            protected INXHairStylist m_HairStylist ;           
            protected PropertyInfo[] m_PropertyList;
            protected int m_Page;

            private static readonly int EntryWidth = 212;
            private static readonly int EntryCount = 13;

            private static readonly int TotalWidth = PropsConfig.OffsetSize + EntryWidth + PropsConfig.OffsetSize + PropsConfig.SetWidth + PropsConfig.OffsetSize;

            private static readonly int BackWidth = PropsConfig.BorderSize + TotalWidth + PropsConfig.BorderSize;

            private static readonly bool PrevLabel = PropsConfig.OldStyle;
            private static readonly bool NextLabel = PropsConfig.OldStyle;

            private static readonly int PrevLabelOffsetX = PropsConfig.PrevWidth + 1;
            private static readonly int PrevLabelOffsetY = 0;

            private static readonly int NextLabelOffsetX = -29;
            private static readonly int NextLabelOffsetY = 0;

            protected object[] m_Values;

            public HairStylistSetListOptionGump(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, string[] names, object[] values, int propsPage)
                : base(PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
            {
                m_Property = prop;
                m_From = from;
                m_HairStylist  = hairStylist;

                m_Page = propsPage;
                m_PropertyList = propertyList;

                m_Values = values;

                int pages = (names.Length + EntryCount - 1) / EntryCount;
                int index = 0;

                for (int page = 1; page <= pages; ++page)
                {
                    AddPage(page);

                    int start = (page - 1) * EntryCount;
                    int count = names.Length - start;

                    if (count > EntryCount)
                        count = EntryCount;

                    int totalHeight = PropsConfig.OffsetSize + ((count + 2) * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));
                    int backHeight = PropsConfig.BorderSize + totalHeight + PropsConfig.BorderSize;

                    AddBackground(0, 0, BackWidth, backHeight, PropsConfig.BackGumpID);
                    AddImageTiled(PropsConfig.BorderSize, PropsConfig.BorderSize, TotalWidth - (PropsConfig.OldStyle ? PropsConfig.SetWidth + PropsConfig.OffsetSize : 0), totalHeight, PropsConfig.OffsetGumpID);



                    int x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                    int y = PropsConfig.BorderSize + PropsConfig.OffsetSize;

                    int emptyWidth = TotalWidth - PropsConfig.PrevWidth - PropsConfig.NextWidth - (PropsConfig.OffsetSize * 4) - (PropsConfig.OldStyle ? PropsConfig.SetWidth + PropsConfig.OffsetSize : 0);

                    AddImageTiled(x, y, PropsConfig.PrevWidth, PropsConfig.EntryHeight, PropsConfig.HeaderGumpID);

                    if (page > 1)
                    {
                        AddButton(x + PropsConfig.PrevOffsetX, y + PropsConfig.PrevOffsetY, PropsConfig.PrevButtonID1, PropsConfig.PrevButtonID2, 0, GumpButtonType.Page, page - 1);

                        if (PrevLabel)
                            AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, PropsConfig.TextHue, "Previous");
                    }

                    x += PropsConfig.PrevWidth + PropsConfig.OffsetSize;

                    if (!PropsConfig.OldStyle)
                        AddImageTiled(x - (PropsConfig.OldStyle ? PropsConfig.OffsetSize : 0), y, emptyWidth + (PropsConfig.OldStyle ? PropsConfig.OffsetSize * 2 : 0), PropsConfig.EntryHeight, PropsConfig.HeaderGumpID);

                    x += emptyWidth + PropsConfig.OffsetSize;

                    if (!PropsConfig.OldStyle)
                        AddImageTiled(x, y, PropsConfig.NextWidth, PropsConfig.EntryHeight, PropsConfig.HeaderGumpID);

                    if (page < pages)
                    {
                        AddButton(x + PropsConfig.NextOffsetX, y + PropsConfig.NextOffsetY, PropsConfig.NextButtonID1, PropsConfig.NextButtonID2, 0, GumpButtonType.Page, page + 1);

                        if (NextLabel)
                            AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, PropsConfig.TextHue, "Next");
                    }



                    AddRect(0, prop.Name, 0);

                    for (int i = 0; i < count; ++i)
                        AddRect(i + 1, names[index], ++index);
                }
            }

            private void AddRect(int index, string str, int button)
            {
                int x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                int y = PropsConfig.BorderSize + PropsConfig.OffsetSize + ((index + 1) * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, str);

                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                if (button != 0)
                    AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, button, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                int index = info.ButtonID - 1;

                if (index >= 0 && index < m_Values.Length)
                {
                    try
                    {
                        object toSet = m_Values[index];

                        string result = Properties.SetDirect(m_From, m_HairStylist , m_HairStylist , m_Property, m_Property.Name, toSet, true);

                        m_From.SendMessage(result);                           
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
            }
        }

        public class HairStylistSetGump : Gump
        {
            private readonly PropertyInfo m_Property;
            private readonly Mobile m_From;
            private readonly INXHairStylist m_HairStylist ;
            
            private readonly int m_Page;
            private readonly PropertyInfo[] m_PropertyList;

            private static readonly int EntryWidth = 212;

            private static readonly int TotalWidth = PropsConfig.OffsetSize + EntryWidth + PropsConfig.OffsetSize + PropsConfig.SetWidth + PropsConfig.OffsetSize;
            private static readonly int TotalHeight = PropsConfig.OffsetSize + (2 * (PropsConfig.EntryHeight + PropsConfig.OffsetSize));

            private static readonly int BackWidth = PropsConfig.BorderSize + TotalWidth + PropsConfig.BorderSize;
            private static readonly int BackHeight = PropsConfig.BorderSize + TotalHeight + PropsConfig.BorderSize;

            public HairStylistSetGump(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page)
                : base(PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
            {
                m_Property = prop;
                m_From = from;
                m_HairStylist  = hairStylist;
                
                m_Page = page;
                m_PropertyList = propertyList;

                bool canNull = !prop.PropertyType.IsValueType;
                bool canDye = prop.IsDefined(typeof(HueAttribute), false);
                bool isBody = prop.IsDefined(typeof(BodyAttribute), false);

                object val = prop.GetValue(m_HairStylist , null);
                string initialText;

                if (val == null)
                    initialText = "";
                else
                    initialText = val.ToString();

                AddPage(0);

                AddBackground(0, 0, BackWidth, BackHeight + (canNull ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0) + (canDye ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0) + (isBody ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0), PropsConfig.BackGumpID);
                AddImageTiled(PropsConfig.BorderSize, PropsConfig.BorderSize, TotalWidth - (PropsConfig.OldStyle ? PropsConfig.SetWidth + PropsConfig.OffsetSize : 0), TotalHeight + (canNull ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0) + (canDye ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0) + (isBody ? (PropsConfig.EntryHeight + PropsConfig.OffsetSize) : 0), PropsConfig.OffsetGumpID);

                int x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                int y = PropsConfig.BorderSize + PropsConfig.OffsetSize;

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, prop.Name);
                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                AddTextEntry(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, 0, initialText);
                x += EntryWidth + PropsConfig.OffsetSize;

                if (PropsConfig.SetGumpID != 0)
                    AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 1, GumpButtonType.Reply, 0);

                if (canNull)
                {
                    x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                    y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                    AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                    AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Null");
                    x += EntryWidth + PropsConfig.OffsetSize;

                    if (PropsConfig.SetGumpID != 0)
                        AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                    AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 2, GumpButtonType.Reply, 0);
                }

                if (canDye)
                {
                    x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                    y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                    AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                    AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Hue Picker");
                    x += EntryWidth + PropsConfig.OffsetSize;

                    if (PropsConfig.SetGumpID != 0)
                        AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                    AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 3, GumpButtonType.Reply, 0);
                }

                if (isBody)
                {
                    x = PropsConfig.BorderSize + PropsConfig.OffsetSize;
                    y += PropsConfig.EntryHeight + PropsConfig.OffsetSize;

                    AddImageTiled(x, y, EntryWidth, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                    AddLabelCropped(x + PropsConfig.TextOffsetX, y, EntryWidth - PropsConfig.TextOffsetX, PropsConfig.EntryHeight, PropsConfig.TextHue, "Body Picker");
                    x += EntryWidth + PropsConfig.OffsetSize;

                    if (PropsConfig.SetGumpID != 0)
                        AddImageTiled(x, y, PropsConfig.SetWidth, PropsConfig.EntryHeight, PropsConfig.SetGumpID);

                    AddButton(x + PropsConfig.SetOffsetX, y + PropsConfig.SetOffsetY, PropsConfig.SetButtonID1, PropsConfig.SetButtonID2, 4, GumpButtonType.Reply, 0);
                }
            }

            private class InternalPicker : HuePicker
            {
                private readonly PropertyInfo m_Property;
                private readonly Mobile m_From;
                private readonly INXHairStylist m_HairStylist ;
                
                private readonly int m_Page;
                private readonly PropertyInfo[] m_PropertyList;

                public InternalPicker(PropertyInfo prop, Mobile from, INXHairStylist hairStylist, PropertyInfo[] propertyList, int page)
                    : base(((IHued)hairStylist).HuedItemID)
                {
                    m_Property = prop;
                    m_From = from;
                    m_HairStylist  = hairStylist;
                    
                    m_Page = page;
                    m_PropertyList = propertyList;
                }

                public override void OnResponse(int hue)
                {
                    try
                    {
                        CommandLogging.LogChangeProperty(m_From, m_HairStylist , string.Format("INX Hair Stylist: {0}", m_Property.Name), hue.ToString());
                        m_Property.SetValue(m_HairStylist , hue, null);
                        
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }

                    m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                object toSet;
                bool shouldSet, shouldSend = true;

                switch (info.ButtonID)
                {
                    case 1:
                        {
                            TextRelay text = info.GetTextEntry(0);

                            if (text != null)
                            {
                                try
                                {
                                    toSet = PropertiesGump.GetObjectFromString(m_Property.PropertyType, text.Text);
                                    shouldSet = true;
                                }
                                catch
                                {
                                    toSet = null;
                                    shouldSet = false;
                                    m_From.SendMessage("Bad format");
                                }
                            }
                            else
                            {
                                toSet = null;
                                shouldSet = false;
                            }

                            break;
                        }
                    case 2: // Null
                        {
                            toSet = null;
                            shouldSet = true;

                            break;
                        }
                    case 3: // Hue Picker
                        {
                            toSet = null;
                            shouldSet = false;
                            shouldSend = false;

                            m_From.SendHuePicker(new InternalPicker(m_Property, m_From, m_HairStylist , m_PropertyList, m_Page ));

                            break;
                        }
                    case 4: // Body Picker
                        {
                            toSet = null;
                            shouldSet = false;
                            shouldSend = false;

                            m_From.SendGump(new SetBodyGump(m_Property, m_From, m_HairStylist , null, m_Page, new ArrayList(m_PropertyList)));

                            break;
                        }
                    default:
                        {
                            toSet = null;
                            shouldSet = false;

                            break;
                        }
                }

                if (shouldSet)
                {
                    try
                    {
                        CommandLogging.LogChangeProperty(m_From, m_HairStylist , string.Format("INX Hair Stylist: {0}", m_Property.Name), toSet == null ? "(null)" : toSet.ToString());
                        m_Property.SetValue(m_HairStylist , toSet, null);
                        
                    }
                    catch
                    {
                        m_From.SendMessage("An exception was caught. The property may not have changed.");
                    }
                }

                if (shouldSend)
                    m_From.SendGump(new HairStylistConfigGump(m_From, m_HairStylist , m_PropertyList, m_Page));
            }
        }
    }
}