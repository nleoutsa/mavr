using UnityEngine;
using System.Collections;

public class WheelControl : MonoBehaviour {

    public GameObject Airship;

    Rigidbody airshipBody;

    CharacterJoint joint;
    SteamVR_Controller.Device device;

    void Awake ()
    {
        airshipBody = Airship.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (joint != null)
        {
            float x = joint.connectedBody.transform.localEulerAngles.x;
            float rot_deg = x > 180F ? x - 360F : x;

            airshipBody.maxAngularVelocity = 100F;
            airshipBody.AddTorque(Airship.transform.up * rot_deg, ForceMode.VelocityChange);
            airshipBody.angularVelocity = Vector3.zero;

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                Object.Destroy(joint);
                joint = null;
            }
        }
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
