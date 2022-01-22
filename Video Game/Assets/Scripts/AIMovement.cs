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
    public float dodgeRange;
    [Range(0f, 1f)] public float dodgeReaction;


    [Header("Unity Setup")]
    public GameObject bullet;
    public Transform firePoint;
    public Transform rangePoint;
    public LayerMask layerMask;
    public float countdown;
    public Transform player;
    public Vector3 startPos;

    Rigidbody rb;
    bool canShoot;
    bool canDodge;
    Bullet b;

    Vector3 posToStartRangeAt;
    float distance;
    private Vector3 vel;

    Renderer pRend;

    public void Reset() {
        b = bullet.GetComponent<Bullet>();
        transform.position = startPos;
        transform.LookAt(Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), Time.deltaTime));
        speed /= b.weight;
        reloadTime = b.reload;
        if(rb != null) {     
            rb.velocity = Vector3.zero;
        }
        player = null;
        rb = null;
        StartCoroutine(Countdown(countdown));
    }

    public IEnumerator Countdown(float time) {
        canShoot = false;
        transform.position = new Vector3(15f * (float) (Random.Range(0, 2) * 2 - 1), 2f, -15f);
        yield return new WaitForSeconds(time);
        canShoot = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        pRend = player.gameObject.GetComponent<Renderer>();
    }

    void FixedUpdate() {
        if(player == null || rb == null || !pRend.enabled) return;

        // if(AvoidBullets()) return;

        distance = Vector3.Distance(transform.position, player.position);
        vel = (transform.position - player.position).normalized; 
        if(distance > stop) {
            rb.velocity = vel * -speed;
            vel = vel * speed;
            
        } else if(distance < stop && distance > retreat) {
            rb.velocity *= 0.95f;   
            vel *= 0.95f;
            
        } else if(distance < retreat) {
            rb.velocity = vel * speed;
            vel = vel * -speed;
            
        } else {
            rb.velocity = vel * -speed;
            vel = vel * speed;
            
        }
    }

    public bool AvoidBullets() {
        Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();

        Bullet correctBullet = null;
        distance = Mathf.Infinity;

        foreach (Bullet bul in bullets)
        {
            if(bul.player) {
                float newDistance = Vector3.Distance(transform.position, bul.transform.position);
                if(distance > newDistance) {
                    distance = newDistance;
                    correctBullet = bul;
                }
            }
        }
        Debug.Log(correctBullet != null);
        if(correctBullet != null) {
            vel = (transform.position - correctBullet.transform.position).normalized;

            if(distance < dodgeRange) {
                Debug.Log("$2");
                rb.AddForce(vel, ForceMode.Impulse);
                vel = vel * speed;
            }
        }
        Debug.Log("$3");
        return false;
    }




    void Update() {
        if(player == null) return;
        
        transform.LookAt(Vector3.Lerp(transform.position, player.position, Time.deltaTime));
        if(CheckAim() && canShoot) {
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        newBullet.GetComponent<Bullet>().player = false;
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
