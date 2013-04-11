//revised

using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "A Charred Corpse" )]
	public class FireLord : BaseCreature
	{
        public override bool ReacquireOnMovement { get { return true; } }
        public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override bool Uncalmable { get { return Core.SE; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool HasBreath { get { return true; } } 
        public override int BreathFireDamage { get { return 20; } }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(3))
            {
                default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.MortalStrike;
                case 2: return WeaponAbility.Dismount;
            }
        }

		[Constructable]
		public FireLord() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Fire Lord";
			Body = 76;
			BaseSoundID = 609;
            Hue = 0x9D7;

			SetStr( 436, 485 );
			SetDex( 326, 385 );
			SetInt( 981, 1005 );

			SetHits( 1822, 2251 );

			SetDamage( 18, 26 );

			SetDamageType( ResistanceType.Physical, 50 );

			SetResistance( ResistanceType.Physical, 40, 65 );
			SetResistance( ResistanceType.Fire, 100, 120 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 60, 75 );

			SetSkill( SkillName.EvalInt, 105.1, 130.0 );
			SetSkill( SkillName.Magery, 125.1, 140.0 );
			SetSkill( SkillName.MagicResist, 80.2, 110.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.Wrestling, 90.1, 111.0 );
            SetSkill(SkillName.Healing, 105.2, 110.1);

			Fame = 8500;
			Karma = -6500;

			VirtualArmor = 70;

            AddItem(new LightSource());
		}

		public override void GenerateLoot()
		{
            AddLoot(LootPack.UltraRich );
			AddLoot( LootPack.MedScrolls, 2);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems, 6);
            PackGold(1500, 3000);
            PackItem(Engines.Plants.Seed.RandomBonsaiSeed());
		}

                public override bool OnBeforeDeath()
        {                       
            return base.OnBeforeDeath();
        }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }


        public override void OnDamagedBySpell(Mobile caster)
        {
            if (Map != null && caster != this && 0.15 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new FlamingMinion(this);

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(8) - 1;
                    int y = Y + Utility.Random(8) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(loc, map, 0x3709, 30);
                spawn.Combatant = caster;
        
                Say("Come to me my Minions!"); 
            }

            base.OnDamagedBySpell(caster);
        }


        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (Map != null && attacker != this && 0.20 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new FlamingMinion(this);

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(8) - 1;
                    int y = Y + Utility.Random(8) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(loc, map, 0x3709, 30);
                spawn.Combatant = attacker;

                Say("Come to me my Minions!"); 
            }

            base.OnGotMeleeAttack(attacker);
        }
        public FireLord(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}