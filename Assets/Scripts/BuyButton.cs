using UnityEngine;
using System.Collections;

public class BuyButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void OnClick()
    {
        Champion.Get(gameObject.name);
        GameObject.FindGameObjectWithTag("BuyWindow").transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
