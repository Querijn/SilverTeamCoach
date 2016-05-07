using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class TimelineEvent
{
    public int Time = 0;
    public EventType Type;
    public int Team = 0;

    public TeamState[] Teams = { new TeamState(0), new TeamState(1) };

    public TimelineEvent(int a_Time, JSONNode a_JSONEvent)
    {
        Time = a_Time;
        switch(a_JSONEvent["name"].Value)
        {
            case "connect":
                Type = EventType.Connect;
                break;
            case "tilt":
                Type = EventType.Tilt;
                break;
            case "invade":
                Type = EventType.Invade;
                break;
            case "empty_jungle":
                Type = EventType.EmptyJungle;
                break;
            case "executed":
                Type = EventType.Executed;
                break;
            case "tower_destroyed":
                Type = EventType.TowerDestroyed;
                break;
            case "useless_objective":
                Type = EventType.UselessObjective;
                break;
            case "inhibitor_destroyed":
                Type = EventType.InhibitorDestroyed;
                break;
            case "game_over":
                Type = EventType.GameOver;
                break;
            case "kill":
                Type = EventType.Kill;
                break;
            case "death":
                Type = EventType.Death;
                break;
            case "surrender":
                Type = EventType.Surrender;
                break;
            case "play":
                Type = EventType.Play;
                break;
            case "end_of_timeline":
                Type = EventType.EndOfTimeline;
                break;
            case "laning_phase":
                Type = EventType.LaningPhase;
                break;
            case "post_laning_phase":
                Type = EventType.PostLaningPhase;
                break;
            case "bot_tower_attack":
                Type = EventType.BotTowerAttack;
                break;
            case "mid_tower_attack":
                Type = EventType.MidTowerAttack;
                break;
            case "top_tower_attack":
                Type = EventType.TopTowerAttack;
                break;
            case "base_tower_attack":
                Type = EventType.BaseTowerAttack;
                break;
            case "bot_tower":
                Type = EventType.BotTowerDestroyed;
                break;
            case "mid_tower":
                Type = EventType.MidTowerDestroyed;
                break;
            case "top_tower":
                Type = EventType.TopTowerDestroyed;
                break;
            case "base_tower":
                Type = EventType.BaseTowerDestroyed;
                break;
            case "teamfight":
                Type = EventType.Teamfight;
                break;
            case "dragon":
                Type = EventType.Dragon;
                break;
            case "baron":
                Type = EventType.Baron;
                break;
        }

        for(int t_Team = 0; t_Team < 2; t_Team++)
        {
            foreach(Role t_Role in Enum.GetValues(typeof(Role)))
            {
                var t_JSONChampion = a_JSONEvent["state"][t_Team][t_Role.ToString().ToLower()];
                Teams[t_Team].Champions.Add(t_Role, new ChampionState(t_JSONChampion["death_timer"].AsFloat, t_JSONChampion["troll"].AsBool, t_JSONChampion["afk"].AsBool, t_JSONChampion["tilt"].AsBool));
            }
        }

    }

    public enum EventType
    {
        Connect,
        Tilt,
        Invade,
        EmptyJungle,
        Executed,
        TowerDestroyed,
        UselessObjective,
        InhibitorDestroyed,
        GameOver,
        Kill,
        Death,
        Surrender,
        Play,
        EndOfTimeline,
        LaningPhase,
        PostLaningPhase,
        BotTowerAttack,
        MidTowerAttack,
        TopTowerAttack,
        BaseTowerAttack,
        BotTowerDestroyed,
        MidTowerDestroyed,
        TopTowerDestroyed,
        BaseTowerDestroyed,
        Teamfight,
        Dragon,
        Baron,

    };

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
        public ChampionState(/*int a_Kills, int a_Deaths, int a_CS, */float a_DeathTimer, bool a_Trolling, bool a_AFK, bool a_Tilting)
        {
            //Kills = a_Kills;
            //Deaths = a_Deaths;
            //CS = a_CS;

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