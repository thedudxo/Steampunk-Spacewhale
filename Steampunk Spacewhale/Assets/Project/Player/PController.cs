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
    public float jumpRange = 2f; // range to detect target wall
    public float jumpLimit = 5f;
    public bool isGrounded;
    public bool jumping = false; // flag "I'm jumping to wall"
    public bool nope = true;
    public Collider currentFloor;
    public Collider previousFloor;
    public Vector3 surfaceNormal; // current surface normal
    public Vector3 myNormal; // character normal
    private Rigidbody rb;
    private float ignore = 1f;
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
        Ray colRay;
        RaycastHit colHit;
        colRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(colRay, out colHit)) {
            currentFloor = colHit.collider; //set current flooras game starts
        }
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.GetComponent<Unclimable>() != null && !isGrounded) {
            moveSpeed = 0;
        }
        if (col.collider == currentFloor || col.collider == previousFloor || col.gameObject.GetComponent<Unclimable>() != null) { return; } // ignore when colliding with current floor
        else if (col.gameObject != currentFloor) {
            ContactPoint contact = col.contacts[0];
            surfaceNormal = contact.normal; //set surface normal to collision
            StartCoroutine(JumpToWall(contact.point, contact.normal)); //jump to wall
        }
    }

    void FixedUpdate() {
        Debug.DrawRay(transform.position, -myNormal * jumpLimit, Color.blue);
        if (Respawn.dead) { return; }
        // apply constant weight force according to character normal:
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-gravity * rb.mass * myNormal);
        transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed * 2.5f, 0);
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        var myForward = Vector3.Cross(transform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        var targetRot = Quaternion.LookRotation(myForward, myNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        if (jumping) { return; } // abort Update while jumping to a wall
        Ray groundRay;
        RaycastHit groundHit;
        groundRay = new Ray (transform.position, -myNormal);
        if(Physics.Raycast(groundRay, out groundHit, jumpLimit)) {
            currentFloor = groundHit.collider;
        }
        Ray ray;
        RaycastHit hit;
        // update surface normal and isGrounded:
        ray = new Ray(transform.position, -myNormal); // cast ray downwards
        if (nope)
        {
            if (Physics.Raycast(ray, out hit, jumpLimit) && hit.collider.gameObject.GetComponent<Unclimable>() == null)
            { // use it to update myNormal and isGrounded
                isGrounded = hit.distance <= deltaGround;
                surfaceNormal = hit.normal;
            }
            else
            {
                surfaceNormal = Vector3.up;
                jumping = true;
                StartCoroutine(Stop());
            }
        }
    }
    
    IEnumerator JumpToWall(Vector3 point, Vector3 normal) { // jump to wall 
        jumping = true; // signal it's jumping to wall
        rb.isKinematic = true; // disable physics while jumping
        previousFloor = currentFloor; //set previous floor
        var orgPos = transform.position;
        var orgRot = transform.rotation;
        var dstPos = point + normal; // will jump to 1 unit above wall
        var myForward = Vector3.Cross(transform.right, normal);
        var dstRot = Quaternion.LookRotation(myForward, normal);
        myNormal = normal; // update myNormal
        for (float t = 0.0f; t < 1.0;) {
            t += Time.deltaTime * 3;
            transform.position = Vector3.Lerp(orgPos, dstPos, t);
            transform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return t;  // return here next frame
        }
        rb.isKinematic = false; // enable physics
        Ray colRay;
        RaycastHit colHit;
        colRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(colRay, out colHit, jumpRange)) { //shoot ray down
            currentFloor = colHit.collider; // ray gets current floor for player
        }
        jumping = false; // jumping to wall finished
        StartCoroutine(WaitIgnore()); //start ignore
        CheckGround.Instance.GroundChecker();//Start void in CheckGround Script
    }
    /*
    void Footsteps() {
        audio.mute = true;
        audio.volume = Random.Range(0.8f, 1);
        audio.pitch = Random.Range(0.8f, 1.1f);
        audio.Play();
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
            audio.mute = false;
            Debug.Log("Stop");
        } else if (jumping && !isGrounded) {
            audio.mute = true;
        }
    }*/

    IEnumerator WaitIgnore() {
        yield return new WaitForSeconds(ignore);
        previousFloor = null; //after certain amount of seconds set previous floor to null
        yield return null;
    }

    IEnumerator Stop() {
        yield return new WaitForSeconds(0.5f);
        jumping = false;
    }
}