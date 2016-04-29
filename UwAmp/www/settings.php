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
$settings["riot_key"]		= file_get_contents(KEY_FOLDER . "key_file");

// Region settings
$settings["regions"]		= array
							(
								"North America"=>"na", 
								"Europe West"=>"euw"
							);
			
// Testing settings
$settings["testing"] = true;
$settings["testing_account"] = 22929336;
$settings["testing_region"] = 'euw';

// Balance settings
$settings["starting_cash_per_champion_point"] = 0.1;
$settings["cash_per_champion_point"] = 0.3;


?>