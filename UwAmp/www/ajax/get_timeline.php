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
	if(isset($_SESSION["game"]) == false)
		throw new Exception("You need to be in a match to get its resources");
	
	if(isset($_GET["sec"]) == false || is_numeric($_GET['sec'] == false))
		throw new Exception("You need to supply how many seconds you want to see.");
	
	if(isset($_GET["start"]) == false || is_numeric($_GET['start'] == false))
		throw new Exception("You need to supply the start time.");
	
	$t_Game = json_decode($_SESSION["game"], true);		
	$t_Display = array();
	foreach($t_Game["Timeline"] as $t_Key=>$t_Event)
	{
		if($t_Key <= $_GET["sec"] * $settings["game_speed"])
			$t_Display[$t_Key] = $t_Event;
		else break;
	}
	echo json_encode($t_Display);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
