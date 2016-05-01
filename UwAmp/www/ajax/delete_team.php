<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

$t_GetID = $_SESSION["summoner"]["id"];

try
{
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"));

	if(!isset($_GET["id"]) || !is_numeric($_GET["id"]))
		die("That Team ID is invalid.");
	
	$t_Team = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("id")->Is($_GET["id"]));
	if(is_object($t_Team) && $t_Team->LoadFailed == false)
	{
		if($t_Team->Player == $_SESSION["summoner"]["id"])
		{
			$t_Team->Delete();
			die("true");
		}
		else die("That team does not belong to you.");
	}
	else die("That team does not exist.");
}
catch(Exception $e)
{
	die($e->getMessage());
}
