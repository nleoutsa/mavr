using UnityEngine;
using System.Collections;

public class ThrustControl : MonoBehaviour {


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
            float angle = joint.connectedBody.transform.localEulerAngles.z;

            angle = angle > 180F ? angle - 360F : angle;

            airshipBody.AddForce(Airship.transform.forward * angle, ForceMode.Impulse); 

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
