using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Color m_DesiredColour = Color.white;
    bool m_Loaded = false;

    void Update()
    {
        if (m_Loaded == false)
        {
            HTTP.Request(Settings.WallpaperURL, delegate (WWW a_Request)
            {
                GetComponent<Image>().sprite = Sprite.Create(a_Request.texture, new Rect(0.0f, 0.0f, a_Request.texture.width, a_Request.texture.height), Vector2.zero);
                GetComponent<Image>().color = m_DesiredColour;
            }, false);
            m_Loaded = true;
        }
    }
}
