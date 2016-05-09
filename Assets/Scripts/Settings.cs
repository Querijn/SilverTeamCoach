using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
    [Header("Actual Settings")]
    // music volume
    public static float music_Volume = 1.0f;
    public static float MusicVolume { get { return music_Volume; } set { music_Volume = value; } }

    // sound effects volume
    public static float SE_Volume = 1.0f;
    public static float SEVolume { get { return SE_Volume; } set { SE_Volume = value; } }

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
    private static string m_Host = /*"http://querijn.codes/silver/team/coach/";*/ "http://localhost/";
    public static string Host { get { return m_Host; } }

    // API folder
    private static string m_APIFolder = "api/";
    public static string APIFolder { get { return m_APIFolder; } }

    // AJAX folder
    private static string m_AjaxFolder = "ajax/";
    public static string AjaxFolder { get { return m_AjaxFolder; } }

    // wallpaper folder
    private static string m_WallpaperFolder = "wallpapers/";
    public static string WallpaperFolder { get { return m_WallpaperFolder; } }

    // Wallpaper count
    public int m_WallpaperCount = 8;
    public static int WallpaperCount { get { return Singleton.m_WallpaperCount; } }

    // Timeline fetching
    private static int m_TimelineFetchSize = 30;
    public static int TimelineFetchSize { get { return m_TimelineFetchSize; } }

    // How many match seconds pass in a real second?
    private static int m_GameSpeed = 30;
    public static int GameSpeed { get { return m_GameSpeed; } }

    // Where do I find the champion images?
    public string m_ChampionImageDirectory = "http://ddragon.leagueoflegends.com/cdn/6.8.1/img/sprite/";
    public static string ChampionImageDirectory { get { return Singleton.m_ChampionImageDirectory; } }

    // Where do I find the champion images?
    public string m_ChampionLoadingImageDirectory = "http://ddragon.leagueoflegends.com/cdn/img/champion/loading/";
    public static string ChampionLoadingImageDirectory { get { return Singleton.m_ChampionLoadingImageDirectory; } }

    public class PassThroughInfo
    {
        public WWW Request = null;
    }

    public static string WallpaperURL
    {
        get
        {
            return Host + WallpaperFolder + Random.Range(0, WallpaperCount).ToString() + ".jpg";
        }
    }

    public static string TimelineURL(int a_Time)
    {
        return Settings.FormAjaxURL("get_timeline.php?start=" + a_Time.ToString() + "&sec=" + TimelineFetchSize.ToString());
    }

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
        if (m_FirstTime == false)
            return;

        m_FirstTime = false;
    }

    public bool m_LoadEverything = true;

    void Start ()
	{
        m_FirstTime = true;
	    Singleton = this;

        if (m_LoadEverything == false)
        {
            Info.Setup(true);
            return;
        }

        //if(Application.isEditor == false)
        {
            OpenRequiredScenes();
        }

        Info.Setup(true);
    }

    void OnDestroy()
    {
        // Allow Game settings to take over
        Singleton = null;
    }
}
