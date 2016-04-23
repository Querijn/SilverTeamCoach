<?php
require_once(API_FOLDER."singleton.php");
if(!class_exists("Table"))
{
	$g_EleTables = Array();
	
	class Table extends GetterSetter 
	{
		protected $m_Name;
		protected $m_Columns;
		protected $m_CleanName;
		protected $m_CleanColumns;
		
		function __construct($a_Name, $a_Clean)
		{
			$this->Name = $a_Name;
			$this->CleanName = $a_Clean;
		}
		
		function IsColumn($a_Column)
		{
			foreach($this->m_Columns as $c)
				if($a_Column==$c)
					return true;
			foreach($this->m_CleanColumns as $c)
				if($a_Column==$c)
					return true;
			return false;
		}
	}
	
	function FindTable($a_Table)
	{
		global $g_EleTables;
		foreach($g_EleTables as $t_Table)
			if($t_Table->CleanName==$a_Table||
			   $t_Table->name==$a_Table||
			   $t_Table->name==$a_Table."s")
			   return $t_Table;
		return false;
	}
}
?>