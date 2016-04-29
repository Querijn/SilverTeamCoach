using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SortButton : MonoBehaviour
{
    public enum State { ascending, descending, none };
    State CurrentState = State.none;

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
        if (CurrentState == State.ascending)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price (DESC)";

            ShopManager Shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
            Shop.SetupShop(Champion.GetSortedBy(Champion.SortValue.Price, Champion.SortType.DESC));

            CurrentState = State.descending;
        }

        else if (CurrentState == State.descending)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price";

            ShopManager Shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
            Shop.SetupShop();

            CurrentState = State.none;
        }

        else if (CurrentState == State.none)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price (ASC)";

            ShopManager Shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
            Shop.SetupShop(Champion.GetSortedBy(Champion.SortValue.Price, Champion.SortType.ASC));

            CurrentState = State.ascending;
        }
    }



}