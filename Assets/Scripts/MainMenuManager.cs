using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour 
{
    bool PlayerName = false;

	void Setup ()
    {
        Text title = transform.Find("Title").GetComponent<Text>();
        title.text = "Hey there, " + Info.Player.Name + "!";
    }
	
	void Update () 
	{
        if (Info.Player != null && PlayerName == false)
        {
            PlayerName = true;
            Setup();
        }
	}
}
