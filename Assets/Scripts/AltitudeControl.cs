using UnityEngine;
using System.Collections;

public class AltitudeControl: MonoBehaviour
{
    public float altitude = 0F;
    float initialY;

    // Use this for initialization
    void Awake()
    {
        initialY = transform.position.y;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<HandController>())
        {
            SteamVR_Controller.Device device = col.gameObject.GetComponent<HandController>().device;
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                Vector3 pos = transform.position;
                pos.y = col.gameObject.transform.position.y;
                transform.position = pos;

                altitude = initialY - transform.position.y;

                Debug.Log("initY: " + initialY + ", posY: " + transform.position.y + ", alt: " + altitude);
            }
        }
    }
}
