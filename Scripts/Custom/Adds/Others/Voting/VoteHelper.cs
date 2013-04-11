using System;
using Server.Accounting;

namespace Server.Voting
{
	/// <summary>
	/// Exposes methods that make some more complex tasks easier.
	/// </summary>
	public static class VoteHelper
	{
		/// <summary>
		/// Checks to see if the given URL string is valid as a Uri.
		/// </summary>
		/// <param name="url">URL string to check.</param>
		/// <returns>True if the URL string can be constructed by Uri, otherwise; False.</returns>
		public static bool IsValidURL(string url)
		{
			Uri testUrl;

			try { testUrl = new Uri(url); }
			catch { return false; }

			if (testUrl != null)
				return true;

			return false;
		}

		/// <summary>
		/// Gets a string representing the Hours and Minutes of a TimeSpan object.
		/// </summary>
		/// <param name="time">TimeSpan object to read.</param>
		/// <returns>"X hour(s) `and` X minute(s)"</returns>
		public static string GetFormattedTime(TimeSpan time)
		{
			bool useHrs = time.Hours > 0;
			bool useMins = time.Minutes > 0;
			string hrsStr = String.Format("{0} hour{1}", time.Hours, time.Hours > 1 ? "s" : String.Empty);
			string minsStr = String.Format("{0} minute{1}", time.Minutes, time.Minutes > 1 ? "s" : String.Empty);

			return String.Format
			(
				"{0}{1}{2}",
				useHrs ? hrsStr : String.Empty,
				(useHrs && useMins) ? " and " : String.Empty,
				useMins ? minsStr : String.Empty
			);
		}

		/// <summary>
		/// Invokes a new vote request for the given Mobile and VoteSite objects.
		/// </summary>
		/// <param name="from">Request sender.</param>
		/// <param name="site">VoteSite object.</param>
		public static void CastVote(Mobile from, VoteSite site)
		{
			VoteEvents.InvokeVoteRequest(new VoteRequestEventArgs(from, site));
		}

		/// <summary>
		/// Gets the time that the given Mobile last voted for the given VoteSite object.
		/// </summary>
		/// <param name="m">Mobile object.</param>
		/// <param name="voteSite">VoteSite object.</param>
		/// <returns>The time that the given Mobile last voted for the given VoteSite object.</returns>
		public static DateTime GetLastVoteTime(Mobile m, VoteSite voteSite)
		{
			bool canVote;
			TimeSpan timeLeft;
			return GetLastVoteTime(m, voteSite, out canVote, out timeLeft);
		}

		/// <summary>
		/// Gets the time that the given Mobile last voted for the given VoteSite object.
		/// </summary>
		/// <param name="m">Mobile object.</param>
		/// <param name="voteSite">VoteSite object.</param>
		/// <param name="canVote">A value indecating whether the Mobile object is allowed to vote.</param>
		/// <param name="timeLeft">A value indecating the amount of time left before the Mobile is allowed to vote again.</param>
		/// <returns>The time that the given Mobile last voted for the given VoteSite object, with extra output information.</returns>
		public static DateTime GetLastVoteTime(Mobile m, VoteSite voteSite, out bool canVote, out TimeSpan timeLeft)
		{
			DateTime now = DateTime.Now;
			DateTime lastVoteTime = now.Subtract(voteSite.CoolDown);

			canVote = true;
			timeLeft = TimeSpan.Zero;

			if (m == null || m.Deleted)
				return DateTime.Now;

			Account a = (Account)m.Account;

			if (a == null)
				return DateTime.Now;

			string tag = a.GetTag("VS_LAST_VOTE_" + voteSite.Name.ToUpper());

			if (String.IsNullOrEmpty(tag))
			{
				SetLastVoteTime(m, voteSite);
				tag = now.Subtract(voteSite.CoolDown).ToString();
			}

			if (String.IsNullOrEmpty(tag))
				tag = now.ToString();

			bool parsed = DateTime.TryParse(tag, out lastVoteTime);

			if (parsed)
			{
				if (lastVoteTime.Add(voteSite.CoolDown) < now)
				{
					timeLeft = TimeSpan.Zero;
					canVote = true;
				}
				else
				{
					timeLeft = lastVoteTime.Add(voteSite.CoolDown) - now;
					canVote = false;
				}
			}
			else
			{
				lastVoteTime = now.Subtract(voteSite.CoolDown);
				timeLeft = TimeSpan.Zero;
				canVote = true;
			}

			return lastVoteTime;
		}

		/// <summary>
		/// Sets the time of the last vote request sent for the given Mobile and VoteSite objects.
		/// </summary>
		/// <param name="m">Mobile object.</param>
		/// <param name="voteSite">VoteSite object.</param>
		public static void SetLastVoteTime(Mobile m, VoteSite voteSite)
		{
			if (m == null || m.Deleted)
				return;

			if (voteSite == null || !voteSite.Valid)
				return;

			Account a = (Account)m.Account;

			if (a == null)
				return;

			DateTime now = DateTime.Now;

			string tag = now.ToString();

			a.SetTag("VS_LAST_VOTE_" + voteSite.Name.ToUpper(), tag);
		}

		/// <summary>
		/// Gets a value indecating whether the given Mobile object can vote for the given VoteSite object.
		/// </summary>
		/// <param name="m">Mobile object.</param>
		/// <param name="site">VoteSite object.</param>
		/// <returns>True if the Mobile can vote, otherwise; False.</returns>
		public static bool GetCanVote(Mobile m, VoteSite site)
		{
			bool canVote;
			TimeSpan timeLeft;
			GetLastVoteTime(m, site, out canVote, out timeLeft);

			return canVote;
		}

		/// <summary>
		/// Gets the amount of time left before the given Mobile object can vote for the given VoteSite object.
		/// </summary>
		/// <param name="m">Mobile object.</param>
		/// <param name="site">VoteSite object.</param>
		/// <returns>The amount of time left before the given Mobile object can vote for the given VoteSite object.</returns>
		public static TimeSpan GetTimeLeft(Mobile m, VoteSite site)
		{
			bool canVote;
			TimeSpan timeLeft;
			GetLastVoteTime(m, site, out canVote, out timeLeft);

			return timeLeft;
		}
	}
}
