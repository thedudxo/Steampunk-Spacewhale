using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

    public GameObject startCamera;
    public GameObject startText;
    public GameObject player;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            startCamera.SetActive(false);
            startText.SetActive(false);
            player.SetActive(true);
            gameObject.SetActive(false);
        }
	}

}
