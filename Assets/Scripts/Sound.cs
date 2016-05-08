using UnityEngine;
using System.Collections;

public class Sound  
{
    public static void Play(AudioClip a_Clip, bool a_Looping = false, bool a_AdjustPitch = false, float a_Wait = 0.0f)
    {
        if (Game.Audio == null)
        {
            Debug.LogError("Cannot play sound! Game Audio missing!");
            return;
        }

        GameObject t_Object = new GameObject();
        t_Object.name = "SecretSoundObject";
        var t_Audio = t_Object.AddComponent<AudioSource>();
        t_Object.transform.parent = Camera.main.transform;
        t_Audio.clip = a_Clip;

        

        if(a_Looping == false)
            t_Audio.volume = Settings.SEVolume;

        else 
            t_Audio.volume = Settings.MusicVolume;

        if (a_AdjustPitch)
            t_Audio.pitch = UnityEngine.Random.Range(0.8f, 1.3f);

        t_Audio.PlayDelayed(a_Wait);
        UnityEngine.Object.Destroy(t_Object, a_Wait + a_Clip.length);
    }
}
