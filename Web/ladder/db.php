<?PHP

class DB
{

	function DB($server, $user, $pswd, $base)
	{
		$link=mysql_connect($server, "user", "password") OR DIE("An error occoured");
		mysql_select_db($base, $link) OR DIE("An error occoured");
	}

	function toDB($sql)
	{
		mysql_query($sql) OR DIE("An error occoured");
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

class User
{
	var $AccessLevel;
	var $nickName;
	var $membID;
	var $login;

	function User($nick, $password, $path)
	{
		$path=explode(";", $path);
		$sql="SELECT * FROM $path[0] WHERE path[1]='$nick' AND path[2]='$password'";
		$result=mysql_query($sql);
		$info=mysql_fetch_array($result, MYSQL_ASSOC);
		
		if(mysql_num_rows($result)>0)
		{
			$this->login=true;
			$this->AccessLevel=$info['Access'];
			$this->nickName=$info[$path[1]];
			$this->membID=$info['id'];
		}
		else
		{
			$this->login=false;
		}
	}
}
?>