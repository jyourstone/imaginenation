using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseWand : BaseBashing
	{
        public override int OldStrengthReq { get { return 0; } }
        public override int OldMinDamage { get { return 2; } }
        public override int OldMaxDamage { get { return 6; } }
        public override int OldSpeed { get { return 200; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 110; } }

		private int m_Charges;
	    private int m_MaxCharges;
	    private bool m_UsedAfterReCharged;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get{ return m_Charges; }
			set{ m_Charges = value; InvalidateProperties(); }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxCharges
        {
            get { return m_MaxCharges; }
            set { m_MaxCharges = value; InvalidateProperties(); }
        }

		public BaseWand( int charges ) : base( Utility.RandomList( 0xDF2, 0xDF3, 0xDF4, 0xDF5 ) )
		{
			Weight = 1.0;
			Charges = charges;
            m_MaxCharges = charges;
		}

		public void ConsumeCharge( Mobile from )
		{
		    --Charges;

		    m_UsedAfterReCharged = true;

			if ( Charges == 0 )
				from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
		}

		public BaseWand( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (!Sphere.EquipOnDouble(from, this))
                return;

            if (!Identified)
            {
                from.SendAsciiMessage("You must identify your wand before using it");
                return;
            }

            if (Charges > 0)
                OnWandUse(from);
            else
                from.SendLocalizedMessage(1019073); // This item is out of charges.
		}

        public override bool AllowEquipedCast(Mobile from)
        {
            return true;
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
            //version 1
            writer.Write(m_MaxCharges);
            writer.Write(m_UsedAfterReCharged);
            //version 0
			writer.Write( m_Charges );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 1:
			        {
			            m_MaxCharges = reader.ReadInt();
			            m_UsedAfterReCharged = reader.ReadBool();
			            goto case 0;
			        }
				case 0:
				{
					m_Charges = reader.ReadInt();
					break;
				}
			}
		}

		public override void OnSingleClick( Mobile from )
		{
            if (!Identified)
				LabelTo( from, "Wand" );
			else
                LabelTo( from, "Magic wand of {0} ({1} charge{2})", Name, Charges.ToString(), Charges != 1 ? "s" : "" );
		}

		public void Cast( Spell spell)
		{
			bool m = Movable;

			Movable = false;
			spell.Cast();
			Movable = m;
		}

		public virtual void OnWandUse( Mobile from )
		{
			from.Target = new WandTarget( this );
		}

		public virtual void DoWandTarget( Mobile from, object o )
		{
			if ( Deleted || Charges <= 0 || Parent != from || o is StaticTarget || o is LandTarget )
				return;

            if (OnWandTarget(from, o))
            {
                ConsumeCharge(from);
            }
		}

		public virtual bool OnWandTarget( Mobile from, object o )
		{
			return true;
		}

        public virtual void RechargeWand(Mobile from, Spell spell)
        {
            if (m_UsedAfterReCharged)
            {
                m_MaxCharges--;
                m_UsedAfterReCharged = false;
            }

            if (from.FindItemOnLayer(Layer.OneHanded) != this)
            {
                from.SendAsciiMessage("You must have your wand equipped to recharge it");
                spell.DoFizzle();
            }
            else if (spell.Name != Name)
            {
                from.SendAsciiMessage("That is not a {0} wand", spell.Name);
                spell.DoFizzle();
            }
            else if (m_Charges >= m_MaxCharges)
            {
                from.SendAsciiMessage("That wand is at max charges");
                spell.DoFizzle();
            }
            else if (spell.CheckSequence())
            {
                m_Charges++;
                from.PlaySound(spell.Sound);
            }

            spell.FinishSequence();
        }
	}
}