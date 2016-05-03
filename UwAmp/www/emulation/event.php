<?php
if(!defined("INCLUDED")) 
	die();

class Event
{
	public $Name;
	public $Description;
	public $EffectFunction;
		
	function __construct($a_Name, $a_Description, $a_Effect)
	{
		$this->Name = $a_Name;
		$this->Description = $a_Description;
		$this->EffectFunction = $a_Effect;
	}
	
	function ApplyTo(Player $a_Player)
	{
		$this->EffectFunction($a_Player);
	}
	
	// Really PHP? I have to do this to make it work?
	public function __call($method, $args)
    {
        if(is_null($this->$method) == false && is_callable(array($this, $method))) 
		{
            return call_user_func_array($this->$method, $args);
		}
    }
	
	function GetDescription()
	{
		if(is_array($this->Description))
		{
			return $this->Description[mt_rand(0, count($this->Description)-1)];
		}
		else return $this->Description;
	}
}

$g_Events = array();
function CreateEvent($a_Name, $a_Description, $a_Effect, $a_Type = "Event")
{
	global $g_Events;
	$g_Events[$a_Name] = new $a_Type($a_Name, $a_Description, $a_Effect);
}
