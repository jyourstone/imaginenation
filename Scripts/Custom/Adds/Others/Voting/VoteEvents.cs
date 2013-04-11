using System;

namespace Server.Voting
{
	/// <summary>
	/// Provides various events relative to the voting system.
	/// </summary>
	public static class VoteEvents
	{
		private static event VoteRequestHandler InternalVoteRequest = new VoteRequestHandler(OnVoteRequest);

		/// <summary>
		/// This event is raised when a new vote request is made.
		/// </summary>
		public static event VoteRequestHandler VoteRequest;

		private static void OnVoteRequest(VoteRequestEventArgs e)
		{
			Mobile from = e.Sender;
			VoteSite voteSite = e.VoteSite;

			if (from == null || from.Deleted)
				return;

			if (voteSite.Valid)
			{
				if (e.CanVote)
				{
					if (voteSite.Parent.OnBeforeVote(from))
					{
						voteSite.Parent.OnVote(from, VoteStatus.Success);
						voteSite.Parent.OnAfterVote(from, VoteStatus.Success);
					}
					else
					{
						voteSite.Parent.OnVote(from, VoteStatus.Custom);
						voteSite.Parent.OnAfterVote(from, VoteStatus.Custom);
					}
				}
				else
				{
					voteSite.Parent.OnVote(from, VoteStatus.TooEarly);
					voteSite.Parent.OnAfterVote(from, VoteStatus.TooEarly);
				}
			}
			else
			{
				voteSite.Parent.OnVote(from, VoteStatus.Invalid);
				voteSite.Parent.OnAfterVote(from, VoteStatus.Invalid);
			}
		}

		/// <summary>
		/// Invokes the VoteRequest event with the specified EventArgs.
		/// </summary>
		/// <param name="e">VoteRequestEventArgs object.</param>
		public static void InvokeVoteRequest(VoteRequestEventArgs e)
		{
			if (InternalVoteRequest != null && e != null)
				InternalVoteRequest.Invoke(e);

			if (VoteRequest != null && e != null)
				VoteRequest.Invoke(e);
		}
	}

	/// <summary>
	/// Vote Request Event Handler
	/// </summary>
	/// <param name="e">VoteRequestEventArgs object.</param>
	public delegate void VoteRequestHandler(VoteRequestEventArgs e);

	/// <summary>
	/// VoteRequestEventArgs object.
	/// </summary>
	public sealed class VoteRequestEventArgs : EventArgs
	{
		private readonly Mobile _Sender;
		/// <summary>
		/// The Mobile reference to the request sender.
		/// </summary>
		public Mobile Sender { get { return _Sender; } }

		private readonly VoteSite _VoteSite;
		/// <summary>
		/// The VoteSiteProfile used by the request.
		/// </summary>
		public VoteSite VoteSite { get { return _VoteSite; } }

		/// <summary>
		/// Gets the total time left before the sender of this request can vote again.
		/// </summary>
		public TimeSpan TimeLeft { get { return GetTimeLeft(); } }

		/// <summary>
		/// Gets the time of the last successful vote request for this request sender.
		/// </summary>
		public DateTime LastVoteTime { get { return GetLastVoteTime(); } }

		/// <summary>
		/// Gets a value indicating whether this request sender may vote again.
		/// </summary>
		public bool CanVote { get { return GetCanVote(); } }

		/// <summary>
		/// Creates a new instance of VoteRequestEventArgs.
		/// </summary>
		/// <param name="sender">The Mobile reference to the request sender.</param>
		/// <param name="voteSite">The VoteSiteProfile used by the request.</param>
		public VoteRequestEventArgs(Mobile sender, VoteSite voteSite)
		{
			_Sender = sender;
			_VoteSite = voteSite;
		}

		public void SetLastVoteTime()
		{
			VoteHelper.SetLastVoteTime(_Sender, _VoteSite);
		}

		public DateTime GetLastVoteTime()
		{
			return VoteHelper.GetLastVoteTime(_Sender, _VoteSite);
		}

		public TimeSpan GetTimeLeft()
		{
			bool canVote;

			return GetTimeLeft(out canVote);
		}

		public TimeSpan GetTimeLeft(out bool canVote)
		{
			TimeSpan timeLeft;
			VoteHelper.GetLastVoteTime(_Sender, _VoteSite, out canVote, out timeLeft);

			return timeLeft;
		}

		public bool GetCanVote()
		{
			TimeSpan timeLeft;

			return GetCanVote(out timeLeft);
		}

		public bool GetCanVote(out TimeSpan timeLeft)
		{
			bool canVote;
			VoteHelper.GetLastVoteTime(_Sender, _VoteSite, out canVote, out timeLeft);

			return canVote;
		}
	}
}
