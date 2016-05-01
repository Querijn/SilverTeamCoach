using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public static class Messages
{
    static Dictionary<int, Message> m_Dictionary = new Dictionary<int, Message>();
    public static Message[] All
    {
        get
        {
            Message[] t_Messages = new Message[m_Dictionary.Count];
            m_Dictionary.Values.CopyTo(t_Messages, 0);
            return t_Messages;
        }
    }

    public static Message[] Unread
    {
        get
        {
            return Array.FindAll(Messages.All, m => m.Read == false);
        }
    }

    public static void MarkAllAsRead()
    {

        if (Info.Player == null)
            return;

        HTTP.Request(Settings.FormAjaxURL("mark_messages_read.php"), delegate (WWW a_Request)
        {
            if (a_Request.text == "true")
            {
                Info.Reset();
            }
            else Error.Show(a_Request.text);
        }, false);
    }

    public static void Reset()
    {
        m_Setup = false;
    }

    static bool m_Setup = false;
    public static void Insert(JSONArray a_Array)
    {
        if (m_Setup)
            return;

        foreach(JSONNode t_Node in a_Array)
        {
            DateTime t_Time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(t_Node["time"].AsInt);
            m_Dictionary.Add(t_Node["id"].AsInt, new Message(t_Node["title"].Value, t_Node["message"].Value, t_Node["unread"].AsBool == false, t_Time));
        }

        m_Setup = true;
    }
}
