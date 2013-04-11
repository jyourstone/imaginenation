using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class AdvancedTeleporter : Teleporter
	{
        public TeleportTimer m_TeleportTimer;

		[Constructable]
        public AdvancedTeleporter()
		{
			Movable = false;
			Visible = false;
		    AllowDead = true;
		    AllowMounted = true;
		    DeadTeleInstantly = true;
		    AllowIsInEvent = true;
		    OnMoveOverHue = 2544;
		    SoundID = -1;
			Name = "Advanced teleporter";
		}

        public AdvancedTeleporter(Serial serial): base(serial)
		{
        }

        #region Getters & Setters
        [CommandProperty(AccessLevel.GameMaster)]
        public int DelayWithCount { get; set; }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool MustStandStill{ get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MustBeFullyHealed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowDead { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeadTeleInstantly { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResurrectDead { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int OnMoveOverHue { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowMounted { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowIsInEvent { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string OnMoveOverMessage { get; set; }
        #endregion

        public override bool OnMoveOff(Mobile m)
        {
            if (MustStandStill)
            {
                if (m == null || (m_TeleportTimer == null || m.SolidHueOverride != OnMoveOverHue))
                    return true;

                m.LocalOverheadMessage(MessageType.Regular, 906, true, "You must stand still to use this!");
                m_TeleportTimer.Stop();
                m.SolidHueOverride = -1;
            }

            return base.OnMoveOff(m);
        }

        public override bool OnMoveOver( Mobile m )
		{
            if (m.SolidHueOverride != -1)
                return true;

            if (m.IsInEvent && !AllowIsInEvent)
            {
                m.SendAsciiMessage("You cannot use this while in an event");
                return true;
            }

            if (m is PlayerMobile && !m.Alive && !AllowDead)
            {
                m.SendAsciiMessage("You cannot use this while dead");
                return true;
            }

            if (!AllowMounted && m.Player && m.Mounted)
            {
                m.SendAsciiMessage("You cannot use this while mounted");
                return true;
            }

		    if( m is PlayerMobile && ((!m.Alive && AllowDead && DeadTeleInstantly) || DelayWithCount <= 0) )
			{
                if (!base.OnMoveOver(m))
                {
                    if (!m.Alive && ResurrectDead)
                        m.Resurrect();

                    return false;
                }
                return true;
			}

            if ((m is PlayerMobile) && (m.Hits < m.HitsMax) && MustBeFullyHealed && m.Alive)
            {
                m.LocalOverheadMessage(MessageType.Regular, 906, true, "You must be fully healed to use this!");
                return true;
            }

            if (CombatCheck && SpellHelper.CheckCombat(m))
            {
                m.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                return true;
            }

            if (!AllowCriminals && (m.Criminal || Misc.NotorietyHandlers.IsGuardCandidate(m)))
            {
                m.SendAsciiMessage("Criminals or murderers can't use this teleporter!");
                return true;
            }

            if (m is PlayerMobile && m.SolidHueOverride != OnMoveOverHue)
            {
                m_TeleportTimer = new TeleportTimer(this, m);
                m_TeleportTimer.Start();
                m.SolidHueOverride = OnMoveOverHue;
                if (!String.IsNullOrEmpty(OnMoveOverMessage))
                    m.SendAsciiMessage(OnMoveOverMessage);
                return true;
            }

		    return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 2 ); // version

            //version 2
            writer.Write(OnMoveOverMessage);

            //version 1
            writer.Write(AllowIsInEvent);

            writer.Write(DelayWithCount);
		    writer.Write(MustStandStill);
		    writer.Write(MustBeFullyHealed);
		    writer.Write(AllowDead);
		    writer.Write(DeadTeleInstantly);
		    writer.Write(ResurrectDead);
		    writer.Write(OnMoveOverHue);
            writer.Write(AllowMounted);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    OnMoveOverMessage = reader.ReadString();
                    goto case 1;
                case 1:
                    AllowIsInEvent = reader.ReadBool();
                    goto case 0;
                case 0:
                    DelayWithCount = reader.ReadInt();
                    MustStandStill = reader.ReadBool();
                    MustBeFullyHealed = reader.ReadBool();
                    AllowDead = reader.ReadBool();
                    DeadTeleInstantly = reader.ReadBool();
                    ResurrectDead = reader.ReadBool();
                    OnMoveOverHue = reader.ReadInt();
                    AllowMounted = reader.ReadBool();
                    break;
            }
		}
	}

	public class TeleportTimer : Timer
	{
		private readonly Mobile m_Mobile;
	    private int m_State;
		private readonly AdvancedTeleporter m_Teleporter;
	    private readonly int m_TeleLocX;
        private readonly int m_TeleLocY;

		public TeleportTimer( AdvancedTeleporter teleporter, Mobile m ) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
		{
			m_Teleporter = teleporter;
            m_Mobile = m;
		    m_State = m_Teleporter.DelayWithCount + 1;
		    m_TeleLocX = m_Teleporter.Location.X;
		    m_TeleLocY = m_Teleporter.Location.Y;
		}

		protected override void OnTick()
		{
			m_State--;

            if (m_Mobile == null)
                Stop();

            else if (m_Teleporter.Deleted || m_Teleporter == null)
            {
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (!m_Teleporter.AllowIsInEvent && m_Mobile.IsInEvent)
            {
                m_Mobile.SendAsciiMessage("You cannot use this while in an event");
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (!m_Teleporter.AllowDead && !m_Mobile.Alive)
            {
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (m_Teleporter.CombatCheck && SpellHelper.CheckCombat(m_Mobile))
            {
                m_Mobile.LocalOverheadMessage(MessageType.Regular, 906, 1005564, ""); // Wouldst thou flee during the heat of battle??
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (!m_Teleporter.AllowCriminals && (m_Mobile.Criminal || Misc.NotorietyHandlers.IsGuardCandidate(m_Mobile)))
            {
                m_Mobile.LocalOverheadMessage(MessageType.Regular, 906, true, "Criminals or murderers can't use this teleporter!");
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (m_Teleporter.MustBeFullyHealed && m_Mobile.Alive && m_Mobile.Hits < m_Mobile.HitsMax) // Health Cancel
            {
                m_Mobile.LocalOverheadMessage(MessageType.Regular, 906, true, "You must be fully healed to use this!");
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (m_Teleporter.MustStandStill && (m_Mobile.Location.X != m_TeleLocX || m_Mobile.Location.Y != m_TeleLocY)) // Mobile changed location, most likely recalled
            {
                m_Mobile.LocalOverheadMessage(MessageType.Regular, 906, true, "You must stand still to use this!");
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }

            else if (m_State <= 0) // Complete
            {
                m_Mobile.SolidHueOverride = -1;

                Map map = m_Teleporter.MapDest;

                if (map == null || map == Map.Internal)
                    map = m_Mobile.Map;

                Point3D p = m_Teleporter.PointDest;

                if (p == Point3D.Zero)
                    p = m_Mobile.Location;

                BaseCreature.TeleportPets(m_Mobile, p, map);

                bool sendEffect = (!m_Mobile.Hidden || m_Mobile.AccessLevel == AccessLevel.Player);

                if (m_Teleporter.SourceEffect && sendEffect)
                    Effects.SendLocationEffect(m_Mobile.Location, m_Mobile.Map, 0x3728, 10, 10);

                m_Mobile.MoveToWorld(p, map);

                if (m_Teleporter.DestEffect && sendEffect)
                    Effects.SendLocationEffect(m_Mobile.Location, m_Mobile.Map, 0x3728, 10, 10);

                if (m_Teleporter.SoundID > 0 && sendEffect)
                    Effects.PlaySound(m_Mobile.Location, m_Mobile.Map, m_Teleporter.SoundID);

                if (!m_Mobile.Alive && m_Teleporter.ResurrectDead)
                    m_Mobile.Resurrect();

                if (m_Teleporter.DeleteOnUse)
                    m_Teleporter.Delete();

                Stop();
            }

            else if (m_State >= 1)
                m_Mobile.PublicOverheadMessage(MessageType.Spell, m_Mobile.SpeechHue, true, (m_State).ToString(), false);
		}
	}
}