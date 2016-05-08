<?php
ob_start();
require_once("include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{	
	$t_File = file_get_contents(AJAX_FOLDER. "data/test_match.json");
	$t_Game = json_decode($t_File, true);	
	
	if(isset($_GET["var_dump"]))
	{
		print_r($t_Game);
	}
	else echo json_encode($t_Game);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
