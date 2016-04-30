<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{
	$t_Players = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	
	
	$t_StaticAPI = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 3600);
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 5);
	$t_Champions = $t_StaticAPI->getStatic('champion?dataById=true&champData=image');
	
	$t_Info = $_SESSION["summoner"];
	if($t_Players->MainTeam != 0)
	{
		$t_MainTeam = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("id")->Is($t_Players->MainTeam));
		
		if(is_object($t_MainTeam) && $t_MainTeam->LoadFailed == false)
			$t_Info["main_team"]["name"] = $t_MainTeam->Name;
		
		// Nothing else is necessary so far
	}
		
	$t_Info["cash"] = $t_Players->Cash;
	$t_Info["champions"] = array();
	
	// Make sure the champion array is generic array
	foreach($t_Champions["data"] as &$t_Champion)
		$t_Info["champions"][] = $t_Champion;
	
	// Make sure the mastery is usable
	$t_Mastery = array();
	$t_TotalPoints = 0;
	foreach($t_API->getChampionMastery($t_Info["id"]) as $t_CurrentMastery)
	{
		$t_TotalPoints = $t_CurrentMastery["championPoints"];
		$t_Mastery[$t_CurrentMastery["championId"]] = $t_CurrentMastery;
	}
	
	$t_TotalPoints -= $t_Players->StartingCash;
	if($t_TotalPoints > 0)
	{
		$t_TotalPoints *= $settings["cash_per_champion_point"];
		$t_Players->Cash += $t_TotalPoints;
		$t_Players->Save();
	}
	
	// Get all the prices
	$t_Prices = GetChampionPrices();
	
	// For every champ
	foreach($t_Info["champions"] as &$t_Champion)
	{
		// Setup the price
		$t_Champion["price"] = $t_Prices[$t_Champion["name"]];

		// Setup the mastery
		if(isset($t_Mastery[$t_Champion['id']]))
		{
			$t_Champion["mastery"] = $t_Mastery[$t_Champion['id']];
		}
		
		// Setup the owned boolean
		if(!is_null($t_Players->OwnedChampions))
		$t_Champion["owned"] = in_array($t_Champion["id"], $t_Players->OwnedChampions) ? "true" : "false";
	}
	
	echo json_encode($t_Info);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
