using System.Collections;
using Server.Network;

namespace Server.DuelSystem
{
	public class DuelStone : Item
	{
		private static Point3D defultStartingLocation = new Point3D( 5450, 1162, 0 );

		private DuelStoneEventType eventType;

		private Point3D duelerLocation1;
		private Point3D duelerLocation2;
		private Point3D afterDuelLocation;

		private Mobile combatant1;
		private Mobile combatant2;
		private Mobile combatant3;
		private Mobile combatant4;

		private ArrayList usingStone;

	    private Timer endDuelTimer;

	    private bool isLadderStone;

		private int stoneIdleTime;
		private int stoneMaxIdleTime;
		private int moneyTaken;

	    [Constructable]
		public DuelStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1109;
			Name = "a duel stone";
			if( usingStone == null )
				usingStone = new ArrayList();

			StoneIdleTime = 15;
			StoneMaxIdleTime = 30;
			StoneType = DuelStoneEventType.Money1vs1;
			DuelCost = 1000;
		}

		public DuelStone( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DuelStoneEventType StoneType
		{
			get { return eventType; }
			set
			{
				if( value.ToString().Contains( "Loot" ) )
					DuelCost = 0;

				if( value.ToString().Contains( "2vs2" ) )
					value = DuelStoneEventType.Loot1vs1;

				eventType = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D DuelerLocation1
		{
			get { return duelerLocation1; }
			set { duelerLocation1 = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D DuelerLocation2
		{
			get { return duelerLocation2; }
			set { duelerLocation2 = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Combatant1
		{
			get { return combatant1; }
			set
			{
				if( value != null && !usingStone.Contains( value ) )
					usingStone.Add( value );
				else if( value == null && combatant1 != null && usingStone.Contains( combatant1 ) )
					usingStone.Remove( combatant1 );

				combatant1 = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Combatant2
		{
			get { return combatant2; }
			set
			{
				if( value != null && !usingStone.Contains( value ) )
					usingStone.Add( value );
				else if( value == null && combatant2 != null && usingStone.Contains( combatant2 ) )
					usingStone.Remove( combatant2 );

				combatant2 = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Combatant3
		{
			get { return combatant3; }
			set
			{
				if( value != null && !usingStone.Contains( value ) )
					usingStone.Add( value );
				else if( value == null && combatant3 != null && usingStone.Contains( combatant3 ) )
					usingStone.Remove( combatant3 );

				combatant3 = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Combatant4
		{
			get { return combatant4; }
			set
			{
				if( value != null && !usingStone.Contains( value ) )
					usingStone.Add( value );
				else if( value == null && combatant4 != null && usingStone.Contains( combatant4 ) )
					usingStone.Remove( combatant4 );

				combatant4 = value;
			}
		}

		public ArrayList UsingStone
		{
			get { return usingStone; }
			set { usingStone = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D AfterDuelLocation
		{
			get { return afterDuelLocation; }
			set { afterDuelLocation = value; }
		}

	    public Timer CountTimer { get; set; }

	    public Timer MoveTimer { get; set; }

	    public Timer EndDuelTimer
		{
			get { return endDuelTimer; }
			set { endDuelTimer = value; }
		}

	    public bool EndTimer { get; set; }

	    [CommandProperty( AccessLevel.Administrator )]
		public bool IsLadderStone
		{
			get { return isLadderStone; }
			set { isLadderStone = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int StoneIdleTime
		{
			get { return stoneIdleTime; }
			set
			{
				if( value > 60 && value < 5 )
					value = 15;

				stoneIdleTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int StoneMaxIdleTime
		{
			get { return stoneMaxIdleTime; }
			set
			{
				if( value > 60 && value < 5 )
					value = 15;

				stoneMaxIdleTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DuelCost
		{
			get { return moneyTaken; }
			set
			{
				moneyTaken = value;
			}
			/*{
				if( StoneType.ToString().Contains( "Loot" ) )
					value = 0;
				else if( value < 1000 )
					value = 1000;

				moneyTaken = value;
			}*/
		}

	    public int TimeLeft { get; set; }

	    public override void OnSingleClick( Mobile from )
		{
			if( StoneType == DuelStoneEventType.Money1vs1 )
				LabelTo( from, "1vs1 for [" + DuelCost + "]" );
			else if( StoneType == DuelStoneEventType.Money2vs2 )
				LabelTo( from, "2vs2 for [" + DuelCost + "]" );
			else if( StoneType == DuelStoneEventType.Loot1vs1 )
				LabelTo( from, "1vs1 for [loot]" );
			else if( StoneType == DuelStoneEventType.Loot2vs2 )
				LabelTo( from, "2vs2 for [loot]" );
			else
				LabelTo( from, "Stone error, page a GM!" );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel != AccessLevel.Player )
			{
				from.SendAsciiMessage( "Only players can join duels!" );
				return;
			}

			if( !from.CanSee( this ) )
			{
				from.SendAsciiMessage( "You can't see this!" );
				return;
			}

			if( !from.InLOS( this ) || !from.InRange( Location, 4 ) )
			{
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}

			if( from.Mounted )
			{
				from.SendAsciiMessage( "Can't join a duel while riding!" );
				return;
			}

			if( DuelStoneConfig.CanUseStone( this, from ) )
				if( IsUsingStone( from ) )
					DuelStoneConfig.LeaveDuel( this, from );
				else if( !IsUsingStone( from ) )
					DuelStoneConfig.JoinDuel( this, from );
				else
					from.SendAsciiMessage( "Duel stone error! Page a GM." );
		}

		public override void OnDoubleClickDead( Mobile from )
		{
			OnDoubleClick( from );
		}

		public bool IsUsingStone( Mobile from )
		{
			if( usingStone.Contains( from ) )
				return true;
			else
				return false;
		}

		public void ResetStone()
		{
			if( afterDuelLocation == new Point3D( 0, 0, 0 ) )
				return;

			if( combatant1 != null )
				combatant1.MoveToWorld( afterDuelLocation, Map.Felucca );
				//if (!this.StoneType.ToString().Contains("Loot") && moneyTaken > 0 && moneyTaken != null)
				//    combatant1.BankBox.AddItem(new Gold(moneyTaken));

			if( combatant2 != null )
				combatant2.MoveToWorld( afterDuelLocation, Map.Felucca );
				//if (!this.StoneType.ToString().Contains("Loot") && moneyTaken > 0 && moneyTaken != null)
				//    combatant2.BankBox.AddItem(new Gold(moneyTaken));

			if( combatant3 != null )
				combatant3.MoveToWorld( afterDuelLocation, Map.Felucca );
				//if (!this.StoneType.ToString().Contains("Loot") && moneyTaken > 0 && moneyTaken != null)
				//    combatant3.BankBox.AddItem(new Gold(moneyTaken));

			if( combatant4 != null )
				combatant4.MoveToWorld( afterDuelLocation, Map.Felucca );
				//if (!this.StoneType.ToString().Contains("Loot") && moneyTaken > 0 && moneyTaken != null)
				//    combatant4.BankBox.AddItem(new Gold(moneyTaken));

			combatant1 = null;
			combatant2 = null;
			combatant3 = null;
			combatant4 = null;
			endDuelTimer = null;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( ( DuelerLocation1 ) );
			writer.Write( ( DuelerLocation2 ) );
			writer.Write( ( AfterDuelLocation ) );

			writer.Write( Combatant1 );
			writer.Write( Combatant2 );
			writer.Write( Combatant3 );
			writer.Write( Combatant4 );

			writer.Write( StoneIdleTime );
			writer.Write( StoneMaxIdleTime );
			writer.Write( (int)( StoneType ) );
			writer.Write( ( DuelCost ) );

			writer.Write( ( IsLadderStone ) );

			//writer.WriteMobileList(UsingStone);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			usingStone = new ArrayList();

			duelerLocation1 = reader.ReadPoint3D();
			duelerLocation2 = reader.ReadPoint3D();
			afterDuelLocation = reader.ReadPoint3D();

			combatant1 = reader.ReadMobile();
			combatant2 = reader.ReadMobile();
			combatant3 = reader.ReadMobile();
			combatant4 = reader.ReadMobile();

			stoneIdleTime = reader.ReadInt();
			stoneMaxIdleTime = reader.ReadInt();
			eventType = (DuelStoneEventType)reader.ReadInt();
			moneyTaken = reader.ReadInt();

			isLadderStone = reader.ReadBool();

			ResetStone();
		}
	}
}