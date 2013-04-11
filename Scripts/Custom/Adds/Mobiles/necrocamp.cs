using Server.Items;
using Server.Mobiles;

namespace Server.Multis
{
    public class NecroCamp : BaseCamp
    {
        private Mobile m_Prisoner;
        private BaseDoor m_Gate;

        [Constructable]
        public NecroCamp()
            : base(0x1D4C)
        {
        }

        public override void AddComponents()
        {
            BaseCreature bc;

            IronGate gate = new IronGate(DoorFacing.EastCCW);
            m_Gate = gate;

            gate.KeyValue = Key.RandomValue();
            gate.Locked = true;

            AddItem(gate, -2, 1, 0);

            AddCampChests(gate);

            AddMobile(new Deathknight(), 15, 0, -2, 0);
            AddMobile(new KhaldunZealot(), 15, 0, 1, 0);
            AddMobile(new KhaldunSummoner(), 15, 0, -1, 0);

            switch (Utility.Random(2))
            {
                case 0: m_Prisoner = new Noble(); break;
                case 1: m_Prisoner = new SeekerOfAdventure(); break;
            }

            bc = (BaseCreature)m_Prisoner;
            bc.IsPrisoner = true;
            bc.CantWalk = true;

            m_Prisoner.YellHue = Utility.RandomList(0x57, 0x67, 0x77, 0x87, 0x117);
            AddMobile(m_Prisoner, 2, -2, 0, 0);
        }

        private void AddCampChests(IronGate gate)
        {
            LockableContainer chest = null;

            chest = new MetalChest();

            chest.ItemID = 3708;
            chest.LiftOverride = true;

            TreasureMapChest.Fill(chest, 3);
            chest.DropItem(new Key(KeyType.Iron, gate.KeyValue));

            AddItem(chest, 4, 4, 1);

            LockableContainer crates = null;

            switch (Utility.Random(4))
            {
                case 0: crates = new SmallCrate(); break;
                case 1: crates = new MediumCrate(); break;
                case 2: crates = new LargeCrate(); break;
                default: crates = new LockableBarrel(); break;
            }

            crates.TrapType = TrapType.ExplosionTrap;
            crates.TrapPower = Utility.RandomMinMax(50, 70);
            crates.TrapLevel = 3;

            crates.RequiredSkill = 86;
            crates.LockLevel = 86;
            crates.MaxLockLevel = 126;
            crates.Locked = true;

            crates.DropItem(new Gold(Utility.RandomMinMax(100, 400)));
            crates.DropItem(new Arrow(10));
            crates.DropItem(new Bolt(10));

            crates.LiftOverride = true;

            if (Utility.RandomDouble() < 0.8)
            {
                switch (Utility.Random(4))
                {
                    case 0: crates.DropItem(new LesserCurePotion()); break;
                    case 1: crates.DropItem(new LesserExplosionPotion()); break;
                    case 2: crates.DropItem(new LesserHealPotion()); break;
                    default: crates.DropItem(new LesserPoisonPotion()); break;
                }
            }

            AddItem(crates, 2, 2, 0);
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (m.Player && m_Prisoner != null && m_Gate != null && m_Gate.Locked && m_Prisoner.CantWalk)
            {
                int number;

                switch (Utility.Random(4))
                {
                    default:
                    case 0: number = 502264; break; // Help a poor prisoner!
                    case 1: number = 502266; break; // Aaah! Help me!
                    case 2: number = 1046000; break; // Help! These savages wish to end my life!
                    case 3: number = 1046003; break; // Quickly! Kill them for me! HELP!!
                }

                m_Prisoner.Yell(number);
            }
        }

        public NecroCamp(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Prisoner);
            writer.Write(m_Gate);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Prisoner = reader.ReadMobile();
                        m_Gate = reader.ReadItem() as BaseDoor;
                        break;
                    }
            }
        }
    }
}