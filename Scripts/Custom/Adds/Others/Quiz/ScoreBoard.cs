/*
 * 
 * Made with love by Unixfreak August 2010
 * 
 * 
*/
using System;
using System.Collections.Generic;

namespace Server.Quiz
{
 public class ScoreBoard
    {
        private static readonly SortedDictionary<Mobile, byte> scoreList = new SortedDictionary<Mobile, byte>();

        public void Add(Mobile mob)
        {
            if (scoreList.ContainsKey(mob))
                scoreList[mob]++;
            else
                scoreList.Add(mob, 1);
        }

        public void Clear()
        {
            scoreList.Clear();
        }

        public static void Print()
        {
            String msg;
            World.Broadcast(133, true, "Current scores are:");
            if (scoreList.Count > 0)
            {
                foreach (KeyValuePair<Mobile, byte> pair in scoreList)
                {
                    msg = pair.Value == 1 ? " point!" : " points!";
                    World.Broadcast(133, true, pair.Key.Name + " with " + pair.Value + msg);
                }
            }
            else
            {
                World.Broadcast(133, true, "No scores");
            }
        }

    }
}