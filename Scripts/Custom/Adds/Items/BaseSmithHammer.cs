using System;
using Server.Custom.CraftMenu;
using Server.Engines.Craft;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.Items
{
	public class BaseSmithHammer : BaseTool
	{
		public BaseSmithHammer( int itemID ) : this( Utility.RandomMinMax( 25, 75 ), itemID )
		{
		}

		public BaseSmithHammer( int uses, int itemID ) : base( itemID )
		{
			UsesRemaining = uses;
			Quality = ToolQuality.Regular;
		}

		public BaseSmithHammer( Serial serial ) : base( serial )
		{
		}

		public override CraftSystem CraftSystem
		{
			get { return DefBlacksmithy.CraftSystem; }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !Sphere.EquipOnDouble( from, this ) )
				return;

			from.Target = new SmithTarget( this, CraftSystem );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class SmithTarget : Target
	{
		private readonly Item m_Tool;
		private readonly CraftSystem m_CraftSystem;

		public SmithTarget( Item tool, CraftSystem craftSystem ) : base( 6, false, TargetFlags.None )
		{
			m_Tool = tool;
			m_CraftSystem = craftSystem;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			bool hasForge, hasAnvil;
			DefBlacksmithy.CheckAnvilAndForge( from, 4, out hasAnvil, out hasForge );

			if( targeted is BaseIngot && m_Tool is BaseTool )
            if( hasAnvil && hasForge )
				{
                    BaseTool tool = (BaseTool)m_Tool;
				    BaseIngot ingot = (BaseIngot)targeted;
					CraftSubRes subRes = CustomCraftMenu.GetSubRes( DefBlacksmithy.CraftSystem, targeted.GetType(), null );
                    int num = CraftResources.GetIndex(ingot.Resource);

					if( subRes == null || !CustomCraftMenu.ResourceInfoList.ContainsKey( subRes ) )
					{
						from.SendAsciiMessage( "You can't use that." );
						return;
					}

                    if (from.Skills[DefBlacksmithy.CraftSystem.MainSkill].Base < subRes.RequiredSkill)
                    {
                        from.SendAsciiMessage("You cannot work this strange and unusual metal.");
                        return;
                    }

                    from.SendGump(new CraftGump(from, m_CraftSystem, tool, null, num));
					//from.SendMenu( new CustomCraftMenu( from, DefBlacksmithy.CraftSystem, ( (BaseTool)m_Tool ), -1, targeted.GetType(), CustomCraftMenu.ResourceInfoList[subRes].ResourceIndex ) );
				}
				else
					from.SendAsciiMessage( "You need to be close to a forge and anvil to smith." );
			else if( targeted is BaseIngot && !( m_Tool is BaseTool ) )
				from.SendAsciiMessage( "You need to use a smith's hammer on that." );
			else
			{
				if( !hasAnvil )
				{
					from.SendAsciiMessage( "You need to be close to an anvil to repair." );
					return;
				}

				int number;

				if( targeted is BaseWeapon )
				{
					BaseWeapon weapon = (BaseWeapon)targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 0;

					if( Core.AOS )
						toWeaken = 1;
					else if( skill != SkillName.Tailoring )
					{
						double skillLevel = from.Skills[skill].Base;

						if( skillLevel >= 90.0 )
							toWeaken = 1;
						else if( skillLevel >= 70.0 )
							toWeaken = 2;
						else
							toWeaken = 3;
					}

					if( m_CraftSystem.CraftItems.SearchForSubclass( weapon.GetType() ) == null && !IsSpecialWeapon( weapon ) )
						number = 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
					else if( !weapon.IsChildOf( from.Backpack ) )
						number = 1044275; // The item must be in your backpack to repair it.
					else if( weapon.MaxHitPoints <= 0 || weapon.HitPoints == weapon.MaxHitPoints )
						number = 1044281; // That item is in full repair
					else if( weapon.MaxHitPoints <= toWeaken )
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					else
					{
						if( CheckWeaken( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
							weapon.MaxHitPoints -= toWeaken;
							weapon.HitPoints = Math.Max( 0, weapon.HitPoints - toWeaken );
						}

						if( CheckRepairDifficulty( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							weapon.HitPoints = weapon.MaxHitPoints;
						}
						else
						{
							number = 1044280; // You fail to repair the item. [And the contract is destroyed]
							m_CraftSystem.PlayCraftEffect( from );
						}
					}
				}
				else if( targeted is BaseArmor )
				{
					BaseArmor armor = (BaseArmor)targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 0;

					if( Core.AOS )
						toWeaken = 1;
					else if( skill != SkillName.Tailoring )
					{
						double skillLevel = from.Skills[skill].Base;

						if( skillLevel >= 90.0 )
							toWeaken = 1;
						else if( skillLevel >= 70.0 )
							toWeaken = 2;
						else
							toWeaken = 3;
					}

					if( m_CraftSystem.CraftItems.SearchForSubclass( armor.GetType() ) == null )
						number = 1044277;
						// That item cannot be repaired. // You cannot repair that item with this type of repair contract.
					else if( !armor.IsChildOf( from.Backpack ) )
						number = 1044275; // The item must be in your backpack to repair it.
					else if( armor.MaxHitPoints <= 0 || armor.HitPoints == armor.MaxHitPoints )
						number = 1044281; // That item is in full repair
					else if( armor.MaxHitPoints <= toWeaken )
						number = 1044278;
						// That item has been repaired many times, and will break if repairs are attempted again.
					else
					{
						if( CheckWeaken( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
							armor.MaxHitPoints -= toWeaken;
							armor.HitPoints = Math.Max( 0, armor.HitPoints - toWeaken );
						}

						if( CheckRepairDifficulty( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							armor.HitPoints = armor.MaxHitPoints;
						}
						else
						{
							number = 1044280;
							// You fail to repair the item. [And the contract is destroyed]
							m_CraftSystem.PlayCraftEffect( from );
						}
					}
				}
				else if( targeted is Item )
					number = 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
				else
					number = 500426; // You can't repair that.
				from.SendAsciiMessage( CliLoc.LocToString( number ) );
			}
		}

		private bool CheckAnvil( Mobile from )
		{
			Map map = from.Map;
			bool isAnvil = false;

			if( map == null )
				return false;

			IPooledEnumerable eable = map.GetItemsInRange( from.Location, 3 );

			foreach( Item item in eable )
			{
				Type type = item.GetType();
				isAnvil = ( type.IsDefined( typeof( AnvilAttribute ), false ) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6 );

				if( isAnvil )
					break;
			}
			eable.Free();

			if( isAnvil )
				return true;
			else
				return false;
		}

		private bool IsSpecialWeapon( BaseWeapon weapon )
		{
			// Weapons repairable but not craftable

			if( m_CraftSystem is DefTinkering )
				return ( weapon is Cleaver ) || ( weapon is Hatchet ) || ( weapon is Pickaxe ) || ( weapon is ButcherKnife ) || ( weapon is SkinningKnife );
			else if( m_CraftSystem is DefCarpentry )
				return ( weapon is Club ) || ( weapon is BlackStaff ) || ( weapon is MagicWand );
			else if( m_CraftSystem is DefBlacksmithy )
				return ( weapon is Pitchfork );

			return false;
		}

		private bool CheckWeaken( Mobile mob, SkillName skill, int curHits, int maxHits )
		{
			return ( GetWeakenChance( mob, skill, curHits, maxHits ) > Utility.Random( 100 ) );
		}

		private int GetWeakenChance( Mobile mob, SkillName skill, int curHits, int maxHits )
		{
			// 40% - (1% per hp lost) - (1% per 10 craft skill)
			return ( 40 + ( maxHits - curHits ) ) - (int)( ( mob.Skills[skill].Value ) / 10 );
		}

		private int GetRepairDifficulty( int curHits, int maxHits )
		{
			return ( ( ( maxHits - curHits ) * 1250 ) / Math.Max( maxHits, 1 ) ) - 250;
		}

		private bool CheckRepairDifficulty( Mobile mob, SkillName skill, int curHits, int maxHits )
		{
			double difficulty = GetRepairDifficulty( curHits, maxHits ) * 0.1;
			return mob.CheckSkill( skill, difficulty - 25.0, difficulty + 25.0 );
		}
	}
}