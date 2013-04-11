<?PHP
include("functions.php");	
?>

<link href="styles.css" rel="stylesheet" type="text/css">
<body bgcolor="#d7cfba" link="black" vlink="black" alink="black" onLoad="window.name='ladder'">

<a name="top" id="top"></a>
<table border="0" width="100%" cellspacing="0" cellpadding="0" id="table1">
	<tr>
		<td>

      <table width="100%" border="0" cellspacing="0" cellpadding="0" id="table7">

        <tr> 


          <td rowspan="2" bgcolor="#d7cfba" background="http://www.in-x.org/templates/xuo/images/test.jpg" valign="top"> 
        


<div align="center" class="menu"><a href="ladder.php">Main</a> | <a href="?top1v1">Top 1v1</a> | <a href="?top2v2">Top 2v2</a> | <a href="?topffa">Top FFA</a> | <a href="?toptotal">Top Overall</a></div><br>

<?PHP
connect_db("localhost", "database", "user", "password");
if(isset($_POST['submit'])){
	$hittade=search_for_players($_POST['searchword'], $_POST['poang'], $_POST['ladder']);
	if(count($hittade)!=0){
		print_top($hittade, $_POST['ladder'], true);
	}
	//echo "This page is under construction.";
}
else if(isset($_GET['search'])){
?>
	<script language="javascript">
		function complete_form(form){
			if(form.searchword.value=="" && form.poang.value==""){
				document.getElementById("Searchdiv").innerHTML="You forgot to fill in a searchword.";
				document.getElementById("Searchdiv").style.backgroundColor="red";
				document.getElementById("Searchdiv").style.fontSize="40";
				return false;
			}
			if(isNaN(form.poang.value)){
				document.getElementById("Searchdiv").innerHTML="You've supplied invalid characters.";
				document.getElementById("Searchdiv").style.backgroundColor="red";
				document.getElementById("Searchdiv").style.fontSize="40";
				return false;
			}				
		}
	</script>
<?
	echo "<div ID=\"Searchdiv\" align=\"center\" style=\"width: 500px;\">Search for a player</div><br>\n";
	echo "<form action=\"?search\" method=\"POST\" name=\"Searchform\" onSubmit=\"return complete_form(this);\">\n";
	echo "\tLadder: <select class=\"inputbox\" name=\"ladder\"><option>1v1</option><option>2v2</option><option value=\"Other\">FFA</option><option value=\"Total\">Overall</option></select><br>\n";
	echo "\tPoints: <input class=\"inputbox\" type=\"text\" name=\"poang\"><br>\n";
	echo "\tName: <input class=\"inputbox\" type=\"text\" name=\"searchword\">\n";
	echo "\t<input class=\"button\" type=\"submit\" value=\"Search\" name=\"submit\">\n";
	echo "</form>";
}
else if(isset($_GET['top1v1'])){
	$array2=findtop(100, "1v1");
	print_top($array2, "1v1", $_GET['top1v1']);
}
else if(isset($_GET['top2v2'])){
	$array2=findtop(100, "2v2");
	print_top($array2, "2v2");
}
else if(isset($_GET['topffa'])){
	$array2=findtop(100, "Other");
	print_top($array2, "Other");
}
else if(isset($_GET['toptotal'])){
	$array2=findtop(100, "Total");
	print_top($array2, "Total");
}
else if(isset($_GET['p'])){
	echo "<table width=\"100%\">";
		echo "\t<tr>\n";
			echo "\t\t<td width=50% align=\"right\" valign=\"top\">";
				player_statics($_GET['p']);
			echo "\t\t</td>\n";
			echo "\t\t<td width=50% align=\"left\" valign=\"top\">";
				player_events($_GET['p']);
			echo "\t\t</td>\n";
		echo"\t</tr>\n";
	echo"</table>\n";
}
else if(isset($_GET['e'])){
	echo "<table width=\"100%\">";
		echo "\t<tr>\n";

		$tour = get_event_info($_GET['e']);
		write_event_table($tour);
		echo"\t</tr>\n";
	echo"</table>\n";
}
else{
	echo "<table width=\"100%\" cellspacing=0 cellpadding=0>";

	echo "\t<tr>\n";

		echo "\t\t<td class=\"toursmain\" width=33% align=\"center\">";
		$array=findtop(10, "1v1");
		print_top($array, "1v1");
		echo "\t\t</td>\n";


		echo "\t\t<td class=\"toursmain\" width=33% align=\"center\">";
		$array=findtop(10, "Total");
		print_top($array, "Total");
		echo "\t\t</td>\n";

		echo "\t\t<td class=\"toursmain\" width=33% align=\"center\">";
		$array=findtop(10, "2v2");
		print_top($array, "2v2");
		echo "\t\t</td>\n";

		echo "\t</tr>\n";

	echo "\t<tr>\n";

	$array=find_three_latest_tours();
	foreach($array as $tour){
		if(!empty($tour)){
			write_event_table($tour);
		}
	}
	echo "\t</tr>\n";
	echo "\t<tr>\n";
	echo "\t\t<td colspan=3 width=\"100%\" align=\"center\">";
		?>
		<script language="javascript">
			function complete_form(form){
				if(form.searchword.value=="" && form.poang.value==""){
					form.searchword.value="Fill this field to search";
					form.searchword.style.backgroundColor="red";
					return false;
				}
				if(isNaN(form.poang.value)){
					form.poang.value="Only numbers here";
					form.poang.style.backgroundColor="red";
					return false;
				}				
			}

			function restore_field()
			{
				Searchform.poang.style.backgroundColor="#7a433c";
				Searchform.searchword.style.backgroundColor="#7a433c";
			}
		</script>
		<?

	echo "<form action=\"?search\" method=\"POST\" name=\"Searchform\" onSubmit=\"return complete_form(this);\">\n";
	echo "<br><br><table>\n";
		echo "\t<tr>\n";
			echo "\t\t<td>";
				echo "Search for a player";
			echo"</td>\n";
		echo"\t</tr>\n";

		echo "\t<tr>\n";
			echo "\t\t<td>";
				echo "Ladder: <select class=\"inputbox\" name=\"ladder\"><option>1v1</option><option>2v2</option><option value=\"Other\">FFA</option><option value=\"Total\">Overall</option></select>";
			echo"</td>\n";
		echo"\t</tr>\n";

		echo "\t<tr>\n";
			echo "\t\t<td>";
				echo "Points: <input class=\"inputbox\" type=\"text\" onKeyPress=\"return restore_field();\" name=\"poang\">";
			echo"</td>\n";
		echo"\t</tr>\n";

		echo "\t<tr>\n";
			echo "\t\t<td>";
				echo "Name: <input class=\"inputbox\" type=\"text\" onKeyPress=\"return restore_field();\" name=\"searchword\">";
			echo"</td>\n";
		echo"\t</tr>\n";


		echo "\t<tr>\n";
			echo "\t\t<td>";
				echo "<input class=\"button\" type=\"submit\" value=\"Search\" name=\"submit\"><input type=\"reset\" class=\"button\" onMouseUp=\"return restore_field();\" value=\"Clear\" name=\"clear\">";
			echo"</td>\n";
		echo"\t</tr>\n";

	echo"</table>\n";
	echo "</form>\n";

	echo "\t\t</td>\n";
	echo "\t</tr>\n";
	echo "</table>";
}

?>

      </table>
</body>
</html>