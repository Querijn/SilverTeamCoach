<?php
if(!defined("INCLUDED")) 
	die();

$g_MoveToPullOrProtect = function(Player $a_Player)
{
	global $g_Game;
	global $g_Settings;
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
		{
			$t_PathToGolems = new Path($a_Player->Position, $g_Settings["golems"][$a_Player->Team]);
			foreach($g_Game->Teams[$a_Player->Team] as $t_Player)
			{
				switch($t_Player->Role)
				{
				case 'support':
				case 'marksman':
				case 'jungle':
					$t_Player->Path = $t_PathToGolems;
					break;
				}
			}
		}
		else
		{
			
		}
		
	}
};