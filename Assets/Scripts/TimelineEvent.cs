using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimelineEvent
{
    public string Name = "";
    public TeamState[] Teams = { new TeamState(0), new TeamState(1) };
    public int Time = 0;

    public TimelineEvent(int a_Time)
    {
        Time = a_Time;
    }

    public class TeamState
    {
        public TeamState(int a_Team)
        {
            Index = a_Team;
        }

        public int Index;
        public Dictionary<Lane, List<TowerState>> Towers = new Dictionary<Lane, List<TowerState>>();
        public Dictionary<Role, ChampionState> Champions = new Dictionary<Role, ChampionState>();
    }

    public class ChampionState
    {
        public ChampionState(int a_Kills, int a_Deaths, int a_CS, float a_DeathTimer, bool a_Trolling, bool a_AFK, bool a_Tilting)
        {
            Kills = a_Kills;
            Deaths = a_Deaths;
            CS = a_CS;

            DeathTimer = a_DeathTimer;
            Trolling = a_Trolling;
            AFK = a_AFK;
            Tilting = a_Tilting;
        }

        public int Kills;
        public int Deaths;
        public int CS;

        public float DeathTimer;
        public bool Active;
        public bool Trolling;
        public bool Tilting;
        public bool AFK;

        public bool Dead { get { return DeathTimer > 0.0f; } }
    }

    public class TowerState
    {
        public TowerState(float a_Health)
        {
            Health = a_Health;
        }

        public float Health;
    }
}