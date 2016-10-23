using UnityEngine;
using VRTK;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnDelay = 1f;

    private float spawnDelayTimer = 0f;
    private BowAim bow;

    private void Start()
    {
        spawnDelayTimer = 0f;
    }

    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = collider.gameObject.GetComponentInParent<VRTK_InteractGrab>();
        if (!grabbingController)
        {
            return;
        }

        bool canGrab = CanGrab(grabbingController);
        bool noArrowNotched = NoArrowNotched(grabbingController.gameObject);
        bool deltaTime = Time.time >= spawnDelayTimer;

//        if (CanGrab(grabbingController) && NoArrowNotched(grabbingController.gameObject) && Time.time >= spawnDelayTimer)
        if (canGrab && noArrowNotched && deltaTime)
        {
            print("success");
            GameObject newArrow = Instantiate(arrowPrefab);
            newArrow.name = "ArrowClone";
            grabbingController.gameObject.GetComponent<VRTK_InteractTouch>().ForceTouch(newArrow);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
        }
    }

    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.gameObject.GetComponent<VRTK_ControllerEvents>().grabPressed);
    }

    private bool NoArrowNotched(GameObject controller)
    {
        if (controller == controllers.left)
        {
            bow = controllers.right.GetComponentInChildren<BowAim>();
        }
        else if (controller == controllers.right)
        {
            bow = controllers.left.GetComponentInChildren<BowAim>();
        }

        return (bow == null || !bow.HasArrow());
    }
}