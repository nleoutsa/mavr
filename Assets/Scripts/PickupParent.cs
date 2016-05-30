using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour
{
    SteamVR_TrackedObject tracked_obj;
    SteamVR_Controller.Device device;

    public const float TOSS_HANDICAP = 5.0F;

    // This function is always called before any Start functions 
    // and also just after a prefab is instantiated. 
    // (If a GameObject is inactive during start up Awake is not called until it is made active.)
    void Awake()
    {
        tracked_obj = GetComponent<SteamVR_TrackedObject>();
    }

    // FixedUpdate is called once for every physics step.
    // Use for regular updates to rigid bodies affected by physics
    void FixedUpdate()
    {
        // Create variable to represent input device (hand controller)
        device = SteamVR_Controller.Input((int)tracked_obj.index);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            resetGame();
        }
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Collied with " + col.name + " and called OnTriggerStay");
        
        if (col.attachedRigidbody)
        {   
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("You have collided with " + col.name + " while holding down touch on the trigger");

                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(gameObject.transform);
            };

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("You have released the trigger while parenting " + col.name);

                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                tossObject(col.attachedRigidbody);
            };
        }
    }

    private void tossObject(Rigidbody rigidbody)
    {
        Transform origin = tracked_obj.origin ? tracked_obj.origin : tracked_obj.transform.parent;

        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity) * TOSS_HANDICAP;
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity) * TOSS_HANDICAP;
        } else
        {
            rigidbody.velocity = device.velocity * TOSS_HANDICAP;
            rigidbody.angularVelocity = device.angularVelocity * TOSS_HANDICAP;
        }
    }

    private void resetGame()
    {
        foreach (TossableObject obj in FindObjectsOfType<TossableObject>())
        {
            obj.ResetObject();
        }
        foreach (CylinderBehavior obj in FindObjectsOfType<CylinderBehavior>())
        {
            obj.ResetObject();
        }
    }
}
