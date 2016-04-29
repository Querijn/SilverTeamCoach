<?php
if(!defined("INCLUDED")) 
	die();

require_once(MYSQL_FOLDER . "mysql.php");
require_once(RIOT_FOLDER . "php-riot-api.php");

function LogIn($a_User, $a_Region, $a_Verify = false)
{
	global $settings;

	if(isset($_SESSION["user"]))
		return;
	
	$t_Key = preg_replace('/\s+/', '', strtolower($a_User));
	$t_API = new riotapi($settings["riot_key"], 'euw', new FileSystemCache(BASE_FOLDER . "cache"));
	
	$t_Data = $t_API->getSummoner($t_Key);
	
	// Verify region
	if(in_array($a_Region, $settings["regions"]))
		$_SESSION["region"] = $a_Region;
	
	else
	{
		$g_Error = "That was an invalid region. How did you even get here?";
		throw new Exception();
	}
		
	$_SESSION["summoner"] = $t_Data[$t_Key];	
			
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($t_Data[$t_Key]["id"]));
		
	// Do we exist?
	if(is_object($t_Player) && $t_Player->LoadFailed) 
	{
		$t_Player = new DatabasePlayer();
	
		$t_Player->User = $t_Data[$t_Key]["id"];
		$t_Player->Title = 'Player';
		$t_Player->OwnedChampions = array(1);
		$t_Player->Cash = 0.0;
		$t_Player->MainTeam = 0;
		
		$t_Info = $t_API->getChampionMastery($t_Player->User);
		foreach($t_Info as $t_Mastery) 
			$t_Player->Cash += $t_Mastery["championPoints"];
		$t_Player->Cash *= $settings["starting_cash_per_champion_point"];
		
		// TODO create message about cash gain
		
		$t_Player->StartingCash = $t_Player->Cash;
		
		$t_Player->Admin = 0;
		
		$t_Player->Save();
	}
	
	if($a_Verify === true)
	{
		$_SESSION["user"] = $a_User;
		
		if(isset($_SESSION['verification']))
			unset($_SESSION['verification']);
	}
}
?>