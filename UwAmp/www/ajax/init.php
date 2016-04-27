<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

global $t_API;
try
{
	$t_Players = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache("cache"));
	$t_Champions = $t_API->getStatic('champion?dataById=true');
	
	$t_Info = $_SESSION["summoner"];
	$t_Info["cash"] = $t_Players->Cash;
	$t_Info["champions"] = array();
	
	foreach($t_Champions["data"] as &$t_Champion)
	{
		$t_Info["champions"][] = $t_Champion;
	}
	
	$t_Prices = GetChampionPrices();
	foreach($t_Info["champions"] as $t_Key=>&$t_Champion)
	{
		$t_Champion["price"]
		= 
		$t_Prices[$t_Champion["name"]];
	}
	
	echo json_encode($t_Info);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
