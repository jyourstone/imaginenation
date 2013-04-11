// Feel Free to modify the code as you wish.
using System;   
using Server;
using Server.Spells;

namespace Server.Items
{
	public class Rot : Item
	{
	private static TimeSpan m_DDT = TimeSpan.FromSeconds( 5.0 );
					
	[Constructable]
	public Rot() : base( 0x36B0 )
	{
	    Timer.DelayCall( m_DDT, new TimerCallback( Delete) ); 		    
        Movable = false;
	    Hue = 1421;
	    Name = "Rot";
	}
				  
	public override void OnMovement( Mobile m, Point3D oldLocation )
	{
	            if ( m.InRange( Location, 1 ) )

	     {
				if ( m.Alive )
		      {
                       m.PlaySound( 0x813 );
		    		   m.ApplyPoison( m, Poison.Deadly );
	             }
         }
}    
     public override bool HandlesOnMovement{ get{ return true; } }
   		
      public Rot( Serial serial ) : base( serial ) 
      { 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); // version 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt();          
     	} 
     
     } 
}
