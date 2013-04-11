using System.Collections.Generic;

namespace Server.Engines.Craft
{
	public enum CraftMarkOption
	{
		MarkItem,
		DoNotMark,
		PromptForMark
	}

	public class CraftContext
	{
        private readonly List<CraftItem> m_Items;

	    public List<CraftItem> Items { get { return m_Items; } }
	    public int LastResourceIndex { get; set; }

	    public int LastResourceIndex2 { get; set; }

	    public int LastGroupIndex { get; set; }

	    public bool DoNotColor { get; set; }

	    public CraftMarkOption MarkOption { get; set; }

	    public CraftContext()
		{
            m_Items = new List<CraftItem>();
			LastResourceIndex = -1;
			LastResourceIndex2 = -1;
			LastGroupIndex = -1;
		}

		public CraftItem LastMade
		{
			get
			{
				if ( m_Items.Count > 0 )
                    return m_Items[0];

				return null;
			}
		}

		public void OnMade( CraftItem item )
		{
			m_Items.Remove( item );

			if ( m_Items.Count == 10 )
				m_Items.RemoveAt( 9 );

			m_Items.Insert( 0, item );
		}
	}
}