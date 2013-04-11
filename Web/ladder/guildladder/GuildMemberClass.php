<?PHP
/*******************************************************************************************************

			GuildMemberClass


*******************************************************************************************************/

include_once("db.php");

class GuildMember
{
	var $Mobile;
	var $Name;
	var $Title;
	var $DuelStoneKills;
	var $OtherKills;
	var $DuelStonePoints;
	var $TourPoints;
	var $TotalPoints;
	var $AmountKilled;
	var $AmountDied;
	var $Database;
	var $GuildID;
	var $dKilledBy;
	var $oKilledBy;

	function GuildMember($mid, $db)
	{
		$this->Mobile=mysql_real_escape_string($mid);
		$this->Database=$db;
		$info=$this->Database->fromDB("SELECT * FROM guildmembers WHERE mobile='$this->Mobile'");
		
		$this->Name=$info[0]['name'];
		$this->Title=$info[0]['title'];

		trim($info[0]['duelStoneKills'], ";");
		$this->DuelStoneKills=explode(";", $info[0]['duelStoneKills']);

		trim($info[0]['otherKills'], ";");
		$this->OtherKills=explode(";", $info[0]['otherKills']);

		$this->dKilledBy=$this->getKilledBy("duelStoneKills");
		$this->oKilledBy=$this->getKilledBy("otherKills");


		$this->DuelStonePoints=$info[0]['duelStonePoints'];
		$this->TourPoints=$info[0]['tourPoints'];
		$this->TotalPoints=$info[0]['totalPoints'];
		$this->AmountKilled=$info[0]['amountKilled'];
		$this->AmountDied=$info[0]['amountDied'];
		$this->GuildID=$info[0]['guildID'];
	}

	function getKilledBy($field)
	{
		$info=$this->Database->fromDB("SELECT mobile, $field FROM guildmembers WHERE $field LIKE '%$this->Mobile%'");
		$retval=$info;

		$i=0;
		foreach($retval as $string)
		{
			preg_match("/(\d+\*{$this->Mobile})/",$string[$field],$matches);
			$matches[1].="*{$string['mobile']}";
			$KilledBy[$i++]=$matches[1];
		}
			
		return $KilledBy;
	}

	function printPlayerInfo($Order, $Rank, $trattr, $tdattr)
	{

		$rank="";
		$rank.=$Rank;

		if($Order!="totalPoints")
		{
			$rank.="(";
			$rank.=$this->get_rank("totalPoints DESC, amountKilled DESC, amountDied ASC");
			$rank.=")";
		}

		echo "\t<tr ".$trattr.">\n";
		echo "\t\t<td ".$tdattr.">#".$rank."</td>";

		$mobile=$this->Database->fromDB("SELECT * FROM guildmembers WHERE mobile='$this->Mobile'");
		echo "<td ".$tdattr.">".link_to_mobile($mobile[0]['mobile'])." [".link_to_guild($mobile[0]['guildID'], "abbrevation")."]</td>";

		echo "<td ".$tdattr." align=\"left\">".$this->Title."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->DuelStoneKills."</td>";
		//echo "<td align=\"right\" ".$tdattr.">".$this->OtherKills."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->TourPoints."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->DuelStonePoints."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->TotalPoints."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->AmountKilled."</td>";
		echo "<td align=\"right\" ".$tdattr.">".$this->AmountDied."</td>\n";
		echo "\t</tr>\n";
	}

	function get_rank($order)
	{
		$guildarray=$this->Database->fromDB("SELECT * FROM guildmembers WHERE mobile='$this->Mobile'");
		$ranksearch=$this->Database->fromDB("SELECT * FROM guildmembers WHERE amountKilled<>0 OR amountDied<>0 OR totalPoints<>0 ORDER BY $order");
		if(!in_array($guildarray[0], $ranksearch))
		{
			return "Not ranked";
		}
		$retval=array_search($guildarray[0], $ranksearch);
		$retval+=1;
		$retval="#".$retval;
		return ($retval);
	}

	function MemberInfo()
	{
		$rows=9;
	
		//if($array['Joined']!=0){$winp=round(100*($array['Won'] / $array['Joined']),0);}else{$winp=0;}
		echo "<table>\n";
			echo "\t<tr>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t\t<td class=\"begin\"><img style=\"float: left;\" src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_sx.gif\"><div style=\"width:150px;float:left\" align=\"left\">".$this->Name."</div><div style=\"width: 82px;float: left;\" align=\"right\">".$this->get_rank("totalPoints DESC, amountKilled DESC, amountDied ASC")."</div><img src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_dx.gif\">";
			echo "</td>\n";
			echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
			echo "\t</tr>\n";

			write_row("Guild: ".link_to_guild($this->GuildID, "name"));
			write_row("Title: ".$this->Title);
			//write_row("Duelstone Kills: ".$this->DuelStoneKills);
			//write_row("Other Kills: ".$this->OtherKills);
			write_row("Tournament Points: ".$this->TourPoints);
			write_row("Duelstone Points: ".$this->DuelStonePoints);
			write_row("Total Points: ".$this->TotalPoints);
			write_row("Kills: ".$this->AmountKilled);
			write_row("Deaths: ".$this->AmountDied);

			echo "\t<tr>\n";
			echo "\t\t<td height=10 class=\"end\"></td>\n";
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
		if(is_array($array)){
		foreach($array as $versus)
		{	
			if(++$i==count($array)){ break; }
			$info=explode("*", $versus);
			if(exists($info[1])==1)
			{
				echo "\t<tr ".$trattr.">\n";
				echo "\t\t<td ".$tdattr.">".$info[0]." x ".link_to_mobile($info[1])."</td>\n";
				echo "\t</tr>\n";
			}
		}
		}
		echo "\t<tr class=\"lastrow\">\n";
		echo "\t\t<td></td>\n";
		echo "\t</tr>\n";
		echo "</table>\n";
	}

	function printVersusTableKB($tableattr, $trattr, $tdattr, $array, $title)
	{
		$rows=count($array)+2;
		$i=0;

		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\">\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\" style=\"width: 150px\" align=\"center\">".$title."</td>";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";
		if(is_array($array)){
		foreach($array as $versus)
		{	
			//if(++$i==count($array)){ break; }
			$info=explode("*", $versus);
			if(exists($info[2])==1)
			{
				echo "\t<tr ".$trattr.">\n";
				echo "\t\t<td ".$tdattr.">".$info[0]." x ".link_to_mobile($info[2])."</td>\n";
				echo "\t</tr>\n";
			}
		}
		}
		echo "\t<tr class=\"lastrow\">\n";
		echo "\t\t<td></td>\n";
		echo "\t</tr>\n";
		echo "</table>\n";
	}



}

?>