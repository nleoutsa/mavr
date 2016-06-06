using UnityEngine;
using System.Collections;

public class ThrustControl : MonoBehaviour
{
    public float thrust = 0F;
    float initialX;

    // Use this for initial.xation
    void Awake()
    {
        initialX = transform.position.x;
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
                Vector3 pos = transform.position;
                pos.x = col.gameObject.transform.position.x;
                transform.position = pos;

                thrust = initialX - transform.position.x;
            }
        }
    }
}