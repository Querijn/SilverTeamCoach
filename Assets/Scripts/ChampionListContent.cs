using UnityEngine;
using System.Collections;

public class ChampionListContent : MonoBehaviour 
{
    bool m_Setup = false;
    	
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

                t_Instance.transform.localPosition = new Vector3(170, 500 + (-y * t_PrefabTransform.sizeDelta.y));
                t_Instance.transform.SetParent(transform);
                y++;
            }
            
            GetComponent<RectTransform>().sizeDelta = new Vector2(1200, (y * t_PrefabTransform.sizeDelta.y * 1.4f));
            m_Setup = true;
        }
	}
}
