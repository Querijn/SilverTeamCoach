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

$g_Death = function($a_Player)
{
	global $g_Game;
	global $g_CouldTilt;
	global $g_CouldTiltEveryone;
	global $g_Settings;
	$a_Player->Efficiency *= $g_Settings["death_efficiency_modifier"];
	
	$a_Player->DeadUntil = $g_Game->Time + $a_Player->Level * 2.5 + 7.5;
	$a_Player->Deaths++;
	if($g_Game->Time > 10*60 && $g_Game->Time < 60*60)
		$a_Player->DeadUntil += $a_Player->DeadUntil * 0.01 * (($g_Game->Time/60)-10);
	else if($g_Game->Time >= 60*60)
		$a_Player->DeadUntil += $a_Player->DeadUntil * 0.5;
	
	$g_CouldTilt($a_Player);
	
	if(($a_Player->Kills / ($a_Player->Deaths + 0.01)) < 0.3)
		$g_CouldTiltEveryone($a_Player);
};