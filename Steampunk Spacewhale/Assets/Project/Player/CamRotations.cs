﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotations : MonoBehaviour {

    public Transform player;
    public float turnSpeed = 5;

    float xAxisClamp = 0;

	void Start () {
		
	}
	
	void Update () {
        if (!Respawn.dead)
        {
            RotateCamera();
        }
    }

    void RotateCamera() {
        float mouseY = Input.GetAxis("Mouse Y");
        
        float rotAmountY = mouseY * turnSpeed;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = transform.localEulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotCam.y += 0;

        if (xAxisClamp > 90)
        {

            xAxisClamp = 90;
            targetRotCam.x = 90;

        }
        else if (xAxisClamp < -90)
        {

            xAxisClamp = -90;
            targetRotCam.x = 270;

        }

        transform.localRotation = Quaternion.Euler(targetRotCam);
    }
}
