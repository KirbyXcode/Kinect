using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float force = 100;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Body" || other.tag == "RightHand" || other.tag == "LeftHand" 
            || other.tag == "RightLeg" || other.tag == "LeftLeg" || other.tag == "Head")
        {
            if (transform.position.y < -0.6f)
            {
                transform.GetComponent<Rigidbody>().AddForce(0, force, 0);
            }
        }
    }
}
