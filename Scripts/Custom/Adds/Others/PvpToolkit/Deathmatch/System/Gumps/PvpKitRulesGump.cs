using System;
using Server.Custom.PvpToolkit.Items;
using Server.Gumps;

namespace Server.Custom.PvpToolkit.Gumps
{
	public class PvpKitRulesGump : Gump
	{
        private readonly BasePvpStone m_Stone;

        public PvpKitRulesGump( BasePvpStone stone, string stoneType, string systemVer )
            : base( 0, 0 )
        {
            m_Stone = stone;
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(26, 15, 366, 479, 2620);
			AddImageTiled(29, 48, 364, 4, 2700);
			AddLabel(36, 25, 4, String.Format("Sorious' PvpKit - {0} System Options - V. {1}", stoneType, systemVer) );
			AddLabel(36, 56, 52, String.Format("Check all the options you wish to allow in the {0}:", stoneType) );
			AddLabel(36, 75, 52, @"Skill Rules");
			AddLabel(36, 185, 52, @"Class Rules");
			AddBackground(37, 207, 41, 181, 9200);
            AddGroup( 0 );
            AddRadio( 42, 214, 9728, 9729, ( stone.ClassRulesRule == pClassRules.PureMageOnly ), ( int )Buttons.PureMage );
            AddRadio( 42, 280, 9728, 9729, ( stone.ClassRulesRule == pClassRules.DexerOnly ), ( int )Buttons.Dexer );
            AddRadio( 42, 247, 9728, 9729, ( stone.ClassRulesRule == pClassRules.TankMageOnly ), ( int )Buttons.TankMage );
            AddRadio( 42, 314, 9728, 9729, ( stone.ClassRulesRule == pClassRules.TamerOnly ), ( int )Buttons.Tamer );
            AddRadio( 42, 347, 9728, 9729, ( stone.ClassRulesRule == pClassRules.NoRule ), ( int )Buttons.NoRule );
			AddLabel(86, 219, 43, @"Pure Mage");
			AddLabel(86, 252, 43, @"Tank Mage");
			AddLabel(86, 285, 43, @"Dexer");
            AddLabel(87, 319, 43, @"Tamer");
			AddLabel(87, 352, 43, @"No Rule");
			AddLabel(219, 252, 43, @"Magic Weapons");
			AddLabel(219, 416, 43, @"Keep Score");
			AddLabel(219, 219, 43, @"Magic Armor");
			AddLabel(219, 285, 43, @"Potions");
			AddLabel(219, 319, 43, @"Bandaids");
			AddLabel(219, 352, 43, @"Mounts");
			AddBackground(168, 207, 41, 245, 9200);
			AddLabel(219, 384, 43, @"Pets");
			AddImage(289, 82, 5536);
			AddButton(58, 409, 2328, 2329, (int)Buttons.OkButton, GumpButtonType.Reply, 0);
			AddLabel(89, 431, 0, @"OK");
			AddLabel(36, 91, 43, @"Enter the minimun skill required to play.");
			AddBackground(36, 113, 254, 24, 9200);
			AddLabel(36, 138, 43, @"Enter the maximum skill required to play.");
			AddBackground(36, 160, 254, 24, 9200);
			AddItem(306, 253, 5185);
            AddItem( 282, 215, 5095 );
            AddItem( 260, 283, 3852 );
            AddItem( 282, 289, 3853 );
            AddItem( 307, 290, 3848 );
            AddItem( 286, 285, 3851 );
            AddItem( 289, 290, 3850 );
            AddItem( 278, 285, 3849 );
            AddItem( 267, 319, 3817 );
            AddItem( 299, 324, 3616 );
            AddItem( 278, 326, 3817 );
            AddItem( 271, 346, 8479 );
            AddItem( 247, 377, 8473 );
            AddItem( 265, 384, 8476 );
            AddItem( 279, 384, 8482 );
            AddItem( 298, 380, 8478 );
            AddItem( 284, 421, 3643 );
			AddTextEntry(42, 115, 238, 20, 4, (int)Buttons.MinEntry, stone.MinSkill.ToString());
            AddTextEntry( 42, 162, 238, 20, 4, ( int )Buttons.MaxEntry, stone.MaxSkill.ToString() );
			AddCheck(173, 214, 9728, 9729, stone.MagicArmorRule == pMagicArmorRule.Allowed, (int)Buttons.MagicArmorChkBox);
			AddCheck(173, 280, 9728, 9729, stone.PotionRule == pPotionRule.Allowed, (int)Buttons.PotionChkBox);
			AddCheck(173, 247, 9728, 9729, stone.MagicWeaponRule == pMagicWeaponRule.Allowed, (int)Buttons.MagicWeaponsChkBox);
			AddCheck(173, 314, 9728, 9729, stone.BandageRule == pBandaidRule.Allowed, (int)Buttons.BandagesChkBox);
			AddCheck(173, 347, 9728, 9729, stone.MountRule == pMountRule.Allowed, (int)Buttons.MountsChkBox);
			AddCheck(173, 380, 9728, 9729, stone.PetRule == pPetRule.Allowed, (int)Buttons.PetsChkBox);
			AddCheck(173, 413, 9728, 9729, stone.KeepScoreRule == pKeepScoreRule.Yes, (int)Buttons.KeepScoreChkBox);

		}
		
		public enum Buttons
		{
            PureMage = 15,
            Dexer = 14,
            TankMage = 13,
            Tamer = 12,
            NoRule = 11,
            OkButton = 10,
            MinEntry = 9,
            MaxEntry = 8,
            MagicArmorChkBox = 7,
            PotionChkBox = 6,
            MagicWeaponsChkBox = 5,
            BandagesChkBox = 4,
            MountsChkBox = 3,
            PetsChkBox = 2,
			KeepScoreChkBox = 1,
		}

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile m = sender.Mobile;

            if( m == null )
                return;

            switch( info.ButtonID )
            {
                case 10:
                    {
                        if( info.IsSwitched( 11 ) )
                            m_Stone.ClassRulesRule = pClassRules.NoRule;
                        else if( info.IsSwitched( 12 ) )
                            m_Stone.ClassRulesRule = pClassRules.TamerOnly;
                        else if( info.IsSwitched( 14 ) )
                            m_Stone.ClassRulesRule = pClassRules.DexerOnly;
                        else if( info.IsSwitched( 13 ) )
                            m_Stone.ClassRulesRule = pClassRules.TankMageOnly;
                        else
                            m_Stone.ClassRulesRule = pClassRules.PureMageOnly;


                        if( info.IsSwitched( 2 ) )
                            m_Stone.PetRule = pPetRule.Allowed;
                        else
                            m_Stone.PetRule = pPetRule.NotAllowed;

                        if( info.IsSwitched( 3 ) )
                            m_Stone.MountRule = pMountRule.Allowed;
                        else
                            m_Stone.MountRule = pMountRule.NotAllowed;

                        if( info.IsSwitched( 4 ) )
                            m_Stone.BandageRule = pBandaidRule.Allowed;
                        else
                            m_Stone.BandageRule = pBandaidRule.NotAllowed;

                        if( info.IsSwitched( 6 ) )
                            m_Stone.PotionRule = pPotionRule.Allowed;
                        else
                            m_Stone.PotionRule = pPotionRule.NotAllowed;

                        if( info.IsSwitched( 7 ) )
                            m_Stone.MagicArmorRule = pMagicArmorRule.Allowed;
                        else
                            m_Stone.MagicArmorRule = pMagicArmorRule.NotAllowed;

                        if( info.IsSwitched( 5 ) )
                            m_Stone.MagicWeaponRule = pMagicWeaponRule.Allowed;
                        else
                            m_Stone.MagicWeaponRule = pMagicWeaponRule.NotAllowed;

                        if( info.IsSwitched( 1 ) )
                            m_Stone.KeepScoreRule = pKeepScoreRule.Yes;
                        else
                            m_Stone.KeepScoreRule = pKeepScoreRule.No;

                        TextRelay min = info.GetTextEntry( 9 );
                        TextRelay max = info.GetTextEntry( 8 );

                        int minskill = 0;
                        int maxskill = 0;

                        try
                        {
                            minskill = Int32.Parse( min.Text );
                            maxskill = Int32.Parse( max.Text );
                        }
                        catch
                        {
                            m.SendMessage( "Either the minimun skill value or the maximum was not input correctly. Please try again" );
                            m.SendGump( this );
                            return;
                        }

                        m_Stone.MinSkill = minskill;
                        m_Stone.MaxSkill = maxskill;

                        m.SendMessage( "Rules set for this stone." );
                        break;
                   }
            }
        }
	}
}