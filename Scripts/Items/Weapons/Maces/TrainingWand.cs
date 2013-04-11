namespace Server.Items
{
    public class TrainingWand : BaseBashing
	{

        public override int AosStrengthReq { get { return 40; } }
        public override int AosMinDamage { get { return 11; } }
        public override int AosMaxDamage { get { return 13; } }
        public override int AosSpeed { get { return 44; } }

        public override int OldStrengthReq { get { return 10; } }
        public override int OldMinDamage { get { return 1; } }
        public override int OldMaxDamage { get { return 5; } }
        public override int OldSpeed { get { return 312; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public TrainingWand() : base( 0xDF5 )
		{
			Weight = 3.0;
            Name = "Training wand";
			Layer = Layer.OneHanded;
		}

		public TrainingWand( Serial serial ) : base( serial )
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
	}
}