using UnityEngine;
using System.Collections;
using System;

public class SettingsModifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


    public void OnSlideMusicVolume(Single Volume)
    {
        Settings.MusicVolume = Volume;
    }

    public void OnSlideSEVolume (Single Volume)
    {
        Settings.SEVolume = Volume;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
