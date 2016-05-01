using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Settings : MonoBehaviour 
{
    [Header("Balance")]
    // How many points do you need for full efficiency
    public float m_PointsForFullEfficiency = 21600;
    public static float PointsForFullEfficiency { get { return Singleton.m_PointsForFullEfficiency; } }

    // How much does the play rate affect the efficiency?
    public float m_MetaCoefficient = 0.1f;
    public static float MetaCoefficient { get { return Singleton.m_MetaCoefficient; } }

    // How many weeks are considered in total
    public int m_MaxWeekModifier = 3;
    public static int MaxWeekModifier { get { return Singleton.m_MaxWeekModifier; } }

    // How much does the play rate affect the efficiency?
    public double m_EfficiencyLossPerWeek = 0.1f;
    public static double EfficiencyLossPerWeek { get { return Singleton.m_EfficiencyLossPerWeek; } }

    // How much does the win rate affect the efficiency?
    public float m_LaneCoefficient = 0.05f;
    public static float LaneCoefficient { get { return Singleton.m_LaneCoefficient; } }

    // How much cash you get for one champion point.
    // This is used to buy champions.
    public double m_CashPerChampionPoint = 0.1;
    public static double CashPerChampionPoint { get { return Singleton.m_CashPerChampionPoint; } }

    // How much it costs to buy a champ compared to its RP equivalent.
    public double m_PriceComparedToOriginal = 1.0;
    public static double PriceComparedToOriginal { get { return Singleton.m_PriceComparedToOriginal; } }

    [Header("Generic")]
    // What appears in front of cash?
    public char m_CashSign = 'ƒ';
    public static char CashSign { get { return Singleton.m_CashSign; } }

    // What menu pops up when you start the game?
    public MenuHandler.Menus m_DefaultMenu = MenuHandler.Menus.Main;
    public static MenuHandler.Menus DefaultMenu { get { return Singleton.m_DefaultMenu; } }

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
    
    public static Settings Singleton
    {
        get;
        private set;
    }

    static bool m_FirstTime = true;
    public static void OpenRequiredScenes()
    {
        List<int> t_InBuild = new List<int>();

        if (m_FirstTime == false)
            return;
        
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            t_InBuild.Add(SceneManager.GetSceneAt(i).buildIndex);
        }

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if(t_InBuild.Contains(i) == false)
            {
                SceneManager.LoadScene(i, LoadSceneMode.Additive);
            }
        }
        m_FirstTime = false;
    }

    void Start ()
	{
	    if(Singleton == null)
            Singleton = this;
        
        if(Application.isEditor == false)
        {
            OpenRequiredScenes();
        }

        Info.Setup();
    }
}
