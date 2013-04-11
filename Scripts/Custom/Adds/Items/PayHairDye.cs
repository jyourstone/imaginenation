using System;
using Server;
using Server.Multis;
using Server.Targeting;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class PayHairDye : Item
	{
		private bool m_Redyable;
		private int m_DyedHue;
		private int m_Price;

		public virtual CustomHuePicker CustomHuePicker{ get{ return null; } }

		public virtual bool AllowRunebooks
		{
			get{ return false; }
		}

		public virtual bool AllowFurniture
		{
			get{ return false; }
		}

		public virtual bool AllowStatuettes
		{
			get{ return false; }
		}

		public virtual bool AllowLeather
		{
			get{ return false; }
		}

		public virtual bool AllowDyables
		{
			get{ return false; }
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );			
			list.Add( "Price Per Item Dyed: "+m_Price.ToString()  );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_Redyable );
			writer.Write( (int) m_DyedHue );
			writer.Write( (int) m_Price );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Redyable = reader.ReadBool();
					m_DyedHue = reader.ReadInt();
					m_Price = reader.ReadInt();

					break;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Price
		{
			get{return m_Price;}
			set{m_Price = value;}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Redyable
		{
			get{return m_Redyable;}
			set{m_Redyable = value;}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DyedHue
		{
			get{return m_DyedHue;}
			set
			{
				if ( m_Redyable )
				{
					m_DyedHue = value;
					Hue = value;
				}
			}
		}

		[Constructable] 
		public PayHairDye() : base( 0xEFE )
		{
			Hue = DyedHue = 0;
			Weight = 10.0;
			Redyable = false;
			Price = 500;
            Movable = false;
		}

		public PayHairDye( Serial serial ) : base( serial )
		{
		}

		// Select the clothing to dye.
		public virtual int TargetMessage{ get{ return 500859; } }

		// You can not dye that.
		public virtual int FailMessage{ get{ return 1042083; } }

		public override void OnDoubleClick( Mobile from )
		{
            if (from.InRange(this.GetWorldLocation(), 5) && from.InLOS(this))
			{
				from.Target = new InternalTarget( this, Price );
			}
			else
			{
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			}
		}

		private class InternalTarget : Target
		{
			private PayHairDye m_Tub;
			private int m_Price;

			public InternalTarget( PayHairDye tub, int Price ) : base( 1, false, TargetFlags.None )
			{
				m_Tub = tub;
				m_Price = Price;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                if (from.HairItemID == 0 && from.FacialHairItemID == 0)
                {
                    from.SendLocalizedMessage(502623);	// You have no hair to dye and cannot use this
                }
				else if( from == targeted )
				{
                    if (from.Backpack != null && from.Backpack.ConsumeTotal(typeof(Gold), m_Price))
                    {
                        from.PlaySound(0x23E);
                        from.SendMessage("{0} has been taken from your backpack!", m_Price.ToString()); // Amount charged
                        from.HairHue = from.FacialHairHue = m_Tub.DyedHue;
                        from.PlaySound(0x23E);
                    }
                    else if ( from.BankBox != null && from.BankBox.ConsumeTotal( typeof( Gold ), m_Price ) )
					{
						from.PlaySound( 0x23E );
						from.SendLocalizedMessage( 1060398, m_Price.ToString() ); // Amount charged
                        from.HairHue = from.FacialHairHue = m_Tub.DyedHue;
                        from.PlaySound(0x23E);
                    }
                    else
                    {
                        from.SendMessage("You cannot afford to dye that!");
                    }
				}
                else
                    from.SendLocalizedMessage(m_Tub.FailMessage);
			}
		}
	}
}