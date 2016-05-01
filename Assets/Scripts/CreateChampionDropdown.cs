using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CreateChampionDropdown : MonoBehaviour 
{
    public static List<CreateChampionDropdown> m_UIElements = new List<CreateChampionDropdown>();
    public Lane m_Type;

    Dropdown m_Dropdown;
    Toggle m_ShowAllChampionsToggle;

    bool m_ShowAllChampions = false;

    void Start ()
	{
        m_Dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        m_ShowAllChampionsToggle = transform.parent.Find("ShowAllChampions").GetComponent<Toggle>();

        m_ShowAllChampions = !m_ShowAllChampionsToggle.isOn;

        m_UIElements.Add(this);

        m_Dropdown.onValueChanged.AddListener(delegate(int a_NewValue)
        {
            Reset();
        });
    }

    public static bool AllLanesFilled()
    {
        foreach (CreateChampionDropdown t_Element in m_UIElements)
            if (t_Element.Value == null)
                return false;
        return true;
    }
	
	void Update () 
	{
        if (Champion.All.Length != 0 && m_ShowAllChampionsToggle.isOn != m_ShowAllChampions)
        {
            m_ShowAllChampions = m_ShowAllChampionsToggle.isOn;
            Reset();
        }
    }

    public static void Reset()
    {
        List<Champion> t_OwnedChampions = new List<Champion>(Champion.Filter(Champion.FilterType.Owned, Champion.GetSortedBy(Champion.SortValue.Name)));
        
        foreach (CreateChampionDropdown t_Element in m_UIElements)
        {
            if(t_Element.Value != null)
            {
                t_OwnedChampions.Remove(t_Element.Value);
            }
        }
        
        foreach (CreateChampionDropdown t_Element in m_UIElements) 
            t_Element.SetChampionList(t_OwnedChampions);
    }

    void SetChampionList(List<Champion> a_Champions)
    {
        Champion t_SelectedChampion = Value;

        m_Dropdown.ClearOptions();
        List<Dropdown.OptionData> t_Options = new List<Dropdown.OptionData>();
        t_Options.Add(new Dropdown.OptionData(""));

        foreach (Champion t_Champion in a_Champions)
        {
            if (m_ShowAllChampions || IsViableInSelectedLane(t_Champion.Viability))
                t_Options.Add(new Dropdown.OptionData(t_Champion.Name, t_Champion.Image));
        }

        if(t_SelectedChampion != null)
        {
            var t_Selected = new Dropdown.OptionData(t_SelectedChampion.Name, t_SelectedChampion.Image);
            t_Options.Add(t_Selected);
            m_Dropdown.AddOptions(t_Options);
            m_Dropdown.value = t_Options.IndexOf(t_Selected);
        }
        else m_Dropdown.AddOptions(t_Options);
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

    bool IsViableInSelectedLane(Champion.ViabilityInfo a_Viability)
    {
        switch (m_Type)
        {
            case Lane.Top:
                return a_Viability.Top >= 0.5;
            case Lane.Mid:
                return a_Viability.Mid >= 0.5;
            case Lane.Jungle:
                return a_Viability.Jungle >= 0.5;
            case Lane.Marksman:
                return a_Viability.Marksman >= 0.5;
            case Lane.Support:
                return a_Viability.Support >= 0.5;
            default:
                return false;
        }
    }
}
