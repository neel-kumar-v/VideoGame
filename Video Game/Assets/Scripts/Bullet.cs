using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {

    }

    void Shoot() {
        
    }

    public void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
