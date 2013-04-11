using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{[CorpseName( "Brent's Corpse" )]
    public class Brent : Mobile{public virtual bool IsInvulnerable{ get{ return true; } }
    public override bool ClickTitle { get { return true; } }
    [Constructable]
    public Brent()
    {

        ///////////STR/DEX/INT
        InitStats(100, 41, 51);

        ///////////name
        Name = "Brent";

        ///////////title
        Title = "The Sewer Engineer";

        ///////////sex. 0x191 is female, 0x190 is male.
        Body = 0x190;

        Fame = 5000;
        Karma = 5000;

        ///////////skincolor
        Hue = Utility.RandomSkinHue();

        ///////////Random hair and haircolor
        Utility.AssignRandomHair(this);

        ///////////clothing and hues
        AddItem(new FancyShirt(Utility.RandomYellowHue()));
        AddItem(new LongPants(Utility.RandomYellowHue()));
        AddItem(new ThighBoots(Utility.RandomNeutralHue()));

        ///////////immortal and frozen to-the-spot features below:
        Blessed = true;
        CantWalk = true;

        ///////////Adding a backpack
        Container pack = new Backpack();
        pack.DropItem(new Gold(250, 300));
        pack.Movable = false;
        AddItem(pack);
    }

public Brent( Serial serial ) : base( serial ){}
public override bool ShowContextMenu { get { return true; } }
public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
{ base.GetContextMenuEntries( from, list ); list.Add( new BrentEntry( from, this ) ); } 
public override void Serialize( GenericWriter writer ){base.Serialize( writer );writer.Write( 0 );}
public override void Deserialize( GenericReader reader ){base.Deserialize( reader );int version = reader.ReadInt();}
public class BrentEntry : ContextMenuEntry{private readonly Mobile m_Mobile;private Mobile m_Giver;
public BrentEntry( Mobile from, Mobile giver ) : base( 6146, 3 ){m_Mobile = from;m_Giver = giver;}
public override void OnClick(){if( !( m_Mobile is PlayerMobile ) )return;
PlayerMobile mobile = (PlayerMobile) m_Mobile;{

///////////gump name
if ( ! mobile.HasGump( typeof( SewerQuestGump ) ) ){
mobile.SendGump( new SewerQuestGump( mobile ));}}}}
public override bool OnDragDrop( Mobile from, Item dropped ){               Mobile m = from;PlayerMobile mobile = m as PlayerMobile;
if ( mobile != null)
{
///////////item to be dropped
if( dropped is RadioactiveAcid ){
if (dropped.Amount<30)
{
    PrivateOverheadMessage( MessageType.Regular, 1153, false, "Please return with the correct amount.", mobile.NetState );return false;
}

if (dropped.Amount>30)
mobile.AddToBackpack( new RadioactiveAcid( (dropped.Amount-30) ) );

dropped.Delete();

///////////the reward
mobile.AddToBackpack( new Gold( 5000 ) );
if (0.20>Utility.RandomDouble())
mobile.AddToBackpack( new RadioactiveGloves( ) );//replace this

///////////thanks message
PrivateOverheadMessage( MessageType.Regular, 1153, false, "Appreciate the help!!", mobile.NetState );


return true;}
    if ( dropped is Whip){PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );return false;}
    PrivateOverheadMessage( MessageType.Regular, 1153, false,"I have no need for this...", mobile.NetState );
}
    return false;}}}