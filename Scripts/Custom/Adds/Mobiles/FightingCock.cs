using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("Fighting cock corpse")]
    public class FightingCock : BaseCreature
    {
        [Constructable]
        public FightingCock()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Fighting Cock";
            Body = 0xD0;
            Hue = Utility.RandomList(1172, 0, 1325, 2544, 2222, 0, 1904, 0, 1160, 1946, 1944, 0, 2954, 0, 1501, 0, 1931, 0, 2496, 0, 1950, 0, 2504, 0, 2515, 0, 2516, 0, 2522, 0, 2529);
            BaseSoundID = 0x6E;

            SetStr(27, 37);
            SetDex(28, 43);
            SetInt(29, 37);

            SetHits(40, 50);  //Random number of hits so that taming one with more HP increases its 'value' as a fighter.
            SetMana(0);

            SetDamage(4, 9);  //Is this modified by stats and skill?  may need tweaking.

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 22.1, 47.0);
            SetSkill(SkillName.Tactics, 19.2, 31.0);
            SetSkill(SkillName.Wrestling, 19.2, 31.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 15;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 50;
        }

        public override void OnHitsChange(int oldValue)
        {
            base.OnHitsChange(oldValue);
			
			if (RawStr>100) RawStr=100;
			if (RawDex>100) RawDex=100;
						
            if (Hits <= 10 && SolidHueOverride != 38)  //if it drops below 10HP and is still not enraged (hue = 0) then enrage.
            {
                SolidHueOverride = 38;  //the color of rage
                RawStr = RawStr+7;
				RawDex = RawDex+10;
                VirtualArmor = 5;
                new CockEnrageTimer(this).Start();  //30 second timer before it calms down.

                string emote;

                if (Utility.RandomDouble() > 0.5)
                    emote = "*enrages*";
                else
                    emote = "*BIRD RAGE*";

                new CockEmoteTimer(this, emote).Start();
            }
        }

        public override bool OnBeforeDeath()  //if it dies, does a little Say and returns to normal color.
        {
            SolidHueOverride = -1;
            string emote;

            int DeathNoise = Utility.Random(5);

            if (DeathNoise == 1) emote = "BUCAAAWK!";
            else if (DeathNoise == 2) emote = "*collapses in a heap*";
            else if (DeathNoise == 3) emote = "*has given up the ghost*";
            else if (DeathNoise == 4) emote = "BUCAaaww...";
            else emote = "*has been overcome*";

            PublicOverheadMessage(MessageType.Emote, 0x22, false, emote);

            return base.OnBeforeDeath();
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }

        public override int Feathers { get { return 25; } }

        public FightingCock(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CockEnrageTimer : Timer  //30 second enrage timer.
    {
        private readonly Mobile mob;

        public CockEnrageTimer(Mobile m)
            : base(TimeSpan.FromSeconds(30))
        {
            mob = m;
            Priority = TimerPriority.FiveSeconds;
        }

        protected override void OnTick()
        {
            if (mob == null || mob.Deleted)
            {
                Stop();
                return;
            }
            
            //Return to normal stats and armor.
            mob.SolidHueOverride = -1;
            mob.RawStr = mob.RawStr - 7;
			if (mob.RawStr>=100) 
			{
				mob.RawStr = mob.RawStr=100;
			}
            mob.RawDex = mob.RawDex - 10;
			if (mob.RawDex>=100) 
			{
				mob.RawStr = mob.RawDex=100;
			}
            mob.VirtualArmor = 17;

            string emote = "*becomes calm*";

            new CockEmoteTimer(mob, emote).Start();
        }
    }

    public class CockEmoteTimer : Timer
    {
        private readonly Mobile m_Mob;
        private readonly string m_Emote;

        public CockEmoteTimer(Mobile m, string e) : base(TimeSpan.FromMilliseconds(100))
        {
            m_Mob = m;
            m_Emote = e;
            Priority = TimerPriority.TwentyFiveMS;
        }

        protected override void OnTick()
        {
            if (m_Mob == null || m_Mob.Deleted)
            {
                Stop();
                return;
            }
            m_Mob.PublicOverheadMessage(MessageType.Emote, 0x22, false, m_Emote );
        }
    }
}