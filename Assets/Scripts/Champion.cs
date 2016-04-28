using System.Collections.Generic;
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

    public static Champion[] GetSortedBy(SortValue a_Value, SortType a_Type = SortType.DESC)
    {
        Champion[] t_Array = Champion.All;

        switch(a_Value)
        {
            case SortValue.Name:
                Array.Sort(t_Array, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Name.CompareTo(Champ2.Name) * (a_Type == SortType.DESC ? 1 : -1);
                });
                break;
            case SortValue.Price:
                Array.Sort(t_Array, delegate (Champion Champ1, Champion Champ2)
                {
                    return Champ1.Price.CompareTo(Champ2.Price) * (a_Type == SortType.DESC ? 1 : -1);
                });
                break;
        }

        return t_Array;
    }

    public Champion(int a_ID, string a_Name, string a_Title, double a_Price, ViabilityInfo a_Viability)
    {
        ID = a_ID;
        Name = a_Name;
        Title = a_Title;
        Price = a_Price;

        if(Champions.ContainsKey(a_ID) == false)
        {
            Champions.Add(a_ID, this);
        }
    }

    public int ID { get; private set; }
    public Sprite Image { get; private set; }
    public string Name { get; private set; }
    public string Title { get; private set; }
    public double Price { get; private set; }
    public ViabilityInfo Viability;
    
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

    static Dictionary<string, Texture2D> m_Textures = new Dictionary<string, Texture2D>();

    private void UpdateShopImage()
    {
        GameObject t_ShopContent = GameObject.FindGameObjectWithTag("ShopContent");
        if (t_ShopContent == null)
            return;

        Transform t_Image = t_ShopContent.transform.Find(Name + "/Image");
        if (t_ShopContent == null)
            return;
        
        t_Image.GetComponent<Image>().sprite = Image;
    }
    

    private static bool m_Setup = false;
    public static bool Setup(JSONArray a_Champions)
    {
        if (m_Setup)
            return true;

        if (a_Champions == null)
            return false;
        
        foreach(JSONNode a_Champion in a_Champions)
        {
            Champion t_Champion = new Champion
            (
                a_Champion["id"].AsInt, // ID
                a_Champion["name"].Value, // Name
                a_Champion["title"].Value, // Title
                a_Champion["price"].AsDouble, // Price

                // TODO setup viability
                new ViabilityInfo(1.0, 1.0, 1.0, 1.0, 1.0, 1.0)
            );

            // Get image
            JSONNode t_ImageInfo = a_Champion["image"];
            Rect t_Rect = new Rect();

            t_Rect.x = t_ImageInfo["x"].AsFloat;
            t_Rect.y = t_ImageInfo["y"].AsFloat;
            t_Rect.width = t_ImageInfo["w"].AsFloat;
            t_Rect.height = t_ImageInfo["h"].AsFloat;


            if (m_Textures.ContainsKey(t_ImageInfo["sprite"]) == false)
            {
                string ImageURL = Settings.ChampionImageDirectory + t_ImageInfo["sprite"];
                HTTP.Request(ImageURL, delegate (WWW a_Request)
                {
                    if (m_Textures.ContainsKey(t_ImageInfo["sprite"]) == false)
                        m_Textures.Add(t_ImageInfo["sprite"], a_Request.texture);

                    t_Rect.y = a_Request.texture.height - t_Rect.y - t_Rect.height;
                    t_Champion.Image = Sprite.Create(a_Request.texture, t_Rect, Vector2.zero);
                    t_Champion.UpdateShopImage();
                }, false);
            }
            else
            {
                Texture2D t_Texture = m_Textures[t_ImageInfo["sprite"]];

                t_Rect.y = t_Texture.height - t_Rect.y - t_Rect.height;
                t_Champion.Image = Sprite.Create(t_Texture, t_Rect, Vector2.zero);
                t_Champion.UpdateShopImage();
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
}