using UnityEngine;
using System.Collections;
using SimpleJSON;

public static class Info 
{
    static bool m_Setup = false;
    static bool m_InProgress = false;

    public class PlayerInfo
    {
        public PlayerInfo(int a_ID, string a_Name, double a_Cash, string a_Team)
        {
            ID = a_ID;
            Name = a_Name;
            Cash = a_Cash;
            Team = a_Team;
        }

        public int ID { get; private set; }
        public string Name { get; private set; }
        public double Cash { get; private set; }
        public string Team { get; private set; }
        public Champion[] OwnedChampions
        {
            get
            {
                return Champion.Filter(Champion.FilterType.Owned);
            }
        }

        public bool Buy(Champion a_Champion)
        {
            if(a_Champion.Price > Cash)
                return false;
            
            HTTP.Request(Settings.FormAjaxURL("buy.php?champion="+a_Champion.ID), delegate(WWW a_Request)
            {
                if (a_Request.text == "true")
                {
                    Info.Reset();
                }
                else Error.Show(a_Request.text);
            }, true);

            return true;
        }
    };

    public static PlayerInfo Player { get; private set; }

    public static bool Reset()
    {
        HTTP.Request(Settings.FormAjaxURL("init.php"), delegate (WWW a_Request)
        {
            var t_JSON = JSON.Parse(a_Request.text);

            if (t_JSON["error"].Value != "")
            {
                Debug.LogError("'" + t_JSON["error"] + "'");
                return;
            }
            
            Player = new PlayerInfo(t_JSON["game_id"].AsInt, t_JSON["name"].Value, t_JSON["cash"].AsDouble, t_JSON["main_team"]["name"].Value);

            Settings.MusicVolume = t_JSON["music_volume"].AsFloat;
            Settings.SEVolume = t_JSON["sound_volume"].AsFloat;

            Champion.Reset(t_JSON["champions"].AsArray);
            Stats.Reset();
            ShopManager.Reset();
            ChampionListContent.Reset();
            TeamManager.Reset();
            Messages.Reset();
            Messages.Insert(t_JSON["messages"].AsArray);
            MessageHandler.Reset();
            //  Debug.Log("Reset complete, username is '" + Player.Name + "', and has " + Player.Cash + " cash.");
        }, true);
        return true;
    }

    public static bool Setup(bool a_Force = false)
	{
        if((m_Setup == true && a_Force != true) || m_InProgress == true)
            return m_Setup;

        HTTP.Request(Settings.FormAjaxURL("init.php"), delegate (WWW a_Request)
        {
            var t_JSON = JSON.Parse(a_Request.text);

            if (t_JSON["error"].Value != "")
            {
                Debug.LogError("'" + t_JSON["error"] + "'");
                switch(t_JSON["error"])
                {
                    case "NOT_LOGGED_IN":
                        Error.Show("You seem not to have logged in. How did you get here? Did you set up the game from source incorrectly?", "Not a clue dude. Lol.");
                        break;
                }
                return;
            }

            Settings.MusicVolume = t_JSON["music_volume"].AsFloat;
            Settings.SEVolume = t_JSON["sound_volume"].AsFloat;

            Champion.Setup(t_JSON["champions"].AsArray);


            Player = new PlayerInfo(t_JSON["game_id"].AsInt, t_JSON["name"].Value, t_JSON["cash"].AsDouble, t_JSON["main_team"]["name"].Value);
            Messages.Insert(t_JSON["messages"].AsArray);
            // Debug.Log("Initialisation complete, username is '" + Player.Name + "', and has " + Player.Cash + " cash.");
            m_Setup = true;
            m_InProgress = false;
        }, true);

        m_InProgress = true;
        return m_Setup;
    }
}
