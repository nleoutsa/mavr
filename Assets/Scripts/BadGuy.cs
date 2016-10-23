using UnityEngine;
using System.Collections;

public class BadGuy : MonoBehaviour {
    public float starting_life = 1000f;

    private float remaining_life;

	// Use this for initialization
	void Start () {
        remaining_life = starting_life;
	}

    void OnCollisionEnter (Collision collision)
    {
        var damage = collision.gameObject.GetComponent<Damage>(); 

        if (damage)
        {
            var damageTaken = damage.DetermineDamage();
            remaining_life = remaining_life - damageTaken;
            Debug.Log("BadGuy took " + damageTaken + " damage. remaining_life = " + remaining_life);

            if (remaining_life <= 0f) {
                Debug.Log("BadGuy died.");
                Die();
            }
        }
    }

    private void Die ()
    {
        Destroy(gameObject, 0);
    }
}
