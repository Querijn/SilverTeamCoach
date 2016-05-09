using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour 
{
    static private Confirmation m_Confirmation = null;
    public delegate void Callback(bool a_Yes);

    private static Callback m_Callback = null;

    void Start()
    {
        m_Confirmation = this;
        Close();
    }

    public static void Show(string a_Title, string a_Message, Callback a_Call, string a_YesButton = "Yes", string a_NoButton = "No")
    {
        if (m_Confirmation == null)
            return;

        m_Confirmation.transform.Find("Content/Title").GetComponent<Text>().text = a_Title;
        m_Confirmation.transform.Find("Content/Context").GetComponent<Text>().text = a_Message;
        m_Confirmation.transform.Find("Content/Yes").GetComponentInChildren<Text>().text = a_YesButton;
        m_Confirmation.transform.Find("Content/No").GetComponentInChildren<Text>().text = a_NoButton;
        m_Callback = a_Call;
        m_Confirmation.gameObject.SetActive(true);
    }

    public void OnCallback(bool a_Yes)
    {
        Close();
        m_Callback(a_Yes);
    }

    public void Close()
    {
        m_Confirmation.gameObject.SetActive(false);
        m_Confirmation.transform.Find("Content/Title").GetComponent<Text>().text = "";
        m_Confirmation.transform.Find("Content/Context").GetComponent<Text>().text = "";
        m_Confirmation.transform.Find("Content/Yes").GetComponentInChildren<Text>().text = "";
        m_Confirmation.transform.Find("Content/No").GetComponentInChildren<Text>().text = "";
    }
}
