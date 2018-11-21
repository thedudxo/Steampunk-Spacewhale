using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dangerous : MonoBehaviour {
    private bool newCollision = true;
    public float decay = 0.03f;
    private float deathTime = 0.01f;
    private float deathTimer;
    public GameObject Green;
    private Color color;
    bool flashing = false;

    private void Reset()
    {
        if (Respawn.dead) { return; }
        newCollision = true;
        flashing = false;
        Green.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        deathTimer = 0;
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

            deathTimer += Time.deltaTime;
            if (deathTimer >= deathTime)
            {
                color = Green.GetComponent<Image>().color = new Color(1, 0, 0, 1f);
                flashing = false;
                Respawn.dead = true;
                Respawn.Kill();
            }
        } else if (!Respawn.dead)
        {
            Green.GetComponent<Image>().color = new Color(0, 0, 0, 0);
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
        Reset();
    }
}
