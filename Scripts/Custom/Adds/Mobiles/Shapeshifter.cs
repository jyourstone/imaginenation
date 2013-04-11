using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using System.Reflection;

namespace Server.Mobiles
{
	[CorpseName( "a shapeshifter corpse" )]
	public class Shapeshifter : BaseCreature
	{

		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }
        private readonly TimeSpan m_ShiftDelay = TimeSpan.FromSeconds(45);
	    private DateTime m_NextShift = DateTime.MinValue;

		[Constructable]
		public Shapeshifter() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.1 )
		{
			Name = "Shapeshifter";
			Body = 58;
			BaseSoundID = 0x451;

			SetStr( 580, 710 );
			SetDex( 250, 275 );
			SetInt( 180, 205 );

			SetHits( 2501, 2620 );

			SetDamage( 28, 39 );

			SetResistance( ResistanceType.Physical, 60, 75 );
			SetResistance( ResistanceType.Fire, 50, 65 );
			SetResistance( ResistanceType.Cold, 50, 65 );
			SetResistance( ResistanceType.Poison, 50, 65 );
			SetResistance( ResistanceType.Energy, 60, 75 );

			SetSkill( SkillName.MagicResist, 135.1, 165.0 );
			SetSkill( SkillName.Tactics, 120.1, 150.0 );
			SetSkill( SkillName.Wrestling, 120.1, 150.0 );
            SetSkill(SkillName.Archery, 110.0, 130.0);
            SetSkill(SkillName.Swords, 109.0, 120.0);
            SetSkill(SkillName.Parry, 115.0, 118.0);
            SetSkill(SkillName.DetectHidden, 90.0, 100.0);
            SetSkill(SkillName.Macing, 110.0, 120.0);
            SetSkill(SkillName.Fencing, 90.0, 100.0);
            SetSkill(SkillName.Anatomy, 150.0, 175.0);
            SetSkill(SkillName.EvalInt, 180.1, 220.0);
            SetSkill(SkillName.Magery, 160.1, 175.0);

			Fame = 11000;
			Karma = -11000;

			VirtualArmor = 95;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss );
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.Gems, 10);
            PackGold(3000, 5000);
		}

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (Hits < 2100)
            {
                if (DateTime.Now > m_NextShift)
                {
                    if (0.20 > Utility.RandomDouble())
                        TurnIntoRandom();
 
                    else if (0.10 > Utility.RandomDouble())
                    {
                        TurnIntoAttacker(from);
                        m_NextShift = DateTime.Now + m_ShiftDelay;
                    }
                }
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

        public void TurnIntoRandom()
        {
            if (Mounted)
            {
                Item mount = FindItemOnLayer(Layer.Mount);
                mount.Delete();
            }

            Hue = 0;

            switch (Utility.Random(40))
            {
                case 0:
                    BodyValue = 212;
                    break; //grizzly
                case 1:
                    BodyValue = 34;
                    break; //wolf
                case 2:
                    BodyValue = 81;
                    break; //frog
                case 3:
                    BodyValue = 88;
                    break; //goat
                case 4:
                    BodyValue = 92;
                    break; //snake
                case 5:
                    BodyValue = 51;
                    break; //slime
                case 6:
                    BodyValue = 215;
                    break; //rat
                case 7:
                    BodyValue = 787;
                    break; //antlion
                case 8:
                    BodyValue = 11;
                    break; //spider
                case 9:
                    BodyValue = 159;
                    break; //blood ele
                case 10:
                    BodyValue = 161;
                    break; //ice ele
                case 11:
                    BodyValue = 13;
                    break; //air ele
                case 12:
                    BodyValue = 40;
                    break; //balron
                case 13:
                    BodyValue = 22;
                    break; //gazer
                case 14:
                    BodyValue = 74;
                    break; //imp
                case 15:
                    BodyValue = 79;
                    break; //lich
                case 16:
                    BodyValue = 140;
                    break; //orc
                case 17:
                    BodyValue = 143;
                    break; //ratman
                case 18:
                    BodyValue = 26;
                    break; //shade
                case 19:
                    BodyValue = 76;
                    break; //titan
                case 20:
                    BodyValue = 149;
                    break; //succubus
                case 21:
                    BodyValue = 57;
                    break; //boneknight
                case 22:
                    BodyValue = 18;
                    break; //ettin
                case 23:
                    BodyValue = 31;
                    break; //headless
                case 24:
                    BodyValue = 154;
                    break; //mummy
                case 25:
                    BodyValue = 1;
                    break; //ogre
                case 26:
                    BodyValue = 67;
                    break; //stonegargoyle
                case 27:
                    BodyValue = 3;
                    break; //zombie
                case 28:
                    BodyValue = 165;
                    break; //shadowwisp
                case 29:
                    BodyValue = 58;
                    break; //wisp
                case 30:
                    BodyValue = 775;
                    break; //plaguebeast
                case 31:
                    BodyValue = 789;
                    break; //quagmire
                case 32:
                    BodyValue = 46;
                    break; //ancientwyrm
                case 33:
                    BodyValue = 12;
                    break; //dragon
                case 34:
                    BodyValue = 85;
                    break; //ophidian
                case 35:
                    BodyValue = 104;
                    break; //skeletaldragon
                case 36:
                    BodyValue = 30;
                    break; //harpy
                case 37:
                    BodyValue = 36;
                    break; //lizardman
                case 38:
                    BodyValue = 48;
                    break; //scorpion
                case 39:
                    BodyValue = 62;
                    break; //wyvern
            }
        }

        public void TurnIntoAttacker(Mobile from)
		{
            Map map = Map;

            if (map == null)
                return;

            if (from.Combatant == null)
                return;

            Mobile m = from.Combatant;

            if (m.Body == 58)
                m.Say("Your soul is now mine!");

            if (m.Body != from.Body)
            {
                if (m.Mounted)
                {
                    Item mount = m.FindItemOnLayer(Layer.Mount);
                    mount.Delete();
                }

                m.BoltEffect(0);

                m.Body = from.Body;
                m.Hue = from.Hue;
                m.HairItemID = from.HairItemID;
                m.HairHue = from.HairHue;
                m.FacialHairItemID = from.FacialHairItemID;
                m.FacialHairHue = from.FacialHairHue;
                m.SkillsCap = from.SkillsCap;
                m.Female = from.Female;
                Warmode = true;

                if (from.Mounted)
                {
                    Item mount = from.FindItemOnLayer(Layer.Mount);
                    new Horse {Hue = mount.Hue, Name = mount.Name, ItemID = mount.ItemID, Rider = m};
                }

                CopyFromLayer(from, this, Layer.Shoes);
                CopyFromLayer(from, this, Layer.Pants);
                CopyFromLayer(from, this, Layer.Shirt);
                CopyFromLayer(from, this, Layer.Helm);
                CopyFromLayer(from, this, Layer.Gloves);
                CopyFromLayer(from, this, Layer.Ring);
                CopyFromLayer(from, this, Layer.Neck);
                CopyFromLayer(from, this, Layer.Hair);
                CopyFromLayer(from, this, Layer.Waist);
                CopyFromLayer(from, this, Layer.InnerTorso);
                CopyFromLayer(from, this, Layer.Bracelet);
                CopyFromLayer(from, this, Layer.MiddleTorso);
                CopyFromLayer(from, this, Layer.Earrings);
                CopyFromLayer(from, this, Layer.Arms);
                CopyFromLayer(from, this, Layer.Cloak);
                CopyFromLayer(from, this, Layer.OuterTorso);
                CopyFromLayer(from, this, Layer.OuterLegs);

                m.BoltEffect(0);
            }
            switch (Utility.Random(5))
            {

                case 0:
                    m.Say("Your existance shall be mine!!");
                    break;
                case 1:
                    m.Say("Your attacks have no effect on me!");
                    break;
                case 2:
                    m.Say("Your end is near weakling!");
                    break;
                case 3:
                    m.Say("Admit defeat and I will make your end quick!");
                    break;
                case 4:
                    m.Say("You are no match for me!");
                    break;
            }
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }


		public Shapeshifter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
        
        private static void CopyFromLayer( Mobile from, Mobile mimic, Layer layer ) 
        {
            if (from.FindItemOnLayer(layer) != null)
            {
                Item copy = from.FindItemOnLayer(layer);
                Type t = copy.GetType();

                ConstructorInfo[] info = t.GetConstructors();

                foreach ( ConstructorInfo c in info )
                {
                    ParameterInfo[] paramInfo = c.GetParameters();

                    if ( paramInfo.Length == 0 )
                    {
                        object[] objParams = new object[0];

                        try 
                        {
                            Item newItem=null;
                            object o = c.Invoke( objParams );

                            if ( o != null && o is Item )
                            {
                                newItem = (Item)o;
                                CopyProperties( newItem, copy );//copy.Dupe( item, copy.Amount );
                                newItem.Parent = null;

                                mimic.EquipItem(newItem);
                            }
                                
                            if ( newItem!=null)
                            {
                                /*
                                if ( newItem is BaseWeapon&& o is BaseWeapon)
                                {
                                    BaseWeapon weapon=newItem as BaseWeapon;
                                    BaseWeapon oweapon=o as BaseWeapon;
                                    //weapon.Attributes=oweapon.Attributes;
                                    //weapon.WeaponAttributes=oweapon.WeaponAttributes;
                                    
                                }
                                */
                                if ( newItem is BaseArmor&& o is BaseArmor)
                                {
                                    BaseArmor armor=newItem as BaseArmor;
                                    BaseArmor oarmor=o as BaseArmor;
                                    armor.Attributes=oarmor.Attributes;
                                    armor.ArmorAttributes=oarmor.ArmorAttributes;
                                    armor.SkillBonuses=oarmor.SkillBonuses;
                                }
                                mimic.EquipItem(newItem);
                            }
                        }
                        catch
                        {
                            from.Say( "Error!" );
                            return;
                        }
                    }
                }
            }
            if (mimic.FindItemOnLayer(layer) != null && mimic.FindItemOnLayer(layer).LootType != LootType.Blessed)
                mimic.FindItemOnLayer(layer).LootType = LootType.Newbied;
        
        }
        /*
        private void DupeFromLayer( Mobile from, Mobile mimic, Layer layer ) 
        {
            if (mimic.FindItemOnLayer(layer) != null && mimic.FindItemOnLayer(layer).LootType != LootType.Blessed)
                mimic.FindItemOnLayer(layer).LootType = LootType.Newbied;
        
        }
        */
        private static void CopyProperties ( Item dest, Item src ) 
        { 
            PropertyInfo[] props = src.GetType().GetProperties(); 

            for ( int i = 0; i < props.Length; i++ ) 
            { 
                try
                {
                    if ( props[i].CanRead && props[i].CanWrite )
                    {
                        //Console.WriteLine( "Setting {0} = {1}", props[i].Name, props[i].GetValue( src, null ) );
                        props[i].SetValue( dest, props[i].GetValue( src, null ), null ); 
                    }
                }
                catch
                {
                    //Console.WriteLine( "Denied" );
                }
            }
        }
	}
}