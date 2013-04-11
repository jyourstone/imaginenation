<?php
	session_start();
?>
<html>

<head>
	<meta http-equiv="content-type" content="text/html; charset=ISO-8859-1">
<?php
	$_SESSION['mobileHash'] = -1;

	if ($_GET['id'] == '' || $_GET['s'] == '')
		DIE("You cannot access this page.");

	$_SESSION['mobileHash'] = $_GET['id'];
	

	$link=mysql_connect("localhost", "user", "password") OR DIE("An error occoured");
	mysql_select_db("database", $link) OR DIE("An error occoured");

	$result=mysql_query("SELECT endTime, seed FROM CurrentMobiles WHERE mobileHash=".$_SESSION['mobileHash']) OR DIE("An error occoured");
	$numRows=mysql_num_rows($result);

	$endTime = -1;
	$seed = -1;

	if ( $numRows > 0 )
	{	
		while ($row=mysql_fetch_array($result, MYSQL_ASSOC))
		{
			$endTime = $row['endTime'];
			$seed = $row['seed'];
		}
	}

	//Due to php using unix GMT timezone and RunUO using local computer timezone, this variable needs to be set to your timezone
	$timezone = -4; //GMT -4

	$correctTime = ($timezone*60*60)+time();

	if ( $numRows <= 0 || ($endTime-$correctTime) < 0 || $seed != $_GET['s'] )
	{
		echo "\t<title>Imagine Nation - Invalid Page</title>\n";
		echo "</head>\n";
		echo "<body>\n";
		echo "\tYou cannot access this page.\n";
		echo "</body>\n";
		echo "</html>\n";
		return;
	}

?>
	<title>Imagine Nation: Xtreme Anti Macro Code</title>
	<script src="CountDownTimer.js" type="text/javascript"></script>

</head>

<body onLoad="InitializeTimer(<?php echo $endTime-$correctTime ?>)">

<br />

To verify that you are not macroing, please <br />
type the word below in game. <br />
All letters should be lower case (small).<br>
There are no numbers, only letters.

<br /><br />

<?php 
	echo '<img src="image.php" id="image">'."\n<br />";
?>

<a href="javascript:location.reload(true)">New Image</a>
<br>
Click "New Image" if you cannot read the text.<br>
It will not count as a try and you can refresh<br>
the image as many times as you need.
<br><br />

<script type="text/javascript">
<!--
	document.write('<div id="timeLeft">Time left:</div>')
//-->
</script>
<NOSCRIPT>
	Enhance functionality by enabling JavaScript in <br />your browser.<br /> Refresh for a new image.
</NOSCRIPT>

</body>
</html>