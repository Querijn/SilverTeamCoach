<a href="?reset">Reset</a><br><br>
<?php
session_start();
$g_Error = "";

if(isset($_GET["reset"]) || isset($_POST["reset"]))
{
	if(isset($_SESSION["region"])) unset($_SESSION["region"]);
	if(isset($_SESSION["user"])) unset($_SESSION["user"]);
	if(isset($_SESSION["summoner"])) unset($_SESSION["summoner"]);
	
	header("Location: http://localhost");
}

// Check to make sure the right files are opened
define("INCLUDED", true);

// Folders
define("BASE_FOLDER", "");
define("API_FOLDER", BASE_FOLDER . "api" . DIRECTORY_SEPARATOR);
define("MYSQL_FOLDER", API_FOLDER . "mysql" . DIRECTORY_SEPARATOR);
define("RIOT_FOLDER", API_FOLDER . "riot" . DIRECTORY_SEPARATOR);

require_once(BASE_FOLDER . "settings.php");
require_once(API_FOLDER . "hacks.php");
require_once(MYSQL_FOLDER . "mysql.php");
require_once(RIOT_FOLDER . "php-riot-api.php");
require_once(RIOT_FOLDER . "FileSystemCache.php");

if(!class_exists("DatabasePlayer"))
{
	echo "<!--";
	print_r(RunSQL("sql/database.sql"));
	echo "-->";
}

// If we haven't logged in
if(!isset($_SESSION["user"]))
{
	
	// Stage 3, verification page.
	if(isset($_SESSION["summoner"]))
	{		
		require_once(BASE_FOLDER . "verify_user.php");
	}
	
	// Stage 2, the form submission.
	else if(isset($_POST["summoner_name"]) && isset($_POST["region"]))
	{
		
		// Init API
		$t_API = new riotapi($settings["riot_key"], $_POST["region"], new FileSystemCache("cache"));
		
		$t_Key = preg_replace('/\s+/', '', strtolower($_POST["summoner_name"]));
		
		try
		{
			$t_Data = $t_API->getSummonerByName($t_Key);
		
			// Verify region
			if(in_array($_POST["region"], $settings["regions"]))
				$_SESSION["region"] = $_POST["region"];
			
			else
			{
				$g_Error = "That was an invalid region. How did you even get here?";
				throw new Exception();
			}
			
			$_SESSION["summoner"] = $t_Data[$t_Key];	
			
			$t_Players = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
			
			// Do we exist?
			if(!is_object($t_Players) && !is_array($t_Players)) 
			{
				$t_Player = new DatabasePlayer();
			
				$t_Player->User = $_SESSION["summoner"]["id"];
				$t_Player->Admin = 0;
				
				$t_Player->Save();
			}
			
			// Go to stage 3.
			require_once(BASE_FOLDER . "verify_user.php");
		}
		catch(Exception $e)
		{
			switch($e->getMessage())
			{
			case "NOT_FOUND":
				$g_Error = "Unable to find that user in that region.";
				break;
				
			case "RATE_LIMIT_EXCEEDED":
				$g_Error = "Unfortunately we have exceeded our rate limit for the Riot API! Try again in a couple of minutes.";
				break;
				
			default:
			case 'UNAUTHORIZED':
			case 'BAD_REQUEST':
				$g_Error = "There seems to be an error in our code, is our API key set? (".$e->getMessage().")";
				break;
				
			case 'NO_RESPONSE':
			case 'ACCESS_DENIED':
			case 'SERVER_ERROR':
			case 'UNAVAILABLE':
				$g_Error = "Unknown error occurred on the Riot server. (".$e->getMessage().")";
				break;
			
			}
			include(BASE_FOLDER . "login_user.php");
			die();
		}
	}
	
	// Stage 1, login page.
	else require_once(BASE_FOLDER . "login_user.php");
}

// Run game.
else
{
	require_once(MYSQL_FOLDER . "mysql.php");
	echo "RunGame";
}
?>