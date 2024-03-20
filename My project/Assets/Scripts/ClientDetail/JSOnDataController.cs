using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JSOnDataController : MonoBehaviour
{
    [Header("URL")]
    [SerializeField] private string url = "https://qa.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    [Header("Variables for Storing fetched Data")]
    private Dictionary<int, ClientData> clientDataById = new Dictionary<int, ClientData>();
    private List<ClientData> displayedClients = new List<ClientData>();

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown clientFilterDropdown;
    [SerializeField] private GameObject clientListPrefab;
    [SerializeField] private Transform clientListContainer;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button fetchButton;


    private void Awake()
    {
        //Listeners for button and dropdown.
        clientFilterDropdown.onValueChanged.AddListener(OnFilterChanged);
        fetchButton.onClick.AddListener(FetchData);

        //Display List at the beginning(Most probably empty)
        DisplayClients(displayedClients);
    }

    public void FetchData()
    {
        StartCoroutine(MakeRequest());
    }

    IEnumerator MakeRequest()
    {
        statusText.text = "Fetching Data!!!";

        //Clear list before fetching so that no double entries are recorded.
        displayedClients.Clear();

        //Sending JSON request
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result== UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            statusText.text= request.error;
        }
        else
        {
            statusText.text = "Data Fetched Successfully!!!";

            var jsonString = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            // Parse client labels(Details of Clients from Json response)
            var clientLabels = JsonConvert.DeserializeObject<List<ClientData>>(response["clients"].ToString());

            // Parse client data by ID(Details of Data from Json response)
            var clientData = JsonConvert.DeserializeObject<Dictionary<int, ClientData>>(response["data"].ToString());
            clientDataById = clientData;

            // Combine labels and data for displayed clients(Matching ID from Clients list and Data List)
            foreach (var clientLabel in clientLabels)
            {
                if (clientDataById.ContainsKey(clientLabel.id)) // Check if data exists for the ID
                {
                    displayedClients.Add(new ClientData { name = clientDataById[clientLabel.id].name, points = clientDataById[clientLabel.id].points, address = clientDataById[clientLabel.id].address, isManager = clientLabel.isManager, label=clientLabel.label });
                }
                else
                {
                    displayedClients.Add(new ClientData { name = "NA", points = 000, address = "NA", isManager = clientLabel.isManager, label = clientLabel.label });
                    //Debug.LogError("Client data not found for ID: " + clientLabel.id);
                }
            }

            DisplayClients(displayedClients);


        }
    }

    void DisplayClients(List<ClientData> displayedClients)
    {
        // Clear existing client list items
        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject); 
        }

        //Iterate through the list and set data for each client
        foreach (var client in displayedClients)
        {
            var clientItem = Instantiate(clientListPrefab, clientListContainer);
            clientItem.GetComponent<ClientListItem>().SetData(client);
            clientItem.transform.DOScale(1f, 1f).From(0f).SetEase(Ease.OutBack);
            
        }
    }

    void OnFilterChanged(int filterIndex)
    {
        //This list will filter clients based on IsManager boolean and then display only the filtered ones.
        List<ClientData> filteredClients;

        switch (filterIndex)
        {
            case 0: // All clients
                filteredClients = displayedClients;
                break;
            case 1: // Managers only
                filteredClients = displayedClients.Where(c => c.isManager).ToList();
                break;
            case 2: // Non-managers
                filteredClients = displayedClients.Where(c => !c.isManager).ToList();
                break;
            default:
                filteredClients = displayedClients;
                break;
        }

        DisplayClients(filteredClients);
    }

}
