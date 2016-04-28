using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Error : MonoBehaviour 
{
    static private Error m_Error = null;

	void Start ()
	{
        m_Error = this;    
	}
	
	public static void Show(string a_Message, string a_OKButtonText = "OK")
    {
        Debug.LogError(a_Message);
        m_Error.transform.Find("Content/Context").GetComponent<Text>().text = a_Message;
        m_Error.transform.Find("Content/Button/Text").GetComponent<Text>().text = a_OKButtonText;
        m_Error.transform.localScale = Vector3.one;
    }

    public void Close()
    {
        gameObject.transform.localScale = Vector3.zero;
        m_Error.transform.Find("Content/Context").GetComponent<Text>().text = "";
        m_Error.transform.Find("Content/Button/Text").GetComponent<Text>().text = "OK";
    }
}
