using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour {

    public GameObject deadScreen;
    Rigidbody rb;
    public float maxFallSpeed = -20;
    static float lastFallSpeed;


    private void Start() { rb = GetComponent<Rigidbody>(); }

    private void FixedUpdate()
    {
        lastFallSpeed = rb.velocity.y;
    }

    public static void Reset()
    {
        lastFallSpeed = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(lastFallSpeed < maxFallSpeed)
        {
            Respawn.dead = true;
            deadScreen.SetActive(true);
        }
    }
}
