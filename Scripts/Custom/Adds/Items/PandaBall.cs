using System;
using Server.Items;
using Server.Mobiles;
using Xanthos.ShrinkSystem;

namespace Server.Items
{
    public class PandaBall : Item
    {
        [Constructable]
        public PandaBall()
            : base(0xE73)
        {
            Hue = 1164;
            Name = "Panda Ball";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                //Mount
                PandaMount panda = new PandaMount
                                  {
                                      ItemID = 14601,
                                      BodyValue = 133,
                                      Name = "Panda",
                                      ControlMaster = from,
                                      Controlled = true,
                                      MinTameSkill = 90.0,
                                  };

                //Add to backpack
                from.Backpack.DropItem(new ShrinkItem(panda));

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
            }
        }

        public PandaBall(Serial serial)
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
    }

    public class FakePandaBall : Item
    {
        [Constructable]
        public FakePandaBall()
            : base(0xE73)
        {
            Hue = 1953;
            Name = "Discount Panda Ball";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                //Mount
                GrizzlyMount grizzly = new GrizzlyMount
                {
                    ItemID = 14586,
                    BodyValue = 212,
                    Name = "Discount Panda",
                    ControlMaster = from,
                    Controlled = true,
                    MinTameSkill = 90.0,
                    Hue = 1953
                };

                //Add to backpack
                from.Backpack.DropItem(new ShrinkItem(grizzly));

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
            }
        }

        public FakePandaBall(Serial serial)
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
    }

    public class GrizzlyBall : Item
    {
        [Constructable]
        public GrizzlyBall()
            : base(0xE73)
        {
            Hue = 2207;
            Name = "Grizzly Ball";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                //Mount
                GrizzlyMount grizzly = new GrizzlyMount
                {
                    ItemID = 14586,
                    BodyValue = 212,
                    Name = "Grizzly",
                    ControlMaster = from,
                    Controlled = true,
                    MinTameSkill = 90.0
                };

                //Add to backpack
                from.Backpack.DropItem(new ShrinkItem(grizzly));

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
            }
        }

        public GrizzlyBall(Serial serial)
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
    }

    public class BlackStangBall : Item
    {
        [Constructable]
        public BlackStangBall()
            : base(0xE73)
        {
            Hue = 1;
            Name = "Pure black mustang Ball";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                //Mount
                Horse horse = new Horse
                {
                    ItemID = 16034,
                    BodyValue = 120,
                    Name = "Mustang",
                    ControlMaster = from,
                    Controlled = true,
                    MinTameSkill = 90.0,
                    Hue = 1
                };

                //Add to backpack
                from.Backpack.DropItem(new ShrinkItem(horse));

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
            }
        }

        public BlackStangBall(Serial serial)
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
    }

    public class DragonMountBall : Item
    {
        [Constructable]
        public DragonMountBall()
            : base(0xE73)
        {
            Hue = 2586;
            Name = "Dragon Mount Ball";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                //Mount
                DragonMount dragon = new DragonMount
                {
                    ItemID = 0x390F,
                    BodyValue = 34,
                    Name = "Dragon Mount",
                    ControlMaster = from,
                    Controlled = true,
                    MinTameSkill = 90.0
                };

                //Add to backpack
                from.Backpack.DropItem(new ShrinkItem(dragon));

                Delete();
            }
            else
            {
                from.SendAsciiMessage("That must be in your backpack for you to use it.");
            }
        }

        public DragonMountBall(Serial serial)
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
    }
}