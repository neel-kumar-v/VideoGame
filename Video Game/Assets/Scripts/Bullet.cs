using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public bool explode;
    public bool player;
    public Rigidbody rb;
    public GameObject particles;

    public void Start() {
        Shoot(); 
    }

    void Shoot() {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision) {

        ContactPoint point = collision.contacts[0];
        Vector3 calculateAngleOfEffect = Vector3.Reflect(rb.velocity, point.normal);
        Destroy((GameObject) Instantiate(particles, transform.position, Quaternion.Euler(calculateAngleOfEffect)), 3f);

        Destroy(gameObject);
        
        GameObject hit = collision.collider.gameObject;
        if(hit.CompareTag("Player") && !player || hit.CompareTag("Enemy") && player) {
            Health health = hit.GetComponent<Health>();
            health.ApplyDamage(damage);
            Debug.Log("Hit");
        } 
    }
}
