<?php
if(!defined("INCLUDED")) 
	die();

// Generic AFK/Troll/Tilt functionality
$g_CouldAFK = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	
	// You can't be more away from keyboard.
	if($a_Player->IsAFK($g_Game->Time))
		return;
	
	// Solid for an afk chance here
	if($a_Player->GetEfficiency() < $g_Settings["mastery_points_level"][2])
	{
		$t_Chance = $g_Settings["afk_chance_0_points"] - $a_Player->GetEfficiency() * ($g_Settings["afk_chance_0_points"] / $g_Settings["mastery_points_level"][2]);
		
		$t_AFKRoll = (double)(mt_rand(0,1000)) * 0.001;
		if($t_AFKRoll < $t_Chance)
		{
			$t_Event = $g_Game->Time == 0 ? $g_Events["no_connect"] : $g_Events["afk"];
			$g_Game->AddEvent($t_Event, $a_Player);
			$t_AFKTimer = mt_rand($g_Settings["afk_time"]->x, $g_Settings["afk_time"]->y) ** 2;
			$a_Player->AFKUntil = $g_Game->Time + $t_AFKTimer;
		}
	}
};

$g_ShouldSurrender = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	if($g_Game->Time < TimeConv(20,0))
		return;
	
	$t_AFKCount = 0;
	$t_TrollCount = 0;
	$t_DeathOverflow = 0;
	foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
	{
		if(($t_Player->AFKUntil - $g_Game->Time) > TimeConv(5,0))
			$t_AFKCount++;
		
		if(($t_Player->TrollingUntil - $g_Game->Time) > TimeConv(5,0))
			$t_TrollCount++;
		
		$t_DeathOverflow += $t_Player->Deaths;
		$t_DeathOverflow -= $t_Player->Kills;
	}
	
	if($t_AFKCount + $t_TrollCount >= 3 || $t_DeathOverflow >= 20)
	{
		$g_Game->AddEvent($g_Events["surrender"], $a_Player->Team);
	}
};

$g_CouldTilt = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	global $g_CouldAFK;
	
	// You can't be more away from keyboard.
	if($a_Player->IsAFK($g_Game->Time))
		return;
	
	if($a_Player->GetEfficiency() < $g_Settings["mastery_points_level"][2])
	{
		$t_Chance = $g_Settings["tilt_chance_0_points"] - $a_Player->GetEfficiency() * ($g_Settings["tilt_chance_0_points"] / $g_Settings["mastery_points_level"][2]);
		
		$t_Roll = (double)(mt_rand(0, 1000)) * 0.001;
		if($t_Roll < $t_Chance)
		{
			$g_Game->AddEvent($g_Events["tilt"], $a_Player);
			$t_Timer = mt_rand($g_Settings["tilt_time"]->x, $g_Settings["tilt_time"]->y) ** 2;
			$a_Player->TiltingUntil = $g_Game->Time + $t_Timer;
		}
		else $g_CouldAFK($a_Player);
	}
};

$g_CouldTiltEveryone = function(Player $a_Player)
{
	global $g_Game;
	global $g_CouldTilt;
	
	// For each team member
	foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
	{
		// Little hack to check if it's the same player
		if($t_Player->Spawn->x == $a_Player->Spawn->x &&
			$t_Player->Spawn->y == $a_Player->Spawn->y)
			continue;
			
		$g_CouldTilt($a_Player);
	}
};