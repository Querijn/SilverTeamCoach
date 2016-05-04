<?php
header('Access-Control-Allow-Origin: *');
require_once("include.php");
if(!IsLoggedIn())
	die("You are not logged in.");

if($settings['testing'] == false)
	die();

$t_File = @file_get_contents(EMULATION_FOLDER . "waypoints.json");

if($t_File === false)
	echo "";
else echo $t_File;