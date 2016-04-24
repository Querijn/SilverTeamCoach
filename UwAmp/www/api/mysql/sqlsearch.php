<?php
require_once(API_FOLDER."singleton.php");
require_once(API_FOLDER."hacks.php");
if(!class_exists("SQLSearch"))
{	
	class SQLSearch extends GetterSetter 
	{
		protected $m_Table;
		protected $m_Limit = -1;
		public $m_Query = "";
		protected $m_LastColumn;
		protected $m_TablePrefix;
		private $m_Opened = false;
		
		public static function GetConnection()
		{
			global $settings;
			return $settings["mysql_connection"];
		}
				
		public static function In($a_Table)
		{
			global $settings;
			$t_DB = $settings["mysql_connection"];
			$instance = new self();
			
			$instance->TablePrefix = "";
			$instance->Table = strtolower((!StartsWith($a_Table, $instance->TablePrefix))?($instance->TablePrefix.$a_Table):($a_Table));
			$instance->m_Query = "SELECT * FROM ".ELEFORM_SQL_QUOTE.$instance->Table.ELEFORM_SQL_QUOTE;
			return $instance;
		}
		
		public static function Query($a_Query)
		{			
			global $settings;
			$t_DB = $settings["mysql_connection"];
			$instance = new self();
			
			$instance->TablePrefix = "";
			$instance->m_Query = $a_Query;
			return $instance;
		}
		
		public function isLastID()
		{
			global $settings;
				
			$t_Query = "SELECT LAST_INSERT_ID();";
			
			if($t_Result = $settings["mysql_connection"]->query($t_Query))
			{
				$t_Result = $t_Result->fetch_array(MYSQLI_ASSOC);
				return $this->is($t_Result['LAST_INSERT_ID()']);
			}
			else printf("Errormessage: %s\n", $settings["mysql_connection"]->error);
			return false;
		}
		
		public function Where($a_Column)
		{
			global $settings;
			$this->m_Query .= " WHERE (".ELEFORM_SQL_QUOTE.$settings["mysql_connection"]->real_escape_string($a_Column).ELEFORM_SQL_QUOTE."";
			$this->m_LastColumn = $a_Column;
			$this->m_Opened = true;
			return $this;
		}
		
		public function also($a_Column) 
		{ 
			global $settings;
			$t_Table = FindTable($this->m_Table);
			if($t_Table)
			{
				if($t_Table->IsColumn($a_Column))
				{
					if($this->m_Opened) $this->m_Query .= ")";
					$this->m_Query .= " AND (".ELEFORM_SQL_QUOTE.$settings["mysql_connection"]->real_escape_string($a_Column).ELEFORM_SQL_QUOTE."";
					$this->m_Opened = true;
					$this->m_LastColumn = $a_Column;
				}
				else
				{
					$this->m_Query .= " OR ".ELEFORM_SQL_QUOTE.$settings["mysql_connection"]->real_escape_string($this->m_LastColumn).ELEFORM_SQL_QUOTE." = '".$settings["mysql_connection"]->real_escape_string($a_Column)."'";
				}
			}
			return $this; 
		}
		
		public function also_where($a_Column) { return $this->where($a_Column); }
		
		public function is($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " = '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		public function isNot($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " != '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		
		public function isLessThan($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " < '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		
		public function isLessThanOrEqualTo($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " <= '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		
		public function isGreaterThan($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " > '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		
		public function isGreaterThanOrEqualTo($a_Value) 
		{ 
			global $settings;
			$this->m_Query .= " >= '".$settings["mysql_connection"]->real_escape_string($a_Value)."'";
			return $this;
		}
		
		public function otherwise($a_Value)
		{
			return $this;
		}
		
		public function SetLimit($a_Value) { return $this->Limit($a_Value); }
		
		public function Limit($a_Value)
		{
			$this->m_Limit = $a_Value;
			
			return $this;
		}
		
		public function Get($a_Limit = -1)
		{
			global $settings;
			if(is_int($a_Limit) && $a_Limit>0)
				$this->m_Limit = $a_Limit;
				
			if(is_int($a_Limit) && $this->m_Limit>0)
			{
				if($this->m_Opened) $this->m_Query .= ")";
				
				$this->m_Query .= " LIMIT ".$settings["mysql_connection"]->real_escape_string($this->m_Limit).";";
			}
			else if($this->m_Opened) $this->m_Query .= ");";
				
			//echo $this->m_Query."<br>";
			if($t_Result = $settings["mysql_connection"]->query($this->m_Query))
				return $t_Result;
			else printf("Errormessage: %s\n", $settings["mysql_connection"]->error);
			return false;
		}
	}
}
?>