<?php
if(!defined("INCLUDED")) 
	die();

function TimeConv($a_Min, $a_Sec)
{
	return $a_Min * 60 + $a_Sec;
}

$g_Game = null;

$t_TowerInfo =array
(
	'top'=>array("count"=>3, "health"=>3000, "base"=>3000),
	'mid'=>array("count"=>3, "health"=>3000, "base"=>3000),
	'bot'=>array("count"=>3, "health"=>3000, "base"=>3000),
	'base'=>array("count"=>2, "health"=>5000, "base"=>5000),
);
class Game
{
	public static $GameInfo;
	public $Timeline;
	public $Teams = array();
	
	public $Time = 0;
	public $GameOver = false;
	public $Winner = -1;
	
	public $Towers;
	
	function __construct($a_GameInfo)
	{
		// Setup game info
		global $g_Settings;
		global $t_TowerInfo;
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
		
		$this->Towers = array($t_TowerInfo,$t_TowerInfo);
		
		// START OF GAME
		$this->Time = 0; // They spawn_point
		$this->GameStartAction();
		
		$this->Time = 15; // The gates open
		// Determine action
		$this->PreLaningPhaseAction();
		
		$this->LaningPhaseAction();
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
		global $g_Settings;
		$t_BothInvading = true;
		$t_Invading = false;
		for($i = 0; $i < 2; $i++)
		{
			$t_Invade = (double)(mt_rand(0,1000)) * 0.001;
			
			if($t_Invade < $g_Settings["invade_chance"])
			{
				$this->AddEvent($g_Events["invade"], $i);
				$t_Invading = true;
			}
			else $t_BothInvading = false;
		}
		
		if($t_BothInvading)
			$this->AddEvent($g_Events["empty_jungle"]);
		
		else if($t_Invading)
		{
			$this->Time = 30;
			//Time to determine kills
			$t_Team[] = $this->GetTeamEfficiency(0) * $g_Settings["invade_kill_efficiency_modifier"];
			$t_Team[] = $this->GetTeamEfficiency(1) * $g_Settings["invade_kill_efficiency_modifier"];
			$t_Total = $t_Team[0]+$t_Team[1];
			
			$t_TeamKills[0] = (int)($t_Team[0]/$t_Team[1]);
			$t_TeamKills[1] = (int)($t_Team[1]/$t_Team[0]);
			
			for($j = 0; $j < 2; $j++)
				for($i = 0; $i < $t_TeamKills[$j]; $i++)
				{
					$t_Roll = (double)(mt_rand(0,1000)) * 0.001;
					$t_Roll *= $t_Total;
					
					$this->Time += pow(mt_rand(-2,2), 2);
					if($t_Roll < $t_Team[$j])
					{
						$t_Killer = $this->GetRandomActivePlayer($j);
						$t_Deathee = $this->GetRandomActivePlayer(($j + 1)%2);
						$this->Kill($t_Killer, $t_Deathee);
					}
				}
		}			
	}
	
	function LaningPhaseAction()
	{
		global $g_Events;
		global $g_Settings;
		
		$this->Time = TimeConv(1, 40);
		// Jungle camps spawn
		$this->AddEvent($g_Events["play"]);
		$t_TowersKilled = array(0,0);
		
		$t_LaningPhaseLength = mt_rand(14, 22);
		
		while($this->Time < TimeConv($t_LaningPhaseLength,0))
		{			
			if($t_TowersKilled[0] > 4 || $t_TowersKilled[1] > 4)
				break;
	
			$t_HighestTime = 0;
			for($t_Team1 = 0; $t_Team1 < 2; $t_Team1++)
			{
				foreach(self::$GameInfo["teams"][$t_Team1]["champions"] as $t_Key=>$t_Lane)
				{
					$t_Team2 = ($t_Team1 + 1) % 2;
					$t_Opponent = self::$GameInfo["teams"][$t_Team2]["champions"][$t_Key];
					$t_Time = $this->Time;
					
					$t_OppKey = $t_Key;
					if($t_Key == 'marksman' || $t_Key == 'support')
					{
						$t_OppKey = mt_rand(0, 1) == 0 ? 'marksman' : 'support';
					}
					$t_OpponentPlayer = $this->GetPlayer($t_Team2 , $t_OppKey);
					if($t_OpponentPlayer->IsActive())
					{
						$t_MaxTime = max(20.0, 140 - $t_Lane["efficiency"]*0.01);
						$this->Time += mt_rand(20.0, $t_MaxTime);
						// Calculate fight
						
						$t_Draw = 5000;
						$t_Win[$t_Team1] = $t_Lane["efficiency"];
						$t_Win[$t_Team2] = $t_Opponent["efficiency"];
						
						$t_Roll = mt_rand(0, $t_Draw + $t_Win[$t_Team1] + $t_Win[$t_Team2]);
						if($t_Roll < $t_Win[$t_Team1])
						{
							$t_Killer = $this->GetPlayer($t_Team1, $t_Key);
							$this->Kill($t_Killer, $t_OpponentPlayer);
						}
						if($this->Time > $t_HighestTime)
							$t_HighestTime = $this->Time;
						$this->Time = $t_Time;
					}
					else if($this->Time > TimeConv(5, 0))
					{
						// Decrease tower amount 
						$t_LaneName = null;
						switch($t_Key)
						{
							case 'marksman':
							case 'support':
								$t_LaneName = 'bot';
							case 'top':
							case 'mid':
								if(is_null($t_LaneName))
									$t_LaneName = $t_Key;
								
								$t_AddTime = mt_rand(10, 45);
								$this->Time += $t_AddTime;
								$t_DPS = (4000/45);
								if($this->Towers[$t_Team2][$t_LaneName] == 0)
								{
									$t_Lane = 'base';
									$t_DPS = (3500/45);
								}
								
								$t_DPS *= ($t_OpponentPlayer->IsActive() ? 1.0 : 3.0);
								
								$this->Towers[$t_Team2][$t_LaneName]["health"] -= $t_DPS*$t_AddTime;
								if($this->Towers[$t_Team2][$t_LaneName]["health"] <= 0)
								{
									$t_TowersKilled[$t_Team1]++;
									$this->Towers[$t_Team2][$t_LaneName]["health"] = $this->Towers[$t_Team2][$t_LaneName]["base"];
									$this->Towers[$t_Team2][$t_LaneName]["count"]--;
									$this->AddEvent($g_Events[$t_LaneName. "_tower"], $this->GetPlayer($t_Team1, $t_Key));
								}
								else $this->AddEvent($g_Events[$t_LaneName. "_tower_attack"], $this->GetPlayer($t_Team1, $t_Key));
						}
					if($this->Time > $t_HighestTime)
						$t_HighestTime = $this->Time;
					$this->Time = $t_Time;
					}
					
				}
			}
			$this->Time = $t_HighestTime;
		}
	}
	
	function GetPlayer($a_Team, $a_Role)
	{
		foreach($this->Teams[$a_Team] as $t_Player)
			if(strtolower($t_Player->Role) == strtolower($a_Role))
				return $t_Player;
			
		return null;
	}
	
	function Kill($a_Killer, $a_Feeder)
	{
		global $g_Events;
		if(is_null($a_Killer) || is_null($a_Feeder))
			return false;
		if($a_Killer->IsActive() == false || $a_Feeder->IsActive() == false)
			return false;
		
		$this->AddEvent($g_Events["kill"], $a_Killer);
		$this->AddEvent($g_Events["death"], $a_Feeder);
		return true;
	}
	
	private function AddToTimeline($a_Player, $a_Event, $a_ToFuture = 0)
	{
		$t_Event = array
		(
			"name" =>$a_Event->Name,
		);
		
		if(is_a($a_Player, "Player"))
		{
			$t_Event["team"] = $a_Player->Team;
			$t_Event["role"] = $a_Player->Role;
		}
		else if(is_array($a_Player))
		{
			$t_Event["team"] = $a_Player[0]->Team;
		}	
		else if(is_int($a_Player))
		{
			
			$t_Event["team"] = $a_Player;
		}	
		
		$this->Time += $a_ToFuture;
		$this->Timeline[$this->Time][] = $t_Event;
		$this->Time -= $a_ToFuture;
		
		if(is_a($a_Player, "Player"))
			$a_Event->ApplyTo($a_Player);
	}
		
	function AddEvent(Event $a_Event, $a_Player = null)
	{
		// Single player event
		$t_Event = null;
		if(is_null($a_Player) == false)
			$this->AddToTimeline($a_Player, $a_Event);
		
		// Everyone event
		else 
			foreach($this->Teams as $t_Team)
				$this->AddToTimeline($t_Team, $a_Event);
	}
	function GetTeamEfficiency($i)
	{
		$t_Efficiency = 0;
		foreach(self::$GameInfo["teams"][$i]["champions"] as $t_Lane)
			$t_Efficiency += $t_Lane["efficiency"];
			
		return $t_Efficiency;
	}
	
	function GetRandomPlayer($a_Team)
	{
		$a_TeamMember = mt_rand(0, 4);
		return $this->Teams[$a_Team][$a_TeamMember];
	}
	
	function GetRandomActivePlayer($a_Team)
	{
		if($this->HasActivePlayer($a_Team) == false)
			return null;
		
		$a_TeamMember = mt_rand(0, 4);
		for($i = 0; $i < 5; $i++)
		{
			$k = ($a_TeamMember +  $i)%5;
			return $this->Teams[$a_Team][$k];
		}
		
		// Shouldn't occur
		return null;
	}
	
	function HasActivePlayer($a_Team)
	{
		foreach($this->Teams[$a_Team] as $t_Player)
			if($t_Player->IsActive())
				return true;
		return false;
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