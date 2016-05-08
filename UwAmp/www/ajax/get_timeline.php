<?php
ob_start();
require_once("include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{
	if($settings["testing"]==true && file_exists(AJAX_FOLDER. "data/game.json"))
	{
		$_SESSION["game"] = file_get_contents(AJAX_FOLDER. "data/game.json");
		$_SESSION["game_info"] = file_get_contents(AJAX_FOLDER. "data/game_info.json");
	}
	
	$t_Game = null;
	if(isset($_SESSION["game"]) == false)
		throw new Exception("You need to be in a match to get its resources");
	
	$t_Game = json_decode($_SESSION["game"], true);		
	$t_Display = array();
	foreach($t_Game["Timeline"] as $t_Key=>$t_Event)
	{
		$t_Display[] = $t_Event;;
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
