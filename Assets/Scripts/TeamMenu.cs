using UnityEngine;
using System.Collections;

public class TeamMenu : MonoBehaviour 
{
    public void OnCreateTeam()
    {
        if (Info.Player.OwnedChampions.Length >= 5)
        {
            CreateTeamWindow.Instance.SetActive(true);
        }
        else
        {
            Error.Show("You cannot create a team until you have at least 5 champions!");
        }
    }
}
