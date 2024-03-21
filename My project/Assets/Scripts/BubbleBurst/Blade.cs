using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;

    private Camera mainCamera;
    private BoxCollider2D sliceCollider;
    private TrailRenderer sliceTrail;

    private Vector3 direction;

    private bool slicing;

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<BoxCollider2D>();
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
        if (Input.GetMouseButtonDown(0))
        {
            StartMovement();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopMovement();
        }
        else if (slicing)
        {
            ContinueMovement();
        }
    }

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

    private void StopMovement()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void ContinueMovement()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        sliceCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }
}
