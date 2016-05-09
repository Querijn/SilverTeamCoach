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

$g_StartDragon = function(Player $a_Player)
{
	global $g_Events;
	global $g_Game;
	
	$g_Game->AddEvent($g_Events["dragon"], $a_Player);
};

$g_StartBaron = function(Player $a_Player)
{
	global $g_Events;
	global $g_Game;
	
	$g_Game->AddEvent($g_Events["baron"], $a_Player);
};

$g_DefeatedDragon = function(Player $a_Player)
{
	global $g_Events;
	global $g_Game;
	global $g_Settings;
	
	foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
	{
		$t_Player->Efficiency *= $g_Settings["baron_efficiency_modifier"];
	}
	
	$g_Game->DragonUpAt = $g_Game->Time + TimeConv(6,0);
};

$g_DefeatedBaron = function(Player $a_Player)
{
	global $g_Events;
	global $g_Game;
	global $g_Settings;
	
	foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
	{
		$t_Player->HasBaronUntil = $g_Game->Time + $g_Settings["baron_duration"];
	}
	
	$g_Game->BaronUpAt = $g_Game->Time + TimeConv(7,0);
};

$g_Dragon = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	global $g_CouldTiltEveryone;
	
	if($g_Game->Time < TimeConv(2,0) || $g_Game->Time < $g_Game->DragonUpAt  || $g_Game->HasActivePlayer($a_Player->Team) == false)
		return;
	
	$t_Dragon = (float)(6000 + 100 * (int)($g_Game->Time));
	$t_Team = $g_Game->GetTeamEfficiency($a_Player->Team);
	
	$t_Roll = (double)(mt_rand(0,1000)) * 0.001;
	$t_Roll *= ($t_Dragon + $t_Team);
	
	$g_Game->Time += 20;
	if($t_Roll < $t_Dragon)
	{
		$t_Kills = mt_rand(1,2);
		for($i = 0; $i< $t_Kills; $i++)
		{
			if($g_Game->HasActivePlayer($a_Player->Team) == false)
				break;
			$g_Game->Time += 6;
			$g_Game->AddEvent($g_Events["executed"], $g_Game->GetRandomActivePlayer($a_Player->Team));
		}
		
		$g_CouldTiltEveryone($a_Player);
	}
	else
	{
		$g_Game->AddEvent($g_Events["get_dragon"], $a_Player);
	}
};

$g_Baron = function(Player $a_Player)
{
	global $g_Game;
	global $g_CouldTilt;
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	global $g_CouldTiltEveryone;
	
	if($g_Game->Time < TimeConv(20,0) || $g_Game->Time < $g_Game->BaronUpAt || $g_Game->HasActivePlayer($a_Player->Team) == false)
		return;
	
	$t_Baron = (float)(12000 + 50 * (int)($g_Game->Time));
	$t_Team = $g_Game->GetTeamEfficiency($a_Player->Team);
	
	$t_Roll = (double)(mt_rand(0,1000)) * 0.001;
	$t_Roll *= ($t_Baron + $t_Team);
	
	$g_Game->Time += 20;
	if($t_Roll < $t_Baron)
	{
		$t_Kills = mt_rand(1,4);
		for($i = 0; $i< $t_Kills; $i++)
		{
			if($g_Game->HasActivePlayer($a_Player->Team) == false)
				break;
			
			$g_Game->Time += 10;
			$g_Game->AddEvent($g_Events["executed"], $g_Game->GetRandomActivePlayer($a_Player->Team));
		}
		
		$g_CouldTiltEveryone($a_Player);
	}
	else
	{
		$g_Game->Time += 40;
		$g_Game->AddEvent($g_Events["get_baron"], $a_Player);
	}
};