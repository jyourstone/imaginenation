<?PHP
/*************************************************************************************************
			DB(databas) klass
			----------------------
		    Written by: Erik Eliasson © 2006

**************************************************************************************************/

class DB
{

	function DB($server, $user, $pswd, $base)
	{
		$link=mysql_connect($server, $user, $pswd) OR DIE("An error occoured");
		mysql_select_db($base, $link) OR DIE("An error occoured ");
	}

	function toDB($sql)
	{
		mysql_query($sql) OR DIE("An error occoured ");
	}

	function fromDB($sql)
	{
		$result=mysql_query($sql) OR DIE("An error occoured");
		//if(mysql_num_rows($result)>1)
		//{
			$retar=array();
			for($i=0;$info=mysql_fetch_array($result, MYSQL_ASSOC);$i++)
			{
				$retar[$i]=$info;
			}
		//}	
		//else{ $retar=mysql_fetch_array($result, MYSQL_ASSOC); }

		return $retar;
	}

	function getNumRows($sql)
	{
		$result=mysql_query($sql) OR DIE("An error occoured");
		$retval=mysql_num_rows($result);
		return $retval;
	}
}

?>