using System;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
    public class InvisibilityPotion : BasePotion
    {
		[Constructable]
        public InvisibilityPotion() : base(0xF0B, PotionEffect.Invisibility)
		{
            Hue = 1686;
            Name = "Invisibility potion";
		}

        public InvisibilityPotion(Serial serial)
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

        public override void Drink(Mobile from)
        {
            if (from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                if (CanDrink(from))
                {
                    pm.Hidden = true;

                    if (from.Account.AccessLevel == AccessLevel.Player) //Taran: Check account level to prevent issues with speech for staff when toggling GM
                        pm.HiddenWithSpell = true;

                    PlayDrinkEffect(from, this);
                }
            }
        }
    }
}