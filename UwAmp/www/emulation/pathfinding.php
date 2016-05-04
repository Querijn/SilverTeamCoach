<?php
if(!defined("INCLUDED")) 
	die();

if(class_exists('Path') == false)
{
	function PathNodeCompare( $a, $b )
	{ 
		$a->Weight = ($a->Score * 0.1) + $a->DistToEnd;
		$b->Weight = ($b->Score * 0.1) + $b->DistToEnd;
		if(  $a->Weight ==  $b->Weight ) { return 0 ; } 
		return ($a->Weight < $b->Weight) ? -1 : 1;
	} 
		
	class PathNode
	{
		public $Element;
		
		public $Score;
		public $DistToEnd;
		public $Parent;
		public $Weight;
		
		function __construct($a_Element, $a_Score, $a_EndDist, $a_Parent)
		{
			$this->Element = $a_Element;
			$this->Score = $a_Score;
			$this->DistToEnd = $a_EndDist;
			$this->Parent = $a_Parent;
		}
	}
	
	class Path
	{
		static $Waypoints = null;
		public $Route = null;
				
		function __construct($a_From, $a_To)
		{
			if(self::$Waypoints == null)
				self::SetupWaypoints();
			
			$this->Route = $this->Find($a_From, $a_To);
		}
		
		static function GetClosestWaypoint(float $a_X, float $a_Y)
		{
			if(self::$Waypoints == null) 
				self::SetupWaypoints();
			
			$t_Closest = null;
			$t_Distance = 999999999;
			
			$t_ConvertedObject = array("p" => array("x"=>$a_X, "y"=>$a_Y));
			for($i = 143; $i < count(self::$Waypoints); $i++)
			{
				$t_Waypoint = self::$Waypoints[$i];
				$t_NewDistance = self::GetDistance($t_ConvertedObject, $t_Waypoint);
				if($t_NewDistance < $t_Distance)
				{
					$t_Closest = $i;
					$t_Distance = $t_NewDistance;
				}
			}
			
			return $t_Closest;
		}
		
		static function InitialPathNode($a_From, $a_To)
		{
			if($a_From < 0 || $a_From >= count(self::$Waypoints))
				return null;
			if($a_To < 0 || $a_To >= count(self::$Waypoints))
				return null;
			
			$t_Start = self::$Waypoints[$a_From];
			$t_End = self::$Waypoints[$a_To];
			return new PathNode($t_Start, 0, self::GetDistance($t_Start, $t_End), null);
			
		}
		
		static function GetDistance($a_From, $a_To)
		{
			$t_X = $a_To["p"]["x"] - $a_From["p"]["x"];
			$t_Y = $a_To["p"]["y"] - $a_From["p"]["y"];
			
			return sqrt($t_X * $t_X + $t_Y * $t_Y);
		}
		
		function Find($a_From, $a_To)
		{
			$k = 0;
			$t_Closed = array();
			$t_Start = Path::InitialPathNode($a_From, $a_To);
			$t_Open = array($t_Start);
			$t_Found = false;
			
			if($a_To < 0 || $a_To >= count(self::$Waypoints))
				return false;
			
			$t_End = self::$Waypoints[$a_To];
			$t_Tries = 0;
			$t_SortCount = 0;
			while(count($t_Open) != 0)
			{
				$t_Open = array_values($t_Open);
				
				if($t_SortCount != 0)
				{
					//TODO sort just the amount named in sortcount
					
					usort($t_Open, 'PathNodeCompare');
					$t_SortCount = 0;
				}
				//var_dump($t_Open);
				$t_Node = $t_Open[0];
				unset($t_Open[0]);
				$t_Closed[] = $t_Node;
				
				if(is_null($t_Node) || $t_Tries == 500)
					break;
				$t_Tries++;
				
				if($t_Node->DistToEnd == 0.0)
				{
					$t_Found = true;	
					break;
				}
				
				foreach($t_Node->Element["a"] as $t_WayIndex)
				{
					$t_SortCount++;
					$t_Waypoint = self::$Waypoints[$t_WayIndex];
					$t_Open[] = new PathNode($t_Waypoint, $t_Node->Score + 0.1, self::GetDistance($t_Waypoint, $t_End), count($t_Closed)-1);
				}
			}
			
			if($t_Found)
				return $this->ReconstructPath($t_Closed, self::$Waypoints[$a_From], $t_End);
			else return null;
			//die("Found: " . ($t_Found !== false ? print_r($t_Found) : "no"));
		}
		
		function ReconstructPath(array $a_List, $a_Start, $a_End)
		{
			$t_Return = array();
			$t_Element = $a_List[count($a_List)-1];
			
			while(true)
			{
				$t_Return[] = array
				(
					"x"=>$t_Element->Element["p"]["x"],
					"y"=>$t_Element->Element["p"]["y"],
				);
				
				if(is_null($t_Element->Parent))
					break;
				
				$t_Element = $a_List[$t_Element->Parent];
			}
			$t_Return = array_reverse($t_Return);
			return $t_Return;
		}
		
		static function SetupWaypoints()
		{
			$t_File = @file_get_contents(EMULATION_FOLDER . "nodes.json");
			if($t_File === false)
			{
				// Conversion required (waypoints.json is in format 0~512)
				$t_File = @file_get_contents(EMULATION_FOLDER . "waypoints.json");
				if($t_File === false)
					throw new Exception("both the nodes and the waypoints file is missing!");
				
				$t_Array = json_decode($t_File, true);
				
				foreach($t_Array as &$t_Element)
				{
					$t_Element['p']['x'] = ((float)$t_Element['p']['x']) / 512.0;
					$t_Element['p']['y'] = 1.0-(((float)$t_Element['p']['y']) / 512.0);
				}
				
				file_put_contents (EMULATION_FOLDER . "nodes.json", json_encode($t_Array));
				$t_File = @file_get_contents(EMULATION_FOLDER . "nodes.json");
			}
			self::$Waypoints = json_decode($t_File, true);
		}
	}
}


