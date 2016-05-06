<?php
if(!defined("INCLUDED")) 
	die();

function ConvertToElo($a_League, $a_Division, $a_LP = 0)
{
	$t_Number = 0;
	$t_ModifierPerLP = 0.7;
	$t_LP = $a_LP;
	switch(strtoupper($a_League))
	{
		case "BRONZE":
			$t_Number = 800;
			break;
		default:
		case "SILVER":
			$t_Number = 1150;
			break;
		case "GOLD":
			$t_Number = 1500;
			break;
		case "PLATINUM":
			$t_Number = 1850;
			break;
		case "DIAMOND":
			$t_Number = 2200;
			break;
		case "MASTER":
			$t_Number = 2550;
			$t_ModifierPerLP = 0.05;
			break;
		case "CHALLENGER":
			$t_Number = 2900;
			$t_ModifierPerLP = 0.05;
			break;
	}
	
	if($a_League != "MASTER" && $a_League != "CHALLENGER")
	{
		switch($a_Division)
		{
			case 1:
			case "I":
				$t_LP += 100;
			case 2:
			case "II":
				$t_LP += 100;
			case 3:
			case "III":
				$t_LP += 100;
			case 4:
			case "IV":
				$t_LP += 100;
			case "V":
			case 5:
				break;
		}
	}
	
	return $t_Number + $t_LP * $t_ModifierPerLP;
}

function GetElos(riotapi $a_API, $a_EloPlayers)
{
	$t_Elo = array();
	
	$t_LeagueResult = $a_API->getLeague(implode(",", $a_EloPlayers), "entry");
	foreach($a_EloPlayers as $t_EloPlayer)
	{
		$t_Leagues = $t_LeagueResult[$t_EloPlayer];
		foreach($t_Leagues as $t_Queue)
		{
			if($t_Queue["queue"] != 'RANKED_SOLO_5x5')
				continue;
			
			foreach($t_Queue["entries"] as $t_Entry)
			{
				if($t_Entry["playerOrTeamId"] == $t_EloPlayer)
				{
					$t_Elo[$t_EloPlayer] = ConvertToElo($t_Queue["tier"], $t_Entry["division"], $t_Entry["leaguePoints"]);
					break;
				}
			}
		}
	}
	
	return $t_Elo;
}