using System.Collections;

namespace Server.Custom.Games
{
	public class CWTeam : BaseGameTeam
	{

        private Point3D m_Home2;

		public CWTeam( CWGame game, string name ) : base(game, name)
		{
            m_Home2 = Point3D.Zero;
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize(writer);

			writer.Write( 0 );//version

            writer.Write( m_Home2 );
		}

		public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
                    m_Home2 = reader.ReadPoint3D();
					break;
				}
			}
        }

        #region Getters & Setters

        public Point3D Home2 { get { return m_Home2; } set { m_Home2 = value; } }

	    public int KillPoints { get; set; }
        #endregion
	}
}
