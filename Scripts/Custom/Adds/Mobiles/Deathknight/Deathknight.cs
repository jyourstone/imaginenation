using System; 
using System.Collections; 
using Server.Misc; 
using Server.Items; 
using Server.Mobiles; 

namespace Server.Mobiles 
{ 
	public class Deathknight : BaseCreature 
	{ 
		[Constructable] 
		public Deathknight() : base( AIType.AI_Necro, FightMode.Aggressor, 10, 1, 0.2, 0.4 ) 
		{ 

			Title = "the deathknight"; 

			SpeechHue = Utility.RandomDyedHue(); 

			Hue = 0; 
			Name = NameList.RandomName( "daemon" ); 

			SetStr( 306, 430 );
			SetDex( 271, 350 );
			SetInt( 250, 355 );

			SetHits ( 2200 );

			if ( Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191;
			} 
			else 
			{ 
				Body = 0x190;
			} 

			new SkeletalMount().Rider = this; 

			PlateChest chest = new PlateChest();
            chest.LootType = LootType.Blessed;
			chest.Hue = 1175; 
			AddItem( chest ); 
			PlateArms arms = new PlateArms();
            arms.LootType = LootType.Blessed;
			arms.Hue = 1175; 
			AddItem( arms ); 
			PlateGloves gloves = new PlateGloves();
            gloves.LootType = LootType.Blessed;
			gloves.Hue = 1175; 
			AddItem( gloves ); 
			PlateGorget gorget = new PlateGorget();
            gorget.LootType = LootType.Blessed;
			gorget.Hue = 1175; 
			AddItem( gorget ); 
			PlateLegs legs = new PlateLegs();
            legs.LootType = LootType.Blessed;
			legs.Hue = 1175; 
			AddItem( legs ); 
			Cloak cloak = new Cloak();
            cloak.LootType = LootType.Blessed;
			cloak.Hue = 1175;
			AddItem( cloak );

			DeathknightsHelm helm = new DeathknightsHelm();
            helm.LootType = LootType.Blessed;
			AddItem( helm );



			switch ( Utility.Random( 4 ))
			{
				case 0:  LargeBattleAxe battleaxe = new LargeBattleAxe();
				battleaxe.Movable = false; 
				battleaxe.Crafter = this; 
				battleaxe.Hue = 1175;
				battleaxe.Quality = WeaponQuality.Exceptional; 
				AddItem( battleaxe ); break;
				case 1:  DoubleAxe doubleaxe = new DoubleAxe();
				doubleaxe.Movable = false; 
				doubleaxe.Crafter = this; 
				doubleaxe.Hue = 1175;
				doubleaxe.Quality = WeaponQuality.Exceptional; 
				AddItem( doubleaxe ); break;
				case 2:  Broadsword broadsword = new Broadsword();
				broadsword.Movable = false; 
				broadsword.Crafter = this; 
				broadsword.Hue = 1175;
				broadsword.Quality = WeaponQuality.Exceptional; 
				AddItem( broadsword ); break;
				case 3:  VikingSword vikingsword = new VikingSword();
				vikingsword.Movable = false; 
				vikingsword.Crafter = this; 
				vikingsword.Hue = 1175;
				vikingsword.Quality = WeaponQuality.Exceptional; 
				AddItem( vikingsword ); break;
			}

            if (Utility.RandomDouble() <= 0.40)
			    PackItem( new DeathknightsHelm() );
			

			PackGold( 980, 1400 );

			Skills[SkillName.Anatomy].Base = 120.0; 
			Skills[SkillName.Tactics].Base = 120.0; 
			Skills[SkillName.Swords].Base = 120.0; 
			Skills[SkillName.Necromancy].Base = 120.0; 
			Skills[SkillName.SpiritSpeak].Base = 120.0; 
			Skills[SkillName.MagicResist].Base = 120.0; 
			Skills[SkillName.Lumberjacking].Base = 100.0; 

		} 
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }

		public virtual void CreateBones_Callback( object state )
		{
			Mobile from = (Mobile)state;
			Map map = from.Map;

			if ( map == null )
				return;

			int count = Utility.RandomMinMax( 1, 3 );

			for ( int i = 0; i < count; ++i )
			{
				int x = from.X + Utility.RandomMinMax( -1, 1 );
				int y = from.Y + Utility.RandomMinMax( -1, 1 );
				int z = from.Z;

				if ( !map.CanFit( x, y, z, 16, false, true ) )
				{
					z = map.GetAverageZ( x, y );

					if ( z == from.Z || !map.CanFit( x, y, z, 16, false, true ) )
						continue;
				}

				DKMist mist = new DKMist();
				mist.Hue = 0x3F;
				mist.Name = " ";
				mist.MoveToWorld( new Point3D( x, y, z ), map );

				DKRemains bone = new DKRemains();
				bone.Hue = 0;
				bone.Name = "remains";
				bone.ItemID = Utility.Random( 3790, 4 );

				bone.MoveToWorld( new Point3D( x, y, z ), map );

			}
		}

        public override bool OnBeforeDeath()
        {
            if (Mounted)
            {
                Item mount = FindItemOnLayer(Layer.Mount);
                mount.Delete();
            }

            return base.OnBeforeDeath();
        }

		public Deathknight( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	} 
}