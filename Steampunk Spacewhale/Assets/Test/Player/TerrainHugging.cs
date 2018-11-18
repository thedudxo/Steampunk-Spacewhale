using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHugging : MonoBehaviour {

    public GameObject player;
    public Transform cameraRotation;
    public Rigidbody rb;

    public float mouseSensitivity;

    private float rotationAmountX;
    private float rotationAmountY;

    public float speed;

    private RaycastHit hit;
    private Vector3 pos;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private bool hugging = true;
    private Vector3 raycastPoint;
    private GameObject previousHug;
    private GameObject currentHug;
    private float hugTimer;
    private float maxHugTime = 0.1f;

    private Vector3[] directions;

    private void Start()
    {
    }

    public void PlayerCollided(Collision collision)
    {
        //Debug.Log("collision");

        //Aligns player with terrain
        
    }

    private void Update() {

        directions = new Vector3[6]
        {
            Vector3.up,
            -transform.up,
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        if (Input.GetKey(KeyCode.Space)) { hugging = !hugging; } //debug toggle
        if (hugging)
        {
            //Fire the ray
            raycastPoint = player.transform.position;
            Physics.Raycast(raycastPoint, directions[1], out hit);
            Debug.DrawRay(raycastPoint, directions[1]*5, Color.red, 10f);

            //Do we hug?
            if (hit.collider != null)
            {
                bool doHug = false;
                GameObject collison = hit.collider.gameObject;

                if (collison != previousHug)
                { doHug = true;  }
                else if (hugTimer > maxHugTime)
                { doHug = true; }

                if (doHug)
                {
                    transform.up -= (transform.up - hit.normal) * 0.1f;
                    if (currentHug != collison)
                    {
                        previousHug = currentHug;
                        currentHug = collison;
                    }
                    hugTimer = 0;
                }
            }
            
            

            for (int i = 0; i < directions.Length; i++) 
            {
                //Physics.Raycast(raycastPoint.position, directions[i], out hit);
               // Debug.DrawRay(raycastPoint.position, directions[i], Color.red);
               // transform.up -= (transform.up - hit.normal) * 0.1f;
            }

            hugTimer += Time.deltaTime;
        }

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(player.transform.position, fwd, 10))
        {
            Debug.Log("There is something in front of the object!");
        }

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
        transform.position += forwardDirection * Time.deltaTime * speed;
    }
}