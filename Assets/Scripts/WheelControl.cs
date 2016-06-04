using UnityEngine;
using System.Collections;

public class WheelControl : MonoBehaviour
{
    public Vector3 initialRotation;

    // Use this for initialization
    void Awake()
    {
        initialRotation = gameObject.transform.position;
    }
}