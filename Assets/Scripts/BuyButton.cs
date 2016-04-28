using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void OnClick()
    {
        GameObject BuyWindow = GameObject.FindGameObjectWithTag("BuyWindow");
        BuyWindow.transform.localScale = Vector3.one;

        Champion ClickedChampion = Champion.Get(gameObject.transform.parent.name);

        BuyWindow.transform.Find("Content/ChampionName").GetComponent<Text>().text = ClickedChampion.Name;

        BuyWindow.transform.Find("Content/Price").GetComponent<Text>().text = Cash.Format(ClickedChampion.Price);

        string SplashArt = "http://ddragon.leagueoflegends.com/cdn/img/champion/loading/"+ ClickedChampion.Key + "_0.jpg";

        HTTP.Request(SplashArt, delegate (WWW a_Request)
        {
            Rect Rectangle = new Rect();
            Rectangle.x = Rectangle.y = 0;
            Rectangle.width = a_Request.texture.width;
            Rectangle.height = a_Request.texture.height;

            BuyWindow.transform.Find("Content/SplashArt").GetComponent<Image>().sprite = Sprite.Create(a_Request.texture, Rectangle, Vector2.zero);
            BuyWindow.transform.Find("Content/SplashArt").GetComponent<Image>().color = Color.white;

        }, false);


    }

    // Update is called once per frame
    void Update () {
	
	}
}
