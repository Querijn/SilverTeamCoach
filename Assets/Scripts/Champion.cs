using System.Collections.Generic;
using System;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public enum Role
{
    Top, Mid, Support, Jungle, Marksman
}

public enum Lane
{
    Top, Mid, Bot
}

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
    public int kill { get; private set; }
    public int death { get; private set; }
    public float winrate { get; private set; }
    public int CS { get; private set; }
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
        Efficiency
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

    public double GetViabilityByLane(Role a_Lane)
    {
        return Viability.GetViabilityByLane(a_Lane);
    }

    public double RelativeMastery
    {
        get
        {
            return Mastery.Points / Settings.PointsForFullEfficiency;
        }   
    }   

    private void EfficiencyModifier(ref double a_Efficiency)
    {
        double t_MetaModifier = 1.0 + ((double)Mathf.Clamp((float)Viability.Meta, 0.0f, 2.0f * Settings.MetaCoefficient) - Settings.MetaCoefficient);
        a_Efficiency *= t_MetaModifier;

        float t_WeeksUnplayed = (float)DateTime.Now.Subtract(Mastery.LastPlayed).TotalDays / 7.0f;
        float t_ClampedWeeks = Mathf.Clamp(t_WeeksUnplayed, 0.0f, (float)Settings.MaxWeekModifier);
        float t_LossModifier = (float)(a_Efficiency * Settings.EfficiencyLossPerWeek * t_ClampedWeeks);

        a_Efficiency -= (double)t_LossModifier;
    }

    public double Efficiency
    {
        get
        {
            double t_Efficiency = RelativeMastery;
            EfficiencyModifier(ref t_Efficiency);

            return (double)Mathf.Clamp01((float)t_Efficiency);
        }
    }

    public double RealEfficiency
    {
        get
        {
            double t_Efficiency = Mastery.Points;
            EfficiencyModifier(ref t_Efficiency);
            return t_Efficiency;
        }
    }

    public double GetLaneEfficiency(Role a_Lane)
    {
        double t_Efficiency = Efficiency;
        double t_Viability = GetViabilityByLane(a_Lane) - 0.5;
        double t_LaneModifier = 1.0 + ((double)Mathf.Clamp((float)t_Viability, -Settings.LaneCoefficient, Settings.LaneCoefficient));


        t_Efficiency *= t_LaneModifier;

        // Debug.Log(Name + "'s efficiency in " + a_Lane.ToString() + " is " + t_Efficiency);
        return (double)Mathf.Clamp01((float)t_Efficiency);
    }

    public double GetRealLaneEfficiency(Role a_Lane)
    {
        double t_Efficiency = Efficiency;
        double t_Viability = GetViabilityByLane(a_Lane) - 0.5;
        double t_LaneModifier = 1.0 + ((double)Mathf.Clamp((float)t_Viability, -Settings.LaneCoefficient, Settings.LaneCoefficient));


        t_Efficiency *= t_LaneModifier;

        //Debug.Log(Name + "'s efficiency in " + a_Lane.ToString() + " is " + t_Efficiency);
        return t_Efficiency;
    }

    public double GetBestLaneEfficiency(out Role a_Lane)
    {
        double t_HighestEfficiency = 0.0;
        a_Lane = Role.Top; // Just to get rid of the error

        foreach (Role t_Lane in Enum.GetValues(typeof(Role)))
        {
            double t_Efficiency = GetLaneEfficiency(t_Lane);
            if (t_HighestEfficiency < t_Efficiency)
            {
                t_HighestEfficiency = t_Efficiency;
                a_Lane = t_Lane;
            }
        }

        return t_HighestEfficiency;
    }

    public double GetWorstLaneEfficiency(out Role a_Lane)
    {
        double t_LowestEfficiency = 9e9;
        a_Lane = Role.Top; // Just to get rid of the error

        foreach (Role t_Lane in Enum.GetValues(typeof(Role)))
        {
            double t_Efficiency = GetLaneEfficiency(t_Lane);
            if (t_LowestEfficiency > t_Efficiency)
            {
                t_LowestEfficiency = t_Efficiency;
                a_Lane = t_Lane;
            }
        }

        return t_LowestEfficiency;
    }

    public double GetRelativeLaneEfficiency(Role a_Lane)
    {
        Role t_Lane; // To get rid of the out
        double t_Worst = GetWorstLaneEfficiency(out t_Lane);
        double t_Best = GetBestLaneEfficiency(out t_Lane);
        double t_Current = GetLaneEfficiency(a_Lane);

        return (t_Worst - t_Current) / (t_Worst - t_Best);

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

        public double GetViabilityByLane(Role a_Lane)
        {
            switch (a_Lane)
            {
                case Role.Top:
                    return Top;
                case Role.Mid:
                    return Mid;
                case Role.Marksman:
                    return Marksman;
                case Role.Support:
                    return Support;
                case Role.Jungle:
                    return Jungle;
                default:
                    return 0.0f;
            }
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

    public string GetBestLanes(string a_Seperator = ", ")
    {
        string t_BestLane1 = GetBestLane(0);
        string t_BestLane2 = GetBestLane(1);
        if (t_BestLane2 == "Unknown" || t_BestLane1 == "All")
            return t_BestLane1;

        return t_BestLane1 + a_Seperator + t_BestLane2;
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
    
    public static Champion[] GetSortedBy(SortValue a_Value, SortType a_Type = SortType.DESC, Champion[] a_ChampionList = null)
    {
        if (a_ChampionList == null)
            a_ChampionList = Champion.All;

        switch (a_Value)
        {
            case SortValue.Name:
                Array.Sort(a_ChampionList, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Name.CompareTo(Champ2.Name) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
            case SortValue.Price:
                Array.Sort(a_ChampionList, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Price.CompareTo(Champ2.Price) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
            case SortValue.UserMastery:
                Array.Sort(a_ChampionList, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Mastery.Points.CompareTo(Champ2.Mastery.Points) * (a_Type == SortType.ASC ? 1 : -1);
                });
                break;
            case SortValue.Efficiency:
                Array.Sort(a_ChampionList, (Champ1, Champ2) => Champ1.RealEfficiency.CompareTo(Champ2.RealEfficiency) * (a_Type == SortType.ASC ? 1 : -1));
                break;
        }

        return a_ChampionList;
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
                return a_ChampionList.Where(c => c.GetBestLanes().Contains("Top")).ToArray();
            case FilterType.Mid:
                return a_ChampionList.Where(c => c.GetBestLanes().Contains("Mid")).ToArray();
            case FilterType.Support:
                return a_ChampionList.Where(c => c.GetBestLanes().Contains("Support")).ToArray();
            case FilterType.Jungle:
                return a_ChampionList.Where(c => c.GetBestLanes().Contains("Jungle")).ToArray();
            case FilterType.Marksman:
                return a_ChampionList.Where(c => c.GetBestLanes().Contains("Marksman")).ToArray();
        }

        return a_ChampionList;
    }
}