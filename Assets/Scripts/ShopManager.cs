using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    static bool Done = false;
    bool SpawnImages = false;

    public static void Reset()
    {
        Done = false;
    }

    public void SetupShop(Champion[] ChampionArray = null)
    {
        if(ChampionArray == null)
        {
            ChampionArray = Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC);
        }
        ChampionArray = Champion.Filter(Champion.FilterType.NotOwned, ChampionArray);
        // ChampionArray = Champion.Filter(Champion.FilterType.Buyable, ChampionArray); // Too confusing?

        GameObject Prefab = Resources.Load("Prefabs/ShopListObject") as GameObject;
        GameObject ShopContent = GameObject.FindGameObjectWithTag("ShopContent");

        int I = 0;
        int J = 0;

        foreach (Transform Child in ShopContent.transform)
            Destroy(Child.gameObject);

        foreach (Champion CurrentChampion in ChampionArray)
        {
            GameObject Instance = Instantiate(Prefab) as GameObject;
            Instance.transform.SetParent(ShopContent.transform);
            Instance.transform.localPosition = new Vector3((I * 250) + 40, (J * 300), 0);

            Instance.name = CurrentChampion.Name;
            Instance.transform.Find("Name").GetComponent<Text>().text = CurrentChampion.Name;
            Instance.transform.Find("Price").GetComponent<Text>().text = Cash.Format(CurrentChampion.Price);

            I += 1;
            if (I > 4)
            {
                J = J - 1;
                I = 0;
            }

            if (CurrentChampion.Price > Info.Player.Cash)
            {
                Instance.transform.Find("Price").GetComponent<Text>().color = Color.red;
            }
            
            if(CurrentChampion.Image != null)
            {
                Transform t_ImageObject = Instance.transform.Find("Image");
                if (t_ImageObject != null)
                    t_ImageObject.GetComponent<Image>().sprite = CurrentChampion.Image;
            }
            else
            {
                // Spawn them later
                SpawnImages = true;
            }
            Instance.transform.localScale = Vector3.one;
        }

        ShopContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ShopContent.GetComponent<RectTransform>().sizeDelta.x, (Mathf.Abs(J)+1) * 300);
    }

    void Update ()
    {
        if (Champion.All.Length != 0 && Done == false)
        {
            SetupShop();
            Done = true;
        }

        // If we are supposed to spawn images and Annie's image is available (indicator that possibly all are)
        if(SpawnImages && Champion.Get(1).Image != null)
        {
            // Don't keep doing this if unnecessary
            SpawnImages = false;

            // For each shop item
            foreach (Transform t_ChampionTransform in GameObject.FindGameObjectWithTag("ShopContent").transform)
            {
                Transform t_ImageObject = t_ChampionTransform.Find("Image");
                Sprite t_Sprite = Champion.Get(t_ChampionTransform.name).Image;

                // If this champion doesn't have an image yet, try again later
                if(t_Sprite == null)
                {
                    SpawnImages = true;
                    continue;
                }

                // Set image
                if (t_ImageObject != null)
                    t_ImageObject.GetComponent<Image>().sprite = t_Sprite;
            }
        }
	}

    public void OnBuy()
    {
        GameObject BuyWindow = GameObject.FindGameObjectWithTag("BuyWindow");

        string ChampionName = BuyWindow.transform.Find("Content/ChampionName").GetComponent<Text>().text;
        Champion BoughtChampion = Champion.Get(ChampionName);
        Info.Player.Buy(BoughtChampion);

        // Close window
        BuyWindow.transform.Find("Content/Cancel").GetComponent<CloseBuyWindow>().OnClick();
        Message.Create("You've bought a champion!", "Congratulations, " + Info.Player.Name + "! You've just strengthened your forces by buying " + BoughtChampion.Name + ". Have fun playing!", false);
    }

    public enum State { ascending, descending, AZ, ZA, none };
    State CurrentState = State.none;

    public void OnClickSortPrice()

    {
        if (CurrentState == State.AZ || CurrentState == State.ZA)
        {
            CurrentState = State.none;
            Text text = transform.Find("Sort by Name").GetComponentInChildren<Text>();
            text.text = "Sort by Name";
        }
        
        if (CurrentState == State.ascending)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price (DESC)";

            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Price, Champion.SortType.DESC));

            CurrentState = State.descending;
        }

        else if (CurrentState == State.descending)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price";

            this.SetupShop();

            CurrentState = State.none;
        }

        else if (CurrentState == State.none)
        {
            Text text = GetComponentInChildren<Text>();
            text.text = "Sort Price (ASC)";

            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Price, Champion.SortType.ASC));

            CurrentState = State.ascending;
        }
    }
    
    public void OnClickSortName()
    {
        if (CurrentState == State.ascending || CurrentState == State.descending)
        {
            CurrentState = State.none;
            Text text = transform.Find("SortByPrice").GetComponentInChildren<Text>();
            text.text = "Sort Price";
        }

        if (CurrentState == State.none || CurrentState == State.AZ)
        {
            Text textName = transform.Find("Sort by Name").GetComponentInChildren<Text>();
            textName.text = "Sort by Name (Z-A)";

            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.DESC));

            CurrentState = State.ZA;
        }

        else if (CurrentState == State.ZA)
        {
            Text textName = transform.Find("Sort by Name").GetComponentInChildren<Text>();
            textName.text = "Sort by Name (A-Z)";

            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));

            CurrentState = State.AZ;
        }

        else if (CurrentState == State.AZ)
        {
            Text textName = transform.Find("Sort by Name").GetComponentInChildren<Text>();
            textName.text = "Sort by Name";

            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));

            CurrentState = State.ZA;
        }
    }

    public void OnToggleOwned()
    {
        Champion[] OwnedChampions = Champion.Filter(Champion.FilterType.Owned);
        Toggle toggleowned = transform.Find("Owned").GetComponent<Toggle>();

        if (toggleowned.isOn == true)
        {
            SetupShop(OwnedChampions);
        }

        else if (toggleowned.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleNotOwned()
    {
        Champion[] NotOwnedChampions = Champion.Filter(Champion.FilterType.NotOwned);
        Toggle togglenotowned = transform.Find("Not Owned").GetComponent<Toggle>();

        if (togglenotowned.isOn == true)
        {
            SetupShop(NotOwnedChampions);
        }

        else if (togglenotowned.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleBuyable()
    {
        Champion[] BuyableChampions = Champion.Filter(Champion.FilterType.Buyable);
        Toggle togglebuyable = transform.Find("Buyable").GetComponent<Toggle>();

        if (togglebuyable.isOn == true)
        {
            SetupShop(BuyableChampions);
        }

        else if (togglebuyable.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleUnbuyable()
    {
        Champion[] UnbuyableChampions = Champion.Filter(Champion.FilterType.Unbuyable);
        Toggle toggleunbuyable = transform.Find("Unbuyable").GetComponent<Toggle>();

        if (toggleunbuyable.isOn == true)
        {
            SetupShop(UnbuyableChampions);
        }

        else if (toggleunbuyable.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleTop()
    {
        Champion[] TopChampions = Champion.Filter(Champion.FilterType.Top);
        Toggle toggletop = transform.Find("Top").GetComponent<Toggle>();

        if (toggletop.isOn == true)
        {
            SetupShop(TopChampions);
        }

        else if (toggletop.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleMid()
    {
        Champion[] MidChampions = Champion.Filter(Champion.FilterType.Mid);
        Toggle togglemid = transform.Find("Mid").GetComponent<Toggle>();

        if (togglemid.isOn == true)
        {
            SetupShop(MidChampions);
        }

        else if (togglemid.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleSupport()
    {
        Champion[] SupportChampions = Champion.Filter(Champion.FilterType.Support);
        Toggle togglesupport = transform.Find("Support").GetComponent<Toggle>();

        if (togglesupport.isOn == true)
        {
            SetupShop(SupportChampions);
        }

        else if (togglesupport.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleMarksman()
    {
        Champion[] MarksmanChampions = Champion.Filter(Champion.FilterType.Marksman);
        Toggle togglemarksman = transform.Find("Marksman").GetComponent<Toggle>();

        if (togglemarksman.isOn == true)
        {
            SetupShop(MarksmanChampions);
        }

        else if (togglemarksman.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }

    public void OnToggleJungle()
    {
        Champion[] JungleChampions = Champion.Filter(Champion.FilterType.Jungle);
        Toggle togglejungle = transform.Find("Jungle").GetComponent<Toggle>();

        if (togglejungle.isOn == true)
        {
            SetupShop(JungleChampions);
        }

        else if (togglejungle.isOn == false)
        {
            this.SetupShop(Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC));
        }
    }
}