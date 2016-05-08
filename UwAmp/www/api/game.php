<?php
if(!defined("INCLUDED")) 
	die();

function GetOpponent($a_API, DatabasePlayer $a_Player, $a_MatchType)
{
	switch(strtolower($a_MatchType))
	{
	case "bot":
		return GetBot($a_API, $a_Player);
	case "lcs":
		throw new Exception("Challenger is not setup.");
		//return GetLCS($a_API, $a_Player);
	case "ranked":
		return GetRanked($a_API, $a_Player);
	case "challenger":
		throw new Exception("Challenger is not setup.");
		//return GetChallenger($a_API, $a_Player);
	}
}

function SetupGamePlayerArray($a_Name, DatabasePlayer $a_DB, $a_SummonerInfo = null)
{
	return array
	(
		"name" => $a_Name,
		"db" => $a_DB,
		"summoner" => $a_SummonerInfo
	);
}

function GetRanked($a_API, DatabasePlayer $a_Player)
{
	$t_Players = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("title")->Is('Player')->Also('Developer')->Also('id')->IsNot($a_Player->Id));
	if(is_object($t_Players))
	{
		if($t_Players->LoadFailed)
			throw new Exception("No other players were found!");
		
		$t_Players = array($t_Players);
	}
	
	$t_Player = $t_Players[mt_rand(0, count($t_Players)-1)];
	
	return SetupGamePlayerArray($a_API->GetSummoner($t_Player->User)[$t_Player->User]["name"], $t_Player);
}

function GetBot($a_API, DatabasePlayer $a_Player)
{
	$t_Bots = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("title")->Is('Bot'));
	if(is_object($t_Bots))
	{
		if($t_Bots->LoadFailed)
			throw new Exception("No bots were found!");
		
		$t_Bots = array($t_Bots);
	}
	
	$t_BotCount = count($t_Bots);
	if($a_Player->BotsBeaten >= $t_BotCount)
	{
		$a_Player->BotsBeaten = $a_Player->BotsBeaten % $t_BotCount;
		$a_Player->Save();
	}
	
	$t_Bot = $t_Bots[$a_Player->BotsBeaten];
	return SetupGamePlayerArray($t_Bot->AlternativeName, $t_Bot);
}

function GetMainTeam($a_API, DatabasePlayer $a_Player)
{
	$t_Team = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("id")->Is($a_Player->MainTeam));
	if(is_object($t_Team) == false || $t_Team->LoadFailed)
		throw new Exception("Something went wrong loading the main team.");
	
	return $t_Team;
}

function IsBot(DatabasePlayer $a_Player)
{
	return $a_Player->Title == 'Bot';
}

$g_CDISetup = false;
$g_CDI = null;
function GetChampionInfoDatabase()
{
	global $g_CDISetup;
	global $g_CDI;
	if($g_CDISetup == false)
	{		
		$g_CDI = json_decode(file_get_contents(AJAX_FOLDER . "data/champions.json"), true);
		$g_CDISetup = true;
	}
	
	return $g_CDI;
}

function CalculateStartingEfficiency($a_Champion)
{
	global $settings;
	$t_MetaCoefficient = $settings["meta_coefficient"];
	$t_MaxWeekModifier = $settings["max_week_modifier"];
	$t_EfficiencyLossPerWeek = $settings["efficiency_loss_per_week"];
	
	// Get data for champion
	$t_CDI = GetChampionInfoDatabase();
	$t_CDIChampion = null;
	foreach($t_CDI as $t_CDIChampionEntry)
		if($t_CDIChampionEntry['key']==$a_Champion['key'] && 
			strtolower($t_CDIChampionEntry['role']) == strtolower($a_Champion['role']))
		{
			$t_CDIChampion = $t_CDIChampionEntry;
			break;
		}
		
	// Get play rate generally
	$playRate = 0.0;
	if(is_null($t_CDIChampion) == false)
		$playRate = $t_CDIChampion['general']['playPercent'] * 0.01;
		
	// base efficiency.
	$t_Efficiency = $a_Champion['mastery']['points'];
	
	// Meta modifier (using playrate)
	$t_MetaModifier = 1.0 + (min(max($playRate, 0.0), 2.0 * $t_MetaCoefficient) - $t_MetaCoefficient);
	$t_Efficiency *= $t_MetaModifier;

	// Time unspent playing modifier
	$t_WeeksUnplayed = (time() - $a_Champion['mastery']['last_played'] * 0.001) / (60*60*24*7);
	$t_ClampedWeeks = min(max($t_WeeksUnplayed, 0.0), $t_MaxWeekModifier);
	$t_LossModifier = ($t_Efficiency * $t_EfficiencyLossPerWeek * $t_ClampedWeeks);
	$t_Efficiency -= $t_LossModifier;
	
	return $t_Efficiency;//array("playRate" => $playRate, "efficiency" => $t_Efficiency, "meta_modifier"=>$t_MetaModifier, "loss_modifier"=>$t_LossModifier);
}

function GetTeamChampions(riotapi $a_API, DatabaseTeam $a_Team, DatabasePlayer $a_Player)
{
	global $settings;
	$t_StaticAPI = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 3600);
	$t_RiotChampions = $t_StaticAPI->getStatic('champion?dataById=true&champData=image,skins');
	$t_RiotMastery = $a_API->getChampionMastery(IsBot($a_Player) ? $_SESSION["summoner"]["id"] : $a_Player->User);
	
	
	$t_DBChampions = DatabaseChampion::Load(SQLSearch::In(DatabaseChampion::Table)->
		Where("player_id")->Is($a_Team->Player)->
		Also("champion_id")->Is($a_Team->Mid)
					->Also($a_Team->Top)
					->Also($a_Team->Jungle)
					->Also($a_Team->Support)
					->Also($a_Team->Marksman));
	
	
	$t_Champions = array();
	foreach($t_DBChampions as $t_DBChampion)
	{
		$t_RiotChampion = $t_RiotChampions["data"][$t_DBChampion->ChampionId];
		$t_RiotChampionMastery = null;
		foreach($t_RiotMastery as $t_RiotMasteryElement)
			if($t_RiotMasteryElement['championId'] == $t_DBChampion->ChampionId)
			{
				$t_RiotChampionMastery = $t_RiotMasteryElement;
				break;
			}
		
		$t_Champion = array();
		$t_Champion["id"] = $t_DBChampion->ChampionId;
		$t_Champion["key"] = $t_RiotChampion["key"];
		$t_Champion["name"] = ($t_DBChampion->SkinId == 0) ? $t_RiotChampion["name"] : $t_RiotChampion["skins"][$t_DBChampion->SkinId]["name"];
		$t_Champion["role"] = "Unknown";
		$t_Champion["skin"] = $t_DBChampion->SkinId;
		switch($t_DBChampion->ChampionId)
		{
		case $a_Team->Top:
			$t_Champion["role"] = "top";
			break;
		case $a_Team->Mid:
			$t_Champion["role"] = "mid";
			break;
		case $a_Team->Jungle:
			$t_Champion["role"] = "jungle";
			break;
		case $a_Team->Support:
			$t_Champion["role"] = "support";
			break;
		case $a_Team->Marksman:
			$t_Champion["role"] = "marksman";
			break;
		}
		
		$t_Mastery = array('championLevel'=>0, 'championPoints' => 0, "lastPlayTime" => time() * 1000);
		if(is_null($t_RiotChampionMastery) == false)
			$t_Mastery = $t_RiotChampionMastery;
		
		$t_Champion["mastery"] = array
		(
			"level" => $t_Mastery['championLevel'],
			"points" => $t_Mastery['championPoints'],
			"last_played" => $t_Mastery['lastPlayTime'],
			"db" => isset($t_RiotChampionMastery) ? $t_RiotChampionMastery : null,
		);
		
		$t_Champion["efficiency"] = CalculateStartingEfficiency($t_Champion);
		$t_Champions[$t_Champion["role"]] = $t_Champion;
	}
	
	return $t_Champions;
}