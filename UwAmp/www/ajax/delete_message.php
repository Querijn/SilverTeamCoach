<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

$t_GetID = $_SESSION["summoner"]["id"];

try
{
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"));

	if(!isset($_GET["id"]) || !is_numeric($_GET["id"]))
		die("That Message ID is invalid.");
	
	$t_Message = DatabaseMessage::Load(SQLSearch::In(DatabaseMessage::Table)->Where("id")->Is($_GET["id"]));
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	if(is_object($t_Message) && $t_Message->LoadFailed == false)
	{
		if($t_Message->PlayerId == $t_Player->Id)
		{
			$t_Message->Delete();
			die("true");
		}
		else die("That message does not belong to you.");
	}
	else die("That message does not exist.");
}
catch(Exception $e)
{
	die($e->getMessage());
}
