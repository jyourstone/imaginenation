using System;
using System.Collections.Generic;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public abstract class BaseChampion : BaseCreature
    {
        public BaseChampion(AIType aiType)
            : this(aiType, FightMode.Closest)
        {
        }

        public BaseChampion(AIType aiType, FightMode mode)
            : base(aiType, mode, 18, 1, 0.1, 0.2)
        {
        }

        public BaseChampion(Serial serial)
            : base(serial)
        {
        }

        public abstract ChampionSkullType SkullType { get; }

        public abstract Type[] UniqueList { get; }
        public abstract Type[] SharedList { get; }
        public abstract Type[] DecorativeList { get; }
        public abstract MonsterStatuetteType[] StatueTypes { get; }

        public virtual bool NoGoodies { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public Item GetArtifact()
        {
            double random = Utility.RandomDouble();
            if (0.05 >= random)
                return CreateArtifact(UniqueList);
            else if (0.15 >= random)
                return CreateArtifact(SharedList);
            else if (0.30 >= random)
                return CreateArtifact(DecorativeList);
            return null;
        }

        public Item CreateArtifact(Type[] list)
        {
            if (list.Length == 0)
                return null;

            int random = Utility.Random(list.Length);

            Type type = list[random];

            Item artifact = Loot.Construct(type);

            if (artifact is MonsterStatuette && StatueTypes.Length > 0)
            {
                ((MonsterStatuette)artifact).Type = StatueTypes[Utility.Random(StatueTypes.Length)];
                ((MonsterStatuette)artifact).LootType = LootType.Regular;
            }

            return artifact;
        }

        private static PowerScroll CreateRandomPowerScroll()
        {
            int level;
            double random = Utility.RandomDouble();

            if (0.05 >= random)
                level = 20;
            else if (0.4 >= random)
                level = 15;
            else
                level = 10;

            return PowerScroll.CreateRandomNoCraft(level, level);
        }

        public void GivePowerScrolls()
        {
            if (Map != Map.Felucca)
                return;

            List<Mobile> toGive = new List<Mobile>();
            List<DamageStore> rights = GetLootingRights(DamageEntries, HitsMax);

            for (int i = rights.Count - 1; i >= 0; --i)
            {
                DamageStore ds = rights[i];

                if (ds.m_HasRight)
                    toGive.Add(ds.m_Mobile);
            }

            if (toGive.Count == 0)
                return;

            for (int i = 0; i < toGive.Count; i++)
            {
                Mobile m = toGive[i];

                if (!(m is PlayerMobile))
                    continue;

                bool gainedPath = false;

                int pointsToGain = 800;

                if (VirtueHelper.Award(m, VirtueName.Valor, pointsToGain, ref gainedPath))
                {
                    if (gainedPath)
                        m.SendLocalizedMessage(1054032); // You have gained a path in Valor!
                    else
                        m.SendLocalizedMessage(1054030); // You have gained in Valor!

                    //No delay on Valor gains
                }
            }

            // Randomize
            for (int i = 0; i < toGive.Count; ++i)
            {
                int rand = Utility.Random(toGive.Count);
                Mobile hold = toGive[i];
                toGive[i] = toGive[rand];
                toGive[rand] = hold;
            }

            for (int i = 0; i < 6; ++i)
            {
                Mobile m = toGive[i % toGive.Count];

                PowerScroll ps = CreateRandomPowerScroll();

                GivePowerScrollTo(m, ps);
            }
        }

        public static void GivePowerScrollTo(Mobile m, PowerScroll ps)
        {
            if (ps == null || m == null)	//sanity
                return;

            m.SendLocalizedMessage(1049524); // You have received a scroll of power!

            if (!Core.SE || m.Alive)
                m.AddToBackpack(ps);
            else
            {
                if (m.Corpse != null && !m.Corpse.Deleted)
                    m.Corpse.DropItem(ps);
                else
                    m.AddToBackpack(ps);
            }

            if (m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                for (int j = 0; j < pm.JusticeProtectors.Count; ++j)
                {
                    Mobile prot = pm.JusticeProtectors[j];

                    if (prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion(m, prot))
                        continue;

                    int chance = 0;

                    switch (VirtueHelper.GetLevel(prot, VirtueName.Justice))
                    {
                        case VirtueLevel.Seeker: chance = 60; break;
                        case VirtueLevel.Follower: chance = 80; break;
                        case VirtueLevel.Knight: chance = 100; break;
                    }

                    if (chance > Utility.Random(100))
                    {
                        PowerScroll powerScroll = new PowerScroll(ps.Skill, ps.Value);

                        prot.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!

                        if (!Core.SE || prot.Alive)
                            prot.AddToBackpack(powerScroll);
                        else
                        {
                            if (prot.Corpse != null && !prot.Corpse.Deleted)
                                prot.Corpse.DropItem(powerScroll);
                            else
                                prot.AddToBackpack(powerScroll);
                        }
                    }
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            if (!NoKillAwards)
            {
                //GivePowerScrolls();

                if (NoGoodies)
                    return base.OnBeforeDeath();

                Map map = Map;

                if (map != null)
                {
                    for (int x = -12; x <= 12; ++x)
                    {
                        for (int y = -12; y <= 12; ++y)
                        {
                            double dist = Math.Sqrt(x * x + y * y);

                            if (dist <= 12)
                                new GoodiesTimer(map, X + x, Y + y).Start();
                        }
                    }
                }
            }

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

                    m.SendAsciiMessage("You dealt {0} damage to the champion and got {1} gold", amountofgold/10, amountofgold);
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
                    m.SendAsciiMessage("You dealt more than 1000 damage to the champion, your reward is well deserved!");
                }
            }

            //Remove all monsters within 20 tiles when the champion is killed
            foreach (Mobile m in Map.GetMobilesInRange(Location, 20))
            {   
                if (m is BaseCreature && !(m is BaseChampion))
                {
                    BaseCreature bc = (BaseCreature)m;
                    if ( !(m is BaseMount) && m.Spawner == null && bc.LastOwner == null && !bc.Controlled )
                        toRemove.Add(m);
                }
            }

            foreach (Mobile m in toRemove)
            {
                if (m != null)
                    m.Delete();
            }
            #endregion

            /*if (toGive.Count > 0)
                toGive[Utility.Random(toGive.Count)].AddToBackpack(new ChampionSkull(SkullType));
            else
                c.DropItem(new ChampionSkull(SkullType));*/

        base.OnDeath(c);
        }

        private class GoodiesTimer : Timer
        {
            private readonly Map m_Map;
            private readonly int m_X;
            private readonly int m_Y;

            public GoodiesTimer(Map map, int x, int y)
                : base(TimeSpan.FromSeconds(Utility.RandomDouble() * 10.0))
            {
                m_Map = map;
                m_X = x;
                m_Y = y;
            }

            protected override void OnTick()
            {
                int z = m_Map.GetAverageZ(m_X, m_Y);
                bool canFit = m_Map.CanFit(m_X, m_Y, z, 6, false, false);

                for (int i = -3; !canFit && i <= 3; ++i)
                {
                    canFit = m_Map.CanFit(m_X, m_Y, z + i, 6, false, false);

                    if (canFit)
                        z += i;
                }

                if (!canFit)
                    return;

                Gold g = new Gold(150, 250);

                g.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);

                if (0.5 >= Utility.RandomDouble())
                {
                    switch (Utility.Random(3))
                    {
                        case 0: // Fire column
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                                Effects.PlaySound(g, g.Map, 0x208);

                                break;
                            }
                        case 1: // Explosion
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                                Effects.PlaySound(g, g.Map, 0x307);

                                break;
                            }
                        case 2: // Ball of fire
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);

                                break;
                            }
                    }
                }
            }
        }
    }
}