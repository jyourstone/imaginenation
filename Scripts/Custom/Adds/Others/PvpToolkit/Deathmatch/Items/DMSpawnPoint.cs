namespace Server.Custom.PvpToolkit.DMatch.Items
{
    public class DMSpawnPoint : Item
    {
        private DMStone m_Link;

        [CommandProperty( AccessLevel.GameMaster )]
        public DMStone StoneLink
        {
            get
            {
                return m_Link;
            }
            set
            {
                if (m_Link != value && m_Link != null)
                    m_Link.RemoveSpawnPoint(this);

                m_Link = value;
                if ( m_Link != null )
                    m_Link.AddSpawnPoint(this);
            }
        }

        [Constructable]
        public DMSpawnPoint()
            : base( 0x1DB8 )
        {
            Name = "a deathmatch spawnpoint";
            Visible = false;
            Movable = false;
        }

        public DMSpawnPoint( Serial serial )
            : base( serial ) { }


        public override void OnDelete()
        {
            if (m_Link != null)
                m_Link.RemoveSpawnPoint(this);

            base.OnDelete();
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( 0 );
            writer.Write( m_Link );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        m_Link = ( DMStone )reader.ReadItem();
                        break;
                    }
            }
        }
    }
}
