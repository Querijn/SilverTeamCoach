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
	
	public function TiltFor(int $a_UntilTime)
	{
		$this->TiltingUntil = $a_UntilTime;
	}
	
	public function IsAFK(int $a_CurrentTime)
	{
		return ($a_CurrentTime <= $this->AFKUntil);
	}
	
	public function IsTilting(int $a_CurrentTime)
	{
		return ($a_CurrentTime <= $this->TiltingUntil);
	}
	
	public function IsTrolling(int $a_CurrentTime)
	{
		return ($a_CurrentTime <= $this->TrollingUntil);
	}
	
	public function GetEfficiency()
	{
		global $g_Game;
		global $g_Settings;
		return $this->Efficiency * ($this->IsTilting($g_Game->Time) ? $g_Settings["tilt_efficiency_modifier"] : 1.0);
	}
};