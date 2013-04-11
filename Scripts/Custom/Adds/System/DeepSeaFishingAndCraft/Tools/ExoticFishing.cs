using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Engines.Quests.Collector;

namespace Server.Engines.Harvest
{
	public class ExoticFishing : HarvestSystem
	{
		private static ExoticFishing m_System;

		public static ExoticFishing System
		{
			get
			{
				if ( m_System == null )
					m_System = new ExoticFishing();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get{ return m_Definition; }
		}

		private ExoticFishing()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region ExoticFishing
			HarvestDefinition fish = new HarvestDefinition();

			// Resource banks are every 8x8 tiles
			fish.BankWidth = 1;
			fish.BankHeight = 1;

			// Every bank holds from 1 to 2 fish
			fish.MinTotal = 1;
			fish.MaxTotal = 2;

			// A resource bank will respawn its content every 10 to 20 minutes
			fish.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			fish.MaxRespawn = TimeSpan.FromMinutes( 20.0 );

			// Skill checking is done on the Fishing skill
			fish.Skill = SkillName.Fishing;

			// Set the list of harvestable tiles
			fish.Tiles = m_WaterTiles;
			fish.RangedTiles = true;

			// Players must be within 6 tiles to harvest
			fish.MaxRange = 6;

			// One fish per harvest action
			fish.ConsumedPerHarvest = 1;
			fish.ConsumedPerFeluccaHarvest = 1;

			// The ExoticFishing
			fish.EffectActions = new int[]{ 12 };
			fish.EffectSounds = new int[0];
			fish.EffectCounts = new int[]{ 1 };
			fish.EffectDelay = TimeSpan.Zero;
			fish.EffectSoundDelay = TimeSpan.FromSeconds( 4.0 );

			fish.NoResourcesMessage = 503172; // The fish don't seem to be biting here.
			fish.FailMessage = 503171; // You fish a while, but fail to catch anything.
			fish.TimedOutOfRangeMessage = 500976; // You need to be closer to the water to fish!
			fish.OutOfRangeMessage = 500976; // You need to be closer to the water to fish!
			fish.PackFullMessage = 503176; // You do not have room in your backpack for a fish.
			fish.ToolBrokeMessage = 503174; // You broke your ExoticFishing pole.

			res = new HarvestResource[]
				{
					new HarvestResource( 00.0, 00.0, 100.0, 1043297, typeof( Fish ) ),
                    new HarvestResource(90.0, 10.1, 101.0, "You pull up a butterfly fish!", typeof( Butterflyfish ) ),
                    new HarvestResource(80.0, 20.1, 101.0, "You pull up a baroness butterfly fish!", typeof( BaronessButterflyfish ) ),
                    new HarvestResource(70.0, 80.1, 101.0, "You pull up a copperbanded butterfly fish!", typeof( CopperbandedButterflyfish ) ),
                    new HarvestResource(100.0, 80.1, 101.0, "You pull up an albino angel fish!", typeof( AlbinoAngelfish ) ),
                    new HarvestResource(100.0, 90.0, 101.0, "You pull up an angel fish!", typeof( Angelfish ) )
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 20.0, 0.0, res[0], null ),
                    new HarvestVein( 20.0, 0.0, res[1], null ), //butterfly 
                    new HarvestVein( 20.0, 0.0, res[2], null ), //baroness
                    new HarvestVein( 20.0, 0.0, res[3], null ), //copper
                    new HarvestVein( 10.0, 0.0, res[4], null ), //albino
                    new HarvestVein( 10.0, 0.0, res[5], null ) //angel
				};

			fish.Resources = res;
			fish.Veins = veins;

			m_Definition = fish;
			Definitions.Add( fish );
			#endregion
		}

		public override void OnConcurrentHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			from.SendLocalizedMessage( 500972 ); // You are already Fishing.
		}

		private class MutateEntry
		{
			public double m_ReqSkill, m_MinSkill, m_MaxSkill;
			public bool m_DeepWater;
			public Type[] m_Types;

			public MutateEntry( double reqSkill, double minSkill, double maxSkill, bool deepWater, params Type[] types )
			{
				m_ReqSkill = reqSkill;
				m_MinSkill = minSkill;
				m_MaxSkill = maxSkill;
				m_DeepWater = deepWater;
				m_Types = types;
			}
		}

		private static MutateEntry[] m_MutateTable = new MutateEntry[]
			{
				new MutateEntry(  80.0,  80.0,  4080.0,  true, typeof( Butterflyfish ), typeof( BaronessButterflyfish ), typeof( CopperbandedButterflyfish ) ),
				new MutateEntry(  90.0,  80.0,  4080.0,  true, typeof( AlbinoAngelfish ), typeof( Angelfish ), typeof( AnteniAngelfish ), typeof( FlameAngelfish ), typeof( QueenAngelfish ) ),
				new MutateEntry( 100.0,  80.0,  4080.0,  true, typeof( BrineShrimp ), typeof( Crab ), typeof( Jellyfish ), typeof( Pufferfish ), typeof( Shrimp ), typeof( SmallFishies ) ),
				new MutateEntry(   0.0, 125.0, -2375.0, false, typeof( Anthias ), typeof( BambooShark ), typeof( BlueFish ), typeof( FantailGoldfish ), typeof( PowderblueTang ), typeof( YellowTang ) ),
				new MutateEntry(   0.0, 105.0,  -420.0, false, typeof( SeveredAnchor ), typeof( FishBones ), typeof( Coral ), typeof( Pearl ), typeof( SeaGrass ), typeof( SunkenShip ) ),
				new MutateEntry(   0.0, 200.0,  -200.0, false, new Type[1]{ null } )
			};

		public override bool CheckResources( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed )
		{
			Container pack = from.Backpack;

			if ( pack != null )
			{
				Item[] messages = pack.FindItemsByType( typeof( SOS ) );

				for ( int i = 0; i < messages.Length; ++i )
				{
					SOS sos = (SOS)messages[i];

					if ( from.Map == sos.TargetMap && from.InRange( sos.TargetLocation, 60 ) )
						return true;
				}
			}

			return base.CheckResources( from, tool, def, map, loc, timed );
		}

		public override Type MutateType( Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			bool deepWater = SpecialFishingNet.FullValidation( map, loc.X, loc.Y );

			double skillBase = from.Skills[SkillName.Fishing].Base;
			double skillValue = from.Skills[SkillName.Fishing].Value;

			for ( int i = 0; i < m_MutateTable.Length; ++i )
			{
				MutateEntry entry = m_MutateTable[i];

				if ( !deepWater && entry.m_DeepWater )
					continue;

				if ( skillBase >= entry.m_ReqSkill )
				{
					double chance = (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill);

					if ( chance > Utility.RandomDouble() )
						return entry.m_Types[Utility.Random( entry.m_Types.Length )];
				}
			}

			return type;
		}

		private static Map SafeMap( Map map )
		{
			if ( map == null || map == Map.Internal )
				return Map.Trammel;

			return map;
		}

		public override void SendSuccessTo( Mobile from, Item item, HarvestResource resource )
		{
			if ( item is BigFish )
			{
				from.SendLocalizedMessage( 1042635 ); // Your fishing pole bends as you pull a big fish from the depths!

				((BigFish)item).Fisher = from;
			}
			else
			{
				int number;
				string name;

				if ( item is Fish )
				{
					number = 1008124;
					name = "a fish";
				}
				else if ( item is BaseShoes )
				{
					number = 1008124;
					name = "a piece of junk";
				}

				else
				{
					number = 1043297;

					if ( (item.ItemData.Flags & TileFlag.ArticleA) != 0 )
						name = "a " + item.ItemData.Name;
					else if ( (item.ItemData.Flags & TileFlag.ArticleAn) != 0 )
						name = "an " + item.ItemData.Name;
					else
						name = item.ItemData.Name;
				}

				if ( number == 1043297 )
					from.SendLocalizedMessage( number, name );
				else
					from.SendLocalizedMessage( number, true, name );
			}
		}

		public override void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			base.OnHarvestStarted( from, tool, def, toHarvest );

			int tileID;
			Map map;
			Point3D loc;

			if ( GetHarvestDetails( from, tool, toHarvest, out tileID, out map, out loc ) )
				Timer.DelayCall( TimeSpan.FromSeconds( 1.5 ), new TimerStateCallback( Splash_Callback ), new object[]{ loc, map } );
		}

		private void Splash_Callback( object state )
		{
			object[] args = (object[])state;
			Point3D loc = (Point3D)args[0];
			Map map = (Map)args[1];

			Effects.SendLocationEffect( loc, map, 0x352D, 16, 4 );
			Effects.PlaySound( loc, map, 0x364 );
		}

		public override object GetLock( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			return this;
		}

		public override bool BeginHarvesting( Mobile from, Item tool )
		{
			if ( !base.BeginHarvesting( from, tool ) )
				return false;

			from.SendLocalizedMessage( 500974 ); // What water do you want to fish in?
			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 500971 ); // You can't fish while riding!
				return false;
			}

			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 500971 ); // You can't fish while riding!
				return false;
			}

			return true;
		}

		private static int[] m_WaterTiles = new int[]
			{
				0x00A8, 0x00AB,
				0x0136, 0x0137,
				0x5797, 0x579C,
				0x746E, 0x7485,
				0x7490, 0x74AB,
				0x74B5, 0x75D5
			};
	}
}