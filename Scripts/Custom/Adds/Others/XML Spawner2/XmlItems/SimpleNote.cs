using System;
using Server.Gumps;

/*
** SimpleNote
** updated 1/3/04
** ArteGordon
** adds a simple item that displays text messages in a scroll gump.  The size can be varied and the note text and text-color can be specified.
** The title of the note and its color can also be set.
*/
namespace Server.Items
{
	public class SimpleNote : Item
	{
		private int m_size = 1;
		private string m_NoteString;
		private string m_TitleString;
		private int m_TextColor = 0x3e8;
		private int m_TitleColor = 0xef0000;  // cyan 0xf70000, black 0x3e8, brown 0xef0000 darkblue 0x7fff

		[Constructable]
		public SimpleNote() : base( 0x14EE )
		{ 
			Name = "A note";
			TitleString = "A note";
		}

		public SimpleNote( Serial serial ) : base( serial )
		{ 
		} 
      
		[CommandProperty( AccessLevel.GameMaster )]
		public string NoteString
		{
				get{ return m_NoteString; }
			set { m_NoteString = value; InvalidateProperties();}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string TitleString
		{
				get{ return m_TitleString; }
			set { m_TitleString = value; InvalidateProperties();}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Size
		{
				get{ return m_size; }
			set 
			{
				m_size = value;
				if(m_size < 1) m_size = 1;
				InvalidateProperties();}
		}
      
		[CommandProperty( AccessLevel.GameMaster )]
		public int TextColor
		{
				get{ return m_TextColor; }
			set { m_TextColor = value; InvalidateProperties();}
		}
      
		[CommandProperty( AccessLevel.GameMaster )]
		public int TitleColor
		{
				get{ return m_TitleColor; }
			set { m_TitleColor = value; InvalidateProperties();}
		}

		public override void Serialize( GenericWriter writer )
		{ 
			base.Serialize( writer ); 

			writer.Write( 0 ); // version 
         
			writer.Write( m_NoteString );
			writer.Write( m_TitleString );
			writer.Write( m_TextColor );
			writer.Write( m_TitleColor );
			writer.Write( m_size );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_NoteString = reader.ReadString();
					m_TitleString = reader.ReadString();
					m_TextColor = reader.ReadInt();
					m_TitleColor = reader.ReadInt();
					m_size = reader.ReadInt();
				}
					break;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			SimpleNoteGump g = new SimpleNoteGump( this );
			from.SendGump( g );
		}
	}

	public class SimpleNoteGump : Gump
	{
		private SimpleNote m_Note;
        
		public static string HtmlFormat( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0}>{1}</BASEFONT>", color, text);
		}

		public SimpleNoteGump( SimpleNote note ) : base( 0, 0 )
		{
			m_Note = note;

			AddPage( 0 );
			AddAlphaRegion( 40, 41, 225, /*371*/70*note.Size );
			// scroll top
			AddImageTiled( 3, 5, 300, 37, 0x820 );
			// scroll middle, upper portion
			AddImageTiled( 19, 41, 263, 70, 0x821 );
			for(int i=1;i<note.Size;i++)
			{
				// scroll middle , lower portion
				AddImageTiled( 19, 41+70*i, 263, 70, 0x822 );
			}
			// scroll bottom
			AddImageTiled( 20, 111+70*(note.Size-1), 273, 34, 0x823 );
			// title string
			AddHtml( 55, 10, 200, 37, SimpleNoteGump.HtmlFormat( note.TitleString, note.TitleColor), false , false );
			// text string
			AddHtml( 40, 41, 225, 70*note.Size, SimpleNoteGump.HtmlFormat( note.NoteString, note.TextColor ), false , false );
		}
	}
}
