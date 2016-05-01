<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

try
{	
	if(isset($_GET["id"]) == false || is_numeric($_GET["id"]) == false)
		die("Invalid id given.");
	
	if(MarkMessageRead())
		die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
