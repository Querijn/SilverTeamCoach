using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

//[ExecuteInEditMode]
public class EditTeamWindow : MonoBehaviour
{
    public static GameObject Instance = null;
    public static Team EditingTeam = null;

    public void OnEdit()
    {
        if (CreateChampionDropdown.AllLanesFilled() == false)
        {
            Error.Show("You need to fill all the lanes!");
            return;
        }

        Dictionary<string, string> t_Commands = new Dictionary<string, string>();

        t_Commands.Add("id", EditingTeam.ID.ToString());

        // Don't allow name changes atm
        // t_Commands.Add("name", transform.Find("Content/Name").GetComponentInChildren<InputField>().text);

        Transform t_Roles = Instance.transform.Find("Content/Champions");
        foreach (Transform t_RoleElement in t_Roles)
        {
            if (t_RoleElement.name == "ShowAllChampions")
                continue;

            t_Commands.Add(t_RoleElement.name.ToLower(), t_RoleElement.GetComponent<EditChampionDropdown>().Value.ID.ToString());
        }

        string t_CommandString = "edit_team.php?";

        foreach (var t_Command in t_Commands)
        {
            t_CommandString += t_Command.Key + "=" + Uri.EscapeDataString(t_Command.Value) + "&";
        }
        t_CommandString = t_CommandString.Substring(0, t_CommandString.Length - 1);

        Debugger.Log(t_CommandString);
        HTTP.Request(Settings.FormAjaxURL(t_CommandString), delegate (WWW a_Request)
        {
            if (a_Request.text == "true")
            {
                TeamManager.Reset();
                Info.Reset(); // Info needs to be reset as well because of the possibility of getting a new main team
                OnCancel();
            }
            else Error.Show(a_Request.text);
        }, true);
        EditingTeam = null;
    }

    public void OnDelete()
    {
        Confirmation.Show("Delete Team", "Are you sure you want to delete '" + EditingTeam.Name + "'?", delegate (bool a_Delete)
         {
             if(a_Delete)
             {
                 HTTP.Request(Settings.FormAjaxURL("delete_team.php?id=" + EditingTeam.ID), delegate (WWW a_Request)
                 {
                     if (a_Request.text == "true")
                     {
                         TeamManager.Reset();
                         OnCancel();
                     }
                     else Error.Show(a_Request.text);
                 }, true);
             }

             EditingTeam = null;
         });

    }

    public void Awake()
    {
        Instance = gameObject;
        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
        EditingTeam = null;
    }
}
