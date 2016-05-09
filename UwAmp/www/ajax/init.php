<?php
ob_start();
require_once("include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{
	$t_Players = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	$t_DBChampions = DatabaseChampion::Load(SQLSearch::In(DatabaseChampion::Table)->Where("player_id")->Is($t_Players->Id));
	if(is_object($t_DBChampions))
	{
		if($t_DBChampions->LoadFailed)
			$t_DBChampions = array();
		else $t_DBChampions = array($t_DBChampions);
	}
	
	$t_OwnedChampions = array();
	foreach($t_DBChampions as $t_OwnedChampion)
	{
		$t_OwnedChampions[$t_OwnedChampion->ChampionId] = $t_OwnedChampion;
	}
		
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
		
	$t_Info["messages"] = GetMessages(30);
	$t_Info["game_id"] = $t_Players->Id;
	$t_Info["sound_volume"] = $t_Players->SoundVolume;
	$t_Info["music_volume"] = $t_Players->MusicVolume;
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
		$t_TotalPoints += $t_CurrentMastery["championPoints"];
		$t_Mastery[$t_CurrentMastery["championId"]] = $t_CurrentMastery;
	}
	
	$t_TotalPoints -= ($t_Players->StartingCash / $settings["starting_cash_per_champion_point"]);
	if($t_TotalPoints > 0)
	{
		$t_TotalPoints *= $settings["cash_per_champion_point"];
		$t_Players->Cash += $t_TotalPoints;
		$t_Players->StartingCash = $t_Players->Cash;
		$t_Players->Save();
		
		CreateMessage($t_Players->Id, "Money earned", "You gained {CashSign}".number_format($t_TotalPoints, 2, '.', ',')." for gaining champion points in League of Legends.", true);
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
		
		
		if(isset($t_OwnedChampions[$t_Champion["id"]]))
		{
			$t_Champion["personal"]["wins"] = $t_OwnedChampions[$t_Champion["id"]]->Wins;
			$t_Champion["personal"]["losses"] = $t_OwnedChampions[$t_Champion["id"]]->Losses;
			
			$t_Champion["personal"]["kills"] = $t_OwnedChampions[$t_Champion["id"]]->Kills;
			$t_Champion["personal"]["deaths"] = $t_OwnedChampions[$t_Champion["id"]]->Deaths;
			$t_Champion["personal"]["creep_score"] = $t_OwnedChampions[$t_Champion["id"]]->CreepScore;
		}
		else
		{
			$t_Champion["personal"]["wins"] = 0;
			$t_Champion["personal"]["losses"] = 0;
			
			$t_Champion["personal"]["kills"] = 0;
			$t_Champion["personal"]["deaths"] = 0;
			$t_Champion["personal"]["creep_score"] = 0;
		}
	}
	
	if(isset($_GET['var_dump']))
		print_r($t_Info);
	else echo json_encode($t_Info);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
