using UnityEngine;
using System.Collections;

public class CylinderBehavior : MonoBehaviour
{
    public Vector3 initialPosition;

    // Use this for initialization
    void Awake()
    {
        initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {

        if (col.GetComponent<TossableObject>()) // check if type is ball here...
        {
            Debug.Log(gameObject.name + " was hit by " + col.name + ". Called OnTriggerStay");
            foreach (CylinderBehavior obj in FindObjectsOfType<CylinderBehavior>())
            {
                obj.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    public void ResetObject()
    {
        GetComponent<Rigidbody>().transform.position = initialPosition;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().transform.rotation = Quaternion.identity;
    }
}
