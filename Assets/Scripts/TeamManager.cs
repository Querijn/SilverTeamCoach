﻿using UnityEngine;
using System.Collections;

public class TeamManager : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update () 
	{
	
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
