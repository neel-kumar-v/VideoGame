using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
    }

    public void Start() {
        Shoot(); 
    }

    void Shoot() {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
