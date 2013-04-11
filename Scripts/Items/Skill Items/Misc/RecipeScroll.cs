using Server.Engines.Craft;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class RecipeScroll : Item
	{
		public override int LabelNumber { get { return 1074560; } } // recipe scroll

		private int m_RecipeID;

	    private string RecipeName
	    {
	        get
	        {
                if (m_RecipeID == 1)
                    return "Hell's Halberd";
                if (m_RecipeID == 2)
                    return "Black Widow";
                if (m_RecipeID == 3)
                    return "Judgement Hammer";
                if (m_RecipeID == 4)
                    return "Chu Ko Nu";
                if (m_RecipeID == 5)
                    return "Superior Diamond Katana";
                if (m_RecipeID == 6)
                    return "Dwarven Battle Axe";
                if (m_RecipeID == 7)
                    return "Superior Dragon's Blade";
                if (m_RecipeID == 8)
                    return "Heaven's Fury";
                if (m_RecipeID == 9)
                    return "Soul of the Vampire";

	            return null;
	        }
	    }

	    [CommandProperty( AccessLevel.GameMaster )]
		public int RecipeID
		{
			get { return m_RecipeID; }
			set { m_RecipeID = value; InvalidateProperties(); }
		}

		public Recipe Recipe
		{
			get
			{
				if( Recipe.Recipes.ContainsKey( m_RecipeID ) )
					return Recipe.Recipes[m_RecipeID];

				return null;
			}
		}

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            Recipe r = Recipe;

            if (r != null)
                LabelTo(from, RecipeName);
        }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			Recipe r = Recipe;

            if (r != null)
                list.Add(1049644, RecipeName); //r.TextDefinition.ToString() ); // [~1_stuff~]
		}

		public RecipeScroll( Recipe r )
			: this( r.ID )
		{
		}

		[Constructable]
		public RecipeScroll( int recipeID )
			: base( 0x2831 )
		{
			m_RecipeID = recipeID;
		    LootType = LootType.Blessed;
		}

		public RecipeScroll( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !from.InRange( GetWorldLocation(), 2 ) || !from.InLOS(this) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			Recipe r = Recipe;

			if( r != null && from is PlayerMobile )
			{
				PlayerMobile pm = from as PlayerMobile;

				if( !pm.HasRecipe( r ) )
				{
					bool allRequiredSkills = true;
					double chance = r.CraftItem.GetSuccessChance( from, null, r.CraftSystem, false, ref allRequiredSkills );

					if ( allRequiredSkills && chance >= 0.0 )
					{
                        pm.SendLocalizedMessage(1073451, RecipeName); //r.TextDefinition.ToString()); // You have learned a new recipe: ~RecipeName~
						//pm.SendLocalizedMessage( 1073451, r.TextDefinition.ToString() ); // You have learned a new recipe: ~1_RECIPE~
						pm.AcquireRecipe( r );
						Delete();
					}
					else
					{
						pm.SendLocalizedMessage( 1044153 ); // You don't have the required skills to attempt this item.
					}
				}
				else
				{
					pm.SendLocalizedMessage( 1073427 ); // You already know this recipe.
				}
				
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_RecipeID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						m_RecipeID = reader.ReadInt();

						break;
					}
			}
		}
	}
}
