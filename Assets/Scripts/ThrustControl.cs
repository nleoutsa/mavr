using UnityEngine;
using System.Collections;

public class ThrustControl : MonoBehaviour {


    public GameObject Airship;

    CharacterJoint joint;
    SteamVR_Controller.Device device;

    float thrust_amount = 0F;
    float thrust_angle = 0F;

    void FixedUpdate()
    {
        if (joint != null)
        {
            thrust_angle = joint.connectedBody.transform.localEulerAngles.z;

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                Object.Destroy(joint);
                joint = null;
            }
        }
        else
        {
            thrust_angle = 0F;
        }

        thrust_amount = thrust_angle > 180F ? thrust_angle - 360F : thrust_angle;

        Airship.GetComponent<Rigidbody>().AddForce(Airship.transform.forward * thrust_amount, ForceMode.Impulse); 
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
