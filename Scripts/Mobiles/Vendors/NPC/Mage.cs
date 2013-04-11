using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class Mage : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.MagesGuild; } }

		[Constructable]
		public Mage() : base( "the mage" )
		{
			SetSkill( SkillName.EvalInt, 65.0, 88.0 );
			SetSkill( SkillName.Inscribe, 60.0, 83.0 );
			SetSkill( SkillName.Magery, 64.0, 100.0 );
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Wrestling, 60.0, 68.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBMage() );
		}

		public override VendorShoeType ShoeType
		{
			get{ return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Robe( Utility.RandomBlueHue() ) );
		}

		public Mage( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

        /*public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile != null && e.Mobile.InRange(this, 3))
            {
                string mobileSpeech = e.Speech.ToLower();

                if (mobileSpeech.Contains("job") || mobileSpeech.Contains("what do you do"))
                    Say("I am living library of the arcane arts.");
                else if (mobileSpeech.Contains("abbey"))
                    Say("Oh, I haven't been to my abbey in ages.");
                else if (mobileSpeech.Contains("ability") || mobileSpeech.Contains("abilities"))
                    Say("One must have born propensity for magic, a natural ability if you will.");
                else if (mobileSpeech.Contains("arcane") || mobileSpeech.Contains("art"))
                    Say("The arcane art is one of keeping mystical secrets.");
                else if (mobileSpeech.Contains("cast") || mobileSpeech.Contains("casting"))
                    Say( "Just close your eyes, say the words, and visualize. You may need your Spellbook for assistance.");
                else if (mobileSpeech.Contains("component") || mobileSpeech.Contains("supplies"))
                    Say("Reagents and a heavy spellbook are all the components one needs.");
                else if (mobileSpeech.Contains("craft"))
                    Say("The craft of a mage takes many years to learn.");
                else if (mobileSpeech.Contains("des mani"))
                    Say("The words to weaken an opponent.");
                else if (mobileSpeech.Contains("empath"))
                    Say("I know not what someone feels nor do I wish to.");
                else if (mobileSpeech.Contains("ether"))
                    Say("I have heard rumors of ether, but have never seen it myself.");
                else if (mobileSpeech.Contains("guild"))
                    Say("Join the Mage's Guild, and you'll receive discounts on reagents.");
                else if (mobileSpeech.Contains("in lor"))
                    Say("Night Sight is very handy when reagent hunting.");
                else if (mobileSpeech.Contains("in mani"))
                    Say("You shall hear these words quite often on a battlefield.");
                else if (mobileSpeech.Contains("in mani ylem"))
                    Say("No, I am not very hungry.");
                else if (mobileSpeech.Contains("in por ylem"))
                    Say("Beginning mages use this spell quite often.");
                else if (mobileSpeech.Contains("ingredients"))
                    Say("Which reagent are you referring to?");
                else if (mobileSpeech.Contains("reagent"))
                    Say("Reagents are the basis for all magic.");
                else if (mobileSpeech.Contains("res wis"))
                    Say("The Feeblemind spell truly isn't that useful.");
                else if (mobileSpeech.Contains("relvinian"))
                    Say("I think you have been in the sun too long.");
                else if (mobileSpeech.Contains("scroll"))
                    Say("Oh yes, always carry a few extra scrolls. One never knows.");
                else if (mobileSpeech.Contains("skill"))
                    Say("One needs to dedicate himself to the arcane arts to acquire great skill.");
                else if (mobileSpeech.Contains("spell"))
                    Say("One can never practice any spell enough.");
                else if (mobileSpeech.Contains("spellbook") || mobileSpeech.Contains("spell book") || mobileSpeech.Contains("spells"))
                    Say("Never leave town without your spellbook.");
                else if (mobileSpeech.Contains("talent"))
                    Say("I feel that one must have a natural talent when it comes to casting spells.");
                else if (mobileSpeech.Contains("uus jux"))
                    Say("Clumsy is one of the first spells that one learns. Quite easy to cast.");
                else
                    base.OnSpeech(e);
            }
            else
                base.OnSpeech(e);
        }*/

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}