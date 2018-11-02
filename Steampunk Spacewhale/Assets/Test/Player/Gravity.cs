using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    float gravity = -9.8f;
    Rigidbody rb;

    public Transform backLeft;
    public Transform backRight;
    public Transform frontLeft;
    public Transform frontRight;

    public RaycastHit lr;
    public RaycastHit rr;
    public RaycastHit lf;
    public RaycastHit rf;
    public Vector3 upDir;

	void Start () {

        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {

        //Gravity
        rb.AddForce(transform.up * gravity);

        //Clinging to terrain


	}
}
