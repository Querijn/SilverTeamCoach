<?php
if(!defined("INCLUDED")) 
	die();

$g_Settings["afk_chance_0_points"] = 0.1;
$g_Settings["troll_chance_0_points"] = 0.05;
$g_Settings["tilt_chance_0_points"] = 0.03;

$g_Settings["afk_time"] = new vec2(8, 30);
$g_Settings["tilt_time"] = new vec2(4, 15);

$g_Settings["tilt_efficiency_modifier"] = 0.9;

$g_Settings["invade_chance"] = 0.1;

$g_Settings["movement_speed"] = 350.0;

// You probably shouldn't change anything below here.

// mastery level requirements
$g_Settings["mastery_points_level"][0] = 0;
$g_Settings["mastery_points_level"][1] = 1800;
$g_Settings["mastery_points_level"][2] = 6000;
$g_Settings["mastery_points_level"][3] = 12600;
$g_Settings["mastery_points_level"][4] = 21600;

// Summoner's Rift settings
$g_Settings["map_size"] = 14870.0;
$g_Settings["spawn_point"][0][] = new vec2(561, 581);
$g_Settings["spawn_point"][0][] = new vec2(561, 361);
$g_Settings["spawn_point"][0][] = new vec2(351, 293);
$g_Settings["spawn_point"][0][] = new vec2(222, 471);
$g_Settings["spawn_point"][0][] = new vec2(311, 649);

$g_Settings["spawn_point"][1][] = new vec2(14486, 14511);
$g_Settings["spawn_point"][1][] = new vec2(14486, 14291);
$g_Settings["spawn_point"][1][] = new vec2(14277, 14223);
$g_Settings["spawn_point"][1][] = new vec2(14148, 14401);
$g_Settings["spawn_point"][1][] = new vec2(14237, 14579);

foreach($g_Settings["spawn_point"] as &$t_Team)
	foreach($t_Team as &$t_Spawnpoint) 
		$t_Spawnpoint->Divide($g_Settings["map_size"]);
		
$g_Settings["movement_speed"] /= $g_Settings["map_size"];

// Custom positions
$g_Settings["golems"][0][] = new vec2(302.0/512.0, 420.0/512.0);
$g_Settings["red_buff"][0][] = new vec2(263.0/512.0, 384.0/512.0);
$g_Settings["raptors"][0][] = new vec2(245.0/512.0, 340.0/512.0);
$g_Settings["wolves"][0][] = new vec2(130.0/512.0, 290.0/512.0);
$g_Settings["blue_buff"][0][] = new vec2(125.0/512.0, 235.0/512.0);
$g_Settings["frog"][0][] = new vec2(77.0/512.0, 222.0/512.0);

$g_Settings["golems"][1][] = new vec2(223.0/512.0, 84.0/512.0);
$g_Settings["red_buff"][1][] = new vec2(245.0/512.0, 130.0/512.0);
$g_Settings["raptors"][1][] = new vec2(270.0/512.0, 180.0/512.0);
$g_Settings["wolves"][1][] = new vec2(380.0/512.0, 220.0/512.0);
$g_Settings["blue_buff"][1][] = new vec2(385.0/512.0, 275.0/512.0);
$g_Settings["frog"][1][] = new vec2(440.0/512.0, 290.0/512.0);


$g_Settings["bot_lane"][] = new vec2(440.0/512.0, 430.0/512.0);
$g_Settings["mid_lane"][] = new vec2(256.0/512.0, 256.0/512.0);
$g_Settings["top_lane"][] = new vec2(75.0/512.0, 75.0/512.0);
 