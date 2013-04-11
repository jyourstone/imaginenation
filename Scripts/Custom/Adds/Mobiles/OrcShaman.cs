using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "an orcish corpse" )]
	public class OrcishShaman : BaseCreature
	{
        private DateTime recoverDelay;
        bool firstSummoned = true;
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }

		[Constructable]
		public OrcishShaman () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Name = "Orc Shaman";
			Body = 17;
            Hue = 0;
            BaseSoundID = 0x45A;

			SetStr( 200 );
			SetDex( 170 );
			SetInt( 350 );

			SetHits( 590 );
            SetMana( 378 );

			SetDamage( 6, 12 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 17, 20 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 10, 23 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 10, 20 );

            SetSkill(SkillName.Meditation, 90.0);
            SetSkill(SkillName.Magery, 40.1, 55.0);            
			SetSkill(SkillName.EvalInt, 80.0);
            SetSkill(SkillName.MagicResist, 80.2, 110.0);
            SetSkill(SkillName.Wrestling, 70.1, 85.0);
            SetSkill(SkillName.Healing, 85.2, 97.1);

            Fame = 11500;
            Karma = -11500;
			VirtualArmor = 40;
		}

        public override bool BardImmune { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool Unprovokable { get { return true; } }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (this.Map != null && this.Alive && ((recoverDelay < DateTime.Now && Utility.RandomMinMax(1, 4) == 1) || firstSummoned ))
            {
                Map map = this.Map;
                if (map == null)
                    return;
                if( firstSummoned)
                    firstSummoned = false;
                int summonAmount = Utility.RandomMinMax(1, 2);
                for (int k = 0; k < summonAmount; ++k)
                {
                    BaseCreature spawn = new DireWolf();
                    spawn.Team = this.Team;
                    spawn.Name = "Dire Wolf";
                    spawn.Tamable = false;
                    bool validLocation = false;
                    Point3D loc = this.Location;                
                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = X + Utility.Random(3) - 1;
                        int y = Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                            loc = new Point3D(x, y, Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }                   
                    spawn.MoveToWorld(loc, map);
                    Effects.SendLocationEffect(loc, map, 14170, 16);
                    spawn.Combatant = this.Combatant;
                }
                recoverDelay = DateTime.Now + TimeSpan.FromSeconds(30);                
            }
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            if (this.Map != null && this.Alive && ((recoverDelay < DateTime.Now && Utility.RandomMinMax(1, 4) == 1) || firstSummoned))
            {
                Map map = this.Map;
                if (map == null)
                    return;
                if (firstSummoned)
                    firstSummoned = false;
                int summonAmount = Utility.RandomMinMax(1, 2);
                for (int k = 0; k < summonAmount; ++k)
                {
                    BaseCreature spawn = new DireWolf();
                    spawn.Team = this.Team;
                    spawn.Name = "Dire Wolf";
                    spawn.Tamable = false;
                    bool validLocation = false;
                    Point3D loc = this.Location;
                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = X + Utility.Random(3) - 1;
                        int y = Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                            loc = new Point3D(x, y, Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }
                    spawn.MoveToWorld(loc, map);
                    Effects.SendLocationEffect(loc, map, 14170, 16);
                    spawn.Combatant = this.Combatant;
                }
                recoverDelay = DateTime.Now + TimeSpan.FromSeconds(30);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            PackGold(200);
        }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(2))
            {
                default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.ParalyzingBlow;
            }
        }

		public OrcishShaman( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}