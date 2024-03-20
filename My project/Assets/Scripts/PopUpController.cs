using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [Header("UI Elements for Popup")]
    public TextMeshProUGUI popupNameText;
    public TextMeshProUGUI popupPointsText;
    public TextMeshProUGUI popupAddressText;
    public GameObject popUpPanel;

    public void SetData(ClientData clientData)
    {
        popupNameText.text = "Name: "+clientData.name;
        popupPointsText.text = "Points: " + clientData.points.ToString();
        popupAddressText.text = "Address: " + clientData.address;
    }

    private void Start()
    {
        DisablePopUp();
    }

    public void EnablePopUp()
    {
        popUpPanel.SetActive(true);
    }

    public void DisablePopUp()
    {
        popUpPanel.SetActive(false);
    }
}
