namespace VRTK
{
    using UnityEngine;
    using System.Collections;

    public class TurnDirigibleControl : MonoBehaviour
    {
        public GameObject Airship;
        public GameObject Rudder;
        public ThrustControl ThrustControl;
        public float turnSpeedMultiplier = 10;

        private bool grabbingWheel = false;
        private float dotProduct;
        private Quaternion angleDelta;
        private float angleY;
        private float angleNormalized;
        private Quaternion uprightAngle;
        private float CLAMP = 0.5f;

        void Awake()
        {
            uprightAngle = transform.localRotation;
        }

        private void OnTriggerStay(Collider collider)
        {
            VRTK_ControllerEvents controllerEvents = collider.GetComponentInParent<VRTK_ControllerEvents>();

            if (controllerEvents && controllerEvents.gripPressed)
            {
                dotProduct = Vector3.Dot(transform.position, transform.InverseTransformPoint(controllerEvents.transform.position));

                grabbingWheel = true;
            }
            else
            {
                grabbingWheel = false;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            grabbingWheel = false;
        }

        private void Update()
        { 
            if (grabbingWheel)
            {
                // Rotate Wheel
                transform.Rotate(0, dotProduct, 0, Space.Self);
            }
            else
            {
                ReturnWheelUpright(); 
            }

            // Get angle of Wheel, clamp it, and normalize to 0-1 scale.
            angleDelta = Quaternion.Inverse(uprightAngle) * transform.localRotation;
            angleY = Mathf.Clamp(-angleDelta.y, -CLAMP, CLAMP);
            angleNormalized = angleY / CLAMP;

            // If angle of Wheel is greater than clamped value, return it to the previous position:
            if (Mathf.Abs(angleNormalized) >= 1)
            {
                // Capture current Wheel rotation Quaternion:
                transform.Rotate(0, -dotProduct, 0, Space.Self);
            }

            var turnSpeed = ThrustControl.airshipVelocity.magnitude * angleNormalized * 10f;

            Rudder.transform.localRotation = new Quaternion(0, angleY, 0, transform.localRotation.w);

            if (Mathf.Abs(angleNormalized) > (CLAMP / 5f))
            { 
                Airship.transform.Rotate(0, turnSpeed, 0, Space.Self);
            } 
        }

        private void ReturnWheelUpright ()
        {
            if (Mathf.Abs(angleNormalized) > (CLAMP / 20f))
            {
                transform.Rotate(0, Mathf.Sign(angleNormalized) * 1f, 0, Space.Self); 
            } 
        }
    }
}
