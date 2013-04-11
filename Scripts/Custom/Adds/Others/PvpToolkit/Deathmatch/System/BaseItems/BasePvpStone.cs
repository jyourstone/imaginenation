using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PvpToolkit.Items
{
    public class BasePvpStone : Item
    {
        #region Rules     
        private int m_BogusInt;//Here to fix issue with version 0 saves.
        private int m_MinSkill = 0;
        private int m_MaxSkill = 700;
        private pClassRules m_ClassRulesRule;
        private pKeepScoreRule m_KeepScoreRule;
        private pMagicWeaponRule m_MagicWeaponRule;
        private pMagicArmorRule m_MagicArmorRule;
        private pPotionRule m_PotionRule;
        private pBandaidRule m_BandageRule;
        private pMountRule m_MountRule;
        private pPetRule m_PetRule;

        [CommandProperty( AccessLevel.GameMaster )]
        public int MinSkill { get { return m_MinSkill; } set { m_MinSkill = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public int MaxSkill { get { return m_MaxSkill; } set { m_MaxSkill = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pClassRules ClassRulesRule { get { return m_ClassRulesRule; } set { m_ClassRulesRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pKeepScoreRule KeepScoreRule { get { return m_KeepScoreRule; } set { m_KeepScoreRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pMagicWeaponRule MagicWeaponRule { get { return m_MagicWeaponRule; } set { m_MagicWeaponRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pMagicArmorRule MagicArmorRule { get { return m_MagicArmorRule; } set { m_MagicArmorRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pPotionRule PotionRule { get { return m_PotionRule; } set { m_PotionRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pBandaidRule BandageRule { get { return m_BandageRule; } set { m_BandageRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pMountRule MountRule { get { return m_MountRule; } set { m_MountRule = value; } }
        [CommandProperty( AccessLevel.GameMaster )]
        public pPetRule PetRule { get { return m_PetRule; } set { m_PetRule = value; } }
        #endregion

        [Constructable]
        public BasePvpStone() : base( 0xEDD )
        {
            Movable = false;
        }

        public BasePvpStone( Serial serial )
            : base( serial )
        {

        }

        public virtual void AddPlayer( Mobile m )
        {

        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( 1 );

            writer.Write( ( int )m_ClassRulesRule );
            writer.Write( ( int )m_KeepScoreRule );
            writer.Write( ( int )m_MagicWeaponRule );
            writer.Write( ( int )m_MagicArmorRule );
            writer.Write( ( int )m_PotionRule );
            writer.Write( ( int )m_BandageRule );
            writer.Write( ( int )m_MountRule );
            writer.Write( ( int )m_PetRule );
            writer.Write( m_MinSkill );
            writer.Write( m_MaxSkill );
        }

        public virtual void WriteMountCollection( Dictionary<Serial, BaseCreature> dictionary, GenericWriter writer )
        {
            IDictionaryEnumerator myEnum = dictionary.GetEnumerator();

            int count = dictionary.Count;

            writer.Write( count );
            while( myEnum.MoveNext() )
            {
                Serial serial = ( Serial )myEnum.Key;
                Mobile m = (Mobile)myEnum.Value;

                writer.Write( serial );
                writer.Write( m );
            }
        }

        public virtual Dictionary<Serial, BaseCreature> ReadMountCollection(GenericReader reader)
        {
            Dictionary<Serial, BaseCreature> dictionary = new Dictionary<Serial, BaseCreature>();
            int count = reader.ReadInt();

            for( int i = 0; i < count; i++ )
            {
                Serial key = reader.ReadInt();
                BaseCreature bc = (BaseCreature)reader.ReadMobile();

                dictionary.Add(key, bc);
            }

            return dictionary;
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
                case 1:
                    {                        
                        m_ClassRulesRule = ( pClassRules )reader.ReadInt();
                        m_KeepScoreRule = ( pKeepScoreRule )reader.ReadInt();
                        m_MagicWeaponRule = ( pMagicWeaponRule )reader.ReadInt();
                        m_MagicArmorRule = ( pMagicArmorRule )reader.ReadInt();
                        m_PotionRule = ( pPotionRule )reader.ReadInt();
                        m_BandageRule = ( pBandaidRule )reader.ReadInt();
                        m_MountRule = ( pMountRule )reader.ReadInt();
                        m_PetRule = ( pPetRule )reader.ReadInt();
                        m_MinSkill = reader.ReadInt();
                        m_MaxSkill = reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        m_BogusInt = reader.ReadInt();
                        m_ClassRulesRule = ( pClassRules )reader.ReadInt();
                        m_KeepScoreRule = ( pKeepScoreRule )reader.ReadInt();
                        m_MagicWeaponRule = ( pMagicWeaponRule )reader.ReadInt();
                        m_MagicArmorRule = ( pMagicArmorRule )reader.ReadInt();
                        m_PotionRule = ( pPotionRule )reader.ReadInt();
                        m_BandageRule = ( pBandaidRule )reader.ReadInt();
                        m_MountRule = ( pMountRule )reader.ReadInt();
                        m_PetRule = ( pPetRule )reader.ReadInt();
                        break;
                    }
            }
        }
    }
}
