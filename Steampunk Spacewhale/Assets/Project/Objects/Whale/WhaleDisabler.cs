using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDisabler : MonoBehaviour {

    public GameObject whale;
    public bool enable = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        whale.SetActive(enable);
    }
}
