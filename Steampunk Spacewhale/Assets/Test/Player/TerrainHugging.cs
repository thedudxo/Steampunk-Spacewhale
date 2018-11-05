﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHugging : MonoBehaviour {

    public GameObject player;
    public Transform cameraRotation;
    public Transform raycastPoint;
    public Rigidbody rb;

    public float mouseSensitivity;
    float xAxisClamp = 0;

    private float rotationAmountX;
    private float rotationAmountY;

    private float hoverheight = 1.0f;
    private float speed = 500f;
    private float gravity = -10f;

    private float terrainHeight;
    private RaycastHit hit;
    private Vector3 pos;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {


        //Keeps player above ground
        pos = transform.position;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(pos);
        transform.position = new Vector3(pos.x, terrainHeight + hoverheight, pos.z);

        //Aligns player with terrain
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        transform.up -= (transform.up - hit.normal) * 0.1f;
        //rotate camera with input

        rotationAmountX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationAmountX *= Time.deltaTime;
        player.transform.Rotate(0.0f, rotationAmountX, 0.0f);

        rotationAmountY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationAmountY *= Time.deltaTime;




        float rotateCameraX = -rotationAmountY;

        Vector3 currentRotation = cameraRotation.localEulerAngles;
        currentRotation.z += rotateCameraX;
        float currentZrotation = currentRotation.z;
        //Debug.Log(currentZrotation);
        if (currentZrotation < 5)
        {
            currentZrotation = 5;
        }

        if (currentZrotation > 175)
        {
            currentZrotation = 175;
        }
        currentRotation.z = currentZrotation;
        Quaternion newRotation = Quaternion.Euler(currentRotation);
        cameraRotation.transform.localRotation = newRotation;

        //move player
        float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;

        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        rb.AddForce(transform.forward * translation, ForceMode.Force);
    }
}