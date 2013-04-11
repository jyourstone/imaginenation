using System;

//Bounty System Start
using Server.BountySystem;
//End Bounty System

namespace Server.Items
{
	public enum HeadType
	{
		Regular,
		Duel,
		Tournament
	}

	public class Head : Item
	{
        //bount system here
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime CreationTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Killer { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsPlayer { get; set; }

        //end bounty system

	    [CommandProperty(AccessLevel.GameMaster)]
	    public Mobile Owner { get; set; }

	    private string m_PlayerName;
		private HeadType m_HeadType;
	    private int m_Fame;

		[CommandProperty( AccessLevel.GameMaster )]
		public string PlayerName
		{
			get { return m_PlayerName; }
			set { m_PlayerName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public HeadType HeadType
		{
			get { return m_HeadType; }
			set { m_HeadType = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int Fame
        {
            get { return m_Fame; }
            set { m_Fame = value; }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (m_PlayerName == null)
                base.OnSingleClick(from);
            else
            {
                switch (m_HeadType)
                {
                    default:
                        LabelTo(from, String.Format("head of {0} ({1} fame)", m_PlayerName, m_Fame));
                        break;
                    case HeadType.Duel:
                        LabelTo(from, String.Format("head of {0}, taken in a duel ({1} fame)", m_PlayerName, m_Fame));
                        break;
                    case HeadType.Tournament:
                        LabelTo(from, String.Format("head of {0}, taken in a tournament ({1} fame)", m_PlayerName, m_Fame));
                        break;
                }
            }
        }

		[Constructable]
		public Head()
			: this( null )
		{
		}

		[Constructable]
		public Head( string playerName )
			: this( HeadType.Regular, playerName )
		{
		}

		[Constructable]
		public Head( HeadType headType, string playerName )
            : base(0x1CE1)
		{
			m_HeadType = headType;
			m_PlayerName = playerName;
		}

		public Head( Serial serial )
			: base( serial )
		{
		}


        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
            {
                from.SendAsciiMessage("You cannot eat this head");
                return;
            }

            // Fill the Mobile with FillFactor
            if (Food.FillHunger(from, 4))
            {
                // Play a random "eat" sound
                from.PlaySound(Utility.Random(0x3A, 3));

                if (from.Body.IsHuman && !from.Mounted)
                    from.Animate(34, 5, 1, true, false, 0);

                if (PlayerName != null)
                    from.PublicOverheadMessage(Network.MessageType.Emote, 0x22, true, string.Format("*You see {0} eat the head of {1}*", from.Name, m_PlayerName));

                Consume();
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 4 ); // version

            //bounty system
            //ver 4
            writer.Write(CreationTime);
            writer.Write(Killer);
            writer.Write(IsPlayer);
            //end bounty system

            //ver 3
            writer.Write(m_Fame);

            //Ver2
            writer.Write(Owner);

			writer.Write( m_PlayerName );
			writer.WriteEncodedInt( (int) m_HeadType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                //bounty system
                case 4:
			        CreationTime = reader.ReadDateTime();
			        Killer = reader.ReadMobile();
			        IsPlayer = reader.ReadBool();
			        goto case 3;
                //end bounty system
                case 3:
			        m_Fame = reader.ReadInt();
			        goto case 2;
                case 2:
                    Owner = reader.ReadMobile();
                    goto case 1;
				case 1:
					m_PlayerName = reader.ReadString();
					m_HeadType = (HeadType) reader.ReadEncodedInt();
					break;

				case 0:
					string format = Name;

					if ( format != null )
					{
						if ( format.StartsWith( "the head of " ) )
							format = format.Substring( "the head of ".Length );

						if ( format.EndsWith( ", taken in a duel" ) )
						{
							format = format.Substring( 0, format.Length - ", taken in a duel".Length );
							m_HeadType = HeadType.Duel;
						}
						else if ( format.EndsWith( ", taken in a tournament" ) )
						{
							format = format.Substring( 0, format.Length - ", taken in a tournament".Length );
							m_HeadType = HeadType.Tournament;
						}
					}

					m_PlayerName = format;
					Name = null;

					break;
			}
		}
	}
}