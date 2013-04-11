using Server.Gumps;
using Server.Network;

/*
** QuestRewardStone
** used to open the QuestPointsRewardGump that allows players to purchase rewards with their XmlQuestPoints Credits.
*/

namespace Server.Items
{
    public class QuestRewardStone : Item
    {
        [Constructable]
        public QuestRewardStone() : base( 0xED4 )
        {
            Movable = false;
            Name = "a Quest Points Reward Stone";
        }

        public QuestRewardStone( Serial serial ) : base( serial )
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
        }
        
        public override void OnDoubleClick( Mobile from )
        {
            if ( from.InRange( GetWorldLocation(), 2 ) && from.InLOS(this) )
            {
                from.SendGump( new QuestRewardGump( from, 0 ) );
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }
    }
} 
