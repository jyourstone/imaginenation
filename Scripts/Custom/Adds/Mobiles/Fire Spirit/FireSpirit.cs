using Server;
using Server.Items;
using Server.Network;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("fire spirit corpse")]
    public class FireSpirit : BaseCreature
    {
        private bool m_LavaBurst; 
        public bool LavaBurst { get { return m_LavaBurst; } }

		[Constructable]
        public FireSpirit() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
		{
            Name = "Fire Spirit"; 
            Body = 15;
            Hue = 0; 
            //BaseSoundID = ; 

			SetStr( 600, 620 );
			SetDex( 240, 260 );
			SetInt( 900, 920 );

			SetHits( 9000, 9500 );
			SetStam( 2222, 2333 );
			SetMana( 5200, 5555 );

			SetDamage( 4, 95 );

			SetResistance( ResistanceType.Physical, 85 );
			SetResistance( ResistanceType.Fire, 56 );
			SetResistance( ResistanceType.Cold, 44 );
			SetResistance( ResistanceType.Poison, 54);
			SetResistance( ResistanceType.Energy, 47 );

			SetSkill( SkillName.Wrestling, 200.0, 225.0);
			SetSkill( SkillName.Tactics, 160.0, 190.0 );
			SetSkill( SkillName.MagicResist, 200.0, 220.0 );
            SetSkill(SkillName.Magery, 200.0, 220.0);
            SetSkill(SkillName.Anatomy, 145.5, 190.0); 
            SetSkill(SkillName.Healing, 90.0, 110.0);
			SetSkill( SkillName.EvalInt, 201.5, 220.0 );
            SetSkill(SkillName.Meditation, 205.0, 220.0); 
            
            Fame = 22000;
            Karma = -18000; 
        

            VirtualArmor = 128;
        }
         
        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool Uncalmable { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }

		public FireSpirit( Serial serial ) : base( serial )
		{
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.BleedAttack;
        }

        public override void OnHitsChange(int oldValue)
        {

            if (Map != null && 0.05 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new SpiritMinion();

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(8) - 1;
                    int y = Y + Utility.Random(8) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation == map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation == map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true,"In Flam Kal Xen");
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(new Point3D(X, Y, Z), Map, 0x3709, 30);
            }
            else if (Map != null && 0.04 > Utility.RandomDouble())
            {
                Map map = Map;

                if (map == null)
                    return;
                BaseCreature spawn = new FireElemental();

                spawn.Team = Team;
                bool validLocation = false;
                Point3D loc = Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(8) - 1;
                    int y = Y + Utility.Random(8) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation == map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation == map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Kal Vas Xen Flam");
                spawn.MoveToWorld(loc, map);
                Effects.SendLocationEffect(new Point3D(X, Y, Z), Map, 0x3709, 30);
            }

            base.OnHitsChange(oldValue);
        }

        //TODO: Fix lava to rnd spawn within 3 tiles of target and/or mob
        //TODO: Make sure lava doesn't spawn on top of each other
        //TODO: Make fire spells heal a very small amount, no damage
        //TODO: Make firefields do damage

        public override void OnDamage(int amount, Mobile from, bool willKill) 
        {
            if (Hits < 3000 && Utility.RandomDouble() < 0.33)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fire Spirit Erupts! *");
                m_LavaBurst = true;
                SpillLava(TimeSpan.FromSeconds(10), 5, 20, from);
                Effects.SendLocationEffect(new Point3D(X, Y, Z), Map, 0x3709, 20);
                Effects.SendLocationEffect(new Point3D(X, Y, Z), Map, 0x36B0, 20);
                Effects.SendLocationEffect(new Point3D(X, Y, Z), Map, 0x36BF, 20);
            }
            else if (from != null && from != this && InRange(from, 5))
            {
                SpillLava(TimeSpan.FromSeconds(20), 10, 35, from);
            }
            base.OnDamage(amount, from, willKill);
        }

        #region Spill Lava
        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage)
        {
            SpillLava(duration, minDamage, maxDamage, null, 1, 1);
        }

        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage, Mobile target)
        {
            SpillLava(duration, minDamage, maxDamage, target, 1, 1);
        }

        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage, int count)
        {
            SpillLava(duration, minDamage, maxDamage, null, count, count);
        }

        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage, int minAmount, int maxAmount)
        {
            SpillLava(duration, minDamage, maxDamage, null, minAmount, maxAmount);
        }

        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage, Mobile target, int count)
        {
            SpillLava(duration, minDamage, maxDamage, target, count, count);
        }

        public void SpillLava(TimeSpan duration, int minDamage, int maxDamage, Mobile target, int minAmount, int maxAmount)
        {
            if ((target != null && target.Map == null) || Map == null)
                return;

            int pools = Utility.RandomMinMax(minAmount, maxAmount);

            for (int i = 0; i < pools; ++i)
            {
                PoolOfLava lava = new PoolOfLava(duration, minDamage, maxDamage);

                if (target != null && target.Map != null)
                {
                    lava.MoveToWorld(target.Location, target.Map);
                    continue;
                }

                bool validLocation = false;
                Point3D loc = Location;
                Map map = Map;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(4) - 1;
                    int y = Y + Utility.Random(4) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation == map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation == map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                lava.MoveToWorld(loc, map);
            }
        }
        #endregion
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 2 );
		}

        //private string m_Name;
        //private int m_Hue;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (Hits > 0.5 * HitsMax && Utility.RandomDouble() < 0.10)  //May need a timer
                FireRing();
        }

        private static int[] m_North = new int[]
		{
			-1, -1, 
			1, -1,
			-1, 2,
			1, 2
		};

        private static int[] m_East = new int[]
		{
			-1, 0,
			2, 0
		};
        #region Fire Ring
        public virtual void FireRing() //TODO: Make fields do 1-2 damage.
        {
            if (Utility.RandomDouble() >= 0.25)
            {
                for (int i = 0; i < m_North.Length; i += 2)
                {
                    Point3D p = Location;

                    p.X += m_North[i];
                    p.Y += m_North[i + 1];

                    IPoint3D po = p as IPoint3D;

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "In Vas Flam Grav");

                    Effects.SendLocationEffect(po, Map, 0x3E27, 50);
                    Effects.SendLocationEffect(new Point3D(X, Y + 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y + 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y + 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y + 5, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y + 6, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y + 7, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 1, Y - 1, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 2, Y - 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 3, Y - 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 4, Y - 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 5, Y - 5, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 2, Y + 1, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 2, Y + 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 3, Y + 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 4, Y + 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 5, Y + 5, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 1, Y + 1, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 2, Y + 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 3, Y + 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 4, Y + 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X - 5, Y + 5, Z), Map, 0x3E27, 130);
                }

                for (int i = 0; i < m_East.Length; i += 2)
                {
                    Point3D p = Location;

                    p.X += m_East[i];
                    p.Y += m_East[i + 1];

                    IPoint3D po = p as IPoint3D;

                    Effects.SendLocationEffect(po, Map, 0x3E31, 50);
                    Effects.SendLocationEffect(new Point3D(X - 2, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 3, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 4, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 5, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X - 6, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 7, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 3, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 4, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 5, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 6, Y, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 2, Y - 1, Z), Map, 0x3E31, 130);
                    Effects.SendLocationEffect(new Point3D(X + 2, Y - 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 3, Y - 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 4, Y - 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X + 5, Y - 5, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 1, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 2, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 3, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 4, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 5, Z), Map, 0x3E27, 130);
                    Effects.SendLocationEffect(new Point3D(X, Y - 6, Z), Map, 0x3E27, 130);
                }
            }
        }
        #endregion
        public override bool OnBeforeDeath()
        {
            SpillLava(TimeSpan.FromSeconds(10), 30, 30, 1, 4);

            return base.OnBeforeDeath();
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

        public override void Damage(int amount, Mobile from)
        {
            base.Damage(amount, from);

            if (Combatant == null || Hits > HitsMax * 0.2 || Utility.RandomBool())
                return;

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(Teleport));
        }
            
        public virtual void Teleport()
        {
            if (Combatant == null)
                return;

            // 10 tries to teleport
            for (int i = 0; i < 10; i++) 
            {
                int x = Utility.RandomMinMax(4, 6);
                int y = Utility.RandomMinMax(4, 6);
                int z = Z;

                if (Utility.RandomBool())
                    x *= -1;

                if (Utility.RandomBool())
                    y *= -1;

                Point3D from = this.Location;
                Point3D to = new Point3D(x, y, z);

                if (!InLOS(to))
                    continue;

                Location = to;
                //Point3D p = new Point3D(X + x, Y + y, 0);

                FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                PlaySound(0x1FE);

                //Location = p;
            }

            RevealingAction();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_LavaBurst);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        m_LavaBurst = reader.ReadBool();
                        break;
                    }
                case 0:
                    {
                        break;
                    } 

            }	
        }
    }
}