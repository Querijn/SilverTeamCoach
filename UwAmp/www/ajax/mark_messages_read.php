<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

try
{	
	if(MarkAllMessagesRead(0))
		die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
