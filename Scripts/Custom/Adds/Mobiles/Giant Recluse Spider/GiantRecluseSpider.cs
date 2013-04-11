using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "Recluse Spider's corpse" )]
	public class RecluseSpider : BaseCreature
	{
		[Constructable]
		public RecluseSpider() : base( AIType.AI_Mage, FightMode.Strongest, 10, 1, 0.2, 0.3 )
		{
			Name = "Giant Recluse Spider";
			Body = 173;
			BaseSoundID = 387;
			Hue = 2949;

			SetStr( 350, 450 );
			SetDex( 155, 200 );
			SetInt( 2880, 3000 );

			SetHits( 15000, 20000 );

			SetDamage( 19, 55 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 300 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 190, 200 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.EvalInt, 90.1, 200.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.Meditation, 95.1, 120.0 );
			SetSkill( SkillName.MagicResist, 55.1, 85.0 );
			SetSkill( SkillName.Tactics, 55.1, 70.0 );
			SetSkill( SkillName.Wrestling, 70.1, 110.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;

			PackItem( new SpidersSilk( 150 ) );
						
		}

		public override void GenerateLoot()
		{
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.SuperBoss);
			AddLoot( LootPack.UltraRich );
            PackGold(2000, 4000);
            PackGold(1000, 3000);
            PackGold(2000, 5000);
		}
	
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool BardImmune{ get{ return true; } }

		public static void PlaceItemIn( Container parent, int x, int y, Item item )
		{
			parent.AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}

		public virtual void CreateBones_Callback( object state )
		{
			Mobile from = (Mobile)state;
			Map map = from.Map;

			if ( map == null )
				return;

			int count = Utility.RandomMinMax( 1, 3 );

			for ( int i = 0; i < count; ++i )
			{
				int x = from.X + Utility.RandomMinMax( -1, 1 );
				int y = from.Y + Utility.RandomMinMax( -1, 1 );
				int z = from.Z;

				if ( !map.CanFit( x, y, z, 16, false, true ) )
				{
					z = map.GetAverageZ( x, y );

					if ( z == from.Z || !map.CanFit( x, y, z, 16, false, true ) )
						continue;
				}

				RecluseSpiderEggSac sac = new RecluseSpiderEggSac();

				sac.Hue = 0;
				sac.Name = "egg sac";
				sac.ItemID = 4313;

				sac.MoveToWorld( new Point3D( x, y, z ), map );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			BaseCreature c = defender as BaseCreature;

			if ( c is BaseCreature )
			{
				if ( 0.8 >= Utility.RandomDouble() )
					DoSpecial( defender );
			}
			else
			{
				DebugSay( "test" );
			}
		}

		public void DoSpecial( Mobile target )
		{
			AddWebCase( target );
		}

		public void AddWebCase( Mobile m )
		{
			new WebCase().MoveToWorld( m.Location, m.Map );
			PublicOverheadMessage( MessageType.Emote, 0x47E, true, "*Your pets are nothing but food for me!*" );
			m.PlaySound( 922 );
			m.Kill();
		}

        public override void OnDeath(Container c)
        {

            #region Taran: Reward all attackers
            List<DamageEntry> rights = DamageEntries;
            List<Mobile> toGiveGold = new List<Mobile>();
            List<Mobile> toGiveItem = new List<Mobile>();
            List<Mobile> toRemove = new List<Mobile>();
            List<int> GoldToRecieve = new List<int>();

            for (int i = 0; i < rights.Count; ++i)
            {
                DamageEntry de = rights[i];

                //Only players get rewarded
                if (de.HasExpired || !de.Damager.Player)
                {
                    DamageEntries.RemoveAt(i);
                    continue;
                }

                toGiveGold.Add(de.Damager);
                GoldToRecieve.Add(de.DamageGiven * 5); //Player gets 5 times the damage dealt in gold

                if (de.DamageGiven > 700) //Players doing more than 700 damage gets a random weapon or armor
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

                    m.SendAsciiMessage("You dealt {0} damage to the monster and got {1} gold", amountofgold / 5, amountofgold);
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
                    m.SendAsciiMessage("You dealt more than 700 damage to the Fire Spirit, your reward is well deserved!");
                }
            }
            #endregion

            List<Mobile> explosionDamage = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 5))
            {
                if (!(m is FireSpirit))
                    explosionDamage.Add(m);
            }


            for (int i = 0; i < explosionDamage.Count; i++)
            {
                Mobile m = explosionDamage[i];
                m.Damage(5);
                m.FixedParticles(0x36BD, 20, 2, 5044, EffectLayer.Head);
            }

            base.OnDeath(c);
        }

		public RecluseSpider( Serial serial ) : base( serial )
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