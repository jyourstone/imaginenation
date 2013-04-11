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
using Server;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;

namespace Server.Games.PaintBall
{

	public class PBNpc : BaseCreature
	{
		public PBGameItem m_PBGI;
		public int m_TeamHue;
		
		
		[Constructable]
		public PBNpc( PBGameItem PGI ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			m_PBGI = PGI;

			if ( Female = Utility.RandomBool() )
			{
				Body = 401;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 400;
				Name = NameList.RandomName( "male" );
			}

			SetStr( 100, 100 );
			SetDex( 100, 100 );
			SetInt( 100, 100 );

			Utility.AssignRandomHair( this );

			
			SetSkill( SkillName.Archery, 100.0, 100.0 );
			SetSkill( SkillName.Anatomy, 100.0, 100.0 );
			SetSkill( SkillName.Tactics, 100.0, 100.0 );
			SetSkill( SkillName.Meditation, 100.0, 100.0 );
			SetSkill( SkillName.Tactics, 100.0, 100.0 );
			
			Container pack = this.Backpack;
			
			if ( pack == null )
			{
				pack = new Backpack();
				pack.Movable = false;

				this.AddItem( pack );
			}
			
			m_PBGI.AddPlayer( this );


		}
		public override void OnDoubleClick( Mobile from )
		{
			if( m_PBGI == null )
			{
				from.Target = new PBNpcAssignTarget( this );
			    from.SendMessage( "Target the Paintball Game Stone you wish to link this scoreboard to." );
			}
		}
		

		public override bool AlwaysMurderer{ get{ return true; } }


		public override bool IsEnemy( Mobile m )
		{
			if ( m_PBGI != null && m_PBGI.Active && this != null && m != null )
			{
				if ( this.FindItemOnLayer( Layer.Cloak ) != null &&  m.FindItemOnLayer( Layer.Cloak ) != null)
				    
				{
					if  ( this.FindItemOnLayer( Layer.Cloak ).Hue != m.FindItemOnLayer( Layer.Cloak ).Hue  )
					{
							return true;
					}
					else
					{
						return false;
					}
				
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
			//return base.IsEnemy( m );
			
		
		}


		public PBNpc( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
            if (m_PBGI != null)
                writer.Write(m_PBGI);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            m_PBGI = reader.ReadItem() as PBGameItem;
		}
	}
	public class PBNpcAssignTarget : Target // Create our targeting class (which we derive from the base target class)
	{
		private PBNpc m_PBN;

		public PBNpcAssignTarget( PBNpc pn ) : base( 100, false, TargetFlags.None )
		{
			m_PBN = pn;
		}
		protected override void OnTarget( Mobile from, object target ) // Override the protected OnTarget() for our feature
		{
			PBNpc PBN = (PBNpc)m_PBN;

			if ( PBN.Deleted)
				return;

			if ( target is PBGameItem )
			{
				PBGameItem item = (PBGameItem)target;
				
				

				PBN.m_PBGI = item;
				
				
				if ( PBN != null )
				{
					PBN.m_PBGI.EstablishArrays();
					PBN.m_PBGI.Npcs.Add(m_PBN);
				}
				else
				{
					from.SendMessage("PBNPC is Null");
				}
				
				
				
				
				
			}
			else
			{
				from.SendMessage( "That is not a Paintball Game Stone." ); 
			}
		}
	}
}
