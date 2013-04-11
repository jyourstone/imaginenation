using System;
using Server.Custom.PvpToolkit.Items;
using Server.Gumps;

namespace Server.Custom.PvpToolkit.Gumps
{
	public class PvpAcceptGump : Gump
	{
        private readonly BasePvpStone m_Stone;

		public PvpAcceptGump( BasePvpStone stone )
			: base( 0, 0 )
		{
            string skillrule = String.Format( "Min skill: {0}, Max Skill: {1}", stone.MinSkill, stone.MaxSkill);
            string classrule = "";
            string keepscore = "";
            string magicweaps = "";
            string magicarmor = "";
            string potions = "";
            string bandaids = "";
            string mounts = "";
            string pets = "";

            

            switch( stone.ClassRulesRule )
            {
                case pClassRules.PureMageOnly: { classrule = "Pure Mages Only"; break; }
                case pClassRules.TamerOnly: { classrule = "Tamers Only"; break; }
                case pClassRules.TankMageOnly: { classrule = "Tank Mages Only"; break; }
                case pClassRules.NoRule: { classrule = "No Restrictions"; break; }
                case pClassRules.DexerOnly: { classrule = "Dexers Only"; break; }
            }

            switch( stone.KeepScoreRule )
            {
                case pKeepScoreRule.Yes: { keepscore = "Yes"; break; }
                case pKeepScoreRule.No: { keepscore = "No"; break; }
            }

            switch( stone.MagicArmorRule )
            {
                case pMagicArmorRule.Allowed: { magicarmor = "Allowed"; break; }
                case pMagicArmorRule.NotAllowed: { magicarmor = "Not Allowed"; break; }
            }

            switch( stone.MagicWeaponRule )
            {
                case pMagicWeaponRule.Allowed: { magicweaps = "Allowed"; break; }
                case pMagicWeaponRule.NotAllowed: { magicweaps = "Not Allowed"; break; }
            }

            switch( stone.PotionRule )
            {
                case pPotionRule.Allowed: { potions = "Allowed"; break; }
                case pPotionRule.NotAllowed: { potions = "Not Allowed"; break; }
            }

            switch( stone.BandageRule )
            {
                case pBandaidRule.Allowed: { bandaids = "Allowed"; break; }
                case pBandaidRule.NotAllowed: { bandaids = "Not Allowed"; break; }
            }

            switch( stone.PetRule )
            {
                case pPetRule.Allowed: { pets = "Allowed"; break; }
                case pPetRule.NotAllowed: { pets = "Not Allowed"; break; }
            }

            switch( stone.MountRule )
            {
                case pMountRule.Allowed: { mounts = "Allowed"; break; }
                case pMountRule.NotAllowed: { mounts = "Not Allowed"; break; }
            }

            m_Stone = stone;
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(32, 19, 411, 338, 2620);
			AddLabel(44, 29, 4, @"Please review the rules.  If you accept these rules click OK,");
			AddImageTiled(37, 51, 403, 4, 2700);
            AddLabel( 44, 65, 43, String.Format( "Skill Rule: {0}", skillrule ) );
            AddLabel( 44, 85, 43, String.Format( "Class Rule: {0}", classrule ) );
            AddLabel( 44, 105, 43, String.Format( "Keep Score: {0}", keepscore ) );
            AddLabel( 44, 125, 43, String.Format( "Magic Weapons Allowed: {0}", magicweaps ) );
            AddLabel( 44, 145, 43, String.Format( "Magic Armor Allowed: {0}", magicarmor ) );
            AddLabel( 44, 165, 43, String.Format( "Potions Allowed: {0}", potions ) );
            AddLabel( 44, 186, 43, String.Format( "Bandaids Allowed: {0}", bandaids ) );
            AddLabel( 44, 207, 43, String.Format( "Mounts Allowed: {0}", mounts ) );
            AddLabel( 44, 228, 43, String.Format( "Pets Allowed: {0}", pets ) );
			AddButton(136, 280, 2328, 2329, (int)Buttons.OKButton, GumpButtonType.Reply, 0);
			AddButton(268, 281, 2328, 2329, (int)Buttons.CANCELButton, GumpButtonType.Reply, 0);
			AddLabel(167, 301, 0, @"OK");
			AddLabel(285, 302, 0, @"CANCEL");
			AddImageTiled(37, 262, 403, 4, 2700);

		}
		
		public enum Buttons
		{
			OKButton = 1,
			CANCELButton = 0,
		}

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile m = sender.Mobile;

            if( m == null )
                return;

            if( info.ButtonID == 1 )
                m_Stone.AddPlayer( m );
        }
	}
}