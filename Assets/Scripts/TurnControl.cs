namespace VRTK
{
    using UnityEngine;
    using System.Collections;

    // Rotate dirigible based on rotation of wheel.

    public class TurnControl : MonoBehaviour
    {
        public GameObject Airship;
        public GameObject Rudder;
        public float turnSpeedMultiplier = 1;

        private void Update()
        {
            float angleY = (Quaternion.Inverse(transform.parent.transform.localRotation) * transform.localRotation).y;

            Rudder.transform.localRotation = new Quaternion(0, angleY / 2, 0, transform.localRotation.w);
            Airship.transform.Rotate(0, -angleY * 0.1f *  turnSpeedMultiplier, 0, Space.Self);
        }
    }
}
