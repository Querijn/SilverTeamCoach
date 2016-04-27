using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class Champion 
{
    private static Dictionary<int, Champion> Champions = new Dictionary<int, Champion>();
    public static Champion[] All { get { return Champions.Values.ToArray(); } }

    public Champion(int a_ID, string a_Name, string a_Title, double a_Price)
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
    public string Name { get; private set; }
    public string Title { get; private set; }
    public double Price { get; private set; }

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
                a_Champion["price"].AsDouble // Price
            );

            if(Champions.ContainsKey(t_Champion.ID) == false)
                Champions.Add(t_Champion.ID, t_Champion);
        }

        Debug.Log(Champions.Values.Count + " champions added.");
        m_Setup = true;
        return true;
    }
}