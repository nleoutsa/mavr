using UnityEngine;
using System.Collections;

public class TossableObject : MonoBehaviour {
    public bool isKinematic;
    public Vector3 initialPosition;

	// Use this for initialization
	void Awake () {
        isKinematic = GetComponent<Rigidbody>().isKinematic;
        initialPosition = gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetObject()
    {
        GetComponent<Rigidbody>().transform.position = initialPosition;
        GetComponent<Rigidbody>().isKinematic = isKinematic;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
