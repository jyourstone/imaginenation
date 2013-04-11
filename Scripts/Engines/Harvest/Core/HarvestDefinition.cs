using System;
using System.Collections.Generic;

namespace Server.Engines.Harvest
{
	public class HarvestDefinition
	{
		private int m_BankWidth, m_BankHeight;
	    private int[] m_Tiles;
		private bool m_RangedTiles;
	    private HarvestVein[] m_Veins;
        private BonusHarvestResource[] m_BonusResources;
	    private bool m_RandomizeVeins;

		public int BankWidth{ get{ return m_BankWidth; } set{ m_BankWidth = value; } }
		public int BankHeight{ get{ return m_BankHeight; } set{ m_BankHeight = value; } }
	    public int MinTotal { get; set; }

	    public int MaxTotal { get; set; }

	    public int[] Tiles{ get{ return m_Tiles; } set{ m_Tiles = value; } }
		public bool RangedTiles{ get{ return m_RangedTiles; } set{ m_RangedTiles = value; } }
	    public TimeSpan MinRespawn { get; set; }

	    public TimeSpan MaxRespawn { get; set; }

	    public int MaxRange { get; set; }

	    public int ConsumedPerHarvest { get; set; }

	    public int ConsumedPerFeluccaHarvest { get; set; }

	    public bool PlaceAtFeetIfFull { get; set; }

	    public SkillName Skill { get; set; }

	    public int[] EffectActions { get; set; }

	    public int[] EffectActionsRiding { get; set; }

	    public int[] EffectCounts { get; set; }

	    public int[] EffectSounds { get; set; }

	    public TimeSpan EffectSoundDelay { get; set; }

	    public TimeSpan EffectDelay { get; set; }

	    public object NoResourcesMessage { get; set; }

	    public object OutOfRangeMessage { get; set; }

	    public object TimedOutOfRangeMessage { get; set; }

	    public object DoubleHarvestMessage { get; set; }

	    public object FailMessage { get; set; }

	    public object PackFullMessage { get; set; }

	    public object ToolBrokeMessage { get; set; }

	    public HarvestResource[] Resources { get; set; }

	    public HarvestVein[] Veins{ get{ return m_Veins; } set{ m_Veins = value; } }
        public BonusHarvestResource[] BonusResources { get { return m_BonusResources; } set { m_BonusResources = value; } }
	    public bool RaceBonus { get; set; }

	    public bool RandomizeVeins { get { return m_RandomizeVeins; } set { m_RandomizeVeins = value; } }

		private Dictionary<Map, Dictionary<Point2D, HarvestBank>> m_BanksByMap;

		public Dictionary<Map, Dictionary<Point2D, HarvestBank>> Banks{ get{ return m_BanksByMap; } set{ m_BanksByMap = value; } }

		public void SendMessageTo( Mobile from, object message )
		{
			if ( message is int )
				from.SendLocalizedMessage( (int)message );
			else if ( message is string )
				from.SendMessage( (string)message );
		}

		public HarvestBank GetBank( Map map, int x, int y )
		{
			if ( map == null || map == Map.Internal )
				return null;

			x /= m_BankWidth;
			y /= m_BankHeight;

			Dictionary<Point2D, HarvestBank> banks = null;
			m_BanksByMap.TryGetValue( map, out banks );

			if ( banks == null )
				m_BanksByMap[map] = banks = new Dictionary<Point2D, HarvestBank>();

			Point2D key = new Point2D( x, y );
			HarvestBank bank = null;
			banks.TryGetValue( key, out bank );

			if ( bank == null )
				banks[key] = bank = new HarvestBank( this, GetVeinAt( map, x, y ) );

			return bank;
		}

        public HarvestVein GetVeinAt(Map map, int x, int y)
        {
            double randomValue;

            if (m_RandomizeVeins)
            {
                randomValue = Utility.RandomDouble();
            }
            else
            {
                Random random = new Random((x * 17) + (y * 11) + (map.MapID * 3));
                randomValue = random.NextDouble();
            }

            return GetVeinFrom(randomValue);

            //Maka's randomize veins
            /*
            if (m_Veins.Length == 1)
                return m_Veins[0];

            Random random = new Random((x * 17) + (y * 11) + (map.MapID * 3));
            double randomValue = random.NextDouble() * 100;

            for (int i = 0; i < m_Veins.Length; ++i)
            {
                if (randomValue <= m_Veins[i].VeinChance)
                    return m_Veins[i];

                randomValue -= m_Veins[i].VeinChance;
            }

            return null;
            */
        }

        public HarvestVein GetVeinFrom(double randomValue)
        {
            if (m_Veins.Length == 1)
                return m_Veins[0];

            randomValue *= 100;

			for ( int i = 0; i < m_Veins.Length; ++i )
			{
				if ( randomValue <= m_Veins[i].VeinChance )
					return m_Veins[i];

				randomValue -= m_Veins[i].VeinChance;
			}

			return null;
		}

        public BonusHarvestResource GetBonusResource()
        {
            if (m_BonusResources == null)
                return null;

            double randomValue = Utility.RandomDouble() * 100;

            for (int i = 0; i < m_BonusResources.Length; ++i)
            {
                if (randomValue <= m_BonusResources[i].Chance)
                    return m_BonusResources[i];

                randomValue -= m_BonusResources[i].Chance;
            }

            return null;
        }


		public HarvestDefinition()
		{
			m_BanksByMap = new Dictionary<Map, Dictionary<Point2D, HarvestBank>>();
		}

		public bool Validate( int tileID )
		{
			if ( m_RangedTiles )
			{
				bool contains = false;

				for ( int i = 0; !contains && i < m_Tiles.Length; i += 2 )
					contains = ( tileID >= m_Tiles[i] && tileID <= m_Tiles[i + 1] );

				return contains;
			}
			else
			{
				int dist = -1;

				for ( int i = 0; dist < 0 && i < m_Tiles.Length; ++i )
					dist = ( m_Tiles[i] - tileID );

				return ( dist == 0 );
			}
		}
	}
}