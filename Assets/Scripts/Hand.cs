using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    [SerializeField] public InputActionReference controllerActionTrigger;
    private XRDirectInteractor interactor;
    
    private float prevTrigger = 0f;
    private float prevSelect = 0f;

    private void Start() {
        interactor = GetComponent<XRDirectInteractor>();
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
                }
                prevSelect = 0f;
            }
        }
    }
}
