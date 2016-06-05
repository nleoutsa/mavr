using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class HandController : MonoBehaviour {

    SteamVR_TrackedObject tracked_obj;
    public SteamVR_Controller.Device device = null;

    // This function is always called before any Start functions 
    // and also just after a prefab is instantiated. 
    // (If a GameObject is inactive during start up Awake is not called until it is made active.)
    void Awake()
    {
        tracked_obj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        // Create variable to represent input device (hand controller)
        if (device == null)
        {
            device = SteamVR_Controller.Input((int)tracked_obj.index);
        }

    }
}
