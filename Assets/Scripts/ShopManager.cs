using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    bool Done = false;
    public void SetupShop(Champion[] ChampionArray = null)
    {
        if(ChampionArray == null)
        {
            ChampionArray = Champion.GetSortedBy(SortValue.Name);
        }

        GameObject Prefab = Resources.Load("Prefabs/Champion") as GameObject;
        GameObject ShopContent = GameObject.FindGameObjectWithTag("ShopContent");

        int I = 0;
        int J = 0;
        
        foreach (Transform Child in ShopContent.transform)
            Destroy(Child.gameObject);

        foreach (Champion Champion in ChampionArray)
        {
            GameObject Instance = Instantiate(Prefab) as GameObject;
            Instance.transform.SetParent(ShopContent.transform);
            Instance.transform.localPosition = new Vector3((I * 250) + 2, (J * 300), 0);


            Instance.transform.Find("Name").GetComponent<Text>().text = Champion.Name;
            Instance.transform.Find("Price").GetComponent<Text>().text = Cash.Format(Champion.Price);

            //string Image = "http://ddragon.leagueoflegends.com/cdn/6.8.1/img/sprite/champion1.png";
            //break;
            //HTTP.Request(Image, delegate (WWW Request)
            //{
            //    Instance.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = Sprite.Create(Request.texture, new Rect(0, 0, Request.texture.width, Request.texture.height), Vector2.zero);
            //});
    
            I += 1;
            if (I>4)
            {
                J = J - 1;
                I = 0;
            }
            if(Champion.Price > Info.Player.Cash)
            {
                Instance.transform.Find("Price").GetComponent<Text>().color = Color.red;
            }
        }
    }
    
    void Update ()
    {
	    if(Champion.All.Length != 0 && Done == false)
        {
            SetupShop();
            Done = true;
        }
	}
}

//sorteren op laagste/hoogste prijs