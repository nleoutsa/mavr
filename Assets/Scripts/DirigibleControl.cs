using UnityEngine;
using System.Collections;

public class DirigibleControl : MonoBehaviour
{
    public Transform Airship;

    public float speedMultiplier = 1;
    public float verticalSpeedMultiplier = 1;

    public float minElevation = 0;
    public float maxElevation = 400;

    [HideInInspector]
    private float height;

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

        // TODO: UI still reacting to global rotations.
        height = Airship.position.y;

        float positionX = transform.localPosition.x;
        float positionY = transform.localPosition.y;
        float positionZ = transform.localPosition.z;

        float hypoteneuse = Mathf.Sqrt(Mathf.Pow(positionX, 2) + Mathf.Pow(positionZ, 2));
        float sign = Mathf.Sign(positionZ);
        float elevationDirection = Mathf.Sign(positionY);

        if (Mathf.Abs(hypoteneuse) < .01 || Mathf.Abs(positionY) < .01)
        {
            return;
        }

        Airship.Translate(Airship.forward * sign * hypoteneuse * 0.1f * speedMultiplier, Space.World);

        if ((height >= maxElevation && elevationDirection == 1) || (height <= minElevation && elevationDirection == -1))
        {
            return;
        }

        Airship.Translate(Airship.up * positionY * 0.1f * verticalSpeedMultiplier, Space.World);
    }
}
