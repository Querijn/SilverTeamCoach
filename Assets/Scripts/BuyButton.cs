using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuyButton : MonoBehaviour
{
    void Start ()
    {
    }

    public void OnClick()
    {
        BuyWindow.Instance.SetActive(true);

        Champion ClickedChampion = Champion.Get(gameObject.transform.parent.name);

        BuyWindow.Instance.transform.Find("Content/ChampionName").GetComponent<Text>().text = ClickedChampion.Name;
        BuyWindow.Instance.transform.Find("Content/Role").GetComponent<Text>().text = ClickedChampion.GetBestLanes();

        BuyWindow.Instance.transform.Find("Content/Price").GetComponent<Text>().text = Cash.Format(ClickedChampion.Price);

        foreach(Lane t_Lane in Enum.GetValues(typeof(Lane)))
        {
            Transform t_ProgressBar = BuyWindow.Instance.transform.Find("Content/" + t_Lane.ToString());

            t_ProgressBar.Find("Role").GetComponent<Text>().text = t_Lane.ToString();

            float t_Efficiency = (float)ClickedChampion.GetLaneEfficiency(t_Lane);
            // Debug.Log(t_Efficiency);

            t_ProgressBar.Find("Fill").GetComponent<RectTransform>().sizeDelta = new Vector2(t_Efficiency * 595, t_ProgressBar.Find("Fill").GetComponent<RectTransform>().sizeDelta.y);
            t_ProgressBar.Find("Percentage").GetComponent<Text>().text = (t_Efficiency*100.0).ToString("F2") + "%";

            string InWords = "Not Recommended";
            if(t_Efficiency > 0.33 && t_Efficiency < 0.5)
                InWords = "Mediocre";
            else if (t_Efficiency >= 0.5 && t_Efficiency < 0.7)
                InWords = "Above average";
            else if (t_Efficiency >= 0.7 && t_Efficiency < 0.95)
                InWords = "Recommended";
            else if (t_Efficiency >= 0.95)
                InWords = "Perfect";


            t_ProgressBar.Find("InWords").GetComponent<Text>().text = InWords;
        }

        string SplashArt = "http://ddragon.leagueoflegends.com/cdn/img/champion/loading/"+ ClickedChampion.Key + "_0.jpg";

        HTTP.Request(SplashArt, delegate (WWW a_Request)
        {
            Rect Rectangle = new Rect();
            Rectangle.x = Rectangle.y = 0;
            Rectangle.width = a_Request.texture.width;
            Rectangle.height = a_Request.texture.height;

            BuyWindow.Instance.transform.Find("Content/SplashArt").GetComponent<Image>().sprite = Sprite.Create(a_Request.texture, Rectangle, Vector2.zero);
            BuyWindow.Instance.transform.Find("Content/SplashArt").GetComponent<Image>().color = Color.white;

        }, false);


    }

    // Update is called once per frame
    void Update () {
	
	}
}
