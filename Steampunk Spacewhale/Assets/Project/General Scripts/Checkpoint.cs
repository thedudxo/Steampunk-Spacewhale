using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public bool showCheckpoint = true;
    public bool activatable = true;
    public bool doResetObject = false;
    public GameObject resetObject;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = showCheckpoint;
	}

    public void UseCheckpoint(GameObject player)
    {
        player.transform.position = gameObject.transform.position;
        player.transform.rotation = gameObject.transform.rotation;
        if (doResetObject)
        {
            resetObject.GetComponent<IResetable>().Reset();
        }
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
