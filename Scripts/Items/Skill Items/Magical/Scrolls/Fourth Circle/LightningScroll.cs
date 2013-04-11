namespace Server.Items
{
    public class LightningScroll : SpellScroll
    {
        public override int ManaCost { get { return 10; } }

        [Constructable]
        public LightningScroll()
            : this(1)
        {
        }

        [Constructable]
        public LightningScroll(int amount)
            : base(29, 0x1F4A, amount)
        {
        }

        public LightningScroll(Serial serial)
            : base(serial)
        {
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

        /*
        public override void OnDoubleClick(Mobile from)//XUO SCROLLS
        {
            if (!Sphere.CanUse(from, this))
                return;

            if (from.Hits < 5)
            {
                from.LastKiller = from;
                from.Kill();
            }
            else
            {
                from.Hits -= 4;
                from.PlaySound(from.GetHurtSound());

                if (!from.Mounted)
                    from.Animate(20, 5, 1, true, false, 0);
                else
                    from.Animate(29, 5, 1, true, false, 0);

                base.OnDoubleClick(from);
            }
        }
        */
    }
}