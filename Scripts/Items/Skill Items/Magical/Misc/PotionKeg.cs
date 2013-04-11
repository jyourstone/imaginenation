using System;
using Server.Network;

namespace Server.Items
{
	public class PotionKeg : Item
	{
		private PotionEffect m_Type;
		private int m_Held;
        private int m_MaxAmount = 129;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Held
		{
			get
			{
				return m_Held;
			}
			set
			{
				if ( m_Held != value )
				{
					m_Held = value;
					UpdateWeight();
					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public PotionEffect Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

        [CommandProperty(AccessLevel.GameMaster)]
	    public int MaxCount
	    {
            get { return m_MaxAmount; }
            set { m_MaxAmount = value; }
	    }

		[Constructable]
		public PotionKeg() : base( 0x1940 )
		{
            m_MaxAmount = 129;
			UpdateWeight();
		}

		public virtual void UpdateWeight()
		{
			int held = Math.Max( 0, Math.Min( m_Held, 100 ) );

			Weight = 20 + ((held * 80) / 100);
		}

		public PotionKeg( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 2 ); // version

            writer.Write(m_MaxAmount);
			writer.Write( (int) m_Type );
			writer.Write( m_Held );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 2:
			        {
			            m_MaxAmount = reader.ReadInt();
                        goto case 1;
			        }
			    case 1:
				case 0:
				{
					m_Type = (PotionEffect)reader.ReadInt();
					m_Held = reader.ReadInt();

					break;
				}
			}

			if ( version < 1 )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( UpdateWeight ) );
		}

        public override int LabelNumber
        {
            get
            {
                if (m_Held > 0 && (int)m_Type >= (int)PotionEffect.Conflagration)
                {
                    return 1072658 + (int)m_Type - (int)PotionEffect.Conflagration;
                }

                return (m_Held > 0 ? 1041620 + (int)m_Type : 1041641);
            }
        }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int number;

			if ( m_Held <= 0 )
				number = 502246; // The keg is empty.
			else if ( m_Held < 5 )
				number = 502248; // The keg is nearly empty.
			else if ( m_Held < 20 )
				number = 502249; // The keg is not very full.
			else if ( m_Held < 30 )
				number = 502250; // The keg is about one quarter full.
			else if ( m_Held < 40 )
				number = 502251; // The keg is about one third full.
			else if ( m_Held < 47 )
				number = 502252; // The keg is almost half full.
			else if ( m_Held < 54 )
				number = 502254; // The keg is approximately half full.
			else if ( m_Held < 70 )
				number = 502253; // The keg is more than half full.
			else if ( m_Held < 80 )
				number = 502255; // The keg is about three quarters full.
			else if ( m_Held < 96 )
				number = 502256; // The keg is very full.
			else if ( m_Held < 100 )
				number = 502257; // The liquid is almost to the top of the keg.
			else
				number = 502258; // The keg is completely full.

			list.Add( number );
		}

        public override void OnSingleClick(Mobile from)
        {
            if (m_Held <= 0)
                LabelTo(from, "Empty keg");
            else
                switch (m_Type)
                {
                    case PotionEffect.Nightsight:
                        LabelTo(from, "Nightsight keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "" );
                        break;

                    case PotionEffect.CureLesser:
                        LabelTo(from, "Lesser cure keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.Cure:
                        LabelTo(from, "Cure keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.CureGreater:
                        LabelTo(from, "Greater cure keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Agility:
                        LabelTo(from, "Agility keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.AgilityGreater:
                        LabelTo(from, "Greater agility keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Strength:
                        LabelTo(from, "Strength keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.StrengthGreater:
                        LabelTo(from, "Greater strength keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.PoisonLesser:
                        LabelTo(from, "Lesser poison keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.Poison:
                        LabelTo(from, "Poison keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.PoisonGreater:
                        LabelTo(from, "Greater poison keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.PoisonDeadly:
                        LabelTo(from, "Deadly poison keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Refresh:
                        LabelTo(from, "Refresh keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.RefreshTotal:
                        LabelTo(from, "Total refresh keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.HealLesser:
                        LabelTo(from, "Lesser heal keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.Heal:
                        LabelTo(from, "Heal keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.HealGreater:
                        LabelTo(from, "Greater heal keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.ExplosionLesser:
                        LabelTo(from, "Lesser explosion keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.Explosion:
                        LabelTo(from, "Explosion keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.ExplosionGreater:
                        LabelTo(from, "Greater explosion keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Mana:
                        LabelTo(from, "Mana keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.TotalMana:
                        LabelTo(from, "Total Mana keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Shrink:
                        LabelTo(from, "Shrink keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Invisibility:
                        LabelTo(from, "Invisibility keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.Conflagration:
                        LabelTo(from, "Conflagration keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.ConflagrationGreater:
                        LabelTo(from, "Greater Conflagration keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;

                    case PotionEffect.ConfusionBlast:
                        LabelTo(from, "Confusion Blast keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                    case PotionEffect.ConfusionBlastGreater:
                        LabelTo(from, "Greater Confusion Blast keg containing {0} potion{1}", m_Held, m_Held != 1 ? "s" : "");
                        break;
                }
        }

	    public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), 2 ) && from.InLOS(this) )
			{
				if ( m_Held > 0 )
				{
					Container pack = from.Backpack;

					if ( pack != null )
					{
						BasePotion pot = FillBottle(from);

                        if ( pot == null)
                        {
                            from.SendAsciiMessage("You have no empty bottles in your backpack.");
                            return;
                        }

                        from.SendLocalizedMessage(502242); // You pour some of the keg's contents into an empty bottle...
                        
                        if ( pack.TryDropItem( from, pot, false ) )
						{
							from.SendLocalizedMessage( 502243 ); // ...and place it into your backpack.
							from.PlaySound( 0x240 );

                            if (--Held == 0)
                            {
                                from.SendLocalizedMessage(502245); // The keg is now empty.
                                ColorKeg();
                            }
						}
						else
						{
							from.SendLocalizedMessage( 502244 ); // ...but there is no room for the bottle in your backpack.
							
                            //TODO: Put on ground
                            pot.Delete();
						}
					}
				}
				else
				{
					from.SendLocalizedMessage( 502246 ); // The keg is empty.
				}
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			if ( item is BasePotion )
			{
				BasePotion pot = (BasePotion)item;

                if (pot.PotionEffect != m_Type && m_Held != 0)
                    from.SendLocalizedMessage(502236); // You decide that it would be a bad idea to mix different types of potions.
                else if (m_Held >= m_MaxAmount)
                    from.SendLocalizedMessage(502233); // The keg will not hold any more!
                else if (pot.Amount > (m_MaxAmount - m_Held))
                    from.SendAsciiMessage("That keg can't contain all those potions!");
                else
                {
                    if ( m_Held == 0)
                    {
                        m_Type = pot.PotionEffect;
                    }

                    int toConsume = m_MaxAmount - m_Held;

                    if (toConsume > pot.Amount)
                        toConsume = pot.Amount;

                    m_Held += toConsume;

                    pot.Consume(toConsume);

                    Bottle emptyBottles = new Bottle(toConsume);

                    if (!from.AddToBackpack(emptyBottles))
                    {
                        from.SendAsciiMessage(string.Format("You are too heavey, you place the bottle{0} on the ground.", emptyBottles.Amount == 1 ? string.Empty : "s"));
                        emptyBottles.MoveToWorld(from.Location);
                    }
                    else
                        from.SendAsciiMessage(string.Format("You place the empty bottle{0} in your backpack.", emptyBottles.Amount == 1 ? string.Empty : "s"));

                    ColorKeg();
                    from.PlaySound(0x240);
                }
			}
			else
				from.SendLocalizedMessage( 502232 ); // The keg is not designed to hold that type of object.

            //Always return false
            return false;
        }

		public bool GiveBottle( Mobile m )
		{
			Container pack = m.Backpack;

			Bottle bottle = new Bottle();

			if ( pack == null || !pack.TryDropItem( m, bottle, false ) )
			{
				bottle.Delete();
				return false;
			}

			return true;
		}

        public void ColorKeg()
        {
            if (Held == 0)
                Hue = 0;
            else
            {
                switch (m_Type)
                {
                    case PotionEffect.Nightsight:
                        Hue = 88;
                        break;
                    case PotionEffect.CureLesser:
                    case PotionEffect.Cure:
                    case PotionEffect.CureGreater:
                        Hue = 48;
                        break;
                    case PotionEffect.PoisonLesser:
                    case PotionEffect.Poison:

                    case PotionEffect.PoisonGreater:
                    case PotionEffect.PoisonDeadly:
                        Hue = 68;
                        break;
                    case PotionEffect.RefreshTotal:
                    case PotionEffect.Refresh:
                        Hue = 38;
                        break;
                    case PotionEffect.HealGreater:
                    case PotionEffect.Heal:
                    case PotionEffect.HealLesser:
                        Hue = 53;
                        break;
                    case PotionEffect.ExplosionLesser:
                    case PotionEffect.ExplosionGreater:
                    case PotionEffect.Explosion:
                        Hue = 20;
                        break;
                    case PotionEffect.Mana:
                        Hue = 904;
                        break;
                    case PotionEffect.TotalMana:
                        Hue = 903;
                        break;
                    case PotionEffect.Shrink:
                        Hue = 902;
                        break;
                    case PotionEffect.Invisibility:
                        Hue = 1686;
                        break;
                }
            }
        }

		public BasePotion FillBottle(Mobile from)
		{
            if ( from.Backpack == null || !from.Backpack.ConsumeTotal(typeof(Bottle), 1, true))
                return null;

			switch ( m_Type )
			{
				default:
				case PotionEffect.Nightsight:		return new NightSightPotion();

				case PotionEffect.CureLesser:		return new LesserCurePotion();
				case PotionEffect.Cure:				return new CurePotion();
				case PotionEffect.CureGreater:		return new GreaterCurePotion();

				case PotionEffect.Agility:			return new AgilityPotion();
				case PotionEffect.AgilityGreater:	return new GreaterAgilityPotion();

				case PotionEffect.Strength:			return new StrengthPotion();
				case PotionEffect.StrengthGreater:	return new GreaterStrengthPotion();

				case PotionEffect.PoisonLesser:		return new LesserPoisonPotion();
				case PotionEffect.Poison:			return new PoisonPotion();
				case PotionEffect.PoisonGreater:	return new GreaterPoisonPotion();
				case PotionEffect.PoisonDeadly:		return new DeadlyPoisonPotion();

				case PotionEffect.Refresh:			return new RefreshPotion();
				case PotionEffect.RefreshTotal:		return new TotalRefreshPotion();

				case PotionEffect.HealLesser:		return new LesserHealPotion();
				case PotionEffect.Heal:				return new HealPotion();
				case PotionEffect.HealGreater:		return new GreaterHealPotion();

				case PotionEffect.ExplosionLesser:	return new LesserExplosionPotion();
				case PotionEffect.Explosion:		return new ExplosionPotion();
				case PotionEffect.ExplosionGreater:	return new GreaterExplosionPotion();

                case PotionEffect.Mana: return new ManaPotion();
                case PotionEffect.TotalMana: return new TotalManaPotion();

                case PotionEffect.Shrink: return new ShrinkPotion();

                case PotionEffect.Invisibility: return new InvisibilityPotion();

                case PotionEffect.Conflagration: return new ConflagrationPotion();
                case PotionEffect.ConflagrationGreater: return new GreaterConflagrationPotion();

                case PotionEffect.ConfusionBlast: return new ConfusionBlastPotion();
                case PotionEffect.ConfusionBlastGreater: return new GreaterConfusionBlastPotion();
			}
		}

		public static void Initialize()
		{
			TileData.ItemTable[0x1940].Height = 4;
		}
	}
}