using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a mustang corpse" )]
	public class GoodMustang : BaseMount
	{
		[Constructable]
		public GoodMustang() : this( "Righteous Mustang" )
		{
		}

		[Constructable]
        public GoodMustang(string name): base(name, 0x78, 0x3EA2, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 0xA8;
		    Hue = 2827;

			SetStr( 105, 120 );
			SetDex( 75, 90 );
			SetInt( 20, 30 );

			SetHits( 110, 130 );

			SetDamage( 4, 5 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 80 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 10.0, 20.0 );
			SetSkill( SkillName.Tactics, 10.0 );
			SetSkill( SkillName.Wrestling, 10.0 );

			Fame = 500;
            Karma = 6000;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 100.0;
		}

	    public override void OnDoubleClick(Mobile from)
	    {
	        if (from.Karma < 7499)
            {
                from.SendMessage("You are not righteous enough to ride this animal");
                return;
            }
	        base.OnDoubleClick(from);
	    }

	    public override bool CanBeRenamedBy(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
                return false;
            return true;
        }

        public override int Meat { get { return 8; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public GoodMustang( Serial serial ) : base( serial )
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

			if ( BaseSoundID <= 0 )
				BaseSoundID = 0xA8;
		}
	}
}