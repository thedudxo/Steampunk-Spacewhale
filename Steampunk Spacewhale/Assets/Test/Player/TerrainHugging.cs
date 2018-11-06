using System.Collections;
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
    private float speed = 100.0f;

    private float terrainHeight;
    private RaycastHit hit;
    private Vector3 pos;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private bool hugging = true;

    private Vector3[] directions;

    private void Start()
    {
        directions = new Vector3[6]
        {
            transform.up,
            -transform.up,
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };
    }

    public void PlayerCollided(Collision collision)
    {
        Debug.Log("collision");

        //Aligns player with terrain
        if (hugging)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Physics.Raycast(raycastPoint.position, directions[i], out hit);
                Debug.DrawRay(raycastPoint.position, directions[i], Color.red);
                transform.up -= (transform.up - hit.normal) * 0.1f;
            }
        }
    }

    private void Update() {

        if (Input.GetKey(KeyCode.Space))
        {
            hugging = !hugging;
        }

        //Keeps player above ground
        pos = transform.position;
        //float terrainHeight = Terrain.activeTerrain.SampleHeight(pos);
        //transform.position = new Vector3(pos.x, terrainHeight + hoverheight, pos.z);

        

        //rotate camera with input

        rotationAmountX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationAmountX *= Time.deltaTime;
        player.transform.Rotate(0.0f, rotationAmountX, 0.0f);

        rotationAmountY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationAmountY *= Time.deltaTime;



        //Clamps the Camera (possibly in the wrong script)

        float rotateCameraX = -rotationAmountY;
        Vector3 currentRotation = cameraRotation.localEulerAngles;
        currentRotation.z += rotateCameraX;
        float currentZrotation = currentRotation.z;

        if (currentZrotation < 5)
            { currentZrotation = 5; }

        if (currentZrotation > 175)
            { currentZrotation = 175; }

        currentRotation.z = currentZrotation;
        Quaternion newRotation = Quaternion.Euler(currentRotation);
        cameraRotation.transform.localRotation = newRotation;
        

        //move player
        forwardDirection = player.transform.forward;
        //transform.position += forwardDirection * Time.deltaTime * speed;
        rb.AddForce(forwardDirection * Time.deltaTime * speed);

        transform.position -= transform.up * 0.05f;
    }
}