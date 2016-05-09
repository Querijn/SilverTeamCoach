<?php
if(!defined("INCLUDED")) 
	die();

// Site settings
$settings["url"] = "http://localhost/"; 
$settings["build_path"] = "Release"; //"Development"; //  If "Development Build" was checked, please change this to "Development"

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
$settings["testing"] = false;
$settings["testing_account"] = 0;
$settings["testing_region"] = 'euw';

// Balance settings
$settings["free_champion"] = 1; // 1 = annie
$settings["starting_cash_per_champion_point"] = 0.1;
$settings["cash_per_champion_point"] = 0.3;

$settings["meta_coefficient"] = 0.1;
$settings["max_week_modifier"] = 3;
$settings["efficiency_loss_per_week"] = 0.1;

$settings["game_speed"] = 5;
?>