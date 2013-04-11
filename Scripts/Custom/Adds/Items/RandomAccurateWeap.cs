namespace Server.Items
{
	public class RandomAccWeap : Item
	{
	    [Constructable]
		public RandomAccWeap() : base( 0x1ECD )
		{
			if( DamageLevel == WeaponDamageLevel.Regular )
				DamageLevel = WeaponDamageLevel.Ruin;

			Weight = 1.0;
			Name = "Random accurate weapon of " + DamageLevel.ToString().ToLower();
			Hue = 1960;
		}

		[Constructable]
		public RandomAccWeap( int damageLevel ) : base( 0x1ECD )
		{
            switch (damageLevel)
			{
				case 1:
					DamageLevel = WeaponDamageLevel.Ruin;
					break;
				case 2:
					DamageLevel = WeaponDamageLevel.Might;
					break;
				case 3:
					DamageLevel = WeaponDamageLevel.Force;
					break;
				case 4:
					DamageLevel = WeaponDamageLevel.Power;
					break;
				case 5:
					DamageLevel = WeaponDamageLevel.Vanq;
					break;
				default:
					DamageLevel = WeaponDamageLevel.Ruin;
					break;
			}

			Weight = 1.0;
			Name = "Random accurate weapon of " + DamageLevel.ToString().ToLower();
			Hue = 1960;
		}

		public RandomAccWeap( Serial serial ) : base( serial )
		{
		}

	    public WeaponDamageLevel DamageLevel { get; set; }

	    public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 1 );

			writer.Write( (int)DamageLevel );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch( version )
			{
				case 1:
				{
					( DamageLevel ) = (WeaponDamageLevel)reader.ReadInt();
					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			BaseWeapon w = BaseWeapon.CreateRandomWeapon();
			w.DamageLevel = DamageLevel;
			w.Identified = true;

			if( IsChildOf( from.Backpack ) )
			{
				int roll = Utility.Random( 99 );
				if( roll < 31 ) // 30% to get Accurate
					w.AccuracyLevel = WeaponAccuracyLevel.Accurate;
				else if( roll < 56 ) // 25% to get Surpassingly
					w.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
				else if( roll < 76 ) // 20% to get Eminently
					w.AccuracyLevel = WeaponAccuracyLevel.Eminently;
				else if( roll < 91 ) // 15% to get Exceedingly
					w.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
				else if( roll < 100 ) // 10% to get Supremely
					w.AccuracyLevel = WeaponAccuracyLevel.Supremely;
				else
				{
				}

				from.AddToBackpack( w );
				Delete();
			}
			else if( !IsChildOf( from.Backpack ) )
				from.SendAsciiMessage( "That must be in your pack for you to use it." );
		}
	}
}