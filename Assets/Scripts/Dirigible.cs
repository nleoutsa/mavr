using UnityEngine;
using System.Collections;

public class Dirigible : MonoBehaviour {
    public WheelControl wheel_control;
    public ThrustControl thrust_control;
    public AltitudeControl altitude_control;

    // Use this for initialization
    void Awake () {
	}

    // Update is called once per frame
    void FixedUpdate() {
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();

        if (altitude_control.altitude != 0)
        {
            Vector3 altitudeVector = transform.up;
            altitudeVector.y += altitude_control.altitude / 100F;
            rigidBody.AddForce(altitudeVector, ForceMode.Acceleration);
            Debug.Log("altVal: " + altitude_control.altitude + ", vec: " + altitudeVector.y);
        }

        if (thrust_control.thrust != 0 )
        {
            Vector3 thrustVector = transform.up;
            thrustVector.x += thrust_control.thrust / 100F;
            rigidBody.AddForce(thrustVector, ForceMode.Acceleration);
            Debug.Log("thrust: " + thrust_control.thrust + ", vec: " + thrustVector.x);
        }
// 
//         Vector3 rotationVector = transform.up;
//         rotationVector.y += wheel_control.rotationValue / 100F;
//         rigidBody.AddTorque(rotationVector, ForceMode.Acceleration);
//         Debug.Log("rotationVal: " + wheel_control.rotationValue + ", vec: " + rotationVector.y);
	}
}
