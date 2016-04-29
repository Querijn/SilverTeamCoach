using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stats : MonoBehaviour 
{
    public static void Reset()
    {
        m_Setup = false;
    }

    static bool m_Setup = false;

	void Start ()
	{
	}
	
	void Update ()
    {
        if (Info.Setup() && m_Setup == false)
        {
            foreach(Transform t_Transform in gameObject.transform)
            {
                switch(t_Transform.name)
                {
                    case "User":
                        t_Transform.GetComponent<Text>().text = Info.Player.Name;
                        break;

                    case "Cash":
                        t_Transform.GetComponent<Text>().text = Cash.Format(Info.Player.Cash);
                        break;

                    case "Team":

                        break;
                    default:
                        Debug.Log(t_Transform.name);
                        break;

                }
            }
            m_Setup = true;
        }
    }
}
