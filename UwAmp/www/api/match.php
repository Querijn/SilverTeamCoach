<?php
if(!defined("INCLUDED")) 
	die();

function GetChampionInGameTeamArray($a_Game, $a_Team, $a_Role)
{
	foreach($a_Game['Teams'][$a_Team] as $t_Champion)
	{
		if($t_Champion['Role'] == strtolower($a_Role))
			return $t_Champion;
	}
}

function SaveMatch($a_Game, $a_GameInfo, $a_MatchType)
{
	if(IsLoggedIn() == false)
		throw new Exception("User is not logged in!");
	
	// You need to battle the next bot next time
	if($a_MatchType == "bot" && $a_Game->Winner == 0)
	{
		$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
		$t_Player->BotsBeaten = $t_Player->BotsBeaten + 1;
		$t_Player->Save();
	}
	
	// unset($a_Game['Timeline']);
	// print_r($a_GameInfo['teams'][($i+1)%2]['player']['name']);
		
	$t_Match = new DatabaseMatch();
	for($i = 0; $i < 2; $i++)
	{
		// Don't save settings for bots
		if($a_MatchType == "bot" && $i == 1)
			continue;
		
		$t_Team = new DatabaseMatchStat();
		
		$t_Team->TeamId = $a_GameInfo['teams'][$i]['team']['Id'];
		
		$t_Roles = array("Mid", "Top", "Jungle", "Support", "Marksman");
		foreach($t_Roles as $t_Role)
		{
			$t_Champion = new DatabaseMatchChampionStat();
				
			$t_PlayerChampion = DatabaseChampion::Load(SQLSearch::In(DatabaseChampion::Table)->
				Where("player_id")->Is($a_GameInfo['teams'][$i]['player']['db']['Id'])->
				Also("champion_id")->Is($a_GameInfo['teams'][$i]['champions'][strtolower($t_Role)]['id']));
				
			$t_Reference = GetChampionInGameTeamArray($a_Game, $i, $t_Role);
			
			$t_Champion->Kills = $t_Reference["Kills"];
			$t_Champion->Deaths = $t_Reference["Deaths"];
			$t_Champion->CreepScore = (float)($t_Reference["Efficiency"])/100.0;
			
			
			$t_PlayerChampion->Kills += $t_Reference["Kills"];
			$t_PlayerChampion->Deaths += $t_Reference["Deaths"];
			$t_PlayerChampion->CreepScore += $t_Champion->CreepScore;
			
			if($a_Game['Winner'] == $i)
				$t_PlayerChampion->Wins = $t_PlayerChampion->Wins + 1;
			else $t_PlayerChampion->Losses = $t_PlayerChampion->Losses + 1;
			
			$t_Champion->Save();
			$t_PlayerChampion->Save();
			
			$t_Team->{$t_Role} = $t_Champion->Id;
		}
		
		$t_Team->Save();
		$t_Match->{'Team'.($i+1)} = $a_GameInfo['teams'][$i]['team']['Id'];
		$t_Match->{'Team'.($i+1).'Stats'} = $t_Team->Id;
		
		
		if($a_Game['Winner'] == $i)
			CreateMessage($a_GameInfo['teams'][$i]['player']['db']['Id'], "Congratulations!", "You just won a match against " . $a_GameInfo['teams'][($i+1)%2]['player']['name'] . "!", $i != 0);
		else CreateMessage($a_GameInfo['teams'][$i]['player']['db']['Id'], "You have lost!", "You just lost a match against " . $a_GameInfo['teams'][($i+1)%2]['player']['name'] . ".", $i != 0);
	}
	$t_Match->Save();
}