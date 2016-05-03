<?php
if(!defined("INCLUDED")) 
	die();

$g_MoveToPullOrProtect = function(Player $a_Player)
{
	$t_Invade = (double)(mt_rand(0,1000)) * 0.001;
	// if($t_Invade < $g_Settings["invade_chance"])
	// {
		// //$g_Invade($a_Player);
	// }
	// else
	{
		// Determine jungle start 
		$t_Start = (double)(mt_rand(0,1000)) * 0.001;
		if($t_Start >= 0.5)
			$t_Start = "red";
		else $t_Start = "blue";
		
		
	}
};