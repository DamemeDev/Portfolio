using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public GameObject bullet; //this is the reference to prefab we want to instantiate
    public Transform gunpos; // this is the position where we want to instantiate
    Rigidbody bulletrb;
    public float speed;
    public Transform bulletRotation;
    public float rotationChange;


    public float fireRate;
    public GameObject player;
    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //shooty bits
        Vector3 movement = new Vector3(0.0f, 0.0f, 1f);

        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject mybullet;
            mybullet = Instantiate(bullet, gunpos.position, bulletRotation.rotation) as GameObject;
            mybullet.transform.Rotate(Vector3.left, rotationChange);
            bulletrb = mybullet.GetComponent<Rigidbody>();

            bulletrb.AddForce(gunpos.forward * speed);



        }
    }
}
