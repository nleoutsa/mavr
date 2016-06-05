using UnityEngine;
using System.Collections;

public class ThrustControl : MonoBehaviour
{
    public float thrust = 0F;
    float initialZ;

    // Use this for initialization
    void Start()
    {
        initialZ = transform.position.z;
    }

    void FixedUpdate()
    {
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<HandController>())
        {
            SteamVR_Controller.Device device = col.gameObject.GetComponent<HandController>().device;
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                Debug.Log("Inside GetPress");
                Vector3 pos = transform.position;
                pos.z = col.gameObject.transform.position.z;
                transform.position = pos;

                thrust = initialZ - transform.position.z;
            }
        }
    }
}