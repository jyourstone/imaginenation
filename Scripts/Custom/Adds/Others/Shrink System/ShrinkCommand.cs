#region AuthorHeader
//
//	Shrink System version 2.1, by Xanthos
//
//
#endregion AuthorHeader

using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Spells;
using Server.Targeting;
using Xanthos.Interfaces;

namespace Xanthos.ShrinkSystem
{
	public class ShrinkCommands
	{
		private static bool m_LockDown; // TODO: need to persist this.

		public static void Initialize()
		{
			CommandHandlers.Register( "Shrink", AccessLevel.Counselor, Shrink_OnCommand );
			CommandHandlers.Register( "ShrinkLockDown", AccessLevel.Administrator, ShrinkLockDown_OnCommand );
			CommandHandlers.Register( "ShrinkRelease", AccessLevel.Administrator, ShrinkRelease_OnCommand );
		}

		public static bool LockDown
		{
			get { return m_LockDown; }
		}

		[Usage( "Shrink" )]
		[Description( "Shrinks a creature." )]
		private static void Shrink_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				from.Target = new ShrinkTarget( from, null, true );
		}

		[Usage( "ShrinkLockDown" )]
		[Description( "Disables all shrinkitems in the world." )]
		private static void ShrinkLockDown_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				SetLockDown( from, true );
		}

		[Usage( "ShrinkRelease" )]
		[Description( "Re-enables all disabled shrink items in the world." )]
		private static void ShrinkRelease_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				SetLockDown( from, false );
		}

		static private void SetLockDown( Mobile from, bool lockDown )
		{
			if ( m_LockDown = lockDown )
			{
				World.Broadcast( 0x35, true, "A server wide shrinkitem lockout has initiated." );
				World.Broadcast( 0x35, true, "All shrunken pets have will remain shrunken until further notice." );
			}
			else
			{
				World.Broadcast( 0x35, true, "The server wide shrinkitem lockout has been lifted." );
				World.Broadcast( 0x35, true, "You may once again unshrink shrunken pets." );
			}
		}
	}

	public class ShrinkTarget : Target
	{
		private readonly IShrinkTool m_ShrinkTool;
		private readonly bool m_StaffCommand;

		public ShrinkTarget( Mobile from, IShrinkTool shrinkTool, bool staffCommand ) : base( staffCommand ? 12 : 3, false, TargetFlags.None )
		{
			m_ShrinkTool = shrinkTool;
			m_StaffCommand = staffCommand;
			from.SendMessage( "What do you wish to shrink?" );
		}

		protected override void OnTarget( Mobile from, object target )
		{
		    ShrinkPotion pot = null;
		    HitchingPost post = null;

            if (m_ShrinkTool is ShrinkPotion)
                pot = (ShrinkPotion)m_ShrinkTool;
            else if (m_ShrinkTool is HitchingPost)
                post = (HitchingPost) m_ShrinkTool;


            if (!m_StaffCommand)
            {
                if (pot != null)
                {
                    if (!from.InRange(pot.GetWorldLocation(), 1))
                    {
                        from.SendAsciiMessage("You are too far away from the shrink potion");
                        return;
                    }
                }
                else if (post != null)
                {
                    if (!from.InRange(post.GetWorldLocation(), 1))
                    {
                        from.SendAsciiMessage("You are too far away from the hitching post");
                        return;
                    }
                }
            }

		    BaseCreature pet = target as BaseCreature;

            if (target == from)
                from.SendMessage("You cannot shrink yourself!");

            else if (target is Item)
                from.SendMessage("You cannot shrink that!");

            else if (target is PlayerMobile)
                from.SendMessage("That person gives you a dirty look!");

            else if (SpellHelper.CheckCombat(from))
                from.SendMessage("You cannot shrink your pet while you are fighting.");

            else if (null == pet)
                from.SendMessage("That is not a pet!");

            else if ((pet.BodyValue == 400 || pet.BodyValue == 401) && pet.Controlled == false)
                from.SendMessage("That person gives you a dirty look!");

            else if (pet.IsDeadPet)
                from.SendMessage("You cannot shrink the dead!");

            else if (pet.Summoned)
                from.SendMessage("You cannot shrink a summoned creature!");

            else if (!m_StaffCommand && pet.Combatant != null && pet.InRange(pet.Combatant, 12) && pet.Map == pet.Combatant.Map)
                from.SendMessage("Your pet is fighting; you cannot shrink it yet.");

            else if (pet.BodyMod != 0)
                from.SendMessage("You cannot shrink your pet while it is polymorphed.");

            else if (!m_StaffCommand && pet.Controlled == false)
                from.SendMessage("You cannot not shrink wild creatures.");

            else if (!m_StaffCommand && pet.ControlMaster != from)
                from.SendMessage("That is not your pet.");

            else if (!m_StaffCommand && ShrinkItem.IsPackAnimal(pet) && (null != pet.Backpack && pet.Backpack.Items.Count > 0))
                from.SendMessage("You must unload this pet's pack before it can be shrunk.");
                //
            else if (target is YoungNightmare)
                from.SendMessage("The creature refuses!");
                //
            else
            {
                if (pet.ControlMaster != from && !pet.Controlled)
                {
                    ISpawner se = pet.Spawner;
                    if (se != null && se.UnlinkOnTaming)
                    {
                        pet.Spawner.Remove(pet);
                        pet.Spawner = null;
                    }

                    pet.CurrentWayPoint = null;
                    pet.ControlMaster = from;
                    pet.Controlled = true;
                    pet.ControlTarget = null;
                    pet.ControlOrder = OrderType.Come;
                    pet.Guild = null;
                    pet.Delta(MobileDelta.Noto);
                }

                IEntity p1 = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z), from.Map);
                IEntity p2 = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 50), from.Map);

                Effects.SendMovingParticles(p2, p1, ShrinkTable.Lookup(pet), 1, 0, true, false, 0, 3, 1153, 1, 0, EffectLayer.Head, 0x100);
                from.PlaySound(492);
                from.AddToBackpack(new ShrinkItem(pet));

                if (!m_StaffCommand && m_ShrinkTool != null)
                {
                    if (m_ShrinkTool.ShrinkCharges > 0)
                        m_ShrinkTool.ShrinkCharges--;

                    if (m_ShrinkTool.ShrinkCharges <= 0 && m_ShrinkTool is ShrinkPotion)
                        ((ShrinkPotion)m_ShrinkTool).Consume(1);
                }
            }
		}
	}
}