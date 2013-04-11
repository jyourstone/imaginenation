using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Server.Scripts.Custom.Adds.System
{
    public class HashMap<TKey, TValue> : Hashtable
    {
        public HashMap() : base()
        {
            
        }

        public void Add(TKey key, TValue value)
        {
            try
            {
                if (base.Contains(key))
                    base[key] = value;
                else
                    base.Add(key, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public TValue Get(TKey key)
        {
            try
            {
                return (TValue) base[key];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(TValue);
            }
        }
    }
}
