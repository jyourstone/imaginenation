using Server;
using Server.Items;
using Server.Network;
using System;
using Server.Guilds;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a guards corpse")]
    public class BritishGuard : BaseCreature //BaseCreature
    {
        [Constructable]
        public BritishGuard()
            : base(AIType.AI_SphereMelee, FightMode.Closest, 10, 5, 0.2, 0.4)
        {
            Name = "British Guard";
            Body = 400;
            //Hue = 0x1;

            SetStr(200, 220);
            SetDex(150, 180);
            SetInt(100, 120);

            SetHits(250, 280);
            SetStam(150, 180);
            SetMana(400);
            SetDamage(8, 11);

            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Parry, 150.0, 180.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Fencing, 120.0, 130.0);
            SetSkill(SkillName.Swords, 120.0, 130.0);
            SetSkill(SkillName.Macing, 120.0, 130.0);
            SetSkill(SkillName.Archery, 100.0, 110.0);

            Fame = Utility.RandomMinMax(3500, 4000);
            Karma = Utility.RandomMinMax(4000, 6000);

            VirtualArmor = 80;

            PackGold(600, 750);

        }

        public BritishGuard(Serial serial)
            : base(serial)
        {
        }

        public override bool BardImmune
        {
            get { return true; }
        }

        public override bool ShowFameTitle
        {
            get { return false; }
        }

        public override bool ClickTitle
        {
            get { return false; }
        }

        public override void OnDeath(Container c)
        {

            #region Gives gold and Faction coin based on alignment

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
                GoldToRecieve.Add(de.DamageGiven*3); //Player gets 3 times the damage dealt in gold

                if (de.DamageGiven > 50) //Players doing more than 50 damage they get an order coin
                    toGiveItem.Add(de.Damager);
            }

            foreach (Mobile m in toGiveGold)
            {
                if (m is PlayerMobile)
                {
                    int amountofgold = GoldToRecieve[toGiveGold.IndexOf(m)];

                    if (amountofgold > 1000)
                        amountofgold = 1000; 

                    m.AddToBackpack(new Gold(amountofgold));
                }
            }

            foreach (Mobile m in toGiveItem)
            {
                ChaosCoin item = new ChaosCoin();

                if (m is PlayerMobile && m.Guild.Type == GuildType.Chaos)
                {
                    m.AddToBackpack(item);
                    m.SendAsciiMessage("You have been rewarded a Chaos Coin for fighting for Lord Blackthorn.");
                }
            }

            #endregion
        }

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
    }
}