using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SimpleJSON;

public class Team  
{
    public int ID = -1;
    public string Name = "";
    public Info.PlayerInfo Owner = null;

    public int Wins = 0;
    public int Losses = 0;
    public int Games { get { return Wins + Losses; } }
    public double WinRate { get { return (Games == 0) ? 0.0 : ((double)Wins / (double)Games); } }

    public int Kills = 0;
    public int Deaths = 0;
    public double KillDeathRate { get { return ((double)Kills) / (double)Deaths; } }

    public int CreepScore = 0;

    private int TopID = 0;
    private int MidID = 0;
    private int JungleID = 0;
    private int SupportID = 0;
    private int MarksmanID = 0;

    public Champion Top { get { return Champion.Get(TopID); } }
    public Champion Mid { get { return Champion.Get(MidID); } }
    public Champion Jungle { get { return Champion.Get(JungleID); } }
    public Champion Support { get { return Champion.Get(SupportID); } }
    public Champion Marksman { get { return Champion.Get(MarksmanID); } }

    static List<Team> Teams = new List<Team>();
    public static Team[] All
    {
        get
        {
            return Teams.ToArray();
        }
    }
    
    public static void Setup(JSONArray a_Teams)
    {
        Teams.Clear();
        
        foreach(JSONNode t_Node in a_Teams)
        {
            Team t_Team = new Team();
            t_Team.ID = t_Node["id"].AsInt;
            t_Team.Name = t_Node["name"].Value;

            t_Team.TopID = t_Node["top"].AsInt;
            t_Team.MidID = t_Node["mid"].AsInt;
            t_Team.JungleID = t_Node["jungle"].AsInt;
            t_Team.SupportID = t_Node["support"].AsInt;
            t_Team.MarksmanID = t_Node["marksman"].AsInt;

            t_Team.Wins = t_Node["wins"].AsInt;
            t_Team.Losses = t_Node["losses"].AsInt;
            t_Team.Kills = t_Node["kills"].AsInt;
            t_Team.Deaths = t_Node["deaths"].AsInt;
            t_Team.CreepScore = t_Node["creep_score"].AsInt;
            Teams.Add(t_Team);
        }
    }

    public static Team Get(int a_ID)
    {
        return Array.Find(All, t => t.ID == a_ID);
    }

    public static Team Get(string a_Name)
    {
        return Array.Find(All, t => t.Name == a_Name);
    }
}
