using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public enum SawTrapType
	{
		WestWall,
		NorthWall,
		WestFloor,
		NorthFloor
	}

	public class SawTrap : BaseTrap
	{
		[CommandProperty( AccessLevel.GameMaster )]
		public SawTrapType Type
		{
			get
			{
				switch ( ItemID )
				{
					case 0x1103: return SawTrapType.NorthWall;
					case 0x1116: return SawTrapType.WestWall;
					case 0x11AC: return SawTrapType.NorthFloor;
					case 0x11B1: return SawTrapType.WestFloor;
				}

				return SawTrapType.NorthWall;
			}
			set
			{
				ItemID = GetBaseID( value );
			}
		}

		public static int GetBaseID( SawTrapType type )
		{
			switch ( type )
			{
				case SawTrapType.NorthWall: return 0x1103;
				case SawTrapType.WestWall: return 0x1116;
				case SawTrapType.NorthFloor: return 0x11AC;
				case SawTrapType.WestFloor: return 0x11B1;
			}

			return 0;
		}

		[Constructable]
		public SawTrap() : this( SawTrapType.NorthFloor )
		{
            m_AnimHue = 0;
		}

		[Constructable]
		public SawTrap( SawTrapType type ) : base( GetBaseID( type ) )
		{
            m_AnimHue = 0;
		}

		public override bool PassivelyTriggered{ get{ return false; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 0; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

        private int m_AnimHue;

        [CommandProperty(AccessLevel.Counselor)]
        public virtual int AnimHue
        {
            get { return m_AnimHue; }
            set { m_AnimHue = value; }
        }

		public override void OnTrigger( Mobile from )
		{
			if ( !from.Alive || from.AccessLevel > AccessLevel.Player )
				return;

            //if (AnimHue > 0) 
            //    AnimHue--;

			Effects.SendLocationEffect( Location, Map, GetBaseID( Type ) + 1, 6, 3, m_AnimHue, 0 );
			Effects.PlaySound( Location, Map, 0x21C );

			SpellHelper.Damage( TimeSpan.FromTicks( 1 ), from, from, Utility.RandomMinMax( 5, 15 ) );

			from.LocalOverheadMessage( MessageType.Regular, 0x22, 500853 ); // You stepped onto a blade trap!
		}

		public SawTrap( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

            writer.Write(m_AnimHue);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_AnimHue = reader.ReadInt();
                    }
                    break;
            }
		}
	}
}