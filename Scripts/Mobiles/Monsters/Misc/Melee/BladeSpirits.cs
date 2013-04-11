using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a blade spirit corpse" )]
	public class BladeSpirits : BaseCreature
	{
        public override bool DeleteCorpseOnDeath { get { return Summoned; } }
        public override bool AlwaysMurderer { get { return true; } } // Or Llama vortices will appear gray.
        //public override bool IsHouseSummonable { get { return true; } }

		public override double DispelDifficulty { get { return 0.0; } }
		public override double DispelFocus { get { return 20.0; } }

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
			return ( m.Str + m.Skills[SkillName.Tactics].Value ) / Math.Max( GetDistanceToSqrt( m ), 1.0 );
		}

		[Constructable]
		public BladeSpirits()
			: base( AIType.AI_EvAndBsAI, FightMode.Closest, 10, 1, 0.3, 0.6 )
		{
			Name = "Blade Spirit";
			Body = 574;

			SetStr( 150 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( ( Core.SE ) ? 160 : 80 );
			SetStam( 250 );
			SetMana( 1000 );

			SetDamage( 10, 14 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
		    SetSkill(SkillName.Bushido, 650.0);

			Fame = 0;

			VirtualArmor = 40;
			ControlSlots = ( Core.SE ) ? 2 : 1;
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x23A;
		}

		public override int GetAttackSound()
		{
			return 571;
		}

		public override int GetHurtSound()
		{
			return 0x23A;
		}

        /*public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.1 >= Utility.RandomDouble())
                WhirlwindAttack();
        }*/


		// I'm assuming Blade Spirits are in the same boat as Energy Vortexes are
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

		// Copied from EnergyVortex
		public override bool IsEnemy(Mobile m)
		{
			if (Summoned)
			{
				if (m is BaseGuard) return false;
				return true;
			}

			return base.IsEnemy(m);
		}

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.WhirlwindAttack;
        }

		public BladeSpirits( Serial serial )
			: base( serial )
		{
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
}