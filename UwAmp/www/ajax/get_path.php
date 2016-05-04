<?php
header('Access-Control-Allow-Origin: *');
require_once("include.php");
require_once(EMULATION_FOLDER."include.php");
if(!IsLoggedIn())
	die("You are not logged in.");

if($settings['testing'] == false)
	die();

$t_From = (int)$_GET['from'];
$t_To = (int)$_GET['to'];

$t_Path = new Path($t_From, $t_To);
echo json_encode($t_Path->Route);