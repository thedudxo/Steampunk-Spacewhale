using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    float gravity = -9.8f;
    Rigidbody rb;

    private float hoverHeight = 1.0f;
    private float terrainHeight;
    private Vector3 pos;

    private RaycastHit hit;
    public Transform raycastPoint;

	void Start () {

        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {

        //Gravity
        rb.AddForce(transform.up * gravity);

        //Clinging to terrain
        //Keeps player above ground
        pos = transform.position;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(pos);
        transform.position = new Vector3(pos.x, terrainHeight + hoverHeight, pos.z);
        //Align player with terrain
        Debug.DrawRay(raycastPoint.position, Vector3.down, Color.red);
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        transform.up -= (transform.up - hit.normal) * 0.1f;
	}
}
