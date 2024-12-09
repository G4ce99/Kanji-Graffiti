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

    public GameObject paintPrefab;
    public GameObject wall;
    private ParticleSystem particleSystem;
    private Vector3 prevPos;
    private float minDis = 1; //Distance btw paint does before need to fill in
    private int maxTime = 5; //Only fill in if dots are in short succession
    private int timer; //How long btw dots

    private Vector3 initialPosition; //Used to reset if they drop the spray can
    private Quaternion initialRotation; //Used to reset if they drop the spray can
    private Rigidbody Rigidbody;


    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sprayClip;
        isSpraying=false;
        sprayClipStartTime = 0.18f;
        sprayClipEndTime = 0.51f;

        prevPos = wall.transform.position; //Initializes previous paint dot to be at wall
        prevPos.z = -0.05f; 
        timer = 0;
        particleSystem = transform.Find("SprayEffect").GetComponent<ParticleSystem>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y <= 0.01f) {
            // Dropped the can, reset so game continues
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            Rigidbody.velocity = Vector3.zero;
        }
        if (isSpraying) {
            if (audioSource.time > sprayClipEndTime) {
                audioSource.time = sprayClipStartTime;
                audioSource.Play();
            }
            // Do spraying mechanic here (raycasting, plane emission, etc.)
            
            // From Gun.cs for lab:
            RaycastHit hit;
            Vector3 origin = particleSystem.transform.position;
            Vector3 direction = particleSystem.transform.forward;
            if (Physics.Raycast(origin, direction, out hit, 100f)) {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("Wall")|| hitObject.CompareTag("Paint")) //|| hitObject.CompareTag("Paint")) //Paints only on specified wall
                {
            //
                    Vector3 paintLocation = hit.point;
                    paintLocation.z -= 0.01f; //Makes planes slightly infront of wall, change if on diff orientation
                    paintLocation.y += 0.1f;
                    GameObject paint = Instantiate(paintPrefab, paintLocation, Quaternion.Euler(-90, 0, 0));
                    //this.transform.rotation
                    float disBetween = Vector3.Distance(paintLocation, prevPos);
                    if (disBetween > minDis && timer < maxTime) {
                        FillIn(prevPos, paintLocation, disBetween);
                    }

                    prevPos = paintLocation;
                    timer = 0;
                }
                
            }

        }
        timer++;
    }

    public void Shake() {
        audioSource.PlayOneShot(shakeClip);
    }

    public void startSpray() {
        isSpraying = true;
        audioSource.time = sprayClipStartTime;
        audioSource.Play();

        particleSystem.Play();
    }

    public void endSpray() {
        audioSource.Stop();
        isSpraying = false;

        particleSystem.Stop();
    }

    private void FillIn(Vector3 start, Vector3 end, float dist)
    {
        Vector3 direction = end - start;
        direction = direction.normalized;
        Vector3 loc = start;
        for (float i = 0; i < dist; i+=0.2f) {
            loc.x += direction.x * 0.2f;
            loc.y += direction.y * 0.2f;
            GameObject paint = Instantiate(paintPrefab, loc, this.transform.rotation);
        }
    }
}
