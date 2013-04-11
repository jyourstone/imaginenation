namespace Server.Items 
{ 
   public class DecayingHead : Item 
   { 
      [Constructable] 
      public DecayingHead() : this( 1 ) 
      { 
      } 

      [Constructable] 
      public DecayingHead( int amount ) : base( 0x1DA0 ) 
      {
	 Name = "Decaying head";
	 Stackable = false;
	 Hue = 371;
	 Weight = 1.0; 
         Amount = amount; 
      } 

      public DecayingHead( Serial serial ) : base( serial ) 
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