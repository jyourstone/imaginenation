using System;
using Server.Misc;
using Server.Voting;

namespace Server.Items
{
	/// <summary>
	/// The Vote Stone object.
	/// </summary>
	public class VoteStone : VoteItem
	{
		private string _LabelMessage = "Use: Launches your browser to cast a vote for " + ServerList.ServerName;
		/// <summary>
		/// The message appended to the displayed ObjectPropertyList when the vote stone GetProperties method is called.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public string LabelMessage
		{
			get { return _LabelMessage; }
			set
			{
				_LabelMessage = value;
				InvalidateProperties();
			}
		}

		/// <summary>
		/// Constructs a new instance of VoteStone.
		/// </summary>
		[Constructable]
		public VoteStone()
			: this("Vote Stone")
		{ }

		/// <summary>
		/// Constructs a new instance of VoteStone with an optional Name.
		/// </summary>
		/// <param name="name">Name supllied for this instance of VoteStone.</param>
		[Constructable]
		public VoteStone(string name)
			: this(name, 0)
		{ }

		/// <summary>
		/// Constructs a new instance of VoteStone with an option Name and Hue.
		/// </summary>
		/// <param name="name">Name supplied for this instance of VoteStone.</param>
		/// <param name="hue">Hue supplied for this instance of VoteStone.</param>
		[Constructable]
		public VoteStone(string name, int hue)
			: base(0xED4)
		{
			Name = name;
			Hue = hue;

			Movable = false;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (VoteSite.Valid)
			{
				list.Add(1070722,
					String.Format(
						"<BASEFONT COLOR=#00FF00>{0}<BASEFONT COLOR=#FFFFFF>" +
						"\n[{1}] " +
						"{2}",
						_LabelMessage,
						VoteSite.Name,
						VoteSite.URL.ToUpper()
					)
				);
			}
			else
			{
				list.Add(1070722, "<BIG><BASEFONT COLOR=#FF0000>Voting is currently unavailable.</BIG><BASEFONT COLOR=#FFFFFF>");
			}
		}

		public VoteStone(Serial serial)
			: base(serial)
		{ }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "{0} [Reward {1} gold]", Name, VoteConfig.Instance.DefaultGold);
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version 

			writer.Write(_LabelMessage);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						_LabelMessage = reader.ReadString();
					} break;
			}
		}
	}
}