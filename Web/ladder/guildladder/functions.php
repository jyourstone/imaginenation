<?PHP


	function link_to_mobile($mobile)
	{
		$SQL="SELECT * FROM guildmembers WHERE mobile='$mobile'";
		$result=mysql_query($SQL);
		$info=mysql_fetch_array($result, MYSQL_ASSOC);

		$link="<a href=\"http://inx.xs4all.nl/ladder/guildladder/index.php?pid=41&mid={$info['mobile']}\">{$info['name']}</a>";
		return $link;
	}

	function link_to_guild($GuildID, $field)
	{
		$SQL="SELECT * FROM guilds WHERE guildID='$GuildID'";
		$result=mysql_query($SQL);
		$info=mysql_fetch_array($result, MYSQL_ASSOC);
		
		$link="<a href=\"http://inx.xs4all.nl/ladder/guildladder/index.php?pid=41&gid={$info['guildID']}\">{$info[$field]}</a>";
		return $link;
	}

	function write_row($content){
		echo"\t<tr>\n";
			echo"\t\t<td class=\"eventtd\" align=\"left\">".$content."</td>\n";
		echo"\t</tr>\n";
	}

	function exists($mobile)
	{
		$SQL="SELECT * FROM guildmembers WHERE mobile='$mobile'";
		$result=mysql_query($SQL);
		$retval=mysql_num_rows($result);
		return $retval;
	}
?>