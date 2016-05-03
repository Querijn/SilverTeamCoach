<?php

class vec2
{
	public $x;
	public $y;
	
	function __construct($a_X, $a_Y)
	{
		$this->x = $a_X;
		$this->y = $a_Y;
	}
	
	function Subtract($a_Vector)
	{
		$this->x -= $a_Vector->x;
		$this->y -= $a_Vector->y;
	}
		
	function Add($a_Vector)
	{
		$this->x += $a_Vector->x;
		$this->y += $a_Vector->y;
	}
	
	function Divide($a_Number)
	{
		if(is_a('vec2', $a_Number))
		{
			$this->x /= $a_Vector->x;
			$this->y /= $a_Vector->y;
		}
		else
		{
			$this->x /= $a_Number;
			$this->y /= $a_Number;
		}
	}
	
	function Multiply($a_Vector)
	{
		$this->x *= $a_Vector->x;
		$this->y *= $a_Vector->y;
	}
	
	function Length2()
	{
		return ($this->x * $this->x) + ($this->y * $this->y);
	}
	
	function Length()
	{
		return sqrt(Length2());
	}
};