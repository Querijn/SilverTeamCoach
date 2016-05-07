using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class EditChampionDropdown : MonoBehaviour 
{
    public static List<EditChampionDropdown> m_UIElements = new List<EditChampionDropdown>();
    public Role m_Type;

    Dropdown Dropdown { get { return transform.GetComponentInChildren<Dropdown>(); }  }
    Toggle m_ShowAllChampionsToggle;

    bool m_ShowAllChampions = false;

    public void Start ()
	{
        m_ShowAllChampionsToggle = transform.parent.Find("ShowAllChampions").GetComponent<Toggle>();

        m_ShowAllChampions = !m_ShowAllChampionsToggle.isOn;

        m_UIElements.Add(this);

        Dropdown.onValueChanged.AddListener(delegate(int a_NewValue)
        {
            Reset();
        });
    }

    public static bool AllLanesFilled()
    {
        foreach (EditChampionDropdown t_Element in m_UIElements)
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
        
        foreach (EditChampionDropdown t_Element in m_UIElements)
        {
            if(t_Element.Value != null)
            {
                t_OwnedChampions.Remove(t_Element.Value);
            }
        }
        
        foreach (EditChampionDropdown t_Element in m_UIElements) 
            t_Element.SetChampionList(t_OwnedChampions);
    }

    void SetChampionList(List<Champion> a_Champions)
    {
        Champion t_SelectedChampion = Value;

        Dropdown.ClearOptions();
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
            Dropdown.AddOptions(t_Options);
            Dropdown.value = t_Options.IndexOf(t_Selected);
        }
        else Dropdown.AddOptions(t_Options);
    }


    public Champion Value
    {
        get
        {
            try
            {
                return Champion.Get(Dropdown.options[Dropdown.value].text);
            }
            catch
            {
                return null;
            }
        }
        set
        {
            try
            {
                var t_Value = Dropdown.options.IndexOf(Dropdown.options.Find(o => o.text == value.Name));
                if (t_Value >= 0 && t_Value < Dropdown.options.Count)
                    Dropdown.value = t_Value;
            }
            catch
            {

            }
        }
    }

    bool IsViableInSelectedLane(Champion.ViabilityInfo a_Viability)
    {
        switch (m_Type)
        {
            case Role.Top:
                return a_Viability.Top >= 0.5;
            case Role.Mid:
                return a_Viability.Mid >= 0.5;
            case Role.Jungle:
                return a_Viability.Jungle >= 0.5;
            case Role.Marksman:
                return a_Viability.Marksman >= 0.5;
            case Role.Support:
                return a_Viability.Support >= 0.5;
            default:
                return false;
        }
    }
}
