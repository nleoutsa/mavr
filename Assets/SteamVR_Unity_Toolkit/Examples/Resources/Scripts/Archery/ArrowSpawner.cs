using System;
using System.Collections.Generic;
using UnityEditor;

namespace VRTK.Examples.Archery
{
    using UnityEngine;

    public class ArrowSpawner : MonoBehaviour
    {
        public GameObject arrowPrefab;
        public float spawnDelay = 1f;
        public int arrowsLeft = 50;
        public int quiverCapacity = 50;

        private float spawnDelayTimer = 0f;
        private BowAim bow;
        private bool controllerInQuiver = false;
        private List<VRTK_InteractGrab> controllersListening = new List<VRTK_InteractGrab>();

        private void Start()
        {
            spawnDelayTimer = 0f;
        }

        private void OnTriggerStay(Collider collider)
        {
            VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());

            if (arrowsLeft > 0 && CanGrab(grabbingController) && NoArrowNotched(grabbingController.gameObject) && Time.time >= spawnDelayTimer)
            {
                PickupArrow(grabbingController);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());

            if (grabbingController)
            {
                controllerInQuiver = true;

                if (!controllersListening.Contains(grabbingController))
                {
                    grabbingController.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(DropArrow);
                    controllersListening.Add(grabbingController);
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());

            if (grabbingController)
            {
                controllerInQuiver = false;
            }
        }

        private bool CanGrab(VRTK_InteractGrab grabbingController)
        {
            return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.gameObject.GetComponent<VRTK_ControllerEvents>().grabPressed);
        }

        private void RemoveArrows(int num)
        {
            arrowsLeft -= num;
        }

        private void AddArrows(int num)
        {
            arrowsLeft += num;
        }

        private void PickupArrow(VRTK_InteractGrab grabbingController)
        {
            GameObject newArrow = Instantiate(arrowPrefab);
            newArrow.name = "ArrowClone";
            grabbingController.gameObject.GetComponent<VRTK_InteractTouch>().ForceTouch(newArrow);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
            RemoveArrows(1);
        }

        private void DropArrow(object sender, ObjectInteractEventArgs e)
        {
            if (controllerInQuiver && arrowsLeft < quiverCapacity)
            {
                var grabbedArrow = e.target;

                if (grabbedArrow.GetComponent<ArrowNotch>() != null)
                {
                    Destroy(grabbedArrow);
                    AddArrows(1);
                }
            }
        }

        private bool NoArrowNotched(GameObject controller)
        {
            if (VRTK_SDK_Bridge.IsControllerLeftHand(controller))
            {
                bow = VRTK_DeviceFinder.GetControllerRightHand().GetComponentInChildren<BowAim>();
            }
            else if (VRTK_SDK_Bridge.IsControllerRightHand(controller))
            {
                bow = VRTK_DeviceFinder.GetControllerLeftHand().GetComponentInChildren<BowAim>();
            }
            return (bow == null || !bow.HasArrow());
        }
    }
}