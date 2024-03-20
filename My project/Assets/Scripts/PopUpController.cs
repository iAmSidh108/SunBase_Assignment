using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [Header("UI Elements for Popup")]
    [SerializeField] private TextMeshProUGUI popupNameText;
    [SerializeField] private TextMeshProUGUI popupPointsText;
    [SerializeField] private TextMeshProUGUI popupAddressText;
    [SerializeField] private GameObject popUpPanel;

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

        // Animate scaling in
        popUpPanel.transform.DOScale(1f, 0.3f).From(0f).SetEase(Ease.OutBack); 
        
    }

    public void DisablePopUp()
    {
        // Animate scaling out and then deactivate
        popUpPanel.transform.DOScale(0f, 0.3f).From(1f).SetEase(Ease.OutBack).OnComplete(() => popUpPanel.SetActive(false)); 
    }
}
