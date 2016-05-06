<?php
if(!defined("INCLUDED")) 
	die();

$g_TeamFight = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	while(true)
	{
		// Copied code from invade
		$t_Team = array();
		$t_Team[] = $g_Game->GetTeamEfficiency(0) * $g_Settings["teamfight_kill_efficiency_modifier"];
		$t_Team[] = $g_Game->GetTeamEfficiency(1) * $g_Settings["teamfight_kill_efficiency_modifier"];
		
		if($t_Team[0] <= 0.01 || $t_Team[1] <= 0.01)
			break;
		
		$t_Total = $t_Team[0] + $t_Team[1];
		
		$t_TeamKills[0] = (int)($t_Team[0]/$t_Team[1]);
		$t_TeamKills[1] = (int)($t_Team[1]/$t_Team[0]);
		
		$t_Roll = (double)(mt_rand(0,1000)) * 0.001;
		$t_Roll *= $t_Total;
		
		if($t_Roll < $t_Team[0])
		{
			$t_Killer = $g_Game->GetRandomActivePlayer(0);
			$t_Deathee = $g_Game->GetRandomActivePlayer(1);
			$g_Game->Kill($t_Killer, $t_Deathee);
		}
		else
		{
			$t_Killer = $g_Game->GetRandomActivePlayer(1);
			$t_Deathee = $g_Game->GetRandomActivePlayer(0);
			$g_Game->Kill($t_Killer, $t_Deathee);
		}
	}
		
	$g_Game->Time += 25;
};

$g_Dragon = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	if($g_Game->Time < TimeConv(2,0))
		return;
	
	$t_Dragon = (float)(6000 + 100 * (int)($g_Game->Time));
	// TODO
};

$g_Baron = function(Player $a_Player)
{
	global $g_Game;
	global $g_CouldTilt;
	
	if($g_Game->Time < TimeConv(20,0))
		return;
	
	// TODO
};