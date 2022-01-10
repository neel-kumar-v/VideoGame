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

    [Tooltip("Possibly does not change the rotation speed")]
    public float rotationSpeed;
    public float reloadTime;

    Vector3 mousePosition;
    Vector3 movement;
    bool canShoot;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckForPlayer()) {
            Rotate(player.transform.position);
            Move(player.transform.position);
        }
    }

    public void Rotate(Vector3 pos) {
        Vector3 lookDirection = Vector3.Lerp(transform.position, pos, Time.deltaTime);
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

    public void Move(Vector3 pos) {

    }


}
