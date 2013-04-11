using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class DaemonClaws : BaseRing
    {
        [Constructable]
        public DaemonClaws(): base(5200)
        {
            Weight = 0.0;
            Hue = 1171;
            Name = "Daemon's claws";
            Movable = true;
            LootType = LootType.Blessed;
        }

        public DaemonClaws(Serial serial)
            : base(serial)
        {
        }

        public override bool OnEquip(Mobile from)
        {
            from.LocalOverheadMessage(MessageType.Regular, 906, true, "You feel a dark soul protecting you!");
            from.PlaySound(0x246);

            return base.OnEquip(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.EquipItem(this) || IsChildOf(from))
            {
                from.PlaySound(0x166);
                from.PublicOverheadMessage(MessageType.Emote, 33, true, "*The Daemon claws demand a soul*");
                from.BeginTarget(-1, true, TargetFlags.None, OnTarget);
            }
            else
                from.SendAsciiMessage("You must equip the claws to use them");
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (!from.InRange(GetWorldLocation(), 5) || !from.InLOS(this))
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            else if (obj is Corpse)
            {
                Corpse c = obj as Corpse;

                if (c.Carved)
                    return;

                if (c.Owner is PlayerMobile)
                {
                    ((ICarvable)obj).Carve(from, this);
                    c.PublicOverheadMessage(MessageType.Emote, 33, true, string.Format("*You see {0} steal the soul of {1}*", from.Name, c.Owner.Name));
                    from.PlaySound(0x19c);
                    from.PlaySound(20);
                    from.PlaySound(230);
                    Effects.SendLocationEffect(c.Location, c.Map, 0x37c4, 18, 15, 1);
                }
                else
                    ((ICarvable) obj).Carve(from, this);
            }
            else
                from.SendAsciiMessage("You cannot use this on that");
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