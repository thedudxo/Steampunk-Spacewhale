using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour {

    public static float rotateSpeed = 0.5f;
    private static bool rotating = false;
    private static bool rotated = false;

    public AudioSource var;
    public static AudioSource rotate;
    public AudioSource stopRotate;

	void Start () {
        rotate = var;
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

	void FixedUpdate () {
        if (rotating)
        {
                rotated = true;
    public static void Rotate()
    {
        if (rotated) { return; }
        rotating = true;