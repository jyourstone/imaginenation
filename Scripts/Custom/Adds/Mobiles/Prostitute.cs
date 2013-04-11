using System;

namespace Server.Mobiles
{
	/// <summary>
	/// Pretty straightforward, human female, responds to keywords, uses less system resources than previous one (timers) 
	/// Spawns with random attire and names.
	/// Work on replies
	/// </summary>
	public class Prostitute : BaseCreature
	{
		[Constructable]
		public Prostitute() : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.4, 0.8  )
		{
			Body = 0x191;
			Name = NameList.RandomName( "female" );
			Title = "the prostitute";
		    Karma = 200;

			SetStr( 10, 20 );
			SetDex( 500 );
			SetInt( 15, 25 );

			Hue = Utility.RandomSkinHue();

			int n = Utility.RandomList( 2987, 2941, 2981, 2980, 2995, 2994, 2975, 2990, 2972, 2977 );

			Item onfeet = new Item( Utility.RandomList( 0x1711, 0x170D, 0x170B ) );
			onfeet.Hue = n;
			onfeet.Movable = false;
			onfeet.Layer = Layer.Shoes;
			AddItem( onfeet );

			Item onchest = new Item( 0x1C0A );
			onchest.Hue = n;
			onchest.Movable = false;
			onchest.Layer = Layer.InnerTorso;
			AddItem( onchest );

			Item onbody = new Item( Utility.RandomList( 0x1C00, 0x1C08 ) );
			onbody.Hue = n;
			onbody.Movable = false;
			onbody.Layer = Layer.Pants;
			AddItem( onbody );

			Item hair = new Item( Utility.RandomList( 0x203C, 0x203D ) );
			hair.Hue = n;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );
		}

		private static string[] m_mtalk = new string[]
			{
				"Hey there handsome, looking for a good time? *winks*",
				"Greetings my lord, might I interest you in a nightcap?",
				"Hiya!",
				"Hey hunny, wanna party? *winks*",
				"*Tries to wave down passersby",
				"*Raises skirt* Want to see more?",
				"Could I interest you in a good night? *giggles*",
				"Ean even prefers me to a sheep!", //My personal favourite
		};

		private static string[] m_ftalk = new  string[]
			{
				"Sorry hun, I'm not in to that.",
				"Hiya!",
				"You should be talking to one of the lads.",
				"Hey lady, you're cramping my style!",
				"If you're looking for work you should be talking to Nino.",
				"Maybe another time hun *winks*",
		};

		public override bool CanBeDamaged()
		{
			return false;
		}

		private DateTime m_LastAnnounced;
		private static TimeSpan m_AnnouncingInterval = TimeSpan.FromSeconds( 20 ); 
		private DateTime m_LastReplied;
		private static TimeSpan m_ReplyingInterval = TimeSpan.FromSeconds( 2 ); 

		private static void SayRand( string[] say, Mobile m )
		{
			m.Say( say[Utility.Random( say.Length )] );
		} 

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{                                                   
			if ( m.InRange( this, 5 ) && InLOS(m) && m is PlayerMobile && !m.Hidden && DateTime.Now - m_LastAnnounced > m_AnnouncingInterval )
			{
				if ( m.Female == true )
				{
					SayRand( m_ftalk, this );
					m_LastAnnounced = DateTime.Now;
				}
				else
				{
					SayRand( m_mtalk, this );
					m_LastAnnounced = DateTime.Now;
				}
			}
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( e.Mobile.InRange ( this, 6 ) && InLOS(e.Mobile) && e.Mobile is PlayerMobile && DateTime.Now - m_LastReplied > m_ReplyingInterval )
			{
				if ( e.Mobile.Female == true && e.Speech.ToLower().IndexOf( Name.ToLower() ) >= 0 )
				{
					if ( e.Speech.ToLower().IndexOf( "hello" ) >= 0 || e.Speech.ToLower().IndexOf( "hail" ) >= 0 || e.Speech.ToLower().IndexOf( "hiya" ) >= 0 || e.Speech.ToLower().IndexOf( "hi" ) >= 0 || e.Speech.ToLower().IndexOf( "hey" ) >= 0 )
						this.Say( string.Format( "Hail milady, are you sure I'm the one you should be talking to?" ));
					else if ( e.Speech.ToLower().IndexOf( "you're hot" ) >= 0 || e.Speech.ToLower().IndexOf( "you are hot" ) >= 0 || e.Speech.ToLower().IndexOf( "you're beautiful" ) >= 0 || e.Speech.ToLower().IndexOf( "you are beautiful" ) >= 0 || e.Speech.ToLower().IndexOf( "sexy" ) >= 0 )
						this.Say( string.Format ( "Oh *giggles* I didn't realise you were that way" ));
					else if ( e.Speech.ToLower().IndexOf( "bitch" ) >= 0 || e.Speech.ToLower().IndexOf( "whore" ) >= 0 || e.Speech.ToLower().IndexOf( "slut" ) >= 0 || e.Speech.ToLower().IndexOf( "slag" ) >= 0 || e.Speech.ToLower().IndexOf( "hoe" ) >= 0 )
						this.Say ( string.Format ( "*Hmphf* If you don't have what it takes, don't take it out on me!" ));
					else if ( e.Speech.ToLower().IndexOf( "how much" ) >= 0 || e.Speech.ToLower().IndexOf( "sex" ) >= 0 || e.Speech.ToLower().IndexOf( "love" ) >= 0 || e.Speech.ToLower().IndexOf( "money" ) >= 0 || e.Speech.ToLower().IndexOf( "court" ) >= 0 )
						this.Say ( string.Format ( "*Hmmm...* Maybe some other time, girls aren't really my thing *giggles*" ));
					else if ( e.Speech.ToLower().IndexOf( "goods" ) >= 0 || e.Speech.ToLower().IndexOf( "stuff" ) >= 0 || e.Speech.ToLower().IndexOf( "undress" ) >= 0 || e.Speech.ToLower().IndexOf( "hi" ) >= 0 || e.Speech.ToLower().IndexOf( "strip" ) >= 0 )
						this.Say ( string.Format ( "You have a mirror don't you hun? *giggles*" ));
					else
						this.Say ( string.Format ( "*giggles*" ));
					m_LastReplied = DateTime.Now;
				}
				else if ( e.Speech.ToLower().IndexOf( Name.ToLower() ) >= 0 )
				{
					if ( e.Speech.ToLower().IndexOf( "hello" ) >= 0 || e.Speech.ToLower().IndexOf( "hail" ) >= 0 || e.Speech.ToLower().IndexOf( "hiya" ) >= 0 || e.Speech.ToLower().IndexOf( "hi" ) >= 0 || e.Speech.ToLower().IndexOf( "hey" ) >= 0 )
						this.Say( string.Format( "Hail milord, you look like you could do with the company of a good woman *winks*" ));
					else if ( e.Speech.ToLower().IndexOf( "you're hot" ) >= 0 || e.Speech.ToLower().IndexOf( "you are hot" ) >= 0 || e.Speech.ToLower().IndexOf( "you're beautiful" ) >= 0 || e.Speech.ToLower().IndexOf( "you are beautiful" ) >= 0 || e.Speech.ToLower().IndexOf( "sexy" ) >= 0 )
						this.Say( string.Format ( "Oooh! *reveals a little* Maybe you'd like to see some more?" ));
					else if ( e.Speech.ToLower().IndexOf( "bitch" ) >= 0 || e.Speech.ToLower().IndexOf( "whore" ) >= 0 || e.Speech.ToLower().IndexOf( "slut" ) >= 0 || e.Speech.ToLower().IndexOf( "slag" ) >= 0 || e.Speech.ToLower().IndexOf( "hoe" ) >= 0 )
						this.Say ( string.Format ( "*Hmphf* Nino would have you thrown out for that!" ));
					else if ( e.Speech.ToLower().IndexOf( "how much" ) >= 0 || e.Speech.ToLower().IndexOf( "sex" ) >= 0 || e.Speech.ToLower().IndexOf( "love" ) >= 0 || e.Speech.ToLower().IndexOf( "money" ) >= 0 || e.Speech.ToLower().IndexOf( "court" ) >= 0 )
						this.Say ( string.Format ( "You'd best go talk to Nino about that *blows a kiss* See you soon sexy!" ));
					else if ( e.Speech.ToLower().IndexOf( "goods" ) >= 0 || e.Speech.ToLower().IndexOf( "stuff" ) >= 0 || e.Speech.ToLower().IndexOf( "undress" ) >= 0 || e.Speech.ToLower().IndexOf( "hi" ) >= 0 || e.Speech.ToLower().IndexOf( "strip" ) >= 0 )
						this.Say ( string.Format ( "*gives a little twirl* You can have it all handsome, for a price!" ));
					else
						this.Say ( string.Format ( "Does my bum look okay in this? *flutters eyelashes*" ));
					m_LastReplied = DateTime.Now;
				}
			}
		}
		public Prostitute( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
