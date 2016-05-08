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
    public int Deaths { get { return m_Deaths; } set { m_Deaths = value; ResetUI(); } }
    public int CS { get { return m_CS; } set { m_CS = value; ResetUI(); } }
    public Champion Champion = null;

    public bool Tilting { get { return m_TiltIcon.activeSelf; } set { m_TiltIcon.SetActive(value); } }
    public bool Trolling { get { return m_TrollIcon.activeSelf; } set { m_TrollIcon.SetActive(value); } }
    public bool AFK { get { return m_AFKIcon.activeSelf; } set { m_AFKIcon.SetActive(value); } }

    private float m_DeathTimer = 0.0f;

    public void SetDeathTimer(float a_Duration)
    {
        m_DeathTimer = a_Duration;
    }

    void Start()
    {
        m_TrollIcon = transform.Find("Trolling").gameObject;
        m_AFKIcon = transform.Find("AFK").gameObject;
        m_TiltIcon = transform.Find("Tilting").gameObject;
        m_DeadIcon = transform.Find("Dead").gameObject;

        m_Info = transform.Find("Info").GetComponent<Text>();
        m_OriginalScale = m_Info.transform.localScale;

        m_TrollIcon.SetActive(false);
        m_AFKIcon.SetActive(false);
        m_TiltIcon.SetActive(false);
        m_DeadIcon.SetActive(false);
    }

    Vector3 m_OriginalScale = Vector3.one;
    float m_ScaleTimer = 0.0f;
	void ResetUI() 
	{
        m_Info.text = "Kills: " + m_Kills + "\nDeaths: " + m_Deaths + "\nCS: " + m_CS;
        m_OriginalScale = m_Info.transform.localScale;
        m_ScaleTimer = 1.0f;
    }

    void Update()
    {
        if (m_DeathTimer > 0.0f)
        {
            m_DeathTimer -= Time.deltaTime * Settings.GameSpeed;
            m_DeadIcon.SetActive(true);
        }
        else
        {
            m_DeathTimer = 0.0f;
            m_DeadIcon.SetActive(false);
        }

        if (m_ScaleTimer > 0.0f)
        {
            m_Info.transform.localScale = Vector3.Lerp(m_OriginalScale, m_OriginalScale * 1.2f, m_ScaleTimer);
        }
        else m_Info.transform.localScale = m_OriginalScale;
        m_ScaleTimer -= Time.deltaTime;
    }
}
