using System;

namespace VRTK.Examples.Archery
{
    using UnityEngine;
    using System.Collections;

    public class Arrow : MonoBehaviour
    {
        public float maxArrowLife = 600f;

        [HideInInspector] public bool inFlight = false;

        private bool collided = false;
        private bool fired = false;
        private Rigidbody rigidBody;
        private GameObject arrowHolder;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;

        public void SetArrowHolder(GameObject holder)
        {
            arrowHolder = holder;
            arrowHolder.SetActive(false);
        }

        public void OnNock()
        {
            collided = false;
            inFlight = false;
        }

        public void Fired()
        {
            StartCoroutine(EmitSmoke());
            DestroyArrow(maxArrowLife);
            StartCoroutine(SetFired());
        }

        public void ResetArrow()
        {
            collided = true;
            inFlight = false;
            RecreateNotch();
            ResetTransform();
        }

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            SetOrigns();
        }

        private void SetOrigns()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
            originalScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (!collided && rigidBody != null)
            {
                transform.LookAt(transform.position + rigidBody.velocity);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (inFlight)
            {
                StickAfterCollision(collision);
            }
        }

        private void OnTriggerStay(Collider collider)
        {

            // TODO MAKE THIS WORK! Need to reset arrow so it can be picked up again, but still allowing it to stick into walls
            if (fired)
            {
                VRTK_ControllerEvents controllerEvents = collider.GetComponentInParent<VRTK_ControllerEvents>();

                if (controllerEvents != null && controllerEvents.grabPressed)
                {
                    VRTK_InteractGrab grabbingController = collider.GetComponentInParent<VRTK_InteractGrab>();

                    fired = false;
                    gameObject.AddComponent<Rigidbody>();
                    gameObject.GetComponent<Collider>().isTrigger = false;
                    ResetArrow();
                    StartCoroutine(grabArrow(grabbingController));
                }
            }
        }

        private void RecreateNotch()
        {
            //swap the arrow holder to be the parent again
            arrowHolder.transform.parent = null;
            arrowHolder.SetActive(true);

            //make the arrow a child of the holder again
            transform.parent = arrowHolder.transform;

            //reset the state of the rigidbodies and colliders
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            arrowHolder.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void ResetTransform()
        {
            arrowHolder.transform.position = transform.position;
            arrowHolder.transform.rotation = transform.rotation;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
            transform.localScale = originalScale;
        }

        private void DestroyArrow(float time)
        {
            if (fired)
            {
                Destroy(arrowHolder, time);
                Destroy(gameObject, time);
            }
        }

        IEnumerator EmitSmoke()
        {
            yield return new WaitForSeconds(0.05f);
            gameObject.GetComponent<ParticleSystem>().Play();
        }

        IEnumerator SetFired()
        {
            yield return new WaitForSeconds(0.05f);
            fired = true;
        }

        IEnumerator grabArrow(VRTK_InteractGrab grabbingController)
        {
            yield return new WaitForSeconds(0.05f);
            fired = true;
            grabbingController.AttemptGrab();
        }

        public void StickAfterCollision(Collision collision)
        {
            collided = true;
            inFlight = false;

            // child the arrow to the collided object
            transform.parent = collision.transform;
            //Destroy the arrow's rigidbody2D
            Destroy(gameObject.GetComponent<Rigidbody>());
            // Set collider to trigger
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }
}