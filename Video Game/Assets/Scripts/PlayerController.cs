using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Unity Setup")]
    public Rigidbody rb;
    public Camera cam;
    public GameObject bullet;
    public Transform firePoint;
    [Header("Parameters")]
    public float speed;
    [Tooltip("Possibly does not change the rotation speed")]
    public float rotationSpeed;
    public float reloadTime;

    Vector3 mousePosition;
    Vector3 movement;
    bool canShoot;

    public void Start() {
        canShoot = true;
    }

    void Update()
    {
        Rotate();
        if(Input.GetButtonDown("Fire1") && canShoot) {
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
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
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, Quaternion.identity);
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
