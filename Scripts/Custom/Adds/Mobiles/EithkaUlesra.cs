using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Eithka Ulesra's corpse" )]
	public class EithkaUlesra : BaseCreature
	{
		[Constructable]
		public EithkaUlesra() : base( AIType.AI_Archer, FightMode.Closest, 10, 5, 0.2, 0.4 )
		{
			Name = "Eithka Ulesra";
			Body = 0x190;
			Hue = 0x798;

			SetStr( 100 ); //I've set him up with normal stats since we've defined his Hits and his weapons speed elsewhere.
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 250, 350 ); // here are his hits according to the spherescript. random between 250 and 350
			SetStam( 350, 450 );

			SetDamage( 10, 15 ); //i set his damage to be low since he hits like 3 times a second. This will probably need to be tweaked.

			SetSkill( SkillName.Archery, 110.0 ); //we dont need to give him uberskillz
			SetSkill( SkillName.Tactics, 110.0 );
			SetSkill( SkillName.MagicResist, 100.0 );

			Fame = 3900;
			Karma = -4000;

			VirtualArmor = 70; //this might need to be tweaked

			//Here we add his loot. I'm omitting his gloves since that's later in his dress.
            //no here do you NOT add the loot, onyl whats in hes backpack.
			Container pack = new Backpack();
			pack.Movable = false;
			pack.DropItem( new Arrow( 50 ) );
			AddItem( pack );
			//Now we dress him
			Item shroud = new HoodedShroudOfShadows(); //since we want a custom shroud, we define it here.
			shroud.Movable = false; //this way we dont let the shroud drop
			shroud.Hue = 0x798; //omg more props
			AddItem( shroud ); //add the item

			Item smurfshoes = new Sandals(); //since sandals are already a class i've named it smurfshoes. It doesnt matter what we call it.
			smurfshoes.Movable = false; // leet shoes for the players? GM tailoring kthx.
			smurfshoes.Hue = 0x798; // more props
			AddItem( smurfshoes ); //add the item

			Bow weapon = new Bow(); //same thing. define the weapon.
			weapon.Skill = SkillName.Archery; //props
			weapon.Hue = 0x798; //we're leet
			weapon.Speed = 90; //set the speed to 5 higher than the cho ku no.
			weapon.DamageLevel = WeaponDamageLevel.Force; //bow of force
			weapon.Movable = false; // not for players
			AddItem( weapon ); //add the weapon

			Item gloves = new PlateGloves(); // here we add his gloves. this time we let them drop
			gloves.Hue = 0x798; //omg more props
			AddItem( gloves ); //add the item
		}

		public EithkaUlesra( Serial serial ) : base( serial )
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
            AddLoot(LootPack.FilthyRich);
            Bow weapon = new Bow {DamageLevel = WeaponDamageLevel.Force, Hue = 0x798};
		    AddItem(weapon);
		}

		protected override bool OnMove( Direction d )
		{
			bool result = base.OnMove( d );

			if( Combatant != null && result )
				NextCombatTime = DateTime.Now;

			return result;
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