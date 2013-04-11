using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Commands.Generic
{
    public class SendRewardGumpCommand : BaseCommand
    {
        public SendRewardGumpCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] { "SendRewardGump", "SRG" };
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "SendRewardGump <skillname>";
            Description = "Send reward gump for specified skill to targeted player.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new SendRewardGumpCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            Mobile target = obj as Mobile;
            Map map = from.Map;

            if (target == null || map == Map.Internal || map == null)
            {
                from.SendAsciiMessage("Invalid target.");
                return;
            }
            if (e.Length == 0)
            {
                from.SendAsciiMessage("Format:");
                from.SendAsciiMessage("SendRewardGump <skillname> OR SendRewardGump bard OR SendRewardGump thieving");
                return;
            }
            if (e.Length == 1)
            {
                if (e.GetString(0).ToLower() == "bard")
                {
                    if (target.Skills.Peacemaking.Base == 100.0 && target.Skills.Discordance.Base == 100.0 && target.Skills.Provocation.Base == 100.0 && target.Skills.Musicianship.Base == 100.0)
                    {
                        target.CloseGump(typeof (ChooseBardRewardGump));
                        target.CloseGump(typeof(RewardGump));
                        target.SendGump(new ChooseBardRewardGump());
                        from.SendAsciiMessage("You sent a bard rewardgump to {0}", target.Name);
                        target.SendAsciiMessage("{0} sent a bard rewardgump to you", from.Name);
                        CommandLogging.WriteLine(from, "{0} {1} sending a bard reward gump to {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(target));
                    }
                    else
                        from.SendAsciiMessage("That player does not have 100 points in all bardic skills");

                    return;
                }
                if (e.GetString(0).ToLower() == "thieving")
                {
                    if (target.Skills.Hiding.Base == 100.0 && target.Skills.Stealth.Base == 100.0 && target.Skills.Snooping.Base == 100.0 && target.Skills.Stealing.Base == 100.0)
                    {
                        target.CloseGump(typeof (RewardGump));
                        target.SendGump(new RewardGump(target.Skills[21].Info, 0, "Name"));
                        from.SendAsciiMessage("You sent a thieving rewardgump to {0}", target.Name);
                        target.SendAsciiMessage("{0} sent a thieving rewardgump to you", from.Name);
                        CommandLogging.WriteLine(from, "{0} {1} sending a thieving reward gump to {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(target));
                    }
                    else
                        from.SendAsciiMessage("That player does not have 100 points in all thieving skills");

                    return;
                }
                SkillName skillname;
                try
                {
                    skillname = (SkillName)Enum.Parse(typeof(SkillName), e.GetString(0), true);
                }
                catch
                {
                    from.SendAsciiMessage("You have specified an invalid skill");
                    return;
                }

                Skill skill = target.Skills[skillname];

                if (target is PlayerMobile)
                {
                    switch (skill.SkillID)
                    {
                        case 0: // Alchemy
                        case 7: // Blacksmithy
                        case 8: // Bowcraft/fletching
                        case 23: // Inscription
                        case 25: // Magery
                        case 34: // Tailoring
                        case 35: // Animal Taming
                        case 44: // Lumberjacking
                        case 45: // Mining
                            if (skill.Base == 100)
                            {
                                target.CloseGump(typeof(RewardGump));
                                target.SendGump(new RewardGump(skill.Info, 0, "Name"));
                                from.SendAsciiMessage("You sent a rewardgump for {0} to {1}", skill.Name, target.Name);
                                target.SendAsciiMessage("{0} sent a rewardgump for {1} to you", from.Name, skill.Name);
                                CommandLogging.WriteLine(from, "{0} {1} sending a reward gump to {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(target));
                            }
                            else
                                from.SendAsciiMessage("That player does not have 100 points in the specified skill");
                            break;
                        case 11: // Carpentry
                            if (skill.Base == 100)
                            {
                                target.CloseGump(typeof(RewardGump));
                                target.CloseGump(typeof (ChooseCarpRewardGump));
                                target.SendGump(new ChooseCarpRewardGump());
                                from.SendAsciiMessage("You sent a rewardgump for {0} to {1}", skill.Name, target.Name);
                                target.SendAsciiMessage("{0} sent a rewardgump for {1} to you", from.Name, skill.Name);
                                CommandLogging.WriteLine(from, "{0} {1} sending a reward gump to {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(target));
                            }
                            else
                                from.SendAsciiMessage("That player does not have 100 points in the specified skill");
                            break;
                        default:
                            from.SendAsciiMessage("That skill does not have a reward");
                            break;
                    }
                }
                else
                {
                    from.SendAsciiMessage("You must target a player.");
                    return;
                }
            }
        }
    }
}