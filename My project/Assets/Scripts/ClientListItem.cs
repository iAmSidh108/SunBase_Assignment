using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientListItem : MonoBehaviour
{
    [Header("List Items")]
    [SerializeField] private TextMeshProUGUI clientLabel;
    [SerializeField] private TextMeshProUGUI points;

    [Header("PopUp Items")]
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private ClientData clientData;

    [Header("Script References")]
    private JSOnDataController dataController;
    private PopUpController popUpController;

    void Awake()
    {
        //Not the best way to get this but works as of now
        dataController = FindObjectOfType<JSOnDataController>();
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
