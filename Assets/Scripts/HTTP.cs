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
            Callback = a_Callback;
            Important = a_Important;
        }

        public WWW Request { get; private set; }
        public HTTPCallback Callback { get; private set; }
        public bool Important;
    }

    static List<RequestWithCB> m_Requests = new List<RequestWithCB>();

    public static void Request(string a_URL, HTTPCallback a_Callback, bool a_Important)
    {
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
                t_Request.Callback.Invoke(t_Request.Request);
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
