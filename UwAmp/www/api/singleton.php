<?php
if(!class_exists('GetterSetter'))
{
	abstract class GetterSetter 
	{
		public function __get($key) 
		{
			$key = "m_".ucfirst($key);
			if (!property_exists($this, $key))
				throw new Exception('Undefined property "' . $key . '"');
			
			$name = 'Set' . substr($key, 2);
			if (method_exists($this, $name))  return $this->{$name}();
			else return $this->{$key};
		}
		public function __set($key, $value) 
		{
			$key = "m_".ucfirst($key);
			if (!property_exists($this, $key))
				throw new Exception('Undefined property "' . $key . '"');
				
			$name = 'Set' . substr($key, 2);
			if (method_exists($this, $name)) return $this->{$name}($value);
			else return $this->{$key} = $value;
		}
	}
}
?>