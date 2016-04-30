<?php
require_once("include.php");

try
{
	$t_StaticAPI = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 3600);
	$t_Champions = $t_StaticAPI->getStatic('champion?dataById=true&champData=image,skins');

	
	foreach($t_Champions["data"] as $t_Champion)
		var_dump($t_Champion["name"], $t_Champion["skins"]);
}
catch(Exception $e)
{
	echo ($e->getMessage());
}
