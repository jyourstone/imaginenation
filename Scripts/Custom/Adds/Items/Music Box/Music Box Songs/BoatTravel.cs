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
    public class BoatTravelSong : MusicBoxTrack
    {
        [Constructable]
        public BoatTravelSong()
            : base(1075163)
        {
            Song = MusicName.Sailing;
            //Name = "Boat Travel";
        }

        public BoatTravelSong(Serial s)
            : base(s)
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


