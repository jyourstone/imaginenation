namespace Server.Items 
{ 
   public class SeafaringBracelet : Item 
   { 
      [Constructable] 
      public SeafaringBracelet() : this( 1 ) 
      { 
      } 

      [Constructable] 
      public SeafaringBracelet( int amount ) : base( 0x1086 ) 
      {
	 Name = "Seafaring bracelet";
	 Stackable = false;
	 Hue = 387;
	 Weight = 1.0; 
         Amount = amount; 
      } 

      public SeafaringBracelet( Serial serial ) : base( serial ) 
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