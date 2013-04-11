using System;

namespace Server.Mobiles
{
	[CorpseName( "an energy vortex corpse" )]
	public class EnergyVortex : BaseCreature
	{
		[Constructable]
		public EnergyVortex() : base( AIType.AI_EvAndBsAI, FightMode.Closest, 10, 1, 0.1, 0.1 )
		{
			Name = "Energy Vortex";

			if( 0.20 > Utility.RandomDouble() ) // 20% to get a blue vortex
				Body = 0x23d;
			else
				Body = 164;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 60 );

			SetHits( 200 );
			SetStam( 200 );
			SetMana( 0 );

			SetDamage( 20, 35 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Physical, 20, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 200.9 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 222;

			//VirtualArmor = 20;
			ControlSlots = ( Core.SE ) ? 2 : 1;
		}

		public EnergyVortex( Serial serial ) : base( serial )
		{
		}

		public override bool DeleteCorpseOnDeath
		{
			get { return Summoned; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; } // Or Llama vortices will appear gray.
		}

		public override double DispelDifficulty
		{
			get { return 0.0; }
		}

		public override double DispelFocus
		{
			get { return 20.0; }
		}

		public override bool BleedImmune
		{
			get { return true; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Lethal; }
		}

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
		{
			return ( m.Int + m.Skills[SkillName.Magery].Value ) / Math.Max( GetDistanceToSqrt( m ), 1.0 );
		}

		public override int GetAngerSound()
		{
			if( 0.50 > Utility.RandomDouble() )
				return 263;
			else
				return 264;
		}

		public override int GetIdleSound()
		{
			if (0.5 > Utility.RandomDouble())
				return 263;
			else
				return 264;
		}

		public override int GetHurtSound()
		{
			return 266;
		}
		public override int GetAttackSound()
		{
			if (0.5 > Utility.RandomDouble())
				return 21;
			else
				return 264;
		}
		
		/* This is simply wrong. This code does not belong here. This is already handled in AquireFocusMob, which also
		 * does a lot of other checks that this method does not (as in, checks for whether or not the mobile is hidden, or dead
		 * or a GM). - Jonny */
		/*public override void OnThink()
		{
			if (Summoned)
			{
				foreach (Mobile m in GetMobilesInRange(2))
				{
					if ((m is EnergyVortex || m is BladeSpirits || m is PlayerMobile) && m != this)
					{
						if (m is PlayerMobile)
						{
							Combatant = m;
							ControlOrder = OrderType.Attack;
						}
						else if (((BaseCreature)m).Controlled)
						{
							Combatant = m;
							ControlOrder = OrderType.Attack;
						}
					}
				}
			}
			base.OnThink();
		}*/

		/* This is instead of the above method. This seemed like the most logical change to make EV attack everything, while
		 * still following the basic AI rules set in BaseAI. 
		 */
		public override bool IsEnemy(Mobile m)
		{
			if (Summoned)
			{
				if (m is BaseGuard) return false; // Should EVs also attack guards?
                if (m.Hidden) return false; // Taran: Doesn't attack hidden players
				return true; // Summoned EVs attack everything else
			}

			return base.IsEnemy(m);
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

			if( BaseSoundID == 263 )
				BaseSoundID = 0;
		}
	}
}