using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public float m_SkipSpeed = 3.0f;
    float m_Timer = 0.0f;
    Text m_TimerText;

    void Start ()
	{
        gameObject.AddComponent<Timeline>();
        m_TimerText = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<Text>();
	}

    static int m_HandledTime = -1;
    void Update()
    {
        if (m_ReadyToPlay == false)
        {
            CheckSetup();
            return;
        }
        
        HandleTimer();
        
        int t_Timer = Mathf.FloorToInt(m_Timer);
        // Did we do this time already
        if(m_HandledTime != t_Timer)
        {
            var t_Events = Timeline.GetEvents(t_Timer);
            if (t_Events == null)
                return;

            foreach (TimelineEvent t_Event in t_Events)
            {
                Debug.Log("An event called " + t_Event.Type.ToString() + " just occurred.");
            }
            m_HandledTime = t_Timer;
        }
    }

    static bool m_Skipping = false;
    public static bool Skipping { get { return m_Skipping; } }
    static List<int> m_StartSkipping = new List<int>();
    static List<int> m_StopSkipping = new List<int>();

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
        public GameChampion Top = null;
        public GameChampion Mid = null;
        public GameChampion Support = null;
        public GameChampion Marksman = null;
        public GameChampion Jungle = null;
    }
    static Team[] Teams = { new Team(), new Team() };

    Settings.PassThroughInfo m_GameInfo = null;
    static bool m_Setup = false;
    float m_WaitAtLoading = 10.0f;

    bool m_ReadyToPlay = false;
    public Sprite[] m_MasteryLevels = null;

    public static void Reset()
    {
        m_Setup = false;
    }

    void CheckSetup()
    {
        if (Settings.PassThrough != null && m_Setup == false)
        {
            m_GameInfo = Settings.PassThrough;

            // Debug.Log("Match is loaded, displaying.");

            JSONNode t_GameInfo = JSON.Parse(m_GameInfo.Request.text);
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

                    t_Instance.GetComponent<Image>().sprite = GetFlipped(Champion.Get(t_Role["id"].AsInt).Image);

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
