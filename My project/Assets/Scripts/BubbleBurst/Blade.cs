using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [Header("Values related to Blade")]
    [SerializeField] float sliceForce = 5f;
    [SerializeField] float minSliceVelocity = 0.01f;

    [Header("Internal References")]
    private Camera mainCamera;
    private CircleCollider2D sliceCollider;
    private TrailRenderer sliceTrail;
    private List<GameObject> objectsToDestroy=new List<GameObject>();
    private Vector3 direction;

    [Header("Flags")]
    private bool slicing;

    private void Awake()
    {
        //Getting reference of required components
        mainCamera = Camera.main;
        sliceCollider = GetComponent<CircleCollider2D>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopMovement();
    }

    private void OnDisable()
    {
        StopMovement();
    }

    private void Update()
    {
        //Action based on Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            StartMovement();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (objectsToDestroy != null)
            {
                DestroyGameObjects();
            }

            StopMovement();
            
        }
        else if (slicing)
        {
            ContinueMovement();
        }
    }

    //This is called when we start moving our mouse
    private void StartMovement()
    {
        
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;

        slicing = true;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    //This is called when we stop our mouse 
    private void StopMovement()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    //THis is called when we have clicked left mouse button and are moving 
    private void ContinueMovement()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        sliceCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }


    /*Since we want our balloons to only get destroyed after the mouse has been lifted up,
    So we are adding the collided gameObjects in a separate List and then when we do mouseUp,we destroy these gameObjects*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided with a sprite
        if (collision.CompareTag("Balloon"))
        {
            objectsToDestroy.Add(collision.gameObject);
        }
    }

    //This we call on OnMouseButtonUp in Update.
    void DestroyGameObjects()
    {
        foreach(GameObject gb in objectsToDestroy)
        {
            Destroy(gb);
        }
    }
}
