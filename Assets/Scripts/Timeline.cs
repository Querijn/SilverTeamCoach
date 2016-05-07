using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Timeline : MonoBehaviour 
{
    Dictionary<int, List<TimelineEvent>> m_Events = new Dictionary<int, List<TimelineEvent>>();

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
            m_NextFetch = (float)Settings.TimelineFetchSize;
            Debug.Log("Fetching Timeline "+m_FetchIndex);
            HTTP.Request(Settings.TimelineURL(m_FetchIndex), delegate (WWW a_Request)
            {
                JSONNode t_Timeline = JSON.Parse(a_Request.text);
                int t_TimelineDuration = 5 * Settings.TimelineFetchSize;
                foreach (JSONNode t_Time in t_Timeline.AsArray)
                {
                    foreach(JSONNode t_Event in t_Time["events"].AsArray)
                    {
                        Debug.Log(t_Event["name"].Value);
                    }
                }
                m_Ready = true;
            }, false);
            m_FetchIndex++;
        }
        m_NextFetch -= Time.deltaTime;
	}
}
