using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatController : MonoBehaviour
{
    [Header("Water Layer")]
    public string waterLayer;

    public float displacementAmount = 3.0f;

    public Rigidbody rb;
    public float floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Wave height from singleton 
        // todo switch to GPU waves and get height of wave based on shader
        float waveHeight = waveManager.instance.GetWaveWaveHeight(transform.position.x);
           
        // Gravity for each floater object on boat
        rb.AddForceAtPosition(Physics.gravity/ floaterCount, transform.position, ForceMode.Acceleration);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / waveHeight) * displacementAmount;

            // Force to add at floaters
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);

            // Drag for rigidbody in water
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        
            
       
    }
}
