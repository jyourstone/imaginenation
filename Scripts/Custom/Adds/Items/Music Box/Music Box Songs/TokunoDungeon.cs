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
    public class TokunoDungeonSong : MusicBoxTrack
    {
        [Constructable]
        public TokunoDungeonSong()
            : base(1075179)
        {
            Song = MusicName.TokunoDungeon;
            //Name = "Tokuno Dungeon";
        }

        public TokunoDungeonSong(Serial s)
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


