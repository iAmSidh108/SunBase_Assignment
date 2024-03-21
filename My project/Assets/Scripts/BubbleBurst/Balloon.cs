using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Blade>())
        {
            Debug.Log("Collided");
            Destroy(this.gameObject);
        }
    }
}
