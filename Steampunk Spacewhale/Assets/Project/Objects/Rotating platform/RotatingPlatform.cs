using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour {

    public static float rotateSpeed = 0.3f;
    private static bool rotating = false;
    private static bool rotated = false;

    public AudioSource var;
    public static AudioSource rotate;
    public AudioSource stopRotate;

	void Start () {
        rotate = var;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rotating) {
            transform.Rotate(rotateSpeed, 0, 0);
            if(transform.rotation.x > 0) {
                rotate.Stop();
                stopRotate.Play();
                rotating = false;
                rotated = true;
            }
        }
	}

    public static void Rotate() {
        if (rotated) { return; }
        rotating = true;
        rotate.Play();
    }
}