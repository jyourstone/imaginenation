using Server.Network;

namespace Server.Items
{
	public class Grog : Item
	{
		private int m_minDamage;
		private int m_maxDamage;
		private int m_PercentToHit;

		[Constructable]
		public Grog() : this( 1 )
		{
		}

		[Constructable]
		public Grog( int amount ) : base( 2503 )
		{
			Stackable = false;
			Weight = 0.1;
			Amount = amount;
			Name = "Bottle of grog";
		}

        public override void OnSingleClick(Mobile from)
        {
            {
                if (Amount > 1)
                    LabelTo(from, Amount + " Bottles of grog");
                else
                    LabelTo(from, "Bottle of grog");
            }
        }

		public Grog( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int minDamage
		{
			get { return m_minDamage; }
			set { m_minDamage = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int maxDamage
		{
			get { return m_maxDamage; }
			set { m_maxDamage = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PercentToHit
		{
			get { return m_PercentToHit; }
			set { m_PercentToHit = value; }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version

			writer.Write( m_minDamage );
			writer.Write( m_maxDamage );
			writer.Write( m_PercentToHit );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
				{
					m_minDamage = reader.ReadInt();
					m_maxDamage = reader.ReadInt();
					m_PercentToHit = reader.ReadInt();
					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

			from.RevealingAction();

			if( m_PercentToHit > Utility.Random( 100 ) )
			{
				BasePotion.PlayDrinkEffect( from );
				from.Say( "Ugh!" );
				from.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
				from.PlaySound( 0x208 );
				from.Damage( Utility.RandomMinMax( m_minDamage, m_maxDamage ), from );
				Consume();
			}
			else
				BasePotion.PlayDrinkEffect( from );

			Consume();
		}
	}
}