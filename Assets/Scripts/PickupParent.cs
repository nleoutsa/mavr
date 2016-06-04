using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour
{
    SteamVR_TrackedObject tracked_obj;
    SteamVR_Controller.Device device;

    public const float TOSS_HANDICAP = 2.0F;

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
        if (col.attachedRigidbody)
        {   
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (col.gameObject.GetComponent<TossableObject>())
                {
                    col.attachedRigidbody.isKinematic = true;
                    col.gameObject.transform.SetParent(gameObject.transform);
                }
            };

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (col.gameObject.GetComponent<TossableObject>())
                {
                    col.gameObject.transform.SetParent(null);
                    col.attachedRigidbody.isKinematic = false;
                    tossObject(col.attachedRigidbody);
                }
            };
        }

        if (col.gameObject.GetComponent<WheelControl>())
        {
            float initialAngle = col.gameObject.transform.eulerAngles.x;

            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                float gripY = gameObject.transform.position.y;
                float gripZ = gameObject.transform.position.z;
                float wheelZ = col.gameObject.transform.position.z;
                float wheelY = col.gameObject.transform.position.y;

                float numerator = gripY - wheelY;
                float denominator = gripZ - wheelZ;

                denominator = denominator == 0 ? 0.0000000000001F : denominator ;

                float oppOverAdj = numerator / denominator;

                float angle = Mathf.Atan(oppOverAdj) * 180 / Mathf.PI;

                Debug.Log("angle: " + (initialAngle - angle));
                
                col.gameObject.transform.eulerAngles = new Vector3(initialAngle - angle, 0F, 0F);
            }
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
