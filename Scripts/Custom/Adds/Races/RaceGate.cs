using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Scripts.Custom.Adds.Races
{
    public enum CustomRace
    {
        None,
        ResetToHuman,
        Goblin,
        Vampire,
        Daemon,
        WoodlandElf,
        Dwarf,
        ShadowMage,
        ShadowApprentice
    }

    public class RaceGate : Item
    {
        private CustomRace m_Race;

        [CommandProperty(AccessLevel.GameMaster)]
        public CustomRace Race
        {
            get { return m_Race; }
            set
            {
                m_Race = value;

                #region Gate hue per race
                switch (value)
                {
                    case CustomRace.None:
                        Hue = 0;
                        break;
                    case CustomRace.ResetToHuman:
                        Hue = 1002;
                        break;
                    case CustomRace.Goblin:
                        Hue = 2212;
                        break;
                    case CustomRace.Daemon:
                        Hue = 1171;
                        break;
                    case CustomRace.Vampire:
                        Hue = 1175;
                        break;
                    case CustomRace.WoodlandElf:
                        Hue = 1434;
                        break;
                    case CustomRace.Dwarf:
                        Hue = 1160;
                        break;
                    case CustomRace.ShadowMage:
                        Hue = 1907;
                        break;
                    case CustomRace.ShadowApprentice:
                        break;
                }
                #endregion
            }
        }

        [Constructable]
        public RaceGate() : base(0xF6C)
        {
            m_Race = CustomRace.None;
            Movable = false;
            Hue = 0x2D1;
            Name = "Race gate";
            Light = LightType.Circle300;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if ( m.Alive == false )
            {
                m.SendAsciiMessage("You cannot use that while dead.");
                return true;
            }
            if ( m_Race != CustomRace.None )
                SetCustomRace(m);

            return base.OnMoveOver(m);
        }

        private void SetCustomRace(Mobile m)
        {
            if (!(m is PlayerMobile))
                return;

            PlayerMobile pm = m as PlayerMobile;

            switch (m_Race)
            {
                case CustomRace.None:
                    {
                        //TODO: Maybe add a message of some sort.
                    }
                    break;
                case CustomRace.ResetToHuman:
                    {
                        Commands.ResetRace.DoResetRace(m);
                    }
                    break;
                case CustomRace.Vampire:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        VampireShroud shroud = new VampireShroud();
                        shroud.Owner = m;
                        m.AddToBackpack(shroud);
                        shroud.OnDoubleClick(m);

                        m.Hue = 1175;
                        pm.HasCustomRace = true;
                    }
                    break;
                case CustomRace.Goblin:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        m.HairItemID = 0x141B;
                        m.HairHue = 2212;
                        m.Hue = 2212;

                        m.Say("Har!");
                        pm.HasCustomRace = true;

                        Effects.PlaySound(m.Location, m.Map, 1271);
                    }
                    break;
                case CustomRace.Daemon:
                    {
                        if (HasCustomRace(pm))
                            return;

                        DaemonClaws claws = new DaemonClaws();
                        pm.EquipItem(claws);

                        OverrideOriginalValues(pm);

                        m.HairItemID = 7947;
                        m.HairHue = 1171;
                        m.Hue = 1171;
                        pm.HasCustomRace = true;

                        Effects.PlaySound(m.Location, m.Map, 357);
                    }
                    break;
                case CustomRace.WoodlandElf:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        if (m.Female == false)
                        {
                            WoodlandRobe robe = new WoodlandRobe();
                            robe.Owner = m;
                            m.AddToBackpack(robe);
                            robe.OnDoubleClick(m);
                        }
                        else
                        {
                            WoodlandDress dress = new WoodlandDress();
                            dress.Owner = m;
                            m.AddToBackpack(dress);
                            dress.OnDoubleClick(m);
                        }

                        WoodlandHat hat = new WoodlandHat();
                        hat.Owner = m;
                        m.AddToBackpack(hat);
                        hat.OnDoubleClick(m);

                        m.Hue = 1434;
                        m.Say("Aaye!");
                        pm.HasCustomRace = true;
                    }
                    break;
                case CustomRace.Dwarf:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        m.Hue = 1160;
                        m.Say("Where be the ale?");
                        pm.HasCustomRace = true;
                    }
                    break;
                case CustomRace.ShadowMage:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        ShadowShroud shroud = new ShadowShroud();
                        shroud.Owner = m;
                        m.AddToBackpack(shroud);
                        shroud.OnDoubleClick(m);
                        
                        m.Say("I will give my life to protect the dragons!");
                        m.Hue = 1907;
                        pm.HasCustomRace = true;
                    }
                    break;
                case CustomRace.ShadowApprentice:
                    {
                        if (HasCustomRace(pm))
                            return;

                        OverrideOriginalValues(pm);

                        ShadowRobe robe = new ShadowRobe();
                        robe.Owner = m;
                        m.AddToBackpack(robe);
                        robe.OnDoubleClick(m);

                        m.Say("I will give my life to protect the dragons!");
                        pm.HasCustomRace = true;
                    }
                    break;
            }

            Point3D loc = m.Location;
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X + 1, loc.Y, loc.Z), m.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, 9502);
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X, loc.Y - 1, loc.Z), m.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, 9502);
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X - 1, loc.Y, loc.Z), m.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, 9502);
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X, loc.Y + 1, loc.Z), m.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, 9502);
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X, loc.Y, loc.Z), m.Map, EffectItem.DefaultDuration), 0, 0, 0, 5014);

        }

        private static bool HasCustomRace(PlayerMobile pm)
        {
            if (pm.HasCustomRace)
            {
                pm.PublicOverheadMessage(MessageType.Emote, 0x3b2, true, "I'm already a different race!");
                return true;
            }
            return false;
        }

        private static void OverrideOriginalValues(PlayerMobile pm)
        {
            pm.OriginalHairItemID = pm.HairItemID;
            pm.OriginalHairHue = pm.HairHue;
            pm.OriginalHue = pm.Hue;
        }

        public RaceGate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            //ver 1
            writer.Write((int) m_Race);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Race = (CustomRace) reader.ReadInt();
        }
    }
}