using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour {

    public static float rotateSpeed = 0.5f;
    private static bool rotating = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (rotating)
        {
            transform.Rotate(rotateSpeed, 0, 0);
            if(transform.rotation.x > 0)
            {
                rotating = false;
            }
        }
	}

    public static void Rotate()
    {
        rotating = true;
    }
}
