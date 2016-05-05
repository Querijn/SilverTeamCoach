<?php

class Player
{
	public $Name;
	public $Team;
	public $Role;
	public $Position;
	public $TargetPosition;
	public $Spawn;
	private $Events = array();
	public $Path = array();
	public $Efficiency;
	
	public $AFKUntil = -1;
	public $TrollingUntil = -1;
	public $TiltingUntil = -1;
	public $DeadUntil = -1;
	
	public $Level = 1;
	
	public $Kills = 0;
	public $Deaths = 0;
	
	function Player($a_Name, $a_Team, $a_Role, vec2 $a_Position, double $a_Efficiency)
	{
		$this->Name = $a_Name;
		$this->Team = $a_Team;
		$this->Role = $a_Role;
		$this->Position = $a_Position;
		$this->Spawn = $a_Position;
		$this->Efficiency = $a_Efficiency;
	}
	
	public function AddWaypoint(vec2 $a_Waypoint)
	{
		global $g_Game;
		$this->Path[$g_Game->Time] = $a_Waypoint;
	}
	
	public function IsAFK()
	{
		global $g_Game;
		return ($g_Game->Time <= $this->AFKUntil);
	}
	
	public function IsTilting()
	{
		global $g_Game;
		return ($g_Game->Time <= $this->TiltingUntil);
	}
	
	public function IsTrolling()
	{
		global $g_Game;
		return ($g_Game->Time <= $this->TrollingUntil);
	}
	
	public function IsDead()
	{
		global $g_Game;
		return ($g_Game->Time <= $this->DeadUntil);
	}
	
	public function GetEfficiency()
	{
		global $g_Game;
		global $g_Settings;
		return $this->Efficiency * ($this->IsTilting($g_Game->Time) ? $g_Settings["tilt_efficiency_modifier"] : 1.0);
	}
	
	public function IsActive()
	{
		return $this->IsDead() == false && $this->IsAFK() == false;
	}
};