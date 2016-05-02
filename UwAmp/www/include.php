<?php

if(!defined("INCLUDED")) 
	die();

session_start();
define("AJAX_FOLDER", BASE_FOLDER . "ajax/");
define("API_FOLDER", BASE_FOLDER . "api/");
define("KEY_FOLDER", BASE_FOLDER . "key/");
define("MYSQL_FOLDER", API_FOLDER . "mysql/");
define("RIOT_FOLDER", API_FOLDER . "riot/");
define("BUILD_FOLDER", BASE_FOLDER . "build/");

require_once(BASE_FOLDER . "settings.php");
require_once(API_FOLDER . "hacks.php");
require_once(MYSQL_FOLDER . "mysql.php");

$t_InsertBots = false;
if(!class_exists("DatabasePlayer"))
{
	//echo "<!--";
	print_r(RunSQL(BASE_FOLDER."sql/database.sql"));
	unset($settings["mysql_connection"]);
	include(MYSQL_FOLDER . "mysql.php");
	$t_InsertBots = true;
}

require_once(RIOT_FOLDER . "FileSystemCache.php");
require_once(RIOT_FOLDER . "php-riot-api.php");
require_once(API_FOLDER . "login.php");
require_once(API_FOLDER . "champion_names.php");
require_once(API_FOLDER . "champion_prices.php");
require_once(API_FOLDER . "bots.php");
require_once(API_FOLDER . "messages.php");
require_once(API_FOLDER . "ranked.php");
require_once(API_FOLDER . "game.php");

if($settings['testing'] === true && !isset($_SESSION["summoner"]))
{
	LogIn($settings["testing_account"], $settings["testing_region"], true);
}

if($t_InsertBots)
{
	if(CreateBot("Misty",   "Cerulean City Gym", "Tahm Kench", "Fizz", "Zac", "Nami", "Graves", 0, 0, 2, 0, 5))
	if(CreateBot("Wall-E",  "int main()", "Rumble", "Orianna", "Cho'Gath", "Blitzcrank", "Corki", 0, 0, 5, 0, 0))
	if(CreateBot("Gandalf",  "The Blue Wizards", "Ryze", "Veigar", "Evelynn", "Zilean", "Ezreal", 0, 0, 0, 0, 0))
	if(CreateBot("Hattori Hanzo",  "We Hate Pirates", "Akali", "Zed", "Rammus", "Shen", "Kennen", 0, 0, 5, 0, 0))
	if(CreateBot("Hawkeye",  "Hail of Pointy Things", "Pantheon", "Azir", "Nidalee", "Kennen", "Ashe", 0, 0, 0, 0, 0))
	if(CreateBot("Gnar",  "We <3 Yordles", "Poppy", "Heimerdinger", "Rumble", "Lulu", "Tristana", 0, 0, 0, 0, 0))
	if(CreateBot("Steve Irwin",  "National Geographic", "Renekton", "Anivia", "Warwick", "Alistar", "Twitch", 2, 0, 0, 0, 0))
	if(CreateBot("Mike",  "Monsters Inc.", "Tahm Kench", "Cho'Gath", "Rek'Sai", "Vel'Koz", "Kog'Maw", 0, 0, 0, 0, 0))
	if(CreateBot("Blaine",  "Cinnabar Island Gym", "Wukong", "Annie", "Warwick", "Brand", "Jinx", 1, 0, 1, 0, 2))
	if(CreateBot("Bruv McMuscles",  "Do U Even Lift Bro", "Garen", "Jayce", "Olaf", "Braum", "Draven", 0, 0, 3, 0, 0))
		echo "all bots added.";
	// CreateBot("Username",  "TeamName", "a_Top", "a_Mid", "a_Jungle", "a_Support", "a_Marksman", 0, 0, 0, 0, 0);
	//echo "!-->";
}
?>