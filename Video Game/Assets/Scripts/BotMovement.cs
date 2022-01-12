using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [Header("Unity Setup")]
    public Rigidbody rb;
    public Camera cam;
    public GameObject bullet;
    public Transform firePoint;
    public LayerMask layerMask;

    [Header("Parameters")]
    public float speed;
    public float range;
    public float spontaneity;
    public float reloadTime;

    Vector3 movement;
    bool canShoot;
    bool canGenerateNewPosition;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        canGenerateNewPosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckForPlayer()) {
            Rotate(new Vector3(player.transform.position.x, 2f, player.transform.position.z));
            Move(new Vector3(player.transform.position.x, 2f, player.transform.position.z), true);
        } else {
            if(canGenerateNewPosition) {
                StartCoroutine(GenerateRandomPos(spontaneity));
            }
            Rotate(movement);
            Move(movement, false);
        }
        if(CheckAim() && canShoot) {
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    public void Rotate(Vector3 pos) {
        Vector3 lookDirection = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime);
        transform.LookAt(lookDirection);
    }

    public bool CheckForPlayer() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider collider in colliders) {
            GameObject possiblePlayer = collider.gameObject;
            if(possiblePlayer.CompareTag("Player")) {
                player = collider.gameObject;
                return true;
            }
        }
        return false;
    }

    public bool CheckAim() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 200f, layerMask)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return true;
        } 
        return false;
    }

    public void Move(Vector3 pos, bool who) {
        Vector3 moveDirection = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        
        rb.velocity = -moveDirection;
    }

    public IEnumerator GenerateRandomPos(float time) {
        canGenerateNewPosition = false;
        movement = new Vector3(Mathf.Clamp(transform.position.x + Random.Range(-20f, 20f), -50f, 50f), transform.position.y, Mathf.Clamp(transform.position.z + Random.Range(-20f, 20f), -30f, 30f));

        yield return new WaitForSeconds(time);
        canGenerateNewPosition = true;
    }

    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    

}
