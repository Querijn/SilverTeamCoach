<?php
if(!function_exists("reinterpret_cast"))
{
	function reinterpret_cast($instance, $className) 
	{
		return unserialize(sprintf
		(
			'O:%d:"%s"%s',
			strlen($className),
			$className,
			strstr(strstr(serialize($instance), '"'), ':')
		));
	}
	
	function FileExists($fileName, $caseSensitive = false) {

    if(file_exists($fileName)) {
        return $fileName;
    }
    if($caseSensitive) return false;

    // Handle case insensitive requests            
    $directoryName = dirname($fileName);
    $fileArray = glob($directoryName . '/*', GLOB_NOSORT);
    $fileNameLowerCase = strtolower($fileName);
    foreach($fileArray as $file) {
        if(strtolower($file) == $fileNameLowerCase) {
            return $file;
        }
    }
    return false;
}
	
	function IsSerialised($a_Str)
	{
		$t_Data = @unserialize($a_Str);
		if ($a_Str === 'b:0;' || $t_Data !== false)
			return true;
		else return false;
	}
	
	function variablise($a_String, $a_KeepCapitals = false)
	{
		if($a_KeepCapitals)
			return preg_replace('/[^0-9A-Za-z_]/', '', $a_String);
		return strtolower(preg_replace('/[^0-9A-Za-z_]/', '', $a_String));
	}
	
	function GetDayOfWeek($a_Day)
	{
		return date('D', strtotime("Monday +{$a_Day} days"));
	}
	
	function StringContains($contain, $str)
	{
		return (strpos($str, $contain) !== false);
	}
	
	function CurrentTimestamp() { return date("Y-d-m H:i:s"); }
	function CurrentDateForFile() { return date("(Y-d-m   H i s)"); }
	
	function StartsWith($haystack, $needle)
	{
		return !strncmp($haystack, $needle, strlen($needle));
	}
	
	function RunSQL($location)
	{
		global $settings;
		//load file
		$commands = file_get_contents($location);

		//delete comments
		$lines = explode("\n",$commands);
		$commands = '';
		foreach($lines as $line){
			$line = trim($line);
			if( $line && !startsWith($line,'--') )
			{
				$commands .= $line . "\n";
			}
		}

		//convert to array
		$commands = explode(";", $commands);

		//run commands
		$total = $success = 0;
		foreach($commands as $command)
		{
			if(trim($command))
			{
				$success += ($settings["mysql_connection"]->query(str_replace("PREFIXGOESHERE", $settings["mysql_prefix"], $command))==false ? 0 : 1);
				$total += 1;
			}
		}

		//return number of successful queries and total number of queries found
		return array(
			"success" => $success,
			"total" => $total
		);
	}
	
	function FormDate($a_Time)
	{
		return date("j F, Y, g:i a", $a_Time);
	}
	
	function CurPageURL() 
	{
		$pageURL = 'http';
		
		if ($_SERVER["HTTPS"] == "on") {$pageURL .= "s";}
			$pageURL .= "://";
		if ($_SERVER["SERVER_PORT"] != "80") 
		{
			$pageURL .= $_SERVER["SERVER_NAME"].":".$_SERVER["SERVER_PORT"].$_SERVER["REQUEST_URI"];
		} 
		else 
		{
			$pageURL .= $_SERVER["SERVER_NAME"].$_SERVER["REQUEST_URI"];
		}
		return $pageURL;
	}
	
	function GetFileName($a_File)
	{
		return basename($a_File, ".php");
	}

}
?>