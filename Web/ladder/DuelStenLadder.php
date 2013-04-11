
<? /******************************************************************************************************************
				Ladder Duel Stones
				--------------------------------
				Erik Eliasson 2006 ©

**********************************************************************************************************************/?>


<link href="http://teamspeak.xs4all.nl/Ladder/styles.css" rel="stylesheet" type="text/css">
<?PHP
include("db.php");

class DuelStoneLadder
{
	var $players;
	var $database;
	var $ord;
	var $lim;

	function DuelStoneLadder($db, $limit, $order)
	{
		$limit=mysql_real_escape_string($limit);
		$order=mysql_real_escape_string($order);

		if($order==""){ $this->ord="Points"; }
		else if($order=="Name"){ $this->ord="Name"; }
		else if($order=="Points"){ $this->ord="Points"; }
		else if($order=="Won"){ $this->ord="Won"; }
		else if($order=="Lost"){ $this->ord="Lost"; }
		else if($order=="MoneyEarned"){ $this->ord="MoneyEarned"; }
		else{ DIE("An error occoured"); }
	
		if(is_numeric($limit)){ $this->lim=$limit; }
		else { die("Don't try that shit..."); }

		$this->database=$db;
		if($order=="Name")
		{
			$this->players=$this->database->fromDB("SELECT * FROM duelstones ORDER BY $this->ord LIMIT $this->lim, 20");
		}
		else 
		{
			$this->players=$this->database->fromDB("SELECT * FROM duelstones ORDER BY $this->ord DESC LIMIT $this->lim, 20");
		}
	}

	function Order_Link($name, $word)
	{
		if($word=="")
		{
			$extend="";
		}
		else
		{
			$extend="&order=".$word;
		}

		$retval="<a href=\"http://teamspeak.xs4all.nl/ladder/duelstenladder.php?limit=".$this->lim.$extend."\">".$name."</a>";
		return $retval;
	}
		

	function printLadder($tableattr, $trattr, $tdattr)
	{
		$rows=$this->database->getNumRows("SELECT * FROM duelstones ORDER BY $this->ord DESC LIMIT $this->lim, 20")+2;
		$rows--;

		if(count($this->players)==0){ return; }

		$i=1+$this->lim;
		echo "\n<br>";
		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\">\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\">".$this->Order_Link("Rank", "")."</td>";
		echo "<td class=\"column\" style=\"width: 120px\">".$this->Order_Link("Name", "Name")."</td>";
		echo "<td class=\"column\">".$this->Order_Link("Points", "Points")."</td>";
		echo "<td class=\"column\">".$this->Order_Link("Won", "Won")."</td>";
		echo "<td class=\"column\">".$this->Order_Link("Lost", "Lost")."</td>";
		echo "<td class=\"column\">Win %</td>";
		echo "<td class=\"column\">".$this->Order_Link("Balance", "MoneyEarned")."</td>\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		foreach($this->players as $player)
		{
			$rank="";
			if($this->ord=="Points")
			{
				$rank=$i++;
			}
			else 
			{
				$rank.=$i++;
				$rank.="(";
				$rank.=$this->get_rank($player);
				$rank.=")";
			}
			echo "\t<tr ".$trattr.">\n";
			echo "\t\t<td ".$tdattr.">".$rank."</td><td ".$tdattr.">".link_to_mobile($player)."</td><td ".$tdattr." align=\"right\">".$player['Points']."</td><td ".$tdattr." align=\"right\">".$player['Won']."</td><td ".$tdattr." align=\"right\">".$player['Lost']."</td><td ".$tdattr." align=\"right\">".$this->winprocent($player['Won'], $player['Lost'])."%</td><td align=\"right\" ".$tdattr.">".roundMoneyEarned($player['MoneyEarned'])."</td>\n";
			echo "\t</tr>\n";
		}
		//echo "\t<tr class=\"lastrow\">\n";
		//echo "\t\t<td colspan=7></td>\n";
		//echo "\t</tr>\n";
		echo "</table>\n";
	}

	function get_rank($player)
	{
		$ranksearch=$this->database->fromDB("SELECT * FROM duelstones ORDER BY Points DESC");
		$retval=array_search($player, $ranksearch);
		$retval+=1;
		return ($retval);
	}

	function winprocent($won, $lost)
	{
		if(($lost+$won)>0){
			$retval=round(100*($won/($lost+$won)), 0);
			return $retval;
		}
		else{ return 0; }
	}

	function show_next()
	{
		$lower=0;
		$low=1;
		$upper=20;
		$rows=$this->database->getNumRows("SELECT * FROM duelstones");
		$pages=$rows/20;
		echo "<div align=\"center\">\n";
		for($a=0;$pages-->0;$a++)
		{	
			if($lower==$this->lim)
			{
				$style="style=\"color: black;\"";
			}
			echo "| <a href=\"http://www.in-x.org/ladder/duelstenladder.php?limit=".$lower."&order=".$this->ord."\">".$low."-".$upper."</a> | ";
			$lower+=20;
			$low+=20;
			$upper+=20;
		}
		echo "</div>\n";
	}
}

class Player
{
	var $infoArray;
	var $Mobile;
	var $Name;
	var $Wins;
	var $Losses;
	var $ME;
	var $WP;
	var $Killed;
	var $KilledBy;
	var $Stones;
	var $Points;
	var $database;

	function Player($db, $mobile)
	{
		$mobile=mysql_real_escape_string($mobile);
		$this->database=$db;
		$info=$this->database->fromDB("SELECT * FROM duelstones WHERE Mobile='$mobile'") OR DIE("An error occoured");
		if($this->database->getNumRows("SELECT * FROM duelstones WHERE Mobile='$mobile'")==0){ die("Player doesn't exist"); }

		trim($info[0]['Killed'], ";");
		trim($info[0]['KilledBy'], ";");
		trim($info[0]['StonesUsed'], ";");


		$this->infoArray=$info[0];
		$this->Mobile=$mobile;
		$this->Name=$info[0]['Name'];
		$this->Wins=$info[0]['Won'];
		$this->Losses=$info[0]['Lost'];
		$this->ME=$info[0]['MoneyEarned'];
		if(($this->Wins+$this->Losses)>0)
		{
			$this->WP=round($this->Wins/($this->Wins+$this->Losses)*100, 0);
		}
		else
		{
			$this->WP=0;
		}
		$this->Killed=explode(";", $info[0]['Killed']);
		$this->KilledBy=explode(";", $info[0]['KilledBy']);
		$this->Stones=explode(";", $info[0]['StonesUsed']);
		$this->Points=$info[0]['Points'];
	}

	function writePlayerInfo($tableattr, $ladder)
	{
		$rows=7;
		echo "<table ".$tableattr.">\n";
			echo "\t<tr class=\"firstrow\">\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t\t<td class=\"column\" align=\"center\" style=\"width: 250px;\"><div style=\"float:left;\">".$this->Name;
			echo "</div><div align=\"right\">Rank: ".$ladder->get_rank($this->infoArray)."</div></td>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t</tr>\n";

			writeRow("class=\"middlerow\"", "class=\"column\"", "Points: ".$this->Points);
			writeRow("class=\"middlerow\"", "class=\"column\"", "Duels Won: ".$this->Wins);
			writeRow("class=\"middlerow\"", "class=\"column\"", "Duels Lost: ".$this->Losses);
			writeRow("class=\"middlerow\"", "class=\"column\"", "Balance: ".roundMoneyEarned($this->ME));
			writeRow("class=\"middlerow\"", "class=\"column\"", "Win Percentage: ".$this->WP."%");

			echo "\t<tr>\n";
			echo "\t\t<td></td>\n";
			echo "\t</tr>\n";
		echo "</table>\n";
	}

	function printVersusTable($tableattr, $trattr, $tdattr, $array, $title)
	{
		$rows=count($array)+2;
		$i=0;

		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\">\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\" style=\"width: 150px\" align=\"center\">".$title."</td>";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";
		foreach($array as $versus)
		{	
			if(++$i==count($array)){ break; }
			$info=explode("*", $versus);
			echo "\t<tr ".$trattr.">\n";
			echo "\t\t<td ".$tdattr.">".$info[0]." x ".link_to_mobile($info[1])."</td>\n";
			echo "\t</tr>\n";
		}
		echo "\t<tr>\n";
		echo "\t\t<td></td>\n";
		echo "\t</tr>\n";
		echo "</table>\n";
	}

	function printStones($tableattr, $trattr, $tdattr)
	{
		$rows=count($this->Stones)+2;
		$i=0;

		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\">\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\" style=\"width: 150px\" align=\"center\">Stones Used</td>";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";
		foreach($this->Stones as $stone)
		{	
			if(++$i==count($this->Stones)){ break; }
			$stone=str_replace("*", " x ", $stone);
			echo "\t<tr ".$trattr.">\n";
			echo "\t\t<td ".$tdattr.">".$stone."</td>\n";
			echo "\t</tr>\n";
		}
		echo "\t<tr>\n";
		echo "\t\t<td></td>\n";
		echo "\t</tr>\n";
		echo "</table>\n";
	}

}

class Search
{
	var $database;
	var $table;
	var $cols;
	var $searchword;
	var $results;
	var $order;

	function Search($db, $table, $word)
	{
		$this->database=$db;
		$this->searchword=mysql_real_escape_string($word);
		$this->table=mysql_real_escape_string($table);
	}

	function setRows()
	{
		$this->cols=func_get_args();
	}

	function GetResults($ord)
	{
		$this->order=mysql_real_escape_string($ord);
		$sql="SELECT * FROM ".$this->table." WHERE ";
		//foreach($this->cols as $col)
		//{
			$sql.=$this->cols[1]." LIKE '%".$this->searchword."%' ";
		//}
		$sql.="ORDER BY $this->order";
		$this->results=$this->database->fromDB($sql);
	}

	function printResults()
	{
		if(count($this->results)==0){ DIE("No match found."); }
		if($this->cols[0]=="Name")
		{ 
			$for="Players";
			$num=count($this->results);
			echo $num." results searching ".$for." for \"".$this->searchword."\".<br>";

			foreach($this->results as $result)
			{
				echo link_to_mobile($result);
				echo "<br>";
			}
		}
		else if($this->cols[0]=="KilledBy")
		{
			foreach($this->results as $result)
			{
				echo link_to_mobile($result)." has killed<br>";
				$players=explode(";", $result['Killed']);
				if(count($players)==1 || count($players)==0 || !is_array($players)){ echo "No one<br><br>"; continue;}

				foreach($players as $player)
				{
					if($players[count($players)-1]==$player){ break; }
					$player=explode("*", $player);
					echo $player[0]." x ".link_to_mobile($player[1])."<br>";
				}
				echo "<br>";
			}
		}
		else if($this->cols[0]=="Killed")
		{
			foreach($this->results as $result)
			{
				echo link_to_mobile($result)." has been killed by<br>";
				$players=explode(";", $result['KilledBy']);
				if(count($players)==1 || count($players)==0 || !is_array($players)){ echo "No one<br>"; continue;}
				foreach($players as $player)
				{
					if($players[count($players)-1]==$player){ break; }
					$player=explode("*", $player);
					echo $player[0]." x ".link_to_mobile($player[1])."<br>";
				}
				echo "<br>";
			}
		}
	}
}

function write_search_form()
{
	echo "<form action=\"http://www.in-x.org/ladder/duelstenladder.php?search=true\" method=\"POST\" name=\"Search\" onSubmit=\"if(this.searchword.value==''){ return false; }\">\n";
	echo "<table align=\"center\" style=\"margin-top: 12px;\">\n";
	echo "\t<tr>\n";
	echo "\t\t<td>Search: </td><td><select class=\"inputbox\" name=\"SearchPlace\"><option>Killed</option><option value=\"KilledBy\">Killed By</option><option value=\"Name\">Players</option></select></td>\n";
	echo "\t</tr>\n";
	echo "\t<tr>\n";
	echo "\t<td>Player: </td><td><input type=\"text\" class=\"inputbox\" name=\"searchword\"></td>\n";
	echo "\t</tr>\n";
	echo "\t<tr>\n";
	echo "\t<td></td><td><input class=\"button\" type=\"submit\" value=\"Search\" name=\"submit\"></td>\n";
	echo "\t</tr>\n";
	echo "</table>\n";
	echo "</form>";
}


	
function writeRow($trattr, $tdattr, $content){
	echo"\t<tr ".$trattr.">\n";
		echo"\t\t<td ".$tdattr.">".$content."</td>\n";
	echo"\t</tr>\n";
}

function roundMoneyEarned($me)
{
	$me=round($me/1000, 1)."k";
	return $me;
}

function link_to_mobile($player)
{
	if(is_array($player))
	{
		$retval="<a href=\"http://www.in-x.org/ladder/duelstenladder.php?m=".$player['Mobile']."\">".$player['Name']."</a>";
	}
	else
	{
		$retval="<a href=\"http://www.in-x.org/ladder/duelstenladder.php?m=".$player."\">".get_name($player)."</a>";
	}
	return $retval;
}

function get_name($mobile)
{
	$sql="SELECT Name FROM duelstones WHERE Mobile=$mobile";
	$result=mysql_query($sql) OR DIE("An error occoured");
	$retval=mysql_fetch_array($result, MYSQL_ASSOC);
	return $retval['Name'];
}

$db=new DB("localhost", "root", "schweppes", "pvpladder");
if(isset($_GET['search']))
{
	if($_GET['search']=="true")
	{
		echo "<div align=\"center\">\n";
		$search=new Search($db, "duelstones", $_POST['searchword']);
		$search->setRows($_POST['SearchPlace'], "Name");
		$search->getResults("Points");
		$search->printResults();
		echo "</div>\n";
	}
}
else{
	if(!isset($_GET['order']))
	{
		$_GET['order']="Points";
	}
	if(!isset($_GET['limit']))
	{
		$_GET['limit']=0;
	}

	$ladder=new DuelStoneLadder($db, $_GET['limit'], $_GET['order']);

	if(isset($_GET['m']))
	{
		$ros=new Player($db, $_GET['m']);
		echo "<br>";
		echo "<table align=\"center\" valign=\"top\"><tr>";
			echo "<td valign=\"top\">";
				$ros->writePlayerInfo("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", $ladder);
			echo "</td>";
			echo "<td valign=\"top\">";
				$ros->printVersusTable("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $ros->Killed, "Killed");
			echo "</td>";
			echo "<td valign=\"top\">";
				$ros->printVersusTable("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"", $ros->KilledBy, "Killed By");
			echo "</td>";
			echo "<td valign=\"top\">";
				$ros->printStones("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\" align=\"left\"");
			echo "</td>";
		echo "</tr></table>";
	}
	else
	{
		$ladder->printLadder("cellspacing=1 cellpadding=0 align=\"center\" valign=\"top\"", "class=\"middlerow\"", "class=\"column\"");
		$ladder->show_next();
		write_search_form();
	}
}


?>