using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController : MonoBehaviour
{
    //movement
    public float moveSpeed;
    public float turnSpeed = 90;
    public float lerpSpeed = 10; // smoothing speed
    public float gravity = 10;// gravity acceleration
    public float deltaGround = 0.2f; // character is grounded up to this distance
    public float jumpSpeed = 5; // vertical jump initial speed
    public float jumpRange = 2f; // range to detect target wall
    public bool isGrounded;
    public GameObject currentFloor;
    public GameObject previousFloor;
    private float ignore = 1;
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start() {
        myNormal = transform.up; // normal starts as character up direction 
        gameObject.GetComponent<Rigidbody>().freezeRotation = true; // disable physics rotation
        // distance from transform.position to ground
        distGround = GetComponent<Collider>().bounds.extents.y - GetComponent<Collider>().bounds.center.y;
        Ray colRay;
        RaycastHit colHit;
        colRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(colRay, out colHit)) {
            currentFloor = colHit.collider.gameObject; //set current flooras game starts
        }
    }

    private void OnCollisionEnter(Collision col) {
        if (col.collider.gameObject == currentFloor || col.collider.gameObject == previousFloor) { return; } // ignore when colliding with current floor
        else if (col.gameObject != currentFloor) {
            ContactPoint contact = col.contacts[0];
            surfaceNormal = contact.normal; //set surface normal to collision
            StartCoroutine(JumpToWall(contact.point, contact.normal)); //jump to wall
            Debug.Log("Switch");
        } else {
            currentFloor = col.gameObject;
        }
    }

    void FixedUpdate() {
        // apply constant weight force according to character normal:
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-gravity * rb.mass * myNormal);
        // jump code - jump to wall or simple jump
        if (jumping) { return; } // abort Update while jumping to a wall
        Ray jumpRay;
        RaycastHit jumpHit;
        jumpRay = new Ray(transform.position, -myNormal);
        if (Physics.Raycast(jumpRay, out jumpHit, jumpRange)) { //jump ray checks for the ground underneath player
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        if (Input.GetButtonDown("Jump")) { // jump pressed:
            if (isGrounded) { // no: if grounded, jump up
                rb.velocity += jumpSpeed * myNormal;
            }
        }
        Ray ray;
        RaycastHit hit;
        // update surface normal and isGrounded:
        ray = new Ray(transform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit)) { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0);
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        var myForward = Vector3.Cross(transform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        var targetRot = Quaternion.LookRotation(myForward, myNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
    }

    IEnumerator JumpToWall(Vector3 point, Vector3 normal) { // jump to wall 
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
        previousFloor = currentFloor; //set previous floor
        Ray colRay;
        RaycastHit colHit;
        colRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(colRay, out colHit, jumpRange)) { //shoot ray down
            currentFloor = colHit.collider.gameObject; // ray gets current floor for player
        }
        jumping = false; // jumping to wall finished
        StartCoroutine(WaitIgnore()); //start ignore
    }

    IEnumerator WaitIgnore() {
        yield return new WaitForSeconds(ignore);
        previousFloor = null; //after certain amount of seconds set previous floor to null
        yield return null;
    }
}