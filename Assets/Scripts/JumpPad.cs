using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 vel = rb.velocity;
                vel.y = 0f;
                rb.velocity = vel;
            
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }    
        }
    }
}
