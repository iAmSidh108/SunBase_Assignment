using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientListItem : MonoBehaviour
{
    //List Items
    public TextMeshProUGUI clientLabel;
    public TextMeshProUGUI points;

    //PopUp Items
    public GameObject popupPrefab;
    public ClientData clientData;
    

    public JSOnDataController clientManager; // Reference to ClientManager script
    public PopUpController popUpController;
    void Awake()
    {
        clientManager = FindAnyObjectByType<JSOnDataController>(); // Find ClientManager script
        popUpController= FindObjectOfType<PopUpController>();
    }

    

    public void SetData(ClientData client)
    {
        this.clientData = client; // Store the data
        clientLabel.text = client.label;
        points.text=client.points.ToString();
    }

    public void OnListItemClick()
    {
        popUpController.SetData(clientData);
        popUpController.EnablePopUp();
        
    }
}
