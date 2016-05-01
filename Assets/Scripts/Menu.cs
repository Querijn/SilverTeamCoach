using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    static public MenuHandler.Menus Open { get { return MenuHandler.CurrentlyOpenMenu; } }
	void Start ()
	{
        MenuHandler.Add(this);
	}
	
	void Update () 
	{
	
	}
}
