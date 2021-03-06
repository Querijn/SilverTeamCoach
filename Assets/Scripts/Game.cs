﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    int m_Winner = -1;
    bool m_WinnerRequested = false;
    static int m_HandledTime = -1;
    static int m_HandledIndex = -1;
    static bool m_Over = false;

    static bool m_Skipping = false;
    public static bool Skipping { get { return m_Skipping; } }
    static List<int> m_StartSkipping = new List<int>();
    static List<int> m_StopSkipping = new List<int>();

    public static Team[] Teams = { new Team(), new Team() };

    public static Settings.PassThroughInfo Info = null;
    static bool m_Setup = false;
    float m_WaitAtLoading = 10.0f;

    bool m_ReadyToPlay = false;
    public Sprite[] m_MasteryLevels = null;


    public float m_SkipSpeed = 3.0f;
    float m_Timer = 0.0f;
    Text m_TimerText;
    public static Game m_Instance = null;
    public static Game Instance { get { return m_Instance; } }
    public AudioClip m_Music = null;
    public AudioClip m_Victory = null;
    public AudioClip m_Defeat = null;
    public static float CurrentTime { get { return Instance.m_Timer; }  }

    [Serializable]
    public struct SoundReference
    {
        public TimelineEvent.EventType Type;
        public AudioClip Clip;
    }

    public SoundReference[] m_Sounds;
    public static SoundReference[] Sounds { get { return Instance.m_Sounds; } }
    public static SoundReference GetSound(TimelineEvent.EventType a_Type)
    {
        return Array.Find(Sounds, s => s.Type == a_Type);
    }

    void Start ()
	{
        m_Instance = this;
        m_Winner = -1;
        m_WinnerRequested = false;
        m_HandledTime = -1;
        m_HandledIndex = -1;
        m_Over = false;

        m_Skipping = false;
        m_StartSkipping = new List<int>();
        m_StopSkipping = new List<int>();

        Teams[0] = new Team();
        Teams[1] =  new Team();
        
        m_Setup = false;
        m_WaitAtLoading = 10.0f;
        m_ReadyToPlay = false;
        m_Timer = 0.0f;

        if (m_Music != null)
            Sound.Play(m_Music, a_Music: true, a_Looping: true);
        m_TimerText = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<Text>();
	}


    public static void End()
    {
        m_Over = true;
    }

    void Update()
    {
        if (m_ReadyToPlay == false)
        {
            CheckSetup();
            return;
        }
        else if(m_Over == false)
        {
            HandleTimer();

            int t_Timer = Mathf.FloorToInt(m_Timer);
            if (m_HandledTime != t_Timer)
            {
                int i = m_HandledIndex + 1;
                for (; i < Timeline.Events.Count; i++)
                {

                    if (Timeline.Events[i].Time > t_Timer)
                    {
                        m_HandledIndex = i - 1;
                        break;
                    }
                    else if (Timeline.Events[i].Time > m_HandledTime)
                    {
                        Timeline.Events[i].Play();
                    }
                }
                m_HandledTime = i;
            }
        }
        else if(m_Winner == -1 && m_WinnerRequested == false)
        {
            // Determine outcome
            m_WinnerRequested = true;
            HTTP.Request(Settings.FormAjaxURL("get_match_result.php"), delegate (WWW a_Request)
            {
                JSONNode t_Results = JSON.Parse(a_Request.text);
                m_Winner = t_Results["Winner"].AsInt;
                var t_Sound = (m_Winner == 0 ? m_Victory : m_Defeat);
                if (t_Sound != null)
                {
                    Sound.Play(t_Sound, a_Wait: 1.0f);
                }
                Debug.Log("Winner = " + m_Winner);
                m_TimerText.text = Teams[m_Winner].Name + " has won!";
                StartCoroutine(BackToMenu());
            }, false);
        }
        else if(m_Winner != -1)
        {
            m_TimerText.text = Teams[m_Winner].Name + " has won!";

        }
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        
        SceneManager.LoadScene("Combined");
        //SceneManager.UnloadScene("Game");
        Info = null;
    }

    public static void AddSkip(int a_StartTime, int a_StopTime)
    {
        Debug.Log("Added skipping between " + a_StartTime.ToString() + " and " + a_StopTime.ToString());
        m_StartSkipping.Add(a_StartTime);
        m_StopSkipping.Add(a_StopTime);
    }

    void HandleTimer()
    {
        m_Skipping = false;
        int t_Time = Mathf.FloorToInt(m_Timer / 60.0f);
        for (int i = 0; i < m_StopSkipping.Count; i++)
        { 
            if(t_Time < m_StopSkipping[i])
            {
                if(t_Time > m_StartSkipping[i])
                {
                    m_Skipping = true;
                    Debug.Log("Skipping");
                    break;
                }
            }
            else
            {
                m_StopSkipping.RemoveAt(i);
                m_StartSkipping.RemoveAt(i);
            }
        }

        m_Timer += (float)Settings.GameSpeed * Time.deltaTime;
        int t_Minutes = Mathf.FloorToInt(m_Timer / 60.0f);
        string t_Seconds = ((int)m_Timer % 60).ToString();
        if (t_Seconds.Length == 1)
            t_Seconds = "0" + t_Seconds;
        m_TimerText.text = t_Minutes.ToString() + ":" + t_Seconds;
    }

    public class Team
    {
        public string Name;

        public GameChampion Top = null;
        public GameChampion Mid = null;
        public GameChampion Support = null;
        public GameChampion Marksman = null;
        public GameChampion Jungle = null;

        public GameChampion GetChampion(Role a_Role)
        {
            switch(a_Role)
            {
                case Role.Top:
                    return Top;
                case Role.Mid:
                    return Mid;
                case Role.Marksman:
                    return Marksman;
                case Role.Support:
                    return Support;
                case Role.Jungle:
                    return Jungle;
                default:
                    return null;
            }
        }
    }

    public static void Reset()
    {
        m_Setup = false;
    }

    void CheckSetup()
    {
        if (m_Setup == false && Info != null)
        {
            Debug.Log(Info.Request.text);

            JSONNode t_GameInfo = JSON.Parse(Info.Request.text);
            foreach (JSONNode t_Team in t_GameInfo["teams"].AsArray)
            {
                //if(t_Team["is_player"].AsInt == 1)
                //{
                //    GameObject.FindGameObjectWithTag("GameTeamPlayer").transform.Find("Name").GetComponent<Text>().text = ;
                //}

                string[] t_Roles = { "support", "marksman", "mid", "top", "jungle" };

                // Setup games below
                int i = 0;
                if (GameObject.FindGameObjectWithTag("GameLoadingScreen") != null)
                {
                    GameObject t_LoadingScreen = GameObject.FindGameObjectWithTag("GameLoadingScreen").transform.Find((t_Team["is_player"].AsBool ? "Top" : "Bottom")).Find("Champs").gameObject;
                    Text t_Name = t_LoadingScreen.transform.parent.Find("Name").GetComponent<Text>();
                    t_Name.text = t_Team["player"]["name"] + "'s " + t_Team["team"]["Name"].Value;

                    foreach (string t_RoleString in t_Roles)
                    {
                        JSONNode t_Role = t_Team["champions"][t_RoleString];

                        GameObject t_Prefab = Resources.Load("Prefabs/GameLoadingScreenChampion") as GameObject;
                        GameObject t_Instance = Instantiate(t_Prefab) as GameObject;

                        t_Instance.transform.SetParent(t_LoadingScreen.transform);
                        t_Instance.transform.localScale = Vector3.one * 0.8f;
                        t_Instance.transform.localPosition = new Vector3(((float)i - 2) * t_Instance.GetComponent<RectTransform>().rect.width, 0);
                        i++;

                        t_Instance.name = t_Role["name"].Value;
                        t_Instance.transform.Find("Name").GetComponent<Text>().text = t_Role["name"].Value;

                        int t_MasteryLevel = t_Role["mastery"]["level"].AsInt - 1;
                        if (t_MasteryLevel >= 0 && t_MasteryLevel < m_MasteryLevels.Length)
                        {
                            Image t_Image = t_Instance.transform.Find("Mastery").GetComponent<Image>();
                            t_Image.sprite = m_MasteryLevels[t_MasteryLevel];
                            t_Image.color = Color.white;
                        }

                        string t_URL = Settings.ChampionLoadingImageDirectory + t_Role["key"].Value + "_" + t_Role["skin"].AsInt.ToString() + ".jpg";
                        HTTP.Request(t_URL, delegate (WWW a_Request)
                        {
                            t_Instance.transform.Find("Loading").GetComponent<Image>().sprite = Sprite.Create(a_Request.texture, new Rect(0.0f, 0.0f, a_Request.texture.width, a_Request.texture.height), Vector2.zero);
                            t_Instance.transform.Find("Loading").GetComponent<Image>().color = Color.white;
                        }, false);
                    }
                }
                else m_WaitAtLoading = 3.0f;

                i = 0;
                GameObject[] t_TeamObjects = GameObject.FindGameObjectsWithTag("GameTeamUI");
                GameTeamUI t_TeamUI = Array.Find(t_TeamObjects, g => g.GetComponent<GameTeamUI>().Team == (t_Team["is_player"].AsBool ? 0 : 1)).GetComponent<GameTeamUI>();
                Transform t_ChampContainer = t_TeamUI.transform.Find("Champs");
                t_TeamUI.transform.Find("Name").GetComponent<Text>().text = t_Team["team"]["Name"].Value;

                // Debug.Log("Teamname: "+t_Team["team"]["Name"].Value);
                foreach (string t_RoleString in t_Roles)
                {
                    GameObject t_Prefab = Resources.Load("Prefabs/GameTeamUIChampion") as GameObject;
                    GameObject t_Instance = Instantiate(t_Prefab) as GameObject;
                    
                    t_Instance.transform.SetParent(t_ChampContainer.transform);
                    t_Instance.transform.localScale = Vector3.one;
                    Vector3 t_BasePosition = new Vector3(-450, 350);
                    t_Instance.transform.localPosition = t_BasePosition + new Vector3(((float)i) * t_Instance.GetComponent<RectTransform>().rect.width, ((float)-i) * t_Instance.GetComponent<RectTransform>().rect.height * 1.1f);
                    if (t_Team["is_player"].AsBool == false)
                        foreach (Text t_Text in t_Instance.GetComponentsInChildren<Text>())
                        {
                            t_Text.transform.localScale = new Vector3(-1, 1);
                            t_Text.transform.localPosition -= new Vector3(t_Text.GetComponent<RectTransform>().rect.width*0.5f, 0);
                        }
                    i++;

                    Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Name = t_Team["team"]["Name"].Value;
                    switch (t_RoleString)
                    {
                        case "support":
                            Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Support = t_Instance.GetComponent<GameChampion>();
                            break;
                        case "marksman":
                            Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Marksman = t_Instance.GetComponent<GameChampion>();
                            break;
                        case "mid":
                            Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Mid = t_Instance.GetComponent<GameChampion>();
                            break;
                        case "top":
                            Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Top = t_Instance.GetComponent<GameChampion>();
                            break;
                        case "jungle":
                            Teams[(t_Team["is_player"].AsBool) ? 0 : 1].Jungle = t_Instance.GetComponent<GameChampion>();
                            break;
                    }


                    JSONNode t_Role = t_Team["champions"][t_RoleString];
                    t_Instance.name = t_Role["name"].Value;

                    var t_GameChamp = t_Instance.GetComponent<GameChampion>();
                    t_GameChamp.Champion = Champion.Get(t_Role["id"].AsInt);
                    t_Instance.GetComponent<Image>().sprite = GetFlipped(t_GameChamp.Champion.Image);

                }
            }

            Timeline.Fetch();

            if (Matchmaking.IsTesting() == false)
            {
                SceneManager.UnloadScene("Combined");
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
            if (GameObject.FindGameObjectWithTag("GameLoadingScreen") == null)
                m_ReadyToPlay = Timeline.Ready;

            else if (m_WaitAtLoading <= 0.0f)
            {
                if (m_WaitAtLoading < -1.0f)
                {
                    m_ReadyToPlay = Timeline.Ready;
                    GameObject.FindGameObjectWithTag("GameLoadingScreen").SetActive(false);
                }
                else
                {
                    Image t_Background = GameObject.FindGameObjectWithTag("GameLoadingScreen").GetComponent<Image>();

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
        }
    }

    private Sprite GetFlipped(Sprite a_Image)
    {
        Rect rect = a_Image.rect;
        rect.x += rect.width;
        rect.width = -rect.width;
        return Sprite.Create(a_Image.texture, rect, a_Image.pivot);
    }

    void Fade(Image a_Image)
    {
        Color t_Colour = a_Image.color;

        t_Colour.a -= 2.0f * Time.deltaTime;
        a_Image.color = t_Colour;
    }

    void Fade(Text a_Text)
    {
        Color t_Colour = a_Text.color;

        t_Colour.a -= 2.0f*Time.deltaTime;
        a_Text.color = t_Colour;
    }
}
