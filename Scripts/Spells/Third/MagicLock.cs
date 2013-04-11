using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Third
{
    public class MagicLockSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x1FF; } }
        
        public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Magic Lock", "An Por",
				212,
				9001,
				Reagent.Garlic,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public MagicLockSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is LockableContainer)
                Target(Caster, SphereSpellTarget);
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else
                DoFizzle();
        }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

        public void Target(Mobile from, object o)
        {
            IPoint3D loc = o as IPoint3D;

            if (loc == null)
                return;

            if (CheckSequence())
            {
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X, loc.Y, loc.Z), from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5024);

                Effects.PlaySound(loc, from.Map, 0x1FF);

                if (o is Mobile)
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503101); // That did not need to be unlocked.
                else if (!(o is LockableContainer))
                    from.SendLocalizedMessage(501666); // You can't unlock that!
                else
                {
                    LockableContainer cont = (LockableContainer)o;

                    if (Multis.BaseHouse.CheckSecured(cont))
                        from.SendLocalizedMessage(503098); // You cannot cast this on a secure item.
                    else if (!cont.Locked)
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503101); // That did not need to be unlocked.
                    else if (cont.LockLevel == 0)
                        from.SendLocalizedMessage(501666); // You can't unlock that!
                    else
                    {
                        int level = (int)(from.Skills[SkillName.Magery].Value * 0.8) - 4;

                        if (level >= cont.RequiredSkill && !(cont is TreasureMapChest && ((TreasureMapChest)cont).Level > 2))
                        {
                            cont.Locked = false;

                            if (cont.LockLevel == -255)
                                cont.LockLevel = cont.RequiredSkill - 10;
                        }
                        else
                            from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503099); // My spell does not seem to have an effect on that lock.
                    }
                }
            }

            FinishSequence();
        }

        /*INX
		public void Target( LockableContainer targ )
		{
			if ( BaseHouse.CheckLockedDownOrSecured( targ ) )
			{
				// You cannot cast this on a locked down item.
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 501761 );
			}
			else if ( targ.Locked || targ.LockLevel == 0 )
			{
				// Target must be an unlocked chest.
				Caster.SendLocalizedMessage( 501762 );
			}
			else if ( CheckSequence() )
			{

				Point3D loc = targ.GetWorldLocation();

				Effects.SendLocationParticles(
					EffectItem.Create( loc, targ.Map, EffectItem.DefaultDuration ),
					0x376A, 9, 32, 5020 );

				Effects.PlaySound( loc, targ.Map, 0x1FA );

				// The chest is now locked!
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501763 );

				targ.LockLevel = -255; // signal magic lock
				targ.Locked = true;
			}

			FinishSequence();
		}
        */

		private class InternalTarget : Target
		{
			private readonly MagicLockSpell m_Owner;

			public InternalTarget( MagicLockSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

            protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D loc = o as IPoint3D;

				if ( loc == null )
					return;

				if ( m_Owner.CheckSequence() ) {
					SpellHelper.Turn( from, o );

                    Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc), from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5024);

					Effects.PlaySound( loc, from.Map, 0x1FF );

					if ( o is Mobile )
						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
					else if ( !( o is LockableContainer ) )
						from.SendLocalizedMessage( 501666 ); // You can't unlock that!
					else {
						LockableContainer cont = (LockableContainer)o;

						if ( Multis.BaseHouse.CheckSecured( cont ) ) 
							from.SendLocalizedMessage( 503098 ); // You cannot cast this on a secure item.
						else if ( !cont.Locked )
							from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
						else if ( cont.LockLevel == 0 )
							from.SendLocalizedMessage( 501666 ); // You can't unlock that!
						else {
							int level = (int)(from.Skills[SkillName.Magery].Value * 0.8) - 4;

							if ( level >= cont.RequiredSkill && !(cont is TreasureMapChest && ((TreasureMapChest)cont).Level > 2) ) {
								cont.Locked = false;

								if ( cont.LockLevel == -255 )
									cont.LockLevel = cont.RequiredSkill - 10;
							}
							else
								from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503099 ); // My spell does not seem to have an effect on that lock.
						}							
					}
				}

				m_Owner.FinishSequence();
			}

            /*
			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is LockableContainer )
					m_Owner.Target( (LockableContainer)o );
				else
					from.SendLocalizedMessage( 501762 ); // Target must be an unlocked chest.
			}*/

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}