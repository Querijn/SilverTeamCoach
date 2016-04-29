#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HideInEditor : MonoBehaviour 
{
    void Awake()
    {
        gameObject.SetActive(Application.isPlaying);
    }
	
	void Update () 
	{
	
	}
}
#endif
