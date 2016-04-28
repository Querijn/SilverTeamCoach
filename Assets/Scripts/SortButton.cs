using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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
        Text text = GetComponentInChildren<Text>();

        text.text = "Sort Price (ASC)";

        Champion[] ChampionArray = Champion.All;
        Array.Sort(ChampionArray, delegate (Champion Champ1, Champion Champ2)
        {
            return Champ1.Price.CompareTo(Champ2.Price);
        });

        //on second click state = false + champions descending price
        //Champion[] ChampionArray = Champion.All;
        //Array.Reverse(ChampionArray, delegate (Champion Champ1, Champion Champ2)
        //{
        //    return Champ1.Price.CompareTo(Champ2.Price);
        //});

        ShopManager Shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
        Shop.SetupShop(ChampionArray);
        Debug.Log(ChampionArray[0].Price);
    }
    
}

//een koopknop, de prijs, een 'efficiency' en een splash art