﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour 
{
    private static Dictionary<Menus, Menu> m_Menus = new Dictionary<Menus, Menu>();
    public static Menus CurrentlyOpenMenu = Menus.None;

    public AudioClip m_MenuSwitchSound = null;

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

        if (m_Menus.ContainsKey(t_Menu) != false)
            m_Menus.Remove(t_Menu);

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

    void Start()
    {
        CurrentlyOpenMenu = Menus.None;
    }

    bool m_Setup = false;
    void Update()
    { 
        if(m_Setup == false && m_Menus.ContainsKey(Menus.Champions))
        {
            OpenMenu(Settings.DefaultMenu);
            m_Setup = true;

        }
    }

    void SetAllEnabled(bool a_Enabled, Menus a_Exception = Menus.None)
    {
        foreach (Menus t_Menu in Enum.GetValues(typeof(Menus)))
        {
            if (t_Menu != Menus.None && a_Exception != Menus.None && t_Menu != a_Exception)
                SetEnabled(t_Menu, a_Enabled);
        }
    }

    void SetEnabled(Menus a_Menu, bool a_Enabled)
    {
        if(m_Menus.ContainsKey(a_Menu))
        {
            m_Menus[a_Menu].gameObject.SetActive(a_Enabled);
        }
    }

    void SetTitle(Menus a_Menu)
    {
        if (a_Menu != Menus.None && m_Menus.ContainsKey(a_Menu))
        {
            var t_Text = GameObject.FindGameObjectWithTag("MenuTitle").GetComponent<Text>();
            t_Text.text = m_Menus[a_Menu].name;
            if (a_Menu == Menus.Main)
                t_Text.text += " Menu";
        }
    }

    public void OpenMenu(string a_MenuName)
    {
        OpenMenu(GetMenuTypeByName(a_MenuName));
    }

    public void OpenMenu(Menus a_MenuName)
    {
        if (a_MenuName == Menus.None)
            return;

        if (CurrentlyOpenMenu != Menus.None && m_MenuSwitchSound != null)
            Sound.Play(m_MenuSwitchSound);

        Settings.OpenRequiredScenes();

        SetAllEnabled(false, a_MenuName);
        SetEnabled(a_MenuName, true);

        SetTitle(a_MenuName);
        CurrentlyOpenMenu = a_MenuName;
    }
}
