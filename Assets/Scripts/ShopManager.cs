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
            ChampionArray = Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC);
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

            Instance.name = Champion.Name;
            Instance.transform.Find("Name").GetComponent<Text>().text = Champion.Name;
            Instance.transform.Find("Price").GetComponent<Text>().text = Cash.Format(Champion.Price);

            I += 1;
            if (I > 4)
            {
                J = J - 1;
                I = 0;
            }
            if (Champion.Price > Info.Player.Cash)
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

//add OnClick Champion, open buy screen popup, closes buy screen popup
//buy screen corresponds with champion clicked