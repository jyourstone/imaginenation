using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Network;
using Solaris.CliLocHandler;

namespace Server.Items
{
	public enum PotionEffect
	{
		Nightsight,
		CureLesser,
		Cure,
		CureGreater,
		Agility,
		AgilityGreater,
		Strength,
		StrengthGreater,
		PoisonLesser,
		Poison,
		PoisonGreater,
		PoisonDeadly,
		Refresh,
		RefreshTotal,
		HealLesser,
		Heal,
		HealGreater,
		ExplosionLesser,
		Explosion,
		ExplosionGreater,
		Mana,
		Shrink,
		Invisibility,
		Stealth,
        TotalMana,
        Conflagration,
        ConflagrationGreater,
        MaskOfDeath,		// Mask of Death is not available in OSI but does exist in cliloc files
        MaskOfDeathGreater,	// included in enumeration for compatability if later enabled by OSI
        ConfusionBlast,
        ConfusionBlastGreater,
        Parasitic,
        Darkglow
	}

    public abstract class BasePotion : Item, ICraftable, ICommodity
    {
		private PotionEffect m_PotionEffect;

        public virtual double PotionDelay { get { return 17.0; } }

		public PotionEffect PotionEffect
		{
			get
			{
				return m_PotionEffect;
			}
			set
			{
				m_PotionEffect = value;
				InvalidateProperties();
			}
		}

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return (Core.ML); } }

		public override int LabelNumber{ get{ return 1041314 + (int)m_PotionEffect; } }

		public BasePotion( int itemID, PotionEffect effect ) : base( itemID )
		{
			m_PotionEffect = effect;

            //Stackable = Core.ML;
		    Stackable = true;
			Weight = 0.25;
		}

		public BasePotion( Serial serial ) : base( serial )
		{
		}

		public virtual bool RequireFreeHand{ get{ return false; } }

		public static bool HasFreeHand( Mobile m )
		{
			Item handOne = m.FindItemOnLayer( Layer.OneHanded );
			Item handTwo = m.FindItemOnLayer( Layer.TwoHanded );

			if ( handTwo is BaseWeapon )
				handOne = handTwo;
            if (handTwo is BaseRanged)
            {
                BaseRanged ranged = (BaseRanged)handTwo;

                if (ranged.Balanced)
                    return true;
            }

			return ( handOne == null || handTwo == null );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !Movable )
				return;

			if( from.InRange( GetWorldLocation(), 1 ) )
			{
				//if ( !RequireFreeHand || HasFreeHand( from ) )
				Drink( from );
				//else
				//from.SendLocalizedMessage( 502172 ); // You must have a free hand to drink a potion.
			}
			else
			{
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			}
		}

		public override bool OnDroppedInto( Mobile from, Container target, Point3D p )
		{
			PlayerMobile pm = from as PlayerMobile;

			if( pm != null && pm.IsMassmoving )
			{
				if( target == pm.Backpack || target.IsChildOf( pm.Backpack ) )
				{
					Location = p;
					List<BasePotion> potions = pm.Backpack.FindItemsByType( delegate(BasePotion bp) { return ( bp.ItemID == ItemID ); } );

					from.SendAsciiMessage( "Moved {0} potion{1}", potions.Count, potions.Count == 1 ? "." : "s." );

					foreach( BasePotion i in potions )
					{
						target.AddItem( i );
						i.Location = p;
					}
				}
				else
				{
					from.SendAsciiMessage( "You cannot massmove items to places outside of your backpack!" );
				}

				pm.IsMassmoving = false;
				return base.OnDroppedInto( from, target, p );
			}
		    return base.OnDroppedInto( from, target, p );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.Write(2); // version

			writer.Write( (int)m_PotionEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 2:
			        Stackable = true;
			        goto case 1;
                case 1:
				case 0:
				{
					m_PotionEffect = (PotionEffect)reader.ReadInt();
					break;
				}
			}

            if (version == 0)
                Stackable = Core.ML;
		}

		public abstract void Drink( Mobile from );

		public static void PlayDrinkEffect( Mobile m )
		{
			PlayDrinkEffect( m, null );
		}

		public static void PlayDrinkEffect( Mobile m, BasePotion potion )
		{
            if ( m is PlayerMobile && !(m as PlayerMobile).HiddenWithSpell )
			    m.RevealingAction();

			m.PlaySound( 0x31 );

            if (potion != null)
            {
                if (!potion.EventItem || (potion.EventItem && potion.EventItemConsume))
                {
                    Bottle b = new Bottle();

                    if (potion.EventItem)
                    {
                        b.EventItem = true;
                        b.Hue = potion.Hue;
                        b.Name = "event Empty bottle";
                    }

                    #region Add to pack or ground if overweight
                    //Taran: Check to see if player is overweight. If they are and the item drops to the
                    //ground then a check is made to see if it can be stacked. If it can't and  more than 
                    //20 items of the same type exist in the same tile then the last item gets removed. This 
                    //check is made so thousands of items can't exist in 1 tile and crash people in the same area.
                    if (!m.AddToBackpack(b))
                    {
                        IPooledEnumerable eable = m.Map.GetItemsInRange(m.Location, 0);
                        int amount = 0;
                        Item toRemove = null;

                        foreach (Item i in eable)
                        {
                            if (i != b && i.ItemID == b.ItemID)
                            {
                                if (i.StackWith(m, b, false))
                                {
                                    toRemove = b;
                                    break;
                                }

                                amount++;
                            }
                        }

                        m.SendAsciiMessage("You are overweight and put the {0} on the ground.", b.Name ?? CliLoc.LocToString(b.LabelNumber));

                        if (toRemove != null)
                            toRemove.Delete();
                        else if (amount >= 5 && amount < 20)
                            m.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} identical items on the ground detected, no more than 20 is allowed!", amount));
                        else if (amount >= 20)
                        {
                            m.LocalOverheadMessage(MessageType.Regular, 906, true, "Too many identical items on the ground, removing!");
                            b.Delete();
                        }

                        eable.Free();
                    }
                    #endregion

                    potion.Consume(1);
                }
            }

		    if( m.Body.IsHuman )
			    m.Animate(m.Mounted ? 28 : 34, 5, 1, true, false, 0);
		}

        public static int EnhancePotions(Mobile m)
        {
            int EP = AosAttributes.GetValue(m, AosAttribute.EnhancePotions);
            int skillBonus = m.Skills.Alchemy.Fixed / 330 * 10;

            if (Core.ML && EP > 50 && m.AccessLevel <= AccessLevel.Player)
                EP = 50;

            return (EP + skillBonus);
        }

		public static TimeSpan Scale( Mobile m, TimeSpan v )
		{
			if ( !Core.AOS )
				return v;

            double scalar = 1.0 + (0.01 * EnhancePotions(m));

			return TimeSpan.FromSeconds( v.TotalSeconds * scalar );
		}

		public static double Scale( Mobile m, double v )
		{
			if ( !Core.AOS )
				return v;

            double scalar = 1.0 + (0.01 * EnhancePotions(m));

			return v * scalar;
		}

		public static int Scale( Mobile m, int v )
		{
			if ( !Core.AOS )
				return v;

            return AOS.Scale(v, 100 + EnhancePotions(m));
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if (dropped is BasePotion && ((BasePotion)dropped).m_PotionEffect == m_PotionEffect)
                return base.StackWith(from, dropped, playSound);

            return false;
        }

        protected bool CanDrink(Mobile from)
        {
            if (from.BeginAction(typeof(BasePotion)))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(PotionDelay), new TimerStateCallback(ReleaseLock), from);
                return true;
            }

            from.Animate(from.Mounted ? 58 : 34, 5, 1, true, true, 0);
            from.PlaySound(48);
            from.SendAsciiMessage("You can't drink another potion yet!");

            return false;
        }

        protected static void ReleaseLock(object state)
        {
            ((Mobile)state).EndAction(typeof(BasePotion));
        }

		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			if ( craftSystem is DefAlchemy )
			{
				Container pack = from.Backpack;

				if ( pack != null )
				{
                    List<PotionKeg> kegs = pack.FindItemsByType<PotionKeg>();

                    for (int i = 0; i < kegs.Count; ++i)
                    {
                        PotionKeg keg = kegs[i];

						if ( keg == null )
							continue;

						if ( keg.Held <= 0 || keg.Held >= 129 )
							continue;

						if ( keg.Type != PotionEffect )
							continue;

						++keg.Held;

                        from.LocalOverheadMessage(MessageType.Regular, 0x22, true, "*You pour the completed potion into a potion keg*");

                        Consume();
                        from.AddToBackpack(new Bottle(), false);

						return -1; // signal placed in keg
					}

                    from.LocalOverheadMessage(MessageType.Regular, 0x22, true, "*You pour the completed potion into a bottle*");
				}
			}

			return 1;
		}

		#endregion
	}
}