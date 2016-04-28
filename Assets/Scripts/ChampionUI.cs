using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChampionUI : MonoBehaviour 
{
    public enum Type
    {
        Top, Mid, Jungle, Marksman, Support
    };

    public static List<ChampionUI> m_UIElements = new List<ChampionUI>();
    public Type m_Type;

    Dropdown m_Dropdown;
    Toggle m_ShowAllChampions;

    bool m_IsOn = false;

    void Start ()
	{
        m_Dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        m_ShowAllChampions = transform.parent.Find("ShowAllChampions").GetComponent<Toggle>();

        m_IsOn = !m_ShowAllChampions.isOn;

        m_UIElements.Add(this);

        m_Dropdown.onValueChanged.AddListener(delegate(int a_NewValue)
        {
            Reset();
        });
    }
	
	void Update () 
	{
        if (Champion.All.Length != 0 && m_ShowAllChampions.isOn != m_IsOn)
        {
            m_IsOn = m_ShowAllChampions.isOn;
            SetChampionList(m_ShowAllChampions.isOn);
        }
    }

    bool IsViableInSelectedLane(Champion.ViabilityInfo a_Viability)
    {
        switch(m_Type)
        {
            case Type.Top:
                return a_Viability.Top >= 0.5;
            case Type.Mid:
                return a_Viability.Mid >= 0.5;
            case Type.Jungle:
                return a_Viability.Jungle >= 0.5;
            case Type.Marksman:
                return a_Viability.Marksman >= 0.5;
            case Type.Support:
                return a_Viability.Support >= 0.5;
            default:
                return false;
        }
    }

    public Champion Value
    {
        get
        {
            try
            {
                return Champion.Get(m_Dropdown.options[m_Dropdown.value].text);
            }
            catch
            {
                return null;
            }
        }
    }

    public bool IsSelected(string a_Champion)
    {
        try
        {
            return m_Dropdown.options[m_Dropdown.value].text == a_Champion;
        }
        catch
        {
            return false;
        }
    }

    public bool IsInOtherList(string a_Champion)
    {
        foreach (ChampionUI t_UI in m_UIElements)
            if (t_UI.IsSelected(a_Champion))
                return true;

        return false;
    }

    public void Reset()
    {
        SetChampionList(m_IsOn);
    }

    void SetChampionList(bool a_ShowAllChampions)
    {
        List<string> t_Options = new List<string>();

        Champion[] t_OwnedChampions = Champion.Filter(Champion.FilterType.Owned, Champion.GetSortedBy(Champion.SortValue.Name));
        foreach (Champion t_Champion in t_OwnedChampions)
        {
            // No duplicate champion per team.
            if (IsInOtherList(t_Champion.Name))
                continue;

            if(a_ShowAllChampions || IsViableInSelectedLane(t_Champion.Viability))
                t_Options.Add(t_Champion.Name);
        }

        m_Dropdown.AddOptions(t_Options);
    }
}
