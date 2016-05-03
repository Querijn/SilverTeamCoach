<?php
ob_start();
require_once("include.php");
require_once(EMULATION_FOLDER."include.php");
ob_get_contents();
ob_end_clean();

if(!IsLoggedIn())
	die(json_encode(array("error" => "NOT_LOGGED_IN")));

try
{
	if(isset($_GET['team']) === false || is_numeric($_GET['team']) == false)
		throw new Exception("Invalid team!");
	
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
	
	$t_MatchType = "BOT";
	
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
				"player" => $t_DBPlayer,
				"team" => $t_DBPlayerTeam,
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
				"player" => $t_DBOpponent,
				"team" => $t_DBOpposingTeam,
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
	echo json_encode($t_Game);
}
catch(Exception $e)
{
	die(json_encode(array("error"=>$e->getMessage())));
}
