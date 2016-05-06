using UnityEngine;
using System.Collections;

public class DebuffIcon : MonoBehaviour 
{
    Vector3 m_Scale = Vector3.one;
    float m_Time = 0.0f;

	void Start ()
	{
        m_Scale = transform.localScale;   
	}
	
	void Update () 
	{
        m_Time += Time.deltaTime;

        transform.localScale += new Vector3(0.005f * m_Time, 0.005f * m_Time, 0.005f * m_Time);

        if(m_Time >= 1.0f)
        {
            m_Time -= 2.0f;
        }
	}
}
