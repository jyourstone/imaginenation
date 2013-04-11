using System;
using Server.Items;

namespace Server.Spells.First
{
    public class CreateFoodSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1E2; } }

        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Create Food", "In Mani Ylem",
                263,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public CreateFoodSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        private static readonly FoodInfo[] m_Food = new[]
                                                        {
                                                            new FoodInfo(typeof (Grapes), "a grape bunch"),
                                                            new FoodInfo(typeof (Ham), "a ham"),
                                                            new FoodInfo(typeof (CheeseWedge), "a wedge of cheese"),
                                                            new FoodInfo(typeof (Muffins), "a muffin"),
                                                            new FoodInfo(typeof (FishSteak), "a fish steak"),
                                                            new FoodInfo(typeof (Ribs), "a cut of ribs"),
                                                            new FoodInfo(typeof (CookedBird), "a cooked bird"),
                                                            new FoodInfo(typeof (Sausage), "a sausage"),
                                                            new FoodInfo(typeof (Apple), "an apple"),
                                                            new FoodInfo(typeof (Peach), "a peach"),
                                                            new FoodInfo(typeof (Cake), "a cake"),
                                                        };

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is IPoint3D)
                Target((IPoint3D)SphereSpellTarget);
            else
                DoFizzle();
        }

        public override void OnCast()
        {
            Target(Caster.Location);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendAsciiMessage("Target is not in line of sight.");
                DoFizzle();
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else if (CheckSequence())
            {
                FoodInfo foodInfo = m_Food[Utility.Random(m_Food.Length)];
                Item food = foodInfo.Create();
                Point3D loc = new Point3D(p);

                if (food != null)
                {
                    food.MoveToWorld(loc, Caster.Map);

                    // You magically create food in your backpack:
                    Caster.SendAsciiMessage("You create " + foodInfo.Name);

                    Caster.FixedParticles(0, 10, 5, 2003, EffectLayer.RightHand);
                    Caster.PlaySound(Sound);
                }
            }

            FinishSequence();
        }
	}

	public class FoodInfo
	{
		private Type m_Type;

	    public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
	    public string Name { get; set; }

	    public FoodInfo( Type type, string name )
		{
			m_Type = type;
			Name = name;
		}

		public Item Create()
		{
			Item item;

			try
			{
				item = (Item)Activator.CreateInstance( m_Type );
			}
			catch
			{
				item = null;
			}

			return item;
		}
	}
}