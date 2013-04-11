using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Custom.PvpToolkit.Gumps;
using Server.Custom.PvpToolkit.Items;
using Server.Items;
using Server.Mobiles;
using Server.Scripts.Custom.Adds.Items.DeathMatch;

namespace Server.Custom.PvpToolkit.DMatch.Items
{
    #region

    

    #endregion

    public class DMStone : BasePvpStone
    {
        #region Variables

        private bool m_AcceptingContestants;
        private bool m_Active;
        private List<Mobile> m_Contestants;
        private List<DMSpawnPoint> m_DMSpawnPoints;
        private Point3D m_LeaveLocation;
        private Map m_LeaveMap;
        private int m_MatchMin;
        private MatchTimer m_MatchTimer;
        private Dictionary<Serial, BaseCreature> m_MountCollection;
        private Dictionary<Serial, ScoreKeeper> m_ScoreTable;
        private Dictionary<Serial, DMSpawnPoint> m_LatestSpawnPoint;
        private Dictionary<Serial, ResurrectTimer> m_ResurrectionTimers;

        private bool m_Started;

        private EventSupplier m_EventSupplier;
        private bool m_UseSphereRules = true;
        private bool m_GiveHorses = true;

        private static string m_DeathMessage = "Type .DMRes to be resurrected or .LeaveDM to leave. You have {0} seconds.";

        #region Scoring
        public enum Scoring
        {
            Standard,
            KillBased
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Scoring ScoringType { get; set; }

        [CommandProperty( AccessLevel.GameMaster )]
        public double ScoreModifier { get; set; }

        #endregion

        #endregion

        #region Properties

        public List<Mobile> Contestants
        {
            get { return m_Contestants; }
            set { m_Contestants = value; }
        }

        public List<DMSpawnPoint> DMSpawnPoints
        {
            get { return m_DMSpawnPoints; }
            set { m_DMSpawnPoints = value; }
        }

        public Dictionary<Serial, ScoreKeeper> ScoreTable
        {
            get { return m_ScoreTable; }
            set { m_ScoreTable = value; }
        }

        public Dictionary<Serial, DMSpawnPoint> LatestSpawnPoint
        {
            get { return m_LatestSpawnPoint; }
            set { m_LatestSpawnPoint = value; }
        }

        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public int MatchMinutes
        {
            get { return m_MatchMin; }
            set { m_MatchMin = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map LeaveMap
        {
            get { return m_LeaveMap; }
            set { m_LeaveMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D LeaveLocation
        {
            get { return m_LeaveLocation; }
            set { m_LeaveLocation = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool AcceptingContestants
        {
            get { return m_AcceptingContestants; }
            set { m_AcceptingContestants = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EndlessMatches { get; set; }

        public Dictionary<Serial, BaseCreature> MountCollection
        {
            get { return m_MountCollection; }
            set { m_MountCollection = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool Started
        {
            get { return m_Started; }
            set
            {
                if (m_MatchTimer == null)
                {
                    m_MatchTimer = new MatchTimer(this, m_MatchMin);
                }

                if (DMSpawnPoints.Count < 1)
                {
                    CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 38, string.Format("[Deathmatch] Not started due to the lack of spawnpoints"));
                    m_AcceptingContestants = false;
                    m_Started = false;
                    return;
                }

                if (value)
                {
                    if (!m_MatchTimer.Running)
                    {
                        BeginTimer();
                        m_AcceptingContestants = true;
                        PvpCore.AddRunningDM();
                        CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, "A deathmatch has been started, type .joindm to join!");
                    }
                }
                else
                {
                    if (m_MatchTimer.Running)
                        EndDeathmatch(false);
                }

                m_Started = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventSupplier EventSupplier
        {
            get { return m_EventSupplier; }
            set { m_EventSupplier = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool UseSphereRules
        {
            get { return m_UseSphereRules; }
            set { m_UseSphereRules = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool GiveHorses
        {
            get { return m_GiveHorses; }
            set { m_GiveHorses = value; }
        }

        #endregion

        [Constructable]
        public DMStone()
        {
            Name = "Deathmatch Stone";
            m_MatchMin = 30;
            PvpCore.DMStones.Add(this);
            m_Contestants = new List<Mobile>();
            m_DMSpawnPoints = new List<DMSpawnPoint>();
            m_MountCollection = new Dictionary<Serial, BaseCreature>();
            m_ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            m_LatestSpawnPoint = new Dictionary<Serial, DMSpawnPoint>();
            m_ResurrectionTimers = new Dictionary<Serial, ResurrectTimer>();
            m_AcceptingContestants = false;
            m_Started = false;
            m_LeaveLocation = new Point3D(1495, 1629, 10);
            m_LeaveMap = Map.Felucca;

            ScoreModifier = 1.0;
        }

        public DMStone(Serial serial) : base(serial)
        {
            PvpCore.DMStones.Add(this);
            m_Contestants = new List<Mobile>();
            m_DMSpawnPoints = new List<DMSpawnPoint>();
            m_MountCollection = new Dictionary<Serial, BaseCreature>();
            m_ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            m_LatestSpawnPoint = new Dictionary<Serial, DMSpawnPoint>();
            m_ResurrectionTimers = new Dictionary<Serial, ResurrectTimer>();
        }

        public void RemovePlayer(Mobile m, bool kicked)
        {
            SupplySystem.RemoveEventGear(m);
            m.IsInEvent = false;
            m.Frozen = false;

            if (!m.Alive && m is PlayerMobile)
                ((PlayerMobile)m).ForceResurrect();

            m.MoveToWorld(m_LeaveLocation, m_LeaveMap);

            if (m_MountCollection.ContainsKey(m.Serial))
            {
                BaseCreature bc = m_MountCollection[m.Serial];
                if (bc != null)
                {
                    bc.ControlTarget = m;
                    bc.ControlOrder = OrderType.Stay;
                    bc.SetControlMaster(m);
                    bc.SummonMaster = m;
                    bc.IsStabled = false;
                    bc.MoveToWorld(m.Location, m.Map);
                    m.Stabled.Remove(bc);

                    m.Aggressed.Clear();
                    m.Aggressors.Clear();

                    m.Hits = m.HitsMax;
                    m.Stam = m.StamMax;
                    m.Mana = m.ManaMax;

                    m.DamageEntries.Clear();

                    m.Combatant = null;
                }
            }

            if (m_Contestants.Contains(m))
                m_Contestants.Remove(m);

            if (m.NetState != null)
            {
                if (kicked)
                    m.SendMessage(38, "You have been kicked from the deathmatch");
                else
                    m.SendMessage(38, "You have left the deathmatch");
            }

            DMHorse.TryRemoveHorse(m);

            if (m_ResurrectionTimers.ContainsKey(m.Serial))
            {
                Timer t = m_ResurrectionTimers[m.Serial];
                if (t != null)
                {
                    t.Stop();
                    m_ResurrectionTimers.Remove(m.Serial);
                }
            }

            //Only print the message if the match is onging
            if (!m_Started) 
                return;

            string leaveMessage = "";
            if (kicked)
                leaveMessage = string.Format("{0} has been kicked from a deathmatch", m.Name);
            else
                leaveMessage = string.Format("{0} has left a deathmatch", m.Name);
            PvpCore.SendMessage(Contestants, leaveMessage, true);
        }

        public override void AddPlayer(Mobile m)
        {
            if (m_Started && m_AcceptingContestants)
            {
                if (CheckPlayer(m))
                {
                    string joinMessage = string.Format("{0} has entered a deathmatch", m.Name);
                    PvpCore.SendMessage(Contestants, joinMessage, true);

                    ReadyPlayer(m);
                    m.SendMessage(38, "If you wish to leave at any time, please say \".LeaveDM\".");
                }
                else
                    m.SendMessage("You do not meet the requirements for this current match");
            }
            else
                m.SendMessage("This event is either closed or full, please try again later");
        }

        private bool CheckPlayer(Mobile m)
        {
            if (CheckSkills(m))
                if (CheckClass(m))
                    return true;

            return false;
        }

        private bool CheckClass(Mobile m)
        {
            if (m_UseSphereRules)
                return true;

            switch (ClassRulesRule)
            {
                case pClassRules.PureMageOnly:
                    {
                        if (m.Skills.Swords.Base > 0)
                            return false;
                        else if (m.Skills.Macing.Base > 0)
                            return false;
                        else if (m.Skills.Archery.Base > 0)
                            return false;
                        else if (m.Skills.Fencing.Base > 0)
                            return false;

                        return true;
                    }
                case pClassRules.TamerOnly:
                    {
                        if (m.Skills.AnimalLore.Base <= 25)
                            return false;
                        else if (m.Skills.AnimalTaming.Base <= 25)
                            return false;

                        return true;
                    }
                case pClassRules.TankMageOnly:
                    {
                        if (m.Skills.Inscribe.Base > 0)
                            return false;
                        else if (m.Skills.Poisoning.Base > 0)
                            return false;
                        else if (m.Skills.Anatomy.Base > 0)
                            return false;
                        else if (m.Skills.ArmsLore.Base > 0)
                            return false;
                        else if (m.Skills.Healing.Base > 0)
                            return false;

                        return true;
                    }
                case pClassRules.DexerOnly:
                    {
                        if (m.Skills.EvalInt.Base > 0)
                            return false;

                        return true;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        private bool CheckSkills(Mobile m)
        {
            if (m_UseSphereRules)
                return true;

            if (m.Skills.Total < MinSkill*10)
                return false;
            if (m.Skills.Total > MaxSkill*10)
                return false;
            return true;
        }

        private void ReadyPlayer(Mobile m)
        {
            if (!m_ScoreTable.ContainsKey(m.Serial))
                m_ScoreTable.Add(m.Serial, new ScoreKeeper(m));

            if (!m_UseSphereRules)
            {
                #region DistroCode

                bool MagicWeapons = MagicWeaponRule == pMagicWeaponRule.Allowed;
                bool MagicArmor = MagicArmorRule == pMagicArmorRule.Allowed;
                bool Potions = PotionRule == pPotionRule.Allowed;
                bool Bandages = BandageRule == pBandaidRule.Allowed;
                bool Pets = PetRule == pPetRule.Allowed;
                bool Mounts = MountRule == pMountRule.Allowed;

                if (!m.Alive)
                    m.Resurrect();

                Container bp = m.Backpack;
                Container bag = new Bag();
                bag.Hue = 38;
                BankBox bank = m.BankBox;
                Item oncurs = m.Holding;

                if (oncurs != null)
                    bp.DropItem(oncurs);

                m.CurePoison(m);

                m.Hits = m.HitsMax;
                m.Mana = m.ManaMax;
                m.Stam = m.StamMax;

                m.StatMods.Clear();

                List<Item> items = new List<Item>();

                foreach (Layer layer in PvpCore.EquipmentLayers)
                {
                    Item item = m.FindItemOnLayer(layer);

                    if (item != null)
                    {
                        if (item is BaseWeapon && !MagicWeapons)
                        {
                            BaseWeapon weapon = (BaseWeapon)item;

                            if (weapon.AccuracyLevel != WeaponAccuracyLevel.Regular)
                                items.Add(weapon);
                            else if (weapon.DamageLevel != WeaponDamageLevel.Regular)
                                items.Add(weapon);
                            else if (weapon.DurabilityLevel != WeaponDurabilityLevel.Regular)
                                items.Add(weapon);
                        }
                        else if (item is BaseArmor && !MagicArmor)
                        {
                            BaseArmor armor = (BaseArmor)item;

                            if (armor.Durability != ArmorDurabilityLevel.Regular)
                                items.Add(armor);
                            else if (armor.ProtectionLevel != ArmorProtectionLevel.Regular)
                                items.Add(armor);
                        }
                    }
                }

                if (m.Backpack != null)
                {
                    foreach (Item item in m.Backpack.Items)
                    {
                        if (item != null)
                        {
                            if (item is BaseWeapon && !MagicWeapons)
                            {
                                BaseWeapon weapon = (BaseWeapon) item;

                                if (weapon.AccuracyLevel != WeaponAccuracyLevel.Regular)
                                    items.Add(weapon);
                                else if (weapon.DamageLevel != WeaponDamageLevel.Regular)
                                    items.Add(weapon);
                                else if (weapon.DurabilityLevel != WeaponDurabilityLevel.Regular)
                                    items.Add(weapon);
                            }
                            else if (item is BaseArmor && !MagicArmor)
                            {
                                BaseArmor armor = (BaseArmor) item;

                                if (armor.Durability != ArmorDurabilityLevel.Regular)
                                    items.Add(armor);
                                else if (armor.ProtectionLevel != ArmorProtectionLevel.Regular)
                                    items.Add(armor);
                            }
                            else if (item is BasePotion && !Potions)
                                items.Add(item);
                            else if (item is EtherealMount && !Mounts)
                                items.Add(item);
                            else if (item is Bandage && !Bandages)
                                items.Add(item);
                        }
                    }
                }

                if (!Mounts)
                {
                    if (m.Mount != null)
                    {
                        IMount mount = m.Mount;
                        mount.Rider = null;
                        if (mount is BaseMount)
                        {
                            if (mount is BaseCreature)
                            {
                                BaseCreature bc = (BaseCreature)mount;
                                bc.ControlTarget = null;
                                bc.ControlOrder = OrderType.Stay;
                                bc.Internalize();

                                bc.SetControlMaster(null);
                                bc.SummonMaster = null;

                                bc.IsStabled = true;
                                m.Stabled.Add(bc);
                                MountCollection.Add(m.Serial, bc);
                                m.SendMessage(38, "Your mount has been moved to the your stables");
                            }
                        }
                    }
                }

                if (items.Count > 0)
                    m.SendMessage(38, "You had items that did not meet the requirements for the deathmatch and were thus moved to your bank.");

                foreach (Item item in items)
                    bag.AddItem(item);

                if (bag.Items.Count > 0)
                    bank.DropItem(bag);
                else
                    bag.Delete();

                #endregion
            }
            
            if (m_EventSupplier != null)
                m_EventSupplier.OnMoveOver(m);

            Contestants.Add(m);

            SpawnMobile(m);

            if ( m_GiveHorses )
                DMHorse.TryGiveHorse(m);

            if (m.NetState != null)
            {
                m.SendMessage(38, "You have joined a deathmatch");
                m.SendMessage(38, "You can check the score with \".DMScore\"");
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                if (!m_UseSphereRules)
                {
                    from.CloseGump(typeof (PvpKitRulesGump));
                    from.SendGump(new PvpKitRulesGump(this, "Deathmatch", PvpCore.DeathmatchVersion));
                }
                else
                    from.SendAsciiMessage("Staff members can not join. If you intended to change rules, please set UseSphereRules to false first.");
            }
            else
                PvpCore.TryJoinDM(from, this);
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Name);
            LabelTo(from, Started ? "Active" : "Not Active");
        }

        public void SpawnMobile(Mobile m)
        {
            if (DMSpawnPoints.Count > 0)
            {
                DMSpawnPoint spawnPoint;
                do
                {
                    spawnPoint = m_DMSpawnPoints[Utility.RandomMinMax(0, DMSpawnPoints.Count - 1)];
                    if (spawnPoint.Deleted || spawnPoint.StoneLink != this)
                        m_DMSpawnPoints.Remove(spawnPoint);
                    else
                    {
                        if (LatestSpawnPoint != null)
                        {
                            if (LatestSpawnPoint.ContainsKey(m.Serial))
                            {
                                DMSpawnPoint latestSpawn;
                                LatestSpawnPoint.TryGetValue(m.Serial, out latestSpawn);
                                LatestSpawnPoint[m.Serial] = spawnPoint;

                                if (m_DMSpawnPoints.Count > 1 && latestSpawn != null && spawnPoint == latestSpawn)
                                    continue;
                            }
                            else
                                LatestSpawnPoint.Add(m.Serial, spawnPoint);
                        }
                        else
                            LatestSpawnPoint = new Dictionary<Serial, DMSpawnPoint> {{m.Serial, spawnPoint}};

                        break;
                    }

                } while (m_DMSpawnPoints.Count > 0);

                if (m_DMSpawnPoints.Count != 0)
                    m.MoveToWorld(spawnPoint.Location, spawnPoint.Map);
                else
                {
                    PvpCore.SendMessage(Contestants, string.Format("Error: Cannot spawn player {0} to deathmatch, no spawn points exists.", m));
                    RemovePlayer(m, false);
                }
            }
            else
            {
                PvpCore.SendMessage(Contestants, string.Format("Error: Cannot spawn player {0} to deathmatch, no spawn points exists.", m));
                RemovePlayer(m, false);
            }
        }

        public void HandleDeath(Mobile m)
        {
            HandleCorpse(m);

            DMHorse.TryRemoveHorse(m);
            m.Frozen = true;
            var t = new ResurrectTimer(m, this);
            m_ResurrectionTimers[m.Serial] = t;
            t.Start();
                      
            if (m.LastKiller == m)
                m.LastKiller = null;

            if (m.LastKiller != null)
            {
                FixPlayer(m.LastKiller, false);
                m.LastKiller.SendMessage(38, "You have been rewarded health, mana, and stamina for your kill");
            }

            UpdateScores(m, m.LastKiller);

            m.LastKiller = null;

            new ResurrectMessageTimer(m).Start();
        }

        private void FixPlayer(Mobile m, bool removeMods)
        {
            if (m == null) 
                return;

            m.CurePoison(m);

            m.Mana = m.ManaMax;
            m.Stam = m.StamMax;

            //Dead player
            if (removeMods)
            {
                m.Hits = m.HitsMax;
                m.StatMods.Clear();
            }
            else
                m.Hits = m.HitsMax;
        }

        private void UpdateScores(Mobile died, Mobile killer)
        {
          if ( m_ScoreTable == null)
                return;

            ScoreKeeper scoreKeeper;

            double pointValue = ScoreModifier;

            if (died != null && m_ScoreTable.TryGetValue(died.Serial, out scoreKeeper))
            {
                pointValue = ScoringType.Equals(Scoring.Standard) ? ScoreModifier : (ScoreModifier * scoreKeeper.Kills) + 1;

                if (pointValue == 0.0)
                {
                    pointValue = ScoreModifier;
                }
                scoreKeeper.Deaths++;

                if (killer == null) //Minus points if suicide
                {
                    scoreKeeper.Points -= pointValue;
                    if (scoreKeeper.Points < 0.0)
                    {
                        scoreKeeper.Points = 0.0;
                    }
                }
            }

            if (killer != null && m_ScoreTable.TryGetValue(killer.Serial, out scoreKeeper))
            {
                scoreKeeper.Kills++;
                scoreKeeper.Points += pointValue;
            }
        }

        public void HandleCorpse(Mobile from)
        {
            if (from.Corpse != null)
            {
                if (m_EventSupplier != null && m_EventSupplier.NewbieAllItems)
                {
                    from.Corpse.Delete();
                    return;
                }

                Corpse c = (Corpse)from.Corpse;
                c.Open(from, true);
                c.Delete();
                from.SendMessage(38, "The contents of your corpse have been safely placed into your backpack");
            }
        }

        public void ShowScore(Mobile m)
        {
            m.SendGump(new DMScoreGump(m, new List<ScoreKeeper>(m_ScoreTable.Values)));
        }

        public void RemoveSpawnPoint(DMSpawnPoint spawnPoint)
        {
            if (m_DMSpawnPoints.Contains(spawnPoint))
                m_DMSpawnPoints.Remove(spawnPoint);
        }

        public void AddSpawnPoint(DMSpawnPoint spawnPoint)
        {
            if (!m_DMSpawnPoints.Contains(spawnPoint))
                m_DMSpawnPoints.Add(spawnPoint);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(3);
    
            //Version 3
            writer.Write((int)ScoringType);
            writer.Write(ScoreModifier);

            //Version 2
            writer.WriteItem(m_EventSupplier);
            writer.Write(m_UseSphereRules);
            writer.Write(m_GiveHorses);

            writer.Write(m_MatchMin);
            writer.Write(m_LeaveLocation);
            writer.Write(m_LeaveMap);
            writer.Write(m_Started);
            writer.Write(m_Active);
            writer.Write(m_AcceptingContestants);
            writer.WriteItemList(m_DMSpawnPoints);
            WriteMountCollection(m_MountCollection, writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            ScoreModifier = 1.0;

            switch (version)
            {
                case 3:
                    {
                        ScoringType = (Scoring) reader.ReadInt();
                        ScoreModifier = reader.ReadDouble();
                        goto case 2;
                    }
                case 2:
                    {
                        m_EventSupplier = reader.ReadItem<EventSupplier>();
                        m_UseSphereRules = reader.ReadBool();
                        m_GiveHorses = reader.ReadBool();
                        goto case 1;
                    }
                case 1:
                    {
                        m_MatchMin = reader.ReadInt();
                        m_LeaveLocation = reader.ReadPoint3D();
                        m_LeaveMap = reader.ReadMap();
                        goto case 0;
                    }
                case 0:
                    {
                        Started = reader.ReadBool();
                        m_Active = reader.ReadBool();
                        m_AcceptingContestants = reader.ReadBool();
                        m_DMSpawnPoints = reader.ReadStrongItemList<DMSpawnPoint>();

                        #region Verify loaded spawn points

                        List<DMSpawnPoint> removalList = new List<DMSpawnPoint>();
                        foreach (DMSpawnPoint dmSpawnPoint in m_DMSpawnPoints)
                            if (dmSpawnPoint.StoneLink != this)
                                removalList.Add(dmSpawnPoint);

                        foreach (DMSpawnPoint dmSpawnPoint in removalList)
                            m_DMSpawnPoints.Remove(dmSpawnPoint);

                        #endregion

                        m_MountCollection = ReadMountCollection(reader);
                        break;
                    }
            }

            m_Started = false;
            AcceptingContestants = false;
            Active = false;
        }

        public void BeginTimer()
        {
            m_MatchTimer = new MatchTimer(this, m_MatchMin);
            m_MatchTimer.Start();
        }

        public void EndDeathmatch(bool worldLoaded)
        {
            if (m_MatchTimer != null)
                m_MatchTimer.Stop();

            PvpCore.RemoveRunningDM();
            CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, "A deathmatch has ended");

            if (m_ScoreTable != null && m_ScoreTable.Count > 0)
            {
                List<ScoreKeeper> scoreKeeperList = new List<ScoreKeeper>(m_ScoreTable.Values);

                scoreKeeperList.Sort();

                if (scoreKeeperList[0] != null && worldLoaded)
                {
                    CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, String.Format("{0} has won the deathmatch!", scoreKeeperList[0].Player.Name));
                    RewardPlayers(scoreKeeperList);
                }
            }

            if (Contestants != null && Contestants.Count > 0)
            {
                List<Mobile> removeList = new List<Mobile>(Contestants);
                foreach (Mobile m in removeList)
                {
                    ShowScore(m);
                    RemovePlayer(m, false);
                }
            }

            m_Started = false;
            m_AcceptingContestants = false;

            m_ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            m_LatestSpawnPoint = new Dictionary<Serial, DMSpawnPoint>();
            Contestants = new List<Mobile>();

            if (EndlessMatches)
                Started = true;
        }

        private static void RewardPlayers(List<ScoreKeeper> scoreKeeperList)
        {
            Mobile m;
            int activePlayers = 0;
            int gold;

            for (int i = 0; i < scoreKeeperList.Count; i++)
            {
                if (scoreKeeperList[i].Player == null || !(scoreKeeperList[i].Player is PlayerMobile))
                    return;

                m = scoreKeeperList[i].Player;
                
                if (!m.IsInEvent) //Don't count players that left
                    continue;
                    
                activePlayers++;

                //Get 1k per kill...
                gold = scoreKeeperList[i].Kills*1000;

                //...but max 5k
                if (gold > 5000)
                    gold = 5000;

                if (gold > 0)
                {
                    m.AddToBackpack(new Gold(gold));
                    m.SendAsciiMessage("You have been rewarded with {0} gold for your {1} kill{2}", gold, scoreKeeperList[i].Kills, scoreKeeperList[i].Kills != 1 ? "s" : "");
                }
            }

            //Rewards for 5 or more active participants
            if (activePlayers >= 5)
            {
                if (scoreKeeperList[0].Player != null && (scoreKeeperList[0].Player is PlayerMobile))
                {
                    Mobile winner = scoreKeeperList[0].Player;
                    //Winner gets 1k extra per participant
                    if (winner.IsInEvent)
                    {
                        gold = activePlayers*1000;
                        winner.AddToBackpack(new Gold(gold));
                        winner.SendAsciiMessage("You won the deathmatch and get an additional {0} gold", gold);
                    }
                }
            }

            //Runner up gets 500gp extra per participant if 8 or more active participants
            if (activePlayers >= 8)
            {
                if (scoreKeeperList[1].Player != null && (scoreKeeperList[1].Player is PlayerMobile))
                {
                    Mobile second = scoreKeeperList[1].Player;
                    if (second.IsInEvent)
                    {
                        gold = activePlayers*500;
                        second.AddToBackpack(new Gold(gold));
                        second.SendAsciiMessage("Due to the high participation you get an additional {0} gold for finishing second", gold);
                    }
                }
            }

            //Rewards for 10 or more active participants
            if (activePlayers >= 10)
            {
                if (scoreKeeperList[0].Player != null && (scoreKeeperList[0].Player is PlayerMobile))
                {
                    Mobile winner = scoreKeeperList[0].Player;
                    if (winner.IsInEvent)
                    {
                        winner.AddToBackpack(new ImagineNickel(activePlayers));
                        winner.SendAsciiMessage("Due to the high participation you also get {0} imagine nickels", activePlayers);
                    }
                }
            }
        }

        #region Nested type: MatchTimer

        public class MatchTimer : Timer
        {
            private readonly DMStone m_DMStone;
            private int count;

            public MatchTimer(DMStone stone, int mincount)
                : base(TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(1.0))
            {
                m_DMStone = stone;
                count = mincount*60;
            }

            protected override void OnTick()
            {
                if (count <= 0)
                {
                    m_DMStone.EndDeathmatch(true);
                    Stop();
                }

                if (count%300 == 0)
                {
                    string message = string.Format("{0} minutes remaining...", (count / 60));
                    foreach (Mobile m in m_DMStone.Contestants)
                        m.SendMessage(38, message);
                }

                count--;
            }
        }

        #endregion


        #region Nested type: ResurrectTimer

        public class ResurrectTimer : Timer
        {
            private readonly DMStone m_DMStone;
            private readonly Mobile m_From;

            public ResurrectTimer(Mobile m, DMStone stone) : base(TimeSpan.FromSeconds(60.0))
            {
                m_From = m;
                m_DMStone = stone;
            }

            protected override void OnTick()
            {
                if (m_DMStone != null && m_DMStone.m_Started && PvpCore.IsInDeathmatch(m_From))
                {
                    m_From.Frozen = false;
                    m_From.Resurrect();
                    m_DMStone.RemovePlayer(m_From, true);
                }
            }
        }

        #endregion

        public class ResurrectMessageTimer : Timer
        {
            private readonly Mobile m_From;
            private int m_Count = 6;

            public ResurrectMessageTimer(Mobile m) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(10.0))
            {
                m_From = m;
            }

            protected override void OnTick()
            {
                if (m_From.Alive || !PvpCore.IsInDeathmatch(m_From))
                    Stop();
                else if (m_Count <= 0)
                    Stop();
                else
                {
                    m_From.SendMessage(String.Format(m_DeathMessage, m_Count * 10));
                    m_Count--;
                }
            }
        }

        internal void doResurrection(Mobile from)
        {
            from.Frozen = false;
            if (m_ResurrectionTimers.ContainsKey(from.Serial))
            {
                m_ResurrectionTimers[from.Serial].Stop();
                m_ResurrectionTimers.Remove(from.Serial);
            }
            from.Resurrect();
            SpawnMobile(from);
            FixPlayer(from, true);
            DMHorse.TryGiveHorse(from);
        }
    }

    #region dmres command

    public class DMRes
    {
        public static void Initialize() 
        {
            CommandSystem.Register("DMRes", AccessLevel.Player, Execute);
        }

        [Usage("DMRes")]
        [Description("Triggers Resurrection while in a DeathMatch")]
        private static void Execute(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            DMStone stone = PvpCore.GetPlayerStone(e.Mobile);
            if (stone != null && stone.Started && PvpCore.IsInDeathmatch(from) && !from.Alive)
            {
                stone.doResurrection(from);           
            }
        }
    }
    #endregion
}