using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Custom
{
	public class Headpost : Item
	{
		public static int headDecayTime = 40;
		public DateTime decayTime;
		public Head mountedHead;

		[Constructable]
		public Headpost() : base( 0x1296 )
		{
			Weight = 0.1;
			Movable = false;
			Name = "Head post";
		}

		public Headpost( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			//Makes sure you are close enough when using it
			if( from.InRange( GetWorldLocation(), 3 ) && from.InLOS( this ) )
                if (mountedHead != null)
                {
                    if (mountedHead.Owner != null)
                        from.SendAsciiMessage("This pole is taken by {0}'s head", mountedHead.Owner.Name);

                    return;
                }
                else
                {
                    from.SendAsciiMessage("Select the head you want to mount");
                    from.Target = new InternalTarget(this);
                }
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
		}

		public override void OnSingleClick( Mobile from )
		{
			if( mountedHead == null )
				LabelTo( from, "an empty headpost" );
			else if( mountedHead.Owner != null )
				LabelTo( from, "headpost mounted with {0}'s head", mountedHead.Owner.Name );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( decayTime );
			writer.Write( mountedHead );

			//Calls a method that removes the head from the post 30 secs after the was started
			if( DateTime.Now >= decayTime && mountedHead != null )
				Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerStateCallback( RemoveHeadCallback ), null );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			decayTime = reader.ReadDateTime();
			mountedHead = reader.ReadItem() as Head;
		}

		public void RemoveHeadCallback( object state )
		{
			//(if you remove/add something during a ws the server crashes, hence the freeze during saves)
			if( !World.Saving && mountedHead != null )
			{
				mountedHead.Delete();
				mountedHead = null;
			}
		}

		private class InternalTarget : Target
		{
			private readonly Headpost m_Post;

			public InternalTarget( Headpost post ) : base( 3, false, TargetFlags.None )
			{
				m_Post = post;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				//Makes sure that the target is a head
				if( !( target is Head ) )
				{
					from.SendAsciiMessage( "That is not a head!" );
					return;
				}

				//Casts the target object to become a head, you can do (Head)target
				Head head = ( target as Head );

				//Makes sure the head belongs to someone
				if( head.Owner == null )
				{
					from.SendAsciiMessage( "That head does not belong to anyone!" );
					return;
				}

                //Makes sure the head belongs to a player
                if ( !(head.Owner is PlayerMobile) )
                {
                    from.SendAsciiMessage("That head does not belong to a player!");
                    return;
                }

                //Makes sure the head doesnt belong to yourself
                if (head.Owner == from)
                {
                    from.SendAsciiMessage("You cannot mount your own head!");
                    return;
                }

				//Makes sure that you cant mout the same head over and over again
				if( head.Movable == false )
				{
					from.SendAsciiMessage( "This head is taken!" );
					return;
				}

				//Gives the mounter 1/20 of the owner's fame when he died and removes 100 fame from the owner.
			    int fame = head.Fame == 0 ? head.Owner.Fame : head.Fame;
                int fametogain;

                if (fame >= 100)
                {
                    fametogain = fame / 20;
                    if (fametogain + from.Fame >= 10000) //Can't get more than 10000 fame
                        fametogain = 10000 - from.Fame;
                    from.Fame += fametogain;
                    head.Owner.Fame -= 100;
                    from.SendAsciiMessage("You have been rewarded with {0} fame for posting {1}'s head", fametogain, head.Owner.Name);
                    head.Owner.SendAsciiMessage("{0} posted your head, you have lost 100 fame.", from.Name);
                }
                else
                    from.SendAsciiMessage("The owner of the head has too little fame for you to get any reward");

				//Gives the mounter 250 gold and removes the same amount from the dead guy
				/*if( head.Owner.BankBox != null && head.Owner.BankBox.TotalGold >= 250 )
				{
					if( from.BankBox != null )
					{
						from.BankBox.AddItem( new Gold( 250 ) );
						from.SendAsciiMessage( "You have been rewareded with 250 gold for posting {0}'s head", head.Owner.Name );
					}

					head.Owner.BankBox.TotalGold -= 250;
					head.Owner.SendAsciiMessage( "{0} posted your head, you've lost 250 gold.", from.Name );
					head.Owner.BankBox.ConsumeTotal( typeof( Gold ), 250 );
				}*/

				//Sets the location for the head
				Point3D headLocation = m_Post.Location;
				headLocation.Z += 18;
				head.MoveToWorld( headLocation );

				//Sets some general props for the head: Doest decay(movable = false), the name, that the owner is null(so you cant "remount" it) and that its not movable.
				head.Name = string.Format( "{0}'s head mounted by {1}", head.Owner.Name, from.Name );
				head.Movable = false;

				//Makes sure that the the headposts "knows" it has a head on it and that it starts the decay timer of the head
				m_Post.mountedHead = head;
				m_Post.decayTime = ( DateTime.Now + new TimeSpan( 0, headDecayTime, 0 ) );

				//Displays a message when mounting
				m_Post.PublicOverheadMessage( MessageType.Regular, 906, true, string.Format( "{0} has mounted {1}'s head!", from.Name, head.Owner.Name ) );
			}
		}
	}
}