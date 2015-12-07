using UnityEngine;
using System.Collections;

public class ExplodeyMan : MonoBehaviour {

    Rigidbody rb;
    public GameObject ExplosionMan;

    void Start() {
	    rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 1)
            Debug.Log("Velocity: " + rb.velocity.magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1 && collision.gameObject.tag == "Stone")
        {
            Debug.Log("DESTRO");
            GameObject tempExplosion = (GameObject)Instantiate(ExplosionMan, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            Destroy(tempExplosion, 2f);
        }
    }
}
