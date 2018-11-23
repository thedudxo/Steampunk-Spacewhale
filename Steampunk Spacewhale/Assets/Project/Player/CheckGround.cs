using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour {

    private static CheckGround instance;
    public static CheckGround Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CheckGround>();
            }
            return CheckGround.instance;
        }
    }

    public void GroundChecker() {
        if (PController.Instance.jumping) { return; }
        Ray ray;
        RaycastHit hit;
        ray = new Ray(transform.position, -PController.Instance.myNormal);
        if (Physics.Raycast(ray, out hit, PController.Instance.jumpLimit)) {
            PController.Instance.nope = true;
        } else {
            PController.Instance.nope = false;
            PController.Instance.surfaceNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, -PController.Instance.myNormal * PController.Instance.jumpLimit, Color.red);
    }
}
