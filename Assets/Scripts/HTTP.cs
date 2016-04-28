using UnityEngine;
using System.Collections.Generic;

public class HTTP : MonoBehaviour
{
    public delegate void HTTPCallback(WWW a_Request);

    struct RequestWithCB
    {
        public RequestWithCB(WWW a_Request, HTTPCallback a_Callback, bool a_Important)
        {
            Request = a_Request;
            Callbacks = new List<HTTPCallback>();
            Callbacks.Add(a_Callback);
            Important = a_Important;
        }

        public void Add(HTTPCallback a_RequestCallback)
        {
            Callbacks.Add(a_RequestCallback);
        }

        public WWW Request { get; private set; }
        public List<HTTPCallback> Callbacks;
        
        public bool Important;
    }

    static List<RequestWithCB> m_Requests = new List<RequestWithCB>();

    public static void Request(string a_URL, HTTPCallback a_Callback, bool a_Important)
    {
        for(int i = 0; i< m_Requests.Count; i++)
        {
            if (m_Requests[i].Request.url == a_URL)
            {
                m_Requests[i].Add(a_Callback);
                return;
            }
        }

        m_Requests.Add(new RequestWithCB(new WWW(a_URL), a_Callback, a_Important));
    }

    bool HasImportantTask()
    {
        for (int i = m_Requests.Count - 1; i >= 0; i--)
            if (m_Requests[i].Important)
                return true;

        return false;
    }

    void Update ()
    {
        bool t_ListChanged = false;
        for(int i = m_Requests.Count - 1; i >= 0; i--)
        {
            var t_Request = m_Requests[i];

            if (t_Request.Request.isDone)
            {
                foreach(HTTPCallback t_Callback in t_Request.Callbacks)
                    t_Callback.Invoke(t_Request.Request);

                m_Requests.Remove(t_Request);
                t_ListChanged = true;
            }
        }

        if(t_ListChanged && HasImportantTask())
        {
            // TODO turn on or off blocking spinner
        }
    }
}
