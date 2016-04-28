using UnityEngine;
using System.Collections;

public class CloseBuyWindow : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}

    public void OnClick()
    {
        GameObject.FindGameObjectWithTag("BuyWindow").transform.localScale = Vector3.zero;
    }

// Update is called once per frame
void Update ()
    {
	
	}
}
