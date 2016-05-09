using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SettingsModifier : MonoBehaviour
{
    public enum TypeT { Sound, Music};
    public TypeT Type;
    public bool m_IgnoreChanges = false;
    public bool m_Changed = false;

    void Start ()
    {
        m_IgnoreChanges = true;
        m_Changed = false;
    }

    AudioSource t_Sound = null;
    public void OnSlideMusicVolume(Single Volume)
    {
        if (m_IgnoreChanges)
        {
            m_IgnoreChanges = false;
            return;
        }

        Settings.MusicVolume = Volume;

        if (t_Sound != null)
            Destroy(t_Sound.gameObject);

        t_Sound = Sound.Play(Resources.Load<AudioClip>("Sounds/Test"), a_Music:true);
        m_UpdateTimer = 0.33f;
    }

    public void OnSlideSEVolume (Single Volume)
    {
        if(m_IgnoreChanges)
        {
            m_IgnoreChanges = false;
            return;
        }

        Settings.SEVolume = Volume;

        if (t_Sound != null)
            Destroy(t_Sound.gameObject);

        t_Sound = Sound.Play(Resources.Load<AudioClip>("Sounds/Test"));
        m_UpdateTimer = 0.33f;
    }

    float m_UpdateTimer = 0.0f;
	void Update ()
    {
	    if(Champion.All.Length != 0 && m_Changed == false)
        {
            m_IgnoreChanges = true;
            m_Changed = true;
            GetComponent<Slider>().value = ((Type == TypeT.Sound) ? Settings.SEVolume : Settings.MusicVolume);
        }

        if(m_UpdateTimer > 0.0f)
        {
            m_UpdateTimer -= Time.deltaTime;
            
            if(m_UpdateTimer <= 0.0f)
            {
                HTTP.Request(Settings.FormAjaxURL("set_volume.php?sound="+ Settings.SEVolume + "&music=" + Settings.MusicVolume), delegate(WWW a_Request)
                {
                    if(a_Request.text != "true")
                    {
                        Error.Show("The following error appeared changing volumes: " + a_Request.text);
                    }
                }, false);
            }
        }
	}
}
