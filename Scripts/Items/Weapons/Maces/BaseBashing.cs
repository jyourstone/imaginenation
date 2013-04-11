using Server.Engines.Craft;

namespace Server.Items
{
	public abstract class BaseBashing : BaseMeleeWeapon
	{
      		public override int DefHitSound { get { return 0x232; }}
		public override int DefMissSound {get { return Utility.RandomList(0x238, 0x239, 0x23A); }}

		public override SkillName DefSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefType{ get{ return WeaponType.Bashing; } }
		//public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }

        	public override int GetSwingAnim(Mobile from)
	        {
        	    if (from.Mounted)
                	return 26;
	            else
        	        return Utility.RandomList(10, 11);
	        }


		public BaseBashing( int itemID ) : base( itemID )
		{
		}

		public BaseBashing( Serial serial ) : base( serial )
		{
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

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			base.OnHit( attacker, defender );

            /* Formula is wrong
			defender.Stam -= Utility.Random( 4, 6 ); // 4-6 points of stamina loss
             */
            defender.Stam -= Utility.RandomMinMax(2, 4);
		}

		public override double GetBaseDamage( Mobile attacker )
		{
			double damage = base.GetBaseDamage( attacker );

            //if ( !Core.AOS && (attacker.Player || attacker.Body.IsHuman) && Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble() )
            //{
            //    damage *= 1.5;

            //    attacker.SendMessage( "You deliver a crushing blow!" ); // Is this not localized?
            //    attacker.PlaySound( 0x11C );
            //}

			return damage;
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!Sphere.CanUse(from, this))
                return;

            base.OnDoubleClick(from);

            if ( this is HammerPick || this is WarHammer)
                from.Target = new SmithTarget(this, DefBlacksmithy.CraftSystem);
        }
	}
}