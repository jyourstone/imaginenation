<? // © Erik Eliasson 2006-03-19
	function connect_db($host, $account, $password, $db){
		$link = mysql_connect($host, $account, $password);
		mysql_select_db($db, $link);
	}

function write_event_table($tour){
	echo "<td width=33% align=\"center\"><table style=\"border: 1px solid black;\">\n";
	echo "\t<tr>\n";
	//echo "\t\t<td rowspan=8 class=\"dots\"></td>\n";
	echo "\t\t<td class=\"begin\"><div class=\"head\" style=\"float: left;\"><b>".$tour['Participants']." People</b></div><div align=\"right\" class=\"head\"><b>".$tour['Type']."</b></div></td>\n";
	//echo "\t\t<td rowspan=8 class=\"dots\"></td>\n";
	echo "\t</tr>\n";

	echo "\t<tr>\n";
	echo "\t\t<td class=\"eventtd\">Winner: ".link_to_player($tour['Winner']);
	if(!empty($tour['Winner2']) && $tour['Winner2']!=0){
		echo " & ".link_to_player($tour['Winner2']);
	}
	echo "</td>\n";
	echo "\t</tr>\n";

	echo "\t<tr>\n";
	echo "\t\t<td class=\"eventtd\">Second: ".link_to_player($tour['Second']);
	if(!empty($tour['Second2']) && $tour['Second2']!=0){
		echo " & ".link_to_player($tour['Second2']);
	}
	echo "</td>\n";
	echo "\t</tr>\n";

	echo "\t<tr>\n";
	echo "\t\t<td class=\"eventtd\">Third: ".link_to_player($tour['Third']);
	if(!empty($tour['Third2']) && $tour['Third2']!=0){
		echo " & ".link_to_player($tour['Third2']);
	}
	echo "</td>\n";
	echo "\t</tr>\n";

	echo "\t<tr>\n";
	echo "\t\t<td class=\"eventtd\">Hosted by: ".$tour['Hoster']."</td>\n";
	echo "\t</tr>\n";

	echo "\t<tr>\n";
	echo "\t\t<td class=\"eventtd\" style=\"border-bottom: 0\">Date: ".$tour['DateHosted']."</td>\n";
	echo "\t</tr>\n";

	//echo "\t<tr>\n";
	//echo "\t\t<td class=\"end\" height=10></td>\n";
	//echo "\t</tr>\n";
	echo "</table>\n";

	echo "</td>\n";
}

	Function write_tr($content, $class){
	
		echo "\t<tr>\n";
		echo "\t\t<td class=\"eventtd\">".$contet."<td>\n";
		echo "\t</tr>\n";
	}

		
	Function find_top($intervall, $event){
		$player_array=array();
		$qry="SELECT MAX(".$event."Points) FROM participant";
		$result=mysql_query($qry);
		$array=mysql_fetch_array($result);
		$max=$array[0];

		$players=0;
		while($players<=$intervall){
			if($max<0){
				break;
			}

			$qry2="SELECT Mobile, ".$event."Points FROM Participant WHERE ".$event."Points=$max";
			$result=mysql_query($qry2);
			while($char=mysql_fetch_array($result, MYSQL_ASSOC)){
				$player_array[$players]["Mobile"]=$char['Mobile'];
				$player_array[$players][$event."Points"]=$char[$event."Points"];
				$players++;
			}
			$max--;
		}

		return $player_array;
	}

	Function findtop($intervall, $event){
		$players=0;
		$player_array=array();
		$qry2="SELECT Mobile, ".$event."Points FROM Participant ORDER BY ".$event."Points DESC";
		$result=mysql_query($qry2);
		while($char=mysql_fetch_array($result, MYSQL_ASSOC)){
			if($players==$intervall){
				break;
			}
			$player_array[$players]=$char;
			$players++;
		}

		return $player_array;
	}

	Function check_for_two($array, $Mobile){
		foreach($array as $r){
			if($r['Mobile']==$Mobile){
				return true;
			}
		}
		return false;
	}

	Function find_three_latest_tours(){
		$temp=0;
		$tour_array=array();
		$tours=0;

		$qry="SELECT * FROM event ORDER BY DateHosted DESC";
		$result=mysql_query($qry);
		while($tours!=3){
			$temp_array=mysql_fetch_array($result, MYSQL_ASSOC);
			$tour_array[$tours]=$temp_array;
			$tours++;
		}
		return $tour_array;
	}

	Function get_mobile_info($mobile){
		$qry="SELECT * FROM participant WHERE Mobile=".$mobile;
		$result=mysql_query($qry);
		$player_array=mysql_fetch_array($result, MYSQL_ASSOC);
	
		return $player_array;
	}

	Function get_event_info($event){
		$qry="SELECT * FROM event WHERE Event=$event";
		$result=mysql_query($qry);
		$event_array=mysql_fetch_array($result, MYSQL_ASSOC);
	
		return $event_array;
	}

	Function link_to_event($event){
		$info=get_event_info($event);
		//$link="<a href=\"javascript:window.open('popup.php?e=".$event."','nySida', 'height=240, width=300, left=20, top=160'); void(0)\">".$info['Type']."</a>";
		$link="<a href=\"ladder.php?e=$event\" target=\"_self\">".$info['Type']."</a>";
		return $link;
	}

	Function link_to_player($mobile){
		$info=get_mobile_info($mobile);
		$link="<a href=\"ladder.php?p=".$mobile."\" target=\"_self\">".$info['Name']."</a>";
		return $link;
	}

	Function search_for_players($name, $points=0, $event){
		if(empty($points)){
			$points=0;
		}
		else if(!is_numeric($points)){
			$points=0;
			echo "You've supplied incorrect characters.<p>\n";
		}

		$qry="SELECT * FROM participant WHERE Name LIKE '%".$name."%' AND ".$event."Points>=$points ORDER BY ".$event."Points DESC";
		$result=mysql_query($qry);
		$count=0;
		$return_array=array();

		$rows=mysql_num_rows($result);
		if($rows==0){
			echo "No players found.";
		}

		while($info_array=mysql_fetch_array($result, MYSQL_ASSOC)){
			$return_array[$count]=$info_array;
			$count++;
		}
		return $return_array;
	}

	Function print_top($array, $event, $search=NULL)
	{
		$style = "";
		$count=1;
		$rows=2+count($array);
		if($search)
		{
			$colspan = 2;
			$str = "Search results in top ";
		}
		else
		{
			$colspan = 3;
			$str = "Top ";
		}

		echo "<table align=\"center\" style=\"border: 1px solid black;\">\n";
		echo "\t<tr>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"begin\" colspan=$colspan align=\"center\"><b>$str";

		if($event=="Other"){ 
			echo "FFA"; 
		}else if($event=="Total"){
			echo "Overall";
		}else if($event=="1v1"){
			echo "1vs1";
		}else if($event=="2v2"){
			echo "2vs2";
		}else{
			echo $event;
		}

		if($search == true)
		{
			echo " ladder</b>";
		}
		echo "</b></td>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		foreach($array as $value){
			/*if($search==$value['Mobile']){
				echo "\t<tr>\n";
				echo "\t\t<td bgColor=\"red\" class=\"numbers\">".$count.".</td>\n";
				echo "\t\t<td bgColor=\"red\" class=\"eventtd\" width=400><a name=\"".$value['Mobile']."\">".link_to_player($value['Mobile'])."</td>\n";
				echo "\t\t<td bgColor=\"red\" class=\"numbers\" align=\"right\">".$value[$event.'Points']."</td>\n";
				echo "\t</tr>\n";
				$count++;
			}*/
			if($search == true)
			{
				echo "\t<tr>\n";
				//echo "\t\t<td class=\"numbers\">".$count.".</td>\n";

				if($count == count($array))
					$style = "style=\"border-bottom: 0;\"";
				
				echo "\t\t<td class=\"eventtd\" $style width=400>".link_to_player($value['Mobile'])."</td>\n";
				echo "\t\t<td class=\"numbers\" align=\"right\">".$value[$event.'Points']."</td>\n";
				echo "\t</tr>\n";
			}				
			else{
				echo "\t<tr>\n";
				echo "\t\t<td class=\"numbers\">".$count.".</td>\n";

				if($count == count($array))
					$style = "style=\"border-bottom: 0;\"";
				
				echo "\t\t<td class=\"eventtd\" $style width=400>".link_to_player($value['Mobile'])."</td>\n";
				echo "\t\t<td class=\"numbers\" align=\"right\">".$value[$event.'Points']."</td>\n";
				echo "\t</tr>\n";
				$count++;
			}
		}



		//echo "\t<tr>\n";
		//echo "\t\t<td colspan=3 height=10 class=\"end\"></td>\n";
		//echo "\t</tr>\n";
		echo "</table>\n";
	}


	Function all_tours_top3($mobile){
		$sql="SELECT * FROM event WHERE Winner=$mobile OR Second=$mobile OR Third=$mobile OR Winner2=$mobile OR Second2=$mobile OR Third2=$mobile ORDER BY DateHosted DESC";
		$result=mysql_query($sql);
		$return_array=array();
		$count=0;
		while($array=mysql_fetch_array($result, MYSQL_ASSOC)){
			$return_array[$count]=$array;
			$count++;
		}

		return $return_array;
	}

function player_statics($mobile){
	$array=get_mobile_info($mobile);
	$rows=7;
	echo "<table style=\"border: 1px solid black;\">\n";
		echo "\t<tr>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"begin\" align=\"center\"><b>".$array['Name'];
		echo "</b></td>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		write_row("Tournaments won: ".$array['Won']);
		write_row("Tournaments second: ".$array['Second']);
		write_row("Tournaments third: ".$array['Third']);
		write_row("Tournaments joined: ".$array['Joined']);
		write_row("Total Points: ".$array['TotalPoints']);

		//echo "\t<tr>\n";
		//echo "\t\t<td height=10 class=\"end\"></td>\n";
		//echo "\t</tr>\n";

	echo "</table>\n";

}

function player_events($mobile){
	$array2=all_tours_top3($mobile);
	$rows=count($array2)+2;
	echo "<table style=\"border: 1px solid black;\">\n";
		echo "\t<tr>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t\t<td class=\"begin_end_playerstat\"><b>Event</b></td>\n";
		echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\"><b>Joined</b></td>\n";
		echo "\t\t<td class=\"begin_end_playerstat\" align=\"right\"><b>Rank</b></td>\n";
		//echo "\t\t<td class=\"dots\" rowspan=".$rows."></td>\n";
		echo "\t</tr>\n";

		foreach($array2 as $r){
			$e_info=get_event_info($r['Event']);
			echo"\t<tr>\n";
			echo"\t\t<td class=\"playerstat_event\">".link_to_event($r['Event'])."</td>\n";
			echo"\t\t<td class=\"playerstat_event\" align=\"right\">".$e_info['Participants']."</td>\n";

			if($r['Winner']==$mobile || $r['Winner2']==$mobile){
				$rank=1;
			}
			else if($r['Second']==$mobile || $r['Second2']==$mobile){
				$rank=2;
			}
			else if($r['Third']==$mobile || $r['Third2']==$mobile){
				$rank=3;
			}

			echo"\t\t<td class=\"playerstat_event\" align=\"right\">#".$rank."</td>\n";
			echo"\t</tr>\n";
		}
		//echo "\t<tr>\n";
		//echo "\t\t<td height=10 colspan=3 class=\"end\"></td>\n";
		//echo "\t</tr>\n";

	echo "</table>\n";
}

function write_row($content){
	echo"\t<tr>\n";
		echo"\t\t<td class=\"eventtd\">".$content."</td>\n";
	echo"\t</tr>\n";
}
	
			
?>