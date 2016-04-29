<?php
require_once("include.php");

if(!isset($_SESSION['summoner']))
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

$t_GetID = $_SESSION["summoner"]["id"];

if(isset($_GET["id"]) && is_numeric($_GET["id"]))
{
	$t_GetID = $_GET["id"];
}

function AddTeamToArray($a_Team, &$a_Array)
{
	$a_Array[] = array
	(
		"name" => $a_Team->Name,
		
		"top" => $a_Team->Top,
		"mid" => $a_Team->Mid,
		"marksman" => $a_Team->Marksman,
		"support" => $a_Team->Support,
		"jungle" => $a_Team->Jungle,
	);
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
			AddTeamToArray($t_Team, $t_Info);
		}
	}
	else if(is_object($t_Teams) && $t_Teams->LoadFailed == false)
	{
		AddTeamToArray($t_Teams, $t_Info);
	}
	
	echo json_encode($t_Info);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
