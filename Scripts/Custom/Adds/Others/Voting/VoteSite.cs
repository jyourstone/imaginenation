using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Misc;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Voting
{
	/// <summary>
	/// VoteSite object.
	/// </summary>
	[PropertyObject]
	public class VoteSite
	{
		private VoteItem _Parent;
		/// <summary>
		/// Gets the VoteItem object that holds this VoteSite object.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public VoteItem Parent
		{ get { return _Parent; } }

		private string _Name = VoteConfig.Instance.DefaultName;
		/// <summary>
		/// Gets or Sets the Name of this VoteSite object.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public string Name
		{
			get { return _Name; }
			set
			{
				_Name = value;
				InvalidateProperties();
			}
		}

		private string _URL = VoteConfig.Instance.DefaultURL;
		/// <summary>
		/// Gets or Sets the URL of this VoteSite object.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public string URL
		{
			get { return _URL; }
			set
			{
				_URL = value;
				InvalidateProperties();
			}
		}

		private TimeSpan _CoolDown = VoteConfig.Instance.DefaultCoolDown;
		/// <summary>
		/// The CoolDown delay between vote requests. 
		/// A new request will be rejected if the request sender has previously made a vote request within this amount of time.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan CoolDown
		{
			get { return _CoolDown; }
			set
			{
				_CoolDown = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Valid
		{
			get { return VoteHelper.IsValidURL(_URL); }
		}

		public VoteSite(VoteItem parent)
		{
			_Parent = parent;
		}

		public VoteSite(VoteItem parent, string name)
		{
			_Parent = parent;
			_Name = name;
		}

		public VoteSite(VoteItem parent, string name, string url)
		{
			_Parent = parent;
			_Name = name;
			_URL = url;
		}

		public void InvalidateProperties()
		{
			if (_Parent == null || _Parent.Deleted)
				return;

			_Parent.InvalidateProperties();
		}

		public virtual void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //Version

			writer.Write((Item)_Parent);
			writer.Write((string)_Name);
			writer.Write((string)_URL);
			writer.Write((TimeSpan)_CoolDown);
		}

		public virtual void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt(); //Version

			switch (version)
			{
				case 0:
					{
						_Parent = (VoteItem)reader.ReadItem();
						_Name = (string)reader.ReadString();
						_URL = (string)reader.ReadString();
						_CoolDown = (TimeSpan)reader.ReadTimeSpan();
					} break;
			}
		}

		public override string ToString()
		{
			return _Name;
		}
	}
}
