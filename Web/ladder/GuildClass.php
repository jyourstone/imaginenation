<?PHP


/******************************************************************************************************

			GuildKlass


*******************************************************************************************************/


include_once("db.php");
include_once("functions.php");

class Guild
{
	var $Name;
	var $GuildID;
	var $Members;	//Members[0]=Guildmaster
	var $Abbr;
	var $Type;
	var $DuelStoneKills;
	var $DuelStoneDeaths;
	var $OtherKills;
	var $OtherDeaths;
	var $TourPoints;
	var $DuelStonePoints;
	var $TotalPoints;
	var $TotalKills;
	var $TotalDeaths;
	var $Database;
	var $SortType;
	var $PlayerOrder;

	function Guild($db, $id, $st="", $playerOrder="")
	{
		$this->Database = $db;
		$info = $this->Database->fromDB("SELECT * FROM guilds WHERE guildID='$id'");

		$this->Name = $info[0]['name'];

		$guildmaster = $info[0]['guildmaster'];
		$gm = $this->Database->fromDB("SELECT * FROM guildmembers WHERE mobile='$guildmaster'");
		$this->Members['guildmaster'] = $gm[0];

		$this->Abbr = $info[0]['abbrevation'];
		$this->Type = $info[0]['type'];
		$this->DuelStoneKills = $info[0]['duelStoneKills'];
		$this->DuelStoneDeaths = $info[0]['duelStoneDeaths'];
		$this->OtherKills = $info[0]['otherKills'];
		$this->OtherDeaths = $info[0]['otherDeaths'];
		$this->TourPoints = $info[0]['tourPoints'];
		$this->TotalKills = $info[0]['totalKills'];
		$this->TotalDeaths = $info[0]['totalDeaths'];
		$this->DuelStonePoints = $info[0]['duelStonePoints'];
		$this->TotalPoints = $info[0]['totalPoints'];
		$this->GuildID = $id;
		$this->SortType=mysql_real_escape_string($st);
		$this->PlayerOrder=mysql_real_escape_string($playerOrder);

	}

	function getMembers()
	{
		if($this->PlayerOrder=="P")
		{
			$SQL="SELECT * FROM guildmembers WHERE guildID='$this->GuildID'";
			if($this->Database->getNumRows($SQL)==0){ return; }
			$this->Members = $this->Database->fromDB($SQL);
			$this->OrderByPercentage();
			return;
		}
		else if(!($this->PlayerOrder==""))
		{
			unset($this->Members);
			$SQL="SELECT * FROM guildmembers WHERE guildID='$this->GuildID' ORDER BY $this->PlayerOrder $this->SortType";	
			
		}
		else
		{
			$gmMobile=$this->Members['guildmaster']['mobile'];
			$SQL="SELECT * FROM guildmembers WHERE guildID='$this->GuildID' AND mobile<>'$gmMobile'";
		}

		if($this->Database->getNumRows($SQL)==0){ return; }
		$members = $this->Database->fromDB($SQL);

		if($this->PlayerOrder=="")
		{
			for($i=0; $i < count($members); $i++)
			{
				$this->Members[$i] = $members[$i];
			}
		}
		else
		{
			$this->Members=$members;
		}
	}

	function printGuildInfo($Order, $Rank, $trattr, $tdattr)
	{

		if(count($this->Members)==1)
		{
			$this->getMembers($this->GuildID);
		}

		$rank="";
		$rank.=$Rank;

		if($Order!="totalPoints ASC" && $Order!="totalPoints DESC")
		{
			$rank.="(";
			$rank.=$this->get_rank("totalPoints");
			$rank.=")";
		}

		echo "\t<tr ".$trattr.">\n";
		echo "\t\t<td ".$tdattr.">#".$rank."</td>";
		echo "<td ".$tdattr.">".link_to_guild($this->GuildID, "name")." [".$this->Abbr."]</td>";
		echo "<td ".$tdattr." align=\"left\">".link_to_mobile($this->Members['guildmaster']['mobile'])."</td>";
		//echo "<td ".$tdattr." align=\"left\">[".$this->Abbr."]</td>";
		echo "<td ".$tdattr." align=\"left\">".$this->Type."</td>";
		echo "<td ".$tdattr." align=\"right\">".count($this->Members)."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->DuelStoneKills."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->DuelStoneDeaths."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->OtherKills."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->OtherDeaths."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->TourPoints."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->DuelStonePoints."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->TotalPoints."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->TotalKills."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->TotalDeaths."</td>\n";
		echo "\t</tr>\n";
	}

	function get_rank($order)
	{
		$guildarray=$this->Database->fromDB("SELECT * FROM guilds WHERE guildID='$this->GuildID'");
		$ranksearch=$this->Database->fromDB("SELECT * FROM guilds ORDER BY $order DESC");
		$retval=array_search($guildarray[0], $ranksearch);
		$retval+=1;
		return ($retval);
	}

	function writePlayersInfo()
	{
		$rows=count($this->Members)+2;
		echo "<table>\n";
			echo "\t<tr>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t\t<td class=\"begin_end_playerstat\">".$this->Order_Link_PS("<font color=ffffff>Name</font>", "name")."</td>\n";
			echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\">".$this->Order_Link_PS("Total Points", "totalPoints")."</td>\n";
			echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\">".$this->Order_Link_PS("Kills", "amountKilled")."</td>\n";
			echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\">".$this->Order_Link_PS("Deaths", "amountDied")."</td>\n";
			echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\">".$this->Order_Link_PS("Kills/Total", "P")."</td>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t</tr>\n";

			foreach($this->Members as $Member){
				$this->playerStatsShort($Member['mobile'], $Member['totalPoints'], $Member['amountKilled'], $Member['amountDied']);
			}
			echo "\t<tr>\n";
			echo "\t\t<td height=10 colspan=5 class=\"end\"></td>\n";
			echo "\t</tr>\n";

		echo "</table>\n";
	}

	function playerStatsShort($Mobile, $TotalPoints, $AmountKilled, $AmountDied)
	{
		echo"\t<tr>\n";
		echo"\t\t<td class=\"playerstat_event\">".link_to_mobile($Mobile)."</td>\n";

		echo"\t\t<td class=\"playerstat_event\" align=\"right\">".$TotalPoints."</td>\n";
		echo"\t\t<td class=\"playerstat_event\" align=\"right\">".$AmountKilled."</td>\n";
		echo"\t\t<td class=\"playerstat_event\" align=\"right\">".$AmountDied."</td>\n";
		if(($AmountKilled+$AmountDied)>0)
		{
			$Perc=round($AmountKilled/($AmountKilled+$AmountDied),2)*100;
		}
		else{ $Perc=0;}

		echo"\t\t<td class=\"playerstat_event\" align=\"right\">".$Perc."%</td>\n";
		echo"\t</tr>\n";
	}

	function Order_Link_PS($name, $word)
	{
		if($word=="")
		{
			$extend="";
		}
		else
		{
			if(isset($this->SortType))
			{
				if($this->SortType=="desc" && $word==$this->PlayerOrder)
				{ $ad="asc"; } 
				else{ $ad="desc";}
			}
			$extend="&order=".$word."&ad=".$ad;
		}

		$retval="<a href=\"http://www.in-x.org/ladder/guildladder/index.php?pid=41&gid={$this->GuildID}".$extend."\">".$name."</a>";
		return $retval;
	}

	function OrderByPercentage()
	{
		for($j=0;$j<count($this->Members); $j++)
		{
			if(($this->Members[$j]['amountKilled']+$this->Members[$j]['amountDied'])>0)
			{
				$PERC=$this->Members[$j]['amountKilled']/($this->Members[$j]['amountKilled']+$this->Members[$j]['amountDied']);
			}
			else
			{
				$PERC=0;
			}
			$this->Members[$j]['P']=round($PERC,1)*100;
		}

		for($i=count($this->Members), $plats=0, $temp, $j=0; --$i>0; $plats=0, $j=0)
		{
			while(++$j<=$i)
			{
				if(isset($this->SortType))
				{

					if($this->SortType=="asc")
					{
						if($this->Members[$plats]['P'] < $this->Members[$j]['P'])
						{
							$plats=$j;
						}
					}
					else
					{
						if($this->Members[$plats]['P'] > $this->Members[$j]['P'])
						{
							$plats=$j;
						}
					}
				}

				$temp=$this->Members[$i];
				$this->Members[$i]=$this->Members[$plats];
				$this->Members[$plats]=$temp;
			}
		}
	}

	function GuildInfo()
	{
		$rows=15;
	
		if($array['Joined']!=0){$winp=round(100*($array['Won'] / $array['Joined']),0);}else{$winp=0;}
		echo "<table>\n";
			echo "\t<tr>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t\t<td class=\"begin\"><img style=\"float: left;\" src=\"http://www.in-x.org/mkportal/templates/skylight2/images/m_sx.gif\"><div style=\"width:200px;float:left\" align=\"left\">".$this->Name."</div><div style=\"width: 32px;float: left;\" align=\"right\">#".$this->get_rank("totalPoints")."</div><img src=\"http://www.in-x.org/mkportal/templates/skylight2/images/m_dx.gif\">";
			echo "</td>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t</tr>\n";

			write_row("Guildmaster: ".$this->Members['guildmaster']['name']);
			write_row("Abbrevation: ".$this->Abbr);
			write_row("Type: ".$this->Type);
			write_row("Members: ".count($this->Members));
			write_row("Duelstone Kills: ".$this->DuelStoneKills);
			write_row("Duelstone Deaths: ".$this->DuelStoneDeaths);
			write_row("Other Kills: ".$this->OtherKills);
			write_row("Other Deaths: ".$this->OtherDeaths);
			write_row("Tournament Points: ".$this->TourPoints);
			write_row("Duelstone Points: ".$this->DuelStonePoints);
			write_row("Total Points: ".$this->TotalPoints);
			write_row("Total Kills: ".$this->TotalKills);
			write_row("Total Deaths: ".$this->TotalDeaths);

			echo "\t<tr>\n";
			echo "\t\t<td height=10 class=\"end\"></td>\n";
			echo "\t</tr>\n";

		echo "</table>\n";

	}
}

?>