using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class RegionControlGump : Gump
	{
	    readonly RegionControl m_Controller;
		public RegionControlGump( RegionControl r )	: base( 25, 25 )
		{
			m_Controller = r;

			Closable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(23, 32, 412, 256, 9270);
			AddAlphaRegion(19, 29, 418, 263);
			AddButton(55, 46, 5569, 5570, (int)Buttons.SpellButton, GumpButtonType.Reply, 0);
			AddButton(55, 128, 5581, 5582, (int)Buttons.SkillButton, GumpButtonType.Reply, 0);
			AddButton(50, 205, 7006, 7006, (int)Buttons.AreaButton, GumpButtonType.Reply, 0);

			AddLabel(152, 70, 1152, "Edit Restricted Spells");
			AddLabel(152, 153, 1152, "Edit Restricted Skills");
			AddLabel(152, 234, 1152, "Add Region Area");
			AddImage(353, 54, 3953);
			AddImage(353, 180, 3955);

		}
		
		public enum Buttons
		{
			SpellButton = 1,
			SkillButton,
			AreaButton
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if( m_Controller == null || m_Controller.Deleted )
				return;

			Mobile m = sender.Mobile;

			switch( info.ButtonID )
			{
				case 1:
				{
					//m_Controller.SendRestrictGump( m, RestrictType.Spells );
					m.CloseGump( typeof( SpellRestrictGump ) );
					m.SendGump( new SpellRestrictGump( m_Controller.RestrictedSpells ) );

					m.CloseGump( typeof( RegionControlGump ) );
					m.SendGump( new RegionControlGump( m_Controller ));
					break;
				}
				case 2:
				{
					//m_Controller.SendRestrictGump( m, RestrictType.Skills );

					m.CloseGump( typeof( SkillRestrictGump ) );
					m.SendGump( new SkillRestrictGump( m_Controller.RestrictedSkills ) );

					m.CloseGump( typeof( RegionControlGump ) );
					m.SendGump( new RegionControlGump( m_Controller ));
					break;
				}
				case 3:
				{
					m.CloseGump( typeof( RegionControlGump ) );
					m.SendGump( new RegionControlGump( m_Controller ) );

					m.CloseGump( typeof( RemoveAreaGump ) );

					m.SendGump( new RemoveAreaGump( m_Controller ) );

					m_Controller.ChooseArea( m );
					break;
				}
			}
		}
	}
}