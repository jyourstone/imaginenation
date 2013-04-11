#define SVN

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Items;
using Server.Multis;

namespace Server.Items.MusicBox
{
    public interface IMusicBox
    {
        bool Turning { get; }
        void BeginTurn(Mobile from, double duration, MusicBoxComponent box);
    }

    public class MusicBoxComponent : AddonComponent, ISecurable
    {
		public static void Initialize() 
		{
            CommandSystem.Register("AddAllTracks", AccessLevel.GameMaster, new CommandEventHandler(AddAllTracks_OnCommand)); 
		}

        [Usage("AddAllTracks")]
        [Description("Adds all tracks to the MusicBox Addon.")]

        public static void AddAllTracks_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target the MusicBox Addon.");
            e.Mobile.Target = new InternalTarget();
        }

        public class InternalTarget : Target
        {
            public InternalTarget()
                : base(1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is MusicBoxComponent)
                {
                    from.SendMessage("All Tracks were added to the Music Box!");
                    ((MusicBoxComponent)o).AddAllTracks();
                }
                else
                    from.SendMessage("That is not a Music Box!");
            }
        }

#if SVN
        public override bool DisplayWeight { get { return false; } }
#endif
        public const int SONGS = 51;

        private int m_Range = 10;
        private Packet p;
        private SecureLevel m_Level = SecureLevel.Friends;
        private MusicName m_Music;
        private bool[] m_HasTrack;

        public bool[] HasTrack
        {
            get { return m_HasTrack; }
            set { m_HasTrack = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }


        [Constructable]
        public MusicBoxComponent(int itemID)
            : base(itemID)
        {
            Name = "Musicbox: Stopped.";
            m_HasTrack = new bool[SONGS];
            for (int x = 0; x < SONGS; x++)
            {
                m_HasTrack[x] = false;
            }
            AddTrack(new MusicBoxTrack(1075144));
            m_Music = MusicName.Britain1;
            m_HasTrack[12] = true; //Default Song: Britain1
        }

        public MusicBoxComponent(Serial serial)
            : base(serial)
        {
        }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
            list.Add(new MusicBoxEntry(from, this));
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            MusicBoxAddon box;
            if (Addon is MusicBoxAddon)
            {
                box = (MusicBoxAddon)Addon;
                if (box.Turning) list.Add("Song Playing:");
                else list.Add("Active Song:");
            }
            else list.Add("Active Song:");
            list.Add(m_Locals[GetSong()]);
        }

        private static int[] Length = new int[] { 106, 128, 114, 126, 74, 68, 114, 219, 285, 118, 
            48, 135, 53, 39, 59, 36, 17, 62, 66, 56, 56, 48, 61, 36, 35, 60, 58, 50, 18, 22, 18, 
            18, 48, 50, 52, 20, 17, 98, 54, 58, 58, 60, 148, 88, 227, 120, 321, 120, 120, 36, 120 };

        private static int[] m_Locals = new int[] { 1075131, 1075132, 1075133, 1075134, 1075135, 1075136, 1075137, 
            1075138, 1075139, 1075140, 1075142, 1075143, 1075144, 1075145, 1075146, 1075147, 1075148, 1075149, 
            1075150, 1075151, 1075152, 1075154, 1075155, 1075156, 1075157, 1075158, 1075159, 1075160, 1075163, 
            1075164, 1075165, 1075166, 1075167, 1075168, 1075170, 1075171, 1075172, 1075173, 1075174, 1075175, 
            1075176, 1075177, 1075178, 1075179, 1075180, 1075181, 1075182, 1075183, 1075184, 1075185, 1075186
        };

        private static MusicName[] m_Songs = new MusicName[] { 
            MusicName.GwennoConversation,   MusicName.GoodEndGame,      MusicName.GoodVsEvil,   MusicName.GreatEarthSerpents,
            MusicName.Humanoids_U9,         MusicName.MinocNegative,    MusicName.Paws,         MusicName.SelimsBar,
            MusicName.SerpentIsleCombat_U7, MusicName.ValoriaShips,     MusicName.OldUlt02,     MusicName.Serpents,
            MusicName.Britain1,             MusicName.Britain2,         MusicName.Bucsden,      MusicName.Jhelom,
            MusicName.LBCastle,             MusicName.Magincia,         MusicName.Minoc,        MusicName.Ocllo,
            MusicName.Samlethe,             MusicName.Skarabra,         MusicName.Trinsic,      MusicName.Vesper,
            MusicName.Wind,                 MusicName.Yew,              MusicName.Cave01,       MusicName.Dungeon9,
            MusicName.Sailing,              MusicName.Tavern01,         MusicName.Tavern02,     MusicName.Tavern03,
            MusicName.Tavern04,             MusicName.Combat1,          MusicName.Combat3,      MusicName.Death,
            MusicName.Victory,              MusicName.BTCastle,         MusicName.Nujelm,       MusicName.Dungeon2,
            MusicName.Cove,                 MusicName.Moonglow,         MusicName.Zento,        MusicName.TokunoDungeon,
            MusicName.Taiko,                MusicName.DreadHornArea,    MusicName.ElfCity,      MusicName.MelisandesLair,
            MusicName.ParoxysmusLair,       MusicName.Linelle,          MusicName.GrizzleDungeon
        };

        public static int[] Locals
        {
            get { return m_Locals; }
        }

        public static MusicName[] Songs
        {
            get { return m_Songs; }
        }

        public void AddTrack(MusicBoxTrack track)
        {
            for (int x = 0; x < MusicBoxComponent.SONGS; x++)
            {
                if (track.Song == MusicBoxComponent.Songs[x])
                {
                    m_HasTrack[x] = true;
                    track.Delete();
                }
            }
        }

        public double GetSongDuration()
        {
            for (int x = 0; x < SONGS; x++)
            {
                if (m_Music == m_Songs[x])
                    return (double)Length[x];
            }
            return (double)18;
        }

        public int GetSong()
        {
            for (int x = 0; x < SONGS; x++)
            {
                if (m_Music == m_Songs[x])
                    return x;
            }
            return -1;
        }

        public void AddAllTracks()
        {
            for (int x = 0; x < MusicBoxComponent.SONGS; x++)
            {
                m_HasTrack[x] = true;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range
        {
            get { return m_Range; }
            set { m_Range = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MusicName Music
        {
            get { return m_Music; }
            set { m_Music = value;
                if (GetSong() < 0) m_Music = MusicName.Britain1;
                InvalidateProperties(); }
        }

        public virtual void PlayMusic(Mobile m)
        {
            if (m_Music != MusicName.Invalid && (m.NetState != null || m.Player))
            {
                p = Network.PlayMusic.GetInstance(m_Music);
                m.Send(p);
            }
        }

        public virtual void StopMusic(Mobile m)
        {
            if (m_Music != MusicName.Invalid && (m.NetState != null || m.Player))
            {
                p = Network.PlayMusic.GetInstance(m.Region.DefaultMusic);
                m.Send(p);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.WriteEncodedInt((int)m_Level); //version 1

            writer.Write((int)m_Range);
            writer.Write((int)m_Music);
            for (int x = 0; x < SONGS; x++)
            {
                writer.Write((bool)m_HasTrack[x]);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        m_Level = (SecureLevel)reader.ReadEncodedInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Range = reader.ReadInt();
                        m_Music = (MusicName)reader.ReadInt();
                        m_HasTrack = new bool[SONGS];
                        for (int x = 0; x < SONGS; x++)
                        {
                            m_HasTrack[x] = reader.ReadBool();
                        }
                        break;
                    }
            }
            Name = "Musicbox: Stopped.";
        }

    }

    public class MusicBoxEntry : ContextMenuEntry
    {
        private Mobile m_From;
        private MusicBoxComponent m_Box;

        public MusicBoxEntry(Mobile from, MusicBoxComponent box)
            : base(10003, 2)
        {
            m_From = from;
            m_Box = box;
        }

        public override void OnClick()
        {
            m_From.SendGump(new MusicBoxGump(m_From, m_Box));
        }
    }

    public class MusicBoxGump : Gump
    {
        private MusicBoxComponent m_Box;
        private Mobile m_From;
        public MusicBoxComponent Box { get { return m_Box; } }

        public MusicBoxGump(Mobile from, MusicBoxComponent box)
            : base(30, 30)
        {
            from.CloseGump(typeof(MusicBoxGump));
            m_Box = box;
            m_From = from;
            AddBackground(0, 0, 600, 480, 9400);
            AddPage(0);
            AddHtmlLocalized(250, 10, 180, 20, 1075130, 2124, false, false);
            int Xpos = 26, Ypos = 42;
            int z = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    bool has = (box.HasTrack[z]);
                    AddSongEntry(Xpos, Ypos, MusicBoxComponent.Locals[z], has ? 177 : 547, z + 100, has);
                    Ypos += 20;
                    z++;
                }
                Xpos += 200;
                Ypos = 42;
            }
            AddLabel(250, 460, 2124, "Add Songs");
            AddButton(220, 460, 1210, 1210, 99, GumpButtonType.Reply, 0);
        }

        private void AddSongEntry(int x, int y, int local, int color, int song, bool active)
        {
            AddHtmlLocalized(x, y, 180, 20, local, color, false, false);
            if (active) AddButton(x - 20, y, 1210, 1210, song, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 99)
            {
                m_From.Target = new InternalTarget(this);
            }
            else if (info.ButtonID > 99)
            {
                m_Box.Music = MusicBoxComponent.Songs[info.ButtonID - 100];
                m_From.SendMessage("Now active:");
                m_From.SendLocalizedMessage(MusicBoxComponent.Locals[info.ButtonID - 100]);
            }
            else
                return;
        }

        public void Target(MusicBoxTrack target)
        {
            for (int x = 0; x < MusicBoxComponent.SONGS; x++)
            {
                if (target.Song == MusicBoxComponent.Songs[x])
                {
                    if (!m_Box.HasTrack[x])
                    {
                        m_Box.AddTrack(target);
                        m_From.SendMessage("Song successfully added.");
                        m_From.Target = new InternalTarget(this);
                    }
                    else
                    {
                        m_From.SendMessage("That song is already added.");
                        m_From.SendGump(new MusicBoxGump(m_From, m_Box));
                    }
                }
            }
        }

        public class InternalTarget : Target
        {
            private MusicBoxGump m_Owner;

            public InternalTarget(MusicBoxGump owner)
                : base(1, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is MusicBoxTrack)
                {
                    m_Owner.Target((MusicBoxTrack)o);
                }
                else
                    from.SendMessage("That is not a song track!");
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    from.SendGump(new MusicBoxGump(from, m_Owner.Box));
            }
        }
    }
}