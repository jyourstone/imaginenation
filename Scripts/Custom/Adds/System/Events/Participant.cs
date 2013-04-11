using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Custom.Games
{
    public interface Participant : IComparable<Participant>
    {
        int Score
        {
            get;
            set;
        }
    }
}
