using System;


namespace Server.Mobiles
{
	/// <summary>
	/// A talking parrot.
	/// Shows no special abilities until tamed.
	/// Chance to talk when people walk by.
	/// Chance to talk in response to its name (greater chance for its owner).
	/// Chance to talk in response to general chatter (low).
	/// Chance to learn phrases it hears (greater chance to learn from its owner).
	/// May hold 10phrases/words at a time
	/// Has delays between speech and learning to prevent it spamming and to prevent spamming to teach it.
	/// No combat ability, no loot. Can be skinned for meat and feathers.
	/// Eats fruit, vegetables, hay and grain.
	/// </summary>
	[CorpseName( "a parrot corpse" )]
	public class TalkingParrot : BaseCreature
	{
		[Constructable]
		public TalkingParrot() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "Parrot";
			Body = 6;
			BaseSoundID = 191;
			Hue = Utility.RandomRedHue();
			SpeechHue = Utility.RandomDyedHue();

			SetStr( 5 );
			SetDex( 15 );
			SetInt( 5 );

			SetHits( 5 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5 );

			SetSkill( SkillName.MagicResist, 5.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 0;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 55.9;
		}

		private string[] m_phrases = new string[] 
		{
			"Caawwww!", "Arrr Arrr!", "Squawk!", "Wanna cracker!", "Eeek!", "Har har", "Arrr Matey!",
			"Tweet tweet", "Shiver me timbers!", "Ar ar ar!", "Aye aye cap'n!",
		};

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase1
		{ 
			get { return m_phrases[0]; } 
			set { m_phrases[0] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase2
		{ 
			get { return m_phrases[1]; }
			set { m_phrases[1] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase3
		{ 
			get { return m_phrases[2]; }
			set { m_phrases[2] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase4
		{ 
			get { return m_phrases[3]; }
			set { m_phrases[3] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase5
		{ 
			get { return m_phrases[4]; }
			set { m_phrases[4] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase6
		{ 
			get { return m_phrases[5]; }
			set { m_phrases[5] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase7
		{ 
			get { return m_phrases[6]; }
			set { m_phrases[6] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase8
		{ 
			get { return m_phrases[7]; }
			set { m_phrases[7] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase9
		{ 
			get { return m_phrases[8]; }
			set { m_phrases[8] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase10
		{ 
			get { return m_phrases[9]; }
			set { m_phrases[9] = value; }
		}
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public string Phrase11
		{ 
			get { return m_phrases[10]; }
			set { m_phrases[10] = value; }
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( this.Controlled )
			{
				m_AnimalLore = e.Mobile.Skills[ SkillName.AnimalLore ].Value;
				if ( e.Mobile == this.ControlMaster )
				{
					m_LearnBonus = m_AnimalLore/1000; 
					m_SpeakBonus = m_AnimalLore/300; 
					if ( Utility.RandomDouble() < (m_ChanceToSpeakForMaster + m_SpeakBonus) && DateTime.Now - m_LastSpoke > m_SpeakingInterval )
					{
						if ( e.Speech.ToLower().IndexOf( Name.ToLower() ) >= 0 )
						{
							SayRand( m_phrases, this );
							m_LastSpoke = DateTime.Now;
						}
						else if ( Utility.RandomDouble() < 0.2 ) 
						{
							SayRand( m_phrases, this );
							m_LastSpoke = DateTime.Now;
						}
					}
					if ( Utility.RandomDouble() < (m_ChanceToLearnFromMaster + m_LearnBonus) && DateTime.Now - m_LastLearned > m_LearningInterval )
					{
						m_phrases[ Utility.Random( 11 ) ] = e.Speech;
						m_LastLearned = DateTime.Now;
					}
				}
				else
				{
					m_LearnBonus = m_AnimalLore/4000;
					m_SpeakBonus = m_AnimalLore/300;
					if ( Utility.RandomDouble() < (m_ChanceToSpeakForStranger + m_SpeakBonus) && DateTime.Now - m_LastSpoke > m_SpeakingInterval )
					{
						if ( e.Speech.ToLower().IndexOf( Name.ToLower() ) >= 0 )
						{
							SayRand( m_phrases, this );
							m_LastSpoke = DateTime.Now;
						}
						else if ( Utility.RandomDouble() < 0.2 ) 
						{
							SayRand( m_phrases, this );
							m_LastSpoke = DateTime.Now;
						}
					}
					if ( Utility.RandomDouble() < (m_ChanceToLearnFromAnyone + m_LearnBonus) && DateTime.Now - m_LastLearned > m_LearningInterval )
					{
						m_phrases[ Utility.Random( 11 ) ] = e.Speech;
						m_LastLearned = DateTime.Now;
					}
				}

			}
		}

		private static void SayRand( string[] say, Mobile m )
		{
			string words = say[Utility.Random( say.Length )] ;
			if (words != null && words != string.Empty)
				m.Say( words );
		} 

		private double m_SpeakChanceOnMovement = 0.05; 

		private double m_ChanceToLearnFromMaster = 0.1;

		private double m_ChanceToLearnFromAnyone = 0.01; 

		private double m_ChanceToSpeakForMaster = 0.5; 

		private double m_ChanceToSpeakForStranger = 0.1; 

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{                                                   
			if ( m.InRange( this, 2 ) && Controlled )
			{               
				if ( Utility.RandomDouble() < m_SpeakChanceOnMovement && DateTime.Now - m_LastSpoke > m_SpeakingInterval )
				{
					SayRand( m_phrases, this );
					m_LastSpoke = DateTime.Now;
				}
			}
		} 

		private DateTime m_LastLearned;
		private static TimeSpan m_LearningInterval = TimeSpan.FromSeconds( 30 ); 
		private DateTime m_LastSpoke;
		private static TimeSpan m_SpeakingInterval = TimeSpan.FromSeconds( 2 ); 
		
		private double m_AnimalLore = 0.0;
		private double m_SpeakBonus = 0.0;
		private double m_LearnBonus = 0.0;

		public override int Meat{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override int Feathers{ get{ return 5; } }


		public override double GetControlChance( Mobile m, bool useBaseSkill )
		{
			return 1.0;
		}

		public TalkingParrot(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( m_phrases.Length );
			foreach ( string s in m_phrases )
				writer.Write( s );

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int count = reader.ReadInt();
			m_phrases = new string[ count ];

			for ( int i = 0; i < count; i++ )
				m_phrases[ i ] = reader.ReadString();

			int version = reader.ReadInt();
		}
	}
}
