<?php
ob_start();
require_once("include.php");
require_once(EMULATION_FOLDER."include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

function AsArray ($a_Object)
{
    if(!is_object($a_Object) && !is_array($a_Object))
    {
		return $a_Object;
	}
	
    $a_Object = array_map('AsArray', (array) $a_Object);
	$t_Return = array();
	foreach($a_Object as $t_Key=>$t_Value)
	{
		$t_Key = str_replace("m_", "", trim($t_Key));
		$t_Return[$t_Key] = $t_Value;
	}
	return $t_Return;
}

try
{
	unset($_SESSION["game"]);
	@unlink(AJAX_FOLDER. "data/game.json");
	if($settings["testing"]==true && file_exists(AJAX_FOLDER. "data/game.json"))
	{
		$_SESSION["game"] = file_get_contents(AJAX_FOLDER. "data/game.json");
		$_SESSION["game_info"] = file_get_contents(AJAX_FOLDER. "data/game_info.json");
	}
	
	$t_Game = null;
	if(isset($_SESSION["game"]) == false)
	{
		if(isset($_GET['team']) === false || is_numeric($_GET['team']) == false)
		throw new Exception("Invalid team!");
	
		if(isset($_GET['match']) === false)
			throw new Exception("Invalid match");
		if(!(
				$_GET['match'] == "bot" || 
				$_GET['match'] == "ranked" || 
				$_GET['match'] == "challenger" || 
				$_GET['match'] == "lcs"
			))
			throw new Exception("Invalid match (".$_GET['match'].")!");
		
		
		// Get my info
		$t_DBPlayer = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
		$t_DBPlayerTeam = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("player")->Is($t_DBPlayer->Id)->Also("id")->Is($_GET["team"]));
		if(is_object($t_DBPlayerTeam) == false || $t_DBPlayerTeam->LoadFailed)
			throw new Exception("Team does not exist or is not yours!");
		
		
		$t_DBPlayer = SetupGamePlayerArray($_SESSION["summoner"]["name"], $t_DBPlayer);
		$t_API = new riotapi($settings["riot_key"], $_SESSION["region"], new FileSystemCache(BASE_FOLDER . "cache"), 60);
		$t_DBMyChampions = GetTeamChampions($t_API, $t_DBPlayerTeam, $t_DBPlayer["db"]);
		
		// Get Elos
		//$t_EloPlayers = array($_SESSION["summoner"]["id"]);
		//$t_Elo = GetElos($t_API, $t_EloPlayers);
		
		$t_MatchType = $_GET['match'];
		
		// Get opponent information.
		$t_DBOpponent = GetOpponent($t_API, $t_DBPlayer["db"], $t_MatchType);
		$t_DBOpposingTeam = GetMainTeam($t_API, $t_DBOpponent["db"]);
		$t_DBOpposingChampions = GetTeamChampions($t_API, $t_DBOpposingTeam, $t_DBOpponent["db"]);
		
		// Game info
		$t_GameInfo = array
		(
			"teams" => array
			(
				/*Team 1 = */array
				(
					"is_player" => true,
					"player" => AsArray($t_DBPlayer),
					"team" => AsArray($t_DBPlayerTeam),
					"champions" => $t_DBMyChampions,
					
					"top_win" => ($t_DBMyChampions['top']['efficiency'] / (0.001 + $t_DBMyChampions['top']['efficiency'] + $t_DBOpposingChampions['top']['efficiency'])),
					"mid_win" => ($t_DBMyChampions['mid']['efficiency'] / (0.001 + $t_DBMyChampions['mid']['efficiency'] + $t_DBOpposingChampions['mid']['efficiency'])),
					"support_win" => ($t_DBMyChampions['support']['efficiency'] / (0.001 + $t_DBMyChampions['support']['efficiency'] + $t_DBOpposingChampions['support']['efficiency'])),
					"marksman_win" => ($t_DBMyChampions['marksman']['efficiency'] / (0.001 + $t_DBMyChampions['marksman']['efficiency'] + $t_DBOpposingChampions['marksman']['efficiency'])),
					"jungle_win" => ($t_DBMyChampions['jungle']['efficiency'] / (0.001 + $t_DBMyChampions['jungle']['efficiency'] + $t_DBOpposingChampions['jungle']['efficiency'])),
				),
				/*Team 2 = */array
				(
					"is_player" => false,
					"player" => AsArray($t_DBOpponent),
					"team" => AsArray($t_DBOpposingTeam),
					"champions" => $t_DBOpposingChampions,
					
					"top_win" => ($t_DBOpposingChampions['top']['efficiency'] / (0.001 + $t_DBMyChampions['top']['efficiency'] + $t_DBOpposingChampions['top']['efficiency'])),
					"mid_win" => ($t_DBOpposingChampions['mid']['efficiency'] / (0.001 + $t_DBMyChampions['mid']['efficiency'] + $t_DBOpposingChampions['mid']['efficiency'])),
					"support_win" => ($t_DBOpposingChampions['support']['efficiency'] / (0.001 + $t_DBMyChampions['support']['efficiency'] + $t_DBOpposingChampions['support']['efficiency'])),
					"marksman_win" => ($t_DBOpposingChampions['marksman']['efficiency'] / (0.001 + $t_DBMyChampions['marksman']['efficiency'] + $t_DBOpposingChampions['marksman']['efficiency'])),
					"jungle_win" => ($t_DBOpposingChampions['jungle']['efficiency'] / (0.001 + $t_DBMyChampions['jungle']['efficiency'] + $t_DBOpposingChampions['jungle']['efficiency'])),
				),
			)
		);
		
		$t_Game = new Game($t_GameInfo);
		
		$_SESSION["game"] = str_replace('*\\u0000', "", json_encode($t_Game));
		$_SESSION["game_info"] = str_replace('*\\u0000', "", json_encode($t_GameInfo));
		
		if($settings["testing"]==true)
		{
			file_put_contents(AJAX_FOLDER. "data/game.json", $_SESSION["game"]);
			file_put_contents(AJAX_FOLDER. "data/game_info.json", $_SESSION["game_info"]);
		}
		
		SaveMatch(json_decode($_SESSION["game"], true), json_decode($_SESSION["game_info"], true), $t_MatchType);
	}
	
	$t_GameInfo = $_SESSION["game_info"];
	if(isset($_GET["var_dump"]))
		print_r(json_decode($t_GameInfo));
	else echo $t_GameInfo;
}
catch(Exception $e)
{
	if(isset($_GET["var_dump"]))
	{
		echo "Exception occurred: ".$e->getMessage()." <br>";
		die(preg_replace('/\s+/', "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", preg_replace('/\n+/', '<br>', $e->getTraceAsString())));
	}	
	else die(json_encode(array("call_stack"=>$e->getTraceAsString(), "error"=>$e->getMessage())));
}
