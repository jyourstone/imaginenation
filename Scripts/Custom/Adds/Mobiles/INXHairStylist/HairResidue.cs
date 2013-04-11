//Yes, i will upload this to the forums when im done with it

/***************************************************************************
 *                                HairResidue.cs
 *                            -------------------
 *   last edited          : July 6, 2007
 *   web site             : www.in-x.org
 *   author               : Makaveli
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   Created by the Imagine Nation - Xtreme dev team for "IN-X" and the RunUo
 *   community. If you miss the old school Sphere 0.51 gameplay, and want to
 *   try it on the best and most stable emulator, visit us at www.in-x.org.
 *      
 *   www.in-x.org
 *   A full sphere 0.51 replica.
 * 
 ***************************************************************************/

using System;

namespace Server.INXHairStylist
{
    public class HairResidue : Item
    {
        //Someone needs to add more IDs
        private static readonly int[] m_HairIds = new int[] { 0xDFE };

        //Decay time for the hair
        public override TimeSpan DecayTime {  get { return TimeSpan.FromMinutes(30.0); } }

        private static readonly string[] m_HairNameFormat = new string[] { "a pile of {0}'s hair", "{0}'s cut-off hair", "a furry piece of {0}", "{0} was here" };

        [Constructable]
        public HairResidue() : this(null)
        {
        }

        [Constructable]
        public HairResidue(Mobile owner) : base(0xDFE)
        {
            Movable = true;
            ItemID = m_HairIds[Utility.Random(m_HairIds.Length)];

            if (owner != null)
            {
                if (owner.HairHue == 0)
                    Hue = 1001;
                else
                    Hue = owner.HairHue;

                Name = string.Format(m_HairNameFormat[Utility.Random(m_HairNameFormat.Length)], owner.Name);
            }
        }

        public HairResidue(Serial serial) : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}