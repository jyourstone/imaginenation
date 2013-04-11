using System.Collections.Generic;
using Server.ContextMenus;
using Server.Multis;
using Server.Spells;

namespace Server.Items
{
    public class SpellScroll : Item, ICommodity
    {
		private int m_SpellID;

        protected virtual int PrecastManaCost { get { return 0; } }

        public virtual int ManaCost { get { return 0; } }

		public int SpellID { get { return m_SpellID; } }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return (Core.ML); } }

		public SpellScroll( Serial serial ) : base( serial )
		{
		}

		[Constructable]
		public SpellScroll( int spellID, int itemID ) : this( spellID, itemID, 1 )
		{
		}

		[Constructable]
		public SpellScroll( int spellID, int itemID, int amount ) : base( itemID )
		{
			Stackable = true;
		    Weight = 0.1;
			Amount = amount;

			m_SpellID = spellID;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_SpellID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_SpellID = reader.ReadInt();

					break;
				}
			}
		}

        public override bool CanEquip(Mobile m)
        {
            //Workaround so that i can use sphere checks.
            return true;
        }

        public override void Consume()
        {
            if (!EventItem || (EventItem && EventItemConsume))
                base.Consume();
        }

        public override void Consume(int amount)
        {
            if (!EventItem || (EventItem && EventItemConsume))
                base.Consume(amount);
        }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive && Movable )
				list.Add( new AddToSpellbookEntry() );
		}

        public override void OnSingleClick(Mobile from)
        {
            string name = Sphere.ComputeName(this);

            if (Amount != 1)
                name = Amount + " " + name + "s";

            LabelTo(from, name);
        }

		public override void OnDoubleClick( Mobile from )
		{
            if (!Sphere.CanUse(from, this))
                return;

			if ( !DesignContext.Check( from ) )
				return; // They are customizing

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			Spell spell = SpellRegistry.NewSpell( m_SpellID, from, this );

            if (spell != null)
            {
                int mana = ManaCost > 0 ? ManaCost : spell.ScaleMana(spell.GetMana());

                if (from.Mana < mana)
                {
                    from.SendAsciiMessage("You lack sufficient mana for this scroll.");
                    return;
                }
                
                if (from.Mana - PrecastManaCost < 0)
                    from.Mana = 0;
                else
                    from.Mana -= PrecastManaCost;

                spell.Cast();
            }
            else
                from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
		}
	}
}