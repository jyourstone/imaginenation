using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Accounting; 
using Server.Multis;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
    public class TreasureScroll : LockableContainer
    {
        private static readonly int[] m_ItemIDs = new int[]
		{                                       //removed until the issue is resolved where these scrolls give targets..
			3636,3637,3638,3639,3640,3641,3642,/*3827,3828,3829,3830,3831,3832,3833,*/7981,7982,7983,7984,7985,7986,7987,7988,7989,7990,7991,7992,7993,7994,7995,7996,7997,7998,7999,8000,8001,8002,8003,8004,8005,8006,8007,8008,8009,8010,8011,8012,8013,8014,8015,8016,8017,8018,8019,8020,8021,8022,8023,8024,8025,8026,8027,8028,8029,8030,8031,8032,8033,8034,8035,8036,8037,8038,8039,8040,8041,8042,8043,8044,8045,8046,8047,8048,8049,8050,8805,8806,8807,8808,8809,8810,8811,8812,8813,8814,8815,8816,8817,8818,8819,8820,8821,8822,8823,8824,8825,8826,8827,8828,11601,11602,11603,11604,11605,11606,11607,11608,11609,11610,11611,11612,11613,11614,11615,11616
		};

        [Constructable]
        public TreasureScroll() : base(Utility.RandomList( m_ItemIDs ))
        {
            Name = "Treasure scroll";
            GumpID = 0x3c;
            Movable = true;
            Visible = true;
            Hue = Utility.RandomList(Sphere.RareHues);
            LootType = LootType.Regular;
            
        }

        public TreasureScroll(Serial serial) : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                from.SendAsciiMessage("Drop items in here for players to receive.");
                DisplayTo(from);
                return;
            }
            if (!IsChildOf(from.Backpack))
            {
                from.SendAsciiMessage("The scroll must be in your backpack when used.");
                return;
            }

            Internalize(); //So you don't get overweight
            Item[] items = FindItemsByType(typeof (Item), true);
            bool inBank = false;
            foreach (Item item in items)
            {
                if (!from.AddToBackpack(item))
                {
                    from.BankBox.DropItem(item);
                    inBank = true;
                }
            }

            from.SendAsciiMessage("The scroll summons treasures into your pack as you unravel it!");
            if (inBank)
                from.SendAsciiMessage("You are overweight, the treasures were added to your bank");

            Delete();
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