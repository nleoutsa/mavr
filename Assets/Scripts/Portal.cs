namespace VRTK
{
    using UnityEngine;
    using System.Collections;
    using VRTK;

    public class Portal : MonoBehaviour
    {
        public GameObject destination;

        public float teleport_fade_in = 0.5f;
        public float teleport_fade_out = 0.5f;
        public float teleport_delay = 0.5f;

        public bool startLocked = false;
        public GameObject key;

        [HideInInspector]
        private GameObject item_to_teleport;
        private bool locked;

        private bool alerted = false;

        private VRTK_ControllerEvents controller;

        void Awake()
        {
            locked = startLocked;

            item_to_teleport = GameObject.FindGameObjectWithTag("PlayerCharacter");
        }

        private void OnTriggerStay(Collider collider)
        {
            if (item_to_teleport.GetComponent<PlayerCharacter>().currentlyTeleporting == false && controller && controller.usePressed)
            {
                var isLocked = locked || destination.GetComponent<AreaController>().locked;

                if (isLocked && !alerted)
                {
                    alerted = true;
                    Invoke("AlertIsLocked", 1f);
                }
                else if (!isLocked)
                {
                    item_to_teleport.GetComponent<PlayerCharacter>().currentlyTeleporting = true;
                    BeginTeleport();
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            controller = collider.GetComponentInParent<VRTK_ControllerEvents>();

            if (locked && key && collider.GetComponent<Key>())
            {
                if (collider.gameObject == key)
                {
                    Debug.Log("Unlocking Portal.");
                    Unlock();
                }
                else
                {
                    Debug.Log("Wrong key.");
                }
            }
        }

        public void Lock()
        {
            locked = true;
        }

        public void Unlock()
        {
            locked = false;
        }

        private void AlertIsLocked()
        {
            // Visual indicator that the portal is locked.
            // Give clue as to how to unlock it.
            if (destination.GetComponent<AreaController>().locked)
            {
                Debug.Log("Destination Area is occupied. Gate will unlock when Destination Area is clear.");
            }
            else if (locked)
            {
                Debug.Log("Gate is locked. Find the Key!");
            }
            alerted = false;
        }

        protected virtual void BeginTeleport()
        {
            SteamVR_Fade.Start(Color.white, teleport_fade_in);
            Invoke("Teleport", teleport_delay);
        }

        private void Teleport()
        {
            var playerParent = item_to_teleport.transform.parent;
            AreaController areaController = transform.parent.GetComponentInParent<AreaController>() ?? transform.GetComponentInParent<AreaController>();

            if (playerParent == areaController.transform)
            { 
                // If the player is INSIDE the same area as the portal, move to destination area.
                item_to_teleport.transform.SetParent(destination.transform, false); 
            }
            else
            {
                // Otherwise, move into the area
                item_to_teleport.transform.SetParent(areaController.transform, false); 
            }
            SteamVR_Fade.Start(Color.clear, teleport_fade_out);
            item_to_teleport.GetComponent<PlayerCharacter>().currentlyTeleporting = false;
        }
    }
}
