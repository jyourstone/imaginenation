using System;
using Server;
using System.Collections;
using Server.Items;

namespace Server.Items.MusicBox
{
    public class MusicBoxAddon : BaseAddon, IMusicBox
    {
        public override BaseAddonDeed Deed { get{ switch (ItemID) {
            case 0x2AFA:
            case 0x2AF9: return new MusicBoxSouthDeed();
            case 0x2AFE:
            case 0x2AFD: 
            default: return new MusicBoxEastDeed(); } } }

        [Constructable]
        public MusicBoxAddon()
            : this(0x2AFD)
        {
        }

        [Constructable]
        public MusicBoxAddon(int itemID)
        {
            AddComponent(new MusicBoxComponent(itemID), 0, 0, 0);
        }

        public MusicBoxAddon(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private Timer m_Timer;

        public override void OnComponentLoaded(AddonComponent c)
        {
            switch (c.ItemID)
            {
                case 0x2AFA:
                case 0x2AFE: --c.ItemID; break;
            }
        }

        public override void OnComponentUsed(AddonComponent c, Mobile from)
        {
            if (!(c is MusicBoxComponent))
                return;

            MusicBoxComponent box = c as MusicBoxComponent;

            if (box.GetSong() < 0)
                return;

            TogglePlayer(from, box, false);

            if (Turning) EndTurn(from, box, false);
            else BeginTurn(from, box.GetSongDuration(), box);
        }

        private void TogglePlayer(Mobile from, MusicBoxComponent box, bool stopping)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                if (Turning || stopping)
                    box.StopMusic(from);
                else
                    box.PlayMusic(from);
            }
            else
            {
                if (!Turning && !stopping) StopNearbyMusicBoxesPlaying(from, box.Range);
                IPooledEnumerable eable = from.Map.GetMobilesInRange(from.Location, box.Range);
                foreach (Mobile mob in eable)
                {
                    if (Turning || stopping)
                        box.StopMusic(mob);
                    else
                        box.PlayMusic(mob);
                }
                eable.Free();
            }
        }

        private void StopNearbyMusicBoxesPlaying(Mobile from, int range)
        {
            IPooledEnumerable eable = from.Map.GetItemsInRange(from.Location, range);
            foreach (Item i in eable)
            {
                if (i is MusicBoxAddon && i != this)
                {
                    MusicBoxComponent box = ((MusicBoxAddon)i).Components[0] as MusicBoxComponent;
                    if (((MusicBoxAddon)i).Turning)
                        ((MusicBoxAddon)i).EndTurn(from, box, true);
                }
            }
        }

        public bool Turning { get { return m_Timer != null; } }

        public void BeginTurn(Mobile from, double duration, MusicBoxComponent box)
        {
            m_Timer = new TurnTimer(this, from, duration, box);
            m_Timer.Start();
            box.Name = "Music box: Playing.";

            foreach (AddonComponent c in Components)
            {
                switch (c.ItemID)
                {
                    case 0x2AF9:
                    case 0x2AFD: ++c.ItemID; break;
                }
            }
        }

        public void EndTurn(Mobile from, MusicBoxComponent box, bool autostop)
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;
            box.Name = "Music box: Stopped.";

            if (autostop) TogglePlayer(from, box, true);

            foreach (AddonComponent c in Components)
            {
                switch (c.ItemID)
                {
                    case 0x2AFA:
                    case 0x2AFE: --c.ItemID; break;
                }
            }
        }

        private class TurnTimer : Timer
        {
            private MusicBoxAddon m_Addon;
            private MusicBoxComponent m_Box;
            private Mobile m_From;

            public TurnTimer(MusicBoxAddon addon, Mobile from, double duration, MusicBoxComponent box)
                : base(TimeSpan.FromSeconds(duration))
            {
                m_Addon = addon;
                m_From = from;
                m_Box = box;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                m_Addon.EndTurn(m_From, m_Box, true);
            }
        }
    }

    public class MusicBoxEastDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new MusicBoxAddon(0x2AFD); } }

        [Constructable]
        public MusicBoxEastDeed()
        {
            Name = "Music box east deed";
        }

        public MusicBoxEastDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MusicBoxSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new MusicBoxAddon(0x2AF9); } }

        [Constructable]
        public MusicBoxSouthDeed()
        {
            Name = "Music box south deed";
        }

        public MusicBoxSouthDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}