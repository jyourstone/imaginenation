using System;
using System.Collections;
using Server.Mobiles;

namespace Server.Engines.Quests.Collector
{
	public enum ImageType
	{
        //BloodElemental, Taran: Hopefully this will fix a problem with a Collector's Quest (Tomas O'Neerlan subquest, paint creatures)
		SkeletalMage,
		Kraken,
		Gazer,
		Lizardman,
		GiantBlackWidow,
		Scorpion,
		Titan,
		Troll,
		Lich,
		OphidianWarrior,
		Slime,
		Mongbat,
		Mummy,
		Daemon,
		Imp,
		SandVortex,
		StoneGargoyle,
		Reaper,
		Wisp,
		SilverSerpent,
        Dragon
	}

	public class ImageTypeInfo
	{
		private static readonly ImageTypeInfo[] m_Table = new ImageTypeInfo[]
			{
				//new ImageTypeInfo( 9688, typeof( BloodElemental ), 79, 45 ),
				new ImageTypeInfo( 9662, typeof( SkeletalMage ), 82, 45 ),
				new ImageTypeInfo( 9634, typeof( Kraken ), 60, 47 ),
				new ImageTypeInfo( 9615, typeof( Gazer ), 76, 45 ),  
				new ImageTypeInfo( 8414, typeof( Lizardman ), 78, 45 ), 
				new ImageTypeInfo( 9667, typeof( GiantBlackWidow ), 55, 52 ), 
				new ImageTypeInfo( 9657, typeof( Scorpion ), 65, 47 ),  
				new ImageTypeInfo( 9677, typeof( Titan ), 82, 45 ),  
				new ImageTypeInfo( 8425, typeof( Troll ), 78, 45 ),  
				new ImageTypeInfo( 9636, typeof( Lich ), 82, 45 ),  
				new ImageTypeInfo( 9645, typeof( OphidianWarrior ), 68, 45 ),  
				new ImageTypeInfo( 8424, typeof( Slime ), 56, 45 ),  
				new ImageTypeInfo( 9638, typeof( Mongbat ), 70, 50 ), 
				new ImageTypeInfo( 9639, typeof( Mummy ), 80, 45 ),  
				new ImageTypeInfo( 9604, typeof( Daemon ), 75, 45 ), 
				new ImageTypeInfo( 9631, typeof( Imp ), 76, 45 ),  
				new ImageTypeInfo( 9750, typeof( SandVortex ), 60, 43 ), 
				new ImageTypeInfo( 9614, typeof( StoneGargoyle ), 74, 45 ), 
				new ImageTypeInfo( 8442, typeof( Reaper ), 62, 46 ),  
				new ImageTypeInfo( 8448, typeof( Wisp ), 68, 45 ), 
				new ImageTypeInfo( 9666, typeof( SilverSerpent ), 59, 46 ), 
                new ImageTypeInfo( 8406, typeof( Dragon ), 76, 42 ) 
			};

		public static ImageTypeInfo Get( ImageType image )
		{
			int index = (int)image;
			if ( index >= 0 && index < m_Table.Length )
				return m_Table[index];
			else
				return m_Table[0];
		}

		public static ImageType[] RandomList( int count )
		{
			ArrayList list = new ArrayList( m_Table.Length );
			for ( int i = 0; i < m_Table.Length; i++ )
				list.Add( (ImageType)i );

			ImageType[] images = new ImageType[count];

			for ( int i = 0; i < images.Length; i++ )
			{
				int index = Utility.Random( list.Count );
				images[i] = (ImageType)list[index];

				list.RemoveAt( index );
			}

			return images;
		}

		private readonly int m_Figurine;
		private readonly Type m_Type;
		private readonly int m_X;
		private readonly int m_Y;

		public int Figurine { get { return m_Figurine; } }
		public Type Type { get { return m_Type; } }
        public int Name { get { return m_Figurine < 0x4000 ? 1020000 + m_Figurine : 1078872 + m_Figurine; } }
        public int X { get { return m_X; } }
		public int Y { get { return m_Y; } }

		public ImageTypeInfo( int figurine, Type type, int x, int y )
		{
			m_Figurine = figurine;
			m_Type = type;
			m_X = x;
			m_Y = y;
		}
	}
}