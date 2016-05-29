using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour
{
    SteamVR_TrackedObject tracked_obj;
    SteamVR_Controller.Device device;


    List<Rigidbody> tossedObjects = new List<Rigidbody>();

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

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && tossedObjects.Count > 0)
        {
            resetTossedObjectPosition();
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
                col.attachedRigidbody.isKinematic = col.attachedRigidbody.GetComponent<TossableObject>().isKinematic;

                tossChildObject(col.attachedRigidbody);
            };
        }
    }

    private void tossChildObject(Rigidbody rigidbody)
    {

        Transform origin = tracked_obj.origin ? tracked_obj.origin : tracked_obj.transform.parent;

        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        } else
        {
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }

        // Add tossed object to list of tossed objects.
        tossedObjects.Add(rigidbody);
    }

    private void resetTossedObjectPosition()
    {
        tossedObjects[0].transform.position = tossedObjects[0].GetComponent<TossableObject>().initialPosition;
        tossedObjects[0].velocity = Vector3.zero;
        tossedObjects[0].angularVelocity = Vector3.zero;

        // Remove object tossed least recently from list of tossed objects.
        // This way, we reset object positions in order of when they were tossed.
        tossedObjects.RemoveAt(0);
    }
}
