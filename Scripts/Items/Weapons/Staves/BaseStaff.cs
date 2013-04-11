namespace Server.Items
{
	public abstract class BaseStaff : BaseMeleeWeapon
	{
      		public override int DefHitSound { get { return 0x232; }}
		public override int DefMissSound {get { return Utility.RandomList(0x238, 0x239, 0x23A); }}

		public override SkillName DefSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefType{ get{ return WeaponType.Staff; } }
		//public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash2H; } }

        	public override int GetSwingAnim(Mobile from)
	        {
        	    if (from.Mounted)
                	return 29;
	            else
        	        return Utility.RandomList(12, 13, 14);
	        }

		public BaseStaff( int itemID ) : base( itemID )
		{
		}

		public BaseStaff( Serial serial ) : base( serial )
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
			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
             */
            defender.Stam -= Utility.RandomMinMax(2, 4);
		}

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
        }
	}
}