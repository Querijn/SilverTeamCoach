<?php
if(!function_exists("make_verification"))
{
	function make_verification($a_UserID)
	{
		if (session_status() == PHP_SESSION_NONE)
			session_start();
		
		$t_Alphabet = array("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K");
		
		$t_User = "".$a_UserID;
		$t_Length = strlen($t_User);
		$t_Start = substr($t_User, $t_Length - 4, 4);
		$t_Start[0] = $t_Alphabet[$t_Start[0]];
		$t_Start[2] = $t_Alphabet[$t_Start[2]];
		
		if(!isset($_SESSION["verification"]))
		{
			$_SESSION["verification"] = 
				$t_Alphabet[rand(0, 9)] . 
				rand(0, 9) .
				$t_Alphabet[rand(0, 9)] . 
				rand(0, 9);
		}
		
		return $t_Start . $_SESSION["verification"];
	}
	
	function verify_verification($a_UserID, $a_Verification)
	{
		if (session_status() == PHP_SESSION_NONE)
			session_start();
		
		if(!isset($_SESSION["verification"]))
			return false;
		
		$t_Alphabet = array("A", "B", "C", "D", "E", "F", "G", "H", "I", "J");
		
		$t_User = (string)$a_UserID;
		$t_Length = strlen($t_User);
		$t_Start = substr($t_User, $t_Length - 4, 4);
		$t_Start[0] = $t_Alphabet[$t_Start[0]];
		$t_Start[2] = $t_Alphabet[$t_Start[2]];
		
		if($t_Start . $_SESSION["verification"] == $a_Verification)
		{
			unset($_SESSION["verification"]);
			return true;
		}
		return false;
	}
}
?>