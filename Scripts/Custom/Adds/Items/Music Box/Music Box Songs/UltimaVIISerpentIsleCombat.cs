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
    public class UltimaVIISerpentIsleCombatSong : MusicBoxTrack
    {
        [Constructable]
        public UltimaVIISerpentIsleCombatSong()
            : base(1075139)
        {
            Song = MusicName.SerpentIsleCombat_U7;
            //Name = @"Ultima VII / Serpent Isle Combat";
        }

        public UltimaVIISerpentIsleCombatSong(Serial s)
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


