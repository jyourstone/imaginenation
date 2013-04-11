// SpeechHelper -- A RunUO script 
// Last modified 1/18/03 by David 
// 
// Provides methods to: 
// Mobile says a random item from a list 
// Mobile says a lot of things in sequence 

using System; 

namespace Server.Misc 
{ 
   public class SpeechHelper 
   { 
      // Given Mobile says one element of given string array 
      public static void SayRandom( string[] say, Mobile mob ) 
      { 
         mob.Say( say[Utility.Random( say.Length )] ); 
      } 

      // Given Mobile says one localized text string from array of cliloc numbers 
      public static void SayRandom( int[] say, Mobile mob ) 
      { 
         mob.Say( say[Utility.Random( say.Length )] ); 
      } 

      // Given Mobile says each element of a given string array in order 
      // with a given delay between spoken elements 
      public static void SayALot( string[] say, Mobile mob, TimeSpan delay ) 
      { 
         SayALotTimer timer = new SayALotTimer( say, mob, delay ); 
         timer.Start(); 
      } 
       
      // Given Mobile says each element of a given string array in order 
      // with a 2.5 second delay between spoken elements 
      public static void SayALot( string[] say, Mobile mob ) 
      { 
         SayALotTimer timer = new SayALotTimer( say, mob, TimeSpan.FromSeconds( 2.5 ) ); 
         timer.Start(); 
      } 

      // Used by SayALot methods 
      private class SayALotTimer : Timer 
      { 
         private int m_Mode; 
         private readonly Mobile m_Owner; 
         private readonly string[] m_Say; 

         public SayALotTimer( string[] s, Mobile m, TimeSpan delay ) : base( delay, delay ) 
         { 
            m_Mode = 0; 
            m_Owner = m; 
            m_Say = s; 

            Priority = TimerPriority.TwoFiftyMS; 
            m_Owner.Say( m_Say[m_Mode++] ); //Say the first line right away 
         } 

         protected override void OnTick() 
         { 
            m_Owner.Say( m_Say[m_Mode++] ); 
            if ( m_Mode >= m_Say.Length ) 
            { 
               Stop(); 
            } 
         } 
      } 
   } 
}

