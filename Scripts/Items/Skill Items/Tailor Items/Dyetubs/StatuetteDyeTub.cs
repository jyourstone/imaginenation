using Server.Engines.VeteranRewards;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	public class StatuetteDyeTub : DyeTub, IRewardItem
	{
		public override bool AllowDyables{ get{ return false; } }
		public override bool AllowStatuettes{ get{ return true; } }
		//public override int TargetMessage{ get{ return 1049777; } } // Target the statuette to dye
		//public override int FailMessage{ get{ return 1049778; } } // You can only dye veteran reward statuettes with this tub.
		//public override int LabelNumber{ get{ return 1049741; } } // Reward Statuette Dye Tub
		public override CustomHuePicker CustomHuePicker{ get{ return CustomHuePicker.LeatherDyeTub; } }

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		[Constructable]
		public StatuetteDyeTub()
		{
		    Name = "Statue dye tub";
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(TargetMessage);
                from.Target = new InternalTarget(this);
            }
            else
                from.SendLocalizedMessage(500446);
			/*if ( m_IsRewardItem)// && !RewardSystem.CheckIsUsableBy( from, this, null ) )
				return;

			base.OnDoubleClick( from );*/
		}

        private class InternalTarget : Target
        {
            private readonly StatuetteDyeTub m_Tub;

            public InternalTarget(StatuetteDyeTub tub)
                : base(1, false, TargetFlags.None)
            {
                m_Tub = tub;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item && targeted is IRewardItem)
                {
                    Item item = (Item)targeted;

                    if (item.IsChildOf( from.Backpack ))
                    {
                        item.Hue = m_Tub.DyedHue;
                        from.PlaySound(0x23E);
                    }
                    else
                        from.SendAsciiMessage(983, "That must be in your backpack to dye.");
                }
                else
                    from.SendAsciiMessage(983, "You cannot dye that.");
            }
        }

		public StatuetteDyeTub( Serial serial ) : base( serial )
		{
		}

        /*public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (Core.ML && m_IsRewardItem)
                list.Add(1076221); // 5st Year Veteran Reward
        }*/

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_IsRewardItem = reader.ReadBool();
					break;
				}
			}
		}
	}
}