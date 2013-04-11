using System;

namespace Server.Engines.Quests
{
	public class QuestRestartInfo
	{
	    private DateTime m_RestartTime;

	    public Type QuestType { get; set; }

	    public DateTime RestartTime
		{
			get{ return m_RestartTime; }
			set{ m_RestartTime = value; }
		}

		public void Reset( TimeSpan restartDelay )
		{
			if ( restartDelay < TimeSpan.MaxValue )
				m_RestartTime = DateTime.Now + restartDelay;
			else
				m_RestartTime = DateTime.MaxValue;
		}

		public QuestRestartInfo( Type questType, TimeSpan restartDelay )
		{
			QuestType = questType;
			Reset( restartDelay );
		}

		public QuestRestartInfo( Type questType, DateTime restartTime )
		{
			QuestType = questType;
			m_RestartTime = restartTime;
		}
	}
}