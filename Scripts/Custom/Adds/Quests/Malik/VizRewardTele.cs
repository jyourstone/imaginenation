using Server.Engines.Plants;
using Server.Engines.Quests.Doom;

namespace Server.Items
{
    public class SeedRewardTele : Teleporter
    {
        public override void StartTeleport(Mobile m)
        {
            Item RewardItem = null;

            switch (Utility.Random(4))
            {
                case 1:
                    {
                        RewardItem = new WyrmsHeart();
                        RewardItem.Amount = 50;
                        break;
                    }
                case 2:
                    {
                        RewardItem = new Seed(); break;
                    }
                case 3:
                    {
                        RewardItem = new GoldenSkull();
                        RewardItem.Hue = 0x793;
                        RewardItem.Name = "Skull of Iniquity";
                        RewardItem.LootType = LootType.Regular;

                        break;
                    }
                default:
                    {
                        Item Reward = new Log();
                        Reward.Amount = 750;
                        RewardItem = new CommodityDeed(Reward);
                        break;
                    }
            }

            m.AddToBackpack(RewardItem);
            DoTeleport(m);
        }

        [Constructable]
		public SeedRewardTele()
		{
		}

        public SeedRewardTele(Serial serial)
            : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
