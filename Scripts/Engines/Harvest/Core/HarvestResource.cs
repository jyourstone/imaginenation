using System;

namespace Server.Engines.Harvest
{
	public class HarvestResource
	{
	    private readonly object m_SuccessMessage;

	    public Type[] Types { get; set; }

	    public double ReqSkill { get; set; }

	    public double MinSkill { get; set; }

	    public double MaxSkill { get; set; }

	    public object SuccessMessage{ get{ return m_SuccessMessage; } }

		public void SendSuccessTo( Mobile m )
		{
			if ( m_SuccessMessage is int )
				m.SendLocalizedMessage( (int)m_SuccessMessage );
			else if ( m_SuccessMessage is string )
				m.SendMessage( (string)m_SuccessMessage );
		}

		public HarvestResource( double reqSkill, double minSkill, double maxSkill, object message, params Type[] types )
		{
			ReqSkill = reqSkill;
			MinSkill = minSkill;
			MaxSkill = maxSkill;
			Types = types;
			m_SuccessMessage = message;
		}
	}
}