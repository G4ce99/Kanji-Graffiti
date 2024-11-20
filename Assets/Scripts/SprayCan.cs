using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCan : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip shakeClip;
    public AudioClip sprayClip;

    private bool isSpraying;
    private float sprayClipStartTime;
    private float sprayClipEndTime;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sprayClip;
        isSpraying=false;
        sprayClipStartTime = 0.18f;
        sprayClipEndTime = 0.51f;
    }

    // Update is called once per frame
    void Update() {
        if (isSpraying) {
            if (audioSource.time > sprayClipEndTime) {
                audioSource.time = sprayClipStartTime;
                audioSource.Play();
            }
            // Do spraying mechanic here (raycasting, plane emission, etc.)

        }
    }

    public void Shake() {
        audioSource.PlayOneShot(shakeClip);
    }

    public void startSpray() {
        isSpraying = true;
        audioSource.time = sprayClipStartTime;
        audioSource.Play();
    }

    public void endSpray() {
        audioSource.Stop();
        isSpraying = false;

    }
}
