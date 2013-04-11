using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Items.MusicBox
{
    public class MusicBoxTrack : Item
    {
        private int m_Track;
        public int Track { get { return m_Track; } set { m_Track = value; InvalidateProperties(); } }
        private MusicName m_Song;
        public MusicName Song { get { return m_Song; } set { m_Song = value; InvalidateProperties(); } }

        public override int LabelNumber { get { return m_Track; } }

        public MusicBoxTrack(int track)
            : base(0x2830)
        {
            m_Track = track;
            //Name = "Music Box Song Track";
            Weight = 1.0;
        }

        public MusicBoxTrack(Serial s)
            : base(s)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Music box song track ({0})", Song);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendLocalizedMessage(LabelNumber);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Track);
            writer.Write((int)m_Song);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Track = reader.ReadInt();
            m_Song = (MusicName)reader.ReadInt();
        }
    }
}