<?php
header('Access-Control-Allow-Origin: *');
require_once("include.php");
if(!IsLoggedIn())
	die("You are not logged in.");

if($settings['testing'] == false)
	die();

$t_OriginalContent = @file_get_contents(EMULATION_FOLDER . "waypoints.json");
if($t_OriginalContent === false){ $t_OriginalContent = "[]";}

$t_JSON = json_decode($t_OriginalContent, true);
$t_AddJSON = json_decode($_GET["waypoints"], true);
 
foreach($t_AddJSON as $t_Key=>$t_Element)
{
	$t_JSON[] = $t_Element;
}

$t_File = fopen(EMULATION_FOLDER . "waypoints.json", "w");

fwrite($t_File, json_encode($t_JSON));

fclose($t_File);