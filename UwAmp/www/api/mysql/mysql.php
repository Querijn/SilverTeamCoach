<?php 
if(!defined("INCLUDED")) 
	die();

if(!isset($settings["mysql_server"]))
	require_once(BASE_FOLDER . "settings.php");

if(!isset($settings["mysql_connection"]))
{
	$database = $settings['mysql_database'];
	$t_Environment = "Database";
	
	$t_OldErrorLevel = error_reporting(0);
	$settings["mysql_connection"] = new mysqli($settings['mysql_server'], $settings['mysql_username'], 
											   $settings['mysql_password']);

	if ($settings["mysql_connection"]->connect_errno) 
		return;
	
	error_reporting($t_OldErrorLevel);
	if(!$settings["mysql_connection"]->select_db($database))
		return;

	require_once(MYSQL_FOLDER."table.php");
	require_once(MYSQL_FOLDER."sqlobject.php");

	$t_Table = Array();
	$i = 0;
	if ($t_Result = $settings["mysql_connection"]->query("SHOW TABLES")) 
	{
		while($t_Row = $t_Result->fetch_array(MYSQLI_NUM)) // For every table
		{
			if(strlen($settings['mysql_prefix']) != 0 && strpos($t_Row[0], $settings['mysql_prefix']) === false)
				continue;
			
			$t_Row[0] = preg_replace('/[^0-9A-Za-z_]/', '', $t_Row[0]); // Clean the name
			$t_Clean = $t_Row[0];
			$t_Row[0] = str_replace($settings['mysql_prefix'], '', $t_Row[0]); // Remove the prefix
			$t_Exp = explode("_", $t_Row[0]); $t_Row[0] =""; // Changes name_type_stuff_things into
			foreach($t_Exp as $a) $t_Row[0] .= ucfirst($a);  // NameTypeStuffThings				
			
			$t_Table[] = new Table($t_Row[0], $t_Clean); // Create a new table class
			if(substr($t_Row[0], -4)=="ches" || substr($t_Row[0], -4)=="sses") 
				$t_Row[0] = substr($t_Row[0], 0, -2); // Remove the last ES
			else if(substr($t_Row[0], -1)=="s" && substr($t_Row[0], -2)!="ss") 
				$t_Row[0] = substr($t_Row[0], 0, -1); // Remove the last S
			$code[$i] = "Class ".ucfirst($t_Environment).ucfirst($t_Row[0])." extends SQLEntry\n{\n";
			// add the row class definition (DatabaseTable)
			
			$i++;
		}

		$i = 0;
		for($i = 0; $i<count($t_Table); $i++) // For each table column
		if ($t_Result = $settings["mysql_connection"]->query("DESCRIBE ".$t_Table[$i]->CleanName)) // If we have a result
		{
			$columns = array();
			$t_Translate = "\n";
			$code[$i] .= "\tprotected \$m_Tablename = \"".$t_Table[$i]->CleanName."\";\n";
			$code[$i] .= "const Table = \"".$t_Table[$i]->CleanName."\";\n";
			
			$x = 0;
			while($t_Row = $t_Result->fetch_array(MYSQLI_NUM)) // For each table column
			{		
				$t_Type = "";
				// if(strpos(strtolower($t_Row[1]), "int") !== false)
					// $t_Type = "int";
				// else if(strpos(strtolower($t_Row[1]), "text") !== false)
					// $t_Type = "string";
				// else if(strpos(strtolower($t_Row[1]), "double") !== false)
					// $t_Type = "float";
				// else if(strpos(strtolower($t_Row[1]), "enum") !== false)
					// $t_Type = "string";
				$t_OriginalName = $t_Row[0]; // Old name, db name
				$t_Row[0] = preg_replace('/[^0-9A-Za-z_]/', '', $t_Row[0]); // Clean the name
				
				$t_Exp = explode("_", $t_Row[0]); $t_Row[0] =""; // Changes name_type_stuff_things into
				foreach($t_Exp as $a) $t_Row[0] .= ucfirst($a);  // NameTypeStuffThings
							
				$t_Translate .= "\t\t\t\"m_".$t_Row[0]."\"=>\"".$t_OriginalName."\",\n";
				// Save the translate array
				
				$columns[] = $t_Row[0];
				$columns2[] = $t_OriginalName;
				$t_Is = "";
				if(is_null($t_Row[4]) == false)
				{
					if($x == 0)
						$t_Is = " = 'NULL'";
					else if(is_numeric($t_Row[4]))
						$t_Is = " = ".$t_Row[4];
					else 
						$t_Is = " = '".$t_Row[4]."'";
				}
				$code[$i] .= "\tprotected $t_Type \$m_".ucfirst($t_Row[0]).$t_Is.";\n"; // Add the variable
				$x ++;
			}
			
			$t_Table[$i]->columns = $columns;
			$t_Table[$i]->CleanColumns = $columns2;
			$code[$i] .= "\tprotected \$m_SQLTableTranslate = Array(".substr($t_Translate, 0, -2).");\n".SQLEntry::__getloadcode()."\n";
		}

		$codex = "";
		
		for($i = 0; $i<count($t_Table); $i++) // For each table
		{
			$classname = ucfirst($database).ucfirst($t_Table[$i]->CleanName); // Get the classname back
			if(substr($classname, -1)=="s") $classname = substr($classname, 0, -1);
			$functionname = "_".md5($classname.time()); // Get a unique function name
			
			// Create code that autosaves if necessary.
			$code[$i] .= "}\n";
			
			// Compile all code into one string
			$codex .= $code[$i];
		}

		//echo preg_replace('/\s+/', "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", preg_replace('/\n+/', '<br>', print_r($t_Table, true)));
		// die(preg_replace('/\s+/', "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", preg_replace('/\n+/', '<br>', $codex)));
		eval($codex); // Eval the code, making it run in

		$g_EleTables = $t_Table;
	}
}
?>