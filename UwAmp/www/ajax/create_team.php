<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die("You are not logged in.");

$t_GetID = $_SESSION["summoner"]["id"];

try
{
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"));

	// Validate name
	if(is_string($_GET["name"]) == false)
		die("Invalid name for a team!");

	if(empty($_GET["name"]))
		die("Team name cannot be empty!");
	
	if(strlen($_GET["name"]) > 32)
		die("Team name is too long!");
	
	$t_NameCheck = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("name")->Is($_GET["name"]));
	if(is_object($t_InsertedTeam) && $t_InsertedTeam->LoadFailed == false)
		die("A team with this name already exists!");

	$t_Champions = $t_API->getStatic('champion?dataById=true&champData=image')["data"];
	if(!isset($_GET["mid"]) || isset($t_Champions[$_GET["mid"]]) == false)
		die("Invalid mid!");
	
	if(!isset($_GET["top"]) || isset($t_Champions[$_GET["top"]]) == false)
		die("Invalid top!");
	
	if(!isset($_GET["jungle"]) || isset($t_Champions[$_GET["jungle"]]) == false)
		die("Invalid jungle!");
	
	if(!isset($_GET["marksman"]) || isset($t_Champions[$_GET["marksman"]]) == false)
		die("Invalid marksman!");
	
	if(!isset($_GET["support"]) || isset($t_Champions[$_GET["support"]]) == false)
		die("Invalid support!");
	
	$t_Team = new DatabaseTeam();
	$t_Team->Name = $_GET["name"];
	$t_Team->Player = $t_GetID;
	
	$t_Team->Mid = $_GET["mid"];
	$t_Team->Top = $_GET["top"];
	$t_Team->Jungle = $_GET["jungle"];
	$t_Team->Marksman = $_GET["marksman"];
	$t_Team->Support = $_GET["support"];
	
	$t_Team->SkinMid = 0;
	$t_Team->SkinTop = 0;
	$t_Team->SkinJungle = 0;
	$t_Team->SkinMarksman = 0;
	$t_Team->SkinSupport = 0;
	
	
	$t_Team->Enabled = 1;
	
	$t_Team->Wins = 0;
	$t_Team->Losses = 0;
	$t_Team->Kills = 0;
	$t_Team->Deaths = 0;
	$t_Team->CreepScore = 0;
	$t_Team->Save();

	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	if($t_Player->MainTeam == 0)
	{
		$t_InsertedTeam = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("id")->IsLastID());
		if(is_object($t_InsertedTeam) && $t_InsertedTeam->LoadFailed == false)
			$t_Player->MainTeam = $t_InsertedTeam->Id;

		$t_Player->Save();
	}
	die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
