using UnityEngine;
using System.Collections;

public class AreaController : MonoBehaviour {
    [HideInInspector]
    public bool locked = true;

    private Component[] occupiers;

	void FixedUpdate () {
        occupiers = GetComponentsInChildren<Occupier>();

        if (occupiers.Length == 0)
        {
            locked = false;
        }
	}
}
