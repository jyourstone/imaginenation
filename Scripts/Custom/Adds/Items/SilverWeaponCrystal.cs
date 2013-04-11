namespace Server.Items
{
	public class SilverWeaponCrystal : Item
	{
		[Constructable]
		public SilverWeaponCrystal() : base( 0x1ECD )
		{
			Weight = 1.0;
			Name = "Silver weapon crystal";
			Hue = 2941;
		}

        public SilverWeaponCrystal(Serial serial)
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

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
            {
                BaseWeapon w = BaseWeapon.CreateRandomWeapon();

                while (w is BaseRanged)
                {
                    w.Delete();
                    w = BaseWeapon.CreateRandomWeapon();
                }

                w.Identified = true;
                w.Slayer = SlayerName.Silver;

                w.DamageLevel = (WeaponDamageLevel)Utility.Random(6);

                int roll = Utility.Random(99);
                if (roll < 26) // 25% to get Regular
                    w.AccuracyLevel = WeaponAccuracyLevel.Regular;
                else if (roll < 48) // 22% to get Accurate
                    w.AccuracyLevel = WeaponAccuracyLevel.Accurate;
                else if (roll < 66) // 18% to get Surpassingly
                    w.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                else if (roll < 81) // 15% to get Eminently
                    w.AccuracyLevel = WeaponAccuracyLevel.Eminently;
                else if (roll < 93) // 12% to get Exceedingly
                    w.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
                else if (roll < 100) // 8% to get Supremely
                    w.AccuracyLevel = WeaponAccuracyLevel.Supremely;

                from.AddToBackpack(w);
                Delete();
            }
			else
				from.SendAsciiMessage( "That must be in your pack for you to use it." );
		}
	}
}