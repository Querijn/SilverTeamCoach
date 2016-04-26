using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;


public class HTTP : MonoBehaviour
{
    public delegate void HTTPCallback(WWW a_Request);

    struct RequestWithCB
    {
        public RequestWithCB(WWW a_Request, HTTPCallback a_Callback)
        {
            Request = a_Request;
            Callback = a_Callback;
        }

        public WWW Request { get; private set; }
        public HTTPCallback Callback { get; private set; }
    }

    static List<RequestWithCB> m_Requests = new List<RequestWithCB>();

    public static void Request(string a_URL, HTTPCallback a_Callback)
    {
        m_Requests.Add(new RequestWithCB(new WWW(a_URL), a_Callback));
    }

    void Start()
    {
        Request("http://localhost/ajax/init.php", delegate (WWW a_Request)
        {
            print(a_Request.text);
            var t_JSON = JSON.Parse(a_Request.text);

            if (t_JSON["error"] == "")
            {

            }
            else Debug.LogError(t_JSON["error"]);
        });
    }

    void Update ()
    {
        for(int i = m_Requests.Count - 1; i >= 0; i--)
        {
            var t_Request = m_Requests[i];

            if (t_Request.Request.isDone)
            {
                t_Request.Callback.Invoke(t_Request.Request);
                m_Requests.Remove(t_Request);
            }
        }
    }
}
