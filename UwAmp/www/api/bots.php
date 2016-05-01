<?php
if(!defined("INCLUDED")) 
	die();

require_once(MYSQL_FOLDER . "mysql.php");
require_once(RIOT_FOLDER . "FileSystemCache.php");
require_once(RIOT_FOLDER . "php-riot-api.php");

function CreateBot($a_Name, $a_TeamName, $a_Top, $a_Mid, $a_Jungle, $a_Support, $a_Marksman, $a_TopSkin = 0, $a_MidSkin = 0, $a_JungleSkin = 0, $a_SupportSkin = 0, $a_MarksmanSkin = 0)
{
	global $settings;
	
	$t_Top = GetChampionIDByName($a_Top);
	$t_Mid = GetChampionIDByName($a_Mid);
	$t_Jungle = GetChampionIDByName($a_Jungle);
	$t_Support = GetChampionIDByName($a_Support);
	$t_Marksman = GetChampionIDByName($a_Marksman);
	
	if($t_Top == -1)
	{
		echo "$a_Name's $a_TeamName has an invalid top.\n";
		return false;
	}
	if($t_Mid == -1)
	{
		echo "$a_Name's $a_TeamName has an invalid mid.\n";
		return false;
	}
	if($t_Jungle == -1)
	{
		echo "$a_Name's $a_TeamName has an invalid jungler.\n";
		return false;
	}
	if($t_Support == -1)
	{
		echo "$a_Name's $a_TeamName has an invalid support.\n";
		return false;
	}
	if($t_Marksman == -1)
	{
		echo "$a_Name's $a_TeamName has an invalid marksman.\n";
		return false;
	}
	
	$t_API = new riotapi($settings["riot_key"], 'euw', new FileSystemCache(BASE_FOLDER . "cache"));
	
	$t_Bot = new DatabasePlayer();
	$t_Bot->AlternativeName = $a_Name;
	$t_Bot->User = -1;
	$t_Bot->Title = "Bot";
	$t_Bot->MainTeam = 0;
	$t_Bot->Cash = 0;
	$t_Bot->StartingCash = 0;
	$t_Bot->Admin = 0;
	$t_Bot->Save();
	
	$t_Team = new DatabaseTeam();
	$t_Team->Name = $a_TeamName;
	$t_Team->Player = -1;
	
	$t_Team->Mid = $t_Mid;
	$t_Team->Top = $t_Top;
	$t_Team->Jungle = $t_Jungle;
	$t_Team->Support = $t_Support;
	$t_Team->Marksman = $t_Marksman;
	
	$t_Skins = $t_API->getStatic('champion?dataById=true&champData=skins')["data"];
	$t_Found = false;
	
	foreach($t_Skins[$t_Top]["skins"] as $t_Skin) 
		if($t_Skin['num'] == $a_TopSkin)
		{
			$t_Found = true;
			break;
		}
	if($t_Found == false)
	{
		echo "$a_Name's $a_Top does not have skin $a_SkinTop.\n";
		return false;
	}
	//$t_Team->SkinTop = $a_TopSkin;
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $t_Top;
	$t_Champion->SkinId = $a_TopSkin;
	$t_Champion->PlayerId = $t_Bot->Id;
	$t_Champion->Save();
		
	$t_Found = false;
	foreach($t_Skins[$t_Mid]["skins"] as $t_Skin) 
		if($t_Skin['num'] == $a_MidSkin)
		{
			$t_Found = true;
			break;
		}
	if($t_Found == false)
	{
		echo "$a_Name's $a_Mid does not have skin $a_MidSkin.\n";
		return false;
	}
	//$t_Team->SkinMid = $a_MidSkin;
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $t_Mid;
	$t_Champion->SkinId = $a_MidSkin;
	$t_Champion->PlayerId = $t_Bot->Id;
	$t_Champion->Save();
		
	$t_Found = false;
	foreach($t_Skins[$t_Jungle]["skins"] as $t_Skin) 
		if($t_Skin['num'] == $a_JungleSkin)
		{
			$t_Found = true;
			break;
		}
	if($t_Found == false)
	{
		echo "$a_Name's $a_Jungle does not have skin $a_JungleSkin.\n";
		return false;
	}
	//$t_Team->SkinJungle = $a_JungleSkin;
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $t_Jungle;
	$t_Champion->SkinId = $a_JungleSkin;
	$t_Champion->PlayerId = $t_Bot->Id;
	$t_Champion->Save();
		
	$t_Found = false;
	foreach($t_Skins[$t_Support]["skins"] as $t_Skin) 
		if($t_Skin['num'] == $a_SupportSkin)
		{
			$t_Found = true;
			break;
		}
	if($t_Found == false)
	{
		echo "$a_Name's $a_Support does not have skin $a_SupportSkin.\n";
		return false;
	}
	//$t_Team->SkinSupport = $a_SupportSkin;
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $t_Support;
	$t_Champion->SkinId = $a_SupportSkin;
	$t_Champion->PlayerId = $t_Bot->Id;
	$t_Champion->Save();
		
	$t_Found = false;
	foreach($t_Skins[$t_Marksman]["skins"] as $t_Skin) 
		if($t_Skin['num'] == $a_MarksmanSkin)
		{
			$t_Found = true;
			break;
		}
	if($t_Found == false)
	{
		echo "$a_Name's $a_Marksman does not have skin $a_MarksmanSkin.\n";
		return false;
	}
	//$t_Team->SkinMarksman = $a_MarksmanSkin;
	$t_Champion = new DatabaseChampion();
	$t_Champion->ChampionId = $t_Marksman;
	$t_Champion->SkinId = $a_MarksmanSkin;
	$t_Champion->PlayerId = $t_Bot->Id;
	$t_Champion->Save();
	
	$t_Team->Enabled = 1;
	
	$t_Team->Wins = 0;
	$t_Team->Losses = 0;
	$t_Team->Kills = 0;
	$t_Team->Deaths = 0;
	$t_Team->CreepScore = 0;
	$t_Team->Save();

	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION["summoner"]["id"]));
	if($t_Player->MainTeam == 0)
	{
		$t_InsertedTeam = DatabaseTeam::Load(SQLSearch::In(DatabaseTeam::Table)->Where("id")->IsLastID());
		if(is_object($t_InsertedTeam) && $t_InsertedTeam->LoadFailed == false)
			$t_Player->MainTeam = $t_InsertedTeam->Id;

		$t_Player->Save();
	}
	
	echo "Added bot team \"$a_Name's $a_TeamName\" ().\n";
	return true;
}
?>