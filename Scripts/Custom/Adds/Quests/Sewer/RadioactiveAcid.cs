namespace Server.Items
{
public class RadioactiveAcid : Item
{
[Constructable]
public RadioactiveAcid() : this( 1 )
{}
[Constructable]
public RadioactiveAcid( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
{}
[Constructable]

///////////The hexagon value ont he line below is the ItemID
public RadioactiveAcid( int amount ) : base( 3850 )
{


///////////Item name
Name = "Radioactive acid bottle";

///////////Item hue
Hue = 1957;

///////////Stackable
Stackable = true;

///////////Weight of one item
Weight = 1.00;
Amount = amount;

}
public RadioactiveAcid( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( 1 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}
