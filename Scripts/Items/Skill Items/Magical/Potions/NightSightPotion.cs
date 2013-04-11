using System;

namespace Server.Items
{
    public class NightSightPotion : BaseNightSightPotion
    {
        public DateTime cleanupTime;

        [Constructable]
        public NightSightPotion(): base(PotionEffect.Nightsight)
        {
            cleanupTime = (DateTime.Now + TimeSpan.FromDays(7));
        }

        public NightSightPotion(Serial serial)
            : base(serial)
        {
        }

        public override double PotionDelay { get { return 8.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
            writer.Write(cleanupTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        cleanupTime = reader.ReadDateTime();
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
/*
        public override void Drink(Mobile from)
        {
            //Iza if sphere - no animation + keep drinking
            BuffInfo.RemoveBuff(from, BuffIcon.NightSight);
            new LightCycle.NightSightTimer(from).Start();
            from.LightLevel = 25;

            //from.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            from.PlaySound(0x1E3);

            PlayDrinkEffect(from, this);

            Consume();

            //if (from.BeginAction(typeof(LightCycle)))
            //{
            //    new LightCycle.NightSightTimer(from).Start();
            //    from.LightLevel = LightCycle.DungeonLevel / 2;

            //    from.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            //    from.PlaySound(0x1E3);

            //    PlayDrinkEffect(from);

            //    this.Consume();
            //}
            //else
            //{
            //    from.SendMessage("You already have nightsight.");
            //}
        }*/
    }
}