namespace Server.Items 
{ 
   public class SpecialSeaMap : Item 
   { 
      [Constructable] 
      public SpecialSeaMap() : this( 1 ) 
      { 
      } 

      [Constructable] 
      public SpecialSeaMap( int amount ) : base( 0x14EC ) 
      {
	 Name = "Special sea map";
	 Stackable = false;
	 Hue = 1153;
         Weight = 1.0; 
         Amount = amount; 
      } 

      public SpecialSeaMap( Serial serial ) : base( serial ) 
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
   } 
}