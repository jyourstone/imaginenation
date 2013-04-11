namespace Server.Items 
{ 
   public class HeadOfBritainLettuce : Item 
   { 
      [Constructable] 
      public HeadOfBritainLettuce() : this( 1 ) 
      { 
      } 

      [Constructable] 
      public HeadOfBritainLettuce( int amount ) : base( 0xC70 ) 
      {
	 Name = "Head of Britain lettuce";
	 Stackable = false;
	 Hue = 267;
	 Weight = 1.0; 
         Amount = amount; 
      } 

      public HeadOfBritainLettuce( Serial serial ) : base( serial ) 
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