using System;

namespace Server.Custom.PvpToolkit.DMatch.Items
{
    public enum PowerUpType
    { //Type        //ID    //Hue
        Int,        //6175  //04
        Dex,        //6176  //70
        Str,        //6177  //44
        Rage,       //6178  //31
        Spirit,     //6179  //695
        Supremacy,  //6180  //718
        Fury        //6181  //1259
    }

    public class DMPowerUpSpawner : Item
    {
        private PowerUpType m_PowerUp = PowerUpType.Str;

        [CommandProperty( AccessLevel.GameMaster )]
        public PowerUpType PowerUp
        {
            get
            {
                return m_PowerUp;
            }
            set
            {
                switch( value )
                {
                    case PowerUpType.Dex:
                    {
                        ItemID = 6176;
                        Name = "dexterity power-up";
                        Hue = 70;
                        break;
                    }
                    case PowerUpType.Fury:
                    {
                        ItemID = 6181;
                        Name = "fury power-up";
                        Hue = 1259;
                        break;
                    }
                    case PowerUpType.Int:
                    {
                        ItemID = 6175;
                        Name = "intelligence power-up";
                        Hue = 04;
                        break;
                    }
                    case PowerUpType.Rage:
                    {
                        ItemID = 6178;
                        Name = "rage power-up";
                        Hue = 31;
                        break;
                    }
                    case PowerUpType.Spirit:
                    {
                        ItemID = 6179;
                        Name = "spirit power-up";
                        Hue = 695;
                        break;
                    }
                    case PowerUpType.Str:
                    {
                        ItemID = 6177;
                        Name = "strength power-up";
                        Hue = 44;
                        break;
                    }
                    case PowerUpType.Supremacy:
                    {
                        ItemID = 6180;
                        Name = "supremacy power-up";
                        Hue = 718;
                        break;
                    }
                }

                m_PowerUp = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public DMPowerUpSpawner()
        {
            PowerUp = PowerUpType.Str;
            Visible = true;
            Movable = false;
        }

        [Constructable]
        public DMPowerUpSpawner( PowerUpType type )
        {
            PowerUp = type;
            Visible = true;
            Movable = false;
        }

        public override bool OnMoveOver( Mobile m )
        {
            if( !Visible )
                return true;

            if( m == null || !m.Player )
                return true;

            if(!PvpCore.IsInDeathmatch( m ) )
                return true;

            Visible = false;

            StatMod mod = m.GetStatMod( String.Format( "[DM] {0}", m_PowerUp ) );

            if( mod != null )
                m.RemoveStatMod( String.Format( "[DM] {0}", m_PowerUp ) );

            switch( m_PowerUp )
            {
                case PowerUpType.Dex:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.Dex, String.Format("[DM] {0}", m_PowerUp ), 5, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Fury:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.All, String.Format( "[DM] {0}", m_PowerUp ), 5, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Int:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.Int, String.Format( "[DM] {0}", m_PowerUp ), 5, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Rage:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.All, String.Format( "[DM] {0}", m_PowerUp ), 10, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Spirit:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.Int, String.Format( "[DM] {0}", m_PowerUp ), 15, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Str:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.Str, String.Format( "[DM] {0}", m_PowerUp ), 5, TimeSpan.FromMinutes( 1.0 ) ) );
                    break;
                }
                case PowerUpType.Supremacy:
                {
                    m.FixedParticles( 0x3779, 10, 15, 5009, EffectLayer.Waist );
                    m.PlaySound( 0x1E6 );
                    m.AddStatMod( new StatMod( StatType.All, String.Format( "[DM] {0}", m_PowerUp ), 20, TimeSpan.FromSeconds( 20.0 ) ) );
                    break;
                }
            }

            new RespawnTimer( this ).Start();
            return true;
        }

        public DMPowerUpSpawner( Serial serial )
            : base( serial )
        {

        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( 0 );
            writer.Write( ( int )m_PowerUp );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                {
                    m_PowerUp = ( PowerUpType )reader.ReadInt();
                    break;
                }
            }
        }
    }

    public class RespawnTimer : Timer
    {
        private readonly DMPowerUpSpawner m_Spawner;

        public RespawnTimer( DMPowerUpSpawner spawner ) : base( TimeSpan.FromSeconds( 60.0 ) )
        {
            m_Spawner = spawner;
        }

        protected override void OnTick()
        {
 	        m_Spawner.Visible = true;
            Stop();
        }
    }
}
