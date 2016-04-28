<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

$t_GetID = $_SESSION["summoner"]["id"];

if(isset($_GET["id"]) && is_numeric($_GET["id"]))
{
	$t_GetID = $_GET["id"];
}

try
{
	$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"));
	if(isset($_GET["get_all"]))
	{
		$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($t_GetID));
		
		if(is_object($t_Player) && $t_Player->LoadFailed == false && $t_Player->Admin == true)
		{
			$t_Champions = $t_API->getStatic('champion?dataById=true&champData=image');
			
			$t_OwnedChampions = array();
			foreach($t_Champions["data"] as $t_Champion)
				$t_OwnedChampions[] = $t_Champion["id"];
				
			$t_Player->OwnedChampions = $t_OwnedChampions;
			$t_Player->Save();
		}
	}
	else if(isset($_GET["lose_all"]))
	{
		$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($t_GetID));
		
		if(is_object($t_Player) && $t_Player->LoadFailed == false && $t_Player->Admin == true)
		{
			// Always have Annie
			$t_Player->OwnedChampions = array(1);
			$t_Player->Save();
		}
	}

	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($t_GetID));
	
	if(is_object($t_Player) && $t_Player->LoadFailed == false)
		echo json_encode($t_Player->OwnedChampions);
	else echo json_encode(array());
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
?>