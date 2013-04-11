using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Eithka Ocksra's corpse" )]
	public class EithkaOcksra : BaseCreature
	{
		[Constructable]
		public EithkaOcksra() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Eithka Ocksra";
			Body = 0x190;
			Hue = 0x492;

			SetStr( 100 ); //I've set him up with normal stats since we've defined his Hits and his weapons speed elsewhere.
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 600, 700 ); // here are his hits according to the spherescript. random between 250 and 350
			SetStam( 350, 450 );

			SetDamage( 15, 20 ); //i set his damage to be low since he hits like 3 times a second. This will probably need to be tweaked.

			SetSkill( SkillName.Swords, 110.0 ); //we dont need to give him uberskillz
			SetSkill( SkillName.Tactics, 110.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Parry, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 3900;
			Karma = -4000;

			VirtualArmor = 70; //this might need to be tweaked

			//Here we add his loot. I'm omitting his gloves since that's later in his dress.
			//PackGold(1000, 1500);

			//Now we dress him
			Item shroud = new HoodedShroudOfShadows(); //since we want a custom shroud, we define it here.
			shroud.Movable = false; //this way we dont let the shroud drop
			shroud.Hue = 0x492; //omg more props
			AddItem( shroud ); //add the item

			Item smurfshoes = new Sandals(); //since sandals are already a class i've named it smurfshoes. It doesnt matter what we call it.
			smurfshoes.Movable = false; // leet shoes for the players? GM tailoring kthx.
			smurfshoes.Hue = 0x492; // more props
			AddItem( smurfshoes ); //add the item

			Scimitar weapon = new Scimitar(); //same thing. define the weapon.
			weapon.Movable = false;
			weapon.Skill = SkillName.Swords; //props
			weapon.Hue = 0x492; //we're leet
			weapon.Speed = 65; //set the speed to 5 higher than the cho ku no.
			weapon.DamageLevel = WeaponDamageLevel.Force; //katana of force
			AddItem( weapon ); //add the weapon

			Item gloves = new PlateGloves(); // here we add his gloves. this time we let them drop
			gloves.Hue = 0x492; //omg more props
			AddItem( gloves ); //add the item

			MetalShield shie = new MetalShield();
			shie.Hue = 0x0492;
			shie.ProtectionLevel = ArmorProtectionLevel.Hardening;
			AddItem( shie );
		}

		public EithkaOcksra( Serial serial ) : base( serial )
		{
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		//Do we want him immune to poison?
		//public override Poison PoisonImmune { get { return Poison.Deadly; } }

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            PackGold(1300, 1600);
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
	}
}