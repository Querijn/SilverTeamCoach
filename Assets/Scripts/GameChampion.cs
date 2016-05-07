using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameChampion : MonoBehaviour 
{
    GameObject m_TrollIcon;
    GameObject m_AFKIcon;
    GameObject m_TiltIcon;
    GameObject m_DeadIcon;

    Text m_Info;

    private int m_Kills = 0;
    private int m_Deaths = 0;
    private int m_CS = 0;
    public int Kills { get { return m_Kills; } set { m_Kills = value; ResetUI(); } } 

    void Start()
    {
        m_TrollIcon = transform.Find("Trolling").gameObject;
        m_AFKIcon = transform.Find("AFK").gameObject;
        m_TiltIcon = transform.Find("Tilting").gameObject;
        m_DeadIcon = transform.Find("Dead").gameObject;

        m_Info = transform.Find("Info").GetComponent<Text>();

        m_TrollIcon.SetActive(false);
        m_AFKIcon.SetActive(false);
        m_TiltIcon.SetActive(false);
        m_DeadIcon.SetActive(false);
    }
	
	void ResetUI() 
	{
        m_Info.text = "Kills: " + m_Kills + "\nDeaths: " + m_Deaths + "\nCS: " + m_CS;
    }
}
