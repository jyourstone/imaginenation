using System;

namespace Server.Items
{
	public class FishBones : Item
	{

		[Constructable]
		public FishBones() : base( 15116 )
		{
			Name = "Fish Bones";
			Weight = 1.0;
		}

		public FishBones( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}