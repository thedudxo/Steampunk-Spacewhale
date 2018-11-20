using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catwalk : MonoBehaviour {

    public GameObject catwalk;
    public float fallingSpeed;
    public float acceleration;
    private bool falling = false;
	
	void Update () {
        if (falling)
        {
           catwalk.transform.position = new Vector3(catwalk.transform.position.x, catwalk.transform.position.y - fallingSpeed, catwalk.transform.position.z);
            fallingSpeed += acceleration;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        falling = true;
    }
}
