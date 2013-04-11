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
	public class ArmorProtectionGem : Item
	{
		[Constructable]
		public ArmorProtectionGem() : this( 1 )
		{
		}

		[Constructable]
		public ArmorProtectionGem( int amount ) : base( 0xF24 )
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Armor protection gem";
			Amount = amount;
			Hue = 2894;
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
			private ArmorProtectionGem m_ArmorProtectionGem;

			public InternalTarget( ArmorProtectionGem runeaug ) : base( 1, false, TargetFlags.None )
			{
				m_ArmorProtectionGem = runeaug;
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

                        if (Armor.Resource != CraftResource.Iron || Armor.LootType != LootType.Regular )
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
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Regular )
								{
									Armor.ProtectionLevel = ArmorProtectionLevel.Defense;
									from.PlaySound( 0x1F5 );
									from.SendMessage( "Your armor becomes more protective." );
									m_ArmorProtectionGem.Delete();
									return;
								}
							
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Defense )
								{
									Armor.ProtectionLevel = ArmorProtectionLevel.Guarding;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more protective.");
									m_ArmorProtectionGem.Delete();
									return;
								}
							
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Guarding )
								{
									Armor.ProtectionLevel = ArmorProtectionLevel.Hardening;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more protective.");
									m_ArmorProtectionGem.Delete();
									return;
								}
							
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Hardening )
								{
									Armor.ProtectionLevel = ArmorProtectionLevel.Fortification;
									from.PlaySound( 0x1F5 );
                                    from.SendMessage("Your armor becomes more protective.");
									m_ArmorProtectionGem.Delete();
									return;
								}
							
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Fortification )
								{
									from.SendMessage( "This gem is not strong enough to strengthen the armor more." );
									return;
								}
							
								if ( Armor.ProtectionLevel == ArmorProtectionLevel.Invulnerability )
								{												 
									from.SendMessage( "This armor is already at full protection level." );
									return;
								}
							}	

							else // Fail
							{
								from.SendMessage( "You have failed to enhance the armor!" );
								from.PlaySound( 42 );
								m_ArmorProtectionGem.Delete();
							}
					
						}
					}
					
					else 
					{
		       			from.SendMessage( "You cannot use this on that." );
		    		} 
					
		  	}
		
		}

        public ArmorProtectionGem(Serial serial)
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

    public class PerfectArmorProtectionGem : Item
    {
        [Constructable]
        public PerfectArmorProtectionGem()
            : this(1)
        {
        }

        [Constructable]
        public PerfectArmorProtectionGem(int amount)
            : base(0xF24)
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Perfect armor protection gem";
            Amount = amount;
            Hue = 2590;
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
            private PerfectArmorProtectionGem m_PerfectArmorProtectionGem;

            public InternalTarget(PerfectArmorProtectionGem runeaug)
                : base(1, false, TargetFlags.None)
            {
                m_PerfectArmorProtectionGem = runeaug;
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
                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Regular)
                            {
                                Armor.ProtectionLevel = ArmorProtectionLevel.Defense;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more protective.");
                                m_PerfectArmorProtectionGem.Delete();
                                return;
                            }

                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Defense)
                            {
                                Armor.ProtectionLevel = ArmorProtectionLevel.Guarding;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more protective.");
                                m_PerfectArmorProtectionGem.Delete();
                                return;
                            }

                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Guarding)
                            {
                                Armor.ProtectionLevel = ArmorProtectionLevel.Hardening;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more protective.");
                                m_PerfectArmorProtectionGem.Delete();
                                return;
                            }

                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Hardening)
                            {
                                Armor.ProtectionLevel = ArmorProtectionLevel.Fortification;
                                from.PlaySound(0x1F5);
                                from.SendMessage("Your armor becomes more protective.");
                                m_PerfectArmorProtectionGem.Delete();
                                return;
                            }

                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Fortification)
                            {
                                Armor.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
                                from.PlaySound(0x1F5);
                                from.SendMessage("This armor now provides maximum protection!");
                                m_PerfectArmorProtectionGem.Delete();
                                return;
                            }

                            if (Armor.ProtectionLevel == ArmorProtectionLevel.Invulnerability)
                            {
                                from.SendMessage("This armor is already at full protection level.");
                                return;
                            }
                        }

                        else // Fail
                        {
                            from.SendMessage("You have failed to enhance the armor!");
                            from.PlaySound(42);
                            m_PerfectArmorProtectionGem.Delete();
                        }

                    }
                }

                else
                {
                    from.SendMessage("You cannot enhance that.");
                }

            }

        }

        public PerfectArmorProtectionGem(Serial serial)
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