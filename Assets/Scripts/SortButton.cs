using UnityEngine;
using System.Collections;
using System;

public class SortButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}

	// Update is called once per frame
	void Update ()
    {

	}

   public void OnClick()
    {
        Champion[] ChampionArray = Champion.All;
        Array.Sort(ChampionArray, delegate (Champion Champ1, Champion Champ2)
        {
            return Champ1.Price.CompareTo(Champ2.Price);
        });

        ShopManager Shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
        Shop.SetupShop(ChampionArray);
        Debug.Log(ChampionArray[0].Price);
    }
    
}
