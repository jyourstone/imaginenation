using System;
using Server.Items;

namespace Server.Voting
{
	/// <summary>
	/// Provides a derivable class to create items that hold VoteSites and to handle Voting.
	/// </summary>
	public class VoteItem : Item
	{
		private static VoteItem _Instance;
		/// <summary>
		/// Gets an instance of VoteItem that can be used to allow voting without the need of an ingame object.
		/// </summary>
		public static VoteItem Instance
		{
			get
			{
				if (_Instance == null || _Instance.Deleted)
				{
					_Instance = new VoteItem(0) {Movable = false, Visible = false};
				    _Instance.Internalize();
				}

				return _Instance;
			}
		}

	    private bool _Messages = true;
		/// <summary>
		/// Gets or Sets a value indecating whether messages should be sent to a vote request sender on various events.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Messages { get { return _Messages; } set { _Messages = value; } }

		private VoteSite _VoteSite;
		/// <summary>
		/// Gets or Sets the vote website profile. 
		/// Exposes options to configure the vote site, such as Name and URL.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public VoteSite VoteSite
		{
			get { return _VoteSite ?? (_VoteSite = new VoteSite(this)); }
		    set
			{
				_VoteSite = value;
				InvalidateProperties();
			}
		}

		public VoteItem(int itemID)
			: base(itemID)
		{ }

		public VoteItem(Serial serial)
			: base(serial)
		{ }

		public virtual void CastVote(Mobile from)
		{
			VoteHelper.CastVote(from, VoteSite);
		}

		public virtual void OnVote(Mobile from, VoteStatus status)
		{
			if (status == VoteStatus.Success && VoteSite.Valid)
			{
				if (_Messages)
				    from.SendMessage("Thank you for voting on {0}!", VoteConfig.Instance.DefaultName);

                if (VoteConfig.Instance.DefaultGold > 0)
                {
                    from.AddToBackpack(new Gold(VoteConfig.Instance.DefaultGold));
                    from.SendAsciiMessage("{0} gold has been added to your backpack", VoteConfig.Instance.DefaultGold);
                    from.PlaySound(55);
                }

                //from.LaunchBrowser("http://www.gamesites200.com/ultimaonline/in.php?id=2224"); Not allowed to have this :(
			    from.LaunchBrowser(VoteConfig.Instance.DefaultURL);
				VoteHelper.SetLastVoteTime(from, VoteSite);

            }
			else if (status == VoteStatus.TooEarly)
			{
				if (_Messages)
				{
					TimeSpan timeLeft = VoteHelper.GetTimeLeft(from, VoteSite);
                    from.SendMessage(0x22, "Sorry, you can not vote on {0} for {1}.", VoteConfig.Instance.DefaultName, VoteHelper.GetFormattedTime(timeLeft));
				}
			}
			else if (status == VoteStatus.Invalid)
			{
				if (_Messages)
				{
					from.SendMessage(0x22, "Sorry, voting is currently unavailable.");
				}
			}
		}

		public virtual bool OnBeforeVote(Mobile from)
		{ return true; }

		public virtual void OnAfterVote(Mobile from, VoteStatus status)
		{ }

		public override void OnDoubleClick(Mobile from)
		{
			base.OnDoubleClick(from);

			CastVote(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(_Messages);

			VoteSite.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						_Messages = reader.ReadBool();

						VoteSite.Deserialize(reader);
					} break;
			}
		}
	}
}
