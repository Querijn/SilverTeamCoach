using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Matchmaking : MonoBehaviour 
{    
    static bool m_Test = false;
    static Matchmaking m_TestEnv = null;

    static bool m_Setup = false;
    public static void Reset()
    {
        if (m_Test)
            return;

        m_Setup = false;
    }

    public static bool IsTesting()
    {
        return m_Test;
    }

    public static void UnsetTestEnvironment()
    {
        Destroy(m_TestEnv.GetComponent<Settings>());
        Destroy(m_TestEnv.GetComponent<TeamManager>());
    }

    Dropdown m_TeamSelector = null;

	void Start ()
	{
        if (SceneManager.GetActiveScene().name == "Game")
        {
            m_Test = true;
            m_TestEnv = this;
            Debug.Log("Test detected, running matchmaking");
            Settings t_Settings = gameObject.AddComponent<Settings>();
            t_Settings.m_LoadEverything = false;
            gameObject.AddComponent<TeamManager>();
            return;
        }

        m_TeamSelector = GetComponentInChildren<Dropdown>();
    }
	
	void Update () 
	{
        if (m_Test == false)
        {
            if (m_Setup == false && Team.All.Length != 0)
            {
                m_TeamSelector.ClearOptions();
                List<Dropdown.OptionData> t_List = new List<Dropdown.OptionData>();

                int i = 0;
                int t_Selected = 0;
                foreach (Team t_Team in Team.All)
                {
                    if (Info.Player.Team == t_Team.Name)
                        t_Selected = i;

                    t_List.Add(new Dropdown.OptionData(t_Team.Name));
                    i++;
                }
                m_TeamSelector.AddOptions(t_List);
                m_TeamSelector.value = t_Selected;
                m_Setup = true;
            }
        }
        else
        {
            if (m_Setup == false && Team.All.Length != 0 && Info.Player != null)
            {
                //  Debug.Log("Requesting battle against bot..");
                BattleAgainst(BattleType.Bot);
                m_Setup = true;
            }
        }
	}

    public enum BattleType
    {
        Ranked,
        Bot,
        LCS,
        Challenger
    };



    public void BattleAgainst(string a_Type)
    {
        try
        {
            BattleType t_Type = (BattleType)Enum.Parse(typeof(BattleType), a_Type);
            BattleAgainst(t_Type);
        }
        catch(Exception e)
        {
            Debug.LogError("Error when requesting battle: "+e.Message);
        }
    }


    public void BattleAgainst(BattleType a_Type)
    {
        if(GetSelectedTeam() == null)
        {
            Error.Show("No team selected!");
            return;
        }

        Dictionary<string, string> t_Commands = new Dictionary<string, string>();
        t_Commands.Add("team", GetSelectedTeam().ID.ToString());

        switch(a_Type)
        {
            case BattleType.Bot:
                t_Commands.Add("match", "bot");
                break;
            case BattleType.Ranked:
                t_Commands.Add("match", "ranked");
                break;
            case BattleType.LCS:
                t_Commands.Add("match", "lcs");
                break;
            case BattleType.Challenger:
                t_Commands.Add("match", "challenger");
                break;
        }

        string t_CommandString = "get_match.php?";
        foreach (var t_Command in t_Commands)
        {
            t_CommandString += t_Command.Key + "=" + Uri.EscapeDataString(t_Command.Value) + "&";
        }
        t_CommandString = t_CommandString.Substring(0, t_CommandString.Length - 1);

        // Debug.Log(t_CommandString);
        HTTP.Request(Settings.FormAjaxURL(t_CommandString), delegate (WWW a_Request)
        {
            // Match gotten

            // Are we not in Game?
            if (m_Test == false)
            {
                for (int i = 0; i < SceneManager.sceneCount && SceneManager.sceneCount > 1; i++)
                {
                    Scene t_Scene = SceneManager.GetSceneAt(i);
                    if (t_Scene.name == "Main")
                        continue;
                    else SceneManager.UnloadScene(t_Scene.name);
                }

                // All scenes are now unloaded, load Game
                SceneManager.LoadScene("Game");
            }

            Settings.PassThrough = new Settings.PassThroughInfo();
            Settings.PassThrough.Request = a_Request;
            // Debug.Log("Match received. Waiting for Game to hold.");

        }, true);
    }

    public Team GetSelectedTeam()
    {
        if (m_TeamSelector != null)
            return Team.Get(m_TeamSelector.options[m_TeamSelector.value].text);
        else return Team.Get(Info.Player.Team);
    }
}
