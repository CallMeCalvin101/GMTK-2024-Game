using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from Dave / GameDevelopment's Custom Bullet Script
// https://www.youtube.com/watch?v=0jGL5_DFIo8
public class HarpoonSticking : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;
    public LayerMask whatIsEnemies;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;

    // References
    private Vector3 refPos;
    private GameObject target;
    public bool sticked = false;

    int collisions;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (sticked) target.transform.position = transform.position;
    }

    public bool isSticked()
    {
        return sticked;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Adapted from RedDeCipher https://discussions.unity.com/t/how-do-i-make-projectile-weapons-stick-to-enemies-or-targets/201908/3
        if (collision.collider.CompareTag("Fish"))
        {
            ContactPoint contact = collision.contacts[0];
            target = collision.gameObject;
            
            if (target.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
                targetRb.detectCollisions = false;
            
            Vector3 refPos = contact.point - target.transform.position;
            sticked = true;
            rb.drag = 5;
        }

        //Count up collisions
        collisions++;
    }

    private void Setup()
    {

    }
}
