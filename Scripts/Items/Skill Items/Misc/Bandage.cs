//Rev 144

using System;
using System.Collections.Generic;
using Server.Factions;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.Items
{
	public class Bandage : Item, IDyable
	{
		public static int Range = 2; 

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public Bandage() : this( 1 )
		{
		}

		[Constructable]
		public Bandage( int amount ) : base( 0xE21 )
		{
			Stackable = true;
			Amount = amount;
		}

		public Bandage( Serial serial ) : base( serial )
		{
		}

        public virtual bool Dye(Mobile from, DyeTub sender)
        {
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !Movable || from.Paralyzed )
			{
				from.SendAsciiMessage( "You can't do that while frozen." );
				return;
			}

			if( from.InRange( GetWorldLocation(), 3 ) && from.InLOS(this) )
			{
				from.SendAsciiMessage( "What do you want to use the clean bandages on?" );
				from.Target = new InternalTarget( this );
			}
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
		}

		public override void Consume()
		{
            if (!EventItem || (EventItem && EventItemConsume))
				base.Consume();
		}

		public override void Consume( int amount )
		{
            if (!EventItem || (EventItem && EventItemConsume))
				base.Consume( amount );
		}

        public static void ClearHandsBandage(Mobile from)
        {
            Item item1 = from.FindItemOnLayer(Layer.OneHanded);
            Item item2 = from.FindItemOnLayer(Layer.TwoHanded);

            if (item1 != null)
                from.Backpack.DropItem(item1);
            if (item2 != null)
                from.Backpack.DropItem(item2);
        }


		private class InternalTarget : Target
		{
			private readonly Bandage m_Bandage;

			public InternalTarget( Bandage bandage ) : base( 10, false, TargetFlags.Beneficial )
			{
				m_Bandage = bandage;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bandage.Deleted )
					return;

				if( targeted is Mobile )
                {
					Mobile targetedMobile = (Mobile)targeted;

					if( from.InRange( m_Bandage.GetWorldLocation(), 3 ) && from.InRange(targetedMobile.Location, Bandage.Range ))
					{
						if( from is PlayerMobile )
							( (PlayerMobile)from ).SpellCheck();

						from.PlaySound( 0x57 );
                        ClearHandsBandage(from);

						BandageContext.BeginHeal( from, targetedMobile, m_Bandage );
					}
					else
						from.SendAsciiMessage( "You are too far away to do that." );
                }
                else if (targeted is PlagueBeastInnard)
                {
                    if (((PlagueBeastInnard)targeted).OnBandage(from))
                        m_Bandage.Consume();
                }
				else
					from.SendAsciiMessage( "Bandages can not be used on that." );
			}

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                if (targeted is PlagueBeastInnard)
                {
                    if (((PlagueBeastInnard)targeted).OnBandage(from))
                        m_Bandage.Consume();
                }
                else
                    base.OnNonlocalTarget(from, targeted);
            }
		}
	}

	public class BandageContext
	{
		private readonly Mobile m_Healer;
		private readonly Mobile m_Patient;
		private int m_Slips;
		private Timer m_Timer;

		public Mobile Healer
		{
			get { return m_Healer; }
		}

		public Mobile Patient
		{
			get { return m_Patient; }
		}

		public int Slips
		{
			get { return m_Slips; }
			set { m_Slips = value; }
		}

		public Timer Timer
		{
			get { return m_Timer; }
		}

		public void Slip()
		{
			m_Healer.SendMessage( "You fail to heal." );
			StopHeal();
		}

		public BandageContext( Mobile healer, Mobile patient, TimeSpan delay, Bandage origin )
		{
			m_Healer = healer;
			m_Patient = patient;

			if( m_Patient != null && !m_Patient.Alive )
			{
				if( m_Healer.Skills[SkillName.Anatomy].Base < 100.0 || m_Healer.Skills[SkillName.Healing].Base < 100.0 )
				{
					if( m_Healer.Skills[SkillName.Anatomy].Base < 100.0 && m_Healer.Skills[SkillName.Healing].Base < 100.0 )
					{
						m_Healer.SendAsciiMessage( "You need GM healing and anatomy to resurrect your target." );
						return;
					}
					else if( m_Healer.Skills[SkillName.Anatomy].Base < 100.0 )
					{
						m_Healer.SendAsciiMessage( "You need GM anatomy to resurrect your target." );
						return;
					}
					else if( m_Healer.Skills[SkillName.Healing].Base < 100.0 )
					{
						m_Healer.SendAsciiMessage( "You need GM healing to resurrect your target." );
						return;
					}
				}

				if( m_Healer.Hits <= 50 )
				{
					m_Healer.SendAsciiMessage( "You need to have more than 50 hp to resurrect your target" );
					return;
				}

				if( m_Patient.Region is HouseRegion )
				{
					m_Healer.SendAsciiMessage( "You can't resurrect people in house regions." );

					//Server.Multis.BaseHouse patientHouse = (m_Patient.Region as HouseRegion).House;

					////The owner can resurrect who ever he wants.
					//if (patientHouse.IsOwner(m_Healer) || patientHouse.IsCoOwner(m_Healer))
					//{
					//    m_Patient.Resurrect();
					//    m_Patient.Hits = 10;
					//    m_Healer.PublicOverheadMessage(MessageType.Regular, 0x22, true, "*You see " + m_Healer.Name + " resurrecting " + m_Patient.Name + "*");
					//    m_Healer.Hits -= 50;
					//}
					////The patient can be ressed by anoyone as long as he is an owner, co owner or friend
					//else if (patientHouse.IsOwner(m_Patient) || patientHouse.IsCoOwner(m_Patient) || patientHouse.IsFriend(m_Patient))
					//{
					//    m_Patient.Resurrect();
					//    m_Patient.Hits = 10;
					//    m_Healer.PublicOverheadMessage(MessageType.Regular, 0x22, true, "*You see " + m_Healer.Name + " resurrecting " + m_Patient.Name + "*");
					//    m_Healer.Hits -= 50;
					//}
					//else
					//{
					//    m_Patient.SendAsciiMessage("You cannot be resurrected in this region!");
					//    m_Healer.SendAsciiMessage("You cannot resurrect in this region!");
					//}
				}
				else
				{
					m_Patient.PlaySound( 0x214 );
					m_Patient.Resurrect();
					m_Patient.Hits = 10;
					m_Healer.PublicOverheadMessage( MessageType.Regular, 0x22, true, "*You see " + m_Healer.Name + " resurrecting " + m_Patient.Name + "*" );
					m_Healer.Hits -= 50;
                    origin.Consume(1);
				}
			}
			else
			{
				m_Timer = new InternalTimer( this, delay, origin );
				m_Timer.Start();
			}
		}

		public void StopHeal()
		{
			m_Table.Remove( m_Healer );

			if( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
		}

		private static readonly Dictionary<Mobile, BandageContext> m_Table = new Dictionary<Mobile, BandageContext>();

		public static BandageContext GetContext( Mobile healer )
		{
			BandageContext bc = null;
			m_Table.TryGetValue( healer, out bc );
			return bc;
		}

		public static SkillName GetPrimarySkill( Mobile m )
		{
			if( !m.Player && ( m.Body.IsMonster || m.Body.IsAnimal ) )
				return SkillName.Veterinary;
			else
				return SkillName.Healing;
		}

		public static SkillName GetSecondarySkill( Mobile m )
		{
			if( !m.Player && ( m.Body.IsMonster || m.Body.IsAnimal ) )
				return SkillName.AnimalLore;
			else
				return SkillName.Anatomy;
		}

		public void EndHeal( Bandage origin )
		{
			StopHeal();

			if( m_Healer is PlayerMobile )
				((PlayerMobile)m_Healer).WeaponTimerCheck();

			int healerNumber = -1;
		    int patientNumber = -1;
			bool checkSkills = false;

			SkillName primarySkill = GetPrimarySkill( m_Patient );
			SkillName secondarySkill = GetSecondarySkill( m_Patient );

			BaseCreature petPatient = m_Patient as BaseCreature;

			if( !m_Healer.Alive )
				healerNumber = 500962; // You were unable to finish your work before you died.
            else if (m_Healer.Paralyzed)
            {
                m_Healer.SendAsciiMessage("You were unable to finish your work before you got paralyzed");
                return;
            }
			else if( !m_Healer.InRange( m_Patient, Bandage.Range ) )
				healerNumber = 500963; // You did not stay close enough to heal your target.
			else if( !m_Patient.Alive || ( petPatient != null && petPatient.IsDeadPet ) )
			{
				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ( ( healing - 68.0 ) / 50.0 ) - ( m_Slips * 0.02 );

				if( ( ( checkSkills = ( healing >= 80.0 && anatomy >= 80.0 ) ) && chance > Utility.RandomDouble() ) || ( Core.SE && petPatient is FactionWarHorse && petPatient.ControlMaster == m_Healer ) )
					//TODO: Dbl check doesn't check for faction of the horse here?
				{
					if( m_Patient.Map == null || !m_Patient.Map.CanFit( m_Patient.Location, 16, false, false ) )
						healerNumber = 501042; // Target can not be resurrected at that location.
					else if( m_Patient.Region != null && m_Patient.Region.IsPartOf( "Khaldun" ) )
					{
						healerNumber = 1010395;
						// The veil of death in this area is too strong and resists thy efforts to restore life.
					}
					else
					{
						healerNumber = 500965; // You are able to resurrect your patient.

						m_Patient.FixedEffect( 0x376A, 10, 16 );

						if( petPatient != null && petPatient.IsDeadPet )
						{
							Mobile master = petPatient.ControlMaster;

                            if (master != null && m_Healer == master)
                            {
                                petPatient.ResurrectPet();

                                for (int i = 0; i < petPatient.Skills.Length; ++i)
                                {
                                    petPatient.Skills[i].Base -= 0.1;
                                }
                            }
                            else if (master != null && master.InRange(petPatient, 3))
                            {
								healerNumber = 503255; // You are able to resurrect the creature.

								master.CloseGump( typeof( PetResurrectGump ) );
								master.SendGump( new PetResurrectGump( m_Healer, petPatient ) );
							}
							else
							{
								bool found = false;

								List<Mobile> friends = petPatient.Friends;

								for( int i = 0; friends != null && i < friends.Count; ++i )
								{
									Mobile friend = friends[i];

									if( friend.InRange( petPatient, 3 ) )
									{
										healerNumber = 503255; // You are able to resurrect the creature.

										friend.CloseGump( typeof( PetResurrectGump ) );
										friend.SendGump( new PetResurrectGump( m_Healer, petPatient ) );

										found = true;
										break;
									}
								}

								if( !found )
									healerNumber = 1049670; // The pet's owner must be nearby to attempt resurrection.
							}
						}
						else
						{
							m_Patient.CloseGump( typeof( ResurrectGump ) );
							m_Patient.SendGump( new ResurrectGump( m_Patient, m_Healer ) );
						}
					}
				}
				else
				{
					if( petPatient != null && petPatient.IsDeadPet )
						healerNumber = 503256; // You fail to resurrect the creature.
					else
						healerNumber = 500966; // You are unable to resurrect your patient.
				}
			}
            else if (m_Patient.Poisoned)
            {
                m_Healer.SendLocalizedMessage(500969); // You finish applying the bandages.

                double healing = m_Healer.Skills[primarySkill].Value;
                double anatomy = m_Healer.Skills[secondarySkill].Value;
                double chance = ((healing - 40.0) / 50.0) - (m_Patient.Poison.Level * 0.11) - (m_Slips * 0.02);

                if ((checkSkills = (healing >= 60.0 && anatomy >= 60.0)) && chance > Utility.RandomDouble())
                {
                    if (m_Patient.CurePoison(m_Healer))
                    {
                        healerNumber = (m_Healer == m_Patient) ? -1 : 1010058; // You have cured the target of all poisons.
                        patientNumber = 1010059; // You have been cured of all poisons.
                    }
                    else
                    {
                        healerNumber = -1;
                        patientNumber = -1;
                    }
                }
                else
                {
                    healerNumber = 1010060; // You have failed to cure your target!
                    patientNumber = -1;
                }
            }
			else if( BleedAttack.IsBleeding( m_Patient ) )
				BleedAttack.EndBleed( m_Patient, false );
			else if( MortalStrike.IsWounded( m_Patient ) )
				healerNumber = ( m_Healer == m_Patient ? 1005000 : 1010398 );
			else if( m_Patient.Hits == m_Patient.HitsMax )
				healerNumber = 500967; // You heal what little damage your patient had.
			else
			{
			    checkSkills = true;
			    patientNumber = -1;
				Bandage bandage = null;

				if( origin.Parent == null && !origin.Deleted )
					origin.Consume( 1 );
				else if( ( bandage = m_Healer.Backpack.FindItemByType( typeof( Bandage ), true ) as Bandage ) == null )
				{
					m_Healer.SendAsciiMessage( "You don't have any bandages." );
					return;
				}
				else
					bandage.Consume( 1 );
                
				double healing = m_Healer.Skills[primarySkill].Base;
				double anatomy = m_Healer.Skills[secondarySkill].Base;

                //Loki edit: Dexterity improves healing chance
                double chance = (healing/100.0) * (0.91 + (((double)m_Healer.RawDex - 80.0) / 1000.0));

				if( chance > Utility.RandomDouble() )
				{
					double min, max;

					min = 0.04 * ( ( anatomy / 4.0 ) + ( healing / 4.0 ) );
					max = ( ( anatomy / 4.0 ) + ( healing / 4.0 ) ) - 4;

                    //Loki edit: Bonus from dexterity
                    double dexbonus = ((double)m_Healer.RawDex - 80.0) / 10.0;
                    min += dexbonus;
                    max += dexbonus / 2;

					if( max < 2 )
						max = 2;

					double toHeal = Utility.RandomMinMax( (int)min, (int)max );
                    
					if( toHeal < 1 )
					{
						toHeal = 1;
						healerNumber = 500968; // You apply the bandages, but they barely help.
					}
					else if( ( !origin.Deleted && (!origin.EventItem || (origin.EventItem && origin.EventItemConsume)) ) || ( bandage != null && (!bandage.EventItem || (bandage.EventItem && bandage.EventItemConsume)) ) )
					{
                        Item item = new BloodyBandage();

                        if (origin.EventItem || (bandage != null && bandage.EventItem))
                        {
                            item.EventItem = true;
                            item.Hue = origin.Hue;
                            item.Name = "event Bloody bandage";
                        }

					    Mobile from = m_Healer;

                        if (from.AddToBackpack(item))
                        {
                            from.SendAsciiMessage("You put the {0} in your pack.", item.Name ?? CliLoc.LocToString(item.LabelNumber));
                        }
                        else //Taran: Bloody bandages stack on ground if the player is overweight
                        {
                            from.SendAsciiMessage("You are overweight and put the {0} on the ground.", item.Name ?? CliLoc.LocToString(item.LabelNumber));

                            IPooledEnumerable eable = from.Map.GetItemsInRange(from.Location, 0);

                            foreach (Item i in eable)
                            {
                                if (i is BloodyBandage)
                                {
                                    if (i.Serial != item.Serial)
                                    {
                                        i.Amount++;
                                        item.Delete();
                                    }

                                    break;
                                }
                            }

                            eable.Free();
                        }
					}
                    else if ( !origin.Deleted && (origin.EventItem && !origin.EventItemConsume))
                    {
                        Mobile from = m_Healer;

                        from.PlaySound(0x57);
                        from.SendAsciiMessage("You are able to re-use your bandage and put it in your pack.");
                    }

					//m_Patient.Heal( (int)toHeal );
                    //Rev ~ 140 update
                    m_Patient.Heal((int)toHeal, m_Healer, false);
				}
				else
					healerNumber = 500968; // You apply the bandages, but they barely help.

				m_Healer.CheckSkill( secondarySkill, 0.0, 120.0 );
				m_Healer.CheckSkill( primarySkill, 0.0, 120.0 );
			}

			if( healerNumber != -1 )
				m_Healer.SendAsciiMessage( CliLoc.LocToString( healerNumber ) );
            if (patientNumber != -1)
                m_Patient.SendLocalizedMessage(patientNumber);
            if (checkSkills)
            {
                m_Healer.CheckSkill(secondarySkill, 0.0, 100.0);
                m_Healer.CheckSkill(primarySkill, 0.0, 100.0);
            }

		}

		private class InternalTimer : Timer
		{
			private readonly BandageContext m_Context;
			private readonly Bandage m_Origin;

			public InternalTimer( BandageContext context, TimeSpan delay, Bandage origin ) : base( delay )
			{
				m_Context = context;
				Priority = TimerPriority.FiftyMS;
				m_Origin = origin;

				//if (context.Healer != context.Patient)
				//    context.Patient.SendAsciiMessage(string.Format("{0} begins to heal you.", context.Healer.Name));
			}

			protected override void OnTick()
			{
				m_Context.EndHeal( m_Origin );
			}
		}

		public static BandageContext BeginHeal( Mobile healer, Mobile patient, Bandage origin )
		{
			bool isDeadPet = ( patient is BaseCreature && ( (BaseCreature)patient ).IsDeadPet );

			if( patient is Golem )
			{
				healer.SendLocalizedMessage( 500970 ); // Bandages cannot be used on that.
			}
			else if( patient is BaseCreature && ( (BaseCreature)patient ).IsAnimatedDead )
			{
				healer.SendLocalizedMessage( 500951 ); // You cannot heal that.
			}
			else if( !patient.Poisoned && patient.Hits >= patient.HitsMax && !BleedAttack.IsBleeding( patient ) && !isDeadPet )
			{
                healer.SendAsciiMessage("That being is not damaged!");
			}
			else if( !patient.Alive && ( patient.Map == null || !patient.Map.CanFit( patient.Location, 16, false, false ) ) )
			{
				healer.SendLocalizedMessage( 501042 ); // Target cannot be resurrected at that location.
			}
			else if( healer.CanBeBeneficial( patient, true, true ) )
			{
				//Maka
				if( healer is PlayerMobile )
					((PlayerMobile)healer).WeaponTimerCheck();

				healer.RevealingAction();

                if ( patient.Player)
				    healer.DoBeneficial( patient );

				bool onSelf = ( healer == patient );
				int dex = healer.Dex;

				//double seconds;
				double bandageDelay = ( patient.Alive ? 2.5 : 0.0 );
                /*
				if ( onSelf )
				{
				    if ( Core.AOS )
				        seconds = 5.0 + (0.5 * ((double)(120 - dex) / 10)); // TODO: Verify algorithm
				    else
				        seconds = 9.4 + (0.6 * ((double)(120 - dex) / 10));
				}
				else
				{
				    if ( Core.AOS && GetPrimarySkill( patient ) == SkillName.Veterinary )
				    {
				        seconds = 2.0;
				    }
                					else if ( Core.AOS )
					{
						if (dex < 204)
						{		
							seconds = 3.2-(Math.Sin((double)dex/130)*2.5) + resDelay;
						}
						else
						{
							seconds = 0.7 + resDelay;
						}
					}
				    else
				    {
				        if ( dex >= 100 )
				            seconds = 3.0 + resDelay;
				        else if ( dex >= 40 )
				            seconds = 4.0 + resDelay;
				       else
				            seconds = 5.0 + resDelay;
				    }
				}
                */
				BandageContext context = GetContext( healer );

				if( context != null )
					context.StopHeal();

                //seconds *= 1000;
                //context = new BandageContext(healer, patient, TimeSpan.FromMilliseconds(seconds));

				context = new BandageContext( healer, patient, TimeSpan.FromSeconds( bandageDelay ), origin );

				m_Table[healer] = context;

				healer.SendAsciiMessage( string.Format( "You put the clean bandages on the wounded {0}.", patient.Name ) );
				return context;
			}

			return null;
		}
	}
}