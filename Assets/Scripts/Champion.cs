﻿using System.Collections.Generic;
using System;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Champion
{
    private static Dictionary<int, Champion> Champions = new Dictionary<int, Champion>();
    private static Dictionary<string, int> Translation = new Dictionary<string, int>();
    public static Champion[] All { get { return Champions.Values.ToArray(); } }

    public int ID { get; private set; }
    public Sprite Image { get; private set; }
    public string Name { get; private set; }
    public string Key { get; private set; }
    public bool Owned { get; private set; }
    public string Title { get; private set; }
    public double Price { get; private set; }
    public ViabilityInfo Viability { get; private set; }
    public MasteryInfo Mastery { get; private set; }

    public enum SortType
    {
        ASC,
        DESC
    };

    public enum SortValue
    {
        Name,
        Price,
        UserMastery,
        Effectiveness
    };

    public enum FilterType
    {
        Owned,
        NotOwned,
        Buyable,
        Unbuyable,
        Top,
        Mid,
        Support,
        Jungle,
        Marksman
    };

    public Champion(int a_ID, string a_Key, string a_Name, string a_Title, double a_Price, bool a_Owned, MasteryInfo a_MasteryInfo, ViabilityInfo a_Viability)
    {
        ID = a_ID;
        Name = a_Name;
        Key = a_Key;
        Title = a_Title;
        Price = a_Price;
        Owned = a_Owned;
        Mastery = a_MasteryInfo;
        Viability = a_Viability;

        if (Champions.ContainsKey(a_ID) == false)
        {
            Champions.Add(a_ID, this);
        }
    }


    public struct ViabilityInfo
    {
        public ViabilityInfo(double a_Meta, double a_Top, double a_Mid, double a_Jungle, double a_Marksman, double a_Support)
        {
            Meta = a_Meta;
            Top = a_Top;
            Mid = a_Mid;
            Jungle = a_Jungle;
            Marksman = a_Marksman;
            Support = a_Support;
        }

        public double Meta;
        public double Top;
        public double Mid;
        public double Jungle;
        public double Marksman;
        public double Support;
    };

    public struct MasteryInfo
    {
        public MasteryInfo(int a_Level, int a_Points, DateTime a_LastPlayed)
        {
            Level = a_Level;
            Points = a_Points;


            LastPlayed = a_LastPlayed;
        }

        public static MasteryInfo NoMastery
        {
            get
            {
                return new MasteryInfo(0, 0, DateTime.UtcNow);
            }
        }

        public int Level;
        public int Points;
        public DateTime LastPlayed;
    };

    static Dictionary<string, Texture2D> m_Textures = new Dictionary<string, Texture2D>();

    public static bool Reset(JSONArray a_Champions)
    {
        m_Setup = false;
        Champions.Clear();
        return Setup(a_Champions);
    }

    private static bool m_Setup = false;
    public static bool Setup(JSONArray a_Champions)
    {
        if (m_Setup)
            return true;

        if (a_Champions == null)
            return false;

        TextAsset ChampionData = Resources.Load<TextAsset>("Data/Champions");
        JSONArray JSONChampD = JSON.Parse(ChampionData.text).AsArray;
        Dictionary<string, ViabilityInfo> ViabilityInfos = new Dictionary<string, ViabilityInfo>();
        foreach (JSONNode JSONChamp in JSONChampD)
        {
            //voeg champion toe aan dictionary
            if (!ViabilityInfos.ContainsKey(JSONChamp["key"]))
            {
                ViabilityInfos.Add(JSONChamp["key"], new ViabilityInfo());
            }

            //if jungle, etc. show winrate
            ViabilityInfo Viability = ViabilityInfos[JSONChamp["key"]];
            string role = JSONChamp["role"].Value;

            if (role == "Top")
            {
                Viability.Top = JSONChamp["general"]["winPercent"].AsDouble / 100.0;
            }

            else if (role == "Jungle")
            {
                Viability.Jungle = JSONChamp["general"]["winPercent"].AsDouble / 100.0;
            }

            else if (role == "Middle")
            {
                Viability.Mid = JSONChamp["general"]["winPercent"].AsDouble / 100.0;
            }

            else if (role == "ADC")
            {
                Viability.Marksman = JSONChamp["general"]["winPercent"].AsDouble / 100.0;
            }

            else if (role == "Support")
            {
                Viability.Support = JSONChamp["general"]["winPercent"].AsDouble / 100.0;
            }

            Viability.Meta += JSONChamp["general"]["playPercent"].AsDouble / 100.0;

            ViabilityInfos[JSONChamp["key"]] = Viability;
        }
         
        foreach(JSONNode a_Champion in a_Champions)
        {
            var t_Time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(a_Champion["mastery"]["lastPlayTime"].AsInt);

            MasteryInfo t_Mastery;
            if (a_Champion["mastery"].Count > 0)
            {
                t_Mastery = new MasteryInfo
                (
                    a_Champion["mastery"]["championLevel"].AsInt,
                    a_Champion["mastery"]["championPoints"].AsInt,
                    t_Time
                );
            }
            else t_Mastery = MasteryInfo.NoMastery;

            //Debug.Log("You are level " + t_Mastery.Level + " with " + a_Champion["name"] + ".");

            Champion t_Champion = new Champion
            (
                a_Champion["id"].AsInt, // ID
                a_Champion["key"].Value, // Key name
                a_Champion["name"].Value, // Name
                a_Champion["title"].Value, // Title
                a_Champion["price"].AsDouble, // Price
                a_Champion["owned"].AsBool, // Owned
                t_Mastery,
                ViabilityInfos[a_Champion["key"]]
            );

            // Get image
            JSONNode t_ImageInfo = a_Champion["image"];
            Rect t_Rect = new Rect();

            t_Rect.x = t_ImageInfo["x"].AsFloat;
            t_Rect.y = t_ImageInfo["y"].AsFloat;
            t_Rect.width = t_ImageInfo["w"].AsFloat;
            t_Rect.height = t_ImageInfo["h"].AsFloat;

            // Check if we've loaded it before
            if (m_Textures.ContainsKey(t_ImageInfo["sprite"]) == false)
            {
                string ImageURL = Settings.ChampionImageDirectory + t_ImageInfo["sprite"];
                HTTP.Request(ImageURL, delegate (WWW a_Request)
                {
                    if (m_Textures.ContainsKey(t_ImageInfo["sprite"]) == false)
                        m_Textures.Add(t_ImageInfo["sprite"], a_Request.texture);

                    t_Rect.y = a_Request.texture.height - t_Rect.y - t_Rect.height;
                    t_Champion.Image = Sprite.Create(a_Request.texture, t_Rect, Vector2.zero);
                }, false);
            }
            else
            {
                Texture2D t_Texture = m_Textures[t_ImageInfo["sprite"]];

                t_Rect.y = t_Texture.height - t_Rect.y - t_Rect.height;
                t_Champion.Image = Sprite.Create(t_Texture, t_Rect, Vector2.zero);
            }

            if (Champions.ContainsKey(t_Champion.ID) == false)
                Champions.Add(t_Champion.ID, t_Champion);

            if (Translation.ContainsKey(t_Champion.Name) == false)
                Translation.Add(t_Champion.Name, t_Champion.ID);
        }

        //Debug.Log(Champions.Values.Count + " champions added.");
        m_Setup = true;
        return true;
    }

    public string GetBestLanes()
    {
        string t_BestLane1 = GetBestLane(0);
        string t_BestLane2 = GetBestLane(1);
        if (t_BestLane2 == "Unknown" || t_BestLane1 == "All")
            return t_BestLane1;

        return t_BestLane1 + ", " + t_BestLane2;
    }

    public string GetBestLane(int a_Number = 0)
    {
        Debug.Assert(a_Number == 0 || a_Number == 1);
        // TODO: support more than 1 lane

        string t_Ignore = (a_Number>0) ? GetBestLane(a_Number-1) : "";

        string t_Lane = "Unknown";
        double t_HighestViability = 0.0;

        if (Viability.Top == Viability.Mid &&
           Viability.Top == Viability.Jungle &&
           Viability.Top == Viability.Marksman &&
           Viability.Top == Viability.Support)
            return "All";

        if (t_HighestViability < Viability.Top && t_Ignore != "Top")
        {
            t_HighestViability = Viability.Top;
            t_Lane = "Top";
        }

        if (t_HighestViability < Viability.Mid && t_Ignore != "Mid")
        {
            t_HighestViability = Viability.Mid;
            t_Lane = "Mid";
        }

        if (t_HighestViability < Viability.Marksman && t_Ignore != "Marksman")
        {
            t_HighestViability = Viability.Marksman;
            t_Lane = "Marksman";
        }

        if (t_HighestViability < Viability.Support && t_Ignore != "Support")
        {
            t_HighestViability = Viability.Support;
            t_Lane = "Support";
        }

        if (t_HighestViability < Viability.Jungle && t_Ignore != "Jungle")
        {
            t_HighestViability = Viability.Jungle;
            t_Lane = "Jungle";
        }

        return t_Lane;
    }

    public static Champion Get(int a_ID)
    {
        if (Champions.ContainsKey(a_ID))
            return Champions[a_ID];
        else return null;
    }

    public static Champion Get(string a_Name)
    {   
        if (Translation.ContainsKey(a_Name))
            return Get(Translation[a_Name]);
        else return null;
    }
    
    public static Champion[] GetSortedBy(SortValue a_Value, SortType a_Type = SortType.DESC)
    {
        Champion[] t_Array = Champion.All;

        switch (a_Value)
        {
            case SortValue.Name:
                Array.Sort(t_Array, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Name.CompareTo(Champ2.Name) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
            case SortValue.Price:
                Array.Sort(t_Array, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Price.CompareTo(Champ2.Price) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
            case SortValue.UserMastery:
                Array.Sort(t_Array, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Mastery.Level.CompareTo(Champ2.Mastery.Level) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
        }

        return t_Array;
    }

    public static Champion[] Filter(FilterType a_Value, Champion[] a_ChampionList = null)
    {
        if (a_ChampionList == null)
            a_ChampionList = Champion.All;

        switch (a_Value)
        {
            case FilterType.Owned:
                return a_ChampionList.Where(c => c.Owned).ToArray();
            case FilterType.NotOwned:
                return a_ChampionList.Where(c => !c.Owned).ToArray();
            case FilterType.Buyable:
                return a_ChampionList.Where(c => c.Price <= Info.Player.Cash).ToArray();
            case FilterType.Unbuyable:
                return a_ChampionList.Where(c => c.Price > Info.Player.Cash).ToArray();
            case FilterType.Top:
                return a_ChampionList.Where(c => c.Viability.Top > 0.5).ToArray();
            case FilterType.Mid:
                return a_ChampionList.Where(c => c.Viability.Mid > 0.5).ToArray();
            case FilterType.Support:
                return a_ChampionList.Where(c => c.Viability.Support > 0.5).ToArray();
            case FilterType.Jungle:
                return a_ChampionList.Where(c => c.Viability.Support > 0.5).ToArray();
            case FilterType.Marksman:
                return a_ChampionList.Where(c => c.Viability.Support > 0.5).ToArray();
        }

        return a_ChampionList;
    }
}