<?PHP
include_once("db.php");
include_once("GuildClass.php");

class GuildLadder
{
	var $Order;
	var $lim;
	var $Database;
	var $Guilds;
	var $ASDDESC;

	function GuildLadder($db, $limit, $order, $asddesc)
	{
		$this->Database=$db;

		$this->lim=mysql_real_escape_string($limit);
		$this->Order=mysql_real_escape_string($order);	
		$this->ASDDESC=mysql_real_escape_string($asddesc);	

		if(!isset($order))
		{
			$this->Order="totalPoints";
		}

		if(isset($this->ASDDESC))
		{
			if($this->ASDDESC=="asc")
			{
				$this->Order.=" ASC";
			}
			else
			{
				$this->Order.=" DESC";
			}
		}
		else
		{
			$this->Order.=" DESC";
		}

		if(!is_numeric($this->lim)){die("Don't try that shit..."); }
	
		if($this->Order=="guildmaster ASC" || $this->Order=="guildmaster DESC")
		{
			$this->Guilds=$this->Database->fromDB("SELECT * FROM guilds LIMIT $this->lim, 20");	
		}	
		else if($this->Order!="Members ASC" && $this->Order!="Members DESC")
		{
			$this->Guilds=$this->Database->fromDB("SELECT guildID FROM guilds ORDER BY $this->Order LIMIT $this->lim, 20");
		}
		else
		{
			$this->Guilds=$this->Database->fromDB("SELECT guildID FROM guilds LIMIT $this->lim, 20");
		}

		if($this->Order=="Members ASC" || $this->Order=="Members DESC")
		{
			$this->OrderByMembers();
		}
		else if($this->Order=="guildmaster ASC" || $this->Order=="guildmaster DESC")
		{
			$this->OrderByGuildMaster();
		}


	}

	function OrderByMembers()
	{
		for($i=0;$i<count($this->Guilds);$i++)
		{
			$guildID=$this->Guilds[$i]['guildID'];
			$this->Guilds[$i]['Members']=$this->Database->getNumRows("SELECT * FROM guildmembers WHERE guildID='$guildID'");
		}

		for($storlek=count($this->Guilds), $plats=0, $temp, $j=0, $i=$storlek; --$i>0; $plats=0, $j=0)
		{
			while(++$j<=$i)
			{
				if(isset($this->ASDDESC))
				{

					if($this->ASDDESC=="asc")
					{
						if($this->Guilds[$plats]['Members'] < $this->Guilds[$j]['Members'])
						{
							$plats=$j;
						}
					}
					else
					{
						if($this->Guilds[$plats]['Members'] > $this->Guilds[$j]['Members'])
						{
							$plats=$j;
						}
					}
				}

				$temp=$this->Guilds[$i];
				$this->Guilds[$i]=$this->Guilds[$plats];
				$this->Guilds[$plats]=$temp;
			}
		}
	}

	function OrderByGuildMaster()
	{
		for($i=0;$i<count($this->Guilds);$i++)
		{
			$mobile=$this->Guilds[$i]['guildmaster'];
			$name=$this->Database->fromDB("SELECT name FROM guildmembers WHERE mobile='$mobile'");
			$this->Guilds[$i]['GMName']=$name[0]['name'];
		}
	

		for($storlek=count($this->Guilds), $plats=0, $temp, $j=0, $i=$storlek; --$i>0; $plats=0, $j=0)
		{
			while(++$j<=$i)
			{
				if(isset($this->ASDDESC))
				{

					if($this->ASDDESC=="asc")
					{
						if(strcmp($this->Guilds[$plats]['GMName'], $this->Guilds[$j]['GMName'])<0)
						{
							$plats=$j;
						}
					}
					else
					{
						if(strcmp($this->Guilds[$plats]['GMName'], $this->Guilds[$j]['GMName'])>0)
						{
							$plats=$j;
						}
					}
				}

				$temp=$this->Guilds[$i];
				$this->Guilds[$i]=$this->Guilds[$plats];
				$this->Guilds[$plats]=$temp;
			}
		}
	}



	function printLadder($tableattr, $trattr, $tdattr)
	{
		if($this->Order!="Members ASC" && $this->Order!="Members DESC" && $this->Order!="guildmaster ASC" && $this->Order!="guildmaster DESC")
		{
			$rows=$this->Database->getNumRows("SELECT * FROM guilds ORDER BY $this->Order LIMIT $this->lim, 20")+2;
		}
		else
		{
			$rows=$this->Database->getNumRows("SELECT * FROM guilds LIMIT $this->lim, 20")+2;
		}

		if(count($this->Guilds)==0){ return; }

		$i=1+$this->lim;
		echo "\n<table ".$tableattr.">\n";
		echo "\t<tr class=\"firstrow\" rules=rows color=white>\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"column\" style=\"padding-left:0px;\"><img align=middle style=\"float: left;\" src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_sx.gif\"><div style=\"padding-top:7px;\">".$this->Order_Link("Rank", "totalPoints")."</div></td>";
		echo "<td class=\"column\" style=\"width: 230px\">".$this->Order_Link("Name", "name")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Guildmaster", "guildmaster")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Abbr", "abbrevation")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Type", "type")."</td>";
		echo "<td valign=middle class=\"column\">".$this->Order_Link("Members", "Members")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Duelstone Kills", "duelStoneKills")."</td>\n";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Duelstone Deaths", "duelStoneDeaths")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Other Kills", "otherKills")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Other Deaths", "otherDeaths")."</td>\n";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Tour Points", "tourPoints")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Duelstone Points", "duelStonePoints")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Total Kills", "totalKills")."</td>";
		//echo "<td valign=middle class=\"column\">".$this->Order_Link("Total Deaths", "totalDeaths")."</td>";
		echo "<td class=\"column\" style=\"padding-right: 0px;\"><div style=\"float:left; padding-top: 7px;\">".$this->Order_Link("Total Points", "totalPoints")."</div><img align=middle src=\"http://inx.xs4all.nl/mkportal/templates/skylight2/images/m_dx.gif\"></td>\n";
		echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		$j=0;
		foreach($this->Guilds as $Guild)
		{
			++$j;
			$theGuild=new Guild($this->Database, $Guild['guildID']);
			$theGuild->printGuildInfo($this->Order, $j, $trattr, $tdattr);
		}

		echo "\t<tr class=\"lastrow\">\n";
		echo "\t\t<td colspan=6></td>\n";
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
			if(isset($this->ASDDESC))
			{
				$temp=explode(" ", $this->Order);
				if($this->ASDDESC=="desc" && $word==$temp[0])
				{ $ad="asc"; } 
				else{ $ad="desc";}
			}
			$extend="&order=".$word."&ad=".$ad;
		}

		$retval="<a href=\"http://inx.xs4all.nl/ladder/guildladder/index.php?pid=41".$extend."\">".$name."</a>";
		return $retval;
	}
}