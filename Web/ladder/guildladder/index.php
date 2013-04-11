<STYLE>
.column{ padding-left: 3px; padding-right: 3px;}
.lastrow{ background-image: url('http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_sf.gif'); height:14px; }
.firstrow{ background-image: url('http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_sf.gif'); height:26px; font-size: 14px; color: #ffffff;}
.firstrow a:active, .firstrow a:link, .firstrow a:visited { color: #ffffff; text-decoration: none;}
.firstrow a:hover{ text-decoration: underline;}
.firstrow td { color: #ffffff; font-weight: bold; vertical-align: middle;}
.middlerow{ background-color: #d7cfba; color: #000000; font-size: 13px;}
.middlerow td{ background-color: #d7cfba; color: #000000; font-size: 10px;}
.middlerow a:active, .middlerow a:link, .middlerow a:visited { text-decoration: none; color: #000000; }
.middlerow a:hover{ text-decoration: underline; color: #000000; }
</STYLE>
<link href="http://inx.xs4all.nl/ladder/styles.css" rel="stylesheet" type="text/css">
<?PHP

include_once("db.php");
include_once("GuildLadder.php");
include_once("GuildClass.php");
include_once("PlayerLadder.php");

$_SELF="http://inx.xs4all.nl/ladder/guildladder/index.php?pid=41";
		
$db=new DB("localhost", "database", "user", "password");
$Header="<center><a href=\"{$_SELF}&p=1\">Guildmember Ladder</a> | <a href=\"{$_SELF}\">Guild Ladder</a></center><br>";
echo $Header;
if(isset($_GET['gid']))
{
	$Guild=new Guild($db, $_GET['gid'], $_GET['ad'], $_GET['order']);
	$Guild->getMembers();
	echo "<table align=center><tr><td>";
	$Guild->GuildInfo();
	echo "</td><td valign=top>";
	$Guild->writePlayersInfo();
	echo "</td></tr></table>";

}
else if(isset($_GET['mid']))
{
	$GM=new GuildMember($_GET['mid'], $db);
	echo "<table align=center><tr><td valign=top>";
	$GM->MemberInfo();
	echo "</td><td valign=top>";

	$GM->printVersusTable("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\" style=\"padding-bottom: 10px;\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $GM->DuelStoneKills, "Duelstone Kills");
	$GM->printVersusTableKB("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $GM->dKilledBy, "Duelstone KilledBy");

	echo "</td><td valign=top>";

	$GM->printVersusTable("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\" style=\"padding-bottom: 10px;\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $GM->OtherKills, "Other kills");
	$GM->printVersusTableKB("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $GM->oKilledBy, "Other KilledBy");

	echo "</td><td valign=top>";
	echo "</td></tr></table>";
}
else if(isset($_GET['p']))
{
	$PL=new PlayerGLadder($db, $_GET['order'], $_GET['ad']);
	$PL->printLadder("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\"");

}
else
{
	$GuildLadder=new GuildLadder($db, "0", $_GET['order'], $_GET['ad']);
	$GuildLadder->printLadder("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\"");
}

?>