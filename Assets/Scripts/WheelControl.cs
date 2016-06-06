using UnityEngine;
using System.Collections;

public class WheelControl : MonoBehaviour
{
    public float rotationValue = 0F;
    public Transform lookAtTarget = null;
    Vector3 point;

    void Awake()
    {
    }

    void FixedUpdate()
    {
        if (lookAtTarget)
        {
            point = lookAtTarget.position;
            point.x = transform.position.x;
            transform.LookAt(point, transform.up);
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 0F, 0F);
            rotationValue = transform.eulerAngles.x;
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<HandController>())
        {
            SteamVR_Controller.Device device = col.gameObject.GetComponent<HandController>().device;
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                lookAtTarget = col.gameObject.transform;
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                lookAtTarget = null;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<HandController>())
        {
            lookAtTarget = null;
        }
    }
}