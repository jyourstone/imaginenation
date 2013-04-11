namespace Server.Engines.PartySystem
{
	public class PartyMemberInfo
	{
		private readonly Mobile m_Mobile;

	    public Mobile Mobile{ get{ return m_Mobile; } }
	    public bool CanLoot { get; set; }

	    public PartyMemberInfo( Mobile m )
		{
			m_Mobile = m;
			CanLoot = !Core.ML;
		}
	}
}