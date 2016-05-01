using UnityEngine;
using System.Collections;

public class BuyWindow : MonoBehaviour 
{
    public static GameObject Instance = null;
	void Start ()
	{
        Instance = gameObject;
        Instance.SetActive(false);
    }
	
	void Update () 
	{
	
	}
}
