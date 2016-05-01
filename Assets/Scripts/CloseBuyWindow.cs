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
        BuyWindow.Instance.transform.Find("Content/SplashArt").GetComponent<Image>().color = Color.clear;
        BuyWindow.Instance.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
