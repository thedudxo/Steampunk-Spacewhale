using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour {

    public float maxFallTime;
    static float timer = 0;
    static bool falling = false;
    public GameObject deadScreen;
    int collisions = 0;

	// Update is called once per frame
	void Update () {
        if (falling)
        {
            timer += Time.deltaTime;
        }
	}

    public static void reset()
    {
        falling = false;
        timer = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        if( timer >= maxFallTime)
        {
            Respawn.dead = true;
            deadScreen.SetActive(true);
        }
        falling = false;
        timer = 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
        if(collisions <= 0)
        {
            falling = true;
            collisions = 0;
        }
        
    }
}
