﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Message 
{
    public string Title;
    public string Content;
    public bool Read;
    public DateTime Time;

    public Message(string a_Title, string a_Message, bool a_Read, DateTime a_Time)
    {
        Title = a_Title;
        Content = a_Message;
        Read = a_Read;
        Time = a_Time;
    }

    public static void Create(string a_Title, string a_Message, bool a_Notify = true)
    {
        if (Info.Player == null)
            return;

        Dictionary<string, string> t_Commands = new Dictionary<string, string>();
        t_Commands.Add("id", Info.Player.ID.ToString());
        t_Commands.Add("title", a_Title);
        t_Commands.Add("message", a_Message);

        string t_CommandString = "create_message.php?";

        foreach (var t_Command in t_Commands)
        {
            t_CommandString += t_Command.Key + "=" + Uri.EscapeDataString(t_Command.Value) + "&";
        }
        t_CommandString = t_CommandString.Substring(0, t_CommandString.Length - 1);

        Debug.Log(t_CommandString);
        HTTP.Request(Settings.FormAjaxURL(t_CommandString), delegate (WWW a_Request)
        {
            if (a_Request.text == "true")
            {
                Info.Reset();
            }
            else Error.Show(a_Request.text);
        }, false);
    }

    public static void MarkAsRead()
    {
        if (Info.Player == null)
            return;

        Dictionary<string, string> t_Commands = new Dictionary<string, string>();
        t_Commands.Add("id", Info.Player.ID.ToString());

        string t_CommandString = "mark_read.php?";

        foreach (var t_Command in t_Commands)
        {
            t_CommandString += t_Command.Key + "=" + Uri.EscapeDataString(t_Command.Value) + "&";
        }
        t_CommandString = t_CommandString.Substring(0, t_CommandString.Length - 1);
        
        HTTP.Request(Settings.FormAjaxURL(t_CommandString), delegate (WWW a_Request)
        {
            if (a_Request.text == "true")
            {
                Info.Reset();
            }
            else Error.Show(a_Request.text);
        }, false);
    }
}
