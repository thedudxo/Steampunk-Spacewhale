using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController : MonoBehaviour {
    //movement
    public float moveSpeed;
    public float turnSpeed = 90;
    public float lerpSpeed = 10; // smoothing speed
    public float gravity = 10;// gravity acceleration
    public float deltaGround = 0.2f; // character is grounded up to this distance
    public float jumpSpeed = 5; // vertical jump initial speed
    public float jumpRange = 1.5f; // range to detect target wall
    public bool isGrounded;
    public GameObject currentFloor;
    private Rigidbody rb;
    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal
    private float distGround; // distance from character position to ground
    private bool jumping = false; // flag &quot;I'm jumping to wall&quot;
    private static PController instance;
    public static PController Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<PController>();
            }
            return PController.instance;
        }
    }
    private void Awake() {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Start() {
        myNormal = transform.up; // normal starts as character up direction 
        gameObject.GetComponent<Rigidbody>().freezeRotation = true; // disable physics rotation
        // distance from transform.position to ground
        distGround = GetComponent<Collider>().bounds.extents.y - GetComponent<Collider>().bounds.center.y;
    }
    
    private void OnCollisionEnter(Collision col) {
        /*Ray colRay;
        RaycastHit colHit;
        colRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(colRay, out colHit, jumpRange)) {
            currentFloor = colHit.collider.gameObject;
        } if(col.collider.gameObject == currentFloor) {   
            return;
        } else { */
            ContactPoint contact = col.contacts[0];
        if (col.gameObject != currentFloor)
        {
            StartCoroutine(JumpToWall(contact.point, contact.normal));
            Debug.Log("Switch");
        } else
        {
            currentFloor = col.gameObject;
        }
        //}
    }

    void FixedUpdate() {
        // apply constant weight force according to character normal:
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-gravity * rb.mass * myNormal);
        // jump code - jump to wall or simple jump
        if (jumping) { return; } // abort Update while jumping to a wall
        Ray ray;
        RaycastHit hit;
        if (Input.GetButtonDown("Jump")) { // jump pressed:
            /*ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, jumpRange)) { // wall ahead?
                StartCoroutine(JumpToWall(hit.point, hit.normal)); // yes: jump to the wall
            } else */if (isGrounded) { // no: if grounded, jump up
                rb.velocity += jumpSpeed * myNormal;
                isGrounded = false;
            }
        }

        // update surface normal and isGrounded:
        ray = new Ray(transform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit, jumpRange)) { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
            isGrounded = true;
        } else {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            //surfaceNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, transform.forward * jumpRange);
        Debug.DrawRay(transform.position, -transform.up * jumpRange, Color.red);
    }

    void Update() {
        transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0);
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        var myForward = Vector3.Cross(transform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        var targetRot = Quaternion.LookRotation(myForward, myNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        //Debug.Log(moveSpeed);
    }
    
    IEnumerator JumpToWall(Vector3 point, Vector3 normal) {
        // jump to wall 
        jumping = true; // signal it's jumping to wall
        rb.isKinematic = true; // disable physics while jumping
        var orgPos = transform.position;
        var orgRot = transform.rotation;
        var dstPos = point + normal * (distGround + 1); // will jump to 0.5 above wall
        var myForward = Vector3.Cross(transform.right, normal);
        var dstRot = Quaternion.LookRotation(myForward, normal);
        myNormal = normal; // update myNormal
        for (float t = 0.0f; t < 1.0;) {
            t += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(orgPos, dstPos, t);
            transform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return t;  // return here next frame
        }
        rb.isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished
    }
}