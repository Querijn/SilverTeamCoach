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
	
	$t_Game = json_decode($_SESSION["game"], true);
	unset($t_Game["Timeline"]);

	if(isset($_GET["var_dump"]))
	{
		print_r($t_Game);
	}
	else echo json_encode($t_Game);
	unset($_SESSION["game"]);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
