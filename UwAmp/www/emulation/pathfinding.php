<?php
if(!defined("INCLUDED")) 
	die();

if(class_exists('Path'))
{
	class Path
	{
		static $Waypoints = null;
		
		function __construct()
		{
			if(self::$Waypoints == null)
			{
				self::$Waypoints = json_decode(file_get_contents(EMULATION_FOLDER . "waypoints.json"));
				var_dump(self::$Waypoints);die();
			}
		}
	}
}


