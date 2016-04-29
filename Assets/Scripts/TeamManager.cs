using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour 
{
	void Start ()
	{
	 
	}

    static bool m_Setup = false;
    static bool m_InProgress = false;
    public static void Reset()
    {
        m_Setup = false;
        m_InProgress = false;
    }

	void Update () 
	{
        if (m_Setup == false && m_InProgress == false && Champion.Get(1) != null && Champion.Get(1).Image != null)
        {
            HTTP.Request(Settings.FormAjaxURL("teams.php"), delegate(WWW a_Request) 
            {
                var t_JSON = JSON.Parse(a_Request.text);

                GameObject t_Prefab = Resources.Load("Prefabs/TeamListObject") as GameObject;
                RectTransform t_PrefabTransform = t_Prefab.GetComponent<RectTransform>();
                GameObject t_Content = GameObject.FindGameObjectWithTag("TeamsContent");

                int y = 0;
                foreach (JSONNode t_Team in t_JSON.AsArray)
                {
                    GameObject t_Instance = Instantiate(t_Prefab);
                    t_Instance.transform.SetParent(t_Content.transform);

                    t_Instance.name = t_Team["name"];
                    t_Instance.transform.Find("Name").GetComponent<Text>().text = t_Team["name"];
                    
                    t_Instance.transform.Find("Champions/Top").GetComponentInChildren<Image>().sprite = Champion.Get(t_Team["top"].AsInt).Image;
                    t_Instance.transform.Find("Champions/Mid").GetComponentInChildren<Image>().sprite = Champion.Get(t_Team["mid"].AsInt).Image;
                    t_Instance.transform.Find("Champions/Support").GetComponentInChildren<Image>().sprite = Champion.Get(t_Team["support"].AsInt).Image;
                    t_Instance.transform.Find("Champions/Marksman").GetComponentInChildren<Image>().sprite = Champion.Get(t_Team["marksman"].AsInt).Image;
                    t_Instance.transform.Find("Champions/Jungle").GetComponentInChildren<Image>().sprite = Champion.Get(t_Team["jungle"].AsInt).Image;

                    t_Instance.transform.localPosition = new Vector3(0, (-y * t_PrefabTransform.sizeDelta.y));
                    t_Instance.transform.localScale = Vector3.one;
                    y++;
                }

                t_Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40 + (y * t_PrefabTransform.sizeDelta.y));

                //t_JSON
                m_Setup = true;
            }, true);
            m_InProgress = true;
        }
	}

    public void OnCreateTeam()
    {
        if(Info.Player.OwnedChampions.Length >= 5)
        {
            CreateTeamWindow.Instance.SetActive(true);
        }
        else
        {
            Error.Show("You cannot create a team until you have at least 5 champions!");
        }
    }
}
