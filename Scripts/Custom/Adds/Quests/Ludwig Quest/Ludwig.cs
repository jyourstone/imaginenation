using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Items.MusicBox;

namespace Server.Mobiles
{
    public class Ludwig : Mobile{public virtual bool IsInvulnerable{ get{ return true; } }
    public override bool ClickTitle { get { return true; } }
    [Constructable]
    public Ludwig()
    {

        ///////////STR/DEX/INT
        InitStats(100, 41, 51);

        ///////////name
        Name = "Ludwig";

        ///////////title
        Title = "the master composer";

        ///////////sex. 0x191 is female, 0x190 is male.
        Body = 0x190;

        Fame = 5000;
        Karma = 5000;

        ///////////skincolor
        Hue = 0x83F8;

        ///////////Random hair and haircolor
        HairItemID = 0x2045;
        HairHue = 986;

        ///////////clothing and hues
        AddItem(new LongPants(656));
        AddItem(new Boots());
        AddItem(new Shirt(785));

        ///////////immortal and frozen to-the-spot features below:
        Blessed = true;
        CantWalk = true;

        ///////////Adding a backpack
        Container pack = new Backpack();
        pack.DropItem(new Gold(250, 300));
        pack.Movable = false;
        AddItem(pack);
    }

public Ludwig( Serial serial ) : base( serial ){}
public override bool ShowContextMenu { get { return true; } }
public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
{ base.GetContextMenuEntries( from, list ); list.Add( new LudwigEntry( from, this ) ); } 
public override void Serialize( GenericWriter writer ){base.Serialize( writer );writer.Write( 0 );}
public override void Deserialize( GenericReader reader ){base.Deserialize( reader );int version = reader.ReadInt();}
public class LudwigEntry : ContextMenuEntry{private readonly Mobile m_Mobile;private Mobile m_Giver;
public LudwigEntry( Mobile from, Mobile giver ) : base( 6146, 3 ){m_Mobile = from;m_Giver = giver;}
public override void OnClick(){if( !( m_Mobile is PlayerMobile ) )return;
PlayerMobile mobile = (PlayerMobile) m_Mobile;{

///////////gump name
if ( ! mobile.HasGump( typeof( LudwigGump ) ) ){
mobile.SendGump( new LudwigGump( mobile ));}}}}
public override bool OnDragDrop( Mobile from, Item dropped ){               Mobile m = from;PlayerMobile mobile = m as PlayerMobile;
if ( mobile != null)
{
///////////item to be dropped
if( dropped is SheetMusic ){
if (dropped.Amount<8)
{
    PrivateOverheadMessage( MessageType.Regular, 1153, false, "Please return with the correct amount.", mobile.NetState );return false;
}

if (dropped.Amount>8)
mobile.AddToBackpack( new SheetMusic( (dropped.Amount-8) ) );

dropped.Delete();

///////////the reward
mobile.AddToBackpack( new Gold( 1000 ) );
mobile.AddToBackpack( new MusicBox( ) );
    if( Utility.Random( 100 ) < 100 ) 
         			switch ( Utility.Random( 4 ) )
			{ 
				case 0: mobile.AddToBackpack( new AmbrosiaSong() ); break; 
				case 1: mobile.AddToBackpack( new ElfCitySong() ); break; 
				case 2: mobile.AddToBackpack( new PubTuneSong() ); break; 
				case 3: mobile.AddToBackpack( new ZentoSong() ); break; 				
			}

PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you very much!", mobile.NetState );

       mobile.SendGump( new LudwigFinishGump( mobile ) );
         			return true;
         		}
         		else if( dropped is SheetMusic )
         		{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         		else
         		{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need for that!", mobile.NetState );
     			}
			}
			return false;
        }
    }
}

/*return true;}
    if ( dropped is Whip){PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );return false;}
    PrivateOverheadMessage( MessageType.Regular, 1153, false,"I have no need for this...", mobile.NetState );
}
    return false;}}}*/