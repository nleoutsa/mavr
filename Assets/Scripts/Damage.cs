using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
    public float damage_min = 30f;
    public float damage_max = 35f;

    public float DetermineDamage ()
    {
        var speed = GetComponent<Rigidbody>().velocity.magnitude;
        var damage_amount = Random.Range(damage_min, damage_max);

        Debug.Log(speed + " x " + damage_amount + " = " + speed * damage_amount);
        return speed * damage_amount;
    }
}
