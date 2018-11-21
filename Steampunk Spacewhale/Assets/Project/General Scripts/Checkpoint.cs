using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public bool showCheckpoint = true;
    public bool activatable = true;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = showCheckpoint;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (activatable)
        {
            Respawn.currentCheckpoint = this.gameObject;
            activatable = false;
        }
    }
}
