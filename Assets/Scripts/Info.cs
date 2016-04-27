using UnityEngine;
using System.Collections;
using SimpleJSON;

public static class Info 
{
    static bool m_Setup = false;
    static bool m_InProgress = false;

    public struct PlayerInfo
    {
        public PlayerInfo(string a_Name, double a_Cash)
        {
            Name = a_Name;
            Cash = a_Cash;
        }

        public string Name { get; private set; }
        public double Cash { get; private set; }
    };

    public static PlayerInfo Player { get; private set; }

    public static bool Setup()
	{
        if (m_Setup == true || m_InProgress == true)
            return m_Setup;

        HTTP.Request("http://localhost/ajax/init.php", delegate (WWW a_Request)
        {
            Debug.Log(a_Request.text);
            var t_JSON = JSON.Parse(a_Request.text);

            if (t_JSON["error"].Value!="")
            {
                Debug.LogError("'" + t_JSON["error"]+ "'");
                return;
            }

            Champion.Setup(t_JSON["champions"].AsArray);

            Player = new PlayerInfo(t_JSON["name"], t_JSON["cash"].AsDouble);
            Debug.Log("Initialisation complete, username is '" + Player.Name + "', and has " + Player.Cash + " cash.");
            m_Setup = true;
            m_InProgress = false;
        });

        m_InProgress = true;
        return m_Setup;
    }
}
