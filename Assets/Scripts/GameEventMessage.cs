using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameEventMessage : MonoBehaviour 
{
    static int m_Messages = 0;
    static GameObject m_Prefab = null;

    public enum MessageType
    {
        Positive,
        Negative,
        Neutral
    };

    [SerializeField]
    protected Color m_Positive = Color.green;
    [SerializeField]
    protected Color m_Negative = Color.red;
    [SerializeField]
    protected Color m_Neutral = Color.gray;

    public static void Spawn(string a_Message, MessageType a_Type = MessageType.Neutral, Sprite a_Image = null)
    {
        if (m_Prefab == null) m_Prefab = Resources.Load<GameObject>("Prefabs/GameEventMessage");
       
        GameObject t_Instance = Instantiate(m_Prefab);

        t_Instance.transform.SetParent(GameObject.Find("Canvas").transform);
        Text t_Text = t_Instance.transform.Find("Text").GetComponent<Text>();
        t_Text.text = a_Message;
        t_Instance.transform.localScale = Vector3.one;
        t_Instance.transform.localPosition = Vector3.zero - new Vector3(0.0f, GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y * 0.5f - 50.0f) + new Vector3(0.0f, m_Messages * 120.0f);

        var t_Image = t_Instance.transform.Find("Image").GetComponent<Image>();
        if (a_Image != null)
        {
            t_Image.sprite = a_Image;
        }
        else
        {
            var t_Colour = t_Image.color;
            t_Colour.a = 0.0f;
            t_Image.color = t_Colour;

            var t_RectTransform = t_Text.GetComponent<RectTransform>();
            t_RectTransform.offsetMin = new Vector2(t_RectTransform.offsetMax.x * -1 , t_RectTransform.offsetMin.y);
        }
        
        switch (a_Type)
        {
            case MessageType.Positive:
                t_Instance.GetComponent<Image>().color = m_Prefab.GetComponent<GameEventMessage>().m_Positive;
                break;
            case MessageType.Negative:
                t_Instance.GetComponent<Image>().color = m_Prefab.GetComponent<GameEventMessage>().m_Negative;
                break;
            case MessageType.Neutral:
                t_Instance.GetComponent<Image>().color = m_Prefab.GetComponent<GameEventMessage>().m_Neutral;
                break;

        }

        m_Messages++;
    }

    void OnDestroy()
    {
        m_Messages--;
    }

    void Start()
    {
    }

    float m_FadeWait = 3.0f;
    void Update()
    {
        m_FadeWait -= Time.deltaTime;
        if (m_FadeWait >= 0.0f)
            return;

        if(m_FadeWait < -1.0f)
            Destroy(gameObject, 1.0f);

        if (GetComponent<Image>() != null)
            Fade(GetComponent<Image>());

        foreach (Transform t_Child in transform)
        {
            if (t_Child.GetComponent<Text>() != null)
                Fade(t_Child.GetComponent<Text>());

            else if (t_Child.GetComponent<Image>() != null)
                Fade(t_Child.GetComponent<Image>());
        }
    }

    void Fade(Image a_Image)
    {
        Color t_Colour = a_Image.color;

        t_Colour.a -= 0.5f * Time.deltaTime;
        a_Image.color = t_Colour;
    }

    void Fade(Text a_Text)
    {
        Color t_Colour = a_Text.color;

        t_Colour.a -= 0.5f * Time.deltaTime;
        a_Text.color = t_Colour;
    }
}
