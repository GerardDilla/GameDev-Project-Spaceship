using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{

    [Header("Ship Attributes")]
    public float verticalPush;
    public float horizontalPush;
    public float health = 100;
    public float currentHealth;
    public Vector3 direction;

    [Header("Object References")]
    public Rigidbody2D spaceShip;
    public Rigidbody2D backThruster;
    public Rigidbody2D leftThruster;
    public Rigidbody2D rightThruster;
    public Transform spaceShipTransform;
    public ParticleSystem shipWeapon;
    // Start is called before the first frame update
    void Start()
    {
        spaceShip = GetComponent<Rigidbody2D>();
        spaceShipTransform = GetComponent<Transform>();
        backThruster = GameObject.Find("BackThruster").GetComponent<Rigidbody2D>();
        leftThruster = GameObject.Find("LeftThruster").GetComponent<Rigidbody2D>();
        rightThruster = GameObject.Find("RightThruster").GetComponent<Rigidbody2D>();
        currentHealth = health;
        shipWeapon.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spaceShip.AddForce(transform.up * horizontalPush * 10);
        if (Input.GetKey(KeyCode.W))
        {
            // spaceShip.AddForce(transform.up * horizontalPush * 10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * -verticalPush, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * verticalPush, Space.World);
        }

    }
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            shipWeapon.Play();
            // spaceShip.AddForce(transform.up * horizontalPush * 10);
        }
        // if (Input.GetMouseButtonUp(0))
        // {
        //     shipWeapon.Stop();
        //     // spaceShip.AddForce(transform.up * horizontalPush * 10);
        // }
    }


}
