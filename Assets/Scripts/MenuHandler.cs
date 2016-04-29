using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour 
{
    private static Dictionary<Menus, Menu> m_Menus = new Dictionary<Menus, Menu>();

    public enum Menus
    {
        None,
        Main,
        Matches,
        Teams,
        Champions,
        Shop,
        Settings
    };

    public static void Add(Menu a_Menu)
    {
        Menus t_Menu = GetMenuTypeByName(a_Menu.name);
        if(t_Menu != Menus.None)
        m_Menus.Add(t_Menu, a_Menu);
        //Debug.Log("Filed new menu '" + a_Menu.name + "' under '" + t_Menu.ToString() + "'");
    }

    public static Menus GetMenuTypeByName(string a_Name)
    {
        try
        {
            return (Menus)Enum.Parse(typeof(Menus), a_Name);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to convert string to menu '" + a_Name + "': " + e.Message);
        }
        return Menus.None;
    }

    public static Menu GetMenuByName(string a_Name)
    {
        Menus t_Type = GetMenuTypeByName(a_Name);
        if (m_Menus.ContainsKey(t_Type))
            return m_Menus[t_Type];

        return null;
    }

    void Start ()
	{
        Info.Setup();

        OpenMenu("Champions");
	}

    void SetAllSizes(Vector3 a_Size, Menus a_Exception = Menus.None)
    {
        foreach (Menus t_Menu in Enum.GetValues(typeof(Menus)))
        {
            if (t_Menu != Menus.None && m_Menus.ContainsKey(t_Menu))
            {
                m_Menus[t_Menu].transform.localScale = a_Size;
            }
        }
    }

    void SetSize(Menus a_Menu, Vector3 a_Size)
    {
        if (a_Menu != Menus.None && m_Menus.ContainsKey(a_Menu))
        {
            m_Menus[a_Menu].transform.localScale = a_Size;
        }
    }

    void SetTitle(Menus a_Menu)
    {
        if (a_Menu != Menus.None && m_Menus.ContainsKey(a_Menu))
        {
            GameObject.FindGameObjectWithTag("MenuTitle").GetComponent<Text>().text = m_Menus[a_Menu].name;
        }
    }

    void Update () 
	{
	
	}

    public void OpenMenu(string a_MenuName)
    {
        Menus t_Menu = GetMenuTypeByName(a_MenuName);
        if (t_Menu == Menus.None)
            return;

        SetAllSizes(Vector3.zero, t_Menu);
        SetSize(t_Menu, Vector3.one);

        SetTitle(t_Menu);

        // Debug.Log("I was asked to open the " + a_MenuName + " menu.");
    }
}
