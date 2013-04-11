using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sheep corpse")]
    public class Sheep : BaseCreature, ICarvable
    {
        private DateTime m_NextWoolTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextWoolTime
        {
            get { return m_NextWoolTime; }
            set { m_NextWoolTime = value; Body = (DateTime.Now >= m_NextWoolTime) ? 0xCF : 0xDF; }
        }

        public void Carve(Mobile from, Item item)
        {
            if (from.BeginAction(typeof(IAction)))
            {
                bool releaseLock;

                if (Summoned)
                {
                    from.SendAsciiMessage("You cannot sheer summoned sheep.");
                    return;
                }

                if (Utility.Random(1000) <= 2)
                    AntiMacro.AntiMacroGump.SendGumpThreaded((PlayerMobile)from);

                if (DateTime.Now < m_NextWoolTime)
                {
                    //This sheep is not yet ready to be shorn.
                    PrivateOverheadMessage(MessageType.Regular, 0x3b2, 500449, from.NetState);
                    releaseLock = true;
                    return;
                }

                from.SendAsciiMessage("You begin sheering the sheep...");
                new InternalTimer(from, this).Start();
                releaseLock = false;

                if (releaseLock && from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");
        }

        public override void OnThink()
        {
            base.OnThink();
            Body = (DateTime.Now >= m_NextWoolTime) ? 0xCF : 0xDF;
        }

        [Constructable]
        public Sheep()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            //Name = "Sheep";
            switch (Utility.Random(5))
            {
                case 0: Name = "Dorset"; break;
                case 1: Name = "Drysdale"; break;
                case 2: Name = "Radnor"; break;
                case 3: Name = "Ewe"; break;
                case 4: Name = "Sheep"; break;
            }
            Body = 0xCF;
            BaseSoundID = 0xD6;

            SetStr(19);
            SetDex(25);
            SetInt(5);

            SetHits(12);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 6.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 300;

            VirtualArmor = 6;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 11.1;
        }

        public override int Meat { get { return 3; } }
        public override MeatType MeatType { get { return MeatType.LambLeg; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override int Wool { get { return (Body == 0xCF ? 2 : 0); } }

        public Sheep(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.WriteDeltaTime(m_NextWoolTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        NextWoolTime = reader.ReadDeltaTime();
                        break;
                    }
            }
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly Sheep m_Sheep;

            public InternalTimer(Mobile from, Sheep sheep) : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Sheep = sheep;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                m_From.SendLocalizedMessage(500452);
                m_From.AddToBackpack(new Wool(m_From.Map == Map.Felucca ? 2 : 1));
                
                m_Sheep.NextWoolTime = DateTime.Now + TimeSpan.FromHours(3.0); // TODO: Proper time delay

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You fail to produce any wool.", from.NetState);

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
    }
}