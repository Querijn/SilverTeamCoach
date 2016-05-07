<?php
ob_start();
require_once("include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{
	$t_Game = null;
	if(isset($_GET["sec"]) == false || is_numeric($_GET['sec'] == false))
		throw new Exception("You need to supply how many seconds you want to see.");
	
	if(isset($_GET["start"]) == false || is_numeric($_GET['start'] == false))
		throw new Exception("You need to supply the start time.");
	
	$t_Start = $_GET["sec"] * $_GET["start"];// * $settings["game_speed"];
	$t_End = $t_Start + $_GET["sec"];// * $settings["game_speed"];
	$t_File = file_get_contents(AJAX_FOLDER. "data/test_match.json");
	$t_Game = json_decode($t_File, true);		
	$t_Display = array();
	foreach($t_Game as $t_Key=>$t_Event)
	{
		if($t_Key >= $t_Start)
		{
			if($t_Key <= $t_End)
				$t_Display[] = array("time"=>$t_Key, "events"=>$t_Event);
			else break;
		}
	}
	if(isset($_GET["var_dump"]))
	{
		echo "Showing from ".$t_Start." until ".$t_End."\n";
		print_r($t_Display);
	}
	else echo json_encode($t_Display);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
