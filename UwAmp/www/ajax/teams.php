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
	
	$t_Teams = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("player")->Is($t_GetID));

	$t_Info = array();
	if(is_array($t_Teams))
	{
		foreach($t_Teams as $t_Team)
		{
			$t_Info[] = array
			(
				"name" => $t_Team->Name,
				
				"top" => $t_Team->Top,
				"mid" => $t_Team->Mid,
				"marksman" => $t_Team->Marksman,
				"support" => $t_Team->Support,
				"jungle" => $t_Team->Jungle,
			);
		}
	}
	
	echo json_encode($t_Info);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
