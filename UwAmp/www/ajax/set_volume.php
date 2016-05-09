<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

try
{
	if(isset($_GET["sound"]) == false || is_numeric($_GET["sound"]) == false)
		throw new Exception("Sound value is not passed in!");
	if($_GET["sound"] < 0.0 || $_GET["sound"] > 1.0)
		throw new Exception("Sound value is out of range!");
	
	if(isset($_GET["music"]) == false || is_numeric($_GET["music"]) == false)
		throw new Exception("Music value is not passed in!");
	if($_GET["music"] < 0.0 || $_GET["music"] > 1.0)
		throw new Exception("Music value is out of range!");
	
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	$t_Player->SoundVolume = $_GET["sound"];
	$t_Player->MusicVolume = $_GET["music"];
	$t_Player->Save();
	echo "true";
}
catch(Exception $e)
{
	die($e->getMessage());
}
