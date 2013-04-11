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
	public class ArmorDurabilityGem : Item
	{
		[Constructable]
		public ArmorDurabilityGem() : this( 1 )
		{
		}

		[Constructable]ArmorDurabilityGem( int amount ) : base( 0xF24 )
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Armor durability gem";
			Amount = amount;
			Hue = 2851;
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
			private ArmorDurabilityGem m_ArmorDurabilityGem;

			public InternalTarget( ArmorDurabilityGem runeaug ) : base( 1, false, TargetFlags.None )
			{
				m_ArmorDurabilityGem = runeaug;
			}

		 	protected override void OnTarget( Mobile from, object targeted ) 
		 	{ 
				
			    	if ( targeted is BaseArmor ) 
					{ 
			       		BaseArmor Armor = targeted as BaseArmor; 

						if ( !from.InRange( ((Item)targeted).GetWorldLocation(), 1 ) ) 
						{ 
			          		from.SendLocalizedMessage( 500446 ); // That is too far away. 
		       			}

                        if (Armor.Resource != CraftResource.Iron || Armor.LootType != LootType.Regular)
                        {
                            from.SendMessage("That cannot be enhanced.");
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
								if ( Armor.Durability == ArmorDurabilityLevel.Regular )
								{
									Armor.Durability = ArmorDurabilityLevel.Durable;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more durable.");
									m_ArmorDurabilityGem.Delete();
									return;
								}
							
								if ( Armor.Durability == ArmorDurabilityLevel.Durable )
								{
									Armor.Durability = ArmorDurabilityLevel.Substantial;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more durable.");
									m_ArmorDurabilityGem.Delete();
									return;
								}
							
								if ( Armor.Durability == ArmorDurabilityLevel.Substantial )
								{
									Armor.Durability = ArmorDurabilityLevel.Massive;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more durable.");
									m_ArmorDurabilityGem.Delete();
									return;
								}
							
								if ( Armor.Durability == ArmorDurabilityLevel.Massive )
								{
									Armor.Durability = ArmorDurabilityLevel.Fortified;
									from.PlaySound( 0x1F5 );
									from.SendMessage( "Your armor becomes more durable." );
									m_ArmorDurabilityGem.Delete();
									return;
								}
							
								if ( Armor.Durability == ArmorDurabilityLevel.Fortified )
								{
									from.SendMessage( "This gem is not powerful enough to increase the durability more." );
									return;
								}
							
								if ( Armor.Durability == ArmorDurabilityLevel.Indestructible )
								{												 
									from.SendMessage( "This armor is already at full durability level." );
									return;
								}
							}	

							else // Fail
							{
								from.SendMessage( "You have failed to enhance the armor!" );
								from.PlaySound( 42 );
								m_ArmorDurabilityGem.Delete();
							}
					
						}
					}
					
					else 
					{
		       			from.SendMessage( "You cannot use this on that." );
		    		} 
					
		  	}
		
		}

		public ArmorDurabilityGem( Serial serial ) : base( serial )
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

    public class PerfectArmorDurabilityGem : Item
    {
        [Constructable]
        public PerfectArmorDurabilityGem()
            : this(1)
        {
        }

        [Constructable]
        PerfectArmorDurabilityGem(int amount)
            : base(0xF24)
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Armor durability gem";
            Amount = amount;
            Hue = 2905;
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
            private PerfectArmorDurabilityGem m_PerfectArmorDurabilityGem;

            public InternalTarget(PerfectArmorDurabilityGem runeaug)
                : base(1, false, TargetFlags.None)
            {
                m_PerfectArmorDurabilityGem = runeaug;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {

                if (targeted is BaseArmor)
                {
                    BaseArmor Armor = targeted as BaseArmor;

                    if (!from.InRange(((Item)targeted).GetWorldLocation(), 1))
                    {
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that. 
                    }

                    if (Armor.Resource != CraftResource.Iron)
                    {
                        from.SendMessage("That cannot be enhanced.");
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
                            if (Armor.Durability == ArmorDurabilityLevel.Regular)
                            {
                                Armor.Durability = ArmorDurabilityLevel.Durable;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more durable.");
                                m_PerfectArmorDurabilityGem.Delete();
                                return;
                            }

                            if (Armor.Durability == ArmorDurabilityLevel.Durable)
                            {
                                Armor.Durability = ArmorDurabilityLevel.Substantial;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more durable.");
                                m_PerfectArmorDurabilityGem.Delete();
                                return;
                            }

                            if (Armor.Durability == ArmorDurabilityLevel.Substantial)
                            {
                                Armor.Durability = ArmorDurabilityLevel.Massive;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more durable.");
                                m_PerfectArmorDurabilityGem.Delete();
                                return;
                            }

                            if (Armor.Durability == ArmorDurabilityLevel.Massive)
                            {
                                Armor.Durability = ArmorDurabilityLevel.Fortified;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more durable.");
                                m_PerfectArmorDurabilityGem.Delete();
                                return;
                            }

                            if (Armor.Durability == ArmorDurabilityLevel.Fortified)
                            {
                                Armor.Durability = ArmorDurabilityLevel.Indestructible;
                                from.PlaySound(0x1F5);
                                from.SendMessage("You have made this armor as durable as possible!");
                                m_PerfectArmorDurabilityGem.Delete();
                                return;
                            }

                            if (Armor.Durability == ArmorDurabilityLevel.Indestructible)
                            {
                                from.SendMessage("This armor is already at full durability.");
                                return;
                            }
                        }

                        else // Fail
                        {
                            from.SendMessage("You have failed to enhance the armor!");
                            from.PlaySound(42);
                            m_PerfectArmorDurabilityGem.Delete();
                        }

                    }
                }

                else
                {
                    from.SendMessage("You cannot use this on that.");
                }

            }

        }

        public PerfectArmorDurabilityGem(Serial serial)
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