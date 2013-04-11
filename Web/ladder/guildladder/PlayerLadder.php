<?PHP
/*******************************************************************************************
			Player(guild)ladder


*******************************************************************************************/

include_once("GuildMemberClass.php");

class PlayerGLadder
{
	var $PlayerMobiles;
	var $Database;
	var $OrderBy;
	var $OrderType;
	var $OrderExt;

	function PlayerGLadder($db, $order, $ordertype)
	{
		if(isset($order))
		{
			$this->OrderBy=mysql_real_escape_string($order);
		}
		else
		{
			$this->OrderBy="totalPoints";
		}

		if(isset($ordertype))
		{
			$this->OrderType=mysql_real_escape_string($ordertype);
		}
		else
		{
			$this->OrderType="DESC";
		}
		$this->OrderExt=", amountKilled DESC, amountDied ASC";
		

		$this->Database=$db;
	
		$this->PlayerMobiles=$this->Database->fromDB("SELECT mobile FROM guildmembers WHERE amountKilled<>0 OR amountDied<>0 OR totalPoints<>0 ORDER BY $this->OrderBy $this->OrderType $this->OrderExt");
	}

	function printLadder($tableattr, $trattr, $tdattr)
	{
		$rows=$this->Database->getNumRows("SELECT * FROM guildmembers WHERE amountKilled<>0 OR amountDied<>0 OR totalPoints<>0 ORDER BY $this->OrderBy $this->OrderType")+2;


		if(count($this->PlayerMobiles)==0){ return; }

		$i=1+$this->lim;
		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\" rules=rows color=white>\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\" style=\"padding-left:0px;\"><img align=middle style=\"float: left;\" src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_sx.gif\"><div style=\"padding-top:7px;\">".$this->Order_Link("Rank", "totalPoints")."</div></td>";
		echo "<td class=\"column\" style=\"width: 200px\">".$this->Order_Link("Name", "name")." [Abbr]</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Title", "title")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Duelstone Kills", "duelStoneKills")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Other Kills", "otherKills")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Tour Points", "tourPoints")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Duelstone Points", "duelStonePoints")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Total Points", "totalPoints")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Kills", "amountKilled")."</td>";
		echo "<td class=\"column\" style=\"padding-right: 0px;\"><div style=\"float:left; padding-top: 7px;\">".$this->Order_Link("Deaths", "amountDied")."</div><img align=middle src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_dx.gif\"></td>\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		$j=0;
		foreach($this->PlayerMobiles as $Mobile)
		{
			++$j;
			$Player=new GuildMember($Mobile['mobile'], $this->Database);
			$Player->printPlayerInfo($this->OrderBy, $j, $trattr, $tdattr);
		}

		echo "\t<tr class=\"lastrow\">\n";
		echo "\t\t<td colspan=8></td>\n";
		echo "\t</tr>\n";
		echo "</table>\n";
			
	}

	function Order_Link($name, $word)
	{
		if($word=="")
		{
			$extend="";
		}
		else
		{
			if(isset($this->OrderType))
			{
				if($this->OrderType=="desc" && $word==$this->OrderBy)
				{ $ad="asc"; } 
				else{ $ad="desc";}
			}
			$extend="&order=".$word."&ad=".$ad;
		}

		$retval="<a href=\"http://inx.xs4all.nl/ladder/guildladder/index.php?pid=41&p=1".$extend."\">".$name."</a>";
		return $retval;
	}

}

?>