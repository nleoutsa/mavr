using UnityEngine;
using System.Collections;

namespace VRTK
{
    using UnityEngine;
    using System.Collections;

    public class ThrustControl : MonoBehaviour
    {
        public Transform Airship;
        public float speedMultiplier = 1;

        private void FixedUpdate()
        {
            float angleY = (Quaternion.Inverse(transform.parent.transform.localRotation) * transform.localRotation).y;
            Airship.Translate(Airship.forward * angleY * 0.05f * speedMultiplier, Space.World);
        }
    }
}