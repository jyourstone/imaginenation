using System;

namespace Server.Items
{
	[Flipable( 0x13B2, 0x13B1 )]
	public class ElvenBow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override int AosMinDamage{ get{ return 20; } }
		public override int AosMaxDamage{ get{ return 24; } }
		public override int AosSpeed{ get{ return 21; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 15; } }
		public override int OldMaxDamage{ get{ return 21; } }
		public override int OldSpeed{ get{ return 265; } }

		public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

        private SkillMod m_ARCHERYMod;
        private SkillMod m_TACTICSMod;

		//public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public ElvenBow() : base( 0x13B2 )
		{
			Weight = 6.0;
			Name = "Elven bow";
            Hue = 0x237;
			Layer = Layer.TwoHanded;
		}

		public ElvenBow( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile m)
        {
            base.OnDoubleClick(m);
        }

        public override bool OnEquip(Mobile from)
        {
            if (UseSkillMod)
            {
                if (from.FindItemOnLayer(Layer.TwoHanded) != this && m_ARCHERYMod == null && m_TACTICSMod == null)
                {
                    m_TACTICSMod = new DefaultSkillMod(SkillName.Tactics, true, 15);
                    from.AddSkillMod(m_TACTICSMod);
                    m_ARCHERYMod = new DefaultSkillMod(SkillName.Archery, true, 10);
                    from.AddSkillMod(m_ARCHERYMod);
                }
            }
            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            Mobile pl = null;
            if (parent is Mobile)
                pl = (Mobile)parent;
            if (UseSkillMod && m_ARCHERYMod != null && m_TACTICSMod != null && pl != null)
            {
                if (pl.FindItemOnLayer(Layer.TwoHanded) != this)
                {
                    m_ARCHERYMod.Remove();
                    m_ARCHERYMod = null;
                    m_TACTICSMod.Remove();
                    m_TACTICSMod = null;
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
            {
                MinDamage = 15;
                MaxDamage = 21;
            }
		}
	}
}
