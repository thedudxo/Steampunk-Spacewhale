using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiKrill : MonoBehaviour {

    public Vector3 stop = new Vector3(14.6f, -1.1f, 441.6f); //just outside the blowhole
    public float startSpeed = 0.01f;
    bool taxied = false;
    bool taxy = false;
    public GameObject player;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.z <= stop.z) { taxied = true; return; }
        if (taxy)
        {
            transform.position = new Vector3(stop.x, stop.y, transform.position.z - startSpeed);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - startSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        taxy = true;
    }
}
