namespace Server.Items
{
	public class RandomJewelryDeed : Item
	{
		[Constructable]
		public RandomJewelryDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			Name = "Random jewelry deed";
		}

        public RandomJewelryDeed(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile m )
		{
            Item RewardItem = null;

			if( IsChildOf( m.Backpack ) )
            {
                switch (Utility.Random(11))
                {
                    case 0:
                        {
                            RewardItem = new GoldNecklace();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 1:
                        {
                            RewardItem = new GoldBeadNecklace();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 2:
                        {
                            RewardItem = new SilverNecklace();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 3:
                        {
                            RewardItem = new SilverBeadNecklace();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 4:
                        {
                            RewardItem = new GoldBracelet();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 5:
                        {
                            RewardItem = new SilverBracelet();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 6:
                        {
                            RewardItem = new GoldRing();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 7:
                        {
                            RewardItem = new SilverRing();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 8:
                        {
                            RewardItem = new SilverEarrings();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 9:
                        {
                            RewardItem = new GoldEarrings();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }
                    case 10:
                        {
                            RewardItem = new Necklace();
                            RewardItem.Hue = Utility.RandomList(Sphere.RareHues);
                            break;
                        }   
                }

                m.AddToBackpack(RewardItem);
                Delete();
            }
			else
				m.SendAsciiMessage( "That must be in your pack for you to use it." );
		}
	}
}