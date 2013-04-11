using System;
using Server.Engines.Craft;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public enum ToolQuality
	{
		Low,
		Regular,
		Exceptional
	}

	public abstract class BaseTool : Item, IUsesRemaining, ICraftable
	{
		private Mobile m_Crafter;
		private ToolQuality m_Quality;
		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ToolQuality Quality
		{
			get{ return m_Quality; }
			set{ UnscaleUses(); m_Quality = value; InvalidateProperties(); ScaleUses(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public void ScaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * GetUsesScalar()) / 100;
			InvalidateProperties();
		}

		public void UnscaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * 100) / GetUsesScalar();
		}

		public int GetUsesScalar()
		{
			if ( m_Quality == ToolQuality.Exceptional )
				return 200;

			return 100;
		}

		public bool ShowUsesRemaining{ get{ return true; } set{} }

		public abstract CraftSystem CraftSystem{ get; }

		public BaseTool( int itemID ) : this( Utility.RandomMinMax( 25, 75 ), itemID )
		{
		}

		public BaseTool( int uses, int itemID ) : base( itemID )
		{
			m_UsesRemaining = uses;
			m_Quality = ToolQuality.Regular;
		}

		public BaseTool( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			// Makers mark not displayed on OSI
			//if ( m_Crafter != null )
			//	list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Quality == ToolQuality.Exceptional )
				list.Add( 1060636 ); // exceptional

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public virtual void DisplayDurabilityTo( Mobile m )
		{
			LabelToAffix( m, 1017323, AffixType.Append, ": " + m_UsesRemaining ); // Durability
		}

		public static bool CheckAccessible( Item tool, Mobile m )
		{
			return ( tool.IsChildOf( m ) || tool.Parent == m );
		}

		public static bool CheckTool( Item tool, Mobile m )
		{
			Item check = m.FindItemOnLayer( Layer.OneHanded );

			if ( check is BaseTool && check != tool && !(check is AncientSmithyHammer) )
				return false;

			check = m.FindItemOnLayer( Layer.TwoHanded );

			if ( check is BaseTool && check != tool && !(check is AncientSmithyHammer) )
				return false;

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (!IsChildOf(from.Backpack) && !(Parent == from) && from.InLOS(this) && from.InRange(Location, 3))
                from.AddToBackpack(this);

			if ( IsChildOf( from.Backpack ) || Parent == from )
			{
				CraftSystem system = CraftSystem;

				int num = system.CanCraft( from, this, null );

                if (num > 0 && (num != 1044267 || !Core.SE)) // Blacksmithing shows the gump regardless of proximity of an anvil and forge after SE
                {
					from.SendLocalizedMessage( num );
				}
				else
				{
                    from.SendGump(new CraftGump(from, system, this, null));
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

        public override bool CanEquip(Mobile m)
        {
            return true;
        }

        //Maka : fix the bad code practise
        public void SphereDoubleClick(Mobile from)
        {

            if (Parent == null)
                SpellHelper.Turn(from, this);

            //Maka : New layers
            if (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid) != this)
            {
                Item i = from.FindItemOnLayer(Layer.FirstValid);

                if (i != null)
                {
                    from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));
                    from.AddToBackpack(i);
                }
            }

            if (from.FindItemOnLayer(Layer) != null && from.FindItemOnLayer(Layer) != this)
            {
                Item i = from.FindItemOnLayer(Layer);

                //Is it a shield
                if (i is BaseArmor)
                {
                    from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));
                    from.AddToBackpack(i);

                    //If its a shield, is he holding a weap
                    i = from.FindItemOnLayer(Layer.OneHanded);

                    if (i != null)
                    {
                        from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));
                        from.AddToBackpack(i);
                    }
                }
                else
                {
                    i = from.FindItemOnLayer(Layer);

                    from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));

                    from.AddToBackpack(i);
                }

                from.EquipItem(this);
                from.PlaySound(0x57);
            }
            else if (Layer == Layer.OneHanded && from.FindItemOnLayer(Layer.TwoHanded) != null || Layer == Layer.TwoHanded && from.FindItemOnLayer(Layer.OneHanded) != null)
            {
                Item i = from.FindItemOnLayer(Layer.TwoHanded);

                if (i != null && !(i is BaseShield))
                {
                    from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));
                    from.AddToBackpack(i);
                }

                from.EquipItem(this);
            }
            else if (Layer == Layer.TwoHanded && from.FindItemOnLayer(Layer.OneHanded) != null)
            {
                Item i = from.FindItemOnLayer(Layer.OneHanded);

                if (i != null)
                {
                    from.SendAsciiMessage(string.Format("You put the {0} in your pack.", Sphere.ComputeName(i)));
                    from.AddToBackpack(i);
                }

                from.EquipItem(this);
            }
            else
                from.EquipItem(this);
        }


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

			writer.Write( m_Crafter );
			writer.Write( (int) m_Quality );

			writer.Write( m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Crafter = reader.ReadMobile();
					m_Quality = (ToolQuality) reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_UsesRemaining = reader.ReadInt();
					break;
				}
			}
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Quality = (ToolQuality)quality;

			if ( makersMark )
				Crafter = from;

			return quality;
		}

		#endregion
	}
}