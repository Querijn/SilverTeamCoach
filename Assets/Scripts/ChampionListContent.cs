using UnityEngine;
using System.Collections;

public class ChampionListContent : MonoBehaviour 
{
    static bool m_Setup = false;
    static ChampionListContent m_Element = null;
    static public void Reset()
    {
        foreach (Transform t_Child in m_Element.transform)
            Destroy(t_Child.gameObject);
        m_Setup = false;
    }

    void Start()
    {
        m_Element = this;
        GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }
    	
	void Update () 
	{
	    if(m_Setup == false && Champion.All.Length != 0)
        {
            GameObject t_Prefab = Resources.Load("Prefabs/ChampionListObject") as GameObject;
            RectTransform t_PrefabTransform = t_Prefab.GetComponent<RectTransform>();
            
            int y = 0;
            foreach(Champion t_Champion in Champion.Filter(Champion.FilterType.Owned, Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC)))
            {
                GameObject t_Instance = Instantiate(t_Prefab);
                t_Instance.name = t_Champion.Name;
                t_Instance.GetComponent<ChampionListElement>().FillInfo(t_Champion);
                t_Instance.transform.SetParent(transform);

                t_Instance.transform.localPosition = new Vector3(0, (-y * t_PrefabTransform.sizeDelta.y));
                t_Instance.transform.localScale = Vector3.one;
                y++;
            }
            
            GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40 + (y * t_PrefabTransform.sizeDelta.y));
            m_Setup = true;
        }
	}
}
