using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
    Vector3 m_Position = Vector3.zero;
    float m_ShakingFor = 0.0f;
    public bool Destroyed = false;
    Vector3 m_Velocity = Vector3.zero;
    Vector3 m_Rotation = Vector3.zero;

	void Start ()
	{
        m_Position = transform.position;
        m_Rotation = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        m_Rotation.Normalize();
    }
	
	void Update () 
	{
	    if(m_ShakingFor > 0.0f)
        {
            m_ShakingFor -= Time.deltaTime;

            var t_Dir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            t_Dir.Normalize();

            transform.position = m_Position + t_Dir * 0.1f* m_ShakingFor;
        }

        if(Destroyed)
        {
            m_Velocity.y -= Time.deltaTime * 0.01f;
            transform.position += m_Velocity;
            m_Position += m_Velocity;

            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + m_Rotation);
        }

	}
    
    public enum Location { Base, Top, Mid, Bot };
    public static Tower Get(int a_Team, Location a_Lane, int a_Index = -1)
    {
        int t_Start = 0;
        int t_Count = a_Lane == Location.Base ? 2 : 3;
        if (a_Index != -1)
        {
            t_Start = a_Index;
            t_Count = 1;
        }

        for (int i = t_Start; i < t_Start + t_Count; i++)
        {
            GameObject t_Object = GameObject.Find("Team" + a_Team.ToString() + "/" + a_Lane.ToString() + "/" + i.ToString());
            if (t_Object == null)
                continue;

            var t_Tower = t_Object.GetComponent<Tower>();
            if (t_Tower == null || t_Tower.Destroyed == true)
                continue;

            return t_Tower;
        }

        return null;
    }

    public static Tower Baron
    {
        get
        {
            return GameObject.Find("Baron").GetComponent<Tower>();
        }
    }

    public static Tower Dragon
    {
        get
        {
            return GameObject.Find("Dragon").GetComponent<Tower>();
        }
    }

    public void Shake()
    {
        m_ShakingFor = 1.5f;
    }

    public void Destroy()
    {
        Shake();
        Destroyed = true;
    }
}
