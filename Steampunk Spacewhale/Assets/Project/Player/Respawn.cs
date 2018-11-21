using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public static GameObject currentCheckpoint;
    public static bool dead = false;
    public GameObject deadText;
    public static KeyCode respawnKey = KeyCode.R;
    

	// Use this for initialization
	void Start () {
		
	}

   

    // Update is called once per frame
    void Update () {
        deadText.SetActive(dead);
        if (Input.GetKey(respawnKey))
        {
            dead = false;
            transform.position = currentCheckpoint.transform.position;
        }
    }

    public static void Kill()
    {
        dead = true;
    }
}
