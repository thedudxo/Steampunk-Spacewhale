using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float standHeight = 1;
    private float crouchHeight = 0.7f;
    public float walkSpeed = 6;
    public float runSpeed = 15;
    public float slideSpeed = 15;
    public float crouchSpeed = 3;
    public float runTimer = 0;
    public float runMax = 2;
    public bool running = false;
    public bool crouching = false;
    private bool sliding = false;

    private void Start() {
        PController.Instance.moveSpeed = walkSpeed;
    }

    private void Update() {
        if (!Respawn.dead) {
            if (crouching && !sliding && !running) {
                PController.Instance.moveSpeed = crouchSpeed;
            } else if (!crouching && !sliding && !running) {
                PController.Instance.moveSpeed = walkSpeed;
            } else if (!crouching && !sliding && running) {
                PController.Instance.moveSpeed = runSpeed;
            }

            var crouch = Input.GetKeyDown(KeyCode.C);
            //run script
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !running) {
                StartCoroutine(Run());
            }
            if (crouch && !running) {
                StartCoroutine("Crouch");
            }
            if (running && !sliding) {
                runTimer += Time.deltaTime;
                if (runTimer > runMax || Input.GetKeyUp(KeyCode.W)) {
                    StartCoroutine(Run());
                }
                if (crouch) {
                    StartCoroutine(SlideBoost());
                }
            }
            //Debug.Log(crouching);
        }
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

    IEnumerator Crouch() {
        float c = 0.0f;
        if (!crouching) {
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(standHeight, crouchHeight, c), 1);
                PController.Instance.moveSpeed = Mathf.Lerp(walkSpeed, crouchSpeed, c);
                c += Time.deltaTime * 3;
                yield return c;
            }
            crouching = true;
        }
        else if (crouching) {
            while (c <= 1) {
                transform.localScale = new Vector3(1, Mathf.Lerp(crouchHeight, standHeight, c), 1);
                PController.Instance.moveSpeed = Mathf.Lerp(crouchSpeed, walkSpeed, c);
                c += Time.deltaTime * 3;
                yield return c;
            }
            crouching = false;
        }
    }
    IEnumerator Run() {
        float r = 0.0f;
        if (!running) {
            runTimer = 0;
            while (r <= 1) {
                PController.Instance.moveSpeed = Mathf.Lerp(walkSpeed, runSpeed, r);
                r += Time.deltaTime * 1.5f;
                yield return r;
            }
            running = true;
        } else if (running) {
            while (r <= 1) {
                PController.Instance.moveSpeed = Mathf.Lerp(runSpeed, walkSpeed, r);
                r += Time.deltaTime * 2;
                yield return r;
            }
            running = false;
        }
    }
}