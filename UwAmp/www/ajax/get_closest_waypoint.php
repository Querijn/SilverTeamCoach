<?php
header('Access-Control-Allow-Origin: *');
require_once("include.php");
require_once(EMULATION_FOLDER."include.php");
if(!IsLoggedIn())
	die("You are not logged in.");

if($settings['testing'] == false)
	die();

$x = (float)$_GET['x'];
$y = (float)$_GET['y'];

var_dump(Path::GetClosestWaypoint($x, $y));