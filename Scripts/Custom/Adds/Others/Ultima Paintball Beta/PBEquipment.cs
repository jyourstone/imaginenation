/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/

using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Games.PaintBall
{
	public class PBArmor : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 3; } }
		public override int InitMaxHits{ get{ return 3; } }

		public override int AosStrReq{ get{ return 0; } }

		public override int OldStrReq{ get{ return 0; } }
		public override int OldDexBonus{ get{ return 0; } }

		public override int ArmorBase{ get{ return 0; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }


		public PBArmor( int hue, int itemid, Layer layer ) : base( itemid )
		{
			Name = "paintball armor";
			Hue = hue;
			Layer = layer;
			Movable = false;
			Weight = 1;
			DexRequirement = 0;
			StrRequirement = 0;
			IntRequirement = 0;
		}

		public override int OnHit( BaseWeapon weapon, int damageTaken )
		{
		return 0;
		}
		public void WasHit(Item weapon, int damageTaken )
		{
	
			if( weapon is PBWeapon )
									{
									HitPoints -= 1;
									}
			if( weapon is PBGrenade )
									{
									HitPoints -= 1;
									}
								
									if( HitPoints == 0 )
									{
										Hue = weapon.Hue;
									}
									return;
								
	
		}

		public PBArmor( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}


	public class PaintBall : Item
	{
		[Constructable]
		public PaintBall( int hue ) : this( hue, 1 )
		{
		}
		[Constructable]

		public PaintBall( int hue, int amount ) : base( 3962 )
		{
			Name = "Paintballs";
			Hue = hue;
			Stackable = true;
			Weight = .1;
			Amount = amount;
			Movable = false;
		}

		public PaintBall( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}


	public class PBWeapon : BaseRanged
	{
		public override int EffectID{ get{ return 3962; } }
		public override Type AmmoType{ get{ return typeof( PaintBall ); } }
		public override Item Ammo{ get{ return new PaintBall( this.Hue ); } }

		public override int AosStrengthReq{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 0; } }
		public override int AosMaxDamage{ get{ return 0; } }
		public override int AosSpeed{ get{ return 15; } }


       // public override float MlSpeed { get { return 2.5f; } }//For ML Change Speed Here


		public override int OldStrengthReq{ get{ return 0; } }
		public override int OldMinDamage{ get{ return 0; } }
		public override int OldMaxDamage{ get{ return 0; } }
		public override int OldSpeed{ get{ return 20; } }

		public override int InitMinHits{ get{ return -1; } }
		public override int InitMaxHits{ get{ return -1; } }

		public override int DefMaxRange{ get{ return 10; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		public PBGameItem m_PBGI;

		public PBWeapon( int hue, PBGameItem pbgi ) : base( 5042 ) 
		{
			Name = "a paintball gun";
			Hue = hue;
			Weight = 1;
			Movable = false;
			DexRequirement = 0;
			StrRequirement = 0;
			IntRequirement = 0;
			Layer = Layer.TwoHanded;

			m_PBGI = pbgi;
		}

		public override TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			if ( DateTime.Now > (attacker.LastMoveTime + TimeSpan.FromSeconds( 1.0 )) )
			{
				if( defender is PlayerMobile || defender is PBNpc )
				{
					if( OnFired( attacker, defender ) )
					{
						if( CheckHit( attacker, defender ) )
						{
							string[] onHitMsg = new string[]
							{
								"The paintball hits your opponent!",
								"You hit your opponent with a paintball!",
								"Hit!",
								"The paintball drenches the person with paint!",
							};
							attacker.SendMessage( onHitMsg[Utility.Random( onHitMsg.Length )] );
							OnHit( attacker, defender );
						}
						else
						{
							string[] onMissMsg = new string[]
							{
								"The paintball flies past your opponent's head!",
								"You hit your opponent with a big gust of air!",
								"Miss!",
								"The paintball drenches the the ground behind the person with paint!",
							};
							attacker.SendMessage( onMissMsg[Utility.Random( onMissMsg.Length )] );
							OnMiss(attacker, defender );
						}
					}
				}
				else
					attacker.SendMessage( "You may only shoot other players with paint!" );
			}
			return GetDelay( attacker );
		}

		public override bool OnFired( Mobile attacker, Mobile defender )
		{
			if( !m_PBGI.Active )
			{
				attacker.SendMessage( "The game has not started yet!" );
				return false;
			}

			if( defender.ChestArmor != null && defender.ChestArmor is PBArmor )
			{
				if( defender.FindItemOnLayer( Layer.Cloak ) != null && defender.FindItemOnLayer( Layer.Cloak ).Hue == Hue )
				{
					attacker.SendMessage( "Do not attack your own team!" );
					return false;
				}
			}
			
			else
			{
				attacker.SendMessage( "You cannot attack someone that is not playing!" );
				defender.SendMessage( "You are not playing Paintball, please leave the area." );
				return false;
			}

			Container pack = attacker.Backpack;

			if( attacker.Player && (pack == null || !pack.ConsumeTotal( AmmoType, 1 ) ) )
			{
				attacker.SendMessage( "You are out of paintballs!" );
				return false;
			}
			if ( defender.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
			attacker.MovingEffect( defender, EffectID, 18, 1, false, false, Hue, 3 );
			return true;
			}
			else
			{
				return false;
			}
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus ) // Updated
		{
			
		
			
			PlayHurtAnimation( defender );
			attacker.PlaySound( GetHitAttackSound( attacker, defender ) );
			defender.PlaySound( GetHitDefendSound( attacker, defender ) );

			int damage = AbsorbDamage( attacker, defender, 0 );
		    base.OnHit( attacker, defender, damageBonus );
		}
		
		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			PlaySwingAnimation( attacker );
			Effects.PlaySound( attacker.Location, attacker.Map, 0x27 );
			Effects.PlaySound( defender.Location, defender.Map, 0x27 );
		}

		public override int AbsorbDamage( Mobile attacker, Mobile defender, int damage )
		{
			PBArmor armor;
			
			
			if( defender.FindItemOnLayer( Layer.Cloak ) == null )
			{
				m_PBGI.KillPlayer( defender );
				defender.SendMessage( "You are not on a team!  Removing you from the game." );
				return 0;
			}

			int defenderTeam = defender.FindItemOnLayer( Layer.Cloak ).Hue;

			if( defender.NeckArmor != null && defender.NeckArmor.Hue == defenderTeam )
				 armor = defender.NeckArmor as PBArmor;
			else if( defender.HandArmor != null && defender.HandArmor.Hue == defenderTeam )
				 armor = defender.HandArmor as PBArmor;
			else if( defender.ArmsArmor != null && defender.ArmsArmor.Hue == defenderTeam )
				 armor = defender.ArmsArmor as PBArmor;
			else if( defender.HeadArmor != null && defender.HeadArmor.Hue == defenderTeam )
				 armor = defender.HeadArmor as PBArmor;
			else if( defender.LegsArmor != null && defender.LegsArmor.Hue == defenderTeam )
				 armor = defender.LegsArmor as PBArmor;
			else if( defender.ChestArmor != null && defender.ChestArmor.Hue == defenderTeam )
				 armor = defender.ChestArmor as PBArmor;
			else
			{
				attacker.SendMessage( "You have just removed that player from the game!" );
				m_PBGI.KillPlayer( defender );
				return 0;
			}


			armor.WasHit( this, damage );
	
			return 0;
		}

		public PBWeapon( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_PBGI );
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch( version )
			{
				case 0:
				{
					m_PBGI = reader.ReadItem() as PBGameItem;
					break;
				}
			}
		}
	}
	public class LayerPack : Backpack	
	{
		[Constructable]
		public LayerPack()
		{

		}


		public LayerPack( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	public class ItemsPack : Backpack	
	{
		[Constructable]
		public ItemsPack()
		{

		}

		public ItemsPack( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
