using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dangerous : MonoBehaviour {
    private bool newCollision = true;
    public float decay = 0.03f;
    public GameObject Green;
    private Color color;
    bool flashing = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flashing)
        {
            float oldAlpha = Green.GetComponent<Image>().color.a;
            Green.GetComponent<Image>().color = new Color(0, 1, 0, oldAlpha - decay);
            if(Green.GetComponent<Image>().color.a <= 0f)
            {
                color = Green.GetComponent<Image>().color = new Color(0, 1, 0, 0.8f);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (newCollision)
        {
            color = Green.GetComponent<Image>().color = new Color(0, 1, 0, 0.8f);
            newCollision = false;
            flashing = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        newCollision = true;
        flashing = false;
        Green.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
}
