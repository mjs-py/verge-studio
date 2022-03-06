using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLog : MonoBehaviour
{
    public int count = 0;
    // Collision
    void OnCollisionEnter(UnityEngine.Collision collision) 
    {
        Debug.Log(collision.gameObject.name);
    }
}
