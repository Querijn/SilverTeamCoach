using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour {

    bool SetUp = false;
    GameObject t_UnreadMessages = null;

    // Use this for initialization
    void Start ()
    {
        t_UnreadMessages = GameObject.FindGameObjectWithTag("UnreadMessages");
    }
	
	// Update is called once per frame
	void Update ()
    {
        t_UnreadMessages.GetComponentInChildren<Text>().text = Messages.Unread.Length.ToString();
        t_UnreadMessages.SetActive(Messages.Unread.Length != 0);
        

        if (Messages.All.Length != 0 && SetUp == false)
        {
            GameObject MessageContent = GameObject.FindGameObjectWithTag("MessageContent");

            GameObject Prefab = Resources.Load("Prefabs/Message") as GameObject;

            int I = 0;
            Debug.Log(Messages.All.Length);

            foreach (Message NewMessage in Messages.All)
            {

                GameObject Instance = Instantiate(Prefab) as GameObject;
                Instance.transform.SetParent(MessageContent.transform);
                Instance.transform.localPosition = new Vector3(0, (I * 120), 0);

                Instance.name = NewMessage.Title;
                Instance.transform.Find("Message Title").GetComponent<Text>().text = NewMessage.Title;
                Instance.transform.Find("Message Content").GetComponent<Text>().text = NewMessage.Content;
                Instance.transform.Find("Message Time").GetComponent<Text>().text = NewMessage.Time.ToString();

                I += 1;

                Instance.transform.localScale = Vector3.one;
            }
            MessageContent.transform.localPosition = new Vector3(0, 0, 0);
            SetUp = true;
        } 
    }
}


//Message.Create("Titel", "Message", Belangrijk);
//De 3e is een bool of het een belangrijk bericht is
//Messages.All
//zijn alle berichten
//Messages.Unread
//ongelezen berichten
//- GameObject.FindWithTag("UnreadMessages").GetComponentInChildren<Text>().text = Messages.Unread;
//1) Laat zien hoeveel ongelezen berichten er zijn in dat cirkeltje X
//2) vul de lijst aan op de homepage met de messages(die in Messages.All staan) X
//Messages.All is een array met messages
//3) een knop om alle messages unread te maken
//    Messages.MarkAllUnread();
//4) op message klikken maakt ook unread