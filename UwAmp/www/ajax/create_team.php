<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

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
	
	$t_Champions = $t_API->getStatic('champion?dataById=true&champData=image')["data"];
	if(isset($t_Champions[$_GET["mid"]]) == false)
		die("Invalid mid!");
	
	if(isset($t_Champions[$_GET["top"]]) == false)
		die("Invalid top!");
	
	if(isset($t_Champions[$_GET["jungle"]]) == false)
		die("Invalid jungle!");
	
	if(isset($t_Champions[$_GET["marksman"]]) == false)
		die("Invalid marksman!");
	
	if(isset($t_Champions[$_GET["support"]]) == false)
		die("Invalid support!");
	
	$t_Team = new DatabaseTeam();
	$t_Team->Name = $_GET["name"];
	$t_Team->Player = $t_GetID;
	
	$t_Team->Mid = $_GET["mid"];
	$t_Team->Top = $_GET["top"];
	$t_Team->Jungle = $_GET["jungle"];
	$t_Team->Marksman = $_GET["marksman"];
	$t_Team->Support = $_GET["support"];
	
	$t_Team->Wins = 0;
	$t_Team->Losses = 0;
	$t_Team->Kills = 0;
	$t_Team->Deaths = 0;
	$t_Team->CreepScore = 0;
	$t_Team->Save();

	die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
