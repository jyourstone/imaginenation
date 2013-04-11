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
using Server.Items;
using Server.Mobiles;

namespace Server.INXHairStylist
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        public readonly string Description;

        public DescriptionAttribute(string description)
        {
            Description = description.Trim();
        }
    }

    public class INXHairStylist : BaseVendor
    {
        public enum BuyStyle
        {
            BuyMenu,
            BuyMenuAndSpeech,
            GumpMenu,
            GumpMenuAndSpeech,
            SpeechOnly,
        }

        public enum MovementOptions
        {
            AllowMove,
            AllowMoveWithinMaxDistance,
            DisallowMove
        }

        #region Don't change
        //If the hairStylist has a specific chair
        private Point3D m_StylistChairLoc = Point3D.Zero;
        private CutTimer m_CutTimer;

        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        private static readonly Type[] typeofNormalHair = new Type[]
		{
			typeof( ShortHair ),
			typeof( LongHair ),
			typeof( PonyTail ),
			typeof( Mohawk ),
			typeof( PageboyHair ),
			typeof( BunsHair ),
			typeof( Afro ),
            typeof( ReceedingHair ),
			typeof( TwoPigTails ),
			typeof( KrisnaHair )
		};

        private static readonly Type[] typeofSpecialHair = new Type[]
		{
			typeof( ShortHair ),
			typeof( LongHair ),
			typeof( PonyTail ),
			typeof( Mohawk ),
			typeof( PageboyHair ),
			typeof( BunsHair ),
			typeof( Afro ),
            typeof( ReceedingHair ),
			typeof( TwoPigTails ),
			typeof( KrisnaHair )
		};

        private static readonly Type[] typeofNormalBeard = new Type[]
		{
			typeof( ShortBeard ),
			typeof( LongBeard ),
			typeof( MediumShortBeard ),
			typeof( MediumLongBeard ),
			typeof( Vandyke ),
			typeof( Goatee ),
			typeof( Mustache )
		};

        private static readonly Type[] typeofSpecialBeard = new Type[]
		{
			typeof( ShortBeard ),
			typeof( LongBeard ),
			typeof( MediumShortBeard ),
			typeof( MediumLongBeard ),
			typeof( Vandyke ),
			typeof( Goatee ),
			typeof( Mustache )
		};

        private SortedDictionary<string, Type> m_StyleItems = new SortedDictionary<string, Type>();

        #endregion

        #region Speech messages
        //Seriously, someone change these MSGS
        private readonly string[] m_TooManyStylesBought = new string[] { "You will have to settle for one hair and beard style!" };
        private readonly string[] m_FemaleBuyingBeard = new string[] { "I'm sorry, but I only know how to style men faces." };
        private readonly string[] m_HairCutDone = new string[] { "Ahh! Just look in the mirror!" };
        private readonly string[] m_PleaseDismount = new string[] { "I cannot reach your head if you are sitting on your horse." };
        private readonly string[] m_GoPeace = new string[] { "I would not want to slip while you are in your battle stance, please go out of warmode." };
        private readonly string[] m_ToFarAway = new string[] { "Please... My arms are not that long." };
        private readonly string[] m_DirectionChanged = new string[] { "Are you affraid of the scissors?" };
        private readonly string[] m_CantStyleGhosts = new string[] { "Sorry, I do not cut dead people." };
        private readonly string[] m_RanAway = new string[] { "Come back when you've gathered enough courage for a haircut." };
        private readonly string[] m_InCombatResponse = new string[] { "Quit trying to talk your way out!", "Quit speaking and fight, scum!" };
        private readonly string[] m_InCombatWithYouResponse = new string[] { "Quit trying to talk your way out!", "Quit speaking and fight, scum!" };
        private readonly string[] m_BusyResponse = new string[] { "I'm busy right now, please stand in line." };
        private readonly string[] m_CutInterrupted = new string[] { "I'm sorry but I will have to take a rain check, a little busy right now." };
        private readonly string[] m_NoStyleFound = new string[] { "That is not something that I know of.", "Never heard about that style, try the sheep herder" };
        private readonly string[] m_BadBuyString = new string[] { "I'm a little confused you'll just get your first choice." };
        private readonly string[] m_NotHuman = new string[] { "I'm sorry but I don't know how to cut the hair on your kind" };

        private string m_ErrorMessage = "Please notify the admin on how you got this message";
        #endregion

        #region Setting/Config variables

        private readonly string[] m_BuyKeywords = new string[] { "cut", "hair" };

        private int m_MaxDistanceForCut = 5;

        //Sets how often the cut animation (playing scissors sound and dropping hair) should be displayed
        private TimeSpan m_CutAnimInterval = TimeSpan.Zero;

        //The amount of time it will take to cut someones hair
        private TimeSpan m_CutTime = TimeSpan.Zero;

        //Should we use vendors with the shop menu
        private bool m_AllowFemalesBuyingBeard = true;
        private bool m_PlayScissorSound = true;
        private bool m_DropResidueHair = true;
        private bool m_NeedsToBeDismounted = false;
        private bool m_FreezeCustomer = false;
        private bool m_DropHairWhenBald = false;
        private bool m_NeedsPeaceForCut = false;
        private bool m_UseExtensionSystem = false;
        private bool m_HurtOnMove = false;

        private MovementOptions m_MovementOption = MovementOptions.AllowMove;
        private BuyStyle m_VendorBuyMethod = BuyStyle.BuyMenu;

        private List<string> m_HairAndBeardNames = new List<string>();

        #endregion

        #region Properties

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("The stylist chair location is set if you want the custom to be on a specific location while being cut. Target a chair to force the player to sit there.")]
        public Point3D StylistChairLoc
        {
            get { return m_StylistChairLoc; }
            set { m_StylistChairLoc = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("The amount of time that the hair stylist will take to cut a customers hair.")]
        public TimeSpan CutTime
        {
            get { return m_CutTime; }
            set { m_CutTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets the amount of time that goes in between each cut animation. The animation consist of scissor sounds and dropping hair residue.")]
        public TimeSpan CutAnimInterval
        {
            get { return m_CutAnimInterval; }
            set { m_CutAnimInterval = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("The maxiumum amount of tiles that a player can walk while being cut, if \"MovementOption\" allows it. This is also the max allowed buy distance.")]
        public int MaxDistanceForCut
        {
            get { return m_MaxDistanceForCut; }
            set { m_MaxDistanceForCut = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not females will be allowed to buy beard.")]
        public bool AllowFemalesBuyingBeard
        {
            get { return m_AllowFemalesBuyingBeard; }
            set { m_AllowFemalesBuyingBeard = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not the scissor sounds will be played when cutting animation are issued.")]
        public bool PlayScissorSound
        {
            get { return m_PlayScissorSound; }
            set { m_PlayScissorSound = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not the scissor sounds will be played when cutting animation are issued.")]
        public bool DropResidueHair
        {
            get { return m_DropResidueHair; }
            set { m_DropResidueHair = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not a player is allowed to be mounted when buying a hair cut and while being cut.")]
        public bool NeedsToBeDismounted
        {
            get { return m_NeedsToBeDismounted; }
            set { m_NeedsToBeDismounted = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not the hair stylist will freeze the customer while cutting their hair.")]
        public bool FreezeCustomer
        {
            get { return m_FreezeCustomer; }
            set { m_FreezeCustomer = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not a customer needs to be out of warmode when buying hair and while being cut.")]
        public bool NeedsPeaceForCut
        {
            get { return m_NeedsPeaceForCut; }
            set { m_NeedsPeaceForCut = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("TODO NOTHING YET")]
        public bool UseExtensionSystem
        {
            get { return m_UseExtensionSystem; }
            set { m_UseExtensionSystem = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("The movement rules the customer has to ahead too. You can choose to ignore all customer movement, ignore within \"MaxDistanceForCut\" and disallow all types.")]
        public MovementOptions MovementOption
        {
            get { return m_MovementOption; }
            set { m_MovementOption = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("The ruleset on how to obtain a styling. Speech only, where the customer has to name the hairstyle, the buy menu, a custom gump and a mix of all three.")]
        public BuyStyle VendorBuyMethod
        {
            get { return m_VendorBuyMethod; }
            set 
            {
                if (value == BuyStyle.BuyMenuAndSpeech || value == BuyStyle.GumpMenuAndSpeech || value == BuyStyle.SpeechOnly)
                {
                    //Only build the style string list if this is you go from a non speech system to a speech system
                    //or if your style string list is empty
                    if ( m_VendorBuyMethod != BuyStyle.BuyMenuAndSpeech && m_VendorBuyMethod != BuyStyle.GumpMenuAndSpeech && m_VendorBuyMethod != BuyStyle.SpeechOnly)
                        BuildStyleNames();
                    else if ( m_HairAndBeardNames.Count <= 0 )
                        BuildStyleNames();
                }

                m_VendorBuyMethod = value; 
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        [Description("Sets weather or not the hair stylist will hurt you when you move. Having this enabled will play a hurt sound and drop some blood.")]
        public bool HurtOnMove
        {
            get { return m_HurtOnMove; }
            set { m_HurtOnMove = value; }
        }

        #endregion

        private bool AllowSpeechBuying
        {
            get { return (VendorBuyMethod == BuyStyle.SpeechOnly || 
            VendorBuyMethod == BuyStyle.BuyMenuAndSpeech || 
            VendorBuyMethod == BuyStyle.GumpMenuAndSpeech); }
        }

        [Constructable]
        public INXHairStylist() : base("the hair stylist")
        {
            SetSkill(SkillName.Tailoring, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 110.0);
            SetSkill(SkillName.TasteID, 85.0, 100.0);
        }

        public INXHairStylist(Serial serial) : base(serial)
        {
        }

        private void BuildStyleNames()
        {
            m_HairAndBeardNames = new List<string>();
            m_StyleItems = new SortedDictionary<string, Type>();

            foreach(SBINXHairStylist sbi in m_SBInfos)
                foreach (GenericBuyInfo gbi in sbi.BuyInfo)
                {
                    string toReturn = string.Empty;

                    if (!string.IsNullOrEmpty(gbi.Name))
                        toReturn = gbi.Name;
                    else
                    {
                        string tempName = gbi.Type.ToString();

                        foreach (char c in tempName)
                        {
                            toReturn += c;

                            if (char.IsUpper(c))
                                toReturn += " ";
                        }
                    }

                    toReturn = toReturn.ToLower();

                    m_StyleItems.Add(toReturn, gbi.Type);
                    m_HairAndBeardNames.Add(toReturn);
                }
        }
        
        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBINXHairStylist());
        }

        private Item StyleItemFromString(string s)
        {
            if (m_StyleItems.ContainsKey(s))
                return (Item)Activator.CreateInstance(m_StyleItems[s]);
            else 
                return null;
        }

        public override bool OnBuyItems(Mobile buyer, List<BuyItemResponse> list)
        {
            List<Item> styles = new List<Item>();

            foreach (BuyItemResponse o in list)
            {
                Item item = World.FindItem(o.Serial);

                if (item != null)
                    styles.Add(item);
            }

            if (!CanBuy(buyer, styles))
                return false;

            int totalCost = GetOrderCost(styles);

            UpdateBuyInfo();

            //Does RunUO already have a similar method?
            bool bought = ConsumeBackpackAndBankGold(buyer, totalCost);

            if (!bought)
            {
                SayTo(buyer, true, "Alas, thou dost not possess sufficient gold for this purchase!");
                return false;
            }

            //Start the cutting process if everything is ok
            StartCutting(styles, buyer);

            return true;
        }

        private GenericBuyInfo GetGBI(object obj)
        {
            IBuyItemInfo[] buyInfo = GetBuyInfo();

            for (int i = 0; i < buyInfo.Length; ++i)
            {
                GenericBuyInfo gbi = (GenericBuyInfo)buyInfo[i];

                if (gbi.GetDisplayEntity() == obj)
                    return gbi;
            }

            return null;
        }

        private int GetOrderCost(List<Item> styles)
        {
            int orderCost = 0;

            foreach (Item item in styles)
            {
                GenericBuyInfo gbi = GetGBI(item);

                if (gbi != null)
                    orderCost += gbi.Price;
            }

            return orderCost;
        }

        private void OnSpeechBuy(Mobile buyer, List<Item> styles)
        {
            if (!CanBuy(buyer, styles))
                return;

            int totalCost = GetOrderCost(styles);

            //Does RunUO already have a similar method?
            bool bought = ConsumeBackpackAndBankGold(buyer, totalCost);

            if (!bought)
            {
                SayTo(buyer, true, "Alas, thou dost not possess sufficient gold for this purchase!");
                return;
            }

            //Start the cutting process if everything is ok
            StartCutting(styles, buyer);

            return;
        }

        private bool CanBuy(Mobile buyer, List<Item> styles)
        {
            if (!IsActiveSeller)
                return false;

            if (!buyer.CheckAlive())
                return false;

            if (!CheckVendorAccess(buyer))
            {
                Say(501522); // I shall not treat with scum like thee!
                return false;
            }

            if (styles.Count == 0)
            {
                SayTo(buyer, 500187); // Your order cannot be fulfilled, please try again.
                return false;
            }

            if (Math.Round(buyer.GetDistanceToSqrt(this)) > m_MaxDistanceForCut)
            {
                Speak(m_ToFarAway);
                return false;
            }
            if (buyer.Mounted && m_NeedsToBeDismounted)
            {
                Speak(m_PleaseDismount);
                return false;
            }
            if (buyer.Warmode && m_NeedsPeaceForCut)
            {
                Speak(m_GoPeace);
                return false;
            }
            if (buyer.HairItemID == 5147 || buyer.HairItemID == 7947)
            {
                Speak(m_NotHuman);
                return false;
            }

            int beardCount = 0, hairCount = 0;

            foreach (Item item in styles)
            {
                if (IsHair(item))
                    hairCount++;
                else if (IsBeard(item))
                {
                    if (buyer.Female && !m_AllowFemalesBuyingBeard)
                    {
                        Speak(m_FemaleBuyingBeard);
                        return false;
                    }

                    beardCount++;
                }

                if (hairCount > 1 || beardCount > 1)
                {
                    Speak(m_TooManyStylesBought);
                    return false;
                }
            }

            return true;
        }

        public void Speak(string toSay)
        {
            Say(toSay);
        }

        public void Speak(string[] toSay)
        {
            Say(toSay[Utility.Random(toSay.Length)]);
        }

        private bool IsHair(Item item)
        {
            Type t = item.GetType();

            for (int i = 0; i < typeofNormalHair.Length; i++)
                if (t == typeofNormalHair[i])
                    return true;

            for (int i = 0; i < typeofSpecialHair.Length; i++)
                if (t == typeofSpecialHair[i])
                    return true;

            return false;
        }

        private bool IsBeard(Item item)
        {
            Type t = item.GetType();

            for (int i = 0; i < typeofNormalBeard.Length; i++)
                if (t == typeofNormalBeard[i])
                    return true;

            for (int i = 0; i < typeofSpecialBeard.Length; i++)
                if (t == typeofSpecialBeard[i])
                    return true;

            return false;
        }

        private void StartCutting(List<Item> styles, Mobile customer)
        {
            if (m_CutTime == TimeSpan.Zero)
                CutHairInstant(styles, customer);
            else
            {
                if (m_FreezeCustomer)
                    customer.Frozen = true;

                m_CutTimer = new CutTimer(customer, this, styles, (int)(m_CutTime.TotalMilliseconds * 0.004));
                m_CutTimer.Start();
            }
        }

        private void CutHairInstant(List<Item> styles, Mobile customer)
        {
            bool baldCustomer = false;

            if (customer.HairItemID == 0)
            {
                baldCustomer = true;
                customer.HairHue = 0;
            }

            //Display cutting anitmations if "you are not bald" or if "cutting animations are enabled for bald people"
            //or if "you are bald and both cutting anitmations for bald people and hair extentions are disabled"
            if (!baldCustomer || (baldCustomer && (m_DropHairWhenBald || !m_UseExtensionSystem)))
            {
                if (m_PlayScissorSound)
                    PlayScissors(customer);

                if (m_DropResidueHair)
                {
                    if (Utility.RandomDouble() >= 0.5)
                        DropHairResidue(customer, false);
                    else
                        DropHairResidue(customer, Location, false);
                }
            }

            CutHair(styles, customer);
        }

        public void CutHair(List<Item> styles, Mobile customer)
        {
            foreach (Item style in styles)
            {
                if (IsHair(style))
                {
                    customer.HairItemID = style.ItemID;
                    style.Delete();
                }
                else if (IsBeard(style))
                {
                    customer.FacialHairItemID = style.ItemID;
                    style.Delete();
                }            
            }

            Speak(m_HairCutDone);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
                from.SendGump(new ConfigSelectionGump(from, this));
            else
                base.OnDoubleClick(from);
        }

        public void PlayScissors(Mobile customer)
        {
            customer.PlaySound(0x248);
        }

        public void DropHairResidue(Mobile customer)
        {
            DropHairResidue(customer, customer.Location, true);
        }

        public void DropHairResidue(Mobile customer, bool moreThanOnce)
        {
            DropHairResidue(customer, customer.Location, moreThanOnce);
        }

        public void DropHairResidue(Mobile customer, Point3D loc, bool moreThanOnce)
        {
            int hairPiles = moreThanOnce ? Utility.Random(1, 3) : 1;

            for (int i = 0; i < hairPiles; i++)
            {
                new HairResidue(customer).MoveToWorld(new Point3D(
                    loc.X + Utility.RandomMinMax(-1, 1),
                    loc.Y + Utility.RandomMinMax(-1, 1),
                    loc.Z), customer.Map);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 

            writer.Write(m_StylistChairLoc);
            writer.Write(m_CutAnimInterval);
            writer.Write(m_CutTime);
            writer.Write(m_AllowFemalesBuyingBeard);
            writer.Write(m_PlayScissorSound);
            writer.Write(m_DropResidueHair);
            writer.Write(m_NeedsToBeDismounted);
            writer.Write(m_FreezeCustomer);
            writer.Write(m_DropHairWhenBald);
            writer.Write(m_NeedsPeaceForCut);
            writer.Write(m_UseExtensionSystem);
            writer.Write(m_HurtOnMove);
            writer.Write((int)m_MovementOption);
            writer.Write((int)m_VendorBuyMethod);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_StylistChairLoc = reader.ReadPoint3D();
            m_CutAnimInterval = reader.ReadTimeSpan();
            m_CutTime = reader.ReadTimeSpan();
            m_AllowFemalesBuyingBeard = reader.ReadBool();
            m_PlayScissorSound = reader.ReadBool();
            m_DropResidueHair = reader.ReadBool();
            m_NeedsToBeDismounted = reader.ReadBool();
            m_FreezeCustomer = reader.ReadBool();
            m_DropHairWhenBald = reader.ReadBool();
            m_NeedsPeaceForCut = reader.ReadBool();
            m_UseExtensionSystem = reader.ReadBool();
            m_HurtOnMove = reader.ReadBool();
            m_MovementOption = (MovementOptions)reader.ReadInt();
            m_VendorBuyMethod =(BuyStyle)reader.ReadInt();

            if (m_VendorBuyMethod == BuyStyle.BuyMenuAndSpeech || m_VendorBuyMethod == BuyStyle.GumpMenuAndSpeech || m_VendorBuyMethod == BuyStyle.SpeechOnly)
                BuildStyleNames();
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (m_CutTimer != null)
            {
                m_CutTimer.EndTimer();
                Speak(m_CutInterrupted);
            }

            base.OnDamage(amount, from, willKill);
        }

        public override void OnCombatantChange()
        {
            if (m_CutTimer != null)
            {
                m_CutTimer.EndTimer();
                Speak(m_CutInterrupted);
            }

            base.OnCombatantChange();
        }

        public override void OnDeath(Container c)
        {
            if (m_CutTimer != null)
                m_CutTimer.EndTimer();

            base.OnDeath(c);
        }

        public override void OnDelete()
        {
            if (m_CutTimer != null)
                m_CutTimer.EndTimer();

            base.OnDelete();
        }

        #region AI Overrides

        public override bool HandlesOnSpeech(Mobile from)
        {
            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string mobileSpeechLowered = string.Empty;

            //Holds weather or not the vendor has been requested by a player
            bool hasRequestedVendor = false;

            //Only look for keywords if the speech still isn't handled
            if (!e.Handled)
            {
                mobileSpeechLowered = e.Speech.ToLower();

                hasRequestedVendor = (!AIObject.NamedInRange(from, e.Speech) && (e.HasKeyword(0x171) || e.HasKeyword(0x171))) || AIObject.WasNamed(e.Speech);

                //Look for custom speech triggers if the vendor hasn't been named and if the keywords haven't been specified.
                if (!hasRequestedVendor)
                    for (int i = 0; i < m_BuyKeywords.Length; i++)
                    {
                        if (mobileSpeechLowered.Contains(m_BuyKeywords[i]))
                        {
                            hasRequestedVendor = true;
                            break;
                        }
                    }
            }
            else
                return;

            //Jump out if the vendor hasn't been requeted
            if (!hasRequestedVendor)
                return;

            //The barber will always react in some manner when we are passed this point
            e.Handled = true;

            //The barber is cutting someone
            if (m_CutTime != TimeSpan.Zero && m_CutTimer != null)
            {
                Speak(m_BusyResponse);
                return;
            }
            else if (InRange(from, m_MaxDistanceForCut))
            {
                if (Combatant != null && Combatant != e.Mobile)
                {
                    Speak(m_InCombatResponse);
                    return;
                }
                else if (Combatant != null && Combatant == e.Mobile)
                {
                    Speak(m_InCombatWithYouResponse);
                    return;
                }

                if (FocusMob != from)
                    FocusMob = from;

                //Buy if the mobile was named or if the player provided the buy keyword and another mobile wasnt named
                if (AIObject.WasNamed(e.Speech) || !AIObject.NamedInRange(from, e.Speech))
                {
                    //Checks if the stylist can cut hair and barbers through speech [string] buying.
                    if (AllowSpeechBuying)
                    {
                        List<Item> styles = new List<Item>();
                        bool onlyBuyOne = (!mobileSpeechLowered.Contains("&") && !mobileSpeechLowered.Contains("and"));

                        foreach (string s in m_HairAndBeardNames)
                            if (mobileSpeechLowered.Contains(s))
                            {
                                Item styleItem = StyleItemFromString(s);

                                if (styleItem != null)
                                    if (styles.Count == 1 && onlyBuyOne)
                                    {
                                        Speak(m_BadBuyString);

                                        OnSpeechBuy(from, styles);
                                        return;
                                    }
                                    else
                                        styles.Add(styleItem);
                            }

                        // Try to buy the items if any styles have been selected
                        if (styles.Count != 0)
                        {
                            OnSpeechBuy(from, styles);
                            return;
                        }
                        else if (VendorBuyMethod == BuyStyle.SpeechOnly)
                        {   // Stop the buying if we haven't named a style and if the vendor doesnt accept any other buy method
                            // otherwise just continiue to the other buy menus.
                            Speak(m_NoStyleFound);
                            return;
                        }
                    }

                    if (e.HasKeyword(0x171)) // *buy*
                    {
                        if (VendorBuyMethod == BuyStyle.BuyMenuAndSpeech || VendorBuyMethod == BuyStyle.BuyMenu)
                            VendorBuy(from);
                        else if (VendorBuyMethod == BuyStyle.GumpMenuAndSpeech || VendorBuyMethod == BuyStyle.GumpMenu)
                        {
                            Speak("A BUY GUMP");
                        }
                    }
                    //Check if we can buy a hair style through the buy/sell menu or gump.
                    else if (e.HasKeyword(0x177)) // *sell*
                    {
                        if (VendorBuyMethod == BuyStyle.BuyMenuAndSpeech || VendorBuyMethod == BuyStyle.BuyMenu)
                            VendorSell(from);
                        else if (VendorBuyMethod == BuyStyle.GumpMenuAndSpeech || VendorBuyMethod == BuyStyle.GumpMenu)
                        {
                            Speak("A SELL GUMP");
                        }
                    }
                }
            }
            else
            {
                if (FocusMob != from)
                    FocusMob = from;

                Speak(m_ToFarAway);
            }
        }

        #endregion

        private class CutTimer : Timer
        {
            private readonly Mobile m_Customer;
            private readonly INXHairStylist m_HairStylist;
            private readonly List<Item> m_Styles;
            private int m_CurrentTick = 1;
            private readonly int m_MaxTicks;
            private readonly Point3D m_StartingLocation;
            private Point3D m_LastLocation;
            private Direction m_LastDirection;
            private readonly bool m_BaldCustomer;
            private DateTime m_NextCutAnim = DateTime.Now;

            public CutTimer(Mobile customer, INXHairStylist hairStylist, List<Item> styles, int ticks)  : base(TimeSpan.Zero, TimeSpan.FromMilliseconds(250.0), ticks)
            {
                m_Customer = customer;
                m_HairStylist = hairStylist;
                m_Styles = styles;
                m_MaxTicks = ticks;
      
                m_HairStylist.FocusMob = m_Customer;

                //If the hair stylist has a chair/item assosiated with it, then that will be the location for the player to "sit".
                if (m_HairStylist.StylistChairLoc != Point3D.Zero)
                    m_StartingLocation = m_LastLocation = m_HairStylist.StylistChairLoc;
                else
                    m_StartingLocation = m_LastLocation = m_Customer.Location;

                m_LastDirection = m_Customer.Direction;

                if (m_Customer.HairItemID == 0)
                {
                    m_BaldCustomer = true;
                    m_Customer.HairHue = 0;
                }

                if (m_HairStylist.FreezeCustomer)
                    m_Customer.Frozen = true;
            }

            protected override void OnTick()
            {
                base.OnTick();

                if (m_Customer.Mounted && m_HairStylist.m_NeedsToBeDismounted)
                { //End the cutting if "the customer is mounted and that's not allowed"
                    m_HairStylist.Speak(m_HairStylist.m_PleaseDismount);
                    EndTimer();
                }
                else if (m_Customer.Warmode && m_HairStylist.m_NeedsPeaceForCut)
                { //End the cutting if "the cusomer has gone into warmode and that's not allowed"
                    m_HairStylist.Speak(m_HairStylist.m_GoPeace);
                    EndTimer();
                }
                else if (!m_Customer.Alive)
                { //End the cutting if "the customer is dead"
                    m_HairStylist.Speak(m_HairStylist.m_CantStyleGhosts);
                    EndTimer();
                }
                else if (m_Customer.HairItemID == 5147 || m_Customer.HairItemID == 7947)
                { //End the cutting if "the custom is goblin or daemon"
                    m_HairStylist.Speak(m_HairStylist.m_NotHuman);
                    EndTimer();
                }

                //Hurt and stop the cutting if "the person has moved from the starting location and moving is not allowed" 
                //or if "if you are allowed to move but distance from the vendor has overexceeded the allowed maximum"
                if (m_Customer.Location != m_StartingLocation && m_HairStylist.MovementOption == MovementOptions.DisallowMove || (m_HairStylist.MovementOption == MovementOptions.AllowMoveWithinMaxDistance && Math.Round(m_Customer.GetDistanceToSqrt(m_HairStylist)) > m_HairStylist.MaxDistanceForCut))
                {
                    m_HairStylist.Speak(m_HairStylist.m_RanAway);

                    if (m_HairStylist.m_HurtOnMove)
                        Hurt();

                    EndTimer();
                }

                //Hurt you if "you have looked in a different directin since last check" 
                //or if "you have moved since last check"
                if (m_HairStylist.MovementOption != MovementOptions.AllowMove && (m_Customer.Location != m_LastLocation || m_LastDirection != m_Customer.Direction))
                {
                    m_LastLocation = m_Customer.Location;
                    m_LastDirection = m_Customer.Direction;

                    m_HairStylist.Speak(m_HairStylist.m_DirectionChanged);

                    if (m_HairStylist.m_HurtOnMove)
                        Hurt();        
                }

                //Display the cutting animations if "it has passed x amount of time since last cutting interval" 
                //or if "we are on the last tick and there is less than 1/4th left to next interval"
                if ((DateTime.Now > m_NextCutAnim && m_HairStylist.m_CutAnimInterval != TimeSpan.Zero ) 
                    || (m_CurrentTick == m_MaxTicks && m_NextCutAnim < (DateTime.Now + TimeSpan.FromMilliseconds(m_HairStylist.m_CutAnimInterval.TotalMilliseconds * 0.001))))
                {
                    //Display cutting anitmations if "you are not bald" or if "cutting animations are enabled for bald people"
                    //or if "you are bald and both cutting anitmations for bald people and hair extentions are disabled"
                    if (!m_BaldCustomer || (m_BaldCustomer && (m_HairStylist.m_DropHairWhenBald || !m_HairStylist.m_UseExtensionSystem)))
                    {
                        if (m_HairStylist.m_PlayScissorSound)
                            m_HairStylist.PlayScissors(m_Customer);

                        if (m_HairStylist.m_DropResidueHair)
                        {
                            if (Utility.RandomDouble() >= 0.5)
                                m_HairStylist.DropHairResidue(m_Customer, false);
                            else
                                m_HairStylist.DropHairResidue(m_Customer, m_HairStylist.Location, false);
                        }
                    }
                    else if (m_HairStylist.m_UseExtensionSystem)
                    {
                        m_HairStylist.Speak("ADDING EXTENTIONS");
                    }
                    else
                        m_HairStylist.Speak(m_HairStylist.m_ErrorMessage);

                    m_NextCutAnim = DateTime.Now + m_HairStylist.m_CutAnimInterval;
                }

                //Cut the hair and stop the timer if "this is the last time the timer is ran"
                if (m_CurrentTick == m_MaxTicks)
                {
                    m_HairStylist.CutHair(m_Styles, m_Customer);
                    EndTimer();
                }

                m_CurrentTick++;
            }

            public void EndTimer()
            {
                m_Customer.Frozen = false;

                m_HairStylist.m_CutTimer = null;
                m_HairStylist.FocusMob = null;

                Stop();
            }

            private void Hurt()
            {
                if (m_Customer.Hits > 1)
                {
                    int hurtSound = 0;

                    if (m_Customer.Body.IsMale)
                        hurtSound =  Utility.RandomList(340, 341, 342, 343, 344, 345);
                    else if (m_Customer.Body.IsFemale)
                        hurtSound =  Utility.RandomList(331, 332, 333, 334, 335);

                    if ( hurtSound != 0)
                        m_Customer.PlaySound(m_Customer.GetHurtSound());

                    m_Customer.Hits--;
                    new Blood().MoveToWorld(m_Customer.Location, m_Customer.Map);
                }
            }
        }
    }
}