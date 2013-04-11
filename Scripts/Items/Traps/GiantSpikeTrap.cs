using System;
using Server.Spells;

namespace Server.Items
{
	public class GiantSpikeTrap : BaseTrap
	{
		[Constructable]
		public GiantSpikeTrap() : base( 1 )
		{
            m_AnimHue = 0;
		}

		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 3; } }
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
			if ( from.AccessLevel > AccessLevel.Player )
				return;

            //if (AnimHue > 0)
            //    AnimHue--;

			Effects.SendLocationEffect( Location, Map, 0x1D99, 48, 2, m_AnimHue, 0 );

			if ( from.Alive && CheckRange( from.Location, 0 ) )
				SpellHelper.Damage( TimeSpan.FromTicks( 1 ), from, from, Utility.Dice( 10, 7, 0 ) );
		}

		public GiantSpikeTrap( Serial serial ) : base( serial )
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