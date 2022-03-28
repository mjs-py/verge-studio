using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private Vector3 targetPosition;
    private string handState;
    private bool handEntered = false;

    void Update()
    {
        Debug.Log(handState);
        if (handEntered == true) 
        {   
            //string currentState = GetHandState();
            if (string.IsNullOrEmpty(handState))
            {
                // do nothing
            }
            else 
            {
                if (handState == "closed")
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2.5F);
                }
            }
        }
    }

    void OnTriggerEnter(UnityEngine.Collider collision) 
    {
        if (collision.gameObject.name == "HandLeft" || collision.gameObject.name == "HandRight") 
        {
            handEntered = true;
        }
    }

    // setter for hand position
    // retrieved from BodySourceView.cs
    public void UpdatePosition(Vector3 position)
    {
        targetPosition = position;
    }

    // set handState
    // retrieved and calculated from BodySourceView.cs
    public void UpdateHandState(string state)
    {
        handState = state;
    }
}
