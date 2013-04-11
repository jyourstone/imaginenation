using Server.Network;

namespace Server.Items
{
	public class NewbieDungeonTeleporter : Teleporter
	{
		[Constructable]
		public NewbieDungeonTeleporter()
		{
		}

		public NewbieDungeonTeleporter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
				{
					break;
				}
			}
		}

		public override bool OnMoveOver( Mobile m )
		{
			if( Active )
			{
				if( !Creatures && !m.Player )
					return true;

				double totalSkills = m.Skills[SkillName.Swords].Base + m.Skills[SkillName.Fencing].Base + m.Skills[SkillName.Parry].Base + m.Skills[SkillName.Tactics].Base + m.Skills[SkillName.Macing].Base + m.Skills[SkillName.Wrestling].Base + m.Skills[SkillName.Healing].Base + m.Skills[SkillName.Archery].Base + m.Skills[SkillName.Anatomy].Base + m.Skills[SkillName.Magery].Base;

				if( ( totalSkills <= 500.0 ) || ( m.AccessLevel > AccessLevel.Player ) )
				{
					StartTeleport( m );
					return false;
				}
				else
                    m.PublicOverheadMessage(MessageType.Regular, 906, true, string.Format("Your skill exceeds the level acceptable to enter this area."));
			}
			return true;
		}
	}
}