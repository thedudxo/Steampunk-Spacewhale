using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour {
    
    private float standHeight = 1;
    private float crouchHeight = 0.7f;
    public float walkSpeed = 6;
    public float runSpeed = 15;
    public float slideSpeed = 15;
    public float crouchSpeed = 3;
    public float runTimer = 0;
    public float runMax = 2;
    private bool running = false;
    private bool crouching = false;

    private void Start() {
        PController.Instance.moveSpeed = walkSpeed;
    }

    private void Update() {
        var crouch = Input.GetKeyDown(KeyCode.C);
        //run script
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !running) {
            running = true;
            runTimer = 0.0f;
        }
        if (crouch && !running) {
            StartCoroutine("Crouch");
        }

        if (running) {
            PController.Instance.moveSpeed = runSpeed;
            runTimer += Time.deltaTime;
            if (runTimer > runMax) {
                PController.Instance.moveSpeed = walkSpeed;
                running = false;
            }
        }
    }

    IEnumerator Crouch() {
        if (!crouching) {
            float c = 0.0f;
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(standHeight, crouchHeight, c), 1);
                c += Time.deltaTime * 3;
                yield return null;
            }
            PController.Instance.moveSpeed = crouchSpeed;
            crouching = true;
        }
        else if (crouching) {
            float c = 0.0f;
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(crouchHeight, standHeight, c), 1);
                c += Time.deltaTime * 3;
                yield return null;
            }
            PController.Instance.moveSpeed = walkSpeed;
            crouching = false;
        }
    }
}