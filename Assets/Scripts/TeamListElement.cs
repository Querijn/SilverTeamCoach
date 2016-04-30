using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TeamListElement : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update () 
	{

    }

    public void OnEditTeam()
    {
        int t_ID = Int32.Parse(transform.Find("ID").GetComponent<Text>().text);
        Team t_Team = Team.Get(t_ID);

        GameObject t_Window = EditTeamWindow.Instance;
        EditTeamWindow.EditingTeam = t_Team;
        t_Window.SetActive(true);
        t_Window.transform.Find("Content/Title").GetComponent<Text>().text = "Editing '" + t_Team.Name + "'";
        t_Window.transform.Find("Content/Name/Field").GetComponent<InputField>().text = t_Team.Name;

        Transform t_Roles = t_Window.transform.Find("Content/Champions");
        foreach (Transform t_RoleElement in t_Roles)
        {
            if (t_RoleElement.name == "ShowAllChampions" || t_RoleElement.GetComponent<EditChampionDropdown>() == null)
                continue;

            t_RoleElement.GetComponent<EditChampionDropdown>().Start();
        }
        EditChampionDropdown.Reset();
        
        foreach (Transform t_RoleElement in t_Roles)
        {
            if (t_RoleElement.name == "ShowAllChampions")
                continue;
            
            switch(t_RoleElement.name)
            {
                case "Top":
                    t_RoleElement.GetComponent<EditChampionDropdown>().Value = t_Team.Top;
                    break;
                case "Mid":
                    t_RoleElement.GetComponent<EditChampionDropdown>().Value = t_Team.Mid;
                    break;
                case "Jungle":
                    t_RoleElement.GetComponent<EditChampionDropdown>().Value = t_Team.Jungle;
                    break;
                case "Support":
                    t_RoleElement.GetComponent<EditChampionDropdown>().Value = t_Team.Support;
                    break;
                case "Marksman":
                    t_RoleElement.GetComponent<EditChampionDropdown>().Value = t_Team.Marksman;
                    break;
            }
        }
    }

    public void OnSetMain()
    {

    }
}
