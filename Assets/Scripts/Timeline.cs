using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Timeline : MonoBehaviour 
{
    static List<TimelineEvent> m_Events = new List<TimelineEvent>();
    public static List<TimelineEvent> Events { get { return m_Events; } }

    bool m_Ready = false;
    static Timeline m_Singleton = null;
    static public bool Ready
    {
        get
        {
            if (m_Singleton == null)
                return false;
            else return m_Singleton.m_Ready;
        }
    }

    void Start()
    {
        m_Events = new List<TimelineEvent>();
        m_Singleton = this;
    }

    void Update()
    {
        Get();
    }
    
    static bool FetchRequested = false;
    public static void Fetch()
    {
        FetchRequested = true;
    }
    
	void Get()
	{
        if(FetchRequested == true && m_Ready == false)
        {
            HTTP.Request(Settings.TimelineURL(0), delegate (WWW a_Request)
            {
                // Debug.Log(a_Request.text);
                JSONNode t_Timeline = JSON.Parse(a_Request.text);

                int t_EventCount = 0;
                foreach (JSONNode t_Event in t_Timeline.AsArray)
                {
                    int t_Time = t_Event["time"].AsInt;
                    t_EventCount++;
                    m_Events.Add(new TimelineEvent(t_Time, t_Event));
                }


                m_Events.Sort((E1, E2) => E1.Time.CompareTo(E2.Time));
                m_Ready = true;
                // Debug.Log(t_EventCount + " events pushed.");
            }, false);
            FetchRequested = false;
        }
	}
}
