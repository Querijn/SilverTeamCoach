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
	if($a_Player->Efficiency < $g_Settings["mastery_points_level"][2])
	{
		$t_Chance = $g_Settings["afk_chance_0_points"] - $a_Player->Efficiency*($g_Settings["afk_chance_0_points"] / $g_Settings["mastery_points_level"][2]);
		
		$t_AFKRoll = (double)(mt_rand(0,1000)) * 0.001;
		if($t_AFKRoll < $t_Chance)
		{
			$t_Event = $g_Game->Time == 0 ? $g_Events["no_connect"] : $g_Events["afk"];
			$g_Game->AddEvent($t_Event, $a_Player);
			$t_AFKTimer = mt_rand($g_Settings["afk_time"]->x, $g_Settings["afk_time"]->y) ** 2;
			$a_Player->TrollingUntil = $g_Game->Time + $t_AFKTimer;
		}
	}
};

$g_CouldTiltEveryone = function(Player $a_Player)
{
	global $g_Events;
	global $g_Settings;
	global $g_Game;
	
	// You can't be more away from keyboard.
	if($a_Player->IsAFK($g_Game->Time))
		return;
	
	// Solid for an afk chance here
	if($a_Player->Efficiency < $g_Settings["mastery_points_level"][2])
	{
		$t_Chance = $g_Settings["afk_chance_0_points"] - $a_Player->Efficiency*($g_Settings["afk_chance_0_points"] / $g_Settings["mastery_points_level"][2]);
		
		$t_AFKRoll = (double)(mt_rand(0,1000)) * 0.001;
		if($t_AFKRoll < $t_Chance)
		{
			$t_Event = $g_Game->Time == 0 ? $g_Events["no_connect"] : $g_Events["afk"];
			$g_Game->AddEvent($t_Event, $a_Player);
			$t_AFKTimer = mt_rand($g_Settings["afk_time"]->x, $g_Settings["afk_time"]->y) ** 2;
			$a_Player->TrollingUntil = $g_Game->Time + $t_AFKTimer;
		}
	}
};