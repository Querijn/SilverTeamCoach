using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Debugger : MonoBehaviour
{
    enum Type
    {
        Error,
        Message
    }



    public static void Log(string a_Message)
    {
        AddToLog(a_Message, Type.Message);
    }

    public static void LogError(string a_Message)
    {
        AddToLog(a_Message, Type.Error);
    }

    static List<string> m_Messages = new List<string>();
    static void AddToLog(string a_Message, Type a_Type)
    {
        switch (a_Type)
        {
            case Type.Error:
                m_Messages.Add("<color=#FF0000FF>[" + DateTime.Now.ToString() + "] [Error]: " + a_Message + "</color>");
                Debug.LogError(a_Message);
                break;
            case Type.Message:
                m_Messages.Add("<color=#FFFFFFFF>[" + DateTime.Now.ToString() + "] [Message]: " + a_Message + "</color>");
                Debug.Log(a_Message);
                break;
        }

        if(m_Messages.Count > 10)
        {
            m_Messages.RemoveAt(0);
        }
    }

    static Text m_Text = null;
    void Start ()
	{
        m_Text = GetComponent<Text>();
        Log("Debugger started.");
	}
	
	void Update () 
	{
        m_Text.text = "";

        //for (int i = m_Messages.Count-1; i >= 0; i--)
        for (int i = 0; i < m_Messages.Count; i++)
        {
            m_Text.text += m_Messages[i] + "\n";
        }
	}
}
