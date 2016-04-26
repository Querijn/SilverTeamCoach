<?php
if(!defined("INCLUDED")) 
	die();

// MySQL settings
$settings["mysql_server"]	= "127.0.0.1";
$settings["mysql_database"]	= "stc";
$settings["mysql_username"] = "root";
$settings["mysql_password"]	= "root";
$settings["mysql_prefix"]	= "";

// MySQL settings
$settings["riot_key"]		= "missingkey";

// Region settings
$settings["regions"]		= array
							(
								"North America"=>"na", 
								"Europe West"=>"euw"
							);
			
// Testing settings
$settings["testing"] = true;
$settings["testing_account"] = 22929336;

?>