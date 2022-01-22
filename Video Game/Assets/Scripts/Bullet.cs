using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")]
    public float reload;
    public float speed;
    public float damage;
    public bool explode;
    public float explosionDamage;
    public float explosionRadius;
    public float weight;
    public float pierce;

    [Header("Unity Setup")] 
    public bool player;
    public Rigidbody rb;
    public GameObject particles;
    public Transform playerPos;

    Quaternion savedRotation;

    public void Start() {
        Shoot(); 
        playerPos = player ? GameObject.FindGameObjectWithTag("Player").transform : GameObject.FindGameObjectWithTag("Enemy").transform;
        transform.LookAt(playerPos);
        transform.rotation *= Quaternion.Euler(0, -90, 0);
        Destroy(gameObject, 5f);
    }

    void Shoot() {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void Update() {
        if(!player) {
            // Debug.Log(pierce);
        }
        if(pierce <= 0) {
            Destroy(gameObject);
            Destroy((GameObject) Instantiate(particles, transform.position, transform.rotation), 3f);
        }
    }

    public void OnTriggerEnter(Collider collider) {

        GameObject hit = collider.gameObject;
        Bullet b = hit.GetComponent<Bullet>();

        if((hit.CompareTag("Player") && player) || (hit.CompareTag("Enemy") && !player) || (hit.CompareTag("Bullet") && b.player == player)) {
            // Debug.Log("Hit self");
            return;
        }
        if(hit.CompareTag("Bullet")) {
            pierce -= b.pierce;
            return;
        }
        if(hit.CompareTag("obstacle")) {
            Destroy((GameObject) Instantiate(particles, transform.position, transform.rotation), 3f);
            Destroy(gameObject);
            return;
        }
        ApplyDamage(hit, damage);

        if(explode && !hit.CompareTag("Bullet")) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider col in colliders)
            {
                ApplyDamage(col.gameObject, explosionDamage);
            }
        }
    }

    public void ApplyDamage(GameObject hit, float damage) {
        if(hit.CompareTag("Player") && !player || hit.CompareTag("Enemy") && player) {
            Health health = hit.GetComponent<Health>();
            health.ApplyDamage(damage);
            pierce = 0;
        } 
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
