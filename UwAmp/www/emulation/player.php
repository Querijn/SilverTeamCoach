<?php

class Player
{
	public $Name;
	public $Position;
	public $Spawn;
	private $Events = array();
	public $Efficiency;
	
	public $AFKUntil = -1;
	public $TrollingUntil = -1;
	public $TiltingUntil = -1;
	
	function Player($a_Name, vec2 $a_Position, double $a_Efficiency)
	{
		$this->Name = $a_Name;
		$this->Position = $a_Position;
		$this->Spawn = $a_Position;
		$this->Efficiency = $a_Efficiency;
	}
	
	public function AddWaypoint(int $a_Time, vec2 $a_Waypoint)
	{
		$this->Path[$a_Time] = $a_Waypoint;
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
		global $g_Settings;
		return $this->Efficiency * (IsTilting() ? $g_Settings["tilt_efficiency_modifier"] : 1.0);
	}
};