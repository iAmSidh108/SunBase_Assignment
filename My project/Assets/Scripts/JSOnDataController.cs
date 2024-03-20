using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JSOnDataController : MonoBehaviour
{
    private string url = "https://qa.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    private Dictionary<int, ClientData> clientDataById = new Dictionary<int, ClientData>();
    private List<ClientData> displayedClients = new List<ClientData>();
    public Dropdown clientFilterDropdown;
    public GameObject clientListPrefab;
    public Transform clientListContainer;

    public void Start()
    {
        StartCoroutine(MakeRequest());
    }

    IEnumerator MakeRequest()
    {

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result== UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            // Parse client labels
            var clientLabels = JsonConvert.DeserializeObject<List<ClientData>>(response["clients"].ToString());

            // Parse client data by ID
            var clientData = JsonConvert.DeserializeObject<Dictionary<int, ClientData>>(response["data"].ToString());
            clientDataById = clientData;

            // Combine labels and data for displayed clients
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
        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject); // Clear existing client list items
        }

        foreach (var client in displayedClients)
        {
            var clientItem = Instantiate(clientListPrefab, clientListContainer);
            clientItem.GetComponent<ClientListItem>().SetData(client);
            
        }
    }

    

}
