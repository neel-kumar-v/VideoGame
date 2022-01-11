using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    public float stop;
    public float retreat;
    public float reloadTime;
    public float range;
    public float offset;


    [Header("Unity Setup")]
    public GameObject bullet;
    public Transform firePoint;
    public Transform rangePoint;
    public LayerMask layerMask;

    private Transform player;

    Rigidbody rb;
    bool canShoot;

    public Vector3 posToStartRangeAt;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        canShoot = true;
    }

    void FixedUpdate() {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance > stop) {
            rb.velocity = player.position.normalized * speed; // Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        } else if(distance < stop && distance > retreat) {
            rb.velocity *= 0.9f;
        } else if(distance < retreat) {
            rb.velocity = player.position.normalized * -speed; // Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Update() {
        transform.LookAt(Vector3.Lerp(transform.position, player.position, Time.deltaTime));
        if(CheckAim() && canShoot) {
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    public bool CheckAim() {
        // RaycastHit hit;
        // if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 200f, layerMask)) {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //     return true;
        // } 
        // return false;
        posToStartRangeAt = rangePoint.position;
        Collider[] colliders = Physics.OverlapSphere(posToStartRangeAt, range);
        foreach(Collider collider in colliders) {
            GameObject possiblePlayer = collider.gameObject;
            if(possiblePlayer.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posToStartRangeAt, range);
    }
}