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
			foreach(self::$GameInfo["teams"][$i]["champions"] as $t_Lane)
			{
				$t_Efficiency = $t_Lane["efficiency"];
				$this->Teams[$i][$j] = new Player($t_Lane["name"], $g_Settings["spawn_point"][$i][$j], $t_Lane["efficiency"]);
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
	
	private function AddToTimeline(Player $a_Player, string $a_Description)
	{
		$t_Event = array
		(
			"description" => str_replace("{Champion}", $a_Player->Name, $a_Description)
		);
		
		$this->Timeline[$this->Time][] = $t_Event;
	}
	
	function AddEvent(Event $a_Event, $a_Player = null)
	{
		// Single player event
		$t_Event = null;
		if(is_null($a_Player) == false)
		{
			$this->AddToTimeline($a_Player, $a_Event->GetDescription());
			$a_Event->ApplyTo($a_Player);
		}
		
		// Everyone event
		else foreach($this->Teams as $t_Team)
				foreach($t_Team as $t_Player)
				{
					$this->AddToTimeline($t_Player, $a_Event->GetDescription());
					$a_Event->ApplyTo($t_Player);
				}
	}
	
	function GameStartAction()
	{
		global $g_Events;
		
		// People connect, or try to.
		$this->AddEvent($g_Events["connect"]);
	}
	
	function PreLaningPhaseAction()
	{
		
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