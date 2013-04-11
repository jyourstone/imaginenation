using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Custom.Games
{
    class EventException : Exception
    {
        public EventException() {}
        public EventException(string message) {}
        public EventException(string message, System.Exception inner) {}
     
        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected EventException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}

    }
}
