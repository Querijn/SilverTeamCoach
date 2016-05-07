﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    Settings.PassThroughInfo m_GameInfo = null;
    static bool m_Setup = false;
    float m_WaitAtLoading = 10.0f;

    bool m_ReadyToPlay = false;

    public Sprite[] m_MasteryLevels = null;

    public static void Reset()
    {
        m_Setup = false;
    }

    void Start ()
	{
	
	}
	
	void Update () 
	{
        if (Settings.PassThrough != null && m_Setup == false)
        {
            m_GameInfo = Settings.PassThrough;

            Debug.Log("Match is loaded, displaying.");

            JSONNode t_GameInfo = JSON.Parse(m_GameInfo.Request.text);
            foreach (JSONNode t_Team in t_GameInfo["teams"].AsArray)
            {
                //if(t_Team["is_player"].AsInt == 1)
                //{
                //    GameObject.FindGameObjectWithTag("GameTeamPlayer").transform.Find("Name").GetComponent<Text>().text = ;
                //}

                string[] t_Roles = { "support", "marksman", "mid", "top", "jungle" };

                int i = 0;
                GameObject t_LoadingScreen = GameObject.FindGameObjectWithTag("GameLoadingScreen").transform.Find((t_Team["is_player"].AsBool ? "Top" : "Bottom")).Find("Champs").gameObject;
                Text t_Name = t_LoadingScreen.transform.parent.Find("Name").GetComponent<Text>();
                t_Name.text = t_Team["player"]["name"]+"'s " + t_Team["team"]["Name"].Value;

                foreach (string t_RoleString in t_Roles)
                {
                    JSONNode t_Role = t_Team["champions"][t_RoleString];

                    GameObject t_Prefab = Resources.Load("Prefabs/GameLoadingScreenChampion") as GameObject;
                    GameObject t_Instance = Instantiate(t_Prefab) as GameObject;

                    t_Instance.transform.SetParent(t_LoadingScreen.transform);
                    t_Instance.transform.localScale = Vector3.one * 0.8f;
                    t_Instance.transform.localPosition = new Vector3(((float)i - 2) * t_Instance.GetComponent<RectTransform>().rect.width,  0);
                    i++;
                    
                    t_Instance.name = t_Role["name"].Value;
                    t_Instance.transform.Find("Name").GetComponent<Text>().text = t_Role["name"].Value;

                    int t_MasteryLevel = t_Role["mastery"]["level"].AsInt - 1;
                    if(t_MasteryLevel >= 0 && t_MasteryLevel < m_MasteryLevels.Length)
                    {
                        Image t_Image = t_Instance.transform.Find("Mastery").GetComponent<Image>();
                        t_Image.sprite = m_MasteryLevels[t_MasteryLevel];
                        t_Image.color = Color.white;
                    }

                    string t_URL = Settings.ChampionLoadingImageDirectory + t_Role["key"].Value + "_" + t_Role["skin"].AsInt.ToString() + ".jpg";
                    HTTP.Request(t_URL, delegate(WWW a_Request)
                    {
                        t_Instance.transform.Find("Loading").GetComponent<Image>().sprite = Sprite.Create(a_Request.texture, new Rect(0.0f, 0.0f, a_Request.texture.width, a_Request.texture.height), Vector2.zero);
                        t_Instance.transform.Find("Loading").GetComponent<Image>().color = Color.white;
                    }, false);
                }
            }

            if (Matchmaking.IsTesting() == false)
            {
                SceneManager.UnloadScene("Main");
            }
            else
            {
                Matchmaking.UnsetTestEnvironment();
            }
            m_Setup = true;
        }

        if(m_Setup == true && m_ReadyToPlay == false)
        {
            m_WaitAtLoading -= Time.deltaTime;

            if(m_WaitAtLoading < 0.0f)
            {
                Image t_Background = GameObject.FindGameObjectWithTag("GameLoadingScreen").GetComponent<Image>();
                m_ReadyToPlay = Fade(t_Background);

                foreach (Image t_Image in t_Background.GetComponentsInChildren<Image>())
                {
                    Fade(t_Image);
                }

                foreach (Text t_Text in t_Background.GetComponentsInChildren<Text>())
                {
                    Fade(t_Text);
                }
            }
        }
        if(m_ReadyToPlay)
        {
            Debug.Log("Ready2play");
        }
    }

    bool Fade(Image a_Image)
    {

        Color t_Colour = a_Image.color;

        t_Colour.a -= 2.0f * Time.deltaTime;
        a_Image.color = t_Colour;

        return (t_Colour.a == 0.0);
    }

    bool Fade(Text a_Text)
    {

        Color t_Colour = a_Text.color;

        t_Colour.a -= 2.0f*Time.deltaTime;
        a_Text.color = t_Colour;

        return (t_Colour.a == 0.0);
    }
}