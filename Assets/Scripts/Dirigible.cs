using UnityEngine;
using System.Collections;

public class Dirigible : MonoBehaviour {
    //public WheelControl wheel_control;
    public ThrustControl thrust_control;
    //public AltitudeControl altitude_control;

    // Use this for initialization
    void Awake () {
	}

    // Update is called once per frame
    void FixedUpdate() {
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();

        //if (altitude_control.altitude != 0)
        //{
        //    Vector3 altitudeVector = transform.up;
        //    altitudeVector.y += altitude_control.altitude;
        //    rigidBody.AddForce(altitudeVector, ForceMode.Impulse);
        //    Debug.Log("altVal: " + altitude_control.altitude + ", vec: " + altitudeVector.y);
        //}

        Vector3 thrustVector = transform.forward;
        if (thrust_control.thrust_amount != 0)
        thrustVector.z = -(thrust_control.thrust_amount);
        rigidBody.AddForce(thrustVector, ForceMode.Impulse);

        //Quaternion rotationQuaternion = transform.rotation;
        //rotationQuaternion.y += wheel_control.rotationValue / 100F;
        //rigidBody.AddTorque(rotationQuaternion.x / Time.fixedDeltaTime, rotationQuaternion.y / Time.fixedDeltaTime, rotationQuaternion.z / Time.fixedDeltaTime, ForceMode.VelocityChange);
        //Debug.Log("rotationVal: " + wheel_control.rotationValue);
    }
}
