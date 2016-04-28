using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour 
{
    [Header("Balance")]
    public double m_CashPerChampionPoint = 0.1;
    public static double CashPerChampionPoint { get { return Singleton.m_CashPerChampionPoint; } }
    // How much cash you get for one champion point.
    // This is used to buy champions.

    public double m_PriceComparedToOriginal = 1.0;
    public static double PriceComparedToOriginal { get { return Singleton.m_PriceComparedToOriginal; } }
    // How much it costs to buy a champ compared to its RP equivalent.

    [Header("Generic")]
    // What appears in front of cash?
    public char m_CashSign = 'ƒ';
    public static char CashSign { get { return Singleton.m_CashSign; } }

    [Header("Network")]
    // Where is our network?
    // This is the base root where the ajax, api and sql folder is.
    public string m_Host = "http://localhost/";
    public static string Host { get { return Singleton.m_Host; } }

    // API folder
    public string m_APIFolder = "api/";
    public static string APIFolder { get { return Singleton.m_APIFolder; } }

    // AJAX folder
    public string m_AjaxFolder = "ajax/";
    public static string AjaxFolder { get { return Singleton.m_AjaxFolder; } }

    // Where do I find the champion images?
    public string m_ChampionImageDirectory = "http://ddragon.leagueoflegends.com/cdn/6.8.1/img/sprite/";
    public static string ChampionImageDirectory { get { return Singleton.m_ChampionImageDirectory; } }

    public static string FormAjaxURL(string a_API)
    {
        return Host + AjaxFolder + a_API;
    }

    public static Settings Singleton { get; private set; }

    void Start ()
	{
	    if(Singleton == null)
            Singleton = this;
	}
}
