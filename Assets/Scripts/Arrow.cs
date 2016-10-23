using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    public float maxArrowLife = 6000f;

    [HideInInspector]
    public bool inFlight = false;

    private bool collided = false;
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
    }

    IEnumerator EmitSmoke ()
    {
        yield return new WaitForSeconds(0.05f); 
        gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void StickAfterCollision(Collision collision)
    {
        collided = true;
        inFlight = false;

        // child the arrow to the collided object
        transform.parent = collision.transform;
        //Destroy the arrow's rigidbody2D and collider2D
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<Collider>());
    }

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!collided)
        {
            transform.LookAt(transform.position + rigidBody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (inFlight)
        {
            Debug.Log("Collided with " + collision.gameObject.name);
            StickAfterCollision(collision);
        }
    }

    private void DestroyArrow(float time)
    {
        Destroy(arrowHolder, time);
        Destroy(this.gameObject, time);
    }
}