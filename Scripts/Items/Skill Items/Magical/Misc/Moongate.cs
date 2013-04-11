using System;
using Server.Factions;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
	[DispellableField]
	public class Moongate : Item
	{
		private Point3D m_Target;
		private Map m_TargetMap;
		private bool m_bDispellable;
        private bool m_Creatures;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap
		{
			get
			{
				return m_TargetMap;
			}
			set
			{
				m_TargetMap = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Dispellable
		{
			get
			{
				return m_bDispellable;
			}
			set
			{
				m_bDispellable = value;
			}
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Creatures
        {
            get { return m_Creatures; }
            set { m_Creatures = value; }
        }


		public virtual bool ShowFeluccaWarning{ get{ return false; } }

		[Constructable]
		public Moongate() : this( Point3D.Zero, null )
		{
			m_bDispellable = true;
		}

		[Constructable]
		public Moongate(bool bDispellable, bool creatures) : this( Point3D.Zero, null )
		{
			m_bDispellable = bDispellable;
            m_Creatures = creatures;
		}

		[Constructable]
		public Moongate( Point3D target, Map targetMap) : base( 0xF6C )
		{
			Movable = false;
			Light = LightType.Circle300;

			m_Target = target;
			m_TargetMap = targetMap;

            m_Creatures = true;
		}

		public Moongate( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            return;
            //if ( !from.Player )
            //    return;

            //if ( from.InRange( GetWorldLocation(), 1 ) )
            //    CheckGate( from, 1 );
            //else
            //    from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public override bool OnMoveOver( Mobile m )
		{
            bool allowGate;

            if ( m.Player )
                allowGate = true;
            else if ( m_Creatures && m is BaseCreature && !(m is BaseVendor) && ((BaseCreature)m).CanUseMoonGates)
                allowGate = true;
            else 
                allowGate = false;

            IPooledEnumerable eable = Map.GetItemsInRange(GetWorldLocation(), 0);

            foreach (Item i in eable)
            {
                if (i.ItemID >= 14695 && i.ItemID <= 14730) //If paralyze field exists on gate, don't teleport player
                    return true;
            }

		    if (allowGate)
				CheckGate( m, 0 );

			return true;
		}

		public virtual void CheckGate( Mobile m, int range )
		{
			new DelayTimer( m, this, range ).Start();
		}

		public virtual void OnGateUsed( Mobile m )
		{
		}

		public virtual void UseGate( Mobile m )
		{
            ClientFlags flags = m.NetState == null ? ClientFlags.None : m.NetState.Flags;

            bool hasstonewall = false;
            bool hasgate = false;
		    Item paralyzeField = null;

            IPooledEnumerable eable;

            if (m_TargetMap != null)
            {
                eable = m_TargetMap.GetItemsInRange(m_Target, 0);

                foreach (Item i in eable)
                {
                    if (i.ItemID == 0x80) //Stonewall
                        hasstonewall = true;
                    if (i.ItemID == 0xF6C) //Gate
                        hasgate = true;
                    if (i.ItemID >= 14695 && i.ItemID <= 14730) //Paralyze field
                        paralyzeField = i;
                }

                eable.Free();
            }

			if ( Sigil.ExistsOn( m ) )
			{
				m.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
            //else if ( m_TargetMap == Map.Felucca && m is PlayerMobile && ((PlayerMobile)m).Young )
            //{
            //    m.SendLocalizedMessage( 1049543 ); // You decide against traveling to Felucca while you are still young.
            //}
			//else if ( (m.Kills >= 5 && m_TargetMap != Map.Felucca) || ( m_TargetMap == Map.Tokuno && (flags & ClientFlags.Tokuno) == 0 ) || ( m_TargetMap == Map.Malas && (flags & ClientFlags.Malas) == 0 ) || ( m_TargetMap == Map.Ilshenar && (flags & ClientFlags.Ilshenar) == 0 ) )
            //{
            //    m.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
            //}
            //else if ( m.Spell != null )
            //{
            //    m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
            //}
			else if ( m_TargetMap != null && m_TargetMap != Map.Internal)
			{
                if ( (hasgate || !Dispellable) && !hasstonewall )
                {
                    BaseCreature.TeleportPets(m, m_Target, m_TargetMap);

                    m.MoveToWorld(m_Target, m_TargetMap);

                    if (m.AccessLevel == AccessLevel.Player || !m.Hidden)
                    {
                        //Iza - Teleport Poof
                        Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        Effects.SendLocationParticles(EffectItem.Create(m_Target, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                        m.PlaySound(0x1FE);
                    }

                    if (paralyzeField != null)
                        paralyzeField.OnMoveOver(m); //Paralyze player if paralyze field exist

                    OnGateUsed(m);
                }
                else
                    m.SendAsciiMessage("You cannot teleport to that location.");
			}
			else
			{
				m.SendMessage( "This moongate does not seem to go anywhere." );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

			writer.Write( m_Target );
			writer.Write( m_TargetMap );
			
			// Version 1
			writer.Write( m_bDispellable );
            writer.Write(m_Creatures);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Target = reader.ReadPoint3D();
			m_TargetMap = reader.ReadMap();

            switch ( version )
            {
                case 1:
                    {
                        m_bDispellable = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Creatures = reader.ReadBool();
                        break;
                    }
            }
		}

		public virtual bool ValidateUse( Mobile from, bool message )
		{
			if ( from.Deleted || Deleted )
				return false;

			if ( from.Map != Map || !from.InRange( this, 1 ) )
			{
				if ( message )
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

				return false;
			}

			return true;
		}

		public virtual void BeginConfirmation( Mobile from )
		{
			if ( IsInTown( from.Location, from.Map ) && !IsInTown( m_Target, m_TargetMap ) || (from.Map != Map.Felucca && TargetMap == Map.Felucca && ShowFeluccaWarning) )
			{
				from.Send( new PlaySound( 0x20E, from.Location ) );
				from.CloseGump( typeof( MoongateConfirmGump ) );
				from.SendGump( new MoongateConfirmGump( from, this ) );
			}
			else
			{
				EndConfirmation( from );
			}
		}

		public virtual void EndConfirmation( Mobile from )
		{
			if ( !ValidateUse( from, true ) )
				return;

			UseGate( from );
		}

		public virtual void DelayCallback( Mobile from, int range )
		{
			if ( !ValidateUse( from, false ) || !from.InRange( this, range ) )
				return;

			if ( m_TargetMap != null )
				BeginConfirmation( from );
			else
				from.SendMessage( "This moongate does not seem to go anywhere." );
		}

		public static bool IsInTown( Point3D p, Map map )
		{
			if ( map == null )
				return false;

			GuardedRegion reg = (GuardedRegion) Region.Find( p, map ).GetRegion( typeof( GuardedRegion ) );

			return ( reg != null && !reg.IsDisabled() );
		}

		private class DelayTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly Moongate m_Gate;
			private int m_Range;

			public DelayTimer( Mobile from, Moongate gate, int range ) : base( TimeSpan.Zero )
			{
				m_From = from;
				m_Gate = gate;
				m_Range = range;
			}

			protected override void OnTick()
			{
			    m_Gate.EndConfirmation(m_From);
			}
		}
	}

	public class ConfirmationMoongate : Moongate
	{
		private int m_GumpWidth;
		private int m_GumpHeight;

		private int m_TitleColor;
		private int m_MessageColor;

		private int m_TitleNumber;
		private int m_MessageNumber;

		private string m_MessageString;

		[CommandProperty( AccessLevel.GameMaster )]
		public int GumpWidth
		{
			get{ return m_GumpWidth; }
			set{ m_GumpWidth = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int GumpHeight
		{
			get{ return m_GumpHeight; }
			set{ m_GumpHeight = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TitleColor
		{
			get{ return m_TitleColor; }
			set{ m_TitleColor = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MessageColor
		{
			get{ return m_MessageColor; }
			set{ m_MessageColor = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TitleNumber
		{
			get{ return m_TitleNumber; }
			set{ m_TitleNumber = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MessageNumber
		{
			get{ return m_MessageNumber; }
			set{ m_MessageNumber = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string MessageString
		{
			get{ return m_MessageString; }
			set{ m_MessageString = value; }
		}

		[Constructable]
		public ConfirmationMoongate() : this( Point3D.Zero, null )
		{
		}

		[Constructable]
		public ConfirmationMoongate( Point3D target, Map targetMap ) : base( target, targetMap )
		{
		}

		public ConfirmationMoongate( Serial serial ) : base( serial )
		{
		}

		public virtual void Warning_Callback( Mobile from, bool okay, object state )
		{
			if ( okay )
				EndConfirmation( from );
		}

		public override void BeginConfirmation( Mobile from )
		{
			if ( m_GumpWidth > 0 && m_GumpHeight > 0 && m_TitleNumber > 0 && (m_MessageNumber > 0 || m_MessageString != null) )
			{
				from.CloseGump( typeof( WarningGump ) );
				from.SendGump( new WarningGump( m_TitleNumber, m_TitleColor, m_MessageString == null ? m_MessageNumber : (object)m_MessageString, m_MessageColor, m_GumpWidth, m_GumpHeight, Warning_Callback, from ) );
			}
			else
			{
				base.BeginConfirmation( from );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.WriteEncodedInt( m_GumpWidth );
			writer.WriteEncodedInt( m_GumpHeight );

			writer.WriteEncodedInt( m_TitleColor );
			writer.WriteEncodedInt( m_MessageColor );

			writer.WriteEncodedInt( m_TitleNumber );
			writer.WriteEncodedInt( m_MessageNumber );

			writer.Write( m_MessageString );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_GumpWidth = reader.ReadEncodedInt();
					m_GumpHeight = reader.ReadEncodedInt();

					m_TitleColor = reader.ReadEncodedInt();
					m_MessageColor = reader.ReadEncodedInt();

					m_TitleNumber = reader.ReadEncodedInt();
					m_MessageNumber = reader.ReadEncodedInt();

					m_MessageString = reader.ReadString();

					break;
				}
			}
		}
	}

	public class MoongateConfirmGump : Gump
	{
		private readonly Mobile m_From;
		private readonly Moongate m_Gate;

		public MoongateConfirmGump( Mobile from, Moongate gate ) : base( Core.AOS ? 110 : 20, Core.AOS ? 100 : 30 )
		{
			m_From = from;
			m_Gate = gate;

			if ( Core.AOS )
			{
				Closable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 420, 280, 5054 );

				AddImageTiled( 10, 10, 400, 20, 2624 );
				AddAlphaRegion( 10, 10, 400, 20 );

				AddHtmlLocalized( 10, 10, 400, 20, 1062051, 30720, false, false ); // Gate Warning

				AddImageTiled( 10, 40, 400, 200, 2624 );
				AddAlphaRegion( 10, 40, 400, 200 );

				if ( from.Map != Map.Felucca && gate.TargetMap == Map.Felucca && gate.ShowFeluccaWarning )
					AddHtmlLocalized( 10, 40, 400, 200, 1062050, 32512, false, true ); // This Gate goes to Felucca... Continue to enter the gate, Cancel to stay here
				else
					AddHtmlLocalized( 10, 40, 400, 200, 1062049, 32512, false, true ); // Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here

				AddImageTiled( 10, 250, 400, 20, 2624 );
				AddAlphaRegion( 10, 250, 400, 20 );

				AddButton( 10, 250, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 250, 170, 20, 1011036, 32767, false, false ); // OKAY

				AddButton( 210, 250, 4005, 4007, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 240, 250, 170, 20, 1011012, 32767, false, false ); // CANCEL
			}
			else
			{
				AddPage( 0 );

				AddBackground( 0, 0, 420, 400, 5054 );
				AddBackground( 10, 10, 400, 380, 3000 );

				AddHtml( 20, 40, 380, 60, @"Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here", false, false );

				AddHtmlLocalized( 55, 110, 290, 20, 1011012, false, false ); // CANCEL
				AddButton( 20, 110, 4005, 4007, 0, GumpButtonType.Reply, 0 );

				AddHtmlLocalized( 55, 140, 290, 40, 1011011, false, false ); // CONTINUE
				AddButton( 20, 140, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
				m_Gate.EndConfirmation( m_From );
		}
	}
}