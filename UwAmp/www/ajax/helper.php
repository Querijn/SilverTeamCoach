<?php
require_once("include.php");

$t_Words = "inhibitor_destroyed,
            game_over,
            kill,
            death,
            surrender,
            play,
            end_of_timeline,
            laning_phase,
            post_laning_phase,

            bot_tower_attack,
            mid_tower_attack,
            top_tower_attack,
            base_tower_attack,

            bot_tower,
            mid_tower,
            top_tower,
            base_tower,

            teamfight,
            dragon,
            baron,";
			
$t_Words = explode(",", trim($t_Words));

foreach($t_Words as $t_Word)
{
	$t_Total = "";
	$t_Wordz = explode("_", $t_Word);
	if(!is_array($t_Wordz))
		$t_Wordz = array($t_Word);
	foreach($t_Wordz as $t_Worda)
		$t_Total .= ucfirst(strtolower($t_Worda));
		
	echo ucfirst($t_Total).",<br>";
}