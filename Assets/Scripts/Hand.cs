using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    [SerializeField] public InputActionReference controllerActionTrigger;
    public int isLeftHand; // 1 if true, 0 if false
    private XRDirectInteractor interactor;
    private XRRayInteractor rayInteractor;
    
    private float prevTrigger = 0f;
    private float prevSelect = 0f;

    private void Start() {
        interactor = GetComponent<XRDirectInteractor>();
        if (isLeftHand == 1) {
            rayInteractor = GameObject.Find("Left Ray Interactor").GetComponent<XRRayInteractor>(); 
        } else {
            rayInteractor = GameObject.Find("Right Ray Interactor").GetComponent<XRRayInteractor>();
        }
    }

    // Update is called once per frame 
    void Update() {
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("SprayCan")) {
            if (interactor.hasSelection) {
                if (prevSelect == 0) {
                    prevSelect = 1f;
                    other.gameObject.GetComponent<SprayCan>().Shake();
                    rayInteractor.enabled = false;
                }
                // Trigger checks to see whether spraying or not
                float trigger = controllerActionTrigger.action.ReadValue<float>();
                if (trigger != 0 && prevTrigger == 0){
                    other.gameObject.GetComponent<SprayCan>().startSpray();
                } else if (trigger == 0 && prevTrigger != 0) {
                    other.gameObject.GetComponent<SprayCan>().endSpray();
                }
                prevTrigger = trigger;
            }
            else {
                if (prevTrigger != 0) {
                    other.gameObject.GetComponent<SprayCan>().endSpray();
                    prevTrigger = 0f;
                }
                if (prevSelect != 0) {
                    rayInteractor.enabled = true;
                    prevSelect = 0f;
                }
            }
        }
    }
}
