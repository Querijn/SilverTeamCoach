﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LoadingWindow : MonoBehaviour 
{
    static private LoadingWindow m_Window = null;

    void Start()
    {
        Close();
    }

    public static void Show()
    {
        if (m_Window == null)
            m_Window = GameObject.FindGameObjectWithTag("LoadingWindow").GetComponent<LoadingWindow>();

        m_Window.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Close();
    }
        
    public static void Close()
    {
        if (m_Window == null)
            m_Window = GameObject.FindGameObjectWithTag("LoadingWindow").GetComponent<LoadingWindow>();

        m_Window.gameObject.SetActive(false);
    }
}
