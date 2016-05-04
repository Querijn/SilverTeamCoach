<?php
header('Access-Control-Allow-Origin: *');
require_once("include.php");
if(!IsLoggedIn())
	die("You are not logged in.");

if($settings['testing'] == false)
	die();

$t_File = fopen(EMULATION_FOLDER . "waypoints.json", "w");

fwrite($t_File, $_GET["waypoints"]);
fclose($t_File);