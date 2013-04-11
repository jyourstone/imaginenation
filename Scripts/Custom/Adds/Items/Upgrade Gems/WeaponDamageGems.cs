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
	public class WeaponDamageGem : Item
	{
		[Constructable]
		public WeaponDamageGem() : this( 1 )
		{
		}

		[Constructable]
		public WeaponDamageGem( int amount ) : base( 0xF24 )
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Weapon damage gem";
			Amount = amount;
			Hue = 2706;
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
				
					from.SendMessage( "Select the item to enhance." );
					from.Target = new InternalTarget( this );
				
		        } 

		        else 
		        { 
		        	from.SendLocalizedMessage( 500446 ); // That is too far away. 
		        } 
		} 
		
		private class InternalTarget : Target 
		{
			private WeaponDamageGem m_WeaponDamageGem;

			public InternalTarget( WeaponDamageGem runeaug ) : base( 1, false, TargetFlags.None )
			{
				m_WeaponDamageGem = runeaug;
			}

		 	protected override void OnTarget( Mobile from, object targeted ) 
		 	{ 
				
			    	if ( targeted is BaseWeapon ) 
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
			          		from.SendMessage( "You cannot enhance that in it's current location." ); 
		       			}

						else
		       			{
							int DestroyChance = Utility.Random( 3 );

							if ( DestroyChance > 0 ) // Success
							{
								if ( Weapon.DamageLevel == WeaponDamageLevel.Regular )
								{
									Weapon.DamageLevel = WeaponDamageLevel.Ruin;
									from.PlaySound( 0x1F5 );
									from.SendMessage( "Your weapon becomes more powerful." );
									m_WeaponDamageGem.Delete();
									return;
								}
							
								if ( Weapon.DamageLevel == WeaponDamageLevel.Ruin )
								{
									Weapon.DamageLevel = WeaponDamageLevel.Might;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more powerful.");
									m_WeaponDamageGem.Delete();
									return;
								}
							
								if ( Weapon.DamageLevel == WeaponDamageLevel.Might )
								{
									Weapon.DamageLevel = WeaponDamageLevel.Force;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more powerful.");
									m_WeaponDamageGem.Delete();
									return;
								}
							
								if ( Weapon.DamageLevel == WeaponDamageLevel.Force )
								{
									Weapon.DamageLevel = WeaponDamageLevel.Power;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your weapon becomes more powerful.");
									m_WeaponDamageGem.Delete();
									return;
								}
							
								if ( Weapon.DamageLevel == WeaponDamageLevel.Power )
								{
									from.SendMessage( "This gem is not powerful enough to increase the damage more." );
									return;
								}
							
								if ( Weapon.DamageLevel == WeaponDamageLevel.Vanq )
								{												 
									from.SendMessage( "This weapon is already at full damage level." );
									return;
								}
							}	

							else // Fail
							{
								from.SendMessage( "You have failed to enhance the weapon!" );
								from.PlaySound( 42 );
								m_WeaponDamageGem.Delete();
							}
					
						}
					}
					
					else 
					{
		       			from.SendMessage( "You cannot enhance that." );
		    		} 
					
		  	}
		
		}

        public WeaponDamageGem(Serial serial)
            : base(serial)
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

    public class PerfectWeaponDamageGem : Item
    {
        [Constructable]
        public PerfectWeaponDamageGem()
            : this(1)
        {
        }

        [Constructable]
        public PerfectWeaponDamageGem(int amount)
            : base(0xF24)
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Perfect weapon damage gem";
            Amount = amount;
            Hue = 2718;
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

                from.SendMessage("Select the item to enhance.");
                from.Target = new InternalTarget(this);

            }

            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that. 
            }
        }

        private class InternalTarget : Target
        {
            private PerfectWeaponDamageGem m_PerfectWeaponDamageGem;

            public InternalTarget(PerfectWeaponDamageGem runeaug)
                : base(1, false, TargetFlags.None)
            {
                m_PerfectWeaponDamageGem = runeaug;
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
                            if (Weapon.DamageLevel == WeaponDamageLevel.Regular)
                            {
                                Weapon.DamageLevel = WeaponDamageLevel.Ruin;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your weapon becomes more powerful.");
                                m_PerfectWeaponDamageGem.Delete();
                                return;
                            }

                            if (Weapon.DamageLevel == WeaponDamageLevel.Ruin)
                            {
                                Weapon.DamageLevel = WeaponDamageLevel.Might;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your weapon becomes more powerful.");
                                m_PerfectWeaponDamageGem.Delete();
                                return;
                            }

                            if (Weapon.DamageLevel == WeaponDamageLevel.Might)
                            {
                                Weapon.DamageLevel = WeaponDamageLevel.Force;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your weapon becomes more powerful.");
                                m_PerfectWeaponDamageGem.Delete();
                                return;
                            }

                            if (Weapon.DamageLevel == WeaponDamageLevel.Force)
                            {
                                Weapon.DamageLevel = WeaponDamageLevel.Power;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your weapon becomes more powerful.");
                                m_PerfectWeaponDamageGem.Delete();
                                return;
                            }

                            if (Weapon.DamageLevel == WeaponDamageLevel.Power)
                            {
                                Weapon.DamageLevel = WeaponDamageLevel.Vanq;
                                from.SendMessage("Your weapon becomes as powerful as possible!");
                                return;
                            }

                            if (Weapon.DamageLevel == WeaponDamageLevel.Vanq)
                            {
                                from.SendMessage("This weapon is already at full damage level.");
                                return;
                            }
                        }

                        else // Fail
                        {
                            from.SendMessage("You have failed to enhance the weapon!");
                            from.PlaySound(42);
                            m_PerfectWeaponDamageGem.Delete();
                        }

                    }
                }

                else
                {
                    from.SendMessage("You cannot enhance that.");
                }

            }

        }

        public PerfectWeaponDamageGem(Serial serial)
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