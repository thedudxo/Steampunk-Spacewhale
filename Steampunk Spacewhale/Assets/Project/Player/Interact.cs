using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interact : MonoBehaviour {

    public float armLength = 2.5f;
    public KeyCode interactKey = KeyCode.E;
    public GameObject tutorialText;
    public GameObject[] interactables;
    RaycastHit hit;
    Ray ray;
    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tutorialText.SetActive(false);
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, armLength))
        {
            GameObject interactee = hit.collider.gameObject;
            if (interactables.Contains(interactee))
            {
                tutorialText.SetActive(true);
                if (Input.GetKeyDown(interactKey))
                {
                    if (interactee.tag == "lever1") //rotate the thinggy
                    {
                        animator.SetTrigger("Lever");
                        RotatingPlatform.Rotate();
                        
                    }
                }
            }
           
           
        }

	}

    
}
