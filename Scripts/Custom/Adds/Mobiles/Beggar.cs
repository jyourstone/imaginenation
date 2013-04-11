using System;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class Beggar : BaseCreature
    {
        public override bool CanOpenDoors { get { return true; } }
        public override bool Commandable { get { return false; } }
        public override bool CanTeach { get { return true; } }

        private bool m_Talked = false; // flag to prevent spam

        public bool Talked
        {
            get { return m_Talked; }
            set { m_Talked = value; }
        }

        private bool m_Done = false; // flag to disable for delete

        public bool Done
        {
            get { return m_Done; }
            set { m_Done = value; }
        }

        readonly string[] begsay = new string[] // things to say while begging
		{
			"Excuse me, can you spare some gold?",
			"Please, I need to feed my children!",
			"Food, Clothing, anything at all.",
			"I haven't eaten in three days.",
			"Please I need but a coin or two.",
			"Help me! I need just a bite of food!",
			"Gold? Have you a coin to spare?",
			"It will be cold tonight in these rags."
		};

        readonly string[] newssay = new string[] // just to mix up the response to a news request
		{
			"News? I have no need for news!",
			"News! bah! Where to find my next meal is all the news I need.",
			"I have a story I will share for a coin or two.",
			"No news is good news I always say!"
		};

        [Constructable]
        public Beggar() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
        {
            InitStats(85, 75, 65);

            Title = "the beggar";
            Hue = Utility.RandomSkinHue();

            if (Female == Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            // Start with some rags to wear
            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            // Beggers need hair too
            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A))
                            {
                                Hue = Utility.RandomNondyedHue(),
                                Layer = Layer.Hair,
                                Movable = false
                            };
            AddItem(hair);
            // Start with just a little gold
            Container pack = new Backpack();
            pack.DropItem(new Gold(10, 50));
            pack.Movable = false;
            AddItem(pack);
            // Give some street skills
            Skills[SkillName.Stealing].Base = Utility.Random(50, 70);
            Skills[SkillName.Wrestling].Base = Utility.Random(50, 25);
            Skills[SkillName.Begging].Base = Utility.Random(80, 95);
            // toughen him up a tad
            VirtualArmor = 10;
        }

        public Beggar(Serial serial)
            : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            int nameHue;

            if (NameHue != -1)
                nameHue = NameHue;
            else if (AccessLevel > AccessLevel.Player)
                nameHue = 11;
            else
                nameHue = Notoriety.GetHue(Notoriety.Compute(from, this));

            PrivateOverheadMessage(MessageType.Label, nameHue, AsciiClickMessage, Name, from.NetState);
        }

        public override void OnSpeech(SpeechEventArgs args)
        {
            if (m_Done) return;

            if (args.Mobile.InRange(this, 2)) //if a player is within 2 tiles of the NPC 
            {
                if (args.Speech.ToLower().IndexOf("name") >= 0)
                {
                    Direction = GetDirectionTo(args.Mobile.Location);
                    Say(String.Format("My name is {0}, in your service.", Name)); //Npc tells the player it's name
                    Animate(32, 5, 1, true, false, 0); // Bow
                }

                else if (args.Speech.ToLower().IndexOf("hello") >= 0 || args.Speech.ToLower().IndexOf("hail") >= 0) //Checks to see if the player says Hail or Hello 
                    Say(String.Format("Hail ")); //Npc says hello to the player 

                else if (args.Speech.ToLower().IndexOf("buy") >= 0) //If player says buy
                    Say(String.Format("I have nothing to sell but the rags I wear!")); //Npc will respond 

                else if (args.Speech.ToLower().IndexOf("news") >= 0) //If player says news 
                    SpeechHelper.SayRandom(newssay, this);
            }

            base.OnSpeech(args);
        }

        public override bool OnDragDrop(Mobile m, Item t)
        {
            if (m_Done) return false;

            Container pack = Backpack;

            if (t is BaseClothing)
            {
                Emote("*Puts the clothing on*");
                pack.DropItem(t);
                AddItem(t);
                return true;
            }

            if (t is Food)
            {
                Emote("*Eats the food*");
                PlaySound(Utility.Random(0x3A, 3));
                pack.DropItem(t);
                AddItem(t);
                return true;
            }

            if (t is Gold)
            {
                if (CheckTeachingMatch(m))
                {
                    if (Teach(m_Teaching, m, t.Amount, true))
                    {
                        t.Delete();
                        return true;
                    }
                }

                pack.DropItem(t);
                // Karma gain is 1% of gold with max gain of 25
                int newKarma = (t.Amount > 2500) ? 25 : (t.Amount / 100);
                Titles.AwardKarma(m, newKarma, true);
                Direction = GetDirectionTo(m.Location);

                Animate(32, 5, 1, true, false, 0); // Bow

                if (t.Amount < 50)
                    Say("Thanks.");

                else if ((t.Amount >= 50) && (t.Amount < 100))
                    Say("Thank you for the gold!");

                else if ((t.Amount >= 100) && (t.Amount < 500))
                    Say("Thank you! The pockets are nice and heavy now!");

                else if ((t.Amount >= 500) && (t.Amount < 1000))
                    Say("What a generous sum of gold! Thank you!");

                else if ((t.Amount >= 1000) && (t.Amount < 5000))
                    Say("I owe you my life! Without this money I could not feed or clothe my children!");

                else Say("I am no longer a pauper!");  // t.Amount >= 5000  

                if (TotalGold > 5000)
                {
                    DeleteTimer d = new DeleteTimer(this, m);
                    d.Start();
                }

                return true;
            }

            Say("eh? Whats this for?");
            return false;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m_Done) return;

            if (m_Talked == false)
            {
                if (m.InRange(this, 3) && !m.Hidden)
                {
                    m_Talked = true;
                    SpeechHelper.SayRandom(begsay, this);
                    Move(GetDirectionTo(m.Location));
                    // Start timer to prevent spam
                    SpamTimer t = new SpamTimer(this);
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            private readonly Beggar m_beggar;

            public SpamTimer(Beggar b)
                : base(TimeSpan.FromSeconds(8))
            {
                m_beggar = b;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_beggar.Talked = false;
            }
        }

        private class DeleteTimer : Timer
        {
            private readonly Beggar m_beggar;
            private readonly Mobile m_giver;
            private int ticks;

            public DeleteTimer(Beggar b, Mobile m)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_beggar = b;
                m_giver = m;
                ticks = 0;

                m_beggar.Done = true; // stick a fork in 'im
                m_beggar.Say("With {0} gold, I dont have to wander the streets anymore!", m_beggar.TotalGold);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_beggar != null)
                {
                    if (ticks++ > 5) // 6 seconds
                    {
                        m_beggar.Delete();
                        Stop();
                    }
                    else
                        m_beggar.RunAway(m_giver);
                }
                else
                    Stop();
            }
        }

        public void RunAway(Mobile m)
        {
            Direction = (Direction)(((int)GetDirectionTo(m.Location) + 4) % 8);
            CurrentSpeed = ActiveSpeed;
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