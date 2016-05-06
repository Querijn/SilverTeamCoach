using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
    Vector3 m_Position = Vector3.zero;
    float m_ShakingFor = 0.0f;

	void Start ()
	{
        m_Position = transform.position;
        Shake();
	}
	
	void Update () 
	{
	    if(m_ShakingFor > 0.0f)
        {
            m_ShakingFor -= Time.deltaTime;
        }
	}

    void Shake()
    {

    }
}
