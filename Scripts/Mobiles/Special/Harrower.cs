using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	public class Harrower : BaseCreature
	{
        public Type[] UniqueList { get { return new Type[] { typeof(AcidProofRobe) }; } }
        public Type[] SharedList { get { return new Type[] { typeof(TheRobeOfBritanniaAri) }; } }
        public Type[] DecorativeList { get { return new Type[] { typeof(EvilIdolSkull), typeof(SkullPole) }; } }

		private bool m_TrueForm;
		private Item m_GateItem;
		private List<HarrowerTentacles> m_Tentacles;
		private Timer m_Timer;

		private class SpawnEntry
		{
			public readonly Point3D m_Location;
			public readonly Point3D m_Entrance;

			public SpawnEntry( Point3D loc, Point3D ent )
			{
				m_Location = loc;
				m_Entrance = ent;
			}
		}

		private static readonly SpawnEntry[] m_Entries = new SpawnEntry[]
			{
				new SpawnEntry( new Point3D( 5242, 945, -40 ), new Point3D( 1176, 2638, 0 ) ),	// Destard
				new SpawnEntry( new Point3D( 5225, 798, 0 ), new Point3D( 1176, 2638, 0 ) ),	// Destard
				new SpawnEntry( new Point3D( 5556, 886, 30 ), new Point3D( 1298, 1080, 0 ) ),	// Despise
				new SpawnEntry( new Point3D( 5187, 615, 0 ), new Point3D( 4111, 432, 5 ) ),		// Deceit
				new SpawnEntry( new Point3D( 5319, 583, 0 ), new Point3D( 4111, 432, 5 ) ),		// Deceit
				new SpawnEntry( new Point3D( 5713, 1334, -1 ), new Point3D( 2923, 3407, 8 ) ),	// Fire
				new SpawnEntry( new Point3D( 5860, 1460, -2 ), new Point3D( 2923, 3407, 8 ) ),	// Fire
				new SpawnEntry( new Point3D( 5328, 1620, 0 ), new Point3D( 5451, 3143, -60 ) ),	// Terathan Keep
				new SpawnEntry( new Point3D( 5690, 538, 0 ), new Point3D( 2042, 224, 14 ) ),	// Wrong
				new SpawnEntry( new Point3D( 5609, 195, 0 ), new Point3D( 514, 1561, 0 ) ),		// Shame
				new SpawnEntry( new Point3D( 5475, 187, 0 ), new Point3D( 514, 1561, 0 ) ),		// Shame
				new SpawnEntry( new Point3D( 6085, 179, 0 ), new Point3D( 4721, 3822, 0 ) ),	// Hythloth
				new SpawnEntry( new Point3D( 6084, 66, 0 ), new Point3D( 4721, 3822, 0 ) ),		// Hythloth
				new SpawnEntry( new Point3D( 5499, 2003, 0 ), new Point3D( 2499, 919, 0 ) ),	// Covetous
				new SpawnEntry( new Point3D( 5579, 1858, 0 ), new Point3D( 2499, 919, 0 ) )		// Covetous
			};

		private static readonly ArrayList m_Instances = new ArrayList();

		public static ArrayList Instances{ get{ return m_Instances; } }

		public static Harrower Spawn( Point3D platLoc, Map platMap )
		{
			if ( m_Instances.Count > 0 )
				return null;

			SpawnEntry entry = m_Entries[Utility.Random( m_Entries.Length )];

			Harrower harrower = new Harrower();

			harrower.MoveToWorld( entry.m_Location, Map.Felucca );

			harrower.m_GateItem = new HarrowerGate( harrower, platLoc, platMap, entry.m_Entrance, Map.Felucca );

			return harrower;
		}

		public static bool CanSpawn
		{
			get
			{
				return ( m_Instances.Count == 0 );
			}
		}

		[Constructable]
		public Harrower() : base( AIType.AI_SphereMage, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			m_Instances.Add( this );

			Name = "the harrower";
			BodyValue = 146;

			SetStr( 900, 1000 );
			SetDex( 125, 135 );
			SetInt( 1000, 1200 );

			Fame = 22500;
			Karma = -22500;

            SpeechHue = 1158;

			VirtualArmor = 60;

            Container backpack = Backpack;
            if (backpack == null)
            {
                backpack = new Backpack();

                backpack.Movable = false;

                AddItem(backpack);
            }

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 60, 80 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.2, 110.0 );
			SetSkill( SkillName.MagicResist, 120.2, 160.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );

			m_Tentacles = new List<HarrowerTentacles>();

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 2 );
			AddLoot( LootPack.Meager );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		private static readonly double[] m_Offsets = new double[]
			{
				Math.Cos( 000.0 / 180.0 * Math.PI ), Math.Sin( 000.0 / 180.0 * Math.PI ),
				Math.Cos( 040.0 / 180.0 * Math.PI ), Math.Sin( 040.0 / 180.0 * Math.PI ),
				Math.Cos( 080.0 / 180.0 * Math.PI ), Math.Sin( 080.0 / 180.0 * Math.PI ),
				Math.Cos( 120.0 / 180.0 * Math.PI ), Math.Sin( 120.0 / 180.0 * Math.PI ),
				Math.Cos( 160.0 / 180.0 * Math.PI ), Math.Sin( 160.0 / 180.0 * Math.PI ),
				Math.Cos( 200.0 / 180.0 * Math.PI ), Math.Sin( 200.0 / 180.0 * Math.PI ),
				Math.Cos( 240.0 / 180.0 * Math.PI ), Math.Sin( 240.0 / 180.0 * Math.PI ),
				Math.Cos( 280.0 / 180.0 * Math.PI ), Math.Sin( 280.0 / 180.0 * Math.PI ),
				Math.Cos( 320.0 / 180.0 * Math.PI ), Math.Sin( 320.0 / 180.0 * Math.PI ),
			};

		public void Morph()
		{
			if ( m_TrueForm )
				return;

			m_TrueForm = true;

			Name = "the true harrower";
			BodyValue = 780;
			Hue = 0x497;

			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;

            Blessed = true;

            Hidden = true;

            new TransformTimer(this).Start();
        }

        public void ContinueMorph()
        {
            Blessed = false;

			ProcessDelta();

			Say( 1049499 ); // Behold my true form!

			Map map = Map;

			if ( map != null )
			{
				for ( int i = 0; i < m_Offsets.Length; i += 2 )
				{
					double rx = m_Offsets[i];
					double ry = m_Offsets[i + 1];

					int dist = 0;
					bool ok = false;
					int x = 0, y = 0, z = 0;

					while ( !ok && dist < 10 )
					{
						int rdist = 10 + dist;

						x = X + (int)(rx * rdist);
						y = Y + (int)(ry * rdist);
						z = map.GetAverageZ( x, y );

						if ( !(ok = map.CanFit( x, y, Z, 16, false, false ) ) )
							ok = map.CanFit( x, y, z, 16, false, false );

						if ( dist >= 0 )
							dist = -(dist + 1);
						else
							dist = -(dist - 1);
					}

					if ( !ok )
						continue;

					HarrowerTentacles spawn = new HarrowerTentacles( this );

					spawn.Team = Team;

					spawn.MoveToWorld( new Point3D( x, y, z ), map );

					m_Tentacles.Add( spawn );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax{ get{ return m_TrueForm ? 65000 : 30000; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax{ get{ return 5000; } }

		public Harrower( Serial serial ) : base( serial )
		{
			m_Instances.Add( this );
		}

		public override void OnAfterDelete()
		{
			m_Instances.Remove( this );

			base.OnAfterDelete();
		}

		public override bool DisallowAllMoves{ get{ return m_TrueForm; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_TrueForm );
			writer.Write( m_GateItem );
			writer.WriteMobileList( m_Tentacles );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_TrueForm = reader.ReadBool();
					m_GateItem = reader.ReadItem();
					m_Tentacles = reader.ReadStrongMobileList<HarrowerTentacles>();

					m_Timer = new TeleportTimer( this );
					m_Timer.Start();

					break;
				}
			}
		}

		public void GivePowerScrolls()
		{
			List<Mobile> toGive = new List<Mobile>();
			List<DamageStore> rights = GetLootingRights( DamageEntries, HitsMax );

			for ( int i = rights.Count - 1; i >= 0; --i )
			{
				DamageStore ds = rights[i];

				if ( ds.m_HasRight )
					toGive.Add( ds.m_Mobile );
			}

			if ( toGive.Count == 0 )
				return;

			// Randomize
			for ( int i = 0; i < toGive.Count; ++i )
			{
				int rand = Utility.Random( toGive.Count );
				Mobile hold = toGive[i];
				toGive[i] = toGive[rand];
				toGive[rand] = hold;
			}

			for ( int i = 0; i < 16; ++i )
			{
				int level;
				double random = Utility.RandomDouble();

				if ( 0.1 >= random )
					level = 25;
				else if ( 0.25 >= random )
					level = 20;
				else if ( 0.45 >= random )
					level = 15;
				else if ( 0.70 >= random )
					level = 10;
				else
					level = 5;

				Mobile m = toGive[i % toGive.Count];

				m.SendLocalizedMessage( 1049524 ); // You have received a scroll of power!
				m.AddToBackpack( new StatCapScroll( 225 + level ) );

				if ( m is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile)m;

					for ( int j = 0; j < pm.JusticeProtectors.Count; ++j )
					{
						Mobile prot = pm.JusticeProtectors[j];

						if ( prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion( m, prot ) )
							continue;

						int chance = 0;

						switch ( VirtueHelper.GetLevel( prot, VirtueName.Justice ) )
						{
							case VirtueLevel.Seeker: chance = 60; break;
							case VirtueLevel.Follower: chance = 80; break;
							case VirtueLevel.Knight: chance = 100; break;
						}

						if ( chance > Utility.Random( 100 ) )
						{
							prot.SendLocalizedMessage( 1049368 ); // You have been rewarded for your dedication to Justice!
							prot.AddToBackpack( new StatCapScroll( 225 + level ) );
						}
					}
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( m_TrueForm )
            {
                #region Taran: Reward all attackers
                List<DamageEntry> rights2 = DamageEntries;
                List<Mobile> toGiveGold = new List<Mobile>();
                List<Mobile> toGiveItem = new List<Mobile>();
                List<Mobile> toRemove = new List<Mobile>();
                List<int> GoldToRecieve = new List<int>();

                for (int i = 0; i < rights2.Count; ++i)
                {
                    DamageEntry de = rights2[i];

                                        //Only players get rewarded
                    if (de.HasExpired || !de.Damager.Player)
                    {
                        DamageEntries.RemoveAt(i);
                        continue;
                    }

                    toGiveGold.Add(de.Damager);
                    GoldToRecieve.Add(de.DamageGiven * 10); //Player gets 10 times the damage dealt in gold

                    if (de.DamageGiven > 1000) //Players doing more than 1000 damage gets a random weapon or armor
                        toGiveItem.Add(de.Damager);
                }

                foreach (Mobile m in toGiveGold)
                {
                    if (m is PlayerMobile)
                    {
                        int amountofgold = GoldToRecieve[toGiveGold.IndexOf(m)];

                        if (amountofgold > 100000)
                            amountofgold = 100000; //Taran: Could be good with a max of 100k if damage bugs occur

                        if (amountofgold > 65000)
                            m.AddToBackpack(new BankCheck(amountofgold));
                        else
                            m.AddToBackpack(new Gold(amountofgold));

                        m.SendAsciiMessage("You dealt {0} damage to the champion and got {1} gold", amountofgold / 10, amountofgold);
                    }
                }

                foreach (Mobile m in toGiveItem)
                {
                    if (m is PlayerMobile)
                    {
                        Item item = Loot.RandomArmorOrShieldOrWeapon();

                        if (item is BaseWeapon)
                        {
                            BaseWeapon weapon = (BaseWeapon)item;
                            weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(6);
                            weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(6);
                            weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(6);
                        }
                        else if (item is BaseArmor)
                        {
                            BaseArmor armor = (BaseArmor)item;
                            armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(6);
                            armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
                        }

                        m.AddToBackpack(item);
                        m.SendAsciiMessage("You dealt more than 1000 damage to the harrower, your reward is well deserved!");
                    }
                }

                //Remove all monsters within 20 tiles when the harrower is killed
                foreach (Mobile m in Map.GetMobilesInRange(Location, 20))
                {
                    if (m is BaseCreature && !(m is Harrower))
                    {
                        BaseCreature bc = (BaseCreature)m;
                        if (!(m is BaseMount) && m.Spawner == null && bc.LastOwner == null && !bc.Controlled)
                            toRemove.Add(m);
                    }
                }

                foreach (Mobile m in toRemove)
                {
                    if (m != null)
                        m.Delete();
                }
                #endregion

                List<DamageStore> rights = GetLootingRights( DamageEntries, HitsMax );

				for ( int i = rights.Count - 1; i >= 0; --i )
				{
					DamageStore ds = rights[i];

					if ( ds.m_HasRight && ds.m_Mobile is PlayerMobile )
						PlayerMobile.ChampionTitleInfo.AwardHarrowerTitle( (PlayerMobile)ds.m_Mobile );
				}

				if ( !NoKillAwards )
				{
					//GivePowerScrolls();

					Map map = Map;

					if ( map != null )
					{
						for ( int x = -16; x <= 16; ++x )
						{
							for ( int y = -16; y <= 16; ++y )
							{
								double dist = Math.Sqrt(x*x+y*y);

								if ( dist <= 16 )
									new GoodiesTimer( map, X + x, Y + y ).Start();
							}
						}
					}

					for ( int i = 0; i < m_Tentacles.Count; ++i )
					{
						Mobile m = m_Tentacles[i];

						if ( !m.Deleted )
							m.Kill();
					}

					m_Tentacles.Clear();

					if ( m_GateItem != null )
						m_GateItem.Delete();
				}

				return base.OnBeforeDeath();
			}
			else
			{
				Morph();
				return false;
			}
		}
        
		private class TeleportTimer : Timer
		{
            private List<String> TeleportTalk = new List<String>();
            private int TeleportTalkIndex = 0;

			private Mobile m_Owner;

			private static readonly int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

			public TeleportTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
                Priority = TimerPriority.TwoFiftyMS;

				m_Owner = owner;
                //TeleportTalk.Add("EAT SHIT AND DIE!");
                TeleportTalk.Add("IT'S TIME TO KICK ASS AND CHEW BUBBLE GUM. AND I'M ALL OUTTA GUM!");
                //TeleportTalk.Add("I'LL RIP OFF YOUR HEAD AND SHIT DOWN YOUR NECK!");
                TeleportTalk.Add("DAMN I'M GOOD!");
                TeleportTalk.Add("HAIL TO THE KING BABY");
                TeleportTalk.Add("SOMETIMES I EVEN AMAZE MYSELF!");
                TeleportTalk.Add("I CAN DO THIS ALL DAY!");
                TeleportTalk.Add("I'M NOT GONNA FIGHT YOU. I'M GONNA KICK YOUR ASS!");
                TeleportTalk.Add("YEAH, PIECE OF CAKE!");
                TeleportTalk.Add("WHO WANTS SOME?");
                TeleportTalk.Add("I'VE GOT BALLS OF STEEL!");
                TeleportTalk.Add("COME GET SOME!");
                TeleportTalk.Add("YOUR FACE, YOUR ASS. WHAT'S THE DIFFERENCE?");
                TeleportTalk.Add("SEE YOU IN HELL!");
                TeleportTalk.Add("THERE'S ONLY TWO WAYS THIS CAN END. AND IN BOTH OF THEM, YOU DIE!");
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.25 < Utility.RandomDouble() )
					return;

				Mobile toTeleport = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 16 ) )
				{
					if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					int offset = Utility.Random( 8 ) * 2;

					Point3D to = m_Owner.Location;

					for ( int i = 0; i < m_Offsets.Length; i += 2 )
					{
						int x = m_Owner.X + m_Offsets[(offset + i) % m_Offsets.Length];
						int y = m_Owner.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

						if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
						{
							to = new Point3D( x, y, m_Owner.Z );
							break;
						}
						else
						{
							int z = map.GetAverageZ( x, y );

							if ( map.CanSpawnMobile( x, y, z ) )
							{
								to = new Point3D( x, y, z );
								break;
							}
						}
					}

					Mobile m = toTeleport;

					Point3D from = m.Location;

					m.Location = to;

                    m_Owner.Say(TeleportTalk[TeleportTalkIndex++]);
                    if(TeleportTalkIndex >= TeleportTalk.Count)
                        TeleportTalkIndex = 0;

					SpellHelper.Turn( m_Owner, toTeleport );
					SpellHelper.Turn( toTeleport, m_Owner );

					m.ProcessDelta();

                    

					Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create(   to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

					m.PlaySound( 0x1FE );

					m_Owner.Combatant = toTeleport;
				}
			}
		}

        private class TransformTimer : Timer
        {
            private Harrower m_Harrower;
            private int m_OriginalZ;
            private int m_Count = 0;

            public TransformTimer(Harrower harrower)
                : base(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(5000))
            {
                m_Harrower = harrower;
                m_OriginalZ = m_Harrower.Z;
                m_Harrower.Z -= 110;
            }

            protected override void OnTick()
            {
                
                if (m_Harrower.Z >= m_OriginalZ)
                {
                    m_Harrower.Z = m_OriginalZ;
                    m_Harrower.ContinueMorph();
                    if (++m_Count == 5)
                    {
                        Stop();
                    }
                    else
                    {
                        DoTehEffects();
                    }
                }
                else
                {
                    DoTehEffects();
                    m_Harrower.Z += 2;
                }
                base.OnTick();
            }

            private void DoTehEffects()
            {
                int x;
                int y;
                int zdiff = m_OriginalZ - m_Harrower.Z;
                int amount;
                if (zdiff < 30)
                {
                    m_Harrower.Hidden = false;
                    Interval = TimeSpan.FromMilliseconds(400);
                    amount = 20;
                }
                else if (zdiff < 80)
                {
                    amount = 15;
                    Interval = TimeSpan.FromMilliseconds(800);
                }
                else if (zdiff < 100)
                {
                    amount = 10;
                    Interval = TimeSpan.FromMilliseconds(2000);
                }
                else
                {
                    amount = 3;
                }
                for (int i = 0; i < amount; ++i)
                {
                    if (m_OriginalZ - m_Harrower.Z <= 30)
                    {
                        x = Utility.Random(-7, 14);
                        y = Utility.Random(-7, 14);
                    }
                    else if (m_OriginalZ - m_Harrower.Z < 80 && m_OriginalZ - m_Harrower.Z > 30)
                    {
                        x = Utility.Random(-15, 30);
                        y = Utility.Random(-15, 30);
                    }
                    else
                    {
                        x = Utility.Random(-20, 40);
                        y = Utility.Random(-20, 40);
                    }

                    int z = m_Harrower.Map.Tiles.GetLandTile(m_Harrower.X + x, m_Harrower.Y).Z;
                    Point3D loc = new Point3D(m_Harrower.X + x, m_Harrower.Y + y, z);
                    if (m_Harrower.Map.CanFit(loc, 1))
                    {
                        if (zdiff > 100)
                        {
                            Orc orc = new Orc();
                            //orc.Hidden = true;
                            orc.Frozen = true;
                            orc.MoveToWorld(loc, m_Harrower.Map);
                            orc.BoltEffect(0);
                            orc.Delete();
                        }
                        else
                        {

                            switch (Utility.Random(4))
                            {
                                case 0: // Fire column
                                    {
                                        Effects.SendLocationParticles(EffectItem.Create(loc, m_Harrower.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                                        Effects.PlaySound(loc, m_Harrower.Map, 0x208);

                                        break;
                                    }
                                case 1: // Explosion
                                    {
                                        Effects.SendLocationParticles(EffectItem.Create(loc, m_Harrower.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                                        Effects.PlaySound(loc, m_Harrower.Map, 0x307);

                                        break;
                                    }
                                case 2: // Ball of fire
                                    {
                                        Effects.SendLocationParticles(EffectItem.Create(loc, m_Harrower.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);

                                        break;
                                    }
                                case 3: // Lightning
                                    {
                                        Orc orc = new Orc();
                                        //orc.Hidden = true;
                                        orc.Frozen = true;
                                        orc.MoveToWorld(loc, m_Harrower.Map);
                                        orc.BoltEffect(0);
                                        orc.Delete();
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

		private class GoodiesTimer : Timer
		{
			private readonly Map m_Map;
			private readonly int m_X;
		    private readonly int m_Y;

		    public GoodiesTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 10.0 ) )
			{
                Priority = TimerPriority.TwoFiftyMS;

				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Gold g = new Gold( 250, 750 );
				
				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				if ( 0.5 >= Utility.RandomDouble() )
				{
					switch ( Utility.Random( 3 ) )
					{
						case 0: // Fire column
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
							Effects.PlaySound( g, g.Map, 0x208 );

							break;
						}
						case 1: // Explosion
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
							Effects.PlaySound( g, g.Map, 0x307 );

							break;
						}
						case 2: // Ball of fire
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );

							break;
						}
					}
				}
			}
		}
	}
}
