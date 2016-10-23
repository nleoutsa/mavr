namespace VRTK
{
    using UnityEngine;
    using System.Collections;

    public class ElevationControl : MonoBehaviour
    {
        public Transform Airship;
        public Transform ElevationTransformOrigin;
        public float speedMultiplier = 1;
        public float minElevation = 0;
        public float maxElevation = 400;

        [HideInInspector]
        public Vector3 airshipVelocity;

        private bool grabbingHandle = false;
        private float dotProduct;
        private Quaternion angleDelta;
        private float angleY;
        private float angleNormalized;
        private Quaternion uprightAngle;
        private float height;

        void Awake()
        {
            uprightAngle = transform.localRotation;

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

        private void OnTriggerStay(Collider collider)
        {
            VRTK_ControllerEvents controllerEvents = collider.GetComponentInParent<VRTK_ControllerEvents>();

            if (controllerEvents && controllerEvents.gripPressed)
            {
                dotProduct = Vector3.Dot(ElevationTransformOrigin.position, transform.InverseTransformPoint(controllerEvents.transform.position));

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

        private void Update()
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
            height = Airship.position.y;

            if ((angleY > 0 && height >= maxElevation) || (angleY < 0 && height <= minElevation))
            {
                return;
            }

            angleNormalized = angleY / 0.3f;

            if (Mathf.Abs(angleNormalized) > 0.1f)
            {
                airshipVelocity = Airship.up * angleNormalized * 0.01f * speedMultiplier;
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