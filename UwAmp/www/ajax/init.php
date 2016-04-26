<?php
require_once("include.php");

$settings["money_per_champion_point"];

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));
	
try
{
	//$_SESSION["region"]
	$t_API = new riotapi($settings["riot_key"], 'euw', new FileSystemCache("cache"));	
	
	$t_Info = array
	(
		"summoner" => $_SESSION["summoner"],
		"mastery" => $t_API->getChampionMastery('22929336')
	);
	echo json_encode($t_Info);
	
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
