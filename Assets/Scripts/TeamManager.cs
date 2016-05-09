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
    static JSONArray m_Array = null;
    public static void Reset()
    {
        m_Setup = false;
        m_InProgress = false;
        m_Array = null;
    }

	void Update () 
	{
        if (m_Setup == false && m_InProgress == false && Champion.Get(1) != null && Champion.Get(1).Image != null)
        {
            HTTP.Request(Settings.FormAjaxURL("teams.php"), delegate(WWW a_Request) 
            {
                var t_JSON = JSON.Parse(a_Request.text);

                m_Array = t_JSON.AsArray;

                Team.Setup(m_Array);

                // Debug.Log("Teams loaded.");
                m_Setup = true;
            }, true);
            m_InProgress = true;
        }

        if(m_Array != null && GameObject.FindGameObjectWithTag("TeamsContent") != null)
        {
            GameObject t_Content = GameObject.FindGameObjectWithTag("TeamsContent");

            foreach (Transform t_Child in t_Content.transform)
                Destroy(t_Child.gameObject);

            GameObject t_Prefab = Resources.Load("Prefabs/TeamListObject") as GameObject;
            RectTransform t_PrefabTransform = t_Prefab.GetComponent<RectTransform>();

            int y = 0;
            foreach (JSONNode t_Team in m_Array)
            {
                GameObject t_Instance = Instantiate(t_Prefab);
                t_Instance.transform.SetParent(t_Content.transform);

                t_Instance.name = t_Team["name"];
                t_Instance.transform.Find("Name").GetComponent<Text>().text = t_Team["name"].Value;
                t_Instance.transform.Find("ID").GetComponent<Text>().text = t_Team["id"].Value;

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
            m_Array = null;
        }
	}
}
