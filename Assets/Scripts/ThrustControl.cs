namespace VRTK
{
    using UnityEngine;
    using System.Collections;

    public class ThrustControl : MonoBehaviour
    {
        public Transform Airship;
        public Transform ThrustTransformOrigin;
        public float speedMultiplier = 1;

        [HideInInspector]
        public Vector3 airshipVelocity;

        private bool grabbingHandle = false;
        private float dotProduct;
        private Quaternion angleDelta;
        private float angleY;
        private float angleNormalized;
        private Quaternion uprightAngle;

        void Awake()
        {
            uprightAngle = transform.localRotation;
        }

        private void OnTriggerStay(Collider collider)
        {
            VRTK_ControllerEvents controllerEvents = collider.GetComponentInParent<VRTK_ControllerEvents>();

            if (controllerEvents && controllerEvents.gripPressed)
            {
                dotProduct = Vector3.Dot(ThrustTransformOrigin.position, transform.InverseTransformPoint(controllerEvents.transform.position));

                grabbingHandle = true;
            }
            else
            {
                grabbingHandle = false;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            grabbingHandle = false;
        }

        private void FixedUpdate()
        { 
            if (grabbingHandle)
            {
                transform.Rotate(0, dotProduct, 0, Space.Self); 
            }
            else
            {
                ReturnLeverUpright(); 
            }

            angleDelta = Quaternion.Inverse(uprightAngle) * transform.localRotation;
            angleY = Mathf.Clamp(angleDelta.y, -0.3f, 0.3f);
            angleNormalized = angleY / 0.3f;

            if (Mathf.Abs(angleNormalized) > 0.1f)
            {
                airshipVelocity = Airship.forward * angleNormalized * 0.05f * speedMultiplier;
                Airship.Translate(airshipVelocity, Space.World);
            } 
        }

        private void ReturnLeverUpright ()
        {
            if (Mathf.Abs(angleNormalized) > 0.025f)
            {
                transform.Rotate(0, Mathf.Sign(angleNormalized) * -0.1f, 0, Space.Self); 
            } 
        }
    }
}