using UnityEngine;
using System.Collections;

public class Dirigible : MonoBehaviour {
    public WheelControl wheel_control;
    public ThrustControl thrust_control;
    public AltitudeControl altitude_control;

    // Use this for initialization
    void Awake () {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0F, 0F, 40F), ForceMode.Force);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.eulerAngles = new Vector3(0.0F, wheel_control.rotationValue, 0.0F);

        Vector3 thrustVector = gameObject.GetComponent<Rigidbody>().velocity;
        thrustVector.z = thrust_control.thrust;
        gameObject.GetComponent<Rigidbody>().AddForce(thrustVector, ForceMode.Force);
	}
}
