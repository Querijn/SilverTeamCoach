<?php
if(!defined("INCLUDED")) 
	die();

$g_Teamfight = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	// Copied code from invade
	$t_Team[] = $g_Game->GetTeamEfficiency(0) * $g_Settings["teamfight_kill_efficiency_modifier"];
	$t_Team[] = $g_Game->GetTeamEfficiency(1) * $g_Settings["teamfight_kill_efficiency_modifier"];
	$t_Total = $t_Team[0]+$t_Team[1];
	
	$t_TeamKills[0] = (int)($t_Team[0]/$t_Team[1]);
	$t_TeamKills[1] = (int)($t_Team[1]/$t_Team[0]);
	
	$t_HighestTime = 0;
	for($j = 0; $j < 2; $j++)
		for($i = 0; $i < $t_TeamKills[$j]; $i++)
		{
			$t_Roll = (double)(mt_rand(0,1000)) * 0.001;
			$t_Roll *= $t_Total;
			
			$g_Game->Time += pow(mt_rand(-2,5), 2);
			
			if($g_Game->Time > $t_HighestTime)
				$t_HighestTime = $g_Game->Time;
			if($t_Roll < $t_Team[$j])
			{
				$t_Killer = $g_Game->GetRandomActivePlayer($j);
				$t_Deathee = $g_Game->GetRandomActivePlayer(($j + 1)%2);
				$g_Game->Kill($t_Killer, $t_Deathee);
			}
			
		}
	
	$g_Game->Time = $t_HighestTime + 25;
};

$g_Dragon = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	if($g_Game->Time < TimeConv(2,0))
		return;
	
	$t_Dragon = (float)(6000 + 100 * (int)(g_Game->Time));
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