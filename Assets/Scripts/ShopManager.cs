using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    bool Done = false;
    bool SpawnImages = false;
    public void SetupShop(Champion[] ChampionArray = null)
    {
        if(ChampionArray == null)
        {
            ChampionArray = Champion.GetSortedBy(Champion.SortValue.Name, Champion.SortType.ASC);
            ChampionArray = Champion.Filter(Champion.FilterType.NotOwned, ChampionArray);
            ChampionArray = Champion.Filter(Champion.FilterType.Buyable, ChampionArray);
        }

        GameObject Prefab = Resources.Load("Prefabs/ShopListObject") as GameObject;
        GameObject ShopContent = GameObject.FindGameObjectWithTag("ShopContent");

        int I = 0;
        int J = 0;

        foreach (Transform Child in ShopContent.transform)
            Destroy(Child.gameObject);

        foreach (Champion Champion in ChampionArray)
        {
            GameObject Instance = Instantiate(Prefab) as GameObject;
            Instance.transform.SetParent(ShopContent.transform);
            Instance.transform.localPosition = new Vector3((I * 250) + 2, (J * 300), 0);

            Instance.name = Champion.Name;
            Instance.transform.Find("Name").GetComponent<Text>().text = Champion.Name;
            Instance.transform.Find("Price").GetComponent<Text>().text = Cash.Format(Champion.Price);

            I += 1;
            if (I > 4)
            {
                J = J - 1;
                I = 0;
            }

            if (Champion.Price > Info.Player.Cash)
            {
                Instance.transform.Find("Price").GetComponent<Text>().color = Color.red;
            }
            
            if(Champion.Image != null)
            {
                Transform t_ImageObject = Instance.transform.Find("Image");
                if (t_ImageObject != null)
                    t_ImageObject.GetComponent<Image>().sprite = Champion.Image;
            }
            else
            {
                // Spawn them later
                SpawnImages = true;
            }
        }

        ShopContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ShopContent.GetComponent<RectTransform>().sizeDelta.x, (Mathf.Abs(J)+1) * 300);
    }

    void Update ()
    {
	    if(Champion.All.Length != 0 && Done == false)
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
        Info.Player.Buy(Champion.Get(ChampionName));

        // Close window
        BuyWindow.transform.Find("Content/Cancel").GetComponent<CloseBuyWindow>().OnClick();
    }
}

//buy screen corresponds with champion clicked