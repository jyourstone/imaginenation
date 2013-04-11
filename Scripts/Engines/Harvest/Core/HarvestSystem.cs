using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.Engines.Harvest
{
	public abstract class HarvestSystem : IAction
	{
		private readonly List<HarvestDefinition> m_Definitions;

		public List<HarvestDefinition> Definitions { get { return m_Definitions; } }

		public HarvestSystem()
		{
			m_Definitions = new List<HarvestDefinition>();
		}

		public virtual bool CheckTool( Mobile from, Item tool )
		{
			bool wornOut = ( tool == null || tool.Deleted || (tool is IUsesRemaining && ((IUsesRemaining)tool).UsesRemaining <= 0) );

			if ( wornOut )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool!

			return !wornOut;
		}

		public virtual bool CheckHarvest( Mobile from, Item tool )
		{
			return CheckTool( from, tool );
		}

		public virtual bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			return CheckTool( from, tool );
		}

		public virtual bool CheckRange( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed )
		{
			bool inRange = ( from.Map == map && from.InRange( loc, def.MaxRange ) );

			if ( !inRange )
				def.SendMessageTo( from, timed ? def.TimedOutOfRangeMessage : def.OutOfRangeMessage );

			return inRange;
		}

		public virtual bool CheckResources( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed )
		{
			HarvestBank bank = def.GetBank( map, loc.X, loc.Y );
			bool available = ( bank != null && bank.Current >= def.ConsumedPerHarvest );

			if ( !available )
				def.SendMessageTo( from, timed ? def.DoubleHarvestMessage : def.NoResourcesMessage );

			return available;
		}

        //Taran: Check if region controller allows the skill
        public virtual bool CheckAllowed(Mobile from)
        {
            bool canUse = true;
            CustomRegion cR = from.Region as CustomRegion;

            if (cR == null)
                return true;

            if (this is Lumberjacking && cR.Controller.IsRestrictedSkill(44))
                canUse = false;

            if (this is Mining && cR.Controller.IsRestrictedSkill(45))
                canUse = false;

            if (this is Fishing && cR.Controller.IsRestrictedSkill(18))
                canUse = false;

            if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;

            if (!canUse)
                from.SendAsciiMessage("You cannot do this here");

            return canUse;
        }

		public virtual void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
		}

		public virtual object GetLock( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			/* Here we prevent multiple harvesting.
			 * 
			 * Some options:
			 *  - 'return tool;' : This will allow the player to harvest more than once concurrently, but only if they use multiple tools. This seems to be as OSI.
			 *  - 'return GetType();' : This will disallow multiple harvesting of the same type. That is, we couldn't mine more than once concurrently, but we could be both mining and lumberjacking.
			 *  - 'return typeof( HarvestSystem );' : This will completely restrict concurrent harvesting.
			 */

            return typeof(HarvestSystem);
		}

		public virtual void OnConcurrentHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
		}

		public virtual void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
		}

		public virtual bool BeginHarvesting( Mobile from, Item tool )
		{
			if ( !CheckHarvest( from, tool ) )
				return false;

			from.Target = new HarvestTarget( tool, this );
			return true;
		}

		public virtual void FinishHarvesting( Mobile from, Item tool, HarvestDefinition def, object toHarvest, object locked )
		{
            if (from is PlayerMobile)
                ((PlayerMobile)from).EndPlayerAction();

            if (!CheckHarvest(from, tool))
                return;

            if (!CheckAllowed(from))
                return;

            if (Utility.Random(1000) <= 2)
                AntiMacro.AntiMacroGump.SendGumpThreaded((PlayerMobile)from);

			int tileID;
			Map map;
			Point3D loc;

			if ( !GetHarvestDetails( from, tool, toHarvest, out tileID, out map, out loc ) )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return;
			}
			else if ( !def.Validate( tileID ) )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return;
			}

            if (!CheckRange(from, tool, def, map, loc, true))
                return;
            else if (!CheckResources(from, tool, def, map, loc, true))
                return;
            else if (!CheckHarvest(from, tool, def, toHarvest))
                return;

			if ( SpecialHarvest( from, tool, def, map, loc ) )
				return;

			HarvestBank bank = def.GetBank( map, loc.X, loc.Y );

			if ( bank == null )
				return;

			HarvestVein vein = bank.Vein;

			if ( vein != null )
				vein = MutateVein( from, tool, def, bank, toHarvest, vein );

			if ( vein == null )
				return;

            //Check if we can mine the vein
            CheckMutateVein(from, def, bank, vein);

			HarvestResource primary = vein.PrimaryResource;
			HarvestResource fallback = vein.FallbackResource;
			HarvestResource resource = MutateResource( from, tool, def, map, loc, vein, primary, fallback );

			double skillBase = from.Skills[def.Skill].Base;
			double skillValue = from.Skills[def.Skill].Value;

			Type type = null;

            //Gain the skill
            from.CheckSkill(def.Skill, resource.MinSkill, resource.MaxSkill);

            //hacky All harvest now has 15 percent, if you can mine them.
			if ( skillBase >= resource.ReqSkill && 0.15 < Utility.RandomDouble())
			{
				type = GetResourceType( from, tool, def, map, loc, resource );

				if ( type != null )
					type = MutateType( type, from, tool, def, map, loc, resource );

				if ( type != null )
				{
					Item item = Construct( type, from );

					if ( item == null )
					{
						type = null;
					}
					else
					{
                        //bool spawnNew = false;
						//The whole harvest system is kludgy and I'm sure this is just adding to it.
						if ( item.Stackable )
						{
							int amount = GetOreAmount(vein);
							int racialAmount = (int)Math.Ceiling( amount * 1.1 );

							bool eligableForRacialBonus = ( def.RaceBonus && from.Race == Race.Human );

							if( eligableForRacialBonus && bank.Current >= racialAmount )
								item.Amount = racialAmount;
							else if( bank.Current >= amount )
								item.Amount = amount;
							else
								item.Amount = bank.Current;
								
							//if ( bank.Current <= item.Amount)
								//spawnNew = true;
								
						}

                        bank.Consume(def, item.Amount, from);

                        //Maka
                        //if (spawnNew)
                        //    bank.Vein = def.GetVeinAt(from.Map, from.Location.X, from.Location.Y);

						if ( Give( from, item, /*def.PlaceAtFeetIfFull*/true ) ) //Is there something we dont want to place at the feet
						{
							SendSuccessTo( from, item, resource );
						}
						else
						{
							SendPackFullTo( from, item, def, resource );
							item.Delete();
						}

                        BonusHarvestResource bonus = def.GetBonusResource();

                        if (bonus != null && bonus.Type != null && skillBase >= bonus.ReqSkill)
                        {
                            Item bonusItem = Construct(bonus.Type, from);

                            if (Give(from, bonusItem, true))	//Bonuses always allow placing at feet, even if pack is full irregrdless of def
                            {
                                bonus.SendSuccessTo(from);
                            }
                            else
                            {
                                item.Delete();
                            }
                        }

						if ( tool is IUsesRemaining )
						{
							IUsesRemaining toolWithUses = (IUsesRemaining)tool;

							toolWithUses.ShowUsesRemaining = true;

                            //if ( toolWithUses.UsesRemaining > 0 )
                            //    --toolWithUses.UsesRemaining;

							if ( toolWithUses.UsesRemaining < 1 )
							{
								tool.Delete();
								def.SendMessageTo( from, def.ToolBrokeMessage );
							}
						}
					}
				}
			}

			if ( type == null )
				def.SendMessageTo( from, def.FailMessage );

			OnHarvestFinished( from, tool, def, vein, bank, resource, toHarvest );
		}

        public int GetOreAmount(HarvestVein vein)
        {
            double skillReq = vein.PrimaryResource.ReqSkill;

            if (skillReq >= 98.0)
                return Utility.Random(1, 2);
            else if (skillReq >= 97.0)
                return Utility.Random(1, 3);
            else if (skillReq >= 90.0)
                return Utility.Random(2, 3);
            else if (skillReq >= 75.0)
                return Utility.Random(2, 4);
            else
                return Utility.Random(3, 4);
        }

		public virtual void OnHarvestFinished( Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested )
		{
		}

		public virtual bool SpecialHarvest( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc )
		{
			return false;
		}

		public virtual Item Construct( Type type, Mobile from )
		{
			try{ return Activator.CreateInstance( type ) as Item; }
			catch{ return null; }
		}

		public virtual HarvestVein MutateVein( Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein )
		{
			return vein;
		}

		public virtual void SendSuccessTo( Mobile from, Item item, HarvestResource resource )
		{
			resource.SendSuccessTo( from );
		}

		public virtual void SendPackFullTo( Mobile from, Item item, HarvestDefinition def, HarvestResource resource )
		{
            from.SendAsciiMessage(string.Format("You drop the {0} at your feet.", string.IsNullOrEmpty(item.Name) ? CliLoc.LocToString(item.LabelNumber) : item.Name));
			//def.SendMessageTo( from, def.PackFullMessage );
		}

		public virtual bool Give( Mobile m, Item item, bool placeAtFeet )
		{
			if ( m.PlaceInBackpack( item ) )
				return true;

			if ( !placeAtFeet )
				return false;

			Map map = m.Map;

			if ( map == null )
				return false;

            List<Item> atFeet = new List<Item>();

			foreach ( Item obj in m.GetItemsInRange( 0 ) )
				atFeet.Add( obj );

			for ( int i = 0; i < atFeet.Count; ++i )
			{
                Item check = atFeet[i];

				if ( check.StackWith( m, item, false ) )
					return true;
			}

			item.MoveToWorld( m.Location, map );
			return true;
		}

		public virtual Type MutateType( Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			return from.Region.GetResource( type );
		}

		public virtual Type GetResourceType( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			if ( resource.Types.Length > 0 )
				return resource.Types[Utility.Random( resource.Types.Length )];

			return null;
		}

		public virtual HarvestResource MutateResource( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestVein vein, HarvestResource primary, HarvestResource fallback )
		{
			bool racialBonus = (def.RaceBonus && from.Race == Race.Elf );

			if( vein.ChanceToFallback > (Utility.RandomDouble() + (racialBonus ? .20 : 0)) )
				return fallback;

			double skillValue = from.Skills[def.Skill].Value;

            if (fallback != null && (skillValue < primary.ReqSkill || skillValue < primary.MinSkill))
                return fallback;

			return primary;
		}


        public void CheckMutateVein(Mobile from, HarvestDefinition def, HarvestBank bank, HarvestVein vein)
        {
            if (vein.PrimaryResource.ReqSkill > from.Skills[def.Skill].Base)
            {
                vein = def.Veins[0];
                bank.Vein = vein;
            }

            //while (vein.PrimaryResource.ReqSkill > from.Skills[def.Skill].Base)
            //{
            //    int maxVal = (int)(def.Veins[0].PrimaryResource.ReqSkill*100);
            //    int minVal = (int)(def.Veins[def.Veins.Length - 1].PrimaryResource.ReqSkill * 100);

            //    double spawnChance = Utility.RandomMinMax(minVal, maxVal);
                
            //    //Convert the random number into percent
            //    spawnChance /= 100;

            //    for (int i = def.Veins.Length - 1; i >= 0; i--)
            //    {
            //        if (def.Veins[i].VeinChance >= spawnChance)
            //        {
            //            vein = def.Veins[i];
            //            bank.Vein = vein;
            //            break;
            //        }
            //    }
            //}
        }

		public virtual bool OnHarvesting( Mobile from, Item tool, HarvestDefinition def, object toHarvest, object locked, bool last )
		{
			if ( !CheckHarvest( from, tool ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                
                return false;
			}

			int tileID;
			Map map;
			Point3D loc;

			if ( !GetHarvestDetails( from, tool, toHarvest, out tileID, out map, out loc ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

				OnBadHarvestTarget( from, tool, toHarvest );
				return false;
			}
			else if ( !def.Validate( tileID ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                OnBadHarvestTarget(from, tool, toHarvest);
				return false;
			}
			else if ( !CheckRange( from, tool, def, map, loc, true ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                
                return false;
			}
			else if ( !CheckResources( from, tool, def, map, loc, true ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                return false;
			}
			else if ( !CheckHarvest( from, tool, def, toHarvest ) )
			{
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                return false;
			}

			DoHarvestingEffect( from, tool, def, map, loc );

			new HarvestSoundTimer( from, tool, this, def, toHarvest, locked, last ).Start();

			return !last;
		}

		public virtual void DoHarvestingSound( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( def.EffectSounds.Length > 0 )
				from.PlaySound( Utility.RandomList( def.EffectSounds ) );
		}

		public virtual void DoHarvestingEffect( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc )
		{
			from.Direction = from.GetDirectionTo( loc );

			if( !from.Mounted )
				from.Animate( Utility.RandomList( def.EffectActions ), 5, 1, true, false, 2 );
			else if( def.EffectActionsRiding != null )
				from.Animate( Utility.RandomList( def.EffectActionsRiding ), 5, 1, true, false, 2 );
		}

		public virtual HarvestDefinition GetDefinition( int tileID )
		{
			HarvestDefinition def = null;

			for ( int i = 0; def == null && i < m_Definitions.Count; ++i )
			{
				HarvestDefinition check = m_Definitions[i];

				if ( check.Validate( tileID ) )
					def = check;
			}

			return def;
		}

		public virtual void StartHarvesting( Mobile from, Item tool, object toHarvest )
		{
			if ( !CheckHarvest( from, tool ) )
				return;

			int tileID;
			Map map;
			Point3D loc;

			if ( !GetHarvestDetails( from, tool, toHarvest, out tileID, out map, out loc ) )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return;
			}

			HarvestDefinition def = GetDefinition( tileID );

			if ( def == null )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return;
			}

			if ( !CheckRange( from, tool, def, map, loc, false ) )
				return;
			else if ( !CheckResources( from, tool, def, map, loc, false ) )
				return;
			else if ( !CheckHarvest( from, tool, def, toHarvest ) )
				return;

			object toLock = GetLock( from, tool, def, toHarvest );

            if (from is PlayerMobile && !((PlayerMobile)from).BeginPlayerAction(this, true))
			{
                def.SendMessageTo(from, def.FailMessage);
				OnConcurrentHarvest( from, tool, def, toHarvest );
				return;
			}

			new HarvestTimer( from, tool, this, def, toHarvest, toLock ).Start();
			OnHarvestStarted( from, tool, def, toHarvest );
		}

		public virtual bool GetHarvestDetails( Mobile from, Item tool, object toHarvest, out int tileID, out Map map, out Point3D loc )
		{
			if ( toHarvest is Static && !((Static)toHarvest).Movable )
			{
				Static obj = (Static)toHarvest;

				tileID = (obj.ItemID & 0x3FFF) | 0x4000;
				map = obj.Map;
				loc = obj.GetWorldLocation();
			}
			else if ( toHarvest is StaticTarget )
			{
				StaticTarget obj = (StaticTarget)toHarvest;

				tileID = (obj.ItemID & 0x3FFF) | 0x4000;
				map = from.Map;
				loc = obj.Location;
			}
			else if ( toHarvest is LandTarget )
			{
				LandTarget obj = (LandTarget)toHarvest;

                tileID = obj.TileID;
				map = from.Map;
				loc = obj.Location;
			}
			else
			{
				tileID = 0;
				map = null;
				loc = Point3D.Zero;
				return false;
			}

			return ( map != null && map != Map.Internal );
		}

        #region IAction Members

        public void AbortAction(Mobile from)
        {
        }

        #endregion
    }
}

namespace Server
{
	public interface IChopable
	{
		void OnChop( Mobile from );
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class FurnitureAttribute : Attribute
	{
		public static bool Check( Item item )
		{
			return ( item != null && item.GetType().IsDefined( typeof( FurnitureAttribute ), false ) );
		}
	}
}