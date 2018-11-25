using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    
    private float jumpRange = 1.2f;
    private float jumpSpeed = 5;
    private Rigidbody rb;

	void Update () {
        rb = GetComponent<Rigidbody>();
        Ray jumpRay;
        RaycastHit jumpHit;
        jumpRay = new Ray(transform.position, -PController.Instance.myNormal);
        if (Physics.Raycast(jumpRay, out jumpHit, jumpRange)) { //jump ray checks for the ground underneath player
            PController.Instance.isGrounded = true;
        } else {
            PController.Instance.isGrounded = false;
        }
        if (Input.GetButtonDown("Jump"))
        { // jump pressed:
            if (PController.Instance.isGrounded)
            { // no: if grounded, jump up
                rb.velocity += jumpSpeed * PController.Instance.myNormal;
            }
        }
    }
}
