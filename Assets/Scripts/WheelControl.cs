using UnityEngine;
using System.Collections;

public class WheelControl : MonoBehaviour {

    public GameObject Airship;

    CharacterJoint joint;
    SteamVR_Controller.Device device;

    float rot_deg = 0F;

    void FixedUpdate()
    {
        if (joint != null)
        {
            float x = joint.connectedBody.transform.localEulerAngles.x;
            rot_deg = x > 180F ? x - 360F : x;

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                Object.Destroy(joint);
                joint = null;
            }
        }
        else
        {
            rot_deg = 0F;
        }

        Airship.GetComponent<Rigidbody>().maxAngularVelocity = 10;
        Airship.GetComponent<Rigidbody>().AddTorque(Airship.transform.up * rot_deg * 0.01F, ForceMode.VelocityChange);
        Airship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<HandController>())
        {
            device = col.gameObject.GetComponent<HandController>().device;
            if (joint == null && device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                joint = col.gameObject.AddComponent<CharacterJoint>();
                joint.connectedBody = gameObject.GetComponent<Rigidbody>();
            }   
        }
    }
}
