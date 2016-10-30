using UnityEngine;
using System.Collections;

public class ElevationControl : MonoBehaviour
{
    public Transform Airship;
    public float speedMultiplier = 1;
    public float minElevation = 0;
    public float maxElevation = 400;
    private float height;

    [HideInInspector]
    public Vector3 airshipVelocity;

    void Awake()
    {
        height = Airship.position.y;
        if (height > maxElevation)
        {
            maxElevation = height;
        }
        else if (height < minElevation)
        {
            minElevation = height;
        }
    }

    private void Update()
    {
        height = Airship.position.y;

        float angleY = (Quaternion.Inverse(transform.parent.transform.localRotation) * transform.localRotation).y;

        if ((angleY > 0 && height >= maxElevation) || (angleY < 0 && height <= minElevation))
        {
            return;
        }

        Airship.Translate(Airship.up * angleY * 0.01f * speedMultiplier, Space.World);
    }
}
