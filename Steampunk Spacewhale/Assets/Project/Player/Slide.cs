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
    public bool crouching = false;
    private bool sliding = false;

    private void Start() {
        PController.Instance.moveSpeed = walkSpeed;
    }

    private void Update() {
        if (crouching && !sliding) {
            PController.Instance.moveSpeed = crouchSpeed;
        } else if (!crouching && !sliding){
            PController.Instance.moveSpeed = walkSpeed;
        }
        var crouch = Input.GetKeyDown(KeyCode.C);
        //run script
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !running) {
            running = true;
            runTimer = 0.0f;
        }
        if (crouch && !running) {
            StartCoroutine("Crouch");
        }


        if (running && !sliding) {
            PController.Instance.moveSpeed = runSpeed;
            runTimer += Time.deltaTime;
            if (runTimer > runMax) {
                PController.Instance.moveSpeed = walkSpeed;
                running = false;
            }
            if (crouch) {
                StartCoroutine(SlideBoost());
            }
        }
        //Debug.Log(crouching);
    }

    IEnumerator SlideBoost(float duration = 10f) {
        PController.Instance.moveSpeed = slideSpeed;
        sliding = true;
        float c = 0.0f;
        while (c <= 1) {
            transform.localScale = new Vector3(1, Mathf.Lerp(standHeight, crouchHeight, c), 1);
            c += Time.deltaTime * 3;
            yield return c;
        }
        running = false;
        crouching = true;
        float elapsed = 0.0f;
        while (elapsed <= duration) {
            PController.Instance.moveSpeed = Mathf.Lerp(slideSpeed, walkSpeed, elapsed / duration);
            elapsed += Time.deltaTime * 7;
            yield return elapsed;
        }
        sliding = false;
    }

    IEnumerator Crouch()
    {
        float c = 0.0f;
        if (!crouching) {
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(standHeight, crouchHeight, c), 1);
                c += Time.deltaTime * 3;
                yield return c;
            }
            crouching = true;
        }
        else if (crouching) {
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(crouchHeight, standHeight, c), 1);
                c += Time.deltaTime * 3;
                yield return null;
            }
            crouching = false;
        }
    }
}