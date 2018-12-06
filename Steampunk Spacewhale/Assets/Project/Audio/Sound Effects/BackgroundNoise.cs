using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNoise : MonoBehaviour {

    public float minWait;
    public float maxWait;
    public float waitTime;
    private bool wait = false;
    public int soundChoice;

    private void Update() {
        if (wait) { return; }
        StartCoroutine(RandomPlay());
    }

    IEnumerator RandomPlay() {
        wait = true;
        waitTime = Random.Range(minWait, maxWait);
        soundChoice = Random.Range(0, FindObjectOfType<AudioManager>().sounds.Length);
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<AudioManager>().Play("Bang_" + soundChoice);
        wait = false;
        yield return null;
    }
}
