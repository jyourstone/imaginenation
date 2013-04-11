//revised
namespace Server.Mobiles
{
	[CorpseName( "a slimy corpse" )]
	public class GreaterSlime : BaseCreature
	{
		[Constructable]
		public GreaterSlime () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Greater Slime";
			Body = 775;
			BaseSoundID = 0;
            Hue = 2701;

			SetStr( 250 );
			SetDex( 170 );
			SetInt( 35 );

			SetHits( 553 );

			SetDamage( 18, 36 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 100 );

			SetResistance( ResistanceType.Physical, 17, 20 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 10, 23 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 85.0 );

			VirtualArmor = 40;
		}

        public override bool AutoDispel{ get{ return true; } }
        public override bool Unprovokable{ get{ return true; } }
        public override Poison PoisonImmune{ get{ return Poison.Greater; } }
        public override Poison HitPoison{ get{ return Poison.Greater; } }

        public override bool OnBeforeDeath()
        {
            if (Map != null && Combatant != null)
            {
                Map map = Map;

                if (map == null)
                    return false;                
                for (int k = 0; k < 3; ++k)
                {
                    BaseCreature spawn = new PoisonousSlime();
                    spawn.Team = Team;
                    bool validLocation = false;
                    Point3D loc = Location;                
                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = X + Utility.Random(3) - 1;
                        int y = Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                            loc = new Point3D(x, y, Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }
                    PlaySound( Utility.RandomMinMax( 457, 459 ) );
                    spawn.MoveToWorld(loc, map);
                    spawn.Combatant = Combatant;
                }
            }
            Delete();
            return base.OnBeforeDeath();
        }

		public GreaterSlime( Serial serial ) : base( serial )
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