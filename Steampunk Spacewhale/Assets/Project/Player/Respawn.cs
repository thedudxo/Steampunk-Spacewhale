using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public static GameObject currentCheckpoint;
    public static bool dead = false;
    public GameObject deadText;
    public static KeyCode respawnKey = KeyCode.R;
    
    // Update is called once per frame
    void Update () {
        deadText.SetActive(dead);
        if (Input.GetKey(respawnKey))
        {
            dead = false;
            currentCheckpoint.GetComponent<Checkpoint>().UseCheckpoint(gameObject);
            PController.surfaceNormal = Vector3.up;
        }
    }

    public static void Kill()
    {
        dead = true;
    }
}
