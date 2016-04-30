using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseBuyWindow : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}

    public void OnClick()
    {
        GameObject.FindGameObjectWithTag("BuyWindow").transform.localScale = Vector3.zero;
        GameObject.FindGameObjectWithTag("BuyWindow").transform.Find("Content/SplashArt").GetComponent<Image>().color = Color.clear;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
