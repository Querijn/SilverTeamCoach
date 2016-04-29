using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class LoadingWindow : MonoBehaviour 
{
    static private LoadingWindow m_Window = null;

    void Start()
    {
        Close();
    }

    public static void Show()
    {
        if (m_Window == null && GameObject.FindGameObjectWithTag("LoadingWindow") != null)
            m_Window = GameObject.FindGameObjectWithTag("LoadingWindow").GetComponent<LoadingWindow>();

        if (m_Window != null)
            m_Window.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Close();
    }
        
    public static void Close()
    {
        if (m_Window == null && GameObject.FindGameObjectWithTag("LoadingWindow") != null)
            m_Window = GameObject.FindGameObjectWithTag("LoadingWindow").GetComponent<LoadingWindow>();

        if (m_Window != null)
            m_Window.gameObject.SetActive(false);
    }
}
