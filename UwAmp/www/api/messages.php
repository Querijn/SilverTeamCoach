<?php
if(!defined("INCLUDED")) 
	die();

require_once(MYSQL_FOLDER . "mysql.php");
require_once(RIOT_FOLDER . "FileSystemCache.php");
require_once(RIOT_FOLDER . "php-riot-api.php");

function CreateMessage($a_To, $a_Title, $a_Message, $a_Important = 1)
{
	global $settings;
	
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("id")->Is($a_To));
	if(is_object($t_Player) && $t_Player->LoadFailed)
		return false;
	
	$t_Message = new DatabaseMessage();
	$t_Message->PlayerId = $a_To;
	$t_Message->Title = $a_Title;
	$t_Message->Message = $a_Message;
	$t_Message->Unread = $a_Important;
	$t_Message->Time = time();
	$t_Message->Save();

	return true;
}

function MarkMessageRead(int $a_MessageID)
{
	global $settings;
	
	if(!IsLoggedIn())
		return false;
	
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION['summoner']['id']));
	if(is_object($t_Player) && $t_Player->LoadFailed)
		return false;
	
	$t_Messages = DatabaseMessage::Load(SQLSearch::In(DatabaseMessage::Table)->Where("id")->Is($a_MessageID)->Also("player_id")->Is($t_Player->Id));
	if(is_object($t_Messages))
	{
		if($t_Messages->LoadFailed)
			return false;
		else
		{
			$t_Messages->Unread = 0;
			$t_Messages->Save();
		}
	}
	else
	{
		for($i = 0; $i < count($t_Messages); $i++)
		{
			$t_Message = &$t_Messages[$i];
			$t_Message->Unread = 0;
			$t_Message->Save();
		}
	}
	return true;
}

function MarkAllMessagesRead()
{
	global $settings;
	
	if(!IsLoggedIn())
		return false;
	
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION['summoner']['id']));
	if(is_object($t_Player) && $t_Player->LoadFailed)
		return false;
	
	$t_Messages = DatabaseMessage::Load(SQLSearch::In(DatabaseMessage::Table)->Where("player_id")->Is($t_Player->Id));
	if(is_object($t_Messages))
	{
		if($t_Messages->LoadFailed)
			return false;
		else
		{
			$t_Messages->Unread = 0;
			$t_Messages->Save();
		}
	}
	else
	{
		for($i = 0; $i < count($t_Messages); $i++)
		{
			$t_Message = &$t_Messages[$i];
			$t_Message->Unread = 0;
			$t_Message->Save();
		}
	}
	return true;
}

function TranslateMessage(DatabaseMessage $a_Message)
{
	return array
	(
		"id" => $a_Message->Id,
		"title" => $a_Message->Title,
		"message" => $a_Message->Message,
		"time" => $a_Message->Time,
		"unread" => ($a_Message->Unread == 1)
	);
}

function GetMessages($a_Count = 30)
{
	global $settings;
	
	if(!IsLoggedIn())
		return array();
	
	$t_Player = DatabasePlayer::Load(SQLSearch::In(DatabasePlayer::Table)->Where("user")->Is($_SESSION['summoner']['id'])->Limit($a_Count));
	if(is_object($t_Player) && $t_Player->LoadFailed)
		return array();
	
	$t_Messages = DatabaseMessage::Load(SQLSearch::In(DatabaseMessage::Table)->Where("player_id")->Is($t_Player->Id));
	if(is_object($t_Messages))
	{
		if($t_Messages->LoadFailed)
			return array();
		else
		{
			return array(TranslateMessage($t_Messages));
		}
	}
	else
	{
		$t_Array = array();
		
		for($i = 0; $i < count($t_Messages); $i++)
		{
			$t_Array[] = TranslateMessage($t_Messages[$i]);
		}
		return $t_Array;
	}
}
?>