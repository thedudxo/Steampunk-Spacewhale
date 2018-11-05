using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHugging : MonoBehaviour {

    public GameObject player;
    public Transform cameraRotation;
    public Transform raycastPoint;

    public float mouseSensitivity;
    float xAxisClamp = 0;

    private float rotationAmountX;
    private float rotationAmountY;

    private float hoverheight = 1.0f;
    private float speed = 10.0f;

    private float terrainHeight;
    private RaycastHit hit;
    private Vector3 pos;
    private Vector3 forwardDirection;

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
        Debug.Log(currentZrotation);
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
        
        //Debug.Log(newRotation.eulerAngles);

        

        /*
        xAxisClamp -= rotationAmountY;
        Vector3 targetRotCam = cam.transform.rotation.eulerAngles;
        targetRotCam.x -= rotationAmountY;
        if (xAxisClamp > 90) {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90) {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }
        cam.transform.Rotate(-rotationAmountY, 0.0f, 0.0f);

        /*float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;
        
        Vector3 targetRotCam = cam.transform.rotation.eulerAngles;
        Vector3 targetRotBody = player.transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotBody.y += rotAmountX;

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

        transform.rotation = Quaternion.Euler(targetRotCam);
        player.transform.rotation = Quaternion.Euler(targetRotBody);*/


        //move player
       forwardDirection = player.transform.forward;
       transform.position += forwardDirection * Time.deltaTime * speed;
    }
}

