<?php
if(!defined("INCLUDED")) 
	die();

session_start();
define("API_FOLDER", BASE_FOLDER . "api" . DIRECTORY_SEPARATOR);
define("KEY_FOLDER", BASE_FOLDER . "key" . DIRECTORY_SEPARATOR);
define("MYSQL_FOLDER", API_FOLDER . "mysql" . DIRECTORY_SEPARATOR);
define("RIOT_FOLDER", API_FOLDER . "riot" . DIRECTORY_SEPARATOR);

require_once(BASE_FOLDER . "settings.php");
require_once(API_FOLDER . "hacks.php");
require_once(MYSQL_FOLDER . "mysql.php");
require_once(RIOT_FOLDER . "FileSystemCache.php");
require_once(RIOT_FOLDER . "php-riot-api.php");
require_once(API_FOLDER . "login.php");
require_once(API_FOLDER . "champion_prices.php");

if($settings['testing'] === true && !isset($_SESSION["summoner"]))
{
	LogIn($settings["testing_account"], $settings["testing_region"], true);
}

require_once(RIOT_FOLDER . "FileSystemCache.php");

?>