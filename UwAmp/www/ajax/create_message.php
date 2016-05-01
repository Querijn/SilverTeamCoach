<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

$t_GetID = $_GET["id"];

try
{
	if(isset($_GET["id"]) == false || is_numeric($_GET["id"]) == false)
		die("Invalid sender id!");
	
	if(isset($_GET["title"]) == false || is_string($_GET["title"]) == false)
		die("Invalid title!");
	
	if(isset($_GET["message"]) == false || is_string($_GET["message"]) == false)
		die("Invalid message!");
	
	if(isset($_GET["unread"]) == false || is_numeric($_GET["unread"]) == false)
		die("Invalid unread value!");
	
	if(CreateMessage($_GET["id"], $_GET["title"], $_GET["message"], $_GET['unread']))
		die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
