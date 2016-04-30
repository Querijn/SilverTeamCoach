<?php
if(!defined("INCLUDED")) 
	die();

require_once(RIOT_FOLDER . "FileSystemCache.php");
require_once(RIOT_FOLDER . "php-riot-api.php");

$g_NameTranslationSetup = false;
$g_NameTranslationTable = array();

function SetupChampionNameTranslation()
{
	global $g_NameTranslationSetup;
	global $g_NameTranslationTable;
	global $settings;
	if($g_NameTranslationSetup)
		return;
	
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 3600);
	$t_Champions = $t_API->getStatic('champion?dataById=true&champData=image');
	
	foreach($t_Champions["data"] as $t_Champion)
		$g_NameTranslationTable[$t_Champion["name"]] = $t_Champion["id"];
}

function GetChampionIDByName($a_Name)
{	
	global $g_NameTranslationTable;
	SetupChampionNameTranslation();
	if(isset($g_NameTranslationTable[$a_Name]))
		return $g_NameTranslationTable[$a_Name];
	else return -1;
}