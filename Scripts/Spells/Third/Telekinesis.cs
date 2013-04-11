using Server.Items;
using Server.Targeting;

namespace Server.Spells.Third
{
    public class TelekinesisSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x1F5; } }
        
        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Telekinesis", "Ort Por Ylem",
				203,
				9031,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public TelekinesisSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is TrapableContainer) //Taran: Telekinesis only works on trapable containers
                //if (SphereSpellTarget is ITelekinesisable)
                Target((ITelekinesisable)SphereSpellTarget);
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else
            {
                Caster.SendAsciiMessage("This spell only works on trapable containers!");
                DoFizzle();
            }
        }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( ITelekinesisable obj )
		{
			if ( CheckSequence() )
			{
				obj.OnTelekinesis( Caster );
			}

			FinishSequence();
		}

		public void Target( Container item )
		{
			if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, item );

				object root = item.RootParent;

				if ( !item.IsAccessibleTo( Caster ) )
				{
					item.OnDoubleClickNotAccessible( Caster );
				}
				else if ( !item.CheckItemUse( Caster, item ) )
				{
				}
				else if ( root != null && root is Mobile && root != Caster )
				{
					item.OnSnoop( Caster );
				}
                else if (item is Corpse && !((Corpse)item).CheckLoot(Caster, null))
                {
                }
				else if ( Caster.Region.OnDoubleClick( Caster, item ) )
				{
					Effects.SendLocationParticles( EffectItem.Create( item.Location, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
					Effects.PlaySound( item.Location, item.Map, Sound );

					item.OnItemUsed( Caster, item );
				}
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly TelekinesisSpell m_Owner;

			public InternalTarget( TelekinesisSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is ITelekinesisable )
					m_Owner.Target( (ITelekinesisable)o );
				else if ( o is Container )
					m_Owner.Target( (Container)o );
				else
					from.SendLocalizedMessage( 501857 ); // This spell won't work on that!
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}

namespace Server
{
	public interface ITelekinesisable : IPoint3D
	{
		void OnTelekinesis( Mobile from );
	}
}