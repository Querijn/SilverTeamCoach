<?php
if(!defined("INCLUDED")) 
	die();

$g_Invade = function($a_Player)
{
	global $g_Game;
	global $g_Settings;
	foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
	{
		$t_Player->Efficiency *= $g_Settings["invade_efficiency_modifier"];
	}
};

$g_Kill = function($a_Player)
{
	global $g_Game;
	global $g_Settings;
	$a_Player->Efficiency *= $g_Settings["kill_efficiency_modifier"];
	$a_Player->Kills++;
};

$g_Executed = function($a_Player)
{
	global $g_Game;
	global $g_Settings;
	global $g_CouldTiltEveryone;
	global $g_CouldTilt;

	//$a_Player->Efficiency *= $g_Settings["death_efficiency_modifier"];
	$a_Player->Deaths++;
	
	$t_Minutes = ($g_Game->Time/60);
	
	// Little hack for levels
	$a_Player->Level = ($g_Game->Time / 60);
	if($a_Player->Level <= 0)
		$a_Player->Level = 1;
	else if($a_Player->Level > 18)
	{
		$a_Player->Level = 18;		
	}
	
	$t_DeathTimer = $a_Player->Level * 2.5 + 7.5;
	
	if($g_Game->Time > 10*60 && $g_Game->Time < 60*60)
		$t_DeathTimer += ($t_DeathTimer * $g_Settings["death_timer_increase_modifier"]) * ($t_Minutes-10);
	
	else if($g_Game->Time >= 60*60)
		$t_DeathTimer += $t_DeathTimer * 0.5;
	
	$a_Player->DeadUntil = $g_Game->Time + $t_DeathTimer;
	
	$g_CouldTilt($a_Player);
	if(($a_Player->Kills / ($a_Player->Deaths + 0.01)) < 0.3)
		$g_CouldTiltEveryone($a_Player);
};

$g_Death = function($a_Player)
{
	global $g_Game;
	global $g_CouldTilt;
	global $g_CouldTiltEveryone;
	global $g_Settings;
	$a_Player->Efficiency *= $g_Settings["death_efficiency_modifier"];
	
	$a_Player->Deaths++;
	
	$t_Minutes = ($g_Game->Time/60);
	
	// Little hack for levels
	$a_Player->Level = ($g_Game->Time / 60);
	if($a_Player->Level <= 0)
		$a_Player->Level = 1;
	else if($a_Player->Level > 18)
	{
		$a_Player->Level = 18;		
	}
	
	$t_DeathTimer = $a_Player->Level * 2.5 + 7.5;
	
	if($g_Game->Time > 10*60 && $g_Game->Time < 60*60)
		$t_DeathTimer += ($t_DeathTimer * $g_Settings["death_timer_increase_modifier"]) * ($t_Minutes-10);
	
	else if($g_Game->Time >= 60*60)
		$t_DeathTimer += $t_DeathTimer * 0.5;
	
	$a_Player->DeadUntil = $g_Game->Time + $t_DeathTimer;
	
	$g_CouldTilt($a_Player);
	if(($a_Player->Kills / ($a_Player->Deaths + 0.01)) < 0.3)
		$g_CouldTiltEveryone($a_Player);
};