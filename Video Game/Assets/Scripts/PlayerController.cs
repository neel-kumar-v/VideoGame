using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Unity Setup")]
    public Rigidbody rb;
    public Camera cam;
    [Space(15)]
    public GameObject bullet;
    public Transform firePoint;
    [Space(15)]
    public GameObject cylinder;
    public Vector3 cylinderPos;
    public Vector3 cylinderRot;
    [Space(10)]
    public Vector3 startPos;

    [Header("Parameters")]
    public float speed;
    float reloadTime;
    public float recoil;
    public float recoilTime;
    public float countdown;

    Vector3 mousePosition;
    Vector3 movement;

    bool canShoot; 
    bool canMove;

    public Bullet b;


    public void Awake() {
        GameObject cylinderGO = (GameObject) Instantiate(cylinder, transform, false);
    }

    public void Start() {
        canShoot = true;
        
        Reset();
    }

    public void Reset() {
        transform.position = new Vector3(15f * (float) (Random.Range(0, 2) * 2 - 1), 2f, 15f);
        StartCoroutine(Countdown(countdown));
        rb.velocity = Vector3.zero;
        Vector3 lookDirection = Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), Time.deltaTime);
        transform.LookAt(lookDirection); 
        
        b = bullet.GetComponent<Bullet>();
        speed /= b.weight;
        reloadTime = b.reload;  
    }

    public IEnumerator Countdown(float time) {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void Update()
    {
        if(!canMove) return;
        Rotate();
        if(Input.GetButton("Fire1") && canShoot) {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!canMove) return;
        Move();
    }

    public void Move() {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        rb.velocity = movement * speed;
    }

    public void Rotate() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            mousePosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            // Debug.DrawLine(ray.origin, mousePosition, Color.blue);
        }
        Vector3 lookDirection = Vector3.Lerp(transform.position, mousePosition, Time.deltaTime);
        transform.LookAt(lookDirection);    
    }
    // TODO: Animate the cylinder to go back when shooting
    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        newBullet.GetComponent<Bullet>().player = true;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

}
