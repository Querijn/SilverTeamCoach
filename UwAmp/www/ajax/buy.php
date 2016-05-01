<?php
require_once("include.php");

if(!IsLoggedIn())
	die("You are not logged in.");

$t_GetID = $_SESSION["summoner"]["id"];

if(!isset($_SESSION["summoner"]["id"]) || !isset($_GET["champion"]) || !is_numeric($_GET["champion"]))
{
	die("Input setup failed.");
}

try
{
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"));
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($t_GetID));
	
	if(in_array($_GET["champion"], $t_Player->OwnedChampions))
	{
		die("You already own this champion.");
	}
	
	$t_Prices = GetChampionPrices();
	$t_Champions = $t_API->getStatic('champion?dataById=true&champData=image')["data"];
	
	$t_Champion = $t_Champions[$_GET["champion"]];
	if(!is_array($t_Champion))
		die("Not a real champion.");
	
	$t_Price = $t_Prices[$t_Champion["name"]];
	if(!isset($t_Price))
		die("No known price.");
	
	if($t_Price > $t_Player->Cash)
	{
		die("Not enough money.");
	}
	
	$t_Player->Cash -= $t_Price;
	$t_Owned = $t_Player->OwnedChampions;
	$t_Owned[] = $_GET["champion"];
	$t_Player->OwnedChampions = $t_Owned;
	$t_Player->Save();
	
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $_GET["champion"];
	$t_Champion->PlayerId = $t_Player->Id;
	$t_Champion->Save();
	die("true");
}
catch(Exception $e)
{
	die($e->getMessage());
}
?>