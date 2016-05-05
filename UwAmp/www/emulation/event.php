<?php
if(!defined("INCLUDED")) 
	die();

class Event
{
	public $Name;
	public $EffectFunction;
		
	function __construct($a_Name, $a_Effect)
	{
		$this->Name = $a_Name;
		$this->EffectFunction = $a_Effect;
	}
	
	function ApplyTo($a_Player)
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
	
}

$g_Events = array();
function CreateEvent($a_Name, $a_Effect = null, $a_Type = "Event")
{
	global $g_Events;
	$g_Events[$a_Name] = new Event($a_Name, $a_Effect);
}
