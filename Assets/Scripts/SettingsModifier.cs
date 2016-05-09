using UnityEngine;
using System.Collections;
using System;

public class SettingsModifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    AudioSource t_Sound = null;
    public void OnSlideMusicVolume(Single Volume)
    {
        Settings.MusicVolume = Volume;

        if (t_Sound != null)
            Destroy(t_Sound.gameObject);

        t_Sound = Sound.Play(Resources.Load<AudioClip>("Sounds/Test"), a_Music:true);
    }

    public void OnSlideSEVolume (Single Volume)
    {
        Settings.SEVolume = Volume;

        if (t_Sound != null)
            Destroy(t_Sound.gameObject);

        t_Sound = Sound.Play(Resources.Load<AudioClip>("Sounds/Test"));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
