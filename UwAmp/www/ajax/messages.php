<?php
require_once("include.php");

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{	
	echo json_encode(GetMessages(30));
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}