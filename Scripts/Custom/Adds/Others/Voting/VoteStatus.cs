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
	/// Provides a list of states to describe the current status of a vote request.
	/// </summary>
	public enum VoteStatus
	{
		/// <summary>
		/// The request was rejected because something went wrong. 
		/// This can happen when a VoteItem is not set up properly.
		/// </summary>
		Invalid,
		/// <summary>
		/// The request was successful.
		/// </summary>
		Success,
		/// <summary>
		/// The request was rejected because it was made before the CoolDown delay reached zero.
		/// </summary>
		TooEarly,
		/// <summary>
		/// The request was rejected because the VoteItem.OnBeforeVote method returned false.
		/// </summary>
		Custom
	}
}