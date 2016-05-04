<?php
if(!defined("INCLUDED")) 
	die();

$g_Game = null;
class Game
{
	public static $GameInfo;
	public $Timeline;
	public $Teams = array();
	
	public $Time = 0;
	
	function __construct($a_GameInfo)
	{
		// Setup game info
		global $g_Settings;
		global $g_Game;
		$g_Game = $this;
		
		self::$GameInfo = $a_GameInfo;
		
		for($i = 0; $i < 2; $i++)
		{
			$j = 0;
			foreach(self::$GameInfo["teams"][$i]["champions"] as $t_Key=>$t_Lane)
			{
				$t_Efficiency = $t_Lane["efficiency"];
				$this->Teams[$i][$j] = new Player($t_Lane["name"], $i, $t_Key, $g_Settings["spawn_point"][$i][$j], $t_Lane["efficiency"]);
				$j++;
			}
		}
		
		// START OF GAME
		$this->Time = 0; // They spawn_point
		$this->GameStartAction();
		
		$this->Time = 15; // The gates open
		// Determine action
		$this->PreLaningPhaseAction();
	}
	
	private function AddToTimeline(Player $a_Player, $a_Event, $a_Location = null, $a_Teleporting = false, $a_ToFuture = 0)
	{
		if(is_null($a_Location))
		{
			$a_Location = $a_Player->Position;
		}
		
		$t_Event = array
		(
			"name" =>$a_Event->Name,
			"description" => str_replace("{Champion}", $a_Player->Name, $a_Event->GetDescription()),
			"location" => $a_Location,
			"teleport" => $a_Teleporting ? 1 : 0,
		);
		
		$this->Time += $a_ToFuture;
		$this->Timeline[$this->Time][] = $t_Event;
		$this->Time -= $a_ToFuture;
		
		$a_Event->ApplyTo($a_Player);
	}
		
	function AddEvent(Event $a_Event, $a_Player = null, $a_Location = null, $a_Teleporting = false)
	{
		// Single player event
		$t_Event = null;
		if(is_null($a_Player) == false)
			$this->AddToTimeline($a_Player, $a_Event);
		
		// Everyone event
		else 
			foreach($this->Teams as $t_Team)
				foreach($t_Team as $t_Player)
					$this->AddToTimeline($t_Player, $a_Event);
	}
	
	function GameStartAction()
	{
		global $g_Events;
		
		// People connect, or try to.
		$this->AddEvent($g_Events["connect"]);
	}
	
	function PreLaningPhaseAction()
	{
		global $g_Events;
		$this->AddEvent($g_Events["init_game_pos"]);
	}
}

// Everyone should walk to initial positions,
// Bot or top at pull
// mid covering
// top or bot covering
// Start jungle route
// Find a gank
// Rest of the lanes, pushes or doesn't
// 