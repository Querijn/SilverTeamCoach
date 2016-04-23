<?php 
require_once(API_FOLDER."singleton.php");
require_once(MYSQL_FOLDER."sqlsearch.php");
if(!class_exists("SQLEntry"))
{	
	DEFINE("ELEFORM_SQL_QUOTE", "`");
	abstract class SQLEntry extends GetterSetter 
	{
		protected $m_SQLShouldSave = false; // Autosave functionality
		protected $m_SQLID;				   // Primary
		protected $m_Exists = false;
		protected $m_LoadFailed = false;
	
		public function __construct()
		{
			register_shutdown_function(array($this, '__autosavefunc'));
		}
				
		public function AutoSave($a_TurnOn = true)
		{
			$this->SQLShouldSave = $a_TurnOn;
		}
		
		public function __clone() 
		{
			//$this->instance = ++self::$instances;
			$this->m_Exists = false;
		}
		
		public function Loaded()
		{
			return !$this->m_LoadFailed;
		}
		
		public static function CleanString($a_String)
		{
			global $g_EleConnection;
			if(empty($a_String))
				return false;
			return $g_EleConnection->real_escape_string($a_String);
		}
		
		public function Save($a_Reload = true)
		{
			if($this->m_LoadFailed) return false;
			if(!$this->m_Exists)
			{
				$t_Table = FindTable($this->Tablename)->CleanName;
				global $g_EleConnection;
				$t_Array = $this->SQLTableTranslate; // Translation array
				
				$t_Query = Array("INSERT INTO ".ELEFORM_SQL_QUOTE.$t_Table.ELEFORM_SQL_QUOTE." (", ") VALUES (", ");");
				$t_Variables = "";
				$t_Values = "";
				
				foreach(array_keys(get_class_vars(get_class($this))) as $t_Var) // For each column
				{
					if(isset($t_Array[$t_Var])) // If there's a translation
					{
						$t_Variables .= ELEFORM_SQL_QUOTE.$this->CleanString($t_Array[$t_Var]).ELEFORM_SQL_QUOTE.",";
						if(empty($this->{$t_Var}) || is_null($this->{$t_Var}) || is_string($this->{$t_Var}) || is_numeric($this->{$t_Var}) )
							$t_Values .= "'".$this->CleanString($this->{$t_Var})."',";
						else $t_Values .= "'".$this->CleanString(serialize($this->{$t_Var}))."',";
					}
				}
				$t_Query = $t_Query[0].substr($t_Variables, 0, -1).$t_Query[1].substr($t_Values, 0, -1).$t_Query[2];
				//die($t_Query);
				
				if($g_EleConnection->query($t_Query, true)===false) 
				{
					$this->OnError($t_Query);
					return false;
				}
				else if($a_Reload)
				{ 
					$this->__load(SQLSearch::In($this->m_Tablename)->Where($this->__getidcolumnname())->isLastID());
				}
				
				return true;
			}
			else
			{
				$t_Table = FindTable($this->Tablename)->CleanName;
				global $g_EleConnection;
				$t_Array = $this->SQLTableTranslate; // Translation array
				
				$t_Query = Array("UPDATE ".ELEFORM_SQL_QUOTE.$t_Table.ELEFORM_SQL_QUOTE." SET ", " WHERE ", ";");
				$t_Variables = "";
				$t_Values = "";
				
				foreach(array_keys(get_class_vars(get_class($this))) as $t_Var) // For each column
				{
					if(isset($t_Array[$t_Var]) && $t_Array[$t_Var]!=$this->__getidcolumnname()) // If there's a translation
					{
						$t_Variables .= ELEFORM_SQL_QUOTE.$this->CleanString($this->SQLTableTranslate[$t_Var]).ELEFORM_SQL_QUOTE;
						if(empty($this->{$t_Var}) || is_null($this->{$t_Var}) || is_string($this->{$t_Var}) || is_numeric($this->{$t_Var}))
							$t_Variables .= " = '".$this->CleanString($this->{$t_Var})."', ";
						else 
							$t_Variables .= " = '".$this->CleanString(serialize($this->{$t_Var}))."', ";
					}
				}
				$t_Variables = substr($t_Variables, 0, -1);
				$t_IDCheck = "";
				foreach(array_keys(get_class_vars(get_class($this))) as $t_Var) // For each column
					if(isset($this->SQLTableTranslate[$t_Var]) && $this->SQLTableTranslate[$t_Var]==$this->__getidcolumnname()) // If there's a translation
					{
						$t_IDCheck .= ELEFORM_SQL_QUOTE.$this->CleanString($this->SQLTableTranslate[$t_Var]).ELEFORM_SQL_QUOTE.
										" = '".$this->CleanString($this->{$t_Var})."'";
						break;
					}
				$t_Query = $t_Query[0].substr($t_Variables, 0, -1).$t_Query[1].$t_IDCheck.$t_Query[2];
	
				if($g_EleConnection->query($t_Query, true)===false) 
				{
					$this->OnError($t_Query);	
					return false;
				}
				return true;
			}
		}
		
		public static function __getloadcode()
		{
			return "public static function Load(\$a_Search)
			{
				\$instance = new self();
				\$t_Array = \$instance->__load(\$a_Search);
				if(is_array(\$t_Array))
					return \$t_Array;
				return \$instance;
			}";
		}
		
		public function Delete()
		{
			if(!$this->m_Exists)
				return false;
			global $g_EleConnection;
			$t_IDCheck = "";
			
			foreach(array_keys(get_class_vars(get_class($this))) as $t_Var) // For each column
			{
				if(isset($this->SQLTableTranslate[$t_Var]) && $this->SQLTableTranslate[$t_Var]==$this->__getidcolumnname()) // If there's a translation
				{
					$t_IDCheck .= ELEFORM_SQL_QUOTE.$this->CleanString($this->SQLTableTranslate[$t_Var]).ELEFORM_SQL_QUOTE.
									" = '".$this->CleanString($this->{$t_Var})."'";
					break;
				}
			}
			

			if($g_EleConnection->query("delete from ".FindTable($this->Tablename)->CleanName." WHERE ".$t_IDCheck)===false) 
				$this->OnError();
		}
		
		
		private function OnError($a_Arg) { global $g_EleConnection; printf("<br>(%s) Errormessage: <br>(arg = %s)<br>", get_class($this), $a_Arg); $g_EleConnection->print_error(); }
		
		
		public function __load($a_Search)
		{
			if(is_object($a_Search))
			{
				if($t_Result = $a_Search->Get())
				{
					$t_Return = Array();
					while($t_Row = $t_Result->fetch_array(MYSQLI_ASSOC)) // For every table
					{
						$t_Return[] = $t_Row;
					}
					if(count($t_Return)==1)
					{
						$t_Return = $t_Return[0];
						$this->__insert($t_Return);
						$this->m_Exists = true;
						return $this;
					}
					else if(count($t_Return)==0)
					{
						$this->m_LoadFailed = true;
						return false;
					}
					
					$t_Result = $t_Return;
					$a = Array();
					foreach($t_Result as $i)
					{
						$t_Instance = new $this();
						$t_Instance->__insert($i);
						$t_Instance->m_Exists = true;
						$a[] = $t_Instance;
					}
					
					$this->m_LoadFailed = false;
					$this->m_Exists = true;
					return $a;
				}
			}
			else
			{
				$t_Search = SQLSearch::In($this->m_Tablename)->Where($this->__getidcolumnname())->is($a_Search);

				if($t_Result = $t_Search->Get())
				{
					$t_Return = Array();
					while($t_Row = $t_Result->fetch_array(MYSQLI_ASSOC)) // For every table
					{
						$t_Return[] = $t_Row;
					}
					if(count($t_Return)==1)
					{
						$t_Return = $t_Return[0];
						$this->__insert($t_Return);
					}
					else if(count($t_Return)==0)
					{
						$this->m_LoadFailed = true;
						return false;
					}
					
					$this->m_LoadFailed = false;
					$this->m_Exists = true;
				}
			}
			return false;
		}
		
		public function __getidcolumnname()
		{
			foreach(array_keys(get_class_vars(get_class($this))) as $t_Var) // For each column
			{
				if(isset($this->SQLTableTranslate[$t_Var])) // If there's a translation
				{
					return $this->SQLTableTranslate[$t_Var];
				}
			}
		}
		
		public function __insert($a_Result)
		{
			if(!is_array($a_Result)){ print_r($this); }
			$t_ResultKeys = array_keys($a_Result);
			$t_Translate = array_flip($this->SQLTableTranslate);
			foreach($t_ResultKeys as $t_I)
			{
				if(IsSerialised($a_Result[$t_I]))
					$this->{$t_Translate[$t_I]} = unserialize($a_Result[$t_I]);
				else $this->{$t_Translate[$t_I]} = $a_Result[$t_I];
			}
			
			$this->m_LoadFailed = false;
		}
		
		public function __autosavefunc()
		{
			if($this->SQLShouldSave)
			{
				$this->Save(false);
			}
		}
	}
}
?>