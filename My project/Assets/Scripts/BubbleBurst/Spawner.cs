using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spritePrefabs; // Assign your sprite prefab here in the inspector
    public float numSpawns = 15;
    public float minScale = 5f; // Minimum scale for the sprites
    public float maxScale = 6f; // Maximum scale for the sprites
    private Camera mainCamera;
    public Transform parent;

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < numSpawns; i++)
        {
            // Get random position within screen bounds
            Vector3 spawnPosition = GetRandomScreenPosition();

            // Create a random scale
            float randomScale = Random.Range(minScale, maxScale);

            int randomColourIndex = Random.Range(0, spritePrefabs.Length);

            // Instantiate the prefab with position and adjusted scale
            GameObject newSprite = Instantiate(spritePrefabs[randomColourIndex], spawnPosition, Quaternion.identity,parent);
            newSprite.transform.localScale = new Vector3(maxScale, maxScale, 1f); 
        }
        
    }

    Vector3 GetRandomScreenPosition()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPoint);
        worldPosition.z = 0f;
        return worldPosition;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Adjust collider check based on your sprite prefab's collider type (BoxCollider2D or CircleCollider2D)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position,2.5f); // Assuming a circle collider

        // Check for overlap with any existing colliders (excluding itself)
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                return false;
            }
        }

        return true;
    }

    

}
