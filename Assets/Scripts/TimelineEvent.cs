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
    public GameChampion InvolvedChampion = null;

    public TeamState[] Teams = { new TeamState(0), new TeamState(1) };

    public TimelineEvent(int a_Time, JSONNode a_JSONEvent)
    {
        Time = a_Time;
        Team = a_JSONEvent["team"].AsInt;

        if (a_JSONEvent["role"] != null && a_JSONEvent["role"].Value != "")
        {
            try
            {
                String t_Role = a_JSONEvent["role"].Value.ToLower();
                t_Role = char.ToUpper(t_Role[0]) + t_Role.Substring(1);
                InvolvedChampion = Game.Teams[Team].GetChampion((Role)Enum.Parse(typeof(Role), t_Role));
            }
            catch { }
        }

        for (int t_Team = 0; t_Team < 2; t_Team++)
        {
            foreach (Role t_Role in Enum.GetValues(typeof(Role)))
            {
                var t_JSONChampion = a_JSONEvent["state"][t_Team][t_Role.ToString().ToLower()];
                Teams[t_Team].Champions.Add(t_Role, new ChampionState(t_JSONChampion["death_timer"].AsFloat, t_JSONChampion["troll"].AsBool, t_JSONChampion["afk"].AsBool, t_JSONChampion["tilt"].AsBool));
            }
        }

        switch (a_JSONEvent["name"].Value)
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

    }

    public IEnumerator PlayAfter(float a_Seconds)
    {
        Debugger.Log("Waiting " + a_Seconds + " for "+Type.ToString());
        yield return new WaitForSeconds(a_Seconds);

        Play();
    }

    static bool m_Connected = false;
    public void Play()
    {
        //Debugger.Log("Playing " + Type.ToString());

        for(int i = 0; i < 2; i++)
        {
            //    Game.Teams[i].Top.Kills = Teams[i].Champions[Role.Top].Kills;
            //    Game.Teams[i].Mid.Kills = Teams[i].Champions[Role.Mid].Kills;
            //    Game.Teams[i].Jungle.Kills = Teams[i].Champions[Role.Jungle].Kills;
            //    Game.Teams[i].Support.Kills = Teams[i].Champions[Role.Support].Kills;
            //    Game.Teams[i].Marksman.Kills = Teams[i].Champions[Role.Marksman].Kills;

            //    Game.Teams[i].Top.Deaths = Teams[i].Champions[Role.Top].Deaths;
            //    Game.Teams[i].Mid.Deaths = Teams[i].Champions[Role.Mid].Deaths;
            //    Game.Teams[i].Jungle.Deaths = Teams[i].Champions[Role.Jungle].Deaths;
            //    Game.Teams[i].Support.Deaths = Teams[i].Champions[Role.Support].Deaths;
            //    Game.Teams[i].Marksman.Deaths = Teams[i].Champions[Role.Marksman].Deaths;


            if (Teams[i].Champions[Role.Top].DeathTimer > 0)
                Game.Teams[i].Top.SetDeathTimer(Teams[i].Champions[Role.Top].DeathTimer);
            if (Teams[i].Champions[Role.Mid].DeathTimer > 0)
                Game.Teams[i].Mid.SetDeathTimer(Teams[i].Champions[Role.Mid].DeathTimer);
            if (Teams[i].Champions[Role.Jungle].DeathTimer > 0)
                Game.Teams[i].Jungle.SetDeathTimer(Teams[i].Champions[Role.Jungle].DeathTimer);
            if (Teams[i].Champions[Role.Support].DeathTimer > 0)
                Game.Teams[i].Support.SetDeathTimer(Teams[i].Champions[Role.Support].DeathTimer);
            if (Teams[i].Champions[Role.Marksman].DeathTimer > 0)
                Game.Teams[i].Marksman.SetDeathTimer(Teams[i].Champions[Role.Marksman].DeathTimer);

            //Game.Teams[i].Top.Tilting = Teams[i].Champions[Role.Top].Tilting;
            //Game.Teams[i].Mid.Tilting = Teams[i].Champions[Role.Mid].Tilting;
            //Game.Teams[i].Jungle.Tilting = Teams[i].Champions[Role.Jungle].Tilting;
            //Game.Teams[i].Support.Tilting = Teams[i].Champions[Role.Support].Tilting;
            //Game.Teams[i].Marksman.Tilting = Teams[i].Champions[Role.Marksman].Tilting;

            //Game.Teams[i].Top.Trolling = Teams[i].Champions[Role.Top].Trolling;
            //Game.Teams[i].Mid.Trolling = Teams[i].Champions[Role.Mid].Trolling;
            //Game.Teams[i].Jungle.Trolling = Teams[i].Champions[Role.Jungle].Trolling;
            //Game.Teams[i].Support.Trolling = Teams[i].Champions[Role.Support].Trolling;
            //Game.Teams[i].Marksman.Trolling = Teams[i].Champions[Role.Marksman].Trolling;

            //Game.Teams[i].Top.AFK = Teams[i].Champions[Role.Top].AFK;
            //Game.Teams[i].Mid.AFK = Teams[i].Champions[Role.Mid].AFK;
            //Game.Teams[i].Jungle.AFK = Teams[i].Champions[Role.Jungle].AFK;
            //Game.Teams[i].Support.AFK = Teams[i].Champions[Role.Support].AFK;
            //Game.Teams[i].Marksman.AFK = Teams[i].Champions[Role.Marksman].AFK;
        }

        switch (Type)
        {
            case EventType. Connect:
                {
                    if (Team == 1 || m_Connected)
                        return;

                    m_Connected = true;
                    // Play landing sound 5 times
                    var t_Sound = Game.GetSound(Type).Clip;
                    for (int i = 0; i < 5; i++)
                    {
                        Sound.Play(t_Sound, a_AdjustPitch:true, a_Wait:i * 0.4f);
                    }
                    break;
                }

            case EventType.Play:
                {
                    if (Team == 1)
                        return;
                    
                    //if(Game.GetSound(Type).Clip != null)
                    //    Sound.Play(Game.GetSound(Type).Clip);
                    break;
                }

            case EventType.Tilt:
                {
                    GameEventMessage.Spawn(InvolvedChampion.name + " is tilting!", (Team == 1) ? GameEventMessage.MessageType.Positive : GameEventMessage.MessageType.Negative, InvolvedChampion.Champion.Image);
                    break;
                }

            case EventType.Invade:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    if(Team == 0)
                        GameEventMessage.Spawn("Your team decided to invade..", GameEventMessage.MessageType.Positive);
                    else GameEventMessage.Spawn("Your team is getting invaded!", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.EmptyJungle:
                {                    
                    GameEventMessage.Spawn("Both invades failed.", GameEventMessage.MessageType.Positive);
                    break;
                }
            case EventType.Executed:
            	break;
            case EventType.TowerDestroyed:
            	break;
            case EventType.UselessObjective:
            	break;
            case EventType.InhibitorDestroyed:
            	break;
            case EventType.GameOver:
                {
                    Game.End();
                    break;
                }
            case EventType.Kill:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    InvolvedChampion.Kills++;
                    if (Team == 1)
                        GameEventMessage.Spawn(InvolvedChampion.Champion.Name + " has killed one of your teammembers!", GameEventMessage.MessageType.Negative, InvolvedChampion.Champion.Image);
                    else GameEventMessage.Spawn(InvolvedChampion.Champion.Name + " scored a kill!", GameEventMessage.MessageType.Positive, InvolvedChampion.Champion.Image);
                    break;
                }
            case EventType.Death:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    InvolvedChampion.Deaths++;
                    break;
                }
            case EventType.Surrender:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);
                    
                    GameEventMessage.Spawn(Game.Teams[Team].Name + " agreed to a surrender vote.", (Team == 1) ? GameEventMessage.MessageType.Positive : GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.EndOfTimeline:
            	break;
            case EventType.LaningPhase:
                {
                    if (Team == 1)
                        return;
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);
                    
                    GameEventMessage.Spawn("Laning phase has started.");
                    break;
                }
            case EventType.PostLaningPhase:
                {
                    if (Team == 1)
                        return;
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    GameEventMessage.Spawn("Laning phase has ended.");
                    break;
                }
            case EventType.BotTowerAttack:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(EventType.BaseTowerAttack).Clip);

                    if(Team != 1)
                        GameEventMessage.Spawn("Your team is attacking the tower at bottom lane.", GameEventMessage.MessageType.Positive);
                    else
                        GameEventMessage.Spawn("Your bottom tower is under attack.", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.MidTowerAttack:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(EventType.BaseTowerAttack).Clip);

                    if (Team != 1)
                        GameEventMessage.Spawn("Your team is attacking the tower at middle lane.", GameEventMessage.MessageType.Positive);
                    else
                        GameEventMessage.Spawn("Your middle tower is under attack.", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.TopTowerAttack:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(EventType.BaseTowerAttack).Clip);

                    if (Team != 1)
                        GameEventMessage.Spawn("Your team is attacking the tower at top lane.", GameEventMessage.MessageType.Positive);
                    else
                        GameEventMessage.Spawn("Your top tower is under attack.", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.BaseTowerAttack:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(EventType.BaseTowerAttack).Clip);

                    if (Team != 1)
                        GameEventMessage.Spawn("Your team is sieging their base!", GameEventMessage.MessageType.Positive);
                    else
                        GameEventMessage.Spawn("Your opponents are destroying your base!", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.BaseTowerDestroyed:
            case EventType.TopTowerDestroyed:
            case EventType.MidTowerDestroyed:
            case EventType.BotTowerDestroyed:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound((Team == 1) ? EventType.BaseTowerDestroyed : EventType.TopTowerDestroyed).Clip);

                    if (Team != 1)
                        GameEventMessage.Spawn("Your team has destroyed a turret.", GameEventMessage.MessageType.Positive);
                    else
                        GameEventMessage.Spawn("A turret has been destroyed.", GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.Teamfight:
                {
                    if (Team == 1)
                        return;
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    GameEventMessage.Spawn("A teamfight has started.", GameEventMessage.MessageType.Neutral);
                    break;
                }
            case EventType.Dragon:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    GameEventMessage.Spawn("A dragon has been slain.", (Team != 1) ? GameEventMessage.MessageType.Positive : GameEventMessage.MessageType.Negative);
                    break;
                }
            case EventType.Baron:
                {
                    if (Game.GetSound(Type).Clip != null)
                        Sound.Play(Game.GetSound(Type).Clip);

                    GameEventMessage.Spawn("Baron has been defeated.", (Team != 1) ? GameEventMessage.MessageType.Positive : GameEventMessage.MessageType.Negative);
                    break;
                }
                break;
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