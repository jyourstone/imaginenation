using System;

namespace Server.Engines.Harvest
{
    public class BonusHarvestResource
    {
        private readonly TextDefinition m_SuccessMessage;

        public Type Type { get; set; }

        public double ReqSkill { get; set; }

        public double Chance { get; set; }

        public TextDefinition SuccessMessage { get { return m_SuccessMessage; } }

        public void SendSuccessTo(Mobile m)
        {
            TextDefinition.SendMessageTo(m, m_SuccessMessage);
        }

        public BonusHarvestResource(double reqSkill, double chance, TextDefinition message, Type type)
        {
            ReqSkill = reqSkill;

            Chance = chance;
            Type = type;
            m_SuccessMessage = message;
        }
    }
}