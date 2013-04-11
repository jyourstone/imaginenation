using System;
using Server.Engines.Harvest;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Targets
{
    public class BladedItemTarget : Target
    {
        private readonly Item m_Item;

        public BladedItemTarget(Item item)
            : base(3, false, TargetFlags.None)
        {
            m_Item = item;
        }

        protected override void OnTargetOutOfRange(Mobile from, object targeted)
        {
            if (targeted is UnholyBone && from.InRange(((UnholyBone)targeted), 12))
                ((UnholyBone)targeted).Carve(from, m_Item);
            else
                base.OnTargetOutOfRange(from, targeted);
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Item.Deleted)
                return;

            if (targeted is ICarvable)
            {
                ((ICarvable)targeted).Carve(from, m_Item);
            }
            else if (targeted is SwampDragon && ((SwampDragon)targeted).HasBarding)
            {
                SwampDragon pet = (SwampDragon)targeted;

                if (!pet.Controlled || pet.ControlMaster != from)
                    from.SendLocalizedMessage(1053022); // You cannot remove barding from a swamp dragon you do not own.
                else
                    pet.HasBarding = false;
            }
            else
            {
                if (targeted is StaticTarget)
                {
                    int itemID = ((StaticTarget)targeted).ItemID;

                    if (itemID == 0xD15 || itemID == 0xD16) // red mushroom
                    {
                        PlayerMobile player = from as PlayerMobile;

                        if (player != null)
                        {
                            QuestSystem qs = player.Quest;

                            if (qs is WitchApprenticeQuest)
                            {
                                FindIngredientObjective obj = qs.FindObjective(typeof(FindIngredientObjective)) as FindIngredientObjective;

                                if (obj != null && !obj.Completed && obj.Ingredient == Ingredient.RedMushrooms)
                                {
                                    player.SendLocalizedMessage(1055036); // You slice a red cap mushroom from its stem.
                                    obj.Complete();
                                    return;
                                }
                            }
                        }
                    }
                }

                HarvestSystem system = Lumberjacking.System;
                HarvestDefinition def = Lumberjacking.System.Definition;

                int tileID;
                Map map;
                Point3D loc;

                if (!system.GetHarvestDetails(from, m_Item, targeted, out tileID, out map, out loc))
                {
                    from.SendLocalizedMessage(500494); // You can't use a bladed item on that!
                }
                else if (!def.Validate(tileID))
                {
                    from.SendLocalizedMessage(500494); // You can't use a bladed item on that!
                }
                else
                {
                    HarvestBank bank = def.GetBank(map, loc.X, loc.Y);

                    if (bank == null)
                        return;

                    if (bank.Current < 5)
                    {
                        from.SendLocalizedMessage(500493); // There's not enough wood here to harvest.
                    }
                    else
                        new InternalTimer(from, bank, 2).Start();
                }
            }
        }

        public class InternalTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly HarvestBank m_Bank;
            private int m_Count;
            private readonly int m_MaxCount;
            public InternalTimer(Mobile from, HarvestBank bank, int maxCount)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), maxCount)
            {
                m_From = from;
                m_Bank = bank;
                m_Count = 0;
                m_MaxCount = maxCount;
            }

            protected override void OnTick()
            {
                ++m_Count;
                if (m_Count < m_MaxCount)
                {
                    new SoundTimer(m_From).Start();
                    if (m_From.Mounted)
                        m_From.Animate(26, 5, 2, true, false, 0);
                    else
                        m_From.Animate(9, 5, 2, true, false, 0);
                }
                else
                {
                    m_Bank.Consume(Lumberjacking.System.Definition, 5, m_From);

                    Item item = new Kindling();

                    if (m_From.PlaceInBackpack(item))
                    {
                        m_From.SendLocalizedMessage(500491); // You put some kindling into your backpack.
                        //m_From.SendLocalizedMessage(500492); // An axe would probably get you more wood.
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500490); // You can't place any kindling into your backpack!

                        item.Delete();
                    }
                }
            }
        }

        public class SoundTimer: Timer
        {
            private readonly Mobile m_From;
            public SoundTimer(Mobile from)
                : base(TimeSpan.FromSeconds(1))
            {
                m_From = from;
            }

            protected override void OnTick()
            {
                if (m_From != null)
                    m_From.PlaySound(0x13E);
            }
        }
    }
}