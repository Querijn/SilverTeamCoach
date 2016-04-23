<a href="?reset">Reset</a><br><br>
<?php
if($_GET["reset"])
{
	unset($_SESSION["region"]);
	unset($_SESSION["user"]);
}

// Check to make sure the right files are opened
define("INCLUDED", true);

// Folders
define("BASE_FOLDER", "");
define("API_FOLDER", BASE_FOLDER . "api" . DIRECTORY_SEPARATOR);
define("MYSQL_FOLDER", API_FOLDER . "mysql" . DIRECTORY_SEPARATOR);
define("RIOT_FOLDER", API_FOLDER . "riot" . DIRECTORY_SEPARATOR);

session_start();
require_once(BASE_FOLDER . "settings.php");

if(!isset($_SESSION["user"]))
{
	if(isset($_POST["summoner_name"]) && isset($_POST["region"]))
	{
		require_once(RIOT_FOLDER . "php-riot-api.php");
		require_once(RIOT_FOLDER . "FileSystemCache.php");
		
		$_SESSION["region"] = $_POST["region"];
		
		$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache("cache"));
		$t_API->getStats(urlencode($_POST["summoner_name"]));
	}
	else require_once(BASE_FOLDER . "login_user.php");
}
else
{
	require_once(MYSQL_FOLDER . "mysql.php");
	$t_Player = new DatabasePlayer();
}
?>