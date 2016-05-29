using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]  
public class PickupParent : MonoBehaviour {
    SteamVR_TrackedObject tracked_obj;

    // This function is always called before any Start functions 
    // and also just after a prefab is instantiated. 
    // (If a GameObject is inactive during start up Awake is not called until it is made active.)
    void Awake () {
        tracked_obj = GetComponent<SteamVR_TrackedObject>();
	}

    // FixedUpdate is called once for every physics step.
    // Use for regular updates to rigid bodies affected by physics
    void FixedUpdate () {
        // create variable to represent input device (hand controller)
        Device device = SteamVR_Controller.Input((int)tracked_obj.index);
    }
}
