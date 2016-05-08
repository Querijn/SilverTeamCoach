using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

//[ExecuteInEditMode]
public class CreateTeamWindow : MonoBehaviour
{
    public static GameObject Instance = null;

    public void OnCreate()
    {
        if (CreateChampionDropdown.AllLanesFilled() == false)
        {
            Error.Show("You need to fill all the lanes!");
            return;
        }

        Dictionary<string, string> t_Commands = new Dictionary<string, string>();

        t_Commands.Add("name", transform.Find("Content/Name").GetComponentInChildren<InputField>().text);

        Transform t_Roles = transform.Find("Content/Champions");
        foreach(Transform t_RoleElement in t_Roles)
        {
            if (t_RoleElement.name == "ShowAllChampions")
                continue;
            
            t_Commands.Add(t_RoleElement.name.ToLower(), t_RoleElement.GetComponent<CreateChampionDropdown>().Value.ID.ToString());
        }

        string t_CommandString = "create_team.php?";

        foreach(var t_Command in t_Commands)
        {
            t_CommandString += t_Command.Key + "=" + Uri.EscapeDataString(t_Command.Value) + "&";
        }
        t_CommandString = t_CommandString.Substring(0, t_CommandString.Length - 1);

        //Debug.Log(t_CommandString);
        HTTP.Request(Settings.FormAjaxURL(t_CommandString), delegate (WWW a_Request)
        {
            if (a_Request.text == "true")
            {
                Info.Reset();
                OnCancel();
            }
            else Error.Show(a_Request.text);
        }, true);
    }

    public void Awake()
    {
        Instance = gameObject;
        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
