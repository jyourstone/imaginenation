using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class SkeletalCaptain : BaseCreature
	{
		[Constructable]
		public SkeletalCaptain() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Skeletal Captain";
			Body = 147;
			BaseSoundID = 451;

			SetStr( 226, 250 );
			SetDex( 176, 195 );
			SetInt( 136, 160 );

			SetHits( 658, 720 );
            SetMana(400, 450);

			SetDamage( 16, 28 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 95.1, 100.0 );
			SetSkill( SkillName.Tactics, 125.1, 140.0 );
			SetSkill( SkillName.Wrestling, 145.1, 155.0 );
            SetSkill( SkillName.Magery, 70.0, 75.0);
            SetSkill(SkillName.Healing, 90.0, 100.0);
            SetSkill(SkillName.Parry, 100.0, 110.0);

			Fame = 6000;
            Karma = -6000;

			VirtualArmor = 50;
			
			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackItem( new Scimitar() );
			PackItem( new WoodenShield() );
            PackItem(new Bandage( 20 ) );
		}

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (Map != null && caster != this && 0.10 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new Skeleton();

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(2) - 1;
                    int y = Y + Utility.Random(2) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(loc, map, 0x3709, 30);
                spawn.Combatant = caster;

                Say("You cannot defeat me!");
            }

            base.OnDamagedBySpell(caster);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (Map != null && attacker != this && 0.10 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new BoneKnight();

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(2) - 1;
                    int y = Y + Utility.Random(2) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(loc, map, 0x3709, 30);
                spawn.Combatant = attacker;

                Say("My minions shall destroy you!");
            }

            base.OnGotMeleeAttack(attacker);
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
            PackGold(700, 850);
            AddItem(new Bone(Utility.RandomMinMax(2, 3)));
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(2, 3)));
		}

		public override bool BleedImmune{ get{ return true; } }
        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool BardImmune { get { return true; } }
        public override bool Uncalmable { get { return true; } }

        public override WeaponAbility GetWeaponAbility()
        {
            switch (Utility.Random(3))
            {
                default:
                case 0: return WeaponAbility.BleedAttack;
                case 1: return WeaponAbility.ParalyzingBlow;
            }
        }

		public SkeletalCaptain( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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