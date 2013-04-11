using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
	public class Poisoning
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Poisoning].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.Target = new InternalTargetPoison();
                m.SendLocalizedMessage(502137); // Select the poison you wish to use
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

			return TimeSpan.FromSeconds( 10.0 ); // 10 second delay before beign able to re-use a skill
		}

		private class InternalTargetPoison : Target, IAction
		{
			public InternalTargetPoison() :  base ( 2, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaseLock = true;
				if ( targeted is BasePoisonPotion )
				{
                    releaseLock = false;
					from.SendLocalizedMessage( 502142 ); // To what do you wish to apply the poison?
					from.Target = new InternalTarget( (BasePoisonPotion)targeted );
				}
				else // Not a Poison Potion
				{
					from.SendLocalizedMessage( 502139 ); // That is not a poison potion.
				}
                if (releaseLock && from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
            }

            #region TargetFailed

            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnNonlocalTarget(from, targeted);
            }

            protected override void OnTargetInSecureTrade(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetInSecureTrade(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetCancel(from, cancelType);
            }

            #endregion

            #region IAction Members

            public void AbortAction(Mobile from)
            {
            }

            #endregion


			private class InternalTarget : Target, IAction
			{
				private readonly BasePoisonPotion m_Potion;

				public InternalTarget( BasePoisonPotion potion ) :  base ( 2, false, TargetFlags.None )
				{
					m_Potion = potion;
				}

				protected override void OnTarget( Mobile from, object targeted )
				{
                    bool releaseLock = true;

                    if (m_Potion.Deleted)
                    {
                        if (releaseLock && from is PlayerMobile)
                            ((PlayerMobile)from).EndPlayerAction();

                        return;
                    }

					bool startTimer = false;

					if ( targeted is Food || targeted is FukiyaDarts || targeted is Shuriken )
					{
						startTimer = true;
					}
					else if ( targeted is BaseWeapon )
					{
						BaseWeapon weapon = (BaseWeapon)targeted;

						if ( Core.AOS )
						{
							startTimer = ( weapon.PrimaryAbility == WeaponAbility.InfectiousStrike || weapon.SecondaryAbility == WeaponAbility.InfectiousStrike );
						}
                        else if (weapon.Layer == Layer.OneHanded && !(weapon is SuperiorDiamondKatana || weapon is DiamondKatana || weapon is HellsHalberd || weapon is SuperiorDragonsBlade || weapon is DragonsBlade || weapon is HeavensFury || weapon is SoV)) //Loki edit: 1H blades only
                        {
                            startTimer = (weapon.Type == WeaponType.Slashing || weapon.Type == WeaponType.Piercing);
                        }
					}

					if ( startTimer )
					{
                        releaseLock = false;
						new InternalTimer( from, (Item)targeted, m_Potion ).Start();

						from.PlaySound( 0x4F );

                        if (!m_Potion.EventItem) //Loki edit: Event pots are not consumed and don't create empty bottles
                        {
                            m_Potion.Consume();

                            Bottle bottle = new Bottle();

                            #region Add to pack or ground if overweight
                            //Taran: Check to see if player is overweight. If they are and the item drops to the
                            //ground then a check is made to see if it can be stacked. If it can't and  more than 
                            //20 items of the same type exist in the same tile then the last item gets removed. This 
                            //check is made so thousands of items can't exist in 1 tile and crash people in the same area.
                            if (from.AddToBackpack(bottle))
                                from.SendAsciiMessage("You put the {0} in your pack.", bottle.Name ?? CliLoc.LocToString(bottle.LabelNumber));
                            else if (!bottle.Deleted)
                            {
                                IPooledEnumerable eable = from.Map.GetItemsInRange(from.Location, 0);
                                int amount = 0;
                                Item toRemove = null;

                                foreach (Item i in eable)
                                {
                                    if (i != bottle && i.ItemID == bottle.ItemID)
                                    {
                                        if (i.StackWith(from, bottle, false))
                                        {
                                            toRemove = bottle;
                                            break;
                                        }

                                        amount++;
                                    }
                                }

                                from.SendAsciiMessage("You are overweight and put the {0} on the ground.", bottle.Name ?? CliLoc.LocToString(bottle.LabelNumber));

                                if (toRemove != null)
                                    toRemove.Delete();
                                else if (amount >= 5 && amount < 20)
                                    from.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("{0} identical items on the ground detected, no more than 20 is allowed!", amount));
                                else if (amount >= 20)
                                {
                                    from.LocalOverheadMessage(MessageType.Regular, 906, true, "Too many identical items on the ground, removing!");
                                    bottle.Delete();
                                }

                                eable.Free();
                            }
                            #endregion
                        }
					}
					else // Target can't be poisoned
					{
						if ( Core.AOS )
							from.SendLocalizedMessage( 1060204 ); // You cannot poison that! You can only poison infectious weapons, food or drink.
						else
							//from.SendLocalizedMessage( 502145 ); // You cannot poison that! You can only poison bladed or piercing weapons, food or drink.
                            from.SendAsciiMessage("You cannot poison that! You can only poison certain weapons, food or drink.");
					}

                    if (releaseLock && from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                }

                #region TargetFailed

                protected override void OnCantSeeTarget(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnCantSeeTarget(from, targeted);
                }

                protected override void OnTargetDeleted(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetDeleted(from, targeted);
                }

                protected override void OnTargetUntargetable(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetUntargetable(from, targeted);
                }

                protected override void OnNonlocalTarget(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnNonlocalTarget(from, targeted);
                }

                protected override void OnTargetInSecureTrade(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetInSecureTrade(from, targeted);
                }

                protected override void OnTargetNotAccessible(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetNotAccessible(from, targeted);
                }

                protected override void OnTargetOutOfLOS(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetOutOfLOS(from, targeted);
                }

                protected override void OnTargetOutOfRange(Mobile from, object targeted)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetOutOfRange(from, targeted);
                }

                protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
                {
                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();
                    else
                        from.EndAction(typeof(IAction));

                    base.OnTargetCancel(from, cancelType);
                }

                #endregion

                #region IAction Members

                public void AbortAction(Mobile from)
                {
                }

                #endregion


				private class InternalTimer : Timer, IAction
				{
					private readonly Mobile m_From;
					private readonly Item m_Target;
					private readonly Poison m_Poison;
					private readonly double m_MinSkill;
				    private readonly double m_MaxSkill;

				    public InternalTimer( Mobile from, Item target, BasePoisonPotion potion ) : base( TimeSpan.FromSeconds( 2.0 ) )
					{
						m_From = from;
						m_Target = target;
						m_Poison = potion.Poison;
						m_MinSkill = potion.MinPoisoningSkill;
						m_MaxSkill = potion.MaxPoisoningSkill;
						Priority = TimerPriority.TwoFiftyMS;

                        if (from is PlayerMobile)
                            ((PlayerMobile)from).ResetPlayerAction(this);
					}

					protected override void OnTick()
					{
						if ( m_From.CheckTargetSkill( SkillName.Poisoning, m_Target, m_MinSkill, m_MaxSkill ) )
						{
							if ( m_Target is Food )
							{
								((Food)m_Target).Poison = m_Poison;
							}
							else if ( m_Target is BaseWeapon )
							{
								((BaseWeapon)m_Target).Poison = m_Poison;
								((BaseWeapon)m_Target).PoisonCharges = 18 - (int)(m_Poison.Level * 1.5); //Loki edit: More charges on poison pots, was * 2
							}
							else if ( m_Target is FukiyaDarts )
							{
								((FukiyaDarts)m_Target).Poison = m_Poison;
								((FukiyaDarts)m_Target).PoisonCharges = Math.Min( 18 - (m_Poison.Level * 2), ((FukiyaDarts)m_Target).UsesRemaining );
							}
							else if ( m_Target is Shuriken )
							{
								((Shuriken)m_Target).Poison = m_Poison;
								((Shuriken)m_Target).PoisonCharges = Math.Min( 18 - (m_Poison.Level * 2), ((Shuriken)m_Target).UsesRemaining );
							}

							m_From.SendLocalizedMessage( 1010517 ); // You apply the poison

							//Titles.AwardKarma( m_From, -20, true );
						}
						else // Failed
						{
							// 5% of chance of getting poisoned if failed
							if ( m_From.Skills[SkillName.Poisoning].Base < 80.0 && Utility.Random( 20 ) == 0 )
							{
								m_From.SendLocalizedMessage( 502148 ); // You make a grave mistake while applying the poison.
								m_From.ApplyPoison( m_From, m_Poison );
							}
							else
							{
								if ( m_Target is BaseWeapon )
								{
									BaseWeapon weapon = (BaseWeapon)m_Target;

									if ( weapon.Type == WeaponType.Slashing )
										m_From.SendLocalizedMessage( 1010516 ); // You fail to apply a sufficient dose of poison on the blade
									else
										m_From.SendLocalizedMessage( 1010518 ); // You fail to apply a sufficient dose of poison
								}
								else
								{
									m_From.SendLocalizedMessage( 1010518 ); // You fail to apply a sufficient dose of poison
								}
							}
						}

                        if (m_From is PlayerMobile)
                            ((PlayerMobile)m_From).EndPlayerAction();
                    }

                    #region IAction Members

                    public void AbortAction(Mobile from)
                    {
                        from.SendAsciiMessage("You fail to apply a sufficient dose of poison");

                        if (from is PlayerMobile)
                            ((PlayerMobile)from).EndPlayerAction();

                        Stop();
                    }

                    #endregion
				}
			}
		}
	}
}