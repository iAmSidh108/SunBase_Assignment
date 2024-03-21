using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Data for Balloons")]
    [SerializeField] GameObject[] spritePrefabs; 
    [SerializeField] float numSpawns = 15;
    [SerializeField] Transform parent;
    [SerializeField] Button restartButton;

    [Header("Internal References")]
    private Camera mainCamera;

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartScene);
    }

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < numSpawns; i++)
        {
            // Get random position within screen bounds
            Vector3 spawnPosition = GetRandomScreenPosition();

            int randomColourIndex = Random.Range(0, spritePrefabs.Length);

            // Instantiate the prefab with position and adjusted scale
            GameObject newSprite = Instantiate(spritePrefabs[randomColourIndex], spawnPosition, Quaternion.identity,parent);
            newSprite.transform.localScale = new Vector3(2f, 2f, 1f); 
        }
        
    }

    //Returns a random position to spawn and converts it to worldPoint based on Camera
    Vector3 GetRandomScreenPosition()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPoint);
        worldPosition.z = 0f;
        return worldPosition;
    }


    //For restarting the scene, This will be added as listener to Button 
    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
