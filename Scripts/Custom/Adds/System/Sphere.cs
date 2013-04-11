using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Solaris.CliLocHandler;

namespace Server
{
    public static class Sphere
	{
        private static readonly string[] m_MissMessages = new string[]{"You miss {0}","{0} misses you"};

		private const string m_DropFormat = "You put the {0} in your pack.";
        private static readonly Dictionary<int, string[]> m_HitMessages = new Dictionary<int, string[]>{
                        {0, new []{"You hit {0} straight in the face!","{0} hits you straight in the face!"}},
                        {1, new []{"You hit {0} in the head!","{0} hits you in the head!"}},
                        {2, new []{"You hit {0} square in the jaw!","{0} hits you square in the jaw!"}},
                        {3, new []{"You score a stunning blow to {0}'s head!","{0} scores a stunning blow to your head!"}},
                        {4, new []{"You smash a blow across {0}'s face!","{0} smashes a blow across your face!"}},
                        {5, new []{"You score a terrible hit to {0}'s temple!","{0} scores a terrible hit to your temple!"}},
                        {6, new []{"You hit {0}'s Chest!","{0} hits your Chest!"}},
                        {7, new []{"You land a blow to {0}'s stomach!","{0} lands a blow to your stomach!"}},
                        {8, new []{"You hit {0} in the ribs!","{0} hits you in the ribs!"}},
                        {9, new []{"You land a terrible blow to {0}'s chest!","{0} lands a terrible blow to your chest!"}},
                        {10, new []{"You knock the wind out of {0}!","{0} knocks the wind out of you!"}},
                        {11, new []{"You smash {0} in the rib cage!","{0} smashed you in the rib cage!"}},
                        {12, new []{"You hit {0}'s left arm!","{0} hits your left arm!"}},
                        {13, new []{"You hit {0}'s right arm!","{0} hits your right arm!"}},
                        {14, new []{"You hit {0}'s left thigh!","{0} hits your left thigh!"}},
                        {15, new []{"You hit {0}'s right thigh!","{0} hits your right thigh!"}},
                        {16, new []{"You hit {0} in the groin!","{0} hits you in the groin!"}},
                        {17, new []{"You hit {0}'s left hand!","{0} hits your left hand!"}},
                        {18, new []{"You hit {0}'s right hand!","{0} hits your right hand!"}},
                        {19, new []{"You hit {0} in the throat!","{0} smashes you in the throat!"}},
                        {20, new []{"You score a hit to {0}'s back!","{0} scores a hit to your back!"}},
                        {21, new []{"You hit {0}'s foot!","{0} hits your foot!"}}
                        };

        public static string[] GetHitMessages()
        {
            #region LegacyCode - save in case new messages end up wrong

            /*switch (msg_num)
            {
                case 0:
                    {
                        amsg = "You hit {0} straight in the face!";
                        dmsg = "{0} hits you straight in the face!";
                    }
                    break;
                case 1:
                    {
                        amsg = "You hit {0} in the head!";
                        dmsg = "{0} hits you in the head!";
                    }
                    break;
                case 2:
                    {
                        amsg = "You hit {0} square in the jaw!";
                        dmsg = "{0} hits you square in the jaw!";
                    }
                    break;
                case 3:
                    {
                        amsg = "You score a stunning blow to {0}'s head!";
                        dmsg = "{0} scores a stunning blow to your head!";
                    }
                    break;
                case 4:
                    {
                        amsg = "You smash a blow across {0}'s face!";
                        dmsg = "{0} smashes a blow across your face!";
                    }
                    break;
                case 5:
                    {
                        amsg = "You score a terrible hit to {0}'s temple!";
                        dmsg = "{0} scores a terrible hit to your temple!";
                    }
                    break;
                case 6:
                    {
                        amsg = "You hit {0}'s Chest!";
                        dmsg = "{0} hits your Chest!";
                    }
                    break;
                case 7:
                    {
                        amsg = "You land a blow to {0}'s stomach!";
                        dmsg = "{0} lands a blow to your stomach!";
                    }
                    break;
                case 8:
                    {
                        amsg = "You hit {0} in the ribs!";
                        dmsg = "{0} hits you in the ribs!";
                    }
                    break;
                case 9:
                    {
                        amsg = "You land a terrible blow to {0}'s chest!";
                        dmsg = "{0} lands a terrible blow to your chest!";
                    }
                    break;
                case 10:
                    {
                        amsg = "You knock the wind out of {0}!";
                        dmsg = "{0} knocks the wind out of you!";
                    }
                    break;
                case 11:
                    {
                        amsg = "You smash {0} in the rib cage!";
                        dmsg = "{0} smashed you in the rib cage!";
                    }
                    break;
                case 12:
                    {
                        amsg = "You hit {0}'s left arm!";
                        dmsg = "{0} hits your left arm!";
                    }
                    break;
                case 13:
                    {
                        amsg = "You hit {0}'s right arm!";
                        dmsg = "{0} hits your right arm!";
                    }
                    break;
                case 14:
                    {
                        amsg = "You hit {0}'s left thigh!";
                        dmsg = "{0} hits your left thigh!";
                    }
                    break;
                case 15:
                    {
                        amsg = "You hit {0}'s right thigh!";
                        dmsg = "{0} hits your right thigh!";
                    }
                    break;
                case 16:
                    {
                        amsg = "You hit {0} in the groin!";
                        dmsg = "{0} hits you in the groin!";
                    }
                    break;
                case 17:
                    {
                        amsg = "You hit {0}'s left hand!";
                        dmsg = "{0} hits your left hand!";
                    }
                    break;
                case 18:
                    {
                        amsg = "You hit {0}'s right hand!";
                        dmsg = "{0} hits your right hand!";
                    }
                    break;
                case 19:
                    {
                        amsg = "You hit {0} in the throat!";
                        dmsg = "{0} smashes you in the throat!";
                    }
                    break;
                case 20:
                    {
                        amsg = "You score a hit to {0}'s back!";
                        dmsg = "{0} scores a hit to your back!";
                    }
                    break;
                case 21:
                    {
                        amsg = "You hit {0}'s foot!";
                        dmsg = "{0} hits your foot!";
                    }
                    break;
            }*/

            #endregion

            string[] msgs;
            m_HitMessages.TryGetValue(Utility.Random(22), out msgs);

            return msgs;
        }

        public static string[] GetMissMessages()
        {
            return m_MissMessages;
        }

        public static int RandomHue()
        {
            return Utility.RandomList(443, 443, 902, 902, 907, 907, 928, 928, 946, 947, 1201, 1247, 1301, 1347, 1401, 1447, 1501, 1547, 1601, 1654, 1701, 1747, 1801, 1887);
        }

		// Are hues correct?
		public static int[] RareHues = new int[]
			{
                1190, // sparkle green
                2969, // oceanic
                1172, // Gold
                1177, // sparkle pink&green
                2548, // opiate
                2589, // bright orange
                1158, // shiny red.
                2561, // outlined yellow.
                1150, // blackrock
                1159, // Old copper
                2545, // shiny brown
                2513, // Ice
                2562, // outlined green
                2530, // black diamond
                1196, // sparkle green,blue & pink
                2514, // oilish green
                2535, // metal blue
                1325, // mythril
                2580, // "diamond" bright blue with some white
                2563, // outlined blue
                1173, // bright metal black
                2546, // bright metal green
                1174, // dark brown/black metal
                1155, // old opiate (pink and blue)
                2559, // outlined red
                //1963, // Saphire
                2540, // bright pink
                2948, // Rose
                1947, // Aqua
                1301, // Valorite
                2529, // dark black metal
                2543, // Bright orange with some yellow. fat colour.
                2207, // Verite
                1218, // Blood rock
                1188, // sparkle blue & green
                1197, // sparkle purple
                2544, // shiny red2 (a little brighter than the first)
                2551, // Sand Rock
                2550, // bright pink&white
                2549, // shiny green2 (a little brighter than the first)
                2565, // outlined pink
                2564, // outlined purple
                2547, //shiny blue2 (a little brighter than the first)
                2578, // bright oil green with a little blue
                2541, // ahhh you kill my eyes white red. fat.
                2577, // old opiate but daaark (blue and purple)
                1904, // Shadow
                2944, // outlined white
                2576, // fat dark red with black.
                2591, // fat nice yellow
                1175, // silver
                2573, // oilish blue red.
                1169, // Lime, bright bright green.
                2516, // metal dark green
                2515, // metal dark purple
                2526, // goldish dark yellow fat
                2517, // ice2 (mostly dark blue with some brighter blue in it)
                2585, // oil green2
                2574, // oil blue red
                2571, // oil green3 (brighter)
                2525, // white with some gray
                2518, // warm colour yellow and red.
                1171, // Daemon steel
                2531, // Gold
                1187, // sparkle green2 (darker and with less sparkles)
                2524, // dark metal blue
                1176, // dark metal red.
                1153, // snow
                1154, // silver like the old silver weapons
                1179, // sparkle with many sparkles monstly green.
                1157, // metal white with some gray, bright.
                1178, // oil sparkle dark green
                2527, // metal dark purple
                2560, // outlined orange
                1156, // White
                2579, // Bright old opiate (pink/purple blue)
                2538, // old opiate a little brighter.
                1160, // shiny brown2 (a little brighter)
                2586, // oil red green
                2532, // metal blue bright alittle purple in it also
                2534, //metal red dark
                2523, // nice metal red bright
                2567, // metal green white bright
                2533, // briiiiiiight shiny red
                2552, // peach, purple white neat.
                2569, // "siang yellow" yellow bright warm colour
                1151, // old ice blue warm colour (dark blue and bright blue)
                2583, // oil green, dark green with a outline of bright green, fat
                //2558, // Adamantium
                2570, // warm metal yellow and orange
                2557, // very bright metal blue
                1926, // dark blue and green
                1932, // fat dark red
                1928, // diamond2 like the previous but darker.
                2496, // fat warm bright pink
                1915, // neat fat warm blue.
                1956,
                1941,
                1957, // reactive
                1960,
                1959,
                1961,
                1955,
                1952,
                1954,
                1958,
                1943,
                1942,
                1962,
                1946,
                1945,
                1944

                #region Unsed
                /*/116, //Undine
				//53, //Lunar
				//9,//Aqua
				1109, //Black rock
				1909, //Outlined
				1910, //Outlined
				1911, //Terra
				1912, //Outlined
				1913, //Outlined
				1914, //Outlined
				1915, //Lightning
				1916, //Diamond
				1917, //Uranium
				1918, //Havoc
				1919, //Hydro
				1920, //Tundra
				1921, //Plasma
				1922, //Adantium
				1923, //Cobalt
				1924, //Hades
				1925, //Black Diamond
				1926, //Gold (old)
				1927, //Titan
				1928, //Red hell
				1929, //Red Xtreme (old)
				1930, //Blue Xtreme (old)
				2051, //Green Xtreme
				1932, //Lime
				1933, //Outlined
				1934, //Blood rock
				1935, //Rose
				1937, //Death
				1938, //Fire
				1939, //Devastator
				1940, //Brozne
				1941, //Outlined
				1942, //Water brand
				1943, //Opiate
				1944, //Dragon scale
				1946, //Sand rock
				1947, //Outlined
				1948, //Mytheril
				//1949,//Outlined
				1950, //Venom
				1951, //Storm
				1952, //Alpha
				1953, //Adamantium (old)
				2050, //Oceanic
				1954, //Outlined
				1955, //Outlined
				1956, //Outlined
				1957, //Outlined
				1958, //Outlined
				1959, //Outlined
				1960, //Outlined
				1961, //Outlined*/
                #endregion
			};

		public static string ComputeName( Item i )
		{
		    if( i is BaseWeapon )
				return ComputeName( ( i as BaseWeapon ) );
		    if( i is BaseArmor )
		        return ComputeName( ( i as BaseArmor ) );
		    if( i is BaseClothing )
		        return ComputeName( ( i as BaseClothing ) );
		    if (i is BaseInstrument)
		        return ComputeName((i as BaseInstrument));
            if (i is SpellScroll)
                return ComputeName((i as SpellScroll));
            if (i is RepairDeed)
                return ComputeName((i as RepairDeed));
		    return GenericComputeName( i );
		}

	    public static string ComputeName( BaseArmor ba )
		{
			if( ba.IsRenamed && !string.IsNullOrEmpty( ba.Name ) )
				return ba.Name;

			string name;

			if( ba.Name == null )
				name = CliLoc.LocToString( ba.LabelNumber );
			else
				name = ba.Name;

            if (ba.Amount > 1)
                name = name + "s";

			var resource = string.Empty;

			if( ba.Resource != CraftResource.None && ba.Resource != CraftResource.Iron )
				resource = CraftResources.GetName( ba.Resource );

            if ((ba.ProtectionLevel != ArmorProtectionLevel.Regular))// && ba.Resource == CraftResource.Iron )
            //If the armor is magical
            {
                if (ba.Quality == ArmorQuality.Exceptional)
                    name = string.Format("{0} {1} of {2}", "Exceptional", name.ToLower(), CliLoc.LocToString((1038005 + (int)ba.ProtectionLevel)).ToLower());
                else
                    name = string.Format("{0} of {1}", name, CliLoc.LocToString((1038005 + (int) ba.ProtectionLevel)).ToLower());
            }
            else if (ba.Resource == CraftResource.None && ba.ProtectionLevel == ArmorProtectionLevel.Regular)
            //If the armor is not magical and not crafted
            {
                if (ba.Quality == ArmorQuality.Exceptional)
                    name = string.Format("{0} {1}", "Exceptional", name );
            }
            else if (ba.Resource != CraftResource.None)
            {
                //If it's crafted by a player
                if (ba.Crafter != null)
                {
                    if (ba.Quality == ArmorQuality.Exceptional)
                    {
                        if (ba.Resource != CraftResource.Iron)
                            name = string.Format("{0} {1} {2} crafted by {3}", "Exceptional", resource.ToLower(), name.ToLower(), ba.Crafter.Name);
                        else
                            name = string.Format("{0} {1} crafted by {2}", "Exceptional", name.ToLower(), ba.Crafter.Name);
                    }
                    else if (ba.Resource != CraftResource.Iron)
                        name = string.Format("{0} {1}", resource, name.ToLower());
                    else
                        name = string.Format("{0}", name);
                }
                else
                    if (ba.Quality == ArmorQuality.Exceptional)
                        if (!string.IsNullOrEmpty(resource))
                            name = string.Format("{0} {1} {2}", "Exceptional", resource.ToLower(), name.ToLower());
                        else
                            name = string.Format("{0} {1}", "Exceptional", name.ToLower());
                    else
                        if (!string.IsNullOrEmpty(resource))
                            name = string.Format("{0} {1}", resource, name.ToLower());
                        else
                            name = string.Format(name);
            }

            if (ba.Amount > 1)
                name = ba.Amount + " " + name;

			return name;
		}

        public static string ComputeCustomWeaponName( BaseWeapon bw )
        {
            string name = bw.Name;

            if (bw.Crafter == null)
            {
                if (bw.Quality == WeaponQuality.Exceptional)
                    name = "Exceptional " + bw.Name.ToLower();
            }
            else
            {
                if (bw.Quality == WeaponQuality.Exceptional)
                    name = string.Format("Exceptional {0} crafted by {1} ", bw.Name.ToLower(), bw.Crafter.Name);
                else
                    name = string.Format("{0} crafted by {1}", bw.Name, bw.Crafter.Name);
            }

            return name;
        }

		public static string ComputeName( BaseWeapon bw )
		{
			if( bw.IsRenamed && !string.IsNullOrEmpty( bw.Name ) )
				return bw.Name;

			string name;

			if( bw.Name == null )
				name = CliLoc.LocToString( bw.LabelNumber );
			else
				name = bw.Name;

            if (bw.Amount > 1)
                name = name + "s";

			var resource = string.Empty;

            if (bw.Slayer != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(bw.Slayer);
                if (entry != null)
                {
                    string slayername = CliLoc.LocToString( entry.Title );
                    name = slayername + " " + name.ToLower();
                }
            }

			if( bw.Resource != CraftResource.None && bw.Resource != CraftResource.Iron )
				resource = CraftResources.GetName( bw.Resource );

            if ((bw.DamageLevel != WeaponDamageLevel.Regular || bw.AccuracyLevel != WeaponAccuracyLevel.Regular) && bw.Resource == CraftResource.Iron)
            {
                //If the weapon is accurate or magical
                if (bw.DamageLevel != WeaponDamageLevel.Regular && bw.AccuracyLevel != WeaponAccuracyLevel.Regular)
                    name = string.Format("{0} {1} of {2}", ComputeAccuracyLevel(bw), name.ToLower(), CliLoc.LocToString((1038015 + (int) bw.DamageLevel)).ToLower());
                else if (bw.AccuracyLevel != WeaponAccuracyLevel.Regular)
                    name = string.Format("{0} {1}", ComputeAccuracyLevel(bw), name.ToLower());
                else
                    name = string.Format("{0} of {1}", name, CliLoc.LocToString((1038015 + (int) bw.DamageLevel)).ToLower());

                if (bw.Quality == WeaponQuality.Exceptional)
                    name = "Exceptional " + name.ToLower();
            }
            else if (bw.Resource != CraftResource.None)
            {
                //If it's crafted by a player
                if (bw.Crafter != null)
                    if (bw.Quality == WeaponQuality.Exceptional)
                        if (bw.Resource != CraftResource.Iron)
                            name = string.Format("{0} {1} {2} crafted by {3}", "Exceptional", resource.ToLower(), name.ToLower(), bw.Crafter.Name);
                        else
                            name = string.Format("{0} {1} crafted by {2}", "Exceptional", name.ToLower(), bw.Crafter.Name);
                    else if (bw.Resource != CraftResource.Iron)
                        if (!string.IsNullOrEmpty(resource))
                            name = string.Format("{0} {1} crafted by {2}", resource, name.ToLower(), bw.Crafter.Name);
                        else
                            name = string.Format("{0} crafted by {1}", name, bw.Crafter.Name);
                    else
                        name = string.Format("{0} crafted by {1}", name, bw.Crafter.Name);
                else if (bw.Resource != CraftResource.Iron)
                    if (bw.Quality == WeaponQuality.Exceptional)
                        if (!string.IsNullOrEmpty(resource))
                            name = string.Format("{0} {1} {2}", "Exceptional", resource.ToLower(), name.ToLower());
                        else
                            name = string.Format("{0}, {1}", "Exceptional", name.ToLower());
                    else if (!string.IsNullOrEmpty(resource))
                        name = string.Format("{0} {1}", resource, name.ToLower());
                    else
                        name = string.Format(name);
                else if (bw.Resource == CraftResource.Iron)
                    if (bw.Quality == WeaponQuality.Exceptional)
                        name = string.Format("{0} {1}", "Exceptional", name.ToLower());
            }
		    if (bw.Amount > 1)
                name = bw.Amount + " " + name;

			return name;
		}

		public static string ComputeName( BaseClothing bc )
		{
			if( bc.IsRenamed && !string.IsNullOrEmpty( bc.Name ) )
				return bc.Name;

			string name;

			if( bc.Name == null )
				name = CliLoc.LocToString( bc.LabelNumber );
			else
				name = bc.Name;

            if (bc.Amount > 1)
                name = name + "s";

			var resource = string.Empty;

			if( bc.Resource != CraftResource.None && bc.Resource != CraftResource.Iron )
				resource = CraftResources.GetName( bc.Resource );

			if( bc.Crafter != null )
				if( bc.Quality == ClothingQuality.Exceptional )
					if( bc.Resource != CraftResource.None )
						name = string.Format( "{0} {1} {2} crafted by {3}", "Exceptional", resource.ToLower(), name.ToLower(), bc.Crafter.Name );
					else
						name = string.Format( "{0} {1} crafted by {2}", "Exceptional", name.ToLower(), bc.Crafter.Name );
				else if( bc.Resource != CraftResource.None )
                    if (!string.IsNullOrEmpty(resource))
					    name = string.Format( "{0} {1} crafted by {2}", resource, name.ToLower(), bc.Crafter.Name );
                    else
                        name = string.Format("{0} crafted by {1}", name, bc.Crafter.Name);
				else
					name = string.Format( "{0} crafted by {1}", name.ToLower(), bc.Crafter.Name );
			else if( bc.Resource != CraftResource.None )
                if (bc.Quality == ClothingQuality.Exceptional)
                    if (!string.IsNullOrEmpty(resource))
			            name = string.Format( "{0} {1} {2}", "Exceptional", resource.ToLower(), name.ToLower() );
                    else
                        name = string.Format(" {0} {1}", "Exceptional", name.ToLower());
                else
                    if (!string.IsNullOrEmpty(resource))
                        name = string.Format("{0} {1}", resource, name.ToLower());
                    else
                        name = string.Format(name);
            else if (bc.Resource == CraftResource.None)
                if (bc.Quality == ClothingQuality.Exceptional)
                    name = string.Format("{0} {1}", "Exceptional", name.ToLower());

            if (bc.Amount > 1)
                name = bc.Amount + " " + name;

			return name;
		}

        public static string ComputeName(BaseInstrument bi)
        {
            string name;

            if (bi.Name == null)
                name = CliLoc.LocToString(bi.LabelNumber);
            else
                name = bi.Name;

            if (bi.Crafter != null)
                name = string.Format("{0} crafted by {1}", name, bi.Crafter.Name);

            return name;
        }

        public static string ComputeName(SpellScroll ss)
        {
            string name = string.IsNullOrEmpty(ss.Name) ? CliLoc.LocToString(ss.LabelNumber) : ss.Name;

            return (name + " scroll");
        }

        public static string ComputeName(RepairDeed rd)
        {
            string name = string.Format("Repair service contract from {0} {1} crafted by {2}",
                                        CliLoc.LocToString(rd.GetSkillTitle(rd.SkillLevel)).ToLower(),
                                        rd.CrafterSkill().ToLower(),
                                        rd.Crafter != null ? rd.Crafter.Name : "unknown");

            return name;
        }

		public static string GenericComputeName( Item i )
		{
			string name;

			if( i.Name == null )
				name = CliLoc.LocToString( i.LabelNumber );
			else
				name = i.Name;

			return name;
		}

        public static string ComputeAccuracyLevel(BaseWeapon bw)
        {
            string level;
            switch (bw.AccuracyLevel)
            {
                case WeaponAccuracyLevel.Accurate:
                    level = "Accurate";
                    break;

                case WeaponAccuracyLevel.Surpassingly:
                    level = "Surpassingly accurate";
                    break;

                case WeaponAccuracyLevel.Eminently:
                    level = "Eminently accurate";
                    break;

                case WeaponAccuracyLevel.Exceedingly:
                    level = "Exceedingly accurate";
                    break;

                case WeaponAccuracyLevel.Supremely:
                    level = "Supremely accurate";
                    break;

                default:
                    level = "";
                    break;
            }
            return level;
        }

		public static bool CanUse( Mobile from, Item toUse )
		{
			if( from == null || toUse == null )
				return false;

			if( from.Frozen )
				from.SendAsciiMessage( "You are frozen and you cannot do that." );
			else if( !toUse.Movable )
				from.SendAsciiMessage( "You can't use that." );
			else if( !toUse.IsChildOf( from.Backpack ) && toUse.Parent != from && !from.InRange( toUse.Location, 4 ) && !toUse.IsChildOf( from.BankBox ) && !from.BankBox.Opened )
				from.SendAsciiMessage( "You can't reach that." );
			else if( !from.InLOS( toUse ) && !toUse.IsChildOf( from.BankBox ) && !from.BankBox.Opened )
				from.SendAsciiMessage( "You can't see that." );
			else
				return toUse.CanEquip( from );

			return false;
		}

		public static bool EquipOnDouble( Mobile from, Item toEquip )
		{
			Item toDrop;

			//Can we use it?
			if( !CanUse( from, toEquip ) )
				return false;

			//Is the item equiped already?
			if( from.FindItemOnLayer( toEquip.Layer ) == toEquip )
				return true;

			//Turn towards the item.
			if( toEquip.Parent == null )
				SpellHelper.Turn( from, toEquip );

			if( toEquip.Layer == Layer.TwoHanded )
			{
				//Always drop the 2 handed item, if it exists.
				toDrop = from.FindItemOnLayer( Layer.TwoHanded );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}

				//All non sheild 2handed need all the hand players.
				if( !( toEquip is BaseArmor ) )
				{
					toDrop = from.FindItemOnLayer( Layer.FirstValid );
					if( toDrop != null )
					{
						from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
						from.AddToBackpack( toDrop );
					}

					toDrop = from.FindItemOnLayer( Layer.OneHanded );
					if( toDrop != null )
					{
						from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
						from.AddToBackpack( toDrop );
					}
				}
			}
			else if( toEquip.Layer == Layer.OneHanded )
			{
				//All non shield 2 hands use both hands.
				toDrop = from.FindItemOnLayer( Layer.TwoHanded );
				if( toDrop != null && !( toDrop is BaseArmor ) )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}

				//Drop first valid.
				toDrop = from.FindItemOnLayer( Layer.FirstValid );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}

				//Drop onehand.
				toDrop = from.FindItemOnLayer( Layer.OneHanded );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}
			}
			else if( toEquip.Layer == Layer.FirstValid )
			{
				//Drop first valid.
				toDrop = from.FindItemOnLayer( Layer.FirstValid );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}

				//Drop onehand.
				toDrop = from.FindItemOnLayer( Layer.OneHanded );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}
			}
			else
			{
				//Drop the item that's occupying the layer.
				toDrop = from.FindItemOnLayer( toEquip.Layer );
				if( toDrop != null )
				{
					from.SendAsciiMessage( string.Format( m_DropFormat, ComputeName( toDrop ) ) );
					from.AddToBackpack( toDrop );
				}
			}
            from.EquipItem(toEquip);
            from.PlaySound(0x57);
			return true;
		}
	}
}