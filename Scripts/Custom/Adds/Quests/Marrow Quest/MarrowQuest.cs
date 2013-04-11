using System;using System.Collections;using System.Collections.Generic;using Server.Items;using Server.Targeting;using Server.ContextMenus;using Server.Gumps;using Server.Misc;using Server.Network;using Server.Spells;namespace Server.Mobiles
{[CorpseName( "Marrow's Corpse" )]public class Marrow : Mobile{public virtual bool IsInvulnerable{ get{ return true; } }
[Constructable]public Marrow(){

InitStats( 100, 100, 100 );

Name = "Marrow";

Title = "the chemist";

Body = 0x191;

Fame = 2000;
Karma = 3000;

Hue = Utility.RandomSkinHue();

Utility.AssignRandomHair( this );

AddItem( new Server.Items.Shirt( Utility.RandomRedHue() ) );
AddItem( new Server.Items.Skirt( Utility.RandomRedHue() ) );
AddItem( new Server.Items.Boots( Utility.RandomNeutralHue() ) );

Blessed = true;
CantWalk = true;

Container pack = new Backpack();
pack.DropItem( new Gold( 150, 200 ) );
pack.Movable = false;
AddItem( pack );
}

public Marrow( Serial serial ) : base( serial ){}
public override bool ShowContextMenu { get { return true; } }
public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
{ base.GetContextMenuEntries( from, list ); list.Add( new MarrowEntry( from, this ) ); } 
public override void Serialize( GenericWriter writer ){base.Serialize( writer );writer.Write( (int) 0 );}
public override void Deserialize( GenericReader reader ){base.Deserialize( reader );int version = reader.ReadInt();}
public class MarrowEntry : ContextMenuEntry{private Mobile m_Mobile;private Mobile m_Giver;
public MarrowEntry( Mobile from, Mobile giver ) : base( 6146, 3 ){m_Mobile = from;m_Giver = giver;}
public override void OnClick(){if( !( m_Mobile is PlayerMobile ) )return;
PlayerMobile mobile = (PlayerMobile) m_Mobile;{

//gump name
if ( ! mobile.HasGump( typeof( MarrowQuestGump ) ) ){mobile.SendGump( new MarrowQuestGump ( mobile ));}}}}
public override bool OnDragDrop( Mobile from, Item dropped ){               Mobile m = from;PlayerMobile mobile = m as PlayerMobile;
if ( mobile != null){

//item to be dropped
if( dropped is ReptileBone ){if(dropped.Amount<20)
{this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "There's not the right amount here!", mobile.NetState );return false;}
dropped.Delete();
if (dropped.Amount > 20)
    mobile.AddToBackpack(new ReptileBone((dropped.Amount - 20)));

dropped.Delete();

//the reward
mobile.AddToBackpack( new Gold( 5000 ) );
mobile.AddToBackpack( new SilverWeaponCrystal( ) );
if (0.10 > Utility.RandomDouble())
    mobile.AddToBackpack(new MysticFishingNet());

//thanks message
this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you for gathering the resources for my test!", mobile.NetState );


return true;}else if ( dropped is Whip){this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );return false;}else{this.PrivateOverheadMessage( MessageType.Regular, 1153, false,"I have no need for this...", mobile.NetState );}}return false;}}}
