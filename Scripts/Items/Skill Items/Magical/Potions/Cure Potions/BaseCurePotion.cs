namespace Server.Items
{
	public class CureLevelInfo
	{
		private readonly Poison m_Poison;
		private readonly double m_Chance;

		public Poison Poison
		{
			get{ return m_Poison; }
		}

		public double Chance
		{
			get{ return m_Chance; }
		}

		public CureLevelInfo( Poison poison, double chance )
		{
			m_Poison = poison;
			m_Chance = chance;
		}
	}

	public abstract class BaseCurePotion : BasePotion
	{
        //Loki/Rob edit
        public override double PotionDelay { get { return 5.0; } }

		public abstract CureLevelInfo[] LevelInfo{ get; }

		public BaseCurePotion( PotionEffect effect ) : base( 0xF07, effect )
		{
		}

		public BaseCurePotion( Serial serial ) : base( serial )
		{
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

		public void DoCure( Mobile from )
		{
			bool cure = false;

			CureLevelInfo[] info = LevelInfo;

			for ( int i = 0; i < info.Length; ++i )
			{
				CureLevelInfo li = info[i];

				if ( li.Poison == from.Poison && Scale( from, li.Chance ) > Utility.RandomDouble() )
				{
					cure = true;
					break;
				}
			}

			if ( cure && from.CurePoison( from ) )
			{
				from.SendLocalizedMessage( 500231 ); // You feel cured of poison!

                from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
				from.PlaySound( 0x1E0 );
			}
			else if ( !cure && from.Poisoned ) //Loki edit: It shouldn't say this if you're not actually poisoned (added second condition)
			{
				from.SendLocalizedMessage( 500232 ); // That potion was not strong enough to cure your ailment!
			}
		}

		public override void Drink( Mobile from )
		{
			if ( Spells.TransformationSpellHelper.UnderTransformation( from, typeof( Spells.Necromancy.VampiricEmbraceSpell ) ) )
			{
				from.SendLocalizedMessage( 1061652 ); // The garlic in the potion would surely kill you.
			}
            else if (CanDrink(from))
			{
				DoCure( from );
				PlayDrinkEffect( from, this );
			}
		}
	}
}