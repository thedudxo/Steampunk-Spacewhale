using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

	void Update () {
		if (PController.Instance.isGrounded && Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && GetComponent<AudioSource>().isPlaying == false && !PController.Instance.jumping && PController.Instance.isGrounded) {
            GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
        //Debug.Log(Input.GetAxis("Horizontal"));
	}
}
