using System;
using System.Collections;
using System.Collections.Generic;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;


namespace Server.Items
{
	public class AccuracyGem : Item
	{
		[Constructable]
		public AccuracyGem() : this( 1 )
		{
		}

		[Constructable]
		public AccuracyGem( int amount ) : base( 0xF24 )
		{
			Weight = 1.0;
			Stackable = false;
			Name = "Weapon accuracy gem";
			Amount = amount;
			Hue = 2943;
		}
		
		public override void OnDoubleClick( Mobile from ) 
		{
			PlayerMobile pm = from as PlayerMobile;
		
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}

		        else if( from.InRange( this.GetWorldLocation(), 1 ) ) 
		        {
				
					from.SendMessage( "Select the weapon you wish to use this on." );
					from.Target = new InternalTarget( this );
				
		        } 

		        else 
		        { 
		        	from.SendLocalizedMessage( 500446 ); // That is too far away. 
		        } 
		} 
		
		private class InternalTarget : Target 
		{
			private AccuracyGem m_AccuracyGem;

			public InternalTarget( AccuracyGem runeaug ) : base( 1, false, TargetFlags.None )
			{
				m_AccuracyGem = runeaug;
			}

		 	protected override void OnTarget( Mobile from, object targeted ) 
		 	{ 
				
			    	if ( targeted is BaseWeapon ) //not crafted needs adding
					{ 
			       		BaseWeapon Weapon = targeted as BaseWeapon; 

						if ( !from.InRange( ((Item)targeted).GetWorldLocation(), 1 ) ) 
						{ 
			          		from.SendLocalizedMessage( 500446 ); // That is too far away. 
		       			}

                        if (targeted is HellsHalberd || targeted is JudgementHammer || targeted is BlackWidow || targeted is BloodTentacle || targeted is ChuKoNu || targeted is DiamondKatana || targeted is DwarfWarHammer || targeted is DwarvenBattleAxe || targeted is GoblinClooba || targeted is LionheartAxe || targeted is SerpentsTongue || targeted is Wolfbane || Weapon.LootType != LootType.Regular)
                        {
                            from.SendMessage("This gem cannot be used on that.");
                        }

						else if (( ((Item)targeted).Parent != null ) && ( ((Item)targeted).Parent is Mobile ) ) 
			       		{ 
			          		from.SendMessage( "You cannot use the gem on that in it's current location." ); 
		       			}

						else
		       			{
							int DestroyChance = Utility.Random( 3 );

							if ( DestroyChance > 0 ) // Success
							{
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Regular )
								{
									Weapon.AccuracyLevel = WeaponAccuracyLevel.Accurate;
									from.PlaySound( 0x1F5 );
									from.SendMessage( "Your weapon becomes more accurate." );
									m_AccuracyGem.Delete();
									return;
								}
							
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Accurate )
								{
									Weapon.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more accurate.");
									m_AccuracyGem.Delete();
									return;
								}
							
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Surpassingly )
								{
									Weapon.AccuracyLevel = WeaponAccuracyLevel.Eminently;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more accurate.");
									m_AccuracyGem.Delete();
									return;
								}
							
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Eminently )
								{
									Weapon.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more accurate.");
									m_AccuracyGem.Delete();
									return;
								}
							
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Exceedingly )
								{
                                    from.SendMessage("This gem is not powerful enough to increase the accuracy more.");
                                    return;
								}
							
								if ( Weapon.AccuracyLevel == WeaponAccuracyLevel.Supremely )
								{												 
									from.SendMessage( "This weapon is already at full accuracy." );
									return;
								}
							}	

							else // Fail
							{
								from.SendMessage( "You have failed to enhance the weapon!" );
								from.PlaySound( 42 );
								m_AccuracyGem.Delete();
							}
					
						}
					}
					
					else 
					{
		       			from.SendMessage( "You cannot use this on that." );
		    		} 
					
		  	}
		
		}

		public AccuracyGem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    public class PerfectAccuracyGem : Item
    {
        [Constructable]
        public PerfectAccuracyGem()
            : this(1)
        {
        }

        [Constructable]
        public PerfectAccuracyGem(int amount)
            : base(0xF24)
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Perfect weapon accuracy gem";
            Amount = amount;
            Hue = 1162;
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }

            else if (from.InRange(this.GetWorldLocation(), 1))
            {

                from.SendMessage("Select the weapon you wish to use this on.");
                from.Target = new InternalTarget(this);

            }

            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that. 
            }
        }

        private class InternalTarget : Target
        {
            private PerfectAccuracyGem m_PerfectAccuracyGem;

            public InternalTarget(PerfectAccuracyGem runeaug)
                : base(1, false, TargetFlags.None)
            {
                m_PerfectAccuracyGem = runeaug;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {

                if (targeted is BaseWeapon)
                {
                    BaseWeapon Weapon = targeted as BaseWeapon;

                    if (!from.InRange(((Item)targeted).GetWorldLocation(), 1))
                    {
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that. 
                    }

                    if (targeted is HellsHalberd || targeted is JudgementHammer || targeted is BlackWidow || targeted is BloodTentacle || targeted is ChuKoNu || targeted is DiamondKatana || targeted is DwarfWarHammer || targeted is DwarvenBattleAxe || targeted is GoblinClooba || targeted is LionheartAxe || targeted is SerpentsTongue || targeted is Wolfbane)
                    {
                        from.SendMessage("This gem cannot be used on that.");
                    }
                    
                    else if ((((Item)targeted).Parent != null) && (((Item)targeted).Parent is Mobile))
                    {
                        from.SendMessage("You cannot enhance that in it's current location.");
                    }

                    else
                    {
                        int DestroyChance = Utility.Random(3);

                        if (DestroyChance > 0) // Success
                        {
                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Regular)
                            {
                                Weapon.AccuracyLevel = WeaponAccuracyLevel.Accurate;
                                from.PlaySound(0x1F5);
                                from.SendMessage("The accuracy of your weapon has been enhanced.");
                                m_PerfectAccuracyGem.Delete();
                                return;
                            }

                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Accurate)
                            {
                                Weapon.AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                                from.PlaySound(0x1F5);
                                from.SendMessage("The accuracy of your weapon has been enhanced.");
                                m_PerfectAccuracyGem.Delete();
                                return;
                            }

                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Surpassingly)
                            {
                                Weapon.AccuracyLevel = WeaponAccuracyLevel.Eminently;
                                from.PlaySound(0x1F5);
                                from.SendMessage("The accuracy of your weapon has been enhanced.");
                                m_PerfectAccuracyGem.Delete();
                                return;
                            }

                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Eminently)
                            {
                                Weapon.AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
                                from.PlaySound(0x1F5);
                                from.SendMessage("The accuracy of your weapon has been enhanced.");
                                m_PerfectAccuracyGem.Delete();
                                return;
                            }

                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Exceedingly)
                            {
                                Weapon.AccuracyLevel = WeaponAccuracyLevel.Supremely;
                                from.PlaySound(0x1F5);
                                from.SendMessage("You have made this weapon as accurate as possible!");
                                m_PerfectAccuracyGem.Delete();
                                return;
                            }

                            if (Weapon.AccuracyLevel == WeaponAccuracyLevel.Supremely)
                            {
                                from.SendMessage("This weapon is already at full accuracy.");
                                return;
                            }
                        }

                        else // Fail
                        {
                            from.SendMessage("You have failed to enhance the weapon!");
                            from.PlaySound(42);
                            m_PerfectAccuracyGem.Delete();
                        }

                    }
                }

                else
                {
                    from.SendMessage("You cannot use this on that.");
                }

            }

        }

        public PerfectAccuracyGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}