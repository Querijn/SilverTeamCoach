using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChampionListElement : MonoBehaviour 
{
    int m_Champion = -1;
    bool m_ReloadImage = false;

	public void FillInfo(Champion a_Champion)
    {
        m_Champion = a_Champion.ID;
        if (a_Champion.Image != null)
            GetComponentInChildren<Image>().sprite = a_Champion.Image;

        else m_ReloadImage = true;

        transform.Find("Text/Name").GetComponent<Text>().text = a_Champion.Name;
        transform.Find("Text/Mastery/Level").GetComponent<Text>().text = "Level " + a_Champion.Mastery.Level.ToString();
        transform.Find("Text/Mastery/Points").GetComponent<Text>().text = a_Champion.Mastery.Points.ToString() + " LP";
        transform.Find("Text/Winrate").GetComponent<Text>().text = "No winrate";// (a_Champion.WinRate * 100.0f).ToString() + "%";

        string t_BestLanes = a_Champion.GetBestLanes();
        transform.Find("Text/Lane").GetComponent<Text>().text = "Best fit: " + t_BestLanes;
    }

    void Update()
    {
        if (m_Champion == -1)
            return;

        if (m_ReloadImage == true && Champion.Get(m_Champion).Image != null)
            FillInfo(Champion.Get(m_Champion));
    }
}
