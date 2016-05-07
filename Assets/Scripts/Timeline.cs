using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Timeline : MonoBehaviour 
{
    static Dictionary<int, List<TimelineEvent>> m_Events = new Dictionary<int, List<TimelineEvent>>();

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

    public static List<TimelineEvent> GetEvents(int a_Time)
    {
        if (m_Events.ContainsKey(a_Time))
            return m_Events[a_Time];
        else return null;
    }

    void Start()
    {
        m_Singleton = this;
    }

    void Update()
    {
        Prefetch();
    }

    float m_NextFetch = 0.0f;
    int m_FetchIndex = 0;
	void Prefetch()
	{
        if(m_NextFetch <= 0.0f)
        {
            m_Ready = false;
            m_NextFetch = (float)Settings.TimelineFetchSize * 0.8f; // Be slightly ahead (give 20% error)
            Debug.Log("Next fetch in " + m_NextFetch + " seconds.");
            int t_From = m_FetchIndex * Settings.GameSpeed * Settings.TimelineFetchSize;
            int t_Until = (m_FetchIndex + 1) * Settings.GameSpeed * Settings.TimelineFetchSize;
            Debug.Log("Fetching Timeline ("+m_FetchIndex+") from  " + (t_From / 60).ToString() + ":" + (t_From % 60).ToString() + " until " + (t_Until / 60).ToString() + ":" + (t_Until % 60).ToString());
            Debug.Log(Settings.TimelineURL(m_FetchIndex));
            HTTP.Request(Settings.TimelineURL(m_FetchIndex), delegate (WWW a_Request)
            {
                JSONNode t_Timeline = JSON.Parse(a_Request.text);

                int t_EventCount = 0;
                foreach (JSONNode t_TimeJSON in t_Timeline.AsArray)
                {
                    int t_Time = t_TimeJSON["time"].AsInt;
                    var t_EventList = new List<TimelineEvent>();
                    foreach (JSONNode t_Event in t_TimeJSON["events"].AsArray)
                    {
                        t_EventList.Add(new TimelineEvent(t_Time, t_Event));
                        t_EventCount++;
                    }
                    m_Events.Add(t_Time, t_EventList);
                }

                if (t_EventCount == 0)
                {
                    Game.AddSkip(t_From, t_Until);
                    m_NextFetch = -1.0f;
                }

                Debug.Log(t_EventCount + " events pushed.");
                m_Ready = true;
            }, false);
            m_FetchIndex++;
        }
        m_NextFetch -= Time.deltaTime;
	}
}
