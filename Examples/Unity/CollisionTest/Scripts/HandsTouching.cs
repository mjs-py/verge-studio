using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsTouching : MonoBehaviour
{
    private int count = 0;
    public float speed = 1.0F;

    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var main = ps.main;
        Debug.Log(speed);
        main.simulationSpeed = speed;
    }

    // Collision
    void OnTriggerEnter(UnityEngine.Collider collision) 
    {
        // check if hands is touching object
        // count the number of hands touching if 
        if (collision.gameObject.name == "HandLeft" || collision.gameObject.name == "HandRight") {
            if (count < 2) {
                count += 1; 
            }
            switch (count)
            {
                case 1:
                    Debug.Log("One hand touching");
                    speed = 0.1F;
                    break;
                case 2: 
                    Debug.Log("Two hands touching");
                    speed = 0.1F;
                    break;
            }
        }
    }

    void OnTriggerExit(UnityEngine.Collider collision)
    {
        if (collision.gameObject.name == "HandLeft" || collision.gameObject.name == "HandRight") {
            if (count > 0) {
                count -= 1; 
            }
            speed = 1.0F;
            Debug.Log(collision.gameObject.name + "Hand leaving object");
        }
    }
}
