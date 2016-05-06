using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour 
{
    static bool m_Setup = false;
    public static void Reset()
    {
        m_Setup = false;
    }

    Dropdown m_TeamSelector = null;

	void Start ()
	{
        m_TeamSelector = GetComponentInChildren<Dropdown>();
	}
	
	void Update () 
	{

	    if(m_Setup == false && Team.All.Length != 0)
        {
            Debug.Log("Teams are here!");
            m_Setup = true;
        }
	}
}
